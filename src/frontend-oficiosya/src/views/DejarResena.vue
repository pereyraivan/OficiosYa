<template>
  <div class="dejar-resena">
    <main class="dejar-resena-main">
      <!-- Opción 1: Con ID (link corto) – mostrar formulario para ese profesional -->
      <div v-if="idProfesional" class="container container-form">
        <div v-if="cargando" class="dejar-resena-loading">
          <p>Cargando...</p>
        </div>
        <div v-else-if="errorPerfil" class="dejar-resena-error">
          <p>{{ errorPerfil }}</p>
          <router-link to="/dejar-resena" class="cta">Buscar otro profesional</router-link>
        </div>
        <div v-else-if="profesional" class="card-form">
          <h1 class="titulo-form">Dejale tu opinión a {{ nombreProfesional }}</h1>
          <router-link :to="`/profesional/${profesional.id}`" class="link-perfil">Ver perfil completo</router-link>

          <div v-if="enviado" class="mensaje-exito">
            <p>¡Gracias! Tu reseña fue publicada.</p>
            <router-link :to="`/profesional/${profesional.id}`" class="cta">Ver perfil de {{ nombreProfesional }}</router-link>
          </div>

          <div v-else-if="mostrarFormularioResena" class="resena-form">
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
              rows="4"
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
          <p v-else class="resena-aviso">No podés dejar reseña en tu propio perfil.</p>
        </div>
      </div>

      <!-- Opción 2: Sin ID – landing + búsqueda por nombre -->
      <div v-else class="container container-landing">
        <div class="landing-hero">
          <h1 class="landing-titulo">¿Contrataste a un profesional?</h1>
          <p class="landing-subtitulo">Dejale tu opinión para ayudar a otros. Buscalo por nombre o usá el link que te pasó.</p>
        </div>

        <div class="busqueda-nombre">
          <label for="buscar-nombre" class="label-buscar">Buscar por nombre</label>
          <div class="busqueda-row">
            <input
              id="buscar-nombre"
              v-model="terminoBusqueda"
              type="text"
              class="input-buscar"
              placeholder="Ej: Juan, María García, plomero..."
              @keydown.enter.prevent="buscarPorNombre"
            />
            <button type="button" class="cta cta-buscar" :disabled="buscando" @click="buscarPorNombre">
              {{ buscando ? 'Buscando...' : 'Buscar' }}
            </button>
          </div>
        </div>

        <div v-if="busquedaRealizada" class="resultados-section">
          <div v-if="buscando" class="resultados-loading">Buscando profesionales...</div>
          <div v-else-if="resultados.length === 0" class="resultados-vacio">
            <p>No encontramos profesionales con ese nombre. Probá con otro término o menos palabras.</p>
          </div>
          <div v-else class="resultados-grid">
            <div
              v-for="p in resultados"
              :key="p.id"
              class="resultado-card"
            >
              <div class="resultado-avatar">{{ (p.usuario?.nombre || 'P').charAt(0) }}</div>
              <div class="resultado-info">
                <h3>{{ p.usuario?.nombre || 'Sin nombre' }} {{ p.usuario?.apellido || '' }}</h3>
                <p class="resultado-oficio">{{ p.oficios?.[0]?.nombre || '—' }}</p>
                <p v-if="p.usuario?.ciudad" class="resultado-ubicacion">📍 {{ p.usuario.ciudad }}</p>
              </div>
              <router-link :to="`/dejar-resena/${p.id}`" class="btn-dejar-resena">Dejar reseña</router-link>
            </div>
          </div>
        </div>

        <div class="landing-tip">
          <p><strong>¿Tenés el link que te pasó el profesional?</strong> Entrá directo por ahí; te lleva al formulario para dejar tu opinión.</p>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { profesionalService } from '../services/profesionalService'
import type { Profesional } from '../types'

const route = useRoute()
const authStore = useAuthStore()

const idProfesional = computed(() => {
  const id = route.params.id
  if (id === undefined || id === '') return null
  const n = Number(id)
  return isNaN(n) ? null : n
})

const profesional = ref<Profesional | null>(null)
const cargando = ref(false)
const errorPerfil = ref('')
const enviado = ref(false)

const formResena = ref({ autorNombre: '', calificacion: 0, comentario: '' })
const enviandoResena = ref(false)
const errorResena = ref('')

const terminoBusqueda = ref('')
const resultados = ref<Profesional[]>([])
const buscando = ref(false)
const busquedaRealizada = ref(false)

const nombreProfesional = computed(() => {
  const p = profesional.value
  if (!p?.usuario) return 'este profesional'
  return [p.usuario.nombre, p.usuario.apellido].filter(Boolean).join(' ').trim() || 'este profesional'
})

const esPropioPerfil = computed(() => {
  if (!authStore.estaAutenticado || !profesional.value) return false
  const profUserId = profesional.value.usuarioId ?? profesional.value.usuario?.id
  return profUserId != null && authStore.usuario?.id != null && profUserId === authStore.usuario.id
})

const mostrarFormularioResena = computed(() => !esPropioPerfil.value)

const puedeEnviarResena = computed(() => {
  if (enviandoResena.value || !formResena.value.comentario?.trim() || formResena.value.calificacion < 1) return false
  if (!authStore.estaAutenticado && !formResena.value.autorNombre?.trim()) return false
  return true
})

async function cargarPerfil() {
  const id = idProfesional.value
  if (!id) return
  cargando.value = true
  errorPerfil.value = ''
  profesional.value = null
  enviado.value = false
  try {
    const data = await profesionalService.obtenerPorId(id)
    profesional.value = data as Profesional
  } catch (e: any) {
    errorPerfil.value = e?.response?.status === 404
      ? 'Profesional no encontrado'
      : (e?.message || e?.response?.data?.message || 'No se pudo cargar el perfil.')
  } finally {
    cargando.value = false
  }
}

async function enviarResena() {
  const id = idProfesional.value
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
    enviado.value = true
  } catch (e: any) {
    errorResena.value = e?.response?.data?.message ?? e?.message ?? 'No se pudo enviar la reseña.'
  } finally {
    enviandoResena.value = false
  }
}

async function buscarPorNombre() {
  const termino = terminoBusqueda.value?.trim()
  if (!termino) return
  buscando.value = true
  busquedaRealizada.value = true
  resultados.value = []
  try {
    const data = await profesionalService.buscar({ termino })
    resultados.value = Array.isArray(data) ? data : []
  } catch {
    resultados.value = []
  } finally {
    buscando.value = false
  }
}

watch(idProfesional, (id) => {
  if (id) cargarPerfil()
}, { immediate: true })
</script>

<style scoped>
.dejar-resena {
  min-height: 100vh;
  background: linear-gradient(135deg, #f0fdf4 0%, #ecfdf5 50%, #f0f9ff 100%);
}

.dejar-resena-main {
  padding: 40px 24px 60px;
}

.container-form,
.container-landing {
  max-width: 560px;
  margin: 0 auto;
}

.dejar-resena-loading,
.dejar-resena-error {
  text-align: center;
  padding: 60px 24px;
  color: var(--muted, #64748b);
}

.card-form {
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0,0,0,0.06);
  padding: 32px;
}

.titulo-form {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--text, #0f172a);
  margin: 0 0 8px 0;
}

.link-perfil {
  display: inline-block;
  font-size: 0.9375rem;
  color: var(--brand, #22c55e);
  margin-bottom: 24px;
  text-decoration: none;
}

.link-perfil:hover {
  text-decoration: underline;
}

.mensaje-exito {
  text-align: center;
  padding: 24px 0;
}

.mensaje-exito p {
  font-size: 1.125rem;
  color: var(--text, #0f172a);
  margin: 0 0 16px 0;
}

.resena-aviso {
  color: var(--muted, #64748b);
  margin: 0;
}

/* Formulario reseña (mismo estilo que PerfilProfesional) */
.resena-form { margin-top: 8px; }
.resena-form-campo { margin-bottom: 16px; }
.resena-form-campo label {
  display: block;
  font-size: 0.875rem;
  font-weight: 500;
  color: var(--text, #0f172a);
  margin-bottom: 6px;
}
.resena-input {
  width: 100%;
  padding: 10px 14px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 1rem;
  box-sizing: border-box;
}
.resena-form-stars {
  margin-bottom: 16px;
  display: flex;
  align-items: center;
  gap: 12px;
  flex-wrap: wrap;
}
.resena-form-label { font-size: 0.875rem; font-weight: 500; color: var(--text, #0f172a); }
.star-btn {
  background: none;
  border: none;
  font-size: 1.75rem;
  color: #cbd5e1;
  cursor: pointer;
  padding: 0 4px;
  line-height: 1;
  transition: color 0.15s;
}
.star-btn.active { color: #f59e0b; }
.star-btn:hover { color: #f59e0b; }
.resena-textarea {
  width: 100%;
  padding: 12px 14px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 1rem;
  font-family: inherit;
  resize: vertical;
  margin-bottom: 12px;
  box-sizing: border-box;
}
.resena-form-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
  gap: 12px;
}
.resena-chars { font-size: 0.8125rem; color: var(--muted, #64748b); }
.resena-error { color: #dc2626; font-size: 0.875rem; margin: 8px 0 0 0; }

/* Landing */
.landing-hero {
  text-align: center;
  margin-bottom: 32px;
}
.landing-titulo {
  font-size: 1.75rem;
  font-weight: 700;
  color: var(--text, #0f172a);
  margin: 0 0 12px 0;
}
.landing-subtitulo {
  font-size: 1rem;
  color: var(--muted, #64748b);
  margin: 0;
  max-width: 480px;
  margin-left: auto;
  margin-right: auto;
}

.busqueda-nombre {
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0,0,0,0.06);
  padding: 24px;
  margin-bottom: 24px;
}
.label-buscar {
  display: block;
  font-size: 0.875rem;
  font-weight: 600;
  color: var(--text, #0f172a);
  margin-bottom: 10px;
}
.busqueda-row {
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
}
.input-buscar {
  flex: 1;
  min-width: 200px;
  padding: 12px 16px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 1rem;
}
.cta-buscar { flex-shrink: 0; }

.resultados-section {
  margin-bottom: 24px;
}
.resultados-loading,
.resultados-vacio {
  text-align: center;
  padding: 32px 16px;
  color: var(--muted, #64748b);
  background: white;
  border-radius: 16px;
  box-shadow: 0 4px 20px rgba(0,0,0,0.06);
}
.resultados-vacio p { margin: 0; }
.resultados-grid {
  display: flex;
  flex-direction: column;
  gap: 16px;
}
.resultado-card {
  display: flex;
  align-items: center;
  gap: 16px;
  flex-wrap: wrap;
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.06);
  padding: 20px;
}
.resultado-avatar {
  width: 56px;
  height: 56px;
  border-radius: 50%;
  background: linear-gradient(135deg, var(--brand, #22c55e), #06b6d4);
  color: white;
  font-size: 1.5rem;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}
.resultado-info { flex: 1; min-width: 0; }
.resultado-info h3 {
  font-size: 1.125rem;
  font-weight: 600;
  color: var(--text, #0f172a);
  margin: 0 0 4px 0;
}
.resultado-oficio {
  font-size: 0.9375rem;
  color: var(--muted, #64748b);
  margin: 0 0 4px 0;
}
.resultado-ubicacion {
  font-size: 0.875rem;
  color: var(--muted, #64748b);
  margin: 0;
}
.btn-dejar-resena {
  display: inline-block;
  padding: 10px 20px;
  background: var(--brand, #22c55e);
  color: white;
  border-radius: 8px;
  font-weight: 600;
  text-decoration: none;
  font-size: 0.9375rem;
  white-space: nowrap;
}
.btn-dejar-resena:hover {
  opacity: 0.95;
  color: white;
}

.landing-tip {
  background: #e0f2fe;
  color: #0369a1;
  padding: 16px 20px;
  border-radius: 12px;
  font-size: 0.9375rem;
}
.landing-tip p { margin: 0; }
.landing-tip strong { font-weight: 600; }
</style>
