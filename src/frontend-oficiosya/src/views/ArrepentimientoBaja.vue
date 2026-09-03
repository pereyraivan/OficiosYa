<template>
  <div class="arrepentimiento-baja">
    <section class="section legales-section">
      <div class="container legales-container">
        <div class="card legales-card">
          <h1 class="legales-titulo">Arrepentimiento y baja de servicio</h1>
          <div class="legales-fecha">
            <p>Última actualización: {{ fechaActualizacion }}</p>
          </div>

          <div class="content">
            <p>
              Conforme a la legislación argentina de defensa del consumidor (Ley 24.240 y normativa complementaria),
              cuando corresponda podés ejercer el <strong>derecho de arrepentimiento</strong> en contrataciones a distancia
              o solicitar la <strong>baja de un servicio</strong> contratado con TuOficioPro (por ejemplo, un plan de suscripción para profesionales).
            </p>
            <p class="aviso">
              El plazo, requisitos y excepciones dependen de la normativa vigente y del tipo de contratación.
              Completá el formulario y te responderemos a la brevedad en <strong>{{ contactEmail }}</strong>.
            </p>

            <form class="legal-form" @submit.prevent="enviarSolicitud">
              <div class="form-group">
                <label for="tipo">Tipo de solicitud *</label>
                <select id="tipo" v-model="form.tipo" required>
                  <option value="">Seleccioná una opción</option>
                  <option value="arrepentimiento">Botón de arrepentimiento</option>
                  <option value="baja">Botón de baja de servicio</option>
                </select>
              </div>

              <div class="form-row">
                <div class="form-group">
                  <label for="nombre">Nombre y apellido *</label>
                  <input id="nombre" v-model="form.nombre" type="text" required maxlength="120" />
                </div>
                <div class="form-group">
                  <label for="email">Email de la cuenta *</label>
                  <input id="email" v-model="form.email" type="email" required maxlength="120" />
                </div>
              </div>

              <div class="form-row">
                <div class="form-group">
                  <label for="plan">Plan o servicio contratado</label>
                  <input id="plan" v-model="form.plan" type="text" placeholder="Ej.: Premium, Básica, Pro" maxlength="80" />
                </div>
                <div class="form-group">
                  <label for="fecha">Fecha de contratación</label>
                  <input id="fecha" v-model="form.fechaContratacion" type="date" />
                </div>
              </div>

              <div class="form-group">
                <label for="motivo">Motivo o comentarios *</label>
                <textarea
                  id="motivo"
                  v-model="form.motivo"
                  required
                  rows="4"
                  maxlength="2000"
                  placeholder="Describí brevemente tu solicitud"
                />
              </div>

              <p v-if="error" class="error-msg">{{ error }}</p>
              <p v-if="enviado" class="success-msg">
                Se abrió tu cliente de correo con el mensaje preparado. Si no se abrió, enviá manualmente a
                <strong>{{ contactEmail }}</strong> con los mismos datos.
              </p>

              <div class="form-actions">
                <button type="submit" class="cta">Enviar solicitud</button>
                <a :href="mailtoFallback" class="link-secundario">O escribinos por email</a>
              </div>
            </form>

            <h2>Información adicional</h2>
            <ul>
              <li>El derecho de arrepentimiento aplica cuando la ley lo reconozca para contratos a distancia con consumidores.</li>
              <li>La baja de servicio impide renovaciones futuras; no afecta obligaciones ya devengadas antes de la solicitud.</li>
              <li>Podés consultar también los <RouterLink to="/terminos-y-condiciones">Términos y Condiciones</RouterLink> (secciones 17 y 18).</li>
            </ul>
          </div>

          <div class="legales-acciones">
            <button type="button" @click="irAHome" class="cta cta-outline">Volver al inicio</button>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { ROUTES, APP_CONFIG } from '../utils/constants'

const router = useRouter()
const contactEmail = APP_CONFIG.CONTACT_EMAIL
const fechaActualizacion = ref('27 de agosto de 2026')
const enviado = ref(false)
const error = ref('')

const form = reactive({
  tipo: '',
  nombre: '',
  email: '',
  plan: '',
  fechaContratacion: '',
  motivo: ''
})

const tipoLabel = computed(() =>
  form.tipo === 'arrepentimiento' ? 'Arrepentimiento' : form.tipo === 'baja' ? 'Baja de servicio' : ''
)

const mailtoFallback = computed(() => {
  const subject = encodeURIComponent('Solicitud legal - TuOficioPro')
  return `mailto:${contactEmail}?subject=${subject}`
})

const enviarSolicitud = () => {
  error.value = ''
  if (!form.tipo || !form.nombre.trim() || !form.email.trim() || !form.motivo.trim()) {
    error.value = 'Completá los campos obligatorios.'
    return
  }

  const subject = encodeURIComponent(
    form.tipo === 'arrepentimiento'
      ? 'Solicitud de arrepentimiento - TuOficioPro'
      : 'Solicitud de baja de servicio - TuOficioPro'
  )

  const body = encodeURIComponent(
    [
      `Tipo de solicitud: ${tipoLabel.value}`,
      `Nombre: ${form.nombre.trim()}`,
      `Email: ${form.email.trim()}`,
      form.plan.trim() ? `Plan/servicio: ${form.plan.trim()}` : '',
      form.fechaContratacion ? `Fecha de contratación: ${form.fechaContratacion}` : '',
      '',
      'Motivo:',
      form.motivo.trim(),
      '',
      '---',
      'Enviado desde el formulario de Arrepentimiento y Baja de TuOficioPro'
    ]
      .filter(Boolean)
      .join('\n')
  )

  window.location.href = `mailto:${contactEmail}?subject=${subject}&body=${body}`
  enviado.value = true
}

const irAHome = () => {
  router.push(ROUTES.HOME)
}
</script>

<style scoped>
.arrepentimiento-baja {
  min-height: 100vh;
}

.legales-section {
  background: #f8fafc;
  min-height: calc(100vh - 80px);
  padding: 40px 24px 60px;
}

.legales-container {
  max-width: 800px;
  margin: 0 auto;
}

.legales-card {
  padding: 40px;
  cursor: default;
}

.legales-card:hover {
  transform: none;
}

.legales-titulo {
  text-align: center;
  margin-bottom: 30px;
}

.legales-fecha {
  text-align: center;
  margin-bottom: 30px;
  color: var(--muted);
}

.content {
  line-height: 1.6;
}

.content p {
  margin-bottom: 15px;
  color: var(--muted);
}

.aviso {
  background: #eff6ff;
  border-left: 4px solid #3b82f6;
  padding: 12px 16px;
  border-radius: 0 8px 8px 0;
}

.legal-form {
  margin: 28px 0;
  padding: 24px;
  background: #fff;
  border: 1px solid #e2e8f0;
  border-radius: 12px;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.form-group {
  margin-bottom: 16px;
}

.form-group label {
  display: block;
  margin-bottom: 6px;
  font-weight: 600;
  color: var(--text);
  font-size: 0.9rem;
}

.form-group input,
.form-group select,
.form-group textarea {
  width: 100%;
  padding: 10px 12px;
  border: 1px solid #cbd5e1;
  border-radius: 8px;
  font-family: inherit;
  font-size: 1rem;
}

.form-actions {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: 16px;
  margin-top: 8px;
}

.link-secundario {
  color: var(--primary, #2563eb);
  text-decoration: underline;
}

.error-msg {
  color: #dc2626;
  font-size: 0.9rem;
}

.success-msg {
  color: #059669;
  font-size: 0.9rem;
  background: #ecfdf5;
  padding: 12px;
  border-radius: 8px;
}

.content h2 {
  color: var(--text);
  margin-top: 30px;
  margin-bottom: 15px;
  font-size: 1.2em;
}

.content ul {
  margin-bottom: 20px;
  padding-left: 20px;
}

.content li {
  margin-bottom: 8px;
  color: var(--muted);
}

.legales-acciones {
  text-align: center;
  margin-top: 40px;
}

.cta-outline {
  background: transparent;
  color: var(--primary, #2563eb);
  border: 2px solid var(--primary, #2563eb);
}

@media (max-width: 600px) {
  .legales-section {
    padding: 12px 0 48px;
  }

  .legales-container {
    padding-left: 10px;
    padding-right: 10px;
  }

  .legales-card {
    padding: 18px 14px;
  }

  .form-row {
    grid-template-columns: 1fr;
  }

  .legal-form {
    padding: 16px;
  }
}
</style>
