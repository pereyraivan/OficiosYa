<template>
  <Teleport to="body">
    <button
      id="oy-support-fab"
      type="button"
      class="oy-support-fab"
      :aria-label="menuAbierto ? 'Cerrar menú de soporte' : 'Abrir soporte'"
      :style="fabInlineStyle"
      @click.stop.prevent="toggleMenu"
    >
      <span v-if="menuAbierto" class="oy-support-fab-x" aria-hidden="true">×</span>
      <svg v-else viewBox="0 0 32 32" width="30" height="30" fill="#fff" aria-hidden="true">
        <path d="M16.04 4C9.4 4 4 9.37 4 15.98c0 2.12.56 4.18 1.62 6L4 28l6.2-1.62A12 12 0 0 0 16.04 28C22.68 28 28 22.63 28 16.02 28 9.37 22.68 4 16.04 4zm6.96 17.04c-.29.82-1.7 1.5-2.36 1.6-.6.08-1.37.12-2.21-.14-.51-.15-1.16-.32-2-.62-3.52-1.52-5.81-5.07-5.99-5.3-.17-.24-1.42-1.89-1.42-3.6 0-1.72.9-2.56 1.22-2.91.32-.35.7-.44.93-.44h.67c.22 0 .5-.08.79.6.29.7 1 2.44 1.09 2.62.08.17.14.38.03.61-.11.24-.17.38-.34.59-.17.2-.36.45-.51.61-.17.17-.35.35-.15.69.2.35.89 1.47 1.91 2.38 1.32 1.17 2.43 1.54 2.78 1.71.35.17.55.14.75-.09.2-.22.86-1 .99-1.35.14-.35.28-.29.47-.17.2.11 1.25.59 1.46.7.22.11.36.17.41.26.06.09.06.53-.23 1.35z"/>
      </svg>
    </button>

    <div
      v-show="menuAbierto"
      class="oy-support-backdrop"
      aria-hidden="true"
      @click="cerrarMenu"
    />

    <div
      v-show="menuAbierto"
      class="oy-support-panel"
      role="dialog"
      aria-label="Soporte por WhatsApp"
      @click.stop
    >
      <div class="oy-support-panel-header">
        <div class="oy-support-panel-avatar" aria-hidden="true">
          <svg viewBox="0 0 32 32" width="28" height="28" fill="currentColor">
            <path d="M16.04 4C9.4 4 4 9.37 4 15.98c0 2.12.56 4.18 1.62 6L4 28l6.2-1.62A12 12 0 0 0 16.04 28C22.68 28 28 22.63 28 16.02 28 9.37 22.68 4 16.04 4zm6.96 17.04c-.29.82-1.7 1.5-2.36 1.6-.6.08-1.37.12-2.21-.14-.51-.15-1.16-.32-2-.62-3.52-1.52-5.81-5.07-5.99-5.3-.17-.24-1.42-1.89-1.42-3.6 0-1.72.9-2.56 1.22-2.91.32-.35.7-.44.93-.44h.67c.22 0 .5-.08.79.6.29.7 1 2.44 1.09 2.62.08.17.14.38.03.61-.11.24-.17.38-.34.59-.17.2-.36.45-.51.61-.17.17-.35.35-.15.69.2.35.89 1.47 1.91 2.38 1.32 1.17 2.43 1.54 2.78 1.71.35.17.55.14.75-.09.2-.22.86-1 .99-1.35.14-.35.28-.29.47-.17.2.11 1.25.59 1.46.7.22.11.36.17.41.26.06.09.06.53-.23 1.35z"/>
          </svg>
        </div>
        <div class="oy-support-panel-titles">
          <strong>Soporte OficiosYa</strong>
          <span>Te respondemos por WhatsApp</span>
        </div>
        <button type="button" class="oy-support-cerrar" aria-label="Cerrar" @click="cerrarMenu">×</button>
      </div>
      <div class="oy-support-panel-body">
        <p class="oy-support-intro">¿En qué te podemos ayudar?</p>
        <a
          v-for="opcion in opciones"
          :key="opcion.id"
          class="oy-support-opcion"
          :href="urlWhatsApp(opcion.id)"
          target="_blank"
          rel="noopener noreferrer"
          @click="cerrarMenu"
        >
          <span class="oy-support-opcion-icon" aria-hidden="true">{{ opcion.icono }}</span>
          <span class="oy-support-opcion-text">
            <strong>{{ opcion.titulo }}</strong>
            <small>{{ opcion.subtitulo }}</small>
          </span>
        </a>
      </div>
    </div>
  </Teleport>
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref } from 'vue'
import { SOPORTE_WHATSAPP_EVENT } from '../utils/soporteWhatsApp'

const WHATSAPP_NUMERO = '5493816460066'
const menuAbierto = ref(false)

const mensajes: Record<string, string> = {
  consulta: 'Hola OficiosYa, tengo una consulta sobre la plataforma.',
  registro: 'Hola OficiosYa, soy profesional/oficiante y necesito ayuda con registro, planes o pago.',
  problema: 'Hola OficiosYa, tengo un problema y necesito soporte.'
}

const opciones = [
  { id: 'consulta', icono: '💬', titulo: 'Consulta general', subtitulo: 'Dudas sobre la plataforma' },
  { id: 'registro', icono: '🛠️', titulo: 'Soy profesional', subtitulo: 'Registro, planes o pago' },
  { id: 'problema', icono: '⚠️', titulo: 'Tengo un problema', subtitulo: 'Error o ayuda urgente' }
] as const

/** Estilo inline completo: garantiza position:fixed aunque otro CSS intente pisarlo */
const fabInlineStyle = computed(() => ({
  position: 'fixed',
  right: '16px',
  bottom: '20px',
  left: 'auto',
  top: 'auto',
  width: '58px',
  height: '58px',
  minWidth: '58px',
  minHeight: '58px',
  borderRadius: '50%',
  background: menuAbierto.value ? '#128c7e' : '#25d366',
  color: '#fff',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  zIndex: '99999',
  boxShadow: '0 8px 24px rgba(37, 211, 102, 0.55)',
  border: '0',
  margin: '0',
  padding: '0',
  cursor: 'pointer',
  visibility: 'visible',
  opacity: '1',
  pointerEvents: 'auto'
}))

function urlWhatsApp (tipo: string) {
  const texto = encodeURIComponent(mensajes[tipo] ?? mensajes.consulta)
  return `https://wa.me/${WHATSAPP_NUMERO}?text=${texto}`
}

function toggleMenu () {
  menuAbierto.value = !menuAbierto.value
}

function cerrarMenu () {
  menuAbierto.value = false
}

function abrirMenu () {
  menuAbierto.value = true
}

onMounted(() => {
  // Quitar botones estáticos viejos del HTML para no dejar uno sin position:fixed
  document.getElementById('oy-wa-fab-static')?.remove()
  document.getElementById('oy-support-host')?.remove()
  document.querySelectorAll('body > #oy-support-fab:not(.oy-support-fab)').forEach((el) => el.remove())
  window.addEventListener(SOPORTE_WHATSAPP_EVENT, abrirMenu)
})

onUnmounted(() => {
  window.removeEventListener(SOPORTE_WHATSAPP_EVENT, abrirMenu)
})
</script>

<style>
.oy-support-fab {
  position: fixed !important;
  right: 16px !important;
  bottom: 20px !important;
  z-index: 99999 !important;
}

.oy-support-fab-x {
  font-size: 1.75rem;
  line-height: 1;
  font-weight: 300;
  color: #fff;
}

.oy-support-backdrop {
  position: fixed !important;
  inset: 0;
  background: rgba(15, 23, 42, 0.28);
  z-index: 99998;
}

.oy-support-panel {
  position: fixed !important;
  right: 16px;
  bottom: 88px;
  z-index: 99999;
  width: min(300px, calc(100vw - 32px));
  background: #fff;
  border-radius: 16px;
  overflow: hidden;
  box-shadow: 0 12px 40px rgba(15, 23, 42, 0.2);
  border: 1px solid rgba(15, 23, 42, 0.06);
}

.oy-support-panel-header {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 12px 12px 12px 14px;
  background: #075e54;
  color: #fff;
}

.oy-support-panel-avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: #25d366;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.oy-support-panel-titles {
  display: flex;
  flex-direction: column;
  gap: 2px;
  min-width: 0;
  flex: 1;
}

.oy-support-panel-titles strong {
  font-size: 0.95rem;
  font-weight: 700;
}

.oy-support-panel-titles span {
  font-size: 0.75rem;
  opacity: 0.85;
}

.oy-support-cerrar {
  border: none;
  background: transparent;
  color: #fff;
  font-size: 1.5rem;
  line-height: 1;
  cursor: pointer;
  padding: 4px 8px;
}

.oy-support-panel-body {
  padding: 12px;
  display: flex;
  flex-direction: column;
  gap: 8px;
  background: #f0f2f5;
}

.oy-support-intro {
  margin: 0 4px 4px;
  font-size: 0.85rem;
  color: #64748b;
}

.oy-support-opcion {
  display: flex;
  align-items: center;
  gap: 12px;
  width: 100%;
  text-align: left;
  text-decoration: none;
  background: #fff;
  border-radius: 12px;
  padding: 12px;
  color: inherit;
  box-sizing: border-box;
  box-shadow: 0 1px 2px rgba(15, 23, 42, 0.05);
}

.oy-support-opcion-icon {
  font-size: 1.25rem;
  flex-shrink: 0;
}

.oy-support-opcion-text {
  display: flex;
  flex-direction: column;
  gap: 2px;
  min-width: 0;
}

.oy-support-opcion-text strong {
  font-size: 0.9rem;
  color: #0f172a;
}

.oy-support-opcion-text small {
  font-size: 0.75rem;
  color: #64748b;
}

@media (max-width: 480px) {
  .oy-support-panel {
    right: 12px;
    bottom: 84px;
  }
}
</style>
