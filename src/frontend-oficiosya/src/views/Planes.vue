<template>
  <div class="planes-page">
    <main class="main">
      <div class="container">
        <h1 class="titulo">Planes para profesionales y oficiantes</h1>
        <p class="subtitulo">
          Elegí el plan que se adapte a vos. Más contactos y acceso a empleos publicados.
        </p>

        <div v-if="cargando" class="cargando">Cargando planes...</div>
        <div v-else-if="planes.length === 0" class="sin-planes">
          No hay planes disponibles en este momento.
        </div>
        <div v-else class="planes-grid">
          <article
            v-for="plan in planes"
            :key="plan.id"
            class="plan-card"
            :class="{ destacado: plan.nombre === 'Premium' }"
          >
            <div class="plan-header">
              <h2 class="plan-nombre">{{ plan.nombre }}</h2>
              <p v-if="plan.descripcion" class="plan-desc">{{ plan.descripcion }}</p>
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
              <button type="button" class="btn-plan" :disabled="plan.nombre === 'Free'" @click="solicitarPlan(plan)">
                {{ plan.nombre === 'Free' ? 'Plan actual' : 'Contratar (próximamente)' }}
              </button>
            </div>
          </article>
        </div>

        <p class="hint-bottom">El pago estará disponible pronto. Mientras tanto tenés el plan Free con 2 contactos por mes.</p>
        <div class="volver">
          <router-link to="/mi-panel" class="btn-link">Volver a mi panel</router-link>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { profesionalService } from '../services/profesionalService'

const planes = ref<any[]>([])
const cargando = ref(true)

function formatNum (n: number) {
  return typeof n === 'number' ? n.toLocaleString('es-AR') : n
}

function solicitarPlan (plan: any) {
  if (plan.nombre === 'Free') return
  alert('La contratación de planes estará disponible pronto. Contactá a soporte si querés cambiar de plan.')
}

onMounted(async () => {
  cargando.value = true
  try {
    const list = await profesionalService.obtenerPlanesSuscripcion()
    // Ordenar: Free, Basica, Premium
    const order: Record<string, number> = { Free: 0, Basica: 1, Premium: 2 }
    planes.value = (list || []).sort((a: any, b: any) => (order[a.nombre] ?? 99) - (order[b.nombre] ?? 99))
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
.subtitulo { color: var(--muted, #64748b); margin-bottom: 2rem; }
.cargando, .sin-planes { text-align: center; padding: 3rem; color: var(--muted); }
.planes-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(280px, 1fr)); gap: 1.5rem; margin-bottom: 2rem; }
.plan-card {
  background: white; border-radius: 16px; padding: 1.5rem; box-shadow: 0 4px 20px rgba(0,0,0,0.08);
  display: flex; flex-direction: column;
}
.plan-card.destacado { border: 2px solid var(--brand, #10b981); }
.plan-header { margin-bottom: 1rem; }
.plan-nombre { font-size: 1.5rem; margin: 0 0 0.5rem 0; color: var(--text); }
.plan-desc { font-size: 0.9rem; color: var(--muted); margin: 0; line-height: 1.4; }
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
.hint-bottom { text-align: center; color: var(--muted); font-size: 0.9rem; margin-bottom: 1rem; }
.volver { text-align: center; }
.btn-link { color: var(--brand); text-decoration: none; }
.btn-link:hover { text-decoration: underline; }
</style>
