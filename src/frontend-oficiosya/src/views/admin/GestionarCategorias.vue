<template>
  <div class="container mt-4">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h2>Gestión de Categorías de Servicio</h2>
      <button class="btn btn-primary" @click="abrirModalCrear">
        <i class="bi bi-plus-circle"></i> Nueva Categoría
      </button>
    </div>

    <!-- Filtros -->
    <div class="card mb-3">
      <div class="card-body">
        <div class="row">
          <div class="col-md-4">
            <label class="form-label">Filtrar por tipo:</label>
            <select v-model="filtroTipo" class="form-select" @change="cargarCategorias">
              <option value="">Todos</option>
              <option value="Oficiante">Oficiantes</option>
              <option value="Profesional">Profesionales</option>
            </select>
          </div>
        </div>
      </div>
    </div>

    <!-- Tabla de Categorías -->
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
              <th>Tipo</th>
              <th>Icono</th>
              <th>Estado</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="categoria in categorias" :key="categoria.id">
              <td>{{ categoria.id }}</td>
              <td>{{ categoria.nombre }}</td>
              <td>{{ categoria.descripcion || '-' }}</td>
              <td>
                <span :class="categoria.tipo === 'Oficiante' ? 'badge bg-info' : 'badge bg-primary'">
                  {{ categoria.tipo }}
                </span>
              </td>
              <td>{{ categoria.icono || '-' }}</td>
              <td>
                <span :class="categoria.activo ? 'badge bg-success' : 'badge bg-secondary'">
                  {{ categoria.activo ? 'Activo' : 'Inactivo' }}
                </span>
              </td>
              <td>
                <button class="btn btn-sm btn-warning me-2" @click="abrirModalEditar(categoria)">
                  <i class="bi bi-pencil"></i> Editar
                </button>
                <button 
                  class="btn btn-sm btn-danger" 
                  @click="confirmarEliminar(categoria)"
                  :disabled="eliminando === categoria.id">
                  <span v-if="eliminando === categoria.id" class="spinner-border spinner-border-sm"></span>
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
            <h5 class="modal-title">{{ categoriaEditando ? 'Editar Categoría' : 'Nueva Categoría' }}</h5>
            <button type="button" class="btn-close" @click="cerrarModal"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="guardarCategoria">
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
                <label class="form-label">Tipo *</label>
                <select 
                  v-model="formulario.tipo" 
                  class="form-select" 
                  required
                  :disabled="!!categoriaEditando">
                  <option value="">Seleccionar...</option>
                  <option value="Oficiante">Oficiante</option>
                  <option value="Profesional">Profesional</option>
                </select>
                <div v-if="errores.tipo" class="invalid-feedback d-block">{{ errores.tipo }}</div>
              </div>

              <div class="mb-3">
                <label class="form-label">Icono (emoji)</label>
                <input 
                  v-model="formulario.icono" 
                  type="text" 
                  class="form-control" 
                  placeholder="🔧">
              </div>

              <div v-if="categoriaEditando" class="mb-3">
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
              @click="guardarCategoria"
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
import type { CategoriaServicio, CrearCategoriaServicioRequest, ActualizarCategoriaServicioRequest } from '../../types'

const categorias = ref<CategoriaServicio[]>([])
const cargando = ref(false)
const guardando = ref(false)
const eliminando = ref<number | null>(null)
const mostrarModal = ref(false)
const categoriaEditando = ref<CategoriaServicio | null>(null)
const filtroTipo = ref<string>('')

const formulario = ref<CrearCategoriaServicioRequest & { activo?: boolean }>({
  nombre: '',
  descripcion: '',
  tipo: '',
  icono: '',
  activo: true
})

const errores = ref<Record<string, string>>({})

const cargarCategorias = async () => {
  cargando.value = true
  errores.value = {}
  try {
    const tipo = filtroTipo.value || undefined
    categorias.value = await adminService.obtenerCategoriaServicios(
      tipo as 'Oficiante' | 'Profesional' | undefined, 
      false
    )
  } catch (error: any) {
    errores.value.general = error.message || 'Error al cargar categorías'
  } finally {
    cargando.value = false
  }
}

const abrirModalCrear = () => {
  categoriaEditando.value = null
  formulario.value = {
    nombre: '',
    descripcion: '',
    tipo: '',
    icono: '',
    activo: true
  }
  errores.value = {}
  mostrarModal.value = true
}

const abrirModalEditar = (categoria: CategoriaServicio) => {
  categoriaEditando.value = categoria
  formulario.value = {
    nombre: categoria.nombre,
    descripcion: categoria.descripcion || '',
    tipo: categoria.tipo,
    icono: categoria.icono || '',
    activo: categoria.activo
  }
  errores.value = {}
  mostrarModal.value = true
}

const cerrarModal = () => {
  mostrarModal.value = false
  categoriaEditando.value = null
  errores.value = {}
}

const guardarCategoria = async () => {
  guardando.value = true
  errores.value = {}

  try {
    if (categoriaEditando.value) {
      const datos: ActualizarCategoriaServicioRequest = {
        id: categoriaEditando.value.id,
        nombre: formulario.value.nombre,
        descripcion: formulario.value.descripcion,
        tipo: formulario.value.tipo as 'Oficiante' | 'Profesional',
        icono: formulario.value.icono,
        activo: formulario.value.activo ?? true
      }
      await adminService.actualizarCategoriaServicio(categoriaEditando.value.id, datos)
    } else {
      const datos: CrearCategoriaServicioRequest = {
        nombre: formulario.value.nombre,
        descripcion: formulario.value.descripcion,
        tipo: formulario.value.tipo as 'Oficiante' | 'Profesional',
        icono: formulario.value.icono
      }
      await adminService.crearCategoriaServicio(datos)
    }

    cerrarModal()
    await cargarCategorias()
  } catch (error: any) {
    errores.value.guardar = error.message || 'Error al guardar la categoría'
  } finally {
    guardando.value = false
  }
}

const confirmarEliminar = async (categoria: CategoriaServicio) => {
  if (!confirm(`¿Estás seguro de eliminar la categoría "${categoria.nombre}"?`)) {
    return
  }

  eliminando.value = categoria.id
  try {
    await adminService.eliminarCategoriaServicio(categoria.id)
    await cargarCategorias()
  } catch (error: any) {
    alert(error.message || 'Error al eliminar la categoría')
  } finally {
    eliminando.value = null
  }
}

onMounted(() => {
  cargarCategorias()
})
</script>

<style scoped>
.modal {
  background-color: rgba(0, 0, 0, 0.5);
}
</style>

