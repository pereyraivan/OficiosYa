<template>
  <div
    v-if="abierto && instrucciones"
    class="modal-pago-overlay"
    role="dialog"
    aria-modal="true"
    aria-labelledby="modal-pago-titulo"
    @click.self="emit('cerrar')"
  >
    <div class="modal-pago-card">
      <h3 id="modal-pago-titulo" class="modal-pago-titulo">Pagá tu suscripción por transferencia</h3>
      <p class="modal-pago-intro">
        {{ textoIntro }}
      </p>
      <div class="modal-pago-bloque">
        <span class="modal-pago-label">Código para el concepto</span>
        <div class="modal-pago-fila">
          <code class="modal-pago-codigo">{{ instrucciones.codigoReferencia }}</code>
          <button type="button" class="btn-modal-copiar" @click="copiar(instrucciones.codigoReferencia)">Copiar</button>
        </div>
        <p class="modal-pago-hint">{{ instrucciones.mensajeConcepto }}</p>
      </div>
      <div class="modal-pago-bloque">
        <span class="modal-pago-label">Monto exacto</span>
        <p class="modal-pago-monto">
          ${{ instrucciones.montoArs.toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) }}
        </p>
        <p v-if="instrucciones.planNombre" class="modal-pago-plan">
          Plan {{ instrucciones.planNombre }} · {{ instrucciones.meses }} {{ instrucciones.meses === 1 ? 'mes' : 'meses' }}
        </p>
      </div>
      <div v-if="instrucciones.cuentas?.length" class="modal-pago-cuentas">
        <span class="modal-pago-label">Podés transferir a cualquiera de estas cuentas</span>
        <div
          v-for="(c, idx) in instrucciones.cuentas"
          :key="idx"
          class="modal-pago-cuenta"
        >
          <strong>{{ c.etiqueta }}</strong>
          <div v-if="c.titular" class="modal-pago-detalle">Titular: {{ c.titular }}</div>
          <div class="modal-pago-fila">
            <code class="modal-pago-cvu">{{ c.cvuAlias }}</code>
            <button type="button" class="btn-modal-copiar" @click="copiar(c.cvuAlias)">Copiar</button>
          </div>
        </div>
      </div>
      <p v-else class="modal-pago-aviso-config">
        El administrador aún no cargó los CVU/alias en el servidor. Guardá el código de referencia; cuando tengas los datos de pago, transferí el monto indicado con ese código en el concepto.
      </p>
      <button type="button" class="btn-modal-ok" @click="emit('cerrar')">{{ textoBoton }}</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import type { InstruccionesPagoTransferencia } from '../types'

const props = withDefaults(defineProps<{
  abierto: boolean
  instrucciones: InstruccionesPagoTransferencia | null
  modoRegistro?: boolean
  yaExistente?: boolean
}>(), {
  modoRegistro: false,
  yaExistente: false
})

const emit = defineEmits<{
  cerrar: []
}>()

const textoIntro = computed(() => {
  if (props.yaExistente) {
    return 'Ya tenés una suscripción pendiente de pago. Usá estos datos para completar la transferencia.'
  }
  if (props.modoRegistro) {
    return 'Tu cuenta ya está creada. La suscripción queda pendiente hasta que acreditemos el pago en nuestras cuentas.'
  }
  return 'Tu solicitud quedó registrada. La suscripción se activa cuando acreditemos el pago en nuestras cuentas.'
})

const textoBoton = computed(() => props.modoRegistro ? 'Entendido, ir al inicio' : 'Entendido')

function copiar (texto: string) {
  if (!texto) return
  navigator.clipboard?.writeText(texto).catch(() => {})
}
</script>

<style scoped>
.modal-pago-overlay {
  position: fixed;
  inset: 0;
  z-index: 2000;
  background: rgba(15, 23, 42, 0.55);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
  overflow-y: auto;
}

.modal-pago-card {
  background: #fff;
  border-radius: 16px;
  max-width: 480px;
  width: 100%;
  padding: 24px;
  box-shadow: 0 20px 50px rgba(0, 0, 0, 0.2);
}

.modal-pago-titulo {
  margin: 0 0 12px;
  font-size: 1.25rem;
  color: var(--text, #0f172a);
}

.modal-pago-intro {
  margin: 0 0 20px;
  font-size: 0.95rem;
  color: #475569;
  line-height: 1.45;
}

.modal-pago-bloque {
  margin-bottom: 18px;
}

.modal-pago-label {
  display: block;
  font-size: 0.8rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.04em;
  color: #64748b;
  margin-bottom: 8px;
}

.modal-pago-fila {
  display: flex;
  align-items: center;
  gap: 10px;
  flex-wrap: wrap;
}

.modal-pago-codigo,
.modal-pago-cvu {
  flex: 1;
  min-width: 0;
  font-size: 0.95rem;
  padding: 10px 12px;
  background: #f1f5f9;
  border-radius: 8px;
  word-break: break-all;
}

.modal-pago-monto {
  margin: 0;
  font-size: 1.35rem;
  font-weight: 700;
  color: var(--brand, #059669);
}

.modal-pago-plan {
  margin: 6px 0 0;
  font-size: 0.9rem;
  color: #64748b;
}

.modal-pago-hint {
  margin: 10px 0 0;
  font-size: 0.85rem;
  color: #64748b;
  line-height: 1.4;
}

.modal-pago-cuentas {
  margin-bottom: 20px;
}

.modal-pago-cuenta {
  margin-top: 14px;
  padding: 12px;
  border: 1px solid #e2e8f0;
  border-radius: 10px;
  background: #fafafa;
}

.modal-pago-detalle {
  font-size: 0.88rem;
  color: #475569;
  margin: 6px 0 8px;
}

.modal-pago-aviso-config {
  font-size: 0.88rem;
  color: #b45309;
  background: #fffbeb;
  border: 1px solid #fde68a;
  border-radius: 8px;
  padding: 12px;
  margin-bottom: 16px;
}

.btn-modal-copiar {
  flex-shrink: 0;
  padding: 8px 14px;
  border-radius: 8px;
  border: 1px solid #cbd5e1;
  background: #fff;
  font-size: 0.85rem;
  cursor: pointer;
}

.btn-modal-copiar:hover {
  border-color: var(--brand, #059669);
  color: var(--brand, #059669);
}

.btn-modal-ok {
  width: 100%;
  margin-top: 8px;
  padding: 14px;
  border: none;
  border-radius: 10px;
  background: var(--brand, #059669);
  color: #fff;
  font-weight: 600;
  font-size: 1rem;
  cursor: pointer;
}

.btn-modal-ok:hover {
  filter: brightness(1.05);
}
</style>
