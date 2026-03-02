<template>
  <div class="home">
    <!-- Hero Section -->
    <section class="hero bg-primary text-white py-5" id="inicio">
      <div class="container">
        <div class="row align-items-center min-vh-50">
          <div class="col-lg-6">
            <h1 class="display-4 fw-bold mb-4">
              Encontrá al {{ tipoBusqueda === 'oficiante' ? 'oficiante' : 'profesional' }} que necesitás, <br> en tu zona
            </h1>
            <p class="lead mb-4" v-if="tipoBusqueda === 'oficiante'">
              Electricistas, plomeros, albañiles, gasistas, jardineros y mucho más. Compará perfiles, reseñas y contactá directo.
            </p>
            <p class="lead mb-4" v-else>
              Abogados, contadores, médicos, psicólogos y otros profesionales especializados. Compará perfiles, reseñas y contactá directo.
            </p>
            
            <form @submit.prevent="buscarProfesionales" class="search-form">
              <div class="search-container">
                <SearchableDropdown
                  v-model="busqueda.oficio"
                  :options="opcionesDropdown"
                  :placeholder="cargandoCategorias ? 'Cargando...' : 'Buscar oficio o categoría'"
                  :disabled="cargandoCategorias"
                />
                <CiudadAutocomplete
                  v-model="busqueda.zona"
                  placeholder="Ej: San Miguel de Tucumán"
                  @ciudad-seleccionada="onCiudadSeleccionada"
                />
                <button type="submit" class="search-button">
                  Buscar
                </button>
              </div>
            </form>
            
            <div class="search-tip">
              <strong>Tip:</strong> pronto podrás <b>dibujar tu zona</b> en el mapa para resultados más precisos.
            </div>
          </div>
          <div class="col-lg-6">
            <div class="hero-card">
              <span class="verified-badge">Nuevos perfiles verificados</span>
              <h3 class="professionals-title">Profesionales cerca tuyo</h3>
              <div class="professionals-grid">
                <div v-for="profesional in profesionalesDestacados" :key="profesional.id" class="professional-card" @click="verPerfil(profesional.id)">
                  <div class="professional-avatar"></div>
                  <div class="professional-info">
                    <div class="professional-name">{{ profesional.usuario?.nombre }}</div>
                    <div class="professional-details">{{ profesional.oficios?.[0]?.nombre || 'Electricista' }}</div>
                    <div class="professional-rating">
                      <span class="stars">★★★★★</span>
                      <span class="rating-text">({{ profesional.resenas?.length || 32 }} reseñas)</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Categorías -->
    <section class="section py-5 bg-light" id="categorias">
      <div class="container">
        <h2 class="text-center mb-3">Explorá categorías populares</h2>
        <p class="text-center text-muted mb-5">Elegí una categoría para ver profesionales cerca de tu ubicación.</p>
        <div class="row g-4">
          <div class="col-lg-3 col-md-4 col-sm-6" v-for="categoria in categorias" :key="categoria.nombre">
            <div class="card cat h-100 text-center">
              <div class="card-body d-flex flex-column align-items-center">
                <div class="cat-icon mb-3">{{ categoria.icono }}</div>
                <h5 class="card-title">{{ categoria.nombre }}</h5>
                <p class="card-text text-muted">{{ categoria.descripcion }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Cómo funciona -->
    <section class="section py-5" id="como-funciona" style="background:#f8fafc">
      <div class="container">
        <h2 class="text-center mb-5">¿Cómo funciona?</h2>
        <div class="row g-4">
          <div class="col-lg-4 col-md-6" v-for="paso in pasos" :key="paso.numero">
            <div class="step text-center">
              <div class="step-badge" :style="paso.badgeStyle">{{ paso.numero }}</div>
              <h4 class="mb-3">{{ paso.titulo }}</h4>
              <p class="text-muted">{{ paso.descripcion }}</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Profesionales destacados -->
    <section class="section py-5">
      <div class="container">
        <h2 class="text-center mb-3">Profesionales destacados</h2>
        <p class="text-center text-muted mb-5">Perfiles completos, con certificaciones y excelentes reseñas.</p>
        <div class="row g-4">
          <div class="col-lg-4 col-md-6" v-for="profesional in profesionalesDestacados" :key="profesional.id" @click="verPerfil(profesional.id)" style="cursor: pointer;">
            <div class="pro card h-100">
              <div class="card-body">
                <div class="pro-head d-flex align-items-center mb-3">
                  <div class="avatar me-3"></div>
                  <div>
                    <h6 class="card-title mb-1">{{ profesional.usuario.nombre }}</h6>
                    <small class="text-muted">San Miguel de Tucumán · Tucumán</small>
                  </div>
                </div>
                <div class="mb-2">
                  <span class="stars">★★★★★</span> 
                  <span class="ms-1">{{ profesional.calificacionPromedio.toFixed(1) }} ({{ profesional.resenas.length }})</span>
                </div>
                <div class="small text-muted">
                  Certificaciones: {{ profesional.certificados.length }}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Testimonios -->
    <section class="section py-5 bg-light">
      <div class="container">
        <h2 class="text-center mb-5">Lo que dicen nuestros usuarios</h2>
        <div class="row g-4">
          <div class="col-lg-4 col-md-6" v-for="testimonio in testimonios" :key="testimonio.id">
            <div class="testi card h-100">
              <div class="card-body">
                <blockquote class="blockquote mb-0">
                  <p class="mb-3">"{{ testimonio.comentario }}"</p>
                  <footer class="blockquote-footer">
                    <cite title="Source Title">{{ testimonio.autor }}</cite>
                  </footer>
                </blockquote>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA profesional -->
    <section class="section py-5 cta-section">
      <div class="container">
        <div class="cta-block">
          <div>
            <h2 style="margin:0 0 6px">¿Sos profesional? Empezá a recibir trabajos</h2>
            <div class="small" style="color: white;">Registrate gratis, subí tus certificaciones y elegí tu zona de trabajo.</div>
          </div>
          <button class="btn btn-light btn-lg" @click="irARegistro">
            Crear cuenta de profesional
          </button>
        </div>
      </div>
    </section>

    <!-- Footer -->
    <footer>
      <div class="container footer">
        <div>
          <div class="logo" style="color:white; cursor: pointer;" @click="irAInicio">
            <div class="logo-badge">🛠️</div>
            <span>OficiosYa</span>
          </div>
          <p class="small" style="margin-top:8px">Conectamos personas con profesionales de confianza en su zona.</p>
        </div>
        <div>
          <strong>Explorar</strong>
          <div class="small" style="margin-top:8px"><a href="#" @click="irAOficiantes" style="color:inherit">Oficiantes</a></div>
          <div class="small"><a href="#" style="color:inherit">Profesionales</a></div>
        </div>
        <div>
          <strong>Ayuda</strong>
          <div class="small" style="margin-top:8px"><a href="#" style="color:inherit">Centro de ayuda</a></div>
          <div class="small"><a href="#" style="color:inherit">Contacto</a></div>
        </div>
        <div>
          <strong>Legal</strong>
          <div class="small" style="margin-top:8px"><RouterLink to="/terminos-y-condiciones" style="color:inherit; text-decoration:none">Términos y condiciones</RouterLink></div>
          <div class="small"><RouterLink to="/politica-de-privacidad" style="color:inherit; text-decoration:none">Política de privacidad</RouterLink></div>
        </div>
      </div>
      <div class="container" style="padding:12px 0;border-top:1px solid #1f2937;color:#94a3b8">
        <div class="small">© 2025 OficiosYa</div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { profesionalService } from '../services/profesionalService'
import { CATEGORIAS_SERVICIO } from '../utils/constants'
import CiudadAutocomplete from '../components/CiudadAutocomplete.vue'
import SearchableDropdown from '../components/SearchableDropdown.vue'
import type { Profesional } from '../types'
import type { CiudadSugerida } from '../services/geocodingService'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

// Estado reactivo
const busqueda = ref({
  oficio: '',
  zona: ''
})

const profesionalesDestacados = ref<Profesional[]>([])
const tipoBusqueda = ref<'oficiante' | 'profesional'>('oficiante')
const categoriasCargadas = ref<Array<{ id: number; nombre: string; tipo: string; icono?: string }>>([])
const cargandoCategorias = ref(false)

// Opciones del dropdown según el tipo de búsqueda - cargadas desde el backend
const opcionesDropdown = computed(() => {
  const tipo = tipoBusqueda.value === 'oficiante' ? 'Oficiante' : 'Profesional'
  return categoriasCargadas.value
    .filter(c => c.tipo === tipo)
    .map(c => c.nombre)
})

// Datos estáticos
const categorias = ref([
  { nombre: 'Electricista', descripcion: 'Instalaciones y urgencias', icono: '⚡' },
  { nombre: 'Plomería', descripcion: 'Pérdidas, caños, bombas', icono: '🚰' },
  { nombre: 'Construcción', descripcion: 'Obras y refacciones', icono: '🧱' },
  { nombre: 'Gasista', descripcion: 'Habilitaciones y reparaciones', icono: '🔥' },
  { nombre: 'Pintura', descripcion: 'Interior y exterior', icono: '🎨' },
  { nombre: 'Jardinería', descripcion: 'Mantenimiento y riego', icono: '🌿' },
  { nombre: 'Cerrajería', descripcion: 'Aperturas y copias', icono: '🔒' },
  { nombre: 'Limpieza', descripcion: 'Hogar y oficinas', icono: '🧹' }
])

const pasos = ref([
  {
    numero: 1,
    titulo: 'Paso 1',
    descripcion: 'Usá el buscador o elegí una categoría. Podrás filtrar por distancia y valoración.',
    badgeStyle: 'background:#e0f2fe;color:#075985'
  },
  {
    numero: 2,
    titulo: 'Paso 2',
    descripcion: 'Mirá fotos de trabajos, certificaciones y reseñas reales de clientes.',
    badgeStyle: 'background:#dcfce7;color:#14532d'
  },
  {
    numero: 3,
    titulo: 'Paso 3',
    descripcion: 'Contactate por WhatsApp o por email y pedí tu presupuesto sin compromiso. ¡Listo!',
    badgeStyle: 'background:#fef9c3;color:#854d0e'
  }
])

const testimonios = ref([
  {
    id: 1,
    comentario: 'Conseguí electricista en 5 minutos y resolvió el mismo día.',
    autor: 'Sofía, SM de Tucumán'
  },
  {
    id: 2,
    comentario: 'Me encantó poder ver reseñas y fotos de trabajos reales.',
    autor: 'Martín, Yerba Buena'
  },
  {
    id: 3,
    comentario: 'Como profesional, recibo pedidos todas las semanas.',
    autor: 'Daniela, Plomería'
  }
])

// Métodos
const onCiudadSeleccionada = (ciudad: CiudadSugerida) => {
  // La ciudad ya está asignada al modelo por el componente
  // Aquí puedes hacer algo adicional si es necesario, como guardar las coordenadas
}

const buscarProfesionales = async () => {
  try {
    // Construir los parámetros de búsqueda (sin oficio/zona se listan todos)
    const params: any = {}
    if (busqueda.value.oficio) params.oficio = busqueda.value.oficio
    if (busqueda.value.zona) params.zona = busqueda.value.zona
    
    // Agregar el tipo de búsqueda (oficiante o profesional)
    params.tipo = tipoBusqueda.value

    // Navegar a la página de resultados con los parámetros de búsqueda
    router.push({
      name: tipoBusqueda.value === 'oficiante' ? 'Oficiantes' : 'Oficiantes', // Por ahora usamos la misma vista
      query: params
    })
  } catch (error: any) {
    console.error('Error al buscar profesionales:', error)
    alert('Error al realizar la búsqueda. Por favor, intentá nuevamente.')
  }
}

const irARegistro = () => {
  router.push('/registrar-profesional')
}

const irALogin = () => {
  router.push('/login')
}

const cerrarSesion = () => {
  authStore.logout()
  router.push('/')
}

const verPerfil = (id: number) => {
  router.push({ name: 'PerfilProfesional', params: { id: String(id) } })
}

const irAInicio = () => {
  document.getElementById('inicio')?.scrollIntoView({ behavior: 'smooth' })
}

const irAOficiantes = () => {
  router.push('/oficiantes')
}

const irAPublicarTrabajo = () => {
  router.push('/publicar-trabajo')
}

const irABuscarEmpleos = () => {
  router.push('/empleos')
}

const cambiarTipoBusqueda = async (tipo: 'oficiante' | 'profesional') => {
  tipoBusqueda.value = tipo
  busqueda.value.oficio = '' // Sin filtro por defecto, se listan todos
  await cargarCategorias(tipo === 'oficiante' ? 'Oficiante' : 'Profesional')
  await cargarProfesionalesDestacados()
}

const cargarCategorias = async (tipo?: 'Oficiante' | 'Profesional') => {
  cargandoCategorias.value = true
  try {
    // Si no se especifica tipo, cargar todas las categorías
    // Si se especifica tipo, cargar solo las de ese tipo
    const categorias = await profesionalService.obtenerCategorias(tipo)
    categoriasCargadas.value = categorias || []
    
    // Si no hay categorías y hay un tipo especificado, usar fallback
    if (categoriasCargadas.value.length === 0 && tipo) {
      console.warn(`No se encontraron categorías para el tipo: ${tipo}, usando datos estáticos`)
      // Fallback a datos estáticos si falla la API
      if (tipo === 'Oficiante') {
        categoriasCargadas.value = CATEGORIAS_SERVICIO.OFICIANTE.map((c, i) => ({
          id: i + 1,
          nombre: c.nombre,
          tipo: 'Oficiante',
          icono: c.icono
        }))
      } else {
        categoriasCargadas.value = CATEGORIAS_SERVICIO.PROFESIONAL.map((c, i) => ({
          id: i + 1,
          nombre: c.nombre,
          tipo: 'Profesional',
          icono: c.icono
        }))
      }
    }
  } catch (error: any) {
    console.error('Error al cargar categorías:', error)
    
    // Fallback a datos estáticos si falla la API
    const tipoFallback = tipo || (tipoBusqueda.value === 'oficiante' ? 'Oficiante' : 'Profesional')
    if (tipoFallback === 'Oficiante') {
      categoriasCargadas.value = CATEGORIAS_SERVICIO.OFICIANTE.map((c, i) => ({
        id: i + 1,
        nombre: c.nombre,
        tipo: 'Oficiante',
        icono: c.icono
      }))
    } else {
      categoriasCargadas.value = CATEGORIAS_SERVICIO.PROFESIONAL.map((c, i) => ({
        id: i + 1,
        nombre: c.nombre,
        tipo: 'Profesional',
        icono: c.icono
      }))
    }
  } finally {
    cargandoCategorias.value = false
  }
}

const cargarProfesionalesDestacados = async () => {
  try {
    const tipo = tipoBusqueda.value === 'oficiante' ? 'Oficiante' : 'Profesional'
    profesionalesDestacados.value = await profesionalService.obtenerDestacados(tipo)
  } catch (error) {
    // Datos de ejemplo si falla la API - coinciden con la imagen
    profesionalesDestacados.value = [
      {
        id: 1,
        usuarioId: 1,
        descripcion: 'Electricista matriculada',
        calificacionPromedio: 4.9,
        totalResenas: 32,
        disponible: true,
        fechaRegistro: '2024-01-01',
        activo: true,
        usuario: { id: 1, nombre: 'María López', email: 'maria@email.com', fechaRegistro: '2024-01-01', esProfesional: true, activo: true },
        oficios: [{ id: 1, nombre: 'Electricista', descripcion: '', activo: true, fechaCreacion: '2024-01-01' }],
        certificados: [],
        resenas: Array(32).fill({})
      },
      {
        id: 2,
        usuarioId: 2,
        descripcion: 'Plomería y gas',
        calificacionPromedio: 4.6,
        totalResenas: 21,
        disponible: true,
        fechaRegistro: '2024-01-01',
        activo: true,
        usuario: { id: 2, nombre: 'Diego Pérez', email: 'diego@email.com', fechaRegistro: '2024-01-01', esProfesional: true, activo: true },
        oficios: [{ id: 2, nombre: 'Plomero', descripcion: '', activo: true, fechaCreacion: '2024-01-01' }],
        certificados: [],
        resenas: Array(21).fill({})
      },
      {
        id: 3,
        usuarioId: 3,
        descripcion: 'Pintura profesional',
        calificacionPromedio: 4.9,
        totalResenas: 14,
        disponible: true,
        fechaRegistro: '2024-01-01',
        activo: true,
        usuario: { id: 3, nombre: 'Ana Rivas', email: 'ana@email.com', fechaRegistro: '2024-01-01', esProfesional: true, activo: true },
        oficios: [{ id: 3, nombre: 'Pintora', descripcion: '', activo: true, fechaCreacion: '2024-01-01' }],
        certificados: [],
        resenas: Array(14).fill({})
      },
      {
        id: 4,
        usuarioId: 4,
        descripcion: 'Gasista matriculado',
        calificacionPromedio: 4.6,
        totalResenas: 18,
        disponible: true,
        fechaRegistro: '2024-01-01',
        activo: true,
        usuario: { id: 4, nombre: 'Carlos Díaz', email: 'carlos@email.com', fechaRegistro: '2024-01-01', esProfesional: true, activo: true },
        oficios: [{ id: 4, nombre: 'Gasista', descripcion: '', activo: true, fechaCreacion: '2024-01-01' }],
        certificados: [],
        resenas: Array(18).fill({})
      }
    ]
  }
}

// Sincronizar tipo desde la URL (Oficiantes -> ?tipo=oficiante, Profesionales -> ?tipo=profesional)
function aplicarTipoDesdeRuta () {
  const tipo = route.query.tipo as string
  if (tipo === 'profesional') {
    tipoBusqueda.value = 'profesional'
    cargarCategorias('Profesional')
    cargarProfesionalesDestacados()
  } else {
    tipoBusqueda.value = 'oficiante'
    cargarCategorias('Oficiante')
    cargarProfesionalesDestacados()
  }
}

watch(() => route.query.tipo, () => {
  aplicarTipoDesdeRuta()
}, { immediate: false })

onMounted(async () => {
  const tipo = route.query.tipo as string
  if (tipo === 'profesional') tipoBusqueda.value = 'profesional'
  await Promise.all([
    cargarCategorias(tipo === 'profesional' ? 'Profesional' : 'Oficiante'),
    cargarProfesionalesDestacados()
  ])
})
</script>

<style scoped>
/* Estilos personalizados que mantienen el diseño original */
.avatar {
  width: 40px;
  height: 40px;
  background: linear-gradient(135deg, #e2e8f0, #94a3b8);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* Estilos para el formulario de búsqueda como en la imagen */
.search-form {
  margin-bottom: 20px;
}

.search-container {
  display: flex;
  background: white;
  border-radius: 12px;
  padding: 4px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  gap: 4px;
}

.search-field {
  display: flex;
  align-items: center;
  padding: 12px 16px;
  gap: 8px;
  flex: 1;
  color: #64748b;
}

.search-field svg {
  color: #64748b;
  flex-shrink: 0;
}

.search-select {
  border: none;
  outline: none;
  background: transparent;
  color: #0f172a;
  font-size: 14px;
  width: 100%;
  cursor: pointer;
  appearance: none;
  -webkit-appearance: none;
  -moz-appearance: none;
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 12 12'%3E%3Cpath fill='%2364748b' d='M6 9L1 4h10z'/%3E%3C/svg%3E");
  background-repeat: no-repeat;
  background-position: right 8px center;
  padding-right: 28px;
  padding-left: 12px;
}

.search-select option {
  padding-left: 12px;
}

.search-input::placeholder {
  color: #94a3b8;
}

.search-button {
  background: #22c55e;
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: background-color 0.2s;
}

.search-button:hover {
  background: #16a34a;
}

.search-tip {
  color: #cbd5e1;
  font-size: 14px;
}

/* Estilos para la card de profesionales */
.hero-card {
  background: white;
  border-radius: 16px;
  padding: 24px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
  color: #0f172a;
}

.verified-badge {
  display: inline-block;
  background: #10b981;
  color: white;
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 600;
  margin-bottom: 16px;
}

.professionals-title {
  font-size: 20px;
  font-weight: 700;
  color: #0f172a;
  margin-bottom: 20px;
}

.professionals-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
  margin-top: 16px;
}

.professional-card {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 12px;
  background: #f8fafc;
  border-radius: 12px;
  transition: transform 0.2s;
  border: 1px solid #e5e7eb;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.05);
  cursor: pointer;
}

.professional-card:hover {
  transform: translateY(-2px);
}

.professional-avatar {
  width: 40px;
  height: 40px;
  background: linear-gradient(135deg, #e2e8f0, #94a3b8);
  border-radius: 50%;
  flex-shrink: 0;
}

.professional-info {
  flex: 1;
}

.professional-name {
  font-weight: 600;
  color: #0f172a;
  font-size: 14px;
  margin-bottom: 2px;
}

.professional-details {
  color: #64748b;
  font-size: 12px;
  margin-bottom: 4px;
}

.professional-rating {
  display: flex;
  align-items: center;
  gap: 4px;
}

.stars {
  color: #f59e0b;
  font-size: 12px;
}

.rating-text {
  color: #64748b;
  font-size: 11px;
}

/* Asegurar que los profesionales se vean */
.professionals-grid > div {
  min-height: 80px;
  display: flex;
  align-items: center;
}

/* Debug: hacer visible los elementos */
.professional-card {
  min-height: 60px;
  opacity: 1;
  visibility: visible;
}

.min-vh-50 {
  min-height: 50vh;
}

/* Mantener el estilo de las estrellas */
.stars {
  color: #f59e0b;
}

/* Mantener el estilo de los pasos */
.step-badge {
  width: 60px;
  height: 60px;
  background: var(--brand);
  color: white;
  border-radius: 50%;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 1.8rem;
  font-weight: 700;
  margin: 0 auto 20px auto;
}

/* Mantener el estilo de las categorías */
.cat-icon {
  width: 42px;
  height: 42px;
  border-radius: 12px;
  display: grid;
  place-items: center;
  background: #f1f5f9;
  font-size: 1.5rem;
}

/* Mantener el estilo del hero card */
.hero-card {
  background: #ffffff;
  color: var(--text);
  border-radius: var(--radius-lg);
  padding: 20px;
  box-shadow: var(--shadow);
}

/* Mantener el estilo del badge */
.badge {
  display: inline-block;
  padding: 6px 10px;
  border-radius: 999px;
  background: #10b9811a;
  color: #065f46;
  font-weight: 600;
  font-size: 12px;
}

/* Mantener el estilo del CTA block */
.cta-block {
  background: linear-gradient(135deg, #22c55e, #06b6d4);
  border-radius: var(--radius-lg);
  padding: 28px;
  color: white;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 18px;
}

.cta-block button {
  background: #0f172a;
  color: #fff;
  border: 0;
  border-radius: 999px;
  padding: 12px 18px;
  cursor: pointer;
  transition: background-color 0.2s;
}

.cta-block button:hover {
  background: #1e293b;
}

/* Mantener el estilo del footer */
footer {
  background: var(--bg-soft);
  color: #cbd5e1;
}

.footer {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr 1fr;
  gap: 16px;
  padding: 32px 0;
}

.small {
  font-size: 13px;
  color: #94a3b8;
}

@media (max-width: 480px) {
  /* Responsive para el formulario de búsqueda */
  .search-container {
    flex-direction: column;
    gap: 8px;
  }
  
  .search-field {
    border: 1px solid #e5e7eb;
    border-radius: 8px;
  }
  
  .search-button {
    width: 100%;
  }
  
  /* Responsive para la grid de profesionales */
  .professionals-grid {
    grid-template-columns: 1fr;
    gap: 12px;
  }
  
  .professional-card {
    padding: 16px;
  }
}
</style>
