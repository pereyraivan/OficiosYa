<template>
  <div class="oficiantes">
    <!-- Resultados de Búsqueda -->
    <section v-if="mostrarResultados" class="section section-fondo-publicar section-resultados">
      <div class="container">
        <div class="resultados-header">
          <button type="button" class="btn-volver" @click="limpiarBusqueda">
            ← Explorar categorías
          </button>
          <h2 class="resultados-titulo">Resultados de búsqueda</h2>
          <p v-if="criteriosBusqueda.oficio || criteriosBusqueda.zona" class="resultados-criterios">
            <span v-if="criteriosBusqueda.oficio">Oficio: <strong>{{ criteriosBusqueda.oficio }}</strong></span>
            <span v-if="criteriosBusqueda.oficio && criteriosBusqueda.zona"> • </span>
            <span v-if="criteriosBusqueda.zona">Zona: <strong>{{ criteriosBusqueda.zona }}</strong></span>
          </p>
        </div>

        <div v-if="cargandoResultados" style="text-align: center; padding: 60px 0;">
          <div style="font-size: 1.2rem; color: var(--muted);">Buscando profesionales...</div>
        </div>

        <div v-else-if="resultadosBusqueda.length === 0" style="text-align: center; padding: 60px 0;">
          <div style="font-size: 1.2rem; color: var(--muted); margin-bottom: 20px;">
            No se encontraron profesionales con los criterios de búsqueda
          </div>
          <button class="cta" @click="limpiarBusqueda" style="background: var(--brand); color: white; border: none; padding: 12px 24px; border-radius: 8px; cursor: pointer;">
            Explorar categorías
          </button>
        </div>

        <div v-else class="resultados-grid">
          <div
            v-for="profesional in resultadosBusqueda"
            :key="profesional.id"
            class="profesional-card"
            @click="verPerfil(profesional.id)"
          >
            <div class="profesional-avatar">
              {{ profesional.usuario?.nombre?.charAt(0) || 'P' }}
            </div>
            <div class="profesional-info">
              <h3>{{ profesional.usuario?.nombre || 'Sin nombre' }}</h3>
              <p class="profesional-oficio">
                {{ profesional.oficios?.[0]?.nombre || 'Sin oficio especificado' }}
              </p>
              <p v-if="profesional.usuario?.ciudad || profesional.usuario?.provincia" class="profesional-ubicacion">
                📍 {{ textoUbicacion(profesional) }}
              </p>
              <div v-if="profesional.calificacionPromedio" class="profesional-rating">
                <span
                  v-for="n in 5"
                  :key="n"
                  class="rating-star"
                  :class="claseEstrella(n, profesional.calificacionPromedio)"
                  aria-hidden="true"
                >★</span>
                <span class="rating-value">{{ profesional.calificacionPromedio.toFixed(1) }}</span>
                <span class="rating-count">({{ profesional.totalResenas || 0 }} reseñas)</span>
              </div>
            </div>
            <button class="btn-contactar" @click.stop="verPerfil(profesional.id)">
              Ver perfil
            </button>
          </div>
        </div>
      </div>
    </section>

    <!-- Categorías de Oficiantes -->
    <section v-if="!mostrarResultados" id="categorias-oficiantes" class="section section-fondo-publicar" style="padding: 80px 0;">
      <div class="container">
        <div style="text-align: center; margin-bottom: 60px;">
          <h2 style="font-size: 2.5rem; margin-bottom: 20px; color: var(--text);">
            Categorías de Oficiantes
          </h2>
          <p style="font-size: 1.1rem; color: var(--muted); max-width: 600px; margin: 0 auto;">
            Explorá las diferentes especialidades de oficiantes disponibles
          </p>
        </div>

        <div class="categorias-grid">
          <div
            v-for="categoria in CATEGORIAS_SERVICIO.OFICIANTE"
            :key="categoria.nombre"
            class="categoria-card"
            @click="buscarPorCategoria(categoria.nombre)"
          >
            <div class="categoria-icon">{{ categoria.icono }}</div>
            <h3>{{ categoria.nombre }}</h3>
            <p>{{ categoria.descripcion }}</p>
            <div class="categoria-arrow">→</div>
          </div>
        </div>
      </div>
    </section>

    <!-- Cómo funciona -->
    <section v-if="!mostrarResultados" class="section" style="padding: 80px 0; background: white;">
      <div class="container">
        <div style="text-align: center; margin-bottom: 60px;">
          <h2 style="font-size: 2.5rem; margin-bottom: 20px; color: var(--text);">
            ¿Cómo funciona?
          </h2>
          <p style="font-size: 1.1rem; color: var(--muted); max-width: 600px; margin: 0 auto;">
            Encontrar el oficiante perfecto es fácil y rápido
          </p>
        </div>

        <div class="pasos-grid">
          <div class="paso-card">
            <div class="paso-numero">1</div>
            <h3>Buscar</h3>
            <p>Encontrá oficiantes en tu zona por categoría o especialidad</p>
          </div>
          <div class="paso-card">
            <div class="paso-numero">2</div>
            <h3>Comparar</h3>
            <p>Revisá perfiles, reseñas y precios de diferentes oficiantes</p>
          </div>
          <div class="paso-card">
            <div class="paso-numero">3</div>
            <h3>Contactar</h3>
            <p>Elegí el mejor oficiante y contactalo directamente</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Footer -->
    <footer>
      <div class="container footer">
        <div>
          <div class="logo" style="color:white; cursor: pointer;" @click="irAInicio">
            <div class="logo-badge">🛠️</div>
            <span>{{ appName }}</span>
          </div>
          <p class="small" style="margin-top:8px">Conectamos personas con profesionales de confianza en su zona.</p>
        </div>
        <div>
          <strong>Ayuda</strong>
          <div class="small" style="margin-top:8px"><a href="#" @click.prevent="abrirAyuda" style="color:inherit">Centro de ayuda</a></div>
          <div class="small"><a href="#" @click.prevent="abrirAyuda" style="color:inherit">Contacto</a></div>
        </div>
        <div>
          <strong>Legal</strong>
          <div class="small" style="margin-top:8px"><RouterLink to="/terminos-y-condiciones" style="color:inherit; text-decoration:none">Términos y condiciones</RouterLink></div>
          <div class="small"><RouterLink to="/politica-de-privacidad" style="color:inherit; text-decoration:none">Política de privacidad</RouterLink></div>
          <div class="small"><RouterLink to="/arrepentimiento-y-baja" style="color:inherit; text-decoration:none">Arrepentimiento y baja</RouterLink></div>
        </div>
      </div>
      <div class="container footer-copy" style="border-top:1px solid #1f2937;color:#94a3b8">
        <div class="small">© 2026 {{ appName }}</div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed, nextTick, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { CATEGORIAS_SERVICIO, APP_CONFIG } from '../utils/constants'
import { abrirMenuSoporteWhatsApp } from '../utils/soporteWhatsApp'
import { profesionalService } from '../services/profesionalService'
import type { Profesional } from '../types'

const appName = APP_CONFIG.APP_NAME

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

// Estado de búsqueda
const resultadosBusqueda = ref<Profesional[]>([])
const cargandoResultados = ref(false)
const busquedaRealizada = ref(false)
const criteriosBusqueda = ref({
  oficio: '',
  zona: '',
  tipo: ''
})

// Mostrar resultados cuando hay criterios en la URL o cuando ya se buscó
const tieneCriteriosEnUrl = computed(() => {
  const oficio = (route.query.oficio as string)?.trim() || ''
  const zona = (route.query.zona as string)?.trim() || ''
  return !!(oficio || zona)
})

const mostrarResultados = computed(() => busquedaRealizada.value || tieneCriteriosEnUrl.value)

function claseEstrella (n: number, rating: number | undefined) {
  const r = Math.min(5, Math.max(0, Number(rating ?? 0)))
  return { filled: r >= n, half: r >= n - 0.5 && r < n }
}

// Ubicación sin repetir localidad/ciudad (ej. "Famaillá, Famaillá, Tucumán" -> "Famaillá, Tucumán")
function textoUbicacion (profesional: { usuario?: { ciudad?: string; provincia?: string } }) {
  const c = profesional?.usuario?.ciudad?.trim() || ''
  const p = profesional?.usuario?.provincia?.trim() || ''
  if (!c && !p) return ''
  if (!c) return p
  if (!p) return c
  const partes = c.split(',').map(s => s.trim()).filter(Boolean)
  const sinRepetir: string[] = []
  for (const parte of partes) {
    if (sinRepetir.length === 0 || sinRepetir[sinRepetir.length - 1].toLowerCase() !== parte.toLowerCase())
      sinRepetir.push(parte)
  }
  const cNorm = sinRepetir.join(', ')
  const cLower = cNorm.toLowerCase()
  const pLower = p.toLowerCase()
  if (cLower.endsWith(pLower) || cLower.includes(`, ${pLower}`)) return cNorm
  return `${cNorm}, ${p}`
}

// Métodos de navegación
const irAInicio = () => {
  router.push('/')
}

const abrirAyuda = () => {
  abrirMenuSoporteWhatsApp()
}

const irAOficiantes = () => {
  router.push('/oficiantes')
}

const irAProfesionales = () => {
  // Por ahora redirigir a inicio, luego crear vista de profesionales
  router.push('/')
}

const irAPublicarTrabajo = () => {
  router.push('/publicar-trabajo')
}

const irABuscarEmpleos = () => {
  router.push('/empleos')
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

const buscarPorCategoria = (categoria: string) => {
  router.push({
    name: 'Oficiantes',
    query: { oficio: categoria, tipo: 'oficiante' }
  })
}

// Realizar búsqueda de profesionales (sin filtros = listar todos)
const realizarBusqueda = async (criterios: { oficio?: string; zona?: string; tipo?: string }) => {
  cargandoResultados.value = true
  criteriosBusqueda.value = {
    oficio: criterios.oficio || '',
    zona: criterios.zona || '',
    tipo: criterios.tipo || ''
  }

  try {
    const resultados = await profesionalService.buscar({
      oficio: criterios.oficio || undefined,
      zona: criterios.zona || undefined
    })
    resultadosBusqueda.value = resultados || []
  } catch (error: any) {
    console.error('Error al buscar profesionales:', error)
    resultadosBusqueda.value = []
  } finally {
    cargandoResultados.value = false
    busquedaRealizada.value = true
  }
}

// Volver a explorar categorías (sin resultados ni hero intermedio)
const limpiarBusqueda = async () => {
  await router.replace({ name: 'Oficiantes', query: { vista: 'categorias' } })
  await nextTick()
  document.getElementById('categorias-oficiantes')?.scrollIntoView({ behavior: 'smooth' })
}

function aplicarBusquedaDesdeUrl () {
  const query = route.query
  const oficio = (query.oficio as string)?.trim() || ''
  const zona = (query.zona as string)?.trim() || ''
  const tipo = (query.tipo as string) || 'oficiante'

  // Sin criterios: mostrar categorías (sin listar todos ni hero si vista=categorias)
  if (!oficio && !zona) {
    busquedaRealizada.value = false
    resultadosBusqueda.value = []
    criteriosBusqueda.value = { oficio: '', zona: '', tipo }
    return
  }

  busquedaRealizada.value = true
  criteriosBusqueda.value = { oficio, zona, tipo }
  realizarBusqueda({ oficio, zona, tipo })
}

// Contactar profesional — conservar criterios para volver desde el perfil
const verPerfil = (id: number) => {
  const query: Record<string, string> = { from: 'busqueda' }
  if (criteriosBusqueda.value.oficio) query.oficio = criteriosBusqueda.value.oficio
  if (criteriosBusqueda.value.zona) query.zona = criteriosBusqueda.value.zona
  query.tipo = criteriosBusqueda.value.tipo || 'oficiante'

  router.push({
    name: 'PerfilProfesional',
    params: { id: String(id) },
    query
  })
}

watch(
  () => route.query,
  () => {
    if (route.name === 'Oficiantes') aplicarBusquedaDesdeUrl()
  },
  { deep: true }
)

onMounted(() => {
  aplicarBusquedaDesdeUrl()
})
</script>

<style scoped>
.oficiantes {
  min-height: 100vh;
}

.section-fondo-publicar {
  background: linear-gradient(135deg, #f0fdf4 0%, #ecfdf5 50%, #f0f9ff 100%);
}

.section-resultados {
  padding: 28px 0 48px;
  min-height: calc(100vh - 64px);
}

.resultados-header {
  margin-bottom: 32px;
}

.btn-volver {
  display: inline-block;
  margin-bottom: 12px;
  padding: 0;
  border: none;
  background: transparent;
  color: var(--brand, #22c55e);
  font-size: 0.9rem;
  font-weight: 600;
  cursor: pointer;
}

.btn-volver:hover {
  text-decoration: underline;
}

.resultados-titulo {
  font-size: 1.75rem;
  margin: 0 0 8px;
  color: var(--text);
}

.resultados-criterios {
  margin: 0;
  color: var(--muted);
}

/* Grid de categorías */
.categorias-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 24px;
  margin-top: 40px;
}

.categoria-card {
  background: white;
  border-radius: 16px;
  padding: 32px;
  text-align: center;
  cursor: pointer;
  transition: all 0.3s ease;
  border: 2px solid transparent;
  position: relative;
  overflow: hidden;
}

.categoria-card:hover {
  transform: translateY(-8px);
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
  border-color: var(--brand);
}

.categoria-card:hover .categoria-arrow {
  transform: translateX(8px);
}

.categoria-icon {
  font-size: 3rem;
  margin-bottom: 16px;
  display: block;
}

.categoria-card h3 {
  font-size: 1.5rem;
  margin-bottom: 12px;
  color: var(--text);
  font-weight: 600;
}

.categoria-card p {
  color: var(--muted);
  line-height: 1.6;
  margin-bottom: 20px;
}

.categoria-arrow {
  position: absolute;
  bottom: 20px;
  right: 20px;
  font-size: 1.5rem;
  color: var(--brand);
  transition: transform 0.3s ease;
}

/* Grid de resultados de búsqueda */
.resultados-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 24px;
  margin-top: 40px;
}

.profesional-card {
  background: white;
  border-radius: 12px;
  padding: 24px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;
  display: flex;
  flex-direction: column;
  gap: 16px;
  cursor: pointer;
}

.profesional-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.15);
}

.profesional-avatar {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  background: var(--brand);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  font-weight: 600;
  margin: 0 auto;
}

.profesional-info {
  text-align: center;
}

.profesional-info h3 {
  font-size: 1.3rem;
  margin-bottom: 8px;
  color: var(--text);
  font-weight: 600;
}

.profesional-oficio {
  color: var(--brand);
  font-weight: 500;
  margin-bottom: 8px;
}

.profesional-ubicacion {
  color: var(--muted);
  font-size: 0.9rem;
  margin-bottom: 8px;
}

.profesional-rating {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  margin-top: 8px;
}

.rating-star {
  display: inline-block;
  font-size: 0.9rem;
  color: transparent;
  -webkit-text-stroke: 1.2px #cbd5e1;
}
.rating-star.filled {
  color: #f59e0b;
  -webkit-text-stroke: 0;
}
.rating-star.half {
  position: relative;
}
.rating-star.half::before {
  content: '★';
  position: absolute;
  left: 0;
  top: 0;
  width: 50%;
  overflow: hidden;
  color: #f59e0b;
  -webkit-text-stroke: 0;
}
.stars {
  color: #fbbf24;
  font-size: 0.9rem;
}

.rating-value {
  font-weight: 600;
  color: var(--text);
}

.rating-count {
  color: var(--muted);
  font-size: 0.9rem;
}

.btn-contactar {
  background: var(--brand);
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
  margin-top: auto;
}

.btn-contactar:hover {
  background: #2563eb;
  transform: translateY(-1px);
}

/* Grid de pasos */
.pasos-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 40px;
  margin-top: 40px;
}

.paso-card {
  text-align: center;
  padding: 32px 20px;
}

.paso-numero {
  width: 60px;
  height: 60px;
  background: var(--brand);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  font-weight: 700;
  margin: 0 auto 20px;
}

.paso-card h3 {
  font-size: 1.3rem;
  margin-bottom: 12px;
  color: var(--text);
  font-weight: 600;
}

.paso-card p {
  color: var(--muted);
  line-height: 1.6;
}

/* Responsive */
@media (max-width: 768px) {
  .categorias-grid {
    grid-template-columns: 1fr;
  }
  
  .pasos-grid {
    grid-template-columns: 1fr;
  }

  footer .container.footer,
  footer .footer-copy {
    padding-left: 16px !important;
    padding-right: 16px !important;
    box-sizing: border-box;
  }

  footer .footer {
    padding-top: 16px !important;
    padding-bottom: 12px !important;
    gap: 10px 16px !important;
  }

  footer .footer > div:nth-child(2) {
    padding-left: 12px;
  }

  footer .footer-copy {
    padding-top: 10px !important;
    padding-bottom: 10px !important;
  }
}
</style>
