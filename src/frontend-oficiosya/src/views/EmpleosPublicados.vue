<template>
  <div class="empleos-publicados">
    <section class="section main-section">
      <div class="container">
        <h1 class="titulo">Empleos publicados</h1>
        <p class="subtitulo">
          Encontrá trabajos que necesitan tu oficio o profesión. Cuando tengas un plan activo podrás postularte y enviar tu presupuesto.
        </p>

        <div class="filtro-row">
          <label class="filtro-label">Filtrar por categoría</label>
          <select v-model="categoriaId" @change="buscar" class="select-categoria">
            <option :value="null">Todas las categorías</option>
            <option v-for="c in categorias" :key="c.id" :value="c.id">{{ c.nombre }}</option>
          </select>
          <button type="button" class="btn-buscar" @click="buscar">Buscar</button>
        </div>

        <div v-if="cargando" class="cargando">Cargando empleos...</div>
        <div v-else-if="error" class="error-msg">{{ error }}</div>
        <div v-else-if="trabajos.length === 0" class="sin-resultados">
          No hay empleos publicados con los filtros seleccionados.
        </div>
        <div v-else>
          <div class="grid-trabajos">
            <article
              v-for="t in trabajos"
              :key="t.id"
              class="card-trabajo"
              @click="verDetalle(t.id)"
            >
              <div class="card-trabajo-categorias">
                <span v-for="cat in t.categorias" :key="cat.id" class="badge-cat">{{ cat.nombre }}</span>
              </div>
              <p class="card-trabajo-descripcion">{{ t.descripcionResumen }}</p>
              <div class="card-trabajo-fecha">{{ fechaFormateada(t.fechaCreacion) }}</div>
              <button type="button" class="btn-ver" @click.stop="verDetalle(t.id)">Ver detalle</button>
            </article>
          </div>

          <div v-if="totalPaginas > 1" class="paginacion">
            <button type="button" class="btn-pag" :disabled="page <= 1" @click="irPagina(page - 1)">Anterior</button>
            <span class="pag-info">Página {{ page }} de {{ totalPaginas }}</span>
            <button type="button" class="btn-pag" :disabled="page >= totalPaginas" @click="irPagina(page + 1)">Siguiente</button>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { trabajosService } from '../services/trabajosService'
import { profesionalService } from '../services/profesionalService'
import type { TrabajoListItem } from '../services/trabajosService'

const router = useRouter()
const route = useRoute()

const categorias = ref<Array<{ id: number; nombre: string }>>([])
const categoriaId = ref<number | null>(null)
const trabajos = ref<TrabajoListItem[]>([])
const cargando = ref(false)
const error = ref('')
const page = ref(1)
const pageSize = 20
const totalPaginas = ref(1)

function fechaFormateada (iso: string) {
  try {
    const d = new Date(iso)
    return d.toLocaleDateString('es-AR', { day: 'numeric', month: 'long', year: 'numeric' })
  } catch {
    return iso
  }
}

async function cargarCategorias () {
  try {
    const list = await profesionalService.obtenerCategorias()
    categorias.value = (list || []).map((c: any) => ({ id: c.id, nombre: (c.nombre || '').replace(/^\?+\s*/, '').trim() }))
      .filter((c: any) => c.nombre)
      .sort((a: any, b: any) => a.nombre.localeCompare(b.nombre))
  } catch (e) {
    console.error(e)
    categorias.value = []
  }
}

async function cargarTrabajos () {
  cargando.value = true
  error.value = ''
  try {
    const params: any = { page: page.value, pageSize }
    if (categoriaId.value != null) params.categoriaIds = [categoriaId.value]
    const result = await trabajosService.listar(params)
    trabajos.value = result.items
    totalPaginas.value = result.totalPaginas
  } catch (e: any) {
    error.value = e.response?.data?.message || e.message || 'Error al cargar empleos.'
    trabajos.value = []
  } finally {
    cargando.value = false
  }
}

function buscar () {
  page.value = 1
  cargarTrabajos()
}

function irPagina (p: number) {
  if (p < 1 || p > totalPaginas.value) return
  page.value = p
  cargarTrabajos()
}

function verDetalle (id: number) {
  router.push({ name: 'DetalleEmpleo', params: { id: String(id) } })
}

function irAHome () { router.push('/') }
function irAOficiantes () { router.push('/oficiantes') }
function irAPublicar () { router.push('/publicar-trabajo') }

onMounted(async () => {
  await cargarCategorias()
  const idCat = route.query.categoriaId
  if (idCat) categoriaId.value = Number(idCat)
  await cargarTrabajos()
})

watch(() => route.query.categoriaId, (v) => {
  if (v != null) categoriaId.value = Number(v)
})
</script>

<style scoped>
.empleos-publicados {
  min-height: 100vh;
  background: linear-gradient(135deg, #f0fdf4 0%, #ecfdf5 50%, #f0f9ff 100%);
}

.main-section { padding: 40px 24px 60px; }
.titulo { font-size: 1.75rem; font-weight: 700; color: #0f172a; margin: 0 0 8px 0; }
.subtitulo { color: #64748b; margin: 0 0 24px 0; font-size: 0.9375rem; }

.filtro-row {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 24px;
  flex-wrap: wrap;
}
.filtro-label { font-weight: 500; color: #0f172a; font-size: 14px; }
.select-categoria {
  padding: 10px 14px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 14px;
  min-width: 220px;
}
.btn-buscar {
  padding: 10px 20px;
  background: #22c55e;
  color: white;
  border: none;
  border-radius: 8px;
  font-weight: 500;
  cursor: pointer;
}
.btn-buscar:hover { filter: brightness(1.05); }

.cargando, .error-msg, .sin-resultados {
  padding: 40px 20px;
  text-align: center;
  color: #64748b;
}
.error-msg { color: #dc2626; }

.grid-trabajos {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 20px;
}
.card-trabajo {
  background: white;
  border-radius: 12px;
  padding: 20px;
  border: 1px solid #e2e8f0;
  cursor: pointer;
  transition: box-shadow 0.2s;
}
.card-trabajo:hover {
  box-shadow: 0 4px 12px rgba(0,0,0,0.08);
}
.card-trabajo-categorias {
  display: flex;
  flex-wrap: wrap;
  gap: 6px;
  margin-bottom: 12px;
}
.badge-cat {
  font-size: 0.75rem;
  padding: 4px 10px;
  background: #ecfdf5;
  color: #166534;
  border-radius: 999px;
}
.card-trabajo-descripcion {
  font-size: 0.9375rem;
  color: #475569;
  line-height: 1.5;
  margin: 0 0 12px 0;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
.card-trabajo-fecha {
  font-size: 0.8125rem;
  color: #94a3b8;
  margin-bottom: 12px;
}
.btn-ver {
  padding: 8px 16px;
  background: transparent;
  color: #22c55e;
  border: 1px solid #22c55e;
  border-radius: 6px;
  font-size: 0.875rem;
  font-weight: 500;
  cursor: pointer;
}
.btn-ver:hover { background: #f0fdf4; }

.paginacion {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 16px;
  margin-top: 32px;
}
.btn-pag {
  padding: 8px 16px;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  background: white;
  cursor: pointer;
  font-size: 14px;
}
.btn-pag:hover:not(:disabled) { background: #f1f5f9; }
.btn-pag:disabled { opacity: 0.5; cursor: not-allowed; }
.pag-info { font-size: 14px; color: #64748b; }
</style>
