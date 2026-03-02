<template>
  <div class="mapa-zonas-container">
    <div class="mapa-header">
      <h4 style="margin: 0 0 8px 0; color: var(--text);">
        Zonas de Cobertura
      </h4>
      <p style="font-size: 0.9rem; color: #64748b; margin: 0 0 16px 0;">
        Seleccioná las localidades donde prestás servicios (aplica tanto para oficiantes como profesionales). Podés buscar y agregar múltiples localidades.
      </p>
    </div>

    <!-- Buscador de localidades -->
    <div class="buscador-localidades">
      <div class="input-wrapper">
        <input
          v-model="busquedaLocalidad"
          type="text"
          placeholder="Buscar localidad (ej: San Miguel de Tucumán)"
          class="input-busqueda"
          @input="buscarLocalidades"
          @focus="mostrarSugerencias = true"
        />
        <span v-if="buscando" class="loading-spinner">⏳</span>
      </div>

      <!-- Lista de sugerencias -->
      <div v-if="mostrarSugerencias && sugerencias.length > 0" class="sugerencias-list">
        <div
          v-for="(sugerencia, index) in sugerencias"
          :key="index"
          class="sugerencia-item"
          @click="agregarLocalidad(sugerencia)"
        >
          <div class="sugerencia-info">
            <strong>{{ sugerencia.nombre }}</strong>
            <span class="sugerencia-ubicacion">{{ sugerencia.nombreCompleto }}</span>
          </div>
          <span class="agregar-icon">+</span>
        </div>
      </div>
    </div>

    <!-- Lista de localidades seleccionadas -->
    <div v-if="localidadesSeleccionadas.length > 0" class="localidades-seleccionadas">
      <div
        v-for="(localidad, index) in localidadesSeleccionadas"
        :key="index"
        class="localidad-item"
      >
        <div class="localidad-info">
          <span class="localidad-nombre">{{ localidad.localidad }}</span>
          <span class="localidad-provincia">{{ localidad.provincia }}</span>
        </div>
        <button
          type="button"
          class="btn-eliminar-localidad"
          @click="eliminarLocalidad(index)"
          title="Eliminar localidad"
        >
          ✕
        </button>
      </div>
    </div>

    <!-- Mapa (solo si showMap es true) -->
    <div v-if="showMap" ref="mapContainer" class="mapa-leaflet"></div>

    <p v-if="showMap && localidadesSeleccionadas.length === 0" class="mensaje-vacio">
      No hay localidades seleccionadas. Buscá y agregá localidades para verlas en el mapa.
    </p>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch, nextTick } from 'vue'
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'
import { geocodingService, type CiudadSugerida } from '../services/geocodingService'

interface ZonaCobertura {
  localidad: string
  provincia: string
  departamento?: string
  latitud: number
  longitud: number
  codigoPostal?: string
}

interface Props {
  modelValue?: ZonaCobertura[]
  provincia?: string
  showMap?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: () => [],
  provincia: undefined,
  showMap: true
})

const emit = defineEmits<{
  'update:modelValue': [value: ZonaCobertura[]]
}>()

const mapContainer = ref<HTMLElement | null>(null)
const busquedaLocalidad = ref('')
const sugerencias = ref<CiudadSugerida[]>([])
const mostrarSugerencias = ref(false)
const buscando = ref(false)
const localidadesSeleccionadas = ref<ZonaCobertura[]>([])
let map: L.Map | null = null
let markers: L.Marker[] = []
let timeoutId: number | null = null

// Actualizar marcadores en el mapa (definido antes del watch para evitar "before initialization")
function actualizarMapa () {
  if (!map) return

  markers.forEach(marker => map?.removeLayer(marker))
  markers = []

  if (localidadesSeleccionadas.value.length === 0) {
    map.setView([-34.6037, -58.3816], 5)
    return
  }

  const bounds: L.LatLngBounds = L.latLngBounds([])
  localidadesSeleccionadas.value.forEach((localidad) => {
    if (localidad.latitud && localidad.longitud) {
      const marker = L.marker([localidad.latitud, localidad.longitud])
        .addTo(map!)
        .bindPopup(`<strong>${localidad.localidad}</strong><br>${localidad.provincia}`)
      markers.push(marker)
      bounds.extend([localidad.latitud, localidad.longitud])
    }
  })
  if (bounds.isValid()) {
    map.fitBounds(bounds, { padding: [50, 50] })
  }
}

// Inicializar mapa
function inicializarMapa () {
  if (!mapContainer.value) return

  delete (L.Icon.Default.prototype as any)._getIconUrl
  L.Icon.Default.mergeOptions({
    iconRetinaUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-icon-2x.png',
    iconUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-icon.png',
    shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-shadow.png',
  })

  map = L.map(mapContainer.value).setView([-34.6037, -58.3816], 5)
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenStreetMap contributors',
    maxZoom: 19
  }).addTo(map)

  if (localidadesSeleccionadas.value.length > 0) {
    actualizarMapa()
  }
}

// Inicializar con valores del modelo
watch(() => props.modelValue, (newValue) => {
  if (newValue && newValue.length > 0) {
    localidadesSeleccionadas.value = [...newValue]
    actualizarMapa()
  }
}, { immediate: true })

// Buscar localidades con debounce
const buscarLocalidades = () => {
  if (timeoutId) {
    clearTimeout(timeoutId)
  }

  if (busquedaLocalidad.value.length < 2) {
    sugerencias.value = []
    mostrarSugerencias.value = false
    return
  }

  buscando.value = true
  timeoutId = window.setTimeout(async () => {
    try {
      const resultados = await geocodingService.buscarCiudades(
        busquedaLocalidad.value,
        8,
        props.provincia
      )
      sugerencias.value = resultados
      mostrarSugerencias.value = true
    } catch (error) {
      sugerencias.value = []
    } finally {
      buscando.value = false
    }
  }, 300)
}

// Agregar localidad seleccionada
const agregarLocalidad = (ciudad: CiudadSugerida) => {
  // Verificar si ya está agregada
  const yaExiste = localidadesSeleccionadas.value.some(
    l => l.localidad === ciudad.nombre && l.provincia === ciudad.provincia
  )

  if (yaExiste) {
    alert('Esta localidad ya está agregada')
    return
  }

  const nuevaZona: ZonaCobertura = {
    localidad: ciudad.nombre,
    provincia: ciudad.provincia,
    departamento: ciudad.departamento,
    latitud: ciudad.latitud,
    longitud: ciudad.longitud
  }

  localidadesSeleccionadas.value.push(nuevaZona)
  busquedaLocalidad.value = ''
  sugerencias.value = []
  mostrarSugerencias.value = false

  // Emitir cambio
  emit('update:modelValue', localidadesSeleccionadas.value)
  
  // Actualizar mapa solo si está habilitado
  if (props.showMap) {
    nextTick(() => {
      actualizarMapa()
    })
  }
}

// Eliminar localidad
const eliminarLocalidad = (index: number) => {
  localidadesSeleccionadas.value.splice(index, 1)
  emit('update:modelValue', localidadesSeleccionadas.value)
  if (props.showMap) {
    actualizarMapa()
  }
}

// Cerrar sugerencias al hacer clic fuera
const cerrarSugerencias = (event: MouseEvent) => {
  const target = event.target as HTMLElement
  if (!target.closest('.buscador-localidades')) {
    mostrarSugerencias.value = false
  }
}

onMounted(() => {
  nextTick(() => {
    if (props.showMap) {
      inicializarMapa()
    }
  })
  document.addEventListener('click', cerrarSugerencias)
})

onUnmounted(() => {
  if (map) {
    map.remove()
  }
  if (timeoutId) {
    clearTimeout(timeoutId)
  }
  document.removeEventListener('click', cerrarSugerencias)
})
</script>

<style scoped>
.mapa-zonas-container {
  width: 100%;
  margin-bottom: 24px;
}

.mapa-header {
  margin-bottom: 16px;
}

.buscador-localidades {
  position: relative;
  margin-bottom: 16px;
}

.input-wrapper {
  position: relative;
}

.input-busqueda {
  width: 100%;
  padding: 12px 40px 12px 12px;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  font-size: 14px;
  transition: border-color 0.2s;
}

.input-busqueda:focus {
  outline: none;
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.loading-spinner {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  font-size: 16px;
}

.sugerencias-list {
  position: absolute;
  top: 100%;
  left: 0;
  right: 0;
  background: white;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  margin-top: 4px;
  max-height: 300px;
  overflow-y: auto;
  z-index: 1000;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.sugerencia-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px;
  cursor: pointer;
  transition: background-color 0.2s;
  border-bottom: 1px solid #f1f5f9;
}

.sugerencia-item:last-child {
  border-bottom: none;
}

.sugerencia-item:hover {
  background-color: #f8fafc;
}

.sugerencia-info {
  display: flex;
  flex-direction: column;
  gap: 4px;
  flex: 1;
}

.sugerencia-info strong {
  color: var(--text);
  font-size: 14px;
}

.sugerencia-ubicacion {
  color: #64748b;
  font-size: 12px;
}

.agregar-icon {
  color: #3b82f6;
  font-size: 20px;
  font-weight: bold;
  padding: 4px 8px;
}

.localidades-seleccionadas {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-bottom: 16px;
}

.localidad-item {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 12px;
  background-color: #eff6ff;
  border: 1px solid #bfdbfe;
  border-radius: 6px;
  font-size: 13px;
}

.localidad-info {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.localidad-nombre {
  font-weight: 500;
  color: var(--text);
}

.localidad-provincia {
  color: #64748b;
  font-size: 11px;
}

.btn-eliminar-localidad {
  background: none;
  border: none;
  color: #ef4444;
  cursor: pointer;
  font-size: 16px;
  padding: 0;
  width: 20px;
  height: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: color 0.2s;
}

.btn-eliminar-localidad:hover {
  color: #dc2626;
}

.mapa-leaflet {
  width: 100%;
  height: 400px;
  border-radius: 8px;
  border: 1px solid #e2e8f0;
  overflow: hidden;
}

/* Media queries para móviles */
@media (max-width: 768px) {
  .mapa-leaflet {
    height: 300px; /* Menor altura en móviles */
  }
  
  .mapa-zonas-container {
    margin-bottom: 16px;
  }
  
  .buscador-localidades {
    margin-bottom: 12px;
  }
  
  .input-busqueda {
    font-size: 16px; /* Evita zoom en iOS */
    padding: 10px;
  }
  
  .localidades-seleccionadas {
    gap: 6px;
  }
  
  .localidad-item {
    padding: 6px 10px;
    font-size: 12px;
  }
  
  .sugerencias-list {
    max-height: 200px;
  }
}

.mensaje-vacio {
  text-align: center;
  color: #64748b;
  font-size: 14px;
  padding: 20px;
  background-color: #f8fafc;
  border-radius: 6px;
  margin-top: 16px;
}

/* Estilos para Leaflet */
:deep(.leaflet-popup-content-wrapper) {
  border-radius: 6px;
}

:deep(.leaflet-popup-content) {
  margin: 12px;
  font-size: 14px;
}
</style>

