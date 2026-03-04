<template>
  <div class="panel-profesional">
    <main class="panel-main">
      <div class="container">
        <h1 class="panel-titulo">Mi panel</h1>
        <p class="panel-subtitulo">Gestioná tu perfil, zonas de cobertura, categorías y suscripción.</p>

        <!-- No autenticado -->
        <div v-if="!authStore.estaAutenticado" class="panel-card panel-mensaje">
          <p>Tenés que iniciar sesión para acceder a tu panel.</p>
          <button type="button" class="btn-cta" @click="irALogin">Iniciar sesión</button>
        </div>

        <!-- Autenticado pero no es profesional -->
        <div v-else-if="authStore.estaAutenticado && !authStore.esProfesional" class="panel-card panel-mensaje">
          <p>Esta sección es para profesionales y oficiantes. Si querés publicar tus servicios, registrate como profesional.</p>
          <button type="button" class="btn-cta" @click="irARegistro">Crear cuenta profesional</button>
        </div>

        <!-- Cargando -->
        <div v-else-if="cargando" class="panel-card panel-mensaje">
          <p>Cargando tu perfil...</p>
        </div>

        <!-- Error al cargar -->
        <div v-else-if="error" class="panel-card panel-mensaje panel-error">
          <p>{{ error }}</p>
          <button type="button" class="btn-cta-outline" @click="cargarPerfil">Reintentar</button>
        </div>

        <!-- Contenido del panel -->
        <template v-else-if="miPerfil">
          <div class="panel-tabs">
            <button
              v-for="tab in tabs"
              :key="tab.id"
              type="button"
              class="tab-btn"
              :class="{ active: tabActivo === tab.id }"
              @click="tabActivo = tab.id"
            >
              {{ tab.label }}
            </button>
          </div>

          <div class="panel-card panel-contenido">
            <!-- Pestaña Perfil -->
            <section v-show="tabActivo === 'perfil'" class="tab-content">
              <h2 class="tab-titulo">Editar perfil</h2>
              <form @submit.prevent="guardarPerfil" class="form-panel">
                <div class="form-group">
                  <label for="descripcion">Descripción (lo que ven los clientes)</label>
                  <textarea
                    id="descripcion"
                    v-model="formPerfil.descripcion"
                    rows="4"
                    class="input-textarea"
                    placeholder="Contá brevemente tu experiencia y qué servicios ofrecés."
                  />
                </div>
                <div class="form-group form-group-check">
                  <label>
                    <input type="checkbox" v-model="formPerfil.disponible" />
                    Estoy disponible para recibir trabajos
                  </label>
                </div>
                <div class="form-group">
                  <label>Foto de perfil</label>
                  <div v-if="urlFotoActual" class="foto-actual">
                    <img :src="urlFotoActual" alt="Foto actual" />
                    <button type="button" class="btn-quitar-foto" @click="quitarFoto">Quitar foto</button>
                  </div>
                  <input
                    type="file"
                    accept="image/*"
                    class="input-file"
                    @change="onFotoChange"
                  />
                </div>
                <button type="submit" class="btn-cta" :disabled="guardandoPerfil">
                  {{ guardandoPerfil ? 'Guardando...' : 'Guardar cambios' }}
                </button>
              </form>
              <p class="hint">Para cambiar tu nombre, teléfono o ubicación podés contactar a soporte o usar la opción cuando esté disponible.</p>

              <div v-if="miPerfil?.id" class="link-resenas-box">
                <h3 class="link-resenas-titulo">Link para que te dejen reseñas</h3>
                <p class="link-resenas-desc">Compartí este link con tus clientes (por WhatsApp, email o tarjeta) para que te dejen su opinión en OficiosYa.</p>
                <div class="link-resenas-row">
                  <input
                    type="text"
                    :value="linkDejarResena"
                    readonly
                    class="input-link-resenas"
                  />
                  <button type="button" class="btn-cta-outline btn-copiar" @click="copiarLinkResena">
                    {{ linkCopiado ? 'Copiado' : 'Copiar' }}
                  </button>
                </div>
              </div>
            </section>

            <!-- Pestaña Suscripción -->
            <section v-show="tabActivo === 'suscripcion'" class="tab-content">
              <h2 class="tab-titulo">Tu suscripción</h2>
              <div v-if="suscripcionActiva" class="suscripcion-card activa">
                <div class="suscripcion-tipo">Plan {{ suscripcionActiva.tipo }}</div>
                <div class="suscripcion-detalle">
                  <p><strong>Estado:</strong> {{ suscripcionActiva.estado }}</p>
                  <p><strong>Vencimiento:</strong> {{ formatearFecha(suscripcionActiva.fechaVencimiento) }}</p>
                  <p v-if="limiteContactosMensual < 99999"><strong>Contactos este mes:</strong> {{ contactosUsadosEsteMes }} / {{ limiteContactosMensual }}</p>
                  <p v-else><strong>Contactos:</strong> Ilimitados</p>
                  <p v-if="accesoEmpleos"><strong>Empleos:</strong> Podés postularte a trabajos publicados</p>
                  <p v-else class="hint">Para postularte a empleos necesitás plan Premium.</p>
                </div>
              </div>
              <div v-else class="suscripcion-card inactiva">
                <p><strong>Plan actual: Free</strong></p>
                <p>Contactos este mes: {{ contactosUsadosEsteMes }} / {{ limiteContactosMensual }}</p>
                <p class="small">Para más contactos y poder postularte a empleos, elegí un plan de pago.</p>
              </div>
              <div class="planes-link">
                <router-link to="/planes" class="btn-cta-outline">Ver planes y precios</router-link>
              </div>
            </section>

            <!-- Pestaña Zonas -->
            <section v-show="tabActivo === 'zonas'" class="tab-content">
              <h2 class="tab-titulo">Zonas de cobertura</h2>
              <p class="tab-desc">Seleccioná las localidades donde prestás servicios. Los clientes te encontrarán cuando busquen en esas zonas.</p>
              <MapaZonasCobertura
                v-model="formZonas"
                :show-map="false"
                :max-zonas="maxZonasCobertura"
              />
              <button type="button" class="btn-cta" :disabled="guardandoZonas" @click="guardarZonas">
                {{ guardandoZonas ? 'Guardando...' : 'Guardar zonas' }}
              </button>
            </section>

            <!-- Pestaña Categorías -->
            <section v-show="tabActivo === 'categorias'" class="tab-content">
              <h2 class="tab-titulo">Servicios que ofrecés</h2>
              <p class="tab-desc">Elegí las categorías que correspondan a tu trabajo. Podés seleccionar más de una.</p>
              <div v-if="cargandoCategorias" class="cargando">Cargando categorías...</div>
              <div v-else class="categorias-list">
                <label
                  v-for="cat in categoriasDisponibles"
                  :key="cat.id"
                  class="categoria-item"
                >
                  <input type="checkbox" :value="cat.nombre" v-model="formCategorias" />
                  <span>{{ cat.nombre }}</span>
                </label>
              </div>
              <p v-if="formCategorias.length === 0 && !cargandoCategorias" class="hint">Seleccioná al menos una categoría.</p>
              <button type="button" class="btn-cta" :disabled="guardandoCategorias || formCategorias.length === 0" @click="guardarCategorias">
                {{ guardandoCategorias ? 'Guardando...' : 'Guardar categorías' }}
              </button>
            </section>
          </div>
        </template>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, watch, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import MapaZonasCobertura from '../components/MapaZonasCobertura.vue'
import { useAuthStore } from '../stores/auth'
import { profesionalService } from '../services/profesionalService'
import type { Profesional } from '../types'
import type { ZonaCoberturaDto } from '../types'

const router = useRouter()
const authStore = useAuthStore()

const tabs = [
  { id: 'perfil', label: 'Perfil' },
  { id: 'suscripcion', label: 'Suscripción' },
  { id: 'zonas', label: 'Zonas' },
  { id: 'categorias', label: 'Categorías' }
]

const tabActivo = ref('perfil')
const cargando = ref(true)
const error = ref('')
const miPerfil = ref<(Profesional & { suscripcionActiva?: { tipo: string; fechaVencimiento: string; estado: string } | null }) | null>(null)

const formPerfil = reactive({ descripcion: '', disponible: true, fotoPerfilBase64: null as string | null })
const formZonas = ref<ZonaCoberturaDto[]>([])
const formCategorias = ref<string[]>([])
const guardandoPerfil = ref(false)
const guardandoZonas = ref(false)
const guardandoCategorias = ref(false)
const categoriasDisponibles = ref<Array<{ id: number; nombre: string; tipo?: string }>>([])
const cargandoCategorias = ref(false)

const suscripcionActiva = computed(() => miPerfil.value && (miPerfil.value as any).suscripcionActiva ? (miPerfil.value as any).suscripcionActiva : null)
const limiteContactosMensual = computed(() => (miPerfil.value as any)?.limiteContactosMensual ?? 2)
const contactosUsadosEsteMes = computed(() => (miPerfil.value as any)?.contactosUsadosEsteMes ?? 0)
const accesoEmpleos = computed(() => (miPerfil.value as any)?.accesoEmpleos === true)
const maxZonasCobertura = computed(() => (miPerfil.value as any)?.maxZonasCobertura ?? 99999)

const linkDejarResena = computed(() => {
  const id = miPerfil.value?.id
  if (!id) return ''
  const base = typeof window !== 'undefined' ? window.location.origin : ''
  return `${base}/dejar-resena/${id}`
})

const linkCopiado = ref(false)
function copiarLinkResena () {
  const link = linkDejarResena.value
  if (!link) return
  if (navigator.clipboard?.writeText) {
    navigator.clipboard.writeText(link).then(() => {
      linkCopiado.value = true
      setTimeout(() => { linkCopiado.value = false }, 2000)
    })
  }
}

const API_BASE = (import.meta.env.VITE_API_URL || 'http://localhost:5230/api').replace(/\/api\/?$/, '')

const urlFotoActual = computed(() => {
  const foto = miPerfil.value?.fotoPerfil
  if (!foto) return ''
  if (foto.startsWith('http')) return foto
  return API_BASE + (foto.startsWith('/') ? foto : '/' + foto)
})

function formatearFecha (iso: string) {
  try {
    const d = new Date(iso)
    return d.toLocaleDateString('es-AR', { day: 'numeric', month: 'long', year: 'numeric' })
  } catch {
    return iso
  }
}

async function cargarPerfil () {
  if (!authStore.estaAutenticado || !authStore.esProfesional) {
    cargando.value = false
    return
  }
  cargando.value = true
  error.value = ''
  try {
    const data = await profesionalService.obtenerMiPerfil()
    miPerfil.value = data
    formPerfil.descripcion = data.descripcion ?? ''
    formPerfil.disponible = data.disponible ?? true
    formZonas.value = (data.zonasCobertura || []).map((z: any) => ({
      localidad: z.localidad ?? '',
      provincia: z.provincia ?? '',
      departamento: z.departamento,
      latitud: Number(z.latitud) || 0,
      longitud: Number(z.longitud) || 0,
      codigoPostal: z.codigoPostal
    }))
    formCategorias.value = (data.oficios || []).map((o: any) => (typeof o === 'string' ? o : o.nombre)).filter(Boolean)
  } catch (e: any) {
    error.value = e?.response?.data?.message || e?.message || 'No se pudo cargar tu perfil.'
    miPerfil.value = null
  } finally {
    cargando.value = false
  }
}

async function cargarCategorias () {
  cargandoCategorias.value = true
  try {
    const list = await profesionalService.obtenerCategorias()
    categoriasDisponibles.value = Array.isArray(list) ? list : []
  } catch {
    categoriasDisponibles.value = []
  } finally {
    cargandoCategorias.value = false
  }
}

function onFotoChange (e: Event) {
  const input = e.target as HTMLInputElement
  const file = input.files?.[0]
  if (!file || !file.type.startsWith('image/')) return
  if (file.size > 3 * 1024 * 1024) {
    alert('La imagen no debe superar 3 MB.')
    input.value = ''
    return
  }
  const reader = new FileReader()
  reader.onload = () => {
    formPerfil.fotoPerfilBase64 = (reader.result as string) || null
  }
  reader.readAsDataURL(file)
}

function quitarFoto () {
  formPerfil.fotoPerfilBase64 = null
  // El backend no tiene "quitar foto" en este request; se podría enviar una string vacía si el backend lo soporta
}

async function guardarPerfil () {
  guardandoPerfil.value = true
  try {
    await profesionalService.actualizarMiPerfil({
      descripcion: formPerfil.descripcion.trim() || undefined,
      disponible: formPerfil.disponible,
      fotoPerfilBase64: formPerfil.fotoPerfilBase64 || undefined
    })
    await cargarPerfil()
    alert('Perfil actualizado correctamente.')
  } catch (e: any) {
    alert(e?.response?.data?.message || e?.message || 'Error al guardar.')
  } finally {
    guardandoPerfil.value = false
  }
}

async function guardarZonas () {
  guardandoZonas.value = true
  try {
    await profesionalService.actualizarMiPerfil({
      zonasCobertura: formZonas.value.map(z => ({
        localidad: z.localidad,
        provincia: z.provincia,
        departamento: z.departamento,
        latitud: z.latitud,
        longitud: z.longitud,
        codigoPostal: z.codigoPostal
      }))
    })
    await cargarPerfil()
    alert('Zonas guardadas correctamente.')
  } catch (e: any) {
    alert(e?.response?.data?.message || e?.message || 'Error al guardar zonas.')
  } finally {
    guardandoZonas.value = false
  }
}

async function guardarCategorias () {
  if (formCategorias.value.length === 0) return
  guardandoCategorias.value = true
  try {
    await profesionalService.actualizarMiPerfil({ oficios: formCategorias.value })
    await cargarPerfil()
    alert('Categorías guardadas correctamente.')
  } catch (e: any) {
    alert(e?.response?.data?.message || e?.message || 'Error al guardar categorías.')
  } finally {
    guardandoCategorias.value = false
  }
}

function irALogin () {
  router.push({ name: 'Login', query: { redirect: '/mi-panel' } })
}

function irARegistro () {
  router.push('/registrar-profesional')
}

onMounted(() => {
  if (authStore.estaAutenticado && authStore.esProfesional) {
    cargarPerfil()
    cargarCategorias()
  } else {
    cargando.value = false
  }
})

watch(() => authStore.estaAutenticado, (autenticado) => {
  if (autenticado && authStore.esProfesional && !miPerfil.value)
    cargarPerfil()
})
</script>

<style scoped>
.panel-profesional {
  min-height: 100vh;
  background: linear-gradient(135deg, #f0fdf4 0%, #ecfdf5 50%, #f0f9ff 100%);
}

.panel-main {
  padding: 32px 24px 60px;
}

.container {
  max-width: 720px;
  margin: 0 auto;
}

.panel-titulo {
  font-size: 1.75rem;
  font-weight: 700;
  color: var(--text, #0f172a);
  margin: 0 0 8px 0;
}

.panel-subtitulo {
  color: #64748b;
  margin: 0 0 24px 0;
  font-size: 0.9375rem;
}

.panel-card {
  background: white;
  border-radius: 16px;
  padding: 24px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.06);
  border: 1px solid #e2e8f0;
}

.panel-mensaje {
  text-align: center;
  padding: 48px 24px;
}

.panel-mensaje p {
  margin: 0 0 20px 0;
  color: #475569;
}

.panel-error p { color: #dc2626; }

.panel-tabs {
  display: flex;
  gap: 8px;
  margin-bottom: 16px;
  flex-wrap: wrap;
}

.tab-btn {
  padding: 10px 18px;
  border-radius: 8px;
  border: 1px solid #e2e8f0;
  background: white;
  color: #64748b;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}

.tab-btn:hover {
  border-color: var(--brand);
  color: var(--brand);
}

.tab-btn.active {
  background: var(--brand);
  border-color: var(--brand);
  color: white;
}

.panel-contenido {
  padding: 28px;
}

.tab-content {
  display: block;
}

.tab-titulo {
  font-size: 1.25rem;
  font-weight: 600;
  color: #0f172a;
  margin: 0 0 8px 0;
}

.tab-desc {
  color: #64748b;
  font-size: 0.9375rem;
  margin: 0 0 20px 0;
}

.form-panel {
  display: flex;
  flex-direction: column;
  gap: 20px;
  margin-bottom: 20px;
}

.form-group label {
  display: block;
  font-weight: 600;
  color: #0f172a;
  margin-bottom: 6px;
  font-size: 0.9375rem;
}

.input-textarea {
  width: 100%;
  padding: 12px 14px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 14px;
  resize: vertical;
  min-height: 100px;
}

.input-textarea:focus {
  outline: none;
  border-color: var(--brand);
}

.form-group-check label {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: 500;
  cursor: pointer;
}

.form-group-check input[type="checkbox"] {
  width: 18px;
  height: 18px;
  accent-color: var(--brand);
}

.foto-actual {
  margin-bottom: 10px;
}

.foto-actual img {
  max-width: 120px;
  max-height: 120px;
  object-fit: cover;
  border-radius: 8px;
  display: block;
  margin-bottom: 8px;
}

.btn-quitar-foto {
  font-size: 0.875rem;
  color: #64748b;
  background: none;
  border: none;
  cursor: pointer;
  text-decoration: underline;
}

.input-file {
  font-size: 0.875rem;
}

.btn-cta {
  background: var(--brand);
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  align-self: flex-start;
}

.btn-cta:hover:not(:disabled) {
  filter: brightness(1.05);
}

.btn-cta:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.btn-cta-outline {
  background: transparent;
  color: var(--brand);
  border: 2px solid var(--brand);
  padding: 10px 20px;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
}

.btn-cta-outline:hover:not(:disabled) {
  background: var(--brand);
  color: white;
}

.btn-cta-outline:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.hint, .small {
  font-size: 0.875rem;
  color: #64748b;
  margin-top: 12px;
}

.link-resenas-box {
  margin-top: 28px;
  padding-top: 24px;
  border-top: 1px solid #e2e8f0;
}
.link-resenas-titulo {
  font-size: 1rem;
  font-weight: 600;
  color: #0f172a;
  margin: 0 0 8px 0;
}
.link-resenas-desc {
  font-size: 0.9375rem;
  color: #64748b;
  margin: 0 0 12px 0;
}
.link-resenas-row {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}
.input-link-resenas {
  flex: 1;
  min-width: 200px;
  padding: 10px 14px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 0.875rem;
  background: #f8fafc;
  color: #0f172a;
}
.btn-copiar {
  flex-shrink: 0;
}

.suscripcion-card {
  padding: 20px;
  border-radius: 12px;
  margin-bottom: 20px;
}

.suscripcion-card.activa {
  background: #ecfdf5;
  border: 1px solid #a7f3d0;
}

.suscripcion-card.inactiva {
  background: #f8fafc;
  border: 1px solid #e2e8f0;
}

.suscripcion-tipo {
  font-size: 1.25rem;
  font-weight: 700;
  color: #0f172a;
  margin-bottom: 8px;
}

.suscripcion-detalle p {
  margin: 0 0 4px 0;
  font-size: 0.9375rem;
}

.planes-link {
  margin-top: 16px;
}

.categorias-list {
  display: flex;
  flex-direction: column;
  gap: 10px;
  max-height: 280px;
  overflow-y: auto;
  padding: 12px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  background: #f8fafc;
  margin-bottom: 16px;
}

.categoria-item {
  display: flex;
  align-items: center;
  gap: 10px;
  cursor: pointer;
  font-weight: 400;
  color: #0f172a;
}

.categoria-item input {
  width: 18px;
  height: 18px;
  accent-color: var(--brand);
}

.cargando {
  padding: 16px;
  text-align: center;
  color: #64748b;
}
</style>
