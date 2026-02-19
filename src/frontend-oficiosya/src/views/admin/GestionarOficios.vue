<template>
  <div class="container mt-4">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h2>Gestión de Oficios</h2>
      <button class="btn btn-primary" @click="abrirModalCrear">
        <i class="bi bi-plus-circle"></i> Nuevo Oficio
      </button>
    </div>

    <!-- Tabla de Oficios -->
    <div class="card">
      <div class="card-body">
        <div v-if="cargando" class="text-center py-4">
          <div class="spinner-border" role="status">
            <span class="visually-hidden">Cargando...</span>
          </div>
        </div>

        <div v-else-if="errores.general" class="alert alert-danger">
          {{ errores.general }}
        </div>

        <table v-else class="table table-hover">
          <thead>
            <tr>
              <th>ID</th>
              <th>Nombre</th>
              <th>Descripción</th>
              <th>Icono</th>
              <th>Color</th>
              <th>Estado</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="oficio in oficios" :key="oficio.id">
              <td>{{ oficio.id }}</td>
              <td>{{ oficio.nombre }}</td>
              <td>{{ oficio.descripcion || '-' }}</td>
              <td>{{ oficio.icono || '-' }}</td>
              <td>
                <span v-if="oficio.color" 
                      :style="`background-color: ${oficio.color}; padding: 4px 8px; border-radius: 4px; color: white;`">
                  {{ oficio.color }}
                </span>
                <span v-else>-</span>
              </td>
              <td>
                <span :class="oficio.activo ? 'badge bg-success' : 'badge bg-secondary'">
                  {{ oficio.activo ? 'Activo' : 'Inactivo' }}
                </span>
              </td>
              <td>
                <button class="btn btn-sm btn-warning me-2" @click="abrirModalEditar(oficio)">
                  <i class="bi bi-pencil"></i> Editar
                </button>
                <button 
                  class="btn btn-sm btn-danger" 
                  @click="confirmarEliminar(oficio)"
                  :disabled="eliminando === oficio.id">
                  <span v-if="eliminando === oficio.id" class="spinner-border spinner-border-sm"></span>
                  <i v-else class="bi bi-trash"></i> Eliminar
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Modal Crear/Editar -->
    <div class="modal fade" :class="{ show: mostrarModal, 'd-block': mostrarModal }" tabindex="-1" v-if="mostrarModal">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{ oficioEditando ? 'Editar Oficio' : 'Nuevo Oficio' }}</h5>
            <button type="button" class="btn-close" @click="cerrarModal"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="guardarOficio">
              <div class="mb-3">
                <label class="form-label">Nombre *</label>
                <input 
                  v-model="formulario.nombre" 
                  type="text" 
                  class="form-control" 
                  required
                  :class="{ 'is-invalid': errores.nombre }">
                <div v-if="errores.nombre" class="invalid-feedback">{{ errores.nombre }}</div>
              </div>

              <div class="mb-3">
                <label class="form-label">Descripción</label>
                <textarea 
                  v-model="formulario.descripcion" 
                  class="form-control" 
                  rows="3"></textarea>
              </div>

              <div class="mb-3">
                <label class="form-label">Icono (emoji)</label>
                <input 
                  v-model="formulario.icono" 
                  type="text" 
                  class="form-control" 
                  placeholder="🔧">
              </div>

              <div class="mb-3">
                <label class="form-label">Color (hex)</label>
                <input 
                  v-model="formulario.color" 
                  type="text" 
                  class="form-control" 
                  placeholder="#3498db">
              </div>

              <div v-if="oficioEditando" class="mb-3">
                <div class="form-check">
                  <input 
                    v-model="formulario.activo" 
                    type="checkbox" 
                    class="form-check-input" 
                    id="activo">
                  <label class="form-check-label" for="activo">
                    Activo
                  </label>
                </div>
              </div>

              <div v-if="errores.guardar" class="alert alert-danger">
                {{ errores.guardar }}
              </div>
            </form>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" @click="cerrarModal">Cancelar</button>
            <button 
              type="button" 
              class="btn btn-primary" 
              @click="guardarOficio"
              :disabled="guardando">
              <span v-if="guardando" class="spinner-border spinner-border-sm"></span>
              {{ guardando ? 'Guardando...' : 'Guardar' }}
            </button>
          </div>
        </div>
      </div>
    </div>
    <div v-if="mostrarModal" class="modal-backdrop fade show"></div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { adminService } from '../../services/adminService'
import type { Oficio, CrearOficioRequest, ActualizarOficioRequest } from '../../types'

const oficios = ref<Oficio[]>([])
const cargando = ref(false)
const guardando = ref(false)
const eliminando = ref<number | null>(null)
const mostrarModal = ref(false)
const oficioEditando = ref<Oficio | null>(null)

const formulario = ref<CrearOficioRequest & { activo?: boolean }>({
  nombre: '',
  descripcion: '',
  icono: '',
  color: '',
  activo: true
})

const errores = ref<Record<string, string>>({})

const cargarOficios = async () => {
  cargando.value = true
  errores.value = {}
  try {
    oficios.value = await adminService.obtenerOficios(false)
  } catch (error: any) {
    errores.value.general = error.message || 'Error al cargar oficios'
  } finally {
    cargando.value = false
  }
}

const abrirModalCrear = () => {
  oficioEditando.value = null
  formulario.value = {
    nombre: '',
    descripcion: '',
    icono: '',
    color: '',
    activo: true
  }
  errores.value = {}
  mostrarModal.value = true
}

const abrirModalEditar = (oficio: Oficio) => {
  oficioEditando.value = oficio
  formulario.value = {
    nombre: oficio.nombre,
    descripcion: oficio.descripcion || '',
    icono: oficio.icono || '',
    color: oficio.color || '',
    activo: oficio.activo
  }
  errores.value = {}
  mostrarModal.value = true
}

const cerrarModal = () => {
  mostrarModal.value = false
  oficioEditando.value = null
  errores.value = {}
}

const guardarOficio = async () => {
  guardando.value = true
  errores.value = {}

  try {
    if (oficioEditando.value) {
      const datos: ActualizarOficioRequest = {
        id: oficioEditando.value.id,
        nombre: formulario.value.nombre,
        descripcion: formulario.value.descripcion,
        icono: formulario.value.icono,
        color: formulario.value.color,
        activo: formulario.value.activo ?? true
      }
      await adminService.actualizarOficio(oficioEditando.value.id, datos)
    } else {
      const datos: CrearOficioRequest = {
        nombre: formulario.value.nombre,
        descripcion: formulario.value.descripcion,
        icono: formulario.value.icono,
        color: formulario.value.color
      }
      await adminService.crearOficio(datos)
    }

    cerrarModal()
    await cargarOficios()
  } catch (error: any) {
    errores.value.guardar = error.message || 'Error al guardar el oficio'
  } finally {
    guardando.value = false
  }
}

const confirmarEliminar = async (oficio: Oficio) => {
  if (!confirm(`¿Estás seguro de eliminar el oficio "${oficio.nombre}"?`)) {
    return
  }

  eliminando.value = oficio.id
  try {
    await adminService.eliminarOficio(oficio.id)
    await cargarOficios()
  } catch (error: any) {
    alert(error.message || 'Error al eliminar el oficio')
  } finally {
    eliminando.value = null
  }
}

onMounted(() => {
  cargarOficios()
})
</script>

<style scoped>
.modal {
  background-color: rgba(0, 0, 0, 0.5);
}
</style>

