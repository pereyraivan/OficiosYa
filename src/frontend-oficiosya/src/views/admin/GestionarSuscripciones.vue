<template>
  <div class="container mt-4 mb-5">
    <div class="d-flex flex-wrap justify-content-between align-items-center gap-2 mb-4">
      <div>
        <router-link to="/admin" class="text-muted small d-inline-block mb-1">← Volver al panel admin</router-link>
        <h2 class="mb-0">Pagos pendientes</h2>
        <p class="text-muted mb-0">
          Confirmá cuando veas la transferencia del profesional (el DNI/CUIT aparece en el comprobante).
        </p>
      </div>
      <button type="button" class="btn btn-outline-secondary" :disabled="cargando" @click="cargar">
        {{ cargando ? 'Actualizando…' : 'Actualizar' }}
      </button>
    </div>

    <div class="mb-3">
      <label for="buscar-documento" class="form-label mb-1">Buscar por DNI / CUIT</label>
      <input
        id="buscar-documento"
        v-model="filtroDocumento"
        type="search"
        class="form-control"
        style="max-width: 320px;"
        placeholder="Ej: 30123456"
        autocomplete="off"
      />
    </div>

    <div v-if="cargando" class="text-center py-5">
      <div class="spinner-border" role="status"><span class="visually-hidden">Cargando</span></div>
    </div>

    <div v-else-if="error" class="alert alert-danger">{{ error }}</div>

    <div v-else-if="pendientes.length === 0" class="alert alert-info">
      No hay suscripciones pendientes de pago.
    </div>

    <div v-else-if="pendientesFiltrados.length === 0" class="alert alert-warning">
      No hay resultados para “{{ filtroDocumento }}”.
    </div>

    <div v-else class="table-responsive">
      <table class="table table-hover align-middle">
        <thead>
          <tr>
            <th>Fecha</th>
            <th>Profesional</th>
            <th>DNI / CUIT</th>
            <th>Plan</th>
            <th>Monto</th>
            <th>Duración</th>
            <th class="text-end">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="s in pendientesFiltrados" :key="s.id">
            <td>{{ formatFecha(s.fechaRegistro) }}</td>
            <td>
              <strong>{{ s.profesionalNombre || '—' }}</strong>
              <div class="small text-muted">{{ s.profesionalEmail }}</div>
              <div v-if="s.profesionalTelefono" class="small text-muted">{{ s.profesionalTelefono }}</div>
            </td>
            <td>
              <code v-if="documentoMostrar(s)" class="codigo-pago">{{ documentoMostrar(s) }}</code>
              <span v-else class="text-danger">Sin documento</span>
            </td>
            <td>{{ s.planNombre }}</td>
            <td><strong>${{ formatMonto(s.precio) }}</strong></td>
            <td>{{ s.mesesDuracion }} mes(es)</td>
            <td class="text-end acciones-cell">
              <button
                type="button"
                class="btn btn-sm btn-success me-1"
                :disabled="procesandoId === s.id"
                @click="confirmar(s)"
              >
                {{ procesandoId === s.id ? '…' : 'Confirmar pago' }}
              </button>
              <button
                type="button"
                class="btn btn-sm btn-outline-danger"
                :disabled="procesandoId === s.id"
                @click="rechazar(s)"
              >
                Rechazar
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { adminService } from '../../services/adminService'
import type { SuscripcionPendienteAdmin } from '../../types'

const pendientes = ref<SuscripcionPendienteAdmin[]>([])
const cargando = ref(false)
const error = ref('')
const procesandoId = ref<number | null>(null)
const filtroDocumento = ref('')

function soloDigitos (v?: string | null): string {
  return (v || '').replace(/\D/g, '')
}

function documentoMostrar (s: SuscripcionPendienteAdmin): string {
  return s.profesionalDocumento || s.codigoPago || ''
}

const pendientesFiltrados = computed(() => {
  const q = soloDigitos(filtroDocumento.value)
  if (!q) return pendientes.value
  return pendientes.value.filter((s) => {
    const doc = soloDigitos(s.profesionalDocumento)
    const codigo = soloDigitos(s.codigoPago)
    return doc.includes(q) || codigo.includes(q)
  })
})

function formatMonto (n: number): string {
  return n.toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
}

function formatFecha (iso: string): string {
  try {
    return new Date(iso).toLocaleString('es-AR', { dateStyle: 'short', timeStyle: 'short' })
  } catch {
    return iso
  }
}

async function cargar () {
  cargando.value = true
  error.value = ''
  try {
    pendientes.value = await adminService.obtenerSuscripcionesPendientes()
  } catch (e: any) {
    error.value = e?.response?.data?.message || e?.message || 'Error al cargar pendientes'
    pendientes.value = []
  } finally {
    cargando.value = false
  }
}

async function confirmar (s: SuscripcionPendienteAdmin) {
  const doc = documentoMostrar(s) || '(sin documento)'
  const ok = window.confirm(
    `¿Confirmás el pago de ${s.profesionalNombre}?\n\n` +
    `Plan: ${s.planNombre}\nMonto: $${formatMonto(s.precio)}\nDNI/CUIT: ${doc}\n\n` +
    'Solo confirmá si viste el crédito en tu cuenta.'
  )
  if (!ok) return

  procesandoId.value = s.id
  try {
    const res = await adminService.confirmarSuscripcion(s.id)
    alert(res.mensaje || 'Suscripción activada.')
    await cargar()
  } catch (e: any) {
    alert(e?.response?.data?.message || e?.message || 'No se pudo confirmar.')
  } finally {
    procesandoId.value = null
  }
}

async function rechazar (s: SuscripcionPendienteAdmin) {
  const ok = window.confirm(`¿Cancelar la solicitud de ${s.profesionalNombre}?`)
  if (!ok) return

  procesandoId.value = s.id
  try {
    await adminService.rechazarSuscripcion(s.id)
    await cargar()
  } catch (e: any) {
    alert(e?.response?.data?.message || e?.message || 'No se pudo rechazar.')
  } finally {
    procesandoId.value = null
  }
}

onMounted(() => { cargar() })
</script>

<style scoped>
.codigo-pago {
  font-size: 0.95rem;
  font-weight: 600;
  padding: 0.25rem 0.5rem;
  background: #ecfdf5;
  color: #047857;
  border-radius: 4px;
}

.acciones-cell {
  min-width: 220px;
}
</style>
