<template>
  <div class="detalle-empleo">
    <main class="main">
      <div v-if="cargando" class="cargando">Cargando...</div>
      <div v-else-if="error" class="error-msg">{{ error }} <button type="button" class="btn-link" @click="irAEmpleos">Volver a empleos</button></div>
      <div v-else-if="trabajo" class="container detalle-card">
        <div class="detalle-header">
          <div class="detalle-categorias">
            <span v-for="cat in trabajo.categorias" :key="cat.id" class="badge-cat">{{ cat.nombre }}</span>
          </div>
          <div class="detalle-fecha">{{ fechaFormateada(trabajo.fechaCreacion) }}</div>
        </div>

        <section class="detalle-section">
          <h2>Descripción del trabajo</h2>
          <p class="detalle-descripcion">{{ trabajo.descripcion }}</p>
        </section>

        <div class="detalle-actions">
          <template v-if="puedePostularEmpleos === true">
            <button type="button" class="btn-postularme" @click="abrirPostulacion">
              Postularme
            </button>
          </template>
          <template v-else-if="authStore.estaAutenticado && puedePostularEmpleos === false">
            <p class="mensaje-plan">Para postularte a empleos necesitás plan <strong>Pro o Premium</strong>.</p>
            <router-link to="/planes" class="btn-postularme btn-planes">Ver planes</router-link>
          </template>
          <template v-else-if="!authStore.estaAutenticado">
            <p class="mensaje-plan">Iniciá sesión como profesional u oficiante para postularte.</p>
            <router-link to="/login" class="btn-postularme btn-planes">Iniciar sesión</router-link>
          </template>
          <button type="button" class="btn-volver" @click="irAEmpleos">Volver a empleos</button>
        </div>
      </div>
    </main>

    <!-- Modal Postularme -->
    <div v-if="mostrarModal" class="modal-overlay" @click.self="cerrarModal">
      <div class="modal-box">
        <div class="modal-header">
          <h3>Postularme al trabajo</h3>
          <button type="button" class="modal-cerrar" @click="cerrarModal" aria-label="Cerrar">×</button>
        </div>
        <p class="modal-intro">Completá los datos y elegí si enviar por WhatsApp o por email al empleador (quien publicó el trabajo).</p>
        <form @submit.prevent="() => {}" class="modal-form">
          <div class="form-group">
            <label for="post-desc">Descripción del trabajo que realizaré *</label>
            <textarea
              id="post-desc"
              v-model="postulacion.descripcion"
              rows="4"
              class="input-textarea"
              placeholder="Ej: Realizaré el divisorio de durlock según lo indicado, con estructura metálica y placas antihumedad. Incluye mano de obra y materiales básicos."
            />
          </div>
          <div class="form-group">
            <label for="post-monto">Monto a cobrar *</label>
            <input
              id="post-monto"
              v-model="postulacion.monto"
              type="text"
              class="input"
              placeholder="Ej: $ 45.000 o A convenir"
            />
          </div>
          <div class="modal-actions">
            <button type="button" class="btn-sec" @click="cerrarModal">Cancelar</button>
            <button type="button" class="btn-enviar btn-wa" @click="enviarPorWhatsApp">Enviar por WhatsApp</button>
            <button type="button" class="btn-enviar btn-mail" @click="enviarPorEmail">Enviar por email</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { trabajosService } from '../services/trabajosService'
import { profesionalService } from '../services/profesionalService'
import type { TrabajoDetalle } from '../services/trabajosService'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const trabajo = ref<TrabajoDetalle | null>(null)
const cargando = ref(true)
const error = ref('')
const mostrarModal = ref(false)
const postulacion = reactive({ descripcion: '', monto: '' })
/** true = puede postular (Pro/Premium), false = no (Free/Básico), null = no cargado o no es profesional */
const puedePostularEmpleos = ref<boolean | null>(null)

const INTRO_OFICIOSYA = 'Te contacto desde la página OficiosYa, me postulo al trabajo que publicaste.'

function fechaFormateada (iso: string) {
  try {
    const d = new Date(iso)
    return d.toLocaleDateString('es-AR', { day: 'numeric', month: 'long', year: 'numeric' })
  } catch {
    return iso
  }
}

function urlWhatsApp (telefono: string | null | undefined, mensaje: string): string {
  if (!telefono || !telefono.trim()) return ''
  let numero = telefono.replace(/\D/g, '')
  if (!numero.length) return ''
  if (numero.startsWith('0')) numero = numero.slice(1)
  if (numero.length === 10) numero = '549' + numero
  else if (numero.length === 11 && numero.startsWith('9')) numero = '54' + numero
  else if (!numero.startsWith('54') && numero.length >= 10) numero = '54' + numero
  return `https://wa.me/${numero}?text=${encodeURIComponent(mensaje)}`
}

function abrirPostulacion () {
  postulacion.descripcion = ''
  postulacion.monto = ''
  mostrarModal.value = true
}

function cerrarModal () {
  mostrarModal.value = false
}

const MSG_SIN_WHATSAPP = 'El empleador no dejó asentado el número de WhatsApp.'
const MSG_SIN_EMAIL = 'El empleador no dejó asentado su email.'

function textoPostulacion (): string {
  const desc = postulacion.descripcion.trim()
  const monto = postulacion.monto.trim()
  const lineas = [
    INTRO_OFICIOSYA,
    '',
    '*Descripción del trabajo que realizaré:*',
    desc || '(Sin descripción)',
    '',
    '*Monto:* ' + (monto || 'A convenir')
  ]
  return lineas.join('\n')
}

function enviarPorWhatsApp () {
  if (!trabajo.value) return
  const desc = postulacion.descripcion.trim()
  const monto = postulacion.monto.trim()
  if (!desc || !monto) {
    alert('Completá la descripción y el monto antes de enviar.')
    return
  }
  const tel = trabajo.value.telefono
  if (!tel || !tel.trim()) {
    alert(MSG_SIN_WHATSAPP)
    return
  }
  const mensaje = textoPostulacion()
  const url = urlWhatsApp(tel, mensaje)
  if (url) {
    window.open(url, '_blank', 'noopener')
    cerrarModal()
  }
}

function enviarPorEmail () {
  if (!trabajo.value) return
  const desc = postulacion.descripcion.trim()
  const monto = postulacion.monto.trim()
  if (!desc || !monto) {
    alert('Completá la descripción y el monto antes de enviar.')
    return
  }
  const email = trabajo.value.email
  if (!email || !email.trim()) {
    alert(MSG_SIN_EMAIL)
    return
  }
  const mensaje = textoPostulacion().replace(/\*/g, '')
  const mailto = `mailto:${email}?subject=Postulación desde OficiosYa&body=${encodeURIComponent(mensaje)}`
  window.location.href = mailto
  cerrarModal()
}

async function cargar () {
  const id = Number(route.params.id)
  if (!id || isNaN(id)) {
    error.value = 'Empleo no válido.'
    cargando.value = false
    return
  }
  cargando.value = true
  error.value = ''
  try {
    trabajo.value = await trabajosService.obtenerPorId(id)
  } catch (e: any) {
    error.value = e.response?.data?.message || e.response?.status === 404 ? 'Empleo no encontrado.' : e.message || 'Error al cargar.'
    trabajo.value = null
  } finally {
    cargando.value = false
  }
  // Si está logueado, verificar si puede postularse (Pro/Premium).
  if (authStore.estaAutenticado) {
    try {
      const r = await profesionalService.yoPuedoPostularEmpleos()
      puedePostularEmpleos.value = r.puedePostularEmpleos
    } catch {
      puedePostularEmpleos.value = false
    }
  } else {
    puedePostularEmpleos.value = false
  }
}

function irAHome () { router.push('/') }
function irAOficiantes () { router.push('/oficiantes') }
function irAEmpleos () { router.push('/empleos') }
function irAPublicar () { router.push('/publicar-trabajo') }

onMounted(cargar)
</script>

<style scoped>
.detalle-empleo {
  min-height: 100vh;
  background: linear-gradient(135deg, #f0fdf4 0%, #ecfdf5 50%, #f0f9ff 100%);
}

.main { padding: 40px 24px 60px; }
.container { max-width: 640px; margin: 0 auto; }
.cargando, .error-msg { text-align: center; padding: 40px 20px; color: #64748b; }
.error-msg { color: #dc2626; }
.btn-link { margin-left: 8px; color: #22c55e; text-decoration: underline; background: none; border: none; cursor: pointer; }

.detalle-card {
  background: white;
  border-radius: 16px;
  padding: 32px;
  border: 1px solid #e2e8f0;
  box-shadow: 0 2px 8px rgba(0,0,0,0.04);
}
.detalle-header { margin-bottom: 24px; }
.detalle-categorias { display: flex; flex-wrap: wrap; gap: 8px; margin-bottom: 8px; }
.badge-cat {
  font-size: 0.8125rem;
  padding: 6px 12px;
  background: #ecfdf5;
  color: #166534;
  border-radius: 999px;
}
.detalle-fecha { font-size: 0.875rem; color: #94a3b8; }
.detalle-section { margin-bottom: 24px; }
.detalle-section h2 { font-size: 1rem; font-weight: 600; color: #0f172a; margin: 0 0 8px 0; }
.detalle-descripcion { font-size: 0.9375rem; color: #475569; line-height: 1.6; margin: 0; white-space: pre-wrap; }
.detalle-actions { display: flex; gap: 12px; margin-top: 32px; flex-wrap: wrap; align-items: center; }
.mensaje-plan { margin: 0; font-size: 0.9375rem; color: #64748b; }
.btn-postularme, .btn-planes {
  padding: 12px 24px;
  background: #22c55e;
  color: white;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  text-decoration: none;
  display: inline-block;
}
.btn-postularme:hover, .btn-planes:hover { filter: brightness(1.05); }
.btn-volver {
  padding: 12px 24px;
  background: white;
  color: #22c55e;
  border: 1px solid #22c55e;
  border-radius: 8px;
  font-weight: 500;
  cursor: pointer;
}
.btn-volver:hover { background: #f0fdf4; }

/* Modal */
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  padding: 20px;
}
.modal-box {
  background: white;
  border-radius: 16px;
  max-width: 480px;
  width: 100%;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.15);
}
.modal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 20px 24px;
  border-bottom: 1px solid #e2e8f0;
}
.modal-header h3 { margin: 0; font-size: 1.25rem; font-weight: 600; color: #0f172a; }
.modal-cerrar {
  width: 32px;
  height: 32px;
  border: none;
  background: #f1f5f9;
  border-radius: 8px;
  font-size: 1.25rem;
  line-height: 1;
  color: #64748b;
  cursor: pointer;
}
.modal-cerrar:hover { background: #e2e8f0; color: #0f172a; }
.modal-intro {
  padding: 16px 24px 0;
  font-size: 0.875rem;
  color: #64748b;
  margin: 0;
  line-height: 1.5;
}
.modal-form { padding: 20px 24px 24px; }
.modal-form .form-group { margin-bottom: 16px; }
.modal-form label { display: block; font-weight: 500; color: #0f172a; margin-bottom: 6px; font-size: 0.9375rem; }
.modal-form .input,
.modal-form .input-textarea {
  width: 100%;
  padding: 10px 14px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 14px;
}
.modal-form .input-textarea { resize: vertical; min-height: 100px; }
.modal-actions {
  display: flex;
  gap: 12px;
  justify-content: flex-end;
  margin-top: 20px;
  flex-wrap: wrap;
}
.btn-sec {
  padding: 10px 20px;
  background: #f1f5f9;
  color: #475569;
  border: none;
  border-radius: 8px;
  font-weight: 500;
  cursor: pointer;
}
.btn-sec:hover { background: #e2e8f0; }
.btn-enviar {
  padding: 10px 20px;
  border: none;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
}
.btn-enviar.btn-wa { background: #25D366; color: white; }
.btn-enviar.btn-wa:hover { filter: brightness(1.05); }
.btn-enviar.btn-mail { background: #0f172a; color: white; }
.btn-enviar.btn-mail:hover { filter: brightness(1.1); }
</style>
