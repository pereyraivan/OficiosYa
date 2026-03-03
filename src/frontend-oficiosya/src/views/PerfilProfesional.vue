<template>
  <div class="perfil-profesional">
    <main class="perfil-main">
      <div v-if="cargando" class="perfil-loading">
        <p>Cargando perfil...</p>
      </div>

      <div v-else-if="error" class="perfil-error">
        <p>{{ error }}</p>
        <button class="cta" @click="irAOficiantes">Volver a búsqueda</button>
      </div>

      <div v-else-if="profesional" class="container perfil-container">
        <!-- Card principal -->
        <div class="perfil-card">
          <!-- Tabs: paso 1 Datos y contacto, paso 2 Reseñas -->
          <div class="perfil-tabs">
            <button
              type="button"
              class="perfil-tab"
              :class="{ active: tabActivo === 'datos' }"
              @click="tabActivo = 'datos'"
            >
              Datos y contacto
            </button>
            <button
              type="button"
              class="perfil-tab"
              :class="{ active: tabActivo === 'resenas' }"
              @click="tabActivo = 'resenas'"
            >
              Reseñas ({{ resenas.length }})
            </button>
          </div>

          <!-- Paso 1: Datos personales y contacto -->
          <div v-show="tabActivo === 'datos'" class="perfil-tab-panel">
          <div class="perfil-header">
            <div class="perfil-avatar">
              {{ nombreIniciales }}
            </div>
            <div class="perfil-header-info">
              <h1 class="perfil-nombre">{{ profesional.usuario?.nombre || 'Sin nombre' }}</h1>
              <div class="perfil-oficios">
                <span
                  v-for="(oficio, i) in profesional.oficios"
                  :key="i"
                  class="badge-oficio"
                >
                  {{ oficio.nombre || oficio }}
                </span>
              </div>
              <div v-if="profesional.usuario?.ciudad || profesional.usuario?.provincia" class="perfil-ubicacion">
                📍 {{ textoUbicacion }}
              </div>
              <div class="perfil-meta">
                <div v-if="(profesional.calificacionPromedio ?? 0) > 0" class="perfil-rating">
                  <span
                    v-for="n in 5"
                    :key="n"
                    class="rating-star"
                    :class="{
                      filled: ratingPromedio >= n,
                      half: ratingPromedio >= n - 0.5 && ratingPromedio < n
                    }"
                    aria-hidden="true"
                  >★</span>
                  <span class="rating-num">{{ Number(profesional.calificacionPromedio).toFixed(1) }}</span>
                  <span class="rating-count">({{ profesional.totalResenas ?? 0 }} reseñas)</span>
                </div>
                <span v-if="profesional.disponible" class="badge-disponible">Disponible</span>
              </div>
            </div>
          </div>

          <div v-if="profesional.usuario?.email || profesional.usuario?.telefono" class="perfil-section perfil-contacto">
            <h2>Contacto</h2>
            <div class="contacto-lista">
              <a v-if="profesional.usuario?.email" :href="`mailto:${profesional.usuario.email}`" class="contacto-item" target="_blank" rel="noopener">
                <span class="contacto-icono">✉️</span>
                <span>{{ profesional.usuario.email }}</span>
              </a>
              <a v-if="profesional.usuario?.telefono" :href="urlWhatsApp(profesional.usuario.telefono)" class="contacto-item" target="_blank" rel="noopener">
                <span class="contacto-icono contacto-icono-whatsapp" aria-hidden="true">
                  <svg viewBox="0 0 24 24" width="22" height="22" fill="currentColor">
                    <path d="M17.472 14.382c-.297-.149-1.758-.867-2.03-.967-.273-.099-.471-.148-.67.15-.197.297-.767.966-.94 1.164-.173.199-.347.223-.644.075-.297-.15-1.255-.463-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.298-.347.446-.52.149-.174.198-.298.298-.497.099-.198.05-.371-.025-.52-.075-.149-.669-1.612-.916-2.207-.242-.579-.487-.5-.669-.51-.173-.008-.371-.01-.57-.01-.198 0-.52.074-.792.372-.272.297-1.04 1.016-1.04 2.479 0 1.462 1.065 2.875 1.213 3.074.149.198 2.096 3.2 5.077 4.487.709.306 1.262.489 1.694.625.712.227 1.36.195 1.871.118.571-.085 1.758-.719 2.006-1.413.248-.694.248-1.289.173-1.413-.074-.124-.272-.198-.57-.347m-5.421 7.403h-.004a9.87 9.87 0 01-5.031-1.378l-.361-.214-3.741.982.998-3.648-.235-.374a9.86 9.86 0 01-1.51-5.26c.001-5.45 4.436-9.884 9.888-9.884 2.64 0 5.122 1.03 6.988 2.898a9.865 9.865 0 012.893 6.994c-.003 5.45-4.437 9.884-9.885 9.884m8.413-18.297A11.815 11.815 0 0012.05 0C5.495 0 .16 5.335.157 11.892c0 2.096.547 4.142 1.588 5.945L.057 24l6.305-1.654a11.882 11.882 0 005.683 1.448h.005c6.554 0 11.89-5.335 11.893-11.893a11.821 11.821 0 00-3.48-8.413z"/>
                  </svg>
                </span>
                <span>{{ profesional.usuario.telefono }}</span>
              </a>
            </div>
          </div>

          <div v-if="profesional.descripcion" class="perfil-section">
            <h2>Sobre mí</h2>
            <p class="perfil-descripcion">{{ profesional.descripcion }}</p>
          </div>

          <div v-if="profesional.oficios?.length" class="perfil-section">
            <h2>Servicios</h2>
            <ul class="lista-servicios">
              <li v-for="(oficio, i) in profesional.oficios" :key="i">
                {{ oficio.nombre || oficio }}
              </li>
            </ul>
          </div>

          <div v-if="zonasCobertura.length" class="perfil-section">
            <h2>Zonas de cobertura</h2>
            <ul class="lista-zonas">
              <li v-for="(zona, i) in zonasCobertura" :key="i">
                {{ [zona.localidad, (zona.departamento && zona.departamento !== zona.localidad ? zona.departamento : null), zona.provincia].filter(Boolean).join(', ') }}
              </li>
            </ul>
          </div>

          <div v-if="imagenesTrabajo.length > 0" class="perfil-section">
            <h2>Trabajos realizados</h2>
            <div class="carousel-trabajos">
              <button type="button" class="carousel-btn carousel-prev" @click="carouselPrev" :disabled="carouselIndex <= 0">‹</button>
              <div class="carousel-inner">
                <div
                  v-for="(img, i) in imagenesTrabajo"
                  :key="img.id || i"
                  class="carousel-slide"
                  :class="{ 'active': i === carouselIndex }"
                >
                  <img :src="urlImagenTrabajo(img.urlImagen)" :alt="img.titulo || 'Trabajo realizado'" class="carousel-img" />
                  <p v-if="img.titulo" class="carousel-caption">{{ img.titulo }}</p>
                </div>
              </div>
              <button type="button" class="carousel-btn carousel-next" @click="carouselNext" :disabled="carouselIndex >= imagenesTrabajo.length - 1">›</button>
            </div>
            <div v-if="imagenesTrabajo.length > 1" class="carousel-dots">
              <button
                v-for="(_, i) in imagenesTrabajo"
                :key="i"
                type="button"
                class="carousel-dot"
                :class="{ 'active': i === carouselIndex }"
                @click="carouselIndex = i"
              />
            </div>
          </div>

          <div class="perfil-actions">
            <button class="cta cta-contactar" @click="contactar">
              Contactar
            </button>
            <button class="cta-outline" @click="irAOficiantes">Ver más profesionales</button>
          </div>
          </div>
          <!-- Fin paso 1 -->

          <!-- Paso 2: Reseñas -->
          <div v-show="tabActivo === 'resenas'" class="perfil-tab-panel perfil-resenas-panel">
          <div class="perfil-section perfil-resenas">
            <h2 class="perfil-resenas-titulo">Reseñas de {{ profesional.usuario?.nombre || 'este profesional' }}</h2>

            <!-- Formulario para dejar reseña (todos pueden, sin login; solo se oculta en el propio perfil) -->
            <div v-if="mostrarFormularioResena" class="resena-form">
              <div class="resena-form-campo">
                <label for="resena-nombre">Tu nombre (cómo querés que aparezca)</label>
                <input
                  id="resena-nombre"
                  v-model="formResena.autorNombre"
                  type="text"
                  class="resena-input"
                  :placeholder="authStore.estaAutenticado ? 'Opcional: se usará tu nombre de cuenta' : 'Ej: María, Juan P., Cliente satisfecho...'"
                  maxlength="100"
                />
              </div>
              <div class="resena-form-stars">
                <span class="resena-form-label">Calificación</span>
                <button
                  v-for="n in 5"
                  :key="n"
                  type="button"
                  class="star-btn"
                  :class="{ active: formResena.calificacion >= n }"
                  @click="formResena.calificacion = n"
                  :aria-label="`${n} estrella(s)`"
                >
                  ★
                </button>
              </div>
              <textarea
                v-model="formResena.comentario"
                class="resena-textarea"
                placeholder="Contá tu experiencia con este profesional..."
                rows="3"
                maxlength="1000"
              />
              <div class="resena-form-footer">
                <span class="resena-chars">{{ formResena.comentario.length }}/1000</span>
                <button
                  type="button"
                  class="cta"
                  :disabled="!puedeEnviarResena"
                  @click="enviarResena"
                >
                  {{ enviandoResena ? 'Enviando...' : 'Publicar reseña' }}
                </button>
              </div>
              <p v-if="errorResena" class="resena-error">{{ errorResena }}</p>
            </div>
            <p v-else-if="esPropioPerfil" class="resena-aviso">
              No podés dejar reseña en tu propio perfil.
            </p>

            <!-- Lista de reseñas -->
            <div v-if="cargandoResenas" class="resenas-loading">Cargando reseñas...</div>
            <ul v-else-if="resenas.length > 0" class="lista-resenas">
              <li v-for="r in resenas" :key="r.id" class="resena-item">
                <div class="resena-header">
                  <span class="resena-autor">{{ r.nombreAutor ?? r.usuario?.nombre ?? 'Usuario' }}</span>
                  <span class="resena-fecha">{{ formatearFechaResena(r.fechaCreacion) }}</span>
                </div>
                <div class="resena-stars" :aria-label="`${r.calificacion} de 5`">
                  <span v-for="n in 5" :key="n" class="resena-star" :class="{ filled: r.calificacion >= n }">★</span>
                </div>
                <p v-if="r.comentario" class="resena-comentario">{{ r.comentario }}</p>
              </li>
            </ul>
            <p v-else class="resenas-vacio">Aún no hay reseñas. ¡Sé el primero en opinar!</p>
          </div>
          </div>
          <!-- Fin paso 2 -->
        </div>
      </div>

      <div v-else class="perfil-error">
        <p>No se pudo cargar el perfil.</p>
        <button class="cta" @click="irAOficiantes">Volver a búsqueda</button>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { profesionalService } from '../services/profesionalService'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const profesional = ref<any>(null)
const cargando = ref(true)
const error = ref('')
const tabActivo = ref<'datos' | 'resenas'>('datos')

const nombreIniciales = computed(() => {
  if (!profesional.value?.usuario?.nombre) return 'P'
  const n = profesional.value.usuario.nombre.trim()
  const parts = n.split(/\s+/)
  if (parts.length >= 2) {
    return (parts[0][0] + parts[1][0]).toUpperCase()
  }
  return (n[0] || 'P').toUpperCase()
})

const zonasCobertura = computed(() => {
  return profesional.value?.zonasCobertura || []
})

// Ubicación sin repetir la provincia si ya está en ciudad (ej. "San Miguel de Tucumán, Capital, Tucumán")
const textoUbicacion = computed(() => {
  const c = profesional.value?.usuario?.ciudad?.trim() || ''
  const p = profesional.value?.usuario?.provincia?.trim() || ''
  if (!c && !p) return ''
  if (!c) return p
  if (!p) return c
  const cLower = c.toLowerCase()
  const pLower = p.toLowerCase()
  if (cLower.endsWith(pLower) || cLower.includes(`, ${pLower}`)) return c
  return `${c}, ${p}`
})

const imagenesTrabajo = computed(() => {
  return profesional.value?.imagenesTrabajo || []
})

const ratingPromedio = computed(() => {
  const v = Number(profesional.value?.calificacionPromedio ?? 0)
  return Math.min(5, Math.max(0, v))
})

const carouselIndex = ref(0)

function urlImagenTrabajo (path: string) {
  if (!path) return ''
  if (path.startsWith('http')) return path
  const base = (import.meta.env.VITE_API_URL || 'http://localhost:5230/api').replace(/\/api\/?$/, '')
  return base + (path.startsWith('/') ? path : '/' + path)
}

function carouselPrev () {
  if (carouselIndex.value > 0) carouselIndex.value--
}
function carouselNext () {
  if (carouselIndex.value < imagenesTrabajo.value.length - 1) carouselIndex.value++
}

const resenas = ref<any[]>([])
const cargandoResenas = ref(false)
const formResena = ref({ autorNombre: '', calificacion: 0, comentario: '' })
const enviandoResena = ref(false)
const errorResena = ref('')

const esPropioPerfil = computed(() => {
  if (!authStore.estaAutenticado || !profesional.value) return false
  const profUserId = profesional.value.usuarioId ?? profesional.value.usuario?.id
  return profUserId != null && authStore.usuario?.id != null && profUserId === authStore.usuario.id
})

const mostrarFormularioResena = computed(() => {
  if (!profesional.value) return false
  return !esPropioPerfil.value
})

const puedeEnviarResena = computed(() => {
  if (enviandoResena.value || !formResena.value.comentario?.trim() || formResena.value.calificacion < 1) return false
  if (!authStore.estaAutenticado && !formResena.value.autorNombre?.trim()) return false
  return true
})

function formatearFechaResena (fecha: string) {
  if (!fecha) return ''
  try {
    const d = new Date(fecha)
    return d.toLocaleDateString('es-AR', { day: 'numeric', month: 'short', year: 'numeric' })
  } catch {
    return fecha
  }
}

async function cargarResenas () {
  const id = Number(route.params.id)
  if (!id || isNaN(id)) return
  cargandoResenas.value = true
  try {
    resenas.value = await profesionalService.listarResenas(id)
  } catch {
    resenas.value = []
  } finally {
    cargandoResenas.value = false
  }
}

async function enviarResena () {
  const id = Number(route.params.id)
  if (!id || formResena.value.calificacion < 1 || !formResena.value.comentario?.trim()) return
  enviandoResena.value = true
  errorResena.value = ''
  try {
    await profesionalService.crearResena(id, {
      autorNombre: formResena.value.autorNombre?.trim() || undefined,
      calificacion: formResena.value.calificacion,
      comentario: formResena.value.comentario.trim()
    })
    formResena.value = { autorNombre: '', calificacion: 0, comentario: '' }
    await cargarResenas()
    if (profesional.value) {
      const prof = await profesionalService.obtenerPorId(id)
      profesional.value.calificacionPromedio = (prof as any).calificacionPromedio
      profesional.value.totalResenas = (prof as any).totalResenas
    }
  } catch (e: any) {
    const msg = e?.response?.data?.message ?? e?.message ?? 'No se pudo enviar la reseña.'
    errorResena.value = msg
  } finally {
    enviandoResena.value = false
  }
}

async function cargarPerfil () {
  const id = Number(route.params.id)
  if (!id || isNaN(id)) {
    error.value = 'ID inválido'
    cargando.value = false
    return
  }
  cargando.value = true
  error.value = ''
  try {
    const data = await profesionalService.obtenerPorId(id)
    if (!data) {
      error.value = 'No se pudo cargar el perfil.'
      profesional.value = null
    } else {
      // Normalizar: si viene formato antiguo (sin usuario, oficios como string[]) lo adaptamos
      const p = data as any
      if (!p.usuario && (p.nombre != null || p.apellido != null)) {
        profesional.value = {
          ...p,
          usuario: {
            nombre: [p.nombre, p.apellido].filter(Boolean).join(' ').trim() || 'Sin nombre',
            email: p.email ?? '',
            telefono: p.telefono,
            ciudad: p.ciudad,
            provincia: p.provincia
          },
          oficios: Array.isArray(p.oficios)
            ? (typeof p.oficios[0] === 'string' ? p.oficios.map((n: string) => ({ nombre: n })) : p.oficios)
            : []
        }
} else {
        profesional.value = p
      }
      carouselIndex.value = 0
      cargarResenas()
    }
  } catch (e: any) {
    error.value = e?.response?.status === 404
      ? 'Profesional no encontrado'
      : (e?.message || e?.response?.data?.message || 'No se pudo cargar el perfil. Intentá de nuevo.')
  } finally {
    cargando.value = false
  }
}

const MENSAJE_WHATSAPP = 'Hola, te contacto desde OficiosYa. Me interesa tu servicio.'

function urlWhatsApp (telefono: string | null | undefined): string {
  if (!telefono || !telefono.trim()) return '#'
  let numero = telefono.replace(/\D/g, '')
  if (!numero.length) return '#'
  if (numero.startsWith('0')) numero = numero.slice(1)
  if (numero.length === 10) numero = '549' + numero
  else if (numero.length === 11 && numero.startsWith('9')) numero = '54' + numero
  else if (!numero.startsWith('54') && numero.length >= 10) numero = '54' + numero
  const texto = encodeURIComponent(MENSAJE_WHATSAPP)
  return `https://wa.me/${numero}?text=${texto}`
}

function contactar () {
  if (!profesional.value?.usuario) return
  const u = profesional.value.usuario
  if (u.telefono) {
    window.open(urlWhatsApp(u.telefono), '_blank', 'noopener')
    return
  }
  if (u.email) {
    window.location.href = `mailto:${u.email}`
    return
  }
  alert('Este profesional no tiene datos de contacto publicados.')
}

function irAInicio () {
  router.push('/')
}
function irAOficiantes () {
  router.push('/oficiantes')
}
function irARegistro () {
  router.push('/registrar-profesional')
}
function irALogin () {
  router.push('/login')
}
function cerrarSesion () {
  authStore.logout()
  router.push('/')
}

onMounted(cargarPerfil)
</script>

<style scoped>
.perfil-profesional {
  min-height: 100vh;
  background: linear-gradient(135deg, #f0fdf4 0%, #ecfdf5 50%, #f0f9ff 100%);
}

.perfil-tabs {
  display: flex;
  gap: 0;
  border-bottom: 2px solid #e2e8f0;
  margin: 0 -32px 24px -32px;
  padding: 0 32px;
}
.perfil-tab {
  padding: 12px 20px;
  font-size: 0.9375rem;
  font-weight: 500;
  color: #64748b;
  background: none;
  border: none;
  border-bottom: 3px solid transparent;
  margin-bottom: -2px;
  cursor: pointer;
  transition: color 0.2s, border-color 0.2s;
}
.perfil-tab:hover {
  color: var(--brand, #22c55e);
}
.perfil-tab.active {
  color: var(--brand, #22c55e);
  border-bottom-color: var(--brand, #22c55e);
}
.perfil-tab-panel {
  min-height: 200px;
}
.perfil-resenas-titulo {
  font-size: 1rem;
  font-weight: 600;
  color: var(--text, #0f172a);
  margin: 0 0 16px 0;
}
.perfil-resenas-panel .perfil-section {
  padding-top: 0;
}

.perfil-main {
  padding: 40px 24px 60px;
}
.perfil-loading,
.perfil-error {
  text-align: center;
  padding: 80px 24px;
  color: var(--muted, #64748b);
}
.perfil-error .cta {
  margin-top: 16px;
}

.perfil-container {
  max-width: 720px;
  margin: 0 auto;
}
.perfil-card {
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0,0,0,0.06);
  overflow: hidden;
  padding: 32px;
}

.perfil-header {
  display: flex;
  gap: 24px;
  flex-wrap: wrap;
  padding-bottom: 24px;
  border-bottom: 1px solid #e2e8f0;
}
.perfil-avatar {
  width: 96px;
  height: 96px;
  border-radius: 50%;
  background: linear-gradient(135deg, var(--brand, #3b82f6), #06b6d4);
  color: white;
  font-size: 2rem;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}
.perfil-header-info {
  flex: 1;
  min-width: 0;
}
.perfil-nombre {
  font-size: 1.75rem;
  font-weight: 700;
  color: var(--text, #0f172a);
  margin: 0 0 8px 0;
}
.perfil-oficios {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
  margin-bottom: 8px;
}
.badge-oficio {
  background: #e0f2fe;
  color: #0369a1;
  padding: 4px 12px;
  border-radius: 999px;
  font-size: 0.875rem;
  font-weight: 500;
}
.perfil-ubicacion {
  font-size: 0.9375rem;
  color: var(--muted, #64748b);
  margin-bottom: 8px;
}
.perfil-meta {
  display: flex;
  align-items: center;
  gap: 16px;
  flex-wrap: wrap;
}
.perfil-rating {
  display: flex;
  align-items: center;
  gap: 2px;
  font-size: 0.9375rem;
}
.perfil-rating .rating-star {
  display: inline-block;
  font-size: 1rem;
  color: transparent;
  -webkit-text-stroke: 1.2px #cbd5e1;
}
.perfil-rating .rating-star.filled {
  color: #f59e0b;
  -webkit-text-stroke: 0;
}
.perfil-rating .rating-star.half {
  position: relative;
}
.perfil-rating .rating-star.half::before {
  content: '★';
  position: absolute;
  left: 0;
  top: 0;
  width: 50%;
  overflow: hidden;
  color: #f59e0b;
  -webkit-text-stroke: 0;
}
.rating-num {
  font-weight: 600;
  color: var(--text, #0f172a);
}
.rating-count {
  color: var(--muted, #64748b);
}
.badge-disponible {
  background: #dcfce7;
  color: #166534;
  padding: 4px 10px;
  border-radius: 6px;
  font-size: 0.8125rem;
  font-weight: 500;
}

.perfil-section {
  padding-top: 24px;
}
.perfil-section h2 {
  font-size: 1.125rem;
  font-weight: 600;
  color: var(--text, #0f172a);
  margin: 0 0 12px 0;
}
.perfil-descripcion {
  color: #475569;
  line-height: 1.6;
  margin: 0;
  white-space: pre-wrap;
}
.lista-servicios,
.lista-zonas {
  margin: 0;
  padding-left: 20px;
  color: #475569;
  line-height: 1.8;
}

.contacto-lista {
  display: flex;
  flex-direction: column;
  gap: 10px;
}
.contacto-item {
  display: inline-flex;
  align-items: center;
  gap: 10px;
  color: var(--text, #0f172a);
  text-decoration: none;
  font-size: 0.9375rem;
}
.contacto-item:hover {
  color: var(--text, #0f172a);
  text-decoration: underline;
}
.contacto-icono {
  font-size: 1.1rem;
}
.contacto-icono-whatsapp {
  display: inline-flex;
  color: #25D366;
}
.contacto-item:hover .contacto-icono-whatsapp {
  color: #20bd5a;
}

/* Reseñas */
.perfil-resenas {
  border-top: 1px solid #e2e8f0;
}
.resena-form {
  background: #f8fafc;
  border-radius: 12px;
  padding: 20px;
  margin-bottom: 24px;
}
.resena-form-titulo {
  font-size: 1rem;
  font-weight: 600;
  color: var(--text, #0f172a);
  margin: 0 0 4px 0;
}
.resena-form-desc {
  font-size: 0.875rem;
  color: #64748b;
  margin: 0 0 12px 0;
}
.resena-form-campo {
  margin-bottom: 12px;
}
.resena-form-campo label {
  display: block;
  font-size: 0.875rem;
  font-weight: 500;
  color: var(--text, #0f172a);
  margin-bottom: 4px;
}
.resena-input {
  width: 100%;
  padding: 10px 12px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 0.9375rem;
  font-family: inherit;
}
.resena-input:focus {
  outline: none;
  border-color: var(--brand, #22c55e);
}
.resena-form-stars {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 12px;
}
.resena-form-label {
  font-size: 0.875rem;
  font-weight: 500;
  color: var(--text, #0f172a);
}
.star-btn {
  background: none;
  border: none;
  font-size: 1.75rem;
  color: #cbd5e1;
  cursor: pointer;
  padding: 0;
  line-height: 1;
  transition: color 0.2s;
}
.star-btn:hover,
.star-btn.active {
  color: #f59e0b;
}
.resena-textarea {
  width: 100%;
  padding: 12px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 0.9375rem;
  font-family: inherit;
  resize: vertical;
  margin-bottom: 12px;
}
.resena-textarea:focus {
  outline: none;
  border-color: var(--brand, #22c55e);
  box-shadow: 0 0 0 2px rgba(34, 197, 94, 0.15);
}
.resena-form-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  flex-wrap: wrap;
}
.resena-chars {
  font-size: 0.8125rem;
  color: #64748b;
}
.resena-error {
  color: #dc2626;
  font-size: 0.875rem;
  margin: 8px 0 0 0;
}
.resena-aviso {
  font-size: 0.9375rem;
  color: #64748b;
  margin: 0 0 16px 0;
}
.resena-aviso a {
  color: var(--brand, #22c55e);
  font-weight: 500;
}
.resenas-loading {
  font-size: 0.9375rem;
  color: #64748b;
  margin-bottom: 16px;
}
.lista-resenas {
  list-style: none;
  margin: 0;
  padding: 0;
}
.resena-item {
  padding: 16px 0;
  border-bottom: 1px solid #e2e8f0;
}
.resena-item:last-child {
  border-bottom: none;
}
.resena-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 4px;
  margin-bottom: 4px;
}
.resena-autor {
  font-weight: 600;
  color: var(--text, #0f172a);
  font-size: 0.9375rem;
}
.resena-fecha {
  font-size: 0.8125rem;
  color: #64748b;
}
.resena-stars {
  margin-bottom: 6px;
  display: flex;
  gap: 2px;
}
.resena-star {
  font-size: 1rem;
  color: transparent;
  -webkit-text-stroke: 1.2px #cbd5e1;
}
.resena-star.filled {
  color: #f59e0b;
  -webkit-text-stroke: 0;
}
.resena-comentario {
  margin: 0;
  font-size: 0.9375rem;
  color: #475569;
  line-height: 1.5;
}
.resenas-vacio {
  font-size: 0.9375rem;
  color: #64748b;
  margin: 0;
}

.perfil-actions {
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
  margin-top: 32px;
  padding-top: 24px;
  border-top: 1px solid #e2e8f0;
}
.cta-contactar {
  background: var(--brand, #3b82f6);
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
}
.cta-contactar:hover {
  filter: brightness(1.05);
}
.cta-outline {
  background: transparent;
  color: var(--brand, #3b82f6);
  border: 2px solid var(--brand, #3b82f6);
  padding: 10px 20px;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
}
.cta-outline:hover {
  background: rgba(59, 130, 246, 0.08);
}

/* Carrusel trabajos realizados */
.carousel-trabajos {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-top: 12px;
}
.carousel-btn {
  flex-shrink: 0;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  border: 2px solid var(--brand, #3b82f6);
  background: white;
  color: var(--brand, #3b82f6);
  font-size: 1.5rem;
  cursor: pointer;
  line-height: 1;
  padding: 0;
}
.carousel-btn:hover:not(:disabled) {
  background: var(--brand, #3b82f6);
  color: white;
}
.carousel-btn:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}
.carousel-inner {
  flex: 1;
  min-width: 0;
  position: relative;
  border-radius: 12px;
  overflow: hidden;
  background: #f1f5f9;
  aspect-ratio: 16/10;
}
.carousel-slide {
  display: none;
  width: 100%;
  height: 100%;
}
.carousel-slide.active {
  display: block;
}
.carousel-img {
  width: 100%;
  height: 100%;
  object-fit: contain;
}
.carousel-caption {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  margin: 0;
  padding: 12px;
  background: rgba(0,0,0,0.6);
  color: white;
  font-size: 0.875rem;
}
.carousel-dots {
  display: flex;
  justify-content: center;
  gap: 8px;
  margin-top: 12px;
}
.carousel-dot {
  width: 10px;
  height: 10px;
  border-radius: 50%;
  border: none;
  background: #cbd5e1;
  cursor: pointer;
  padding: 0;
}
.carousel-dot.active {
  background: var(--brand, #3b82f6);
  transform: scale(1.2);
}

@media (max-width: 600px) {
  .perfil-header {
    flex-direction: column;
    align-items: center;
    text-align: center;
  }
  .perfil-oficios,
  .perfil-meta {
    justify-content: center;
  }
  .navlinks {
    display: none;
  }
}
</style>
