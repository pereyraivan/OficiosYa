<template>
  <div class="container mt-4 mb-5">
    <div class="d-flex flex-wrap justify-content-between align-items-center gap-2 mb-4">
      <div>
        <router-link to="/admin" class="text-muted small d-inline-block mb-1">← Volver al panel admin</router-link>
        <h2 class="mb-0">Profesionales y oficiantes</h2>
        <p class="text-muted mb-0">Listá, editá o eliminá perfiles cuando alguien necesite soporte.</p>
      </div>
      <button type="button" class="btn btn-outline-secondary" :disabled="cargando" @click="cargar">
        {{ cargando ? 'Actualizando…' : 'Actualizar' }}
      </button>
    </div>

    <div class="filtros mb-3 d-flex flex-wrap gap-2 align-items-center">
      <label class="small text-muted mb-0">Tipo:</label>
      <select v-model="filtroTipo" class="form-select form-select-sm filtro-select" @change="cargar">
        <option value="">Todos</option>
        <option value="Oficiante">Oficiantes</option>
        <option value="Profesional">Profesionales</option>
      </select>
      <label class="form-check-label small text-muted ms-1">
        <input v-model="ocultarInactivos" type="checkbox" class="form-check-input me-1" @change="cargar" />
        Solo activos
      </label>
      <label class="small text-muted mb-0 ms-2">Buscar:</label>
      <input
        v-model="busqueda"
        type="search"
        class="form-control form-control-sm filtro-buscar"
        placeholder="Nombre, email, categoría…"
      />
      <span class="small text-muted ms-auto">{{ filtrados.length }} de {{ profesionales.length }}</span>
    </div>

    <div v-if="cargando" class="text-center py-5">
      <div class="spinner-border" role="status"><span class="visually-hidden">Cargando</span></div>
    </div>

    <div v-else-if="error" class="alert alert-danger">{{ error }}</div>

    <div v-else-if="profesionales.length === 0" class="alert alert-info">
      No hay profesionales u oficiantes registrados.
    </div>

    <div v-else-if="filtrados.length === 0" class="alert alert-info">
      Ningún resultado coincide con la búsqueda.
    </div>

    <div v-else class="table-responsive">
      <table class="table table-hover align-middle">
        <thead>
          <tr>
            <th>Registro</th>
            <th>Nombre</th>
            <th>Tipo</th>
            <th>Categorías</th>
            <th>Plan</th>
            <th>Estado</th>
            <th>Ubicación</th>
            <th class="text-end">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="p in filtrados" :key="p.id" :class="{ 'fila-inactiva': !p.activo }">
            <td class="text-nowrap">{{ formatFecha(p.fechaRegistro) }}</td>
            <td>
              <strong>{{ nombreCompleto(p) }}</strong>
              <div class="small text-muted">{{ p.email }}</div>
              <div v-if="p.telefono" class="small text-muted">{{ p.telefono }}</div>
            </td>
            <td>
              <span
                v-for="t in p.tipos"
                :key="t"
                class="badge-tipo"
                :class="t.toLowerCase() === 'oficiante' ? 'badge-oficiante' : 'badge-profesional'"
              >{{ t }}</span>
              <span v-if="!p.tipos?.length" class="text-muted small">—</span>
            </td>
            <td>
              <span v-if="p.categorias?.length" class="categorias-cell">{{ p.categorias.join(', ') }}</span>
              <span v-else class="text-muted small">—</span>
            </td>
            <td>
              <strong>{{ p.planActual }}</strong>
              <div v-if="p.suscripcionEstado === 'Pendiente'" class="small text-warning">Pago pendiente</div>
            </td>
            <td>
              <span class="estado-pill" :class="p.activo ? (p.disponible ? 'ok' : 'warn') : 'off'">
                {{ etiquetaEstado(p) }}
              </span>
            </td>
            <td class="small">
              <template v-if="p.ciudad || p.provincia">
                {{ [p.ciudad, p.provincia].filter(Boolean).join(', ') }}
              </template>
              <span v-else class="text-muted">—</span>
            </td>
            <td class="text-end text-nowrap">
              <button type="button" class="btn btn-sm btn-outline-primary me-1" @click="abrirEditar(p)">
                Editar
              </button>
              <button
                type="button"
                class="btn btn-sm btn-outline-danger"
                :disabled="!p.activo || eliminandoId === p.id"
                @click="confirmarEliminar(p)"
              >
                {{ eliminandoId === p.id ? '…' : 'Eliminar' }}
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal editar -->
    <div
      v-if="mostrarModal"
      class="modal-overlay"
      role="dialog"
      aria-modal="true"
      @click.self="cerrarModal"
    >
      <div class="modal-card">
        <div class="modal-header">
          <h3 class="modal-title">Editar perfil</h3>
          <button type="button" class="btn-cerrar" aria-label="Cerrar" @click="cerrarModal">×</button>
        </div>
        <form class="modal-body" @submit.prevent="guardar">
          <p class="modal-hint">Modificá los datos en nombre del profesional / oficiante.</p>

          <div class="row g-2">
            <div class="col-md-6">
              <label class="form-label">Nombre *</label>
              <input v-model="form.nombre" type="text" class="form-control" required maxlength="100" />
            </div>
            <div class="col-md-6">
              <label class="form-label">Apellido *</label>
              <input v-model="form.apellido" type="text" class="form-control" required maxlength="100" />
            </div>
            <div class="col-md-6">
              <label class="form-label">Email *</label>
              <input v-model="form.email" type="email" class="form-control" required maxlength="150" />
            </div>
            <div class="col-md-6">
              <label class="form-label">Teléfono</label>
              <input v-model="form.telefono" type="text" class="form-control" maxlength="20" />
            </div>
            <div class="col-md-6">
              <label class="form-label">Ciudad</label>
              <input v-model="form.ciudad" type="text" class="form-control" maxlength="100" />
            </div>
            <div class="col-md-6">
              <label class="form-label">Provincia</label>
              <input v-model="form.provincia" type="text" class="form-control" maxlength="100" />
            </div>
            <div class="col-12">
              <label class="form-label">Descripción</label>
              <textarea v-model="form.descripcion" class="form-control" rows="3" maxlength="500" />
            </div>
            <div class="col-12">
              <label class="form-label">Categorías</label>
              <div v-if="cargandoCategorias" class="small text-muted">Cargando categorías…</div>
              <div v-else class="categorias-checkboxes">
                <label v-for="c in categoriasDisponibles" :key="c.id" class="cat-check">
                  <input
                    type="checkbox"
                    :value="c.nombre"
                    :checked="form.categorias.includes(c.nombre)"
                    @change="toggleCategoria(c.nombre, ($event.target as HTMLInputElement).checked)"
                  />
                  <span>{{ c.nombre }}</span>
                  <span class="cat-tipo">{{ c.tipo }}</span>
                </label>
              </div>
            </div>
            <div class="col-md-6">
              <label class="form-check-label d-flex align-items-center gap-2 mt-2">
                <input v-model="form.disponible" type="checkbox" class="form-check-input" />
                Disponible para contactos
              </label>
            </div>
            <div class="col-md-6">
              <label class="form-check-label d-flex align-items-center gap-2 mt-2">
                <input v-model="form.activo" type="checkbox" class="form-check-input" />
                Perfil activo (visible en búsquedas)
              </label>
            </div>
          </div>

          <p v-if="errorModal" class="text-danger small mt-3 mb-0">{{ errorModal }}</p>

          <div class="modal-acciones">
            <button type="button" class="btn btn-outline-secondary" :disabled="guardando" @click="cerrarModal">
              Cancelar
            </button>
            <button type="submit" class="btn btn-success" :disabled="guardando">
              {{ guardando ? 'Guardando…' : 'Guardar cambios' }}
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, reactive } from 'vue'
import { adminService } from '../../services/adminService'
import type { ProfesionalAdmin, CategoriaServicio } from '../../types'

const profesionales = ref<ProfesionalAdmin[]>([])
const cargando = ref(false)
const error = ref('')
const filtroTipo = ref('')
const busqueda = ref('')
const ocultarInactivos = ref(false)
const eliminandoId = ref<number | null>(null)

const mostrarModal = ref(false)
const editandoId = ref<number | null>(null)
const guardando = ref(false)
const errorModal = ref('')
const categoriasDisponibles = ref<CategoriaServicio[]>([])
const cargandoCategorias = ref(false)

const form = reactive({
  nombre: '',
  apellido: '',
  email: '',
  telefono: '',
  ciudad: '',
  provincia: '',
  descripcion: '',
  disponible: true,
  activo: true,
  categorias: [] as string[]
})

const filtrados = computed(() => {
  const q = busqueda.value.trim().toLowerCase()
  if (!q) return profesionales.value
  return profesionales.value.filter(p => {
    const haystack = [
      p.nombre,
      p.apellido,
      p.email,
      p.telefono,
      p.ciudad,
      p.provincia,
      p.planActual,
      ...(p.tipos || []),
      ...(p.categorias || [])
    ].filter(Boolean).join(' ').toLowerCase()
    return haystack.includes(q)
  })
})

function nombreCompleto (p: ProfesionalAdmin): string {
  return `${p.nombre || ''} ${p.apellido || ''}`.trim() || '—'
}

function formatFecha (iso: string): string {
  try {
    return new Date(iso).toLocaleDateString('es-AR', { dateStyle: 'short' })
  } catch {
    return iso
  }
}

function etiquetaEstado (p: ProfesionalAdmin): string {
  if (!p.activo) return 'Inactivo'
  if (!p.disponible) return 'No disponible'
  return 'Activo'
}

function toggleCategoria (nombre: string, checked: boolean) {
  if (checked) {
    if (!form.categorias.includes(nombre)) form.categorias.push(nombre)
  } else {
    form.categorias = form.categorias.filter(c => c !== nombre)
  }
}

async function cargarCategorias () {
  cargandoCategorias.value = true
  try {
    categoriasDisponibles.value = await adminService.obtenerCategoriaServicios(undefined, true)
  } catch {
    categoriasDisponibles.value = []
  } finally {
    cargandoCategorias.value = false
  }
}

async function cargar () {
  cargando.value = true
  error.value = ''
  try {
    profesionales.value = await adminService.obtenerProfesionales({
      tipo: (filtroTipo.value as 'Oficiante' | 'Profesional' | '') || undefined,
      soloActivos: ocultarInactivos.value || undefined
    })
  } catch (e: any) {
    error.value = e?.message || 'Error al cargar profesionales'
    profesionales.value = []
  } finally {
    cargando.value = false
  }
}

async function abrirEditar (p: ProfesionalAdmin) {
  errorModal.value = ''
  editandoId.value = p.id
  if (!categoriasDisponibles.value.length) {
    await cargarCategorias()
  }

  try {
    const detalle = await adminService.obtenerProfesional(p.id)
    form.nombre = detalle.nombre || ''
    form.apellido = detalle.apellido || ''
    form.email = detalle.email || ''
    form.telefono = detalle.telefono || ''
    form.ciudad = detalle.ciudad || ''
    form.provincia = detalle.provincia || ''
    form.descripcion = detalle.descripcion || ''
    form.disponible = detalle.disponible
    form.activo = detalle.activo
    form.categorias = [...(detalle.categorias || [])]
    mostrarModal.value = true
  } catch (e: any) {
    alert(e?.message || 'No se pudo cargar el perfil para editar.')
  }
}

function cerrarModal () {
  if (guardando.value) return
  mostrarModal.value = false
  editandoId.value = null
}

async function guardar () {
  if (editandoId.value == null) return
  guardando.value = true
  errorModal.value = ''
  try {
    await adminService.actualizarProfesional(editandoId.value, {
      nombre: form.nombre.trim(),
      apellido: form.apellido.trim(),
      email: form.email.trim(),
      telefono: form.telefono.trim() || null,
      ciudad: form.ciudad.trim() || null,
      provincia: form.provincia.trim() || null,
      descripcion: form.descripcion.trim() || null,
      disponible: form.disponible,
      activo: form.activo,
      categorias: [...form.categorias]
    })
    mostrarModal.value = false
    editandoId.value = null
    await cargar()
  } catch (e: any) {
    errorModal.value = e?.message || 'No se pudo guardar.'
  } finally {
    guardando.value = false
  }
}

async function confirmarEliminar (p: ProfesionalAdmin) {
  const ok = window.confirm(
    `¿Eliminar el perfil de ${nombreCompleto(p)}?\n\n` +
    'Se desactivará y dejará de aparecer en búsquedas. Podés reactivarlo después editando el perfil.'
  )
  if (!ok) return

  eliminandoId.value = p.id
  try {
    const res = await adminService.eliminarProfesional(p.id)
    alert(res.message || 'Perfil eliminado.')
    await cargar()
  } catch (e: any) {
    alert(e?.message || 'No se pudo eliminar.')
  } finally {
    eliminandoId.value = null
  }
}

onMounted(() => {
  cargar()
  cargarCategorias()
})
</script>

<style scoped>
.filtro-select {
  width: auto;
  min-width: 140px;
}

.filtro-buscar {
  width: 220px;
  max-width: 100%;
}

.fila-inactiva {
  opacity: 0.65;
}

.badge-tipo {
  display: inline-block;
  font-size: 0.7rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.03em;
  padding: 0.2rem 0.45rem;
  border-radius: 4px;
  margin-right: 4px;
  margin-bottom: 2px;
}

.badge-oficiante {
  background: #ecfdf5;
  color: #047857;
}

.badge-profesional {
  background: #eff6ff;
  color: #1d4ed8;
}

.categorias-cell {
  font-size: 0.85rem;
  max-width: 200px;
  display: inline-block;
}

.estado-pill {
  display: inline-block;
  font-size: 0.75rem;
  font-weight: 600;
  padding: 0.2rem 0.5rem;
  border-radius: 999px;
}

.estado-pill.ok {
  background: #ecfdf5;
  color: #047857;
}

.estado-pill.warn {
  background: #fffbeb;
  color: #b45309;
}

.estado-pill.off {
  background: #f1f5f9;
  color: #64748b;
}

.modal-overlay {
  position: fixed;
  inset: 0;
  z-index: 2000;
  background: rgba(15, 23, 42, 0.5);
  display: flex;
  align-items: flex-start;
  justify-content: center;
  padding: 24px 16px;
  overflow-y: auto;
}

.modal-card {
  background: #fff;
  border-radius: 14px;
  width: 100%;
  max-width: 640px;
  box-shadow: 0 20px 50px rgba(0, 0, 0, 0.2);
  margin: auto;
}

.modal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 16px 20px;
  border-bottom: 1px solid #e2e8f0;
}

.modal-title {
  margin: 0;
  font-size: 1.15rem;
}

.btn-cerrar {
  border: none;
  background: transparent;
  font-size: 1.5rem;
  line-height: 1;
  cursor: pointer;
  color: #64748b;
  padding: 0 4px;
}

.modal-body {
  padding: 16px 20px 20px;
}

.modal-hint {
  margin: 0 0 16px;
  font-size: 0.9rem;
  color: #64748b;
}

.form-label {
  font-size: 0.85rem;
  font-weight: 600;
  color: #334155;
  margin-bottom: 4px;
}

.categorias-checkboxes {
  max-height: 180px;
  overflow-y: auto;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  padding: 8px 10px;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.cat-check {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 0.9rem;
  cursor: pointer;
  margin: 0;
}

.cat-tipo {
  margin-left: auto;
  font-size: 0.7rem;
  color: #94a3b8;
  text-transform: uppercase;
}

.modal-acciones {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
  padding-top: 16px;
  border-top: 1px solid #e2e8f0;
}
</style>
