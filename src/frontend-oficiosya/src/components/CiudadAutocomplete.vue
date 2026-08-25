<template>
  <div class="ciudad-autocomplete" ref="containerRef">
    <div class="search-field" :class="{ 'disabled': disabled }">
      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
        <path d="M12 21s8-6.1 8-12a8 8 0 1 0-16 0c0 5.9 8 12 8 12Z" stroke="currentColor" stroke-width="2"/>
        <circle cx="12" cy="9" r="3" stroke="currentColor" stroke-width="2"/>
      </svg>
      <input
        ref="inputRef"
        v-model="inputValue"
        type="text"
        :placeholder="placeholder"
        class="search-input"
        :disabled="disabled"
        @input="onInput"
        @focus="onFocus"
        @blur="onBlur"
        @keydown.arrow-down="navigateDown"
        @keydown.arrow-up="navigateUp"
        @keydown.enter="selectCurrent"
        @keydown.escape="closeSuggestions"
      />
      <div v-if="buscando" class="loading-spinner">
        <span class="spinner"></span>
      </div>
    </div>
    
    <!-- Lista de sugerencias -->
    <div 
      v-if="mostrarSugerencias && sugerencias.length > 0" 
      class="sugerencias-dropdown"
      ref="sugerenciasRef"
    >
      <div
        v-for="(ciudad, index) in sugerencias"
        :key="`${ciudad.nombre}-${ciudad.provincia || ''}-${index}`"
        class="sugerencia-item"
        :class="{ 'active': index === indiceSeleccionado }"
        @click="seleccionarCiudad(ciudad)"
        @mouseenter="indiceSeleccionado = index"
      >
        <div class="sugerencia-nombre">{{ ciudad.nombre }}</div>
        <div v-if="ciudad.provincia" class="sugerencia-provincia">
          {{ ciudad.departamento ? `${ciudad.departamento}, ` : '' }}{{ ciudad.provincia }}
        </div>
      </div>
    </div>
    
    <div 
      v-if="mostrarSugerencias && sugerencias.length === 0 && inputValue.length >= 2 && !buscando" 
      class="sugerencias-dropdown sin-resultados"
    >
      <div class="sugerencia-item">No se encontraron ciudades</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch, onMounted, onUnmounted } from 'vue'
import { geocodingService, type CiudadSugerida } from '../services/geocodingService'

interface Props {
  modelValue: string
  placeholder?: string
  provincia?: string
  disabled?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  placeholder: 'Ej: San Miguel de Tucumán',
  provincia: undefined,
  disabled: false
})

const emit = defineEmits<{
  'update:modelValue': [value: string]
  'ciudadSeleccionada': [ciudad: CiudadSugerida]
}>()

const inputRef = ref<HTMLInputElement | null>(null)
const containerRef = ref<HTMLElement | null>(null)
const sugerenciasRef = ref<HTMLElement | null>(null)
const inputValue = ref(props.modelValue)
const sugerencias = ref<CiudadSugerida[]>([])
const mostrarSugerencias = ref(false)
const buscando = ref(false)
const indiceSeleccionado = ref(-1)
let timeoutId: number | null = null

// Debounce para evitar demasiadas llamadas a la API
const onInput = () => {
  if (props.disabled) return
  
  emit('update:modelValue', inputValue.value)
  
  if (timeoutId) {
    clearTimeout(timeoutId)
  }

  if (inputValue.value.length < 2) {
    sugerencias.value = []
    mostrarSugerencias.value = false
    return
  }

  buscando.value = true
  timeoutId = window.setTimeout(async () => {
    try {
      const resultados = await geocodingService.buscarCiudades(
        inputValue.value,
        8,
        props.provincia
      )
      sugerencias.value = resultados
      mostrarSugerencias.value = true
      indiceSeleccionado.value = -1
    } catch (error) {
      sugerencias.value = []
    } finally {
      buscando.value = false
    }
  }, 300) // Esperar 300ms después de que el usuario deje de escribir
}

const onFocus = () => {
  if (props.disabled) return
  if (sugerencias.value.length > 0) {
    mostrarSugerencias.value = true
  }
}

const onBlur = (event: FocusEvent) => {
  // Esperar un poco para permitir que el click en una sugerencia se procese
  setTimeout(() => {
    const target = event.relatedTarget as HTMLElement
    if (!containerRef.value?.contains(target)) {
      mostrarSugerencias.value = false
    }
  }, 200)
}

const seleccionarCiudad = (ciudad: CiudadSugerida) => {
  inputValue.value = ciudad.nombreCompleto
  emit('update:modelValue', ciudad.nombreCompleto)
  emit('ciudadSeleccionada', ciudad)
  mostrarSugerencias.value = false
  sugerencias.value = []
}

const navigateDown = (event: KeyboardEvent) => {
  event.preventDefault()
  if (sugerencias.value.length === 0) return
  
  indiceSeleccionado.value = Math.min(
    indiceSeleccionado.value + 1,
    sugerencias.value.length - 1
  )
  
  // Scroll a la sugerencia seleccionada
  scrollToSelected()
}

const navigateUp = (event: KeyboardEvent) => {
  event.preventDefault()
  if (sugerencias.value.length === 0) return
  
  indiceSeleccionado.value = Math.max(indiceSeleccionado.value - 1, -1)
  scrollToSelected()
}

const selectCurrent = (event: KeyboardEvent) => {
  event.preventDefault()
  if (indiceSeleccionado.value >= 0 && indiceSeleccionado.value < sugerencias.value.length) {
    seleccionarCiudad(sugerencias.value[indiceSeleccionado.value])
  }
}

const closeSuggestions = () => {
  mostrarSugerencias.value = false
}

const scrollToSelected = () => {
  if (sugerenciasRef.value && indiceSeleccionado.value >= 0) {
    const items = sugerenciasRef.value.querySelectorAll('.sugerencia-item')
    if (items[indiceSeleccionado.value]) {
      items[indiceSeleccionado.value].scrollIntoView({
        block: 'nearest',
        behavior: 'smooth'
      })
    }
  }
}

// Sincronizar con el valor del modelo
watch(() => props.modelValue, (newValue) => {
  if (newValue !== inputValue.value) {
    inputValue.value = newValue
  }
})

// Limpiar timeout al desmontar
onUnmounted(() => {
  if (timeoutId) {
    clearTimeout(timeoutId)
  }
})
</script>

<style scoped>
.ciudad-autocomplete {
  position: relative;
  width: 100%;
  flex: 1;
}

.ciudad-autocomplete .search-field {
  position: relative;
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 12px 16px;
  color: #64748b;
  width: 100%;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  background: white;
  transition: border-color 0.2s, box-shadow 0.2s;
  min-height: 48px;
  box-sizing: border-box;
}

.ciudad-autocomplete .search-field:focus-within {
  border-color: #3b82f6;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.ciudad-autocomplete .search-field > svg:first-child {
  color: #64748b;
  flex-shrink: 0;
  width: 18px;
  height: 18px;
  display: block;
}

.ciudad-autocomplete .search-input {
  border: none;
  outline: none;
  background: transparent;
  color: #0f172a;
  font-size: 14px;
  width: 100%;
  flex: 1;
  min-width: 0;
  padding: 0;
  margin: 0;
  line-height: 1.25;
}

.ciudad-autocomplete .search-input:disabled {
  cursor: not-allowed;
  opacity: 0.6;
  color: #94a3b8;
}

.ciudad-autocomplete .search-field.disabled {
  background-color: #f1f5f9;
  cursor: not-allowed;
  border-color: #e2e8f0;
}

.loading-spinner {
  position: absolute;
  right: 12px;
  display: flex;
  align-items: center;
}

.spinner {
  width: 16px;
  height: 16px;
  border: 2px solid #e2e8f0;
  border-top-color: #3b82f6;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.sugerencias-dropdown {
  position: absolute;
  top: calc(100% + 4px);
  left: 0;
  right: 0;
  background: white;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
  max-height: 300px;
  overflow-y: auto;
  z-index: 9999;
  margin-top: 0;
}

.sugerencia-item {
  padding: 12px 16px;
  cursor: pointer;
  border-bottom: 1px solid #f1f5f9;
  transition: background-color 0.15s;
}

.sugerencia-item:last-child {
  border-bottom: none;
}

.sugerencia-item:hover,
.sugerencia-item.active {
  background-color: #f8fafc;
}

.sugerencia-nombre {
  font-weight: 500;
  color: #1e293b;
  margin-bottom: 2px;
}

.sugerencia-provincia {
  font-size: 0.875rem;
  color: #64748b;
}

.sin-resultados .sugerencia-item {
  color: #94a3b8;
  cursor: default;
}

.sin-resultados .sugerencia-item:hover {
  background-color: transparent;
}
</style>

