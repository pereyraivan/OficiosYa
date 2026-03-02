<template>
  <div class="publicar-trabajo">
    <section class="section formulario-section">
      <div class="container container-form">
        <div class="card card-form">
          <h1 class="titulo">Publicar empleo</h1>
          <p class="subtitulo">
            Contanos qué necesitás. Los oficiantes y profesionales podrán postularse y enviarte su presupuesto.
          </p>

          <form @submit.prevent="enviar" class="form-publicar">
            <div class="form-group">
              <label>Tipo de servicio que necesitás *</label>
              <p class="label-hint">Podés seleccionar más de uno (ej. plomero, albañil, electricista)</p>
              <div v-if="cargandoCategorias" class="cargando">Cargando categorías...</div>
              <div v-else>
                <input
                  v-model="busquedaServicios"
                  type="text"
                  class="input input-buscador"
                  placeholder="Buscar servicio (ej. electricista, plomero...)"
                />
                <div class="categorias-grid">
                  <div v-for="cat in categoriasFiltradas" :key="cat.id" class="categoria-check">
                    <label class="categoria-label">
                      <input
                        type="checkbox"
                        :value="cat.id"
                        v-model="form.categoriaIds"
                      />
                      <span class="categoria-nombre">{{ textoCategoria(cat) }}</span>
                    </label>
                  </div>
                </div>
                <p v-if="categoriasFiltradas.length === 0" class="sin-resultados">No hay servicios que coincidan con la búsqueda.</p>
              </div>
              <span v-if="errores.categoriaIds" class="error">{{ errores.categoriaIds }}</span>
            </div>

            <div class="form-group">
              <label for="descripcion">Descripción del empleo *</label>
              <textarea
                id="descripcion"
                v-model="form.descripcion"
                rows="5"
                class="input-textarea"
                :class="{ error: errores.descripcion }"
                placeholder="Ej: Realizar divisorio de durlock de 4 x 4 m con placas antihumedad y pintura. Incluir estructura metálica.&#10;&#10;O para albañil: Construir tapia de block de 5 x 2 metros, con cimientos y revoque."
              />
              <span v-if="errores.descripcion" class="error">{{ errores.descripcion }}</span>
            </div>

            <div class="form-row">
              <div class="form-group">
                <label for="telefono">Teléfono de contacto</label>
                <input
                  id="telefono"
                  v-model="form.telefono"
                  type="tel"
                  class="input"
                  placeholder="+54 9 381 123-4567"
                />
              </div>
              <div class="form-group">
                <label for="email">Email de contacto *</label>
                <input
                  id="email"
                  v-model="form.email"
                  type="email"
                  required
                  class="input"
                  :class="{ error: errores.email }"
                  placeholder="tu@email.com"
                />
                <span v-if="errores.email" class="error">{{ errores.email }}</span>
              </div>
            </div>

            <div v-if="errores.general" class="error error-general">{{ errores.general }}</div>

            <button type="submit" class="btn-submit" :disabled="enviando">
              {{ enviando ? 'Publicando...' : 'Publicar empleo' }}
            </button>
          </form>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { profesionalService } from '../services/profesionalService'
import { trabajosService } from '../services/trabajosService'

const router = useRouter()
const categorias = ref<Array<{ id: number; nombre: string; tipo: string; icono?: string }>>([])
const cargandoCategorias = ref(true)
const enviando = ref(false)
const busquedaServicios = ref('')

function textoCategoria (cat: { nombre: string; icono?: string }): string {
  const icono = cat.icono && !/^\?+$/.test(cat.icono) ? cat.icono.trim() : ''
  const nombre = (cat.nombre || '').replace(/^\?+\s*/, '').trim()
  return icono ? `${icono} ${nombre}` : nombre
}

const categoriasFiltradas = computed(() => {
  const q = busquedaServicios.value.trim().toLowerCase()
  if (!q) return categorias.value
  return categorias.value.filter(c => {
    const nombre = (c.nombre || '').replace(/^\?+\s*/, '').toLowerCase()
    return nombre.includes(q)
  })
})

const form = reactive({
  descripcion: '',
  telefono: '',
  email: '',
  categoriaIds: [] as number[]
})

const errores = reactive({
  descripcion: '',
  email: '',
  categoriaIds: '',
  general: ''
})

function limpiarErrores () {
  errores.descripcion = ''
  errores.email = ''
  errores.categoriaIds = ''
  errores.general = ''
}

async function cargarCategorias () {
  cargandoCategorias.value = true
  try {
    const list = await profesionalService.obtenerCategorias()
    categorias.value = (list || []).map((c: any) => {
      const icono = c.icono && !/^\?+$/.test(String(c.icono)) ? c.icono : ''
      return {
        id: c.id,
        nombre: (c.nombre || '').replace(/^\?+\s*/, '').trim(),
        tipo: c.tipo || 'Oficiante',
        icono: icono || undefined
      }
    }).sort((a: any, b: any) => a.tipo.localeCompare(b.tipo) || a.nombre.localeCompare(b.nombre))
  } catch (e) {
    console.error(e)
    categorias.value = []
  } finally {
    cargandoCategorias.value = false
  }
}

function validar (): boolean {
  limpiarErrores()
  let ok = true
  if (!form.descripcion.trim()) {
    errores.descripcion = 'Escribí la descripción del trabajo.'
    ok = false
  }
  if (!form.email.trim()) {
    errores.email = 'El email es obligatorio.'
    ok = false
  }
  if (!form.categoriaIds.length) {
    errores.categoriaIds = 'Seleccioná al menos un tipo de servicio.'
    ok = false
  }
  return ok
}

async function enviar () {
  if (!validar() || enviando.value) return
  enviando.value = true
  errores.general = ''
  try {
    await trabajosService.publicar({
      descripcion: form.descripcion.trim(),
      telefono: form.telefono?.trim() || undefined,
      email: form.email.trim(),
      categoriaIds: form.categoriaIds
    })
    alert('Tu empleo fue publicado. Los oficiantes y profesionales podrán postularse.')
    router.push('/')
  } catch (error: any) {
    const msg = error.response?.data?.message || error.message || 'Error al publicar.'
    errores.general = msg
  } finally {
    enviando.value = false
  }
}

onMounted(() => {
  cargarCategorias()
})
</script>

<style scoped>
.publicar-trabajo {
  min-height: 100vh;
  background: linear-gradient(135deg, #f0fdf4 0%, #ecfdf5 50%, #f0f9ff 100%);
}
.formulario-section {
  padding: 40px 24px 60px;
}
.container-form {
  max-width: 720px;
  margin: 0 auto;
}
.card-form {
  background: white;
  border-radius: 16px;
  padding: 40px;
  box-shadow: 0 4px 20px rgba(0,0,0,0.06);
  border: 1px solid #e2e8f0;
}
.titulo {
  font-size: 1.75rem;
  font-weight: 700;
  color: var(--text, #0f172a);
  margin: 0 0 8px 0;
  text-align: center;
}
.subtitulo {
  text-align: center;
  color: #64748b;
  margin: 0 0 32px 0;
  font-size: 0.9375rem;
}
.form-publicar {
  display: flex;
  flex-direction: column;
  gap: 24px;
}
.form-group label {
  display: block;
  font-weight: 600;
  color: var(--text, #0f172a);
  margin-bottom: 6px;
  font-size: 0.9375rem;
}
.label-hint {
  font-size: 0.8125rem;
  color: #64748b;
  margin: -2px 0 10px 0;
}
.input,
.input-textarea {
  width: 100%;
  padding: 12px 14px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 14px;
  color: var(--text, #0f172a);
}
.input-textarea {
  resize: vertical;
  min-height: 120px;
}
.input:focus,
.input-textarea:focus {
  outline: none;
  border-color: var(--brand, #22c55e);
  box-shadow: 0 0 0 2px rgba(34, 197, 94, 0.2);
}
.input.error,
.input-textarea.error {
  border-color: #dc2626;
}
.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}
@media (max-width: 600px) {
  .form-row {
    grid-template-columns: 1fr;
  }
}
.input-buscador {
  margin-bottom: 12px;
}
.categorias-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  gap: 10px;
  max-height: 280px;
  overflow-y: auto;
  padding: 12px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  background: #f8fafc;
}
.sin-resultados {
  margin: 12px 0 0 0;
  font-size: 0.875rem;
  color: #64748b;
  padding: 12px;
}
.categoria-check {
  margin: 0;
}
.categoria-label {
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
  font-weight: 400;
  color: var(--text, #0f172a);
  font-size: 0.9375rem;
}
.categoria-label input {
  width: 18px;
  height: 18px;
  accent-color: var(--brand, #22c55e);
}
.categoria-nombre {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}
.cargando {
  padding: 20px;
  text-align: center;
  color: #64748b;
}
.error {
  font-size: 0.8125rem;
  color: #dc2626;
  margin-top: 4px;
  display: block;
}
.error-general {
  padding: 12px;
  background: #fef2f2;
  border-radius: 8px;
}
.btn-submit {
  padding: 14px 24px;
  background: var(--brand, #22c55e);
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  margin-top: 8px;
}
.btn-submit:hover:not(:disabled) {
  filter: brightness(1.05);
}
.btn-submit:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}
</style>
