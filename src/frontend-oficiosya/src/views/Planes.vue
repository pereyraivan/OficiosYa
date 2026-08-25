<template>
  <div class="planes-page">
    <main class="main">
      <div class="container">
        <h1 class="titulo">Planes para profesionales y oficiantes</h1>
        <p class="subtitulo">
          Elegí el plan que se adapte a vos. Más contactos y acceso a empleos publicados.
        </p>

        <div v-if="promoLanzamiento?.activa" class="aviso-promo-lanzamiento">
          <p>
            <strong>{{ promoLanzamiento.mensaje }}</strong>
          </p>
          <p class="aviso-promo-detalle">
            Al registrarte como profesional activamos {{ promoLanzamiento.planNombre }} gratis por
            {{ promoLanzamiento.diasGratis }} días. Sin transferencia.
          </p>
          <router-link to="/registrar-profesional" class="btn-promo-registrar">Registrate ahora</router-link>
        </div>

        <div v-if="suscripcionActiva" class="aviso-plan-activo">
          <p>
            <strong>Tu plan {{ suscripcionActiva.tipo }}</strong>
            vence el {{ formatearFechaPlan(suscripcionActiva.fechaVencimiento) }}.
          </p>
        </div>

        <div v-if="suscripcionPendiente" class="aviso-pendiente">
          <p>
            <strong>Tenés un pago pendiente</strong> (Plan {{ suscripcionPendiente.tipo }} ·
            ${{ suscripcionPendiente.precio.toLocaleString('es-AR') }}).
          </p>
          <button type="button" class="btn-ver-pago" @click="verPagoPendiente">
            Ver instrucciones de transferencia
          </button>
        </div>

        <div v-if="cargando" class="cargando">Cargando planes...</div>
        <div v-else-if="planes.length === 0" class="sin-planes">
          No hay planes disponibles en este momento.
        </div>
        <div v-else class="planes-grid">
          <article
            v-for="plan in planes"
            :key="plan.id"
            class="plan-card"
            :class="{
              destacado: normalizarPlanNombre(plan.nombre) === 'pro',
              actual: esPlanActual(plan)
            }"
          >
            <div class="plan-header">
              <h2 class="plan-nombre">{{ plan.nombre }}</h2>
              <span v-if="esPlanActual(plan)" class="plan-badge-actual">Tu plan</span>
              <p class="plan-desc">{{ construirDescripcionPlan(plan) }}</p>
            </div>
            <div class="plan-precios">
              <template v-if="plan.precioMensual === 0">
                <div class="precio principal">Gratis</div>
              </template>
              <template v-else>
                <div class="precio principal">$ {{ formatNum(plan.precioMensual) }} <span class="periodo">/ mes</span></div>
                <div class="precio secundario">
                  $ {{ formatNum(plan.precioTrimestral) }} por 3 meses
                  <span v-if="plan.descuentoTrimestral > 0" class="ahorro">(ahorro {{ plan.descuentoTrimestral }}%)</span>
                </div>
              </template>
            </div>
            <div class="plan-cta">
              <button
                type="button"
                class="btn-plan"
                :disabled="botonDeshabilitado(plan)"
                @click="abrirDuracion(plan)"
              >
                {{ textoBotonPlan(plan) }}
              </button>
            </div>
          </article>
        </div>

        <p v-if="error" class="error-msg">{{ error }}</p>
        <p class="hint-bottom">Free incluye 2 contactos por mes. Básico no permite postularse; Pro incluye hasta 5 postulaciones y Premium es ilimitado.</p>
        <div class="volver">
          <router-link to="/mi-panel" class="btn-link">Volver a mi panel</router-link>
        </div>
      </div>
    </main>

    <!-- Modal elegir duración -->
    <div
      v-if="planEnSeleccion"
      class="modal-duracion-overlay"
      role="dialog"
      aria-modal="true"
      @click.self="cerrarDuracion"
    >
      <div class="modal-duracion-card">
        <h3>Elegir duración — Plan {{ planEnSeleccion.nombre }}</h3>
        <p class="modal-duracion-sub">El pago es por transferencia (CVU/alias). Te mostramos el monto y las cuentas al confirmar.</p>
        <div class="duracion-opciones">
          <button
            v-for="op in opcionesDuracion(planEnSeleccion)"
            :key="op.meses"
            type="button"
            class="duracion-opcion"
            :class="{ selected: mesesSeleccionados === op.meses }"
            @click="mesesSeleccionados = op.meses"
          >
            <span class="duracion-label">{{ op.label }}</span>
            <span class="duracion-precio">${{ op.precio.toLocaleString('es-AR') }}</span>
            <span v-if="op.descuento > 0" class="duracion-ahorro">{{ op.descuento }}% OFF</span>
          </button>
        </div>
        <div class="modal-duracion-acciones">
          <button type="button" class="btn-cancelar" @click="cerrarDuracion">Cancelar</button>
          <button type="button" class="btn-confirmar" :disabled="solicitando" @click="confirmarPlan">
            {{ solicitando ? 'Procesando...' : 'Confirmar y ver datos de pago' }}
          </button>
        </div>
      </div>
    </div>

    <ModalTransferenciaPago
      :abierto="modalTransferenciaAbierto"
      :instrucciones="instruccionesTransferencia"
      :ya-existente="pagoYaExistente"
      @cerrar="cerrarModalTransferencia"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { profesionalService } from '../services/profesionalService'
import ModalTransferenciaPago from '../components/ModalTransferenciaPago.vue'
import type { InstruccionesPagoTransferencia, SuscripcionActiva, SuscripcionPendiente } from '../types'

const router = useRouter()
const authStore = useAuthStore()

const planes = ref<any[]>([])
const cargando = ref(true)
const planActual = ref('free')
const suscripcionActiva = ref<SuscripcionActiva | null>(null)
const suscripcionPendiente = ref<SuscripcionPendiente | null>(null)
const planEnSeleccion = ref<any | null>(null)
const mesesSeleccionados = ref(1)
const solicitando = ref(false)
const error = ref('')
const modalTransferenciaAbierto = ref(false)
const instruccionesTransferencia = ref<InstruccionesPagoTransferencia | null>(null)
const pagoYaExistente = ref(false)
const promoLanzamiento = ref<{
  activa: boolean
  planNombre?: string | null
  diasGratis?: number
  mensaje?: string | null
} | null>(null)

function formatNum (n: number) {
  return typeof n === 'number' ? n.toLocaleString('es-AR') : n
}

function normalizarPlanNombre (nombre: unknown): string {
  return (nombre ?? '')
    .toString()
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .trim()
}

function esPlanActual (plan: any): boolean {
  const n = normalizarPlanNombre(plan?.nombre)
  const actual = normalizarPlanNombre(planActual.value)
  if (n === 'free' && (actual === 'free' || !actual)) return true
  if ((n === 'basica' || n === 'basico') && (actual === 'basica' || actual === 'basico')) return true
  return n === actual
}

function botonDeshabilitado (plan: any): boolean {
  if (normalizarPlanNombre(plan.nombre) === 'free') return true
  if (esPlanActual(plan)) return true
  if (suscripcionPendiente.value) return true
  return false
}

function textoBotonPlan (plan: any): string {
  if (normalizarPlanNombre(plan.nombre) === 'free') return planActual.value === 'Free' || normalizarPlanNombre(planActual.value) === 'free' ? 'Plan actual' : 'Incluido'
  if (esPlanActual(plan)) return 'Plan actual'
  if (suscripcionPendiente.value) return 'Pago pendiente'
  return 'Elegir plan'
}

function construirDescripcionPlan (plan: any): string {
  const base = plan?.descripcion ? String(plan.descripcion).trim() : ''
  const n = normalizarPlanNombre(plan?.nombre)

  const lineas: string[] = []
  if (n === 'free') {
    lineas.push('Contactos: 2 por mes')
    lineas.push('Zonas de cobertura: hasta 1 zona')
  } else if (n === 'basica' || n === 'basico') {
    lineas.push('Contactos: 20 por mes')
    lineas.push('Zonas de cobertura: ilimitadas')
    lineas.push('Postulaciones a empleos: no incluidas')
  } else if (n === 'pro') {
    lineas.push('Contactos: ilimitados')
    lineas.push('Postulaciones a empleos: hasta 5')
    lineas.push('Zonas de cobertura: ilimitadas')
  } else if (n === 'premium') {
    lineas.push('Contactos: ilimitados')
    lineas.push('Zonas de cobertura: ilimitadas')
    lineas.push('Postulaciones a empleos: ilimitadas')
  }

  const bloqueIncluye = [
    lineas.length ? 'Incluye:' : null,
    ...lineas.map(l => `• ${l}`),
    'Importante: si tu suscripción vence, volvé automáticamente a Free.'
  ].filter(Boolean)

  if (!base) return bloqueIncluye.join('\n')
  return `${base}\n\n${bloqueIncluye.join('\n')}`
}

function opcionesDuracion (plan: any) {
  return [
    { meses: 1, label: '1 mes', precio: plan.precioMensual, descuento: 0 },
    { meses: 3, label: '3 meses', precio: plan.precioTrimestral, descuento: plan.descuentoTrimestral || 0 },
    { meses: 6, label: '6 meses', precio: plan.precioSemestral, descuento: plan.descuentoSemestral || 0 },
    { meses: 12, label: '12 meses', precio: plan.precioAnual, descuento: plan.descuentoAnual || 0 }
  ].filter(op => op.precio > 0)
}

function abrirDuracion (plan: any) {
  error.value = ''
  if (!authStore.estaAutenticado) {
    router.push({ path: '/login', query: { redirect: '/planes' } })
    return
  }
  if (!authStore.esProfesional) {
    error.value = 'Necesitás una cuenta de profesional u oficiante para contratar un plan.'
    return
  }
  if (suscripcionPendiente.value) {
    verPagoPendiente()
    return
  }
  if (botonDeshabilitado(plan)) return

  planEnSeleccion.value = plan
  mesesSeleccionados.value = 1
}

function cerrarDuracion () {
  planEnSeleccion.value = null
}

async function confirmarPlan () {
  if (!planEnSeleccion.value) return
  solicitando.value = true
  error.value = ''
  try {
    const result = await profesionalService.solicitarSuscripcion({
      planSeleccionado: planEnSeleccion.value.nombre,
      mesesSuscripcion: mesesSeleccionados.value,
      metodoPago: 'transferencia'
    })
    if (result.instruccionesTransferencia) {
      instruccionesTransferencia.value = result.instruccionesTransferencia
      pagoYaExistente.value = result.yaExistente
      modalTransferenciaAbierto.value = true
      cerrarDuracion()
      await cargarPerfil()
    } else {
      error.value = 'No se pudieron generar las instrucciones de pago. Intentá de nuevo.'
    }
  } catch (e: any) {
    error.value = e?.message || 'Error al solicitar el plan.'
  } finally {
    solicitando.value = false
  }
}

async function verPagoPendiente () {
  if (!suscripcionPendiente.value) return
  try {
    const result = await profesionalService.solicitarSuscripcion({
      planSeleccionado: suscripcionPendiente.value.tipo,
      mesesSuscripcion: 1,
      metodoPago: 'transferencia'
    })
    if (result.instruccionesTransferencia) {
      instruccionesTransferencia.value = result.instruccionesTransferencia
      pagoYaExistente.value = true
      modalTransferenciaAbierto.value = true
    }
  } catch (e: any) {
    error.value = e?.message || 'No se pudieron cargar las instrucciones de pago.'
  }
}

function cerrarModalTransferencia () {
  modalTransferenciaAbierto.value = false
}

function formatearFechaPlan (iso: string) {
  try {
    return new Date(iso).toLocaleDateString('es-AR', { day: 'numeric', month: 'long', year: 'numeric' })
  } catch {
    return iso
  }
}

async function cargarPerfil () {
  if (!authStore.estaAutenticado || !authStore.esProfesional) return
  try {
    const perfil = await profesionalService.obtenerMiPerfil()
    planActual.value = perfil.planActual || 'Free'
    suscripcionActiva.value = perfil.suscripcionActiva ?? null
    suscripcionPendiente.value = perfil.suscripcionPendiente ?? null
  } catch {
    // Sin perfil profesional
  }
}

onMounted(async () => {
  cargando.value = true
  try {
    const [list, promo] = await Promise.all([
      profesionalService.obtenerPlanesSuscripcion(),
      profesionalService.obtenerLanzamientoPromo(),
      cargarPerfil()
    ])
    promoLanzamiento.value = promo
    const order: Record<string, number> = { free: 0, basica: 1, basico: 1, pro: 2, premium: 3 }
    planes.value = (list || []).sort((a: any, b: any) =>
      (order[normalizarPlanNombre(a?.nombre)] ?? 99) - (order[normalizarPlanNombre(b?.nombre)] ?? 99)
    )
  } catch {
    planes.value = []
  } finally {
    cargando.value = false
  }
})
</script>

<style scoped>
.planes-page { min-height: 100vh; background: var(--bg, #f0fdf4); padding: 2rem 0; }
.titulo { font-size: 2rem; margin-bottom: 0.5rem; color: var(--text, #0f172a); }
.subtitulo { color: var(--muted, #64748b); margin-bottom: 1.5rem; }
.aviso-plan-activo {
  background: #ecfdf5;
  border: 1px solid #a7f3d0;
  border-radius: 12px;
  padding: 14px 18px;
  margin-bottom: 1rem;
}
.aviso-plan-activo p { margin: 0; color: #065f46; }
.aviso-promo-lanzamiento {
  background: linear-gradient(135deg, #ecfdf5, #eff6ff);
  border: 1px solid #86efac;
  border-radius: 12px;
  padding: 16px 18px;
  margin-bottom: 1.25rem;
  text-align: center;
}
.aviso-promo-lanzamiento p { margin: 0 0 0.45rem; color: #14532d; }
.aviso-promo-detalle { color: #334155 !important; font-size: 0.95rem; }
.btn-promo-registrar {
  display: inline-block;
  margin-top: 0.4rem;
  padding: 0.6rem 1.2rem;
  background: var(--brand, #22c55e);
  color: #fff;
  font-weight: 700;
  border-radius: 8px;
  text-decoration: none;
}
.aviso-pendiente {
  background: #fffbeb;
  border: 1px solid #fde68a;
  border-radius: 12px;
  padding: 14px 18px;
  margin-bottom: 1.5rem;
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
}
.aviso-pendiente p { margin: 0; color: #92400e; }
.btn-ver-pago {
  padding: 8px 16px;
  border-radius: 8px;
  border: none;
  background: #f59e0b;
  color: white;
  font-weight: 600;
  cursor: pointer;
}
.cargando, .sin-planes { text-align: center; padding: 3rem; color: var(--muted); }
.planes-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(280px, 1fr)); gap: 1.5rem; margin-bottom: 2rem; }
.plan-card {
  background: white; border-radius: 16px; padding: 1.5rem; box-shadow: 0 4px 20px rgba(0,0,0,0.08);
  display: flex; flex-direction: column; border: 2px solid transparent;
}
.plan-card.destacado { border-color: var(--brand, #10b981); }
.plan-card.actual { border-color: #94a3b8; }
.plan-header { margin-bottom: 1rem; position: relative; }
.plan-nombre { font-size: 1.5rem; margin: 0 0 0.5rem 0; color: var(--text); }
.plan-badge-actual {
  display: inline-block;
  font-size: 0.75rem;
  font-weight: 700;
  text-transform: uppercase;
  letter-spacing: 0.04em;
  color: var(--brand, #10b981);
  margin-bottom: 8px;
}
.plan-desc { font-size: 0.9rem; color: var(--muted); margin: 0; line-height: 1.4; white-space: pre-line; }
.plan-precios { margin-bottom: 1.5rem; }
.precio.principal { font-size: 1.75rem; font-weight: 700; color: var(--text); }
.precio.principal .periodo { font-size: 1rem; font-weight: 400; color: var(--muted); }
.precio.secundario { font-size: 0.95rem; color: var(--muted); margin-top: 0.25rem; }
.ahorro { color: var(--brand, #10b981); font-weight: 600; }
.plan-cta { margin-top: auto; }
.btn-plan {
  width: 100%; padding: 12px; border-radius: 8px; border: none; font-weight: 600; cursor: pointer;
  background: var(--brand, #10b981); color: white;
}
.btn-plan:disabled { background: var(--muted, #94a3b8); cursor: default; }
.error-msg { text-align: center; color: #dc2626; margin-bottom: 1rem; }
.hint-bottom { text-align: center; color: var(--muted); font-size: 0.9rem; margin-bottom: 1rem; }
.volver { text-align: center; }
.btn-link { color: var(--brand); text-decoration: none; }
.btn-link:hover { text-decoration: underline; }

.modal-duracion-overlay {
  position: fixed; inset: 0; z-index: 1500;
  background: rgba(15, 23, 42, 0.5);
  display: flex; align-items: center; justify-content: center; padding: 20px;
}
.modal-duracion-card {
  background: white; border-radius: 16px; max-width: 440px; width: 100%;
  padding: 24px; box-shadow: 0 20px 50px rgba(0,0,0,0.15);
}
.modal-duracion-card h3 { margin: 0 0 8px; font-size: 1.2rem; }
.modal-duracion-sub { margin: 0 0 20px; color: #64748b; font-size: 0.9rem; }
.duracion-opciones { display: flex; flex-direction: column; gap: 10px; margin-bottom: 20px; }
.duracion-opcion {
  display: flex; align-items: center; gap: 12px; padding: 12px 14px;
  border: 2px solid #e2e8f0; border-radius: 10px; background: #fff; cursor: pointer; text-align: left;
}
.duracion-opcion.selected { border-color: var(--brand, #10b981); background: #f0fdf4; }
.duracion-label { font-weight: 600; flex: 1; }
.duracion-precio { font-weight: 700; color: var(--text); }
.duracion-ahorro { font-size: 0.75rem; background: var(--brand, #10b981); color: white; padding: 2px 8px; border-radius: 4px; }
.modal-duracion-acciones { display: flex; gap: 10px; }
.btn-cancelar {
  flex: 1; padding: 12px; border-radius: 8px; border: 1px solid #cbd5e1;
  background: white; cursor: pointer; font-weight: 600;
}
.btn-confirmar {
  flex: 2; padding: 12px; border-radius: 8px; border: none;
  background: var(--brand, #10b981); color: white; font-weight: 600; cursor: pointer;
}
.btn-confirmar:disabled { opacity: 0.6; cursor: default; }
</style>
