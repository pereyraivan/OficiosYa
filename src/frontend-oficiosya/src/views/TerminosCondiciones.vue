<template>
  <div class="terminos-condiciones">
    <!-- Header -->
    <header>
      <div class="container nav">
        <div class="logo" @click="irAHome">
          <div class="logo-badge">🛠️</div>
          <span>OficiosYa</span>
        </div>
        <nav class="navlinks">
          <a href="#" @click="irAHome">Inicio</a>
          <a href="#" @click="irARegistro">Registrarse</a>
        </nav>
      </div>
    </header>

    <!-- Contenido -->
    <section class="section" style="background: #f8fafc; min-height: calc(100vh - 80px);">
      <div class="container" style="max-width: 800px; padding-top: 40px;">
        <div class="card" style="padding: 40px;">
          <h1 style="text-align: center; margin-bottom: 30px;">Términos y Condiciones</h1>
          
          <div style="text-align: center; margin-bottom: 30px; color: var(--muted);">
            <p>Última actualización: {{ fechaActualizacion }}</p>
          </div>

          <div class="content" style="line-height: 1.6;">
            <h2>1. Aceptación de los Términos</h2>
            <p>Al acceder y utilizar OficiosYa, usted acepta estar sujeto a estos términos y condiciones de uso. Si no está de acuerdo con alguna parte de estos términos, no debe utilizar nuestro servicio.</p>

            <h2>2. Descripción del Servicio</h2>
            <p>OficiosYa es una plataforma que conecta clientes con profesionales de servicios para el hogar, incluyendo pero no limitado a electricistas, plomeros, albañiles, gasistas, jardineros, pintores, cerrajeros y otros servicios relacionados.</p>

            <h2>3. Registro de Usuarios</h2>
            <p>Para utilizar nuestros servicios, debe registrarse y crear una cuenta. Usted es responsable de:</p>
            <ul>
              <li>Proporcionar información veraz y actualizada</li>
              <li>Mantener la confidencialidad de su cuenta</li>
              <li>Notificar inmediatamente cualquier uso no autorizado</li>
              <li>Ser mayor de 18 años o tener el consentimiento de un tutor legal</li>
            </ul>

            <h2>4. Responsabilidades de los Profesionales</h2>
            <p>Los profesionales registrados se comprometen a:</p>
            <ul>
              <li>Proporcionar servicios de calidad y cumplir con los estándares profesionales</li>
              <li>Mantener actualizada su información de contacto y disponibilidad</li>
              <li>Respetar los horarios acordados con los clientes</li>
              <li>Cumplir con todas las regulaciones y licencias aplicables</li>
              <li>Proporcionar presupuestos claros y detallados</li>
            </ul>

            <h2>5. Responsabilidades de los Clientes</h2>
            <p>Los clientes se comprometen a:</p>
            <ul>
              <li>Proporcionar información precisa sobre el trabajo requerido</li>
              <li>Respetar los horarios acordados</li>
              <li>Pagar los servicios según los términos acordados</li>
              <li>Proporcionar acceso seguro al lugar de trabajo</li>
            </ul>

            <h2>6. Pagos y Facturación</h2>
            <p>Los pagos se procesan de acuerdo con los términos acordados entre el cliente y el profesional. OficiosYa puede cobrar una comisión por facilitar la conexión entre las partes.</p>

            <h2>7. Limitación de Responsabilidad</h2>
            <p>OficiosYa actúa únicamente como intermediario entre clientes y profesionales. No somos responsables por:</p>
            <ul>
              <li>La calidad de los servicios proporcionados por los profesionales</li>
              <li>Daños o pérdidas resultantes de los servicios</li>
              <li>Disputas entre clientes y profesionales</li>
            </ul>

            <h2>8. Propiedad Intelectual</h2>
            <p>Todo el contenido de la plataforma, incluyendo textos, gráficos, logotipos, imágenes y software, es propiedad de OficiosYa y está protegido por las leyes de propiedad intelectual.</p>

            <h2>9. Modificaciones</h2>
            <p>Nos reservamos el derecho de modificar estos términos en cualquier momento. Las modificaciones entrarán en vigor inmediatamente después de su publicación en la plataforma.</p>

            <h2>10. Contacto</h2>
            <p>Para preguntas sobre estos términos y condiciones, puede contactarnos en:</p>
            <ul>
              <li>Email: legal@oficiosya.com</li>
              <li>Teléfono: +54 381 123-4567</li>
              <li>Dirección: San Miguel de Tucumán, Argentina</li>
            </ul>
          </div>

          <div style="text-align: center; margin-top: 40px;">
            <button @click="irAHome" class="cta">Volver al Inicio</button>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { ROUTES } from '../utils/constants'

const router = useRouter()
const fechaActualizacion = ref('20 de Octubre de 2025')

const irAHome = () => {
  router.push(ROUTES.HOME)
}

const irARegistro = () => {
  router.push(ROUTES.REGISTER_PROFESSIONAL)
}
</script>

<style scoped>
.terminos-condiciones {
  min-height: 100vh;
}

.logo {
  cursor: pointer;
}

.content h2 {
  color: var(--text);
  margin-top: 30px;
  margin-bottom: 15px;
  font-size: 1.2em;
}

.content p {
  margin-bottom: 15px;
  color: var(--muted);
}

.content ul {
  margin-bottom: 20px;
  padding-left: 20px;
}

.content li {
  margin-bottom: 8px;
  color: var(--muted);
}
</style>
