<template>
  <div class="politica-privacidad">
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
          <h1 style="text-align: center; margin-bottom: 30px;">Política de Privacidad</h1>
          
          <div style="text-align: center; margin-bottom: 30px; color: var(--muted);">
            <p>Última actualización: {{ fechaActualizacion }}</p>
          </div>

          <div class="content" style="line-height: 1.6;">
            <h2>1. Información que Recopilamos</h2>
            <p>Recopilamos información que usted nos proporciona directamente, incluyendo:</p>
            <ul>
              <li><strong>Información de registro:</strong> Nombre, apellido, email, teléfono, dirección</li>
              <li><strong>Información profesional:</strong> Especialidades, certificaciones, experiencia</li>
              <li><strong>Información de transacciones:</strong> Historial de servicios, pagos</li>
              <li><strong>Comunicaciones:</strong> Mensajes entre clientes y profesionales</li>
            </ul>

            <h2>2. Cómo Utilizamos su Información</h2>
            <p>Utilizamos su información para:</p>
            <ul>
              <li>Proporcionar y mejorar nuestros servicios</li>
              <li>Conectar clientes con profesionales apropiados</li>
              <li>Procesar pagos y transacciones</li>
              <li>Comunicarnos con usted sobre servicios y actualizaciones</li>
              <li>Garantizar la seguridad de la plataforma</li>
              <li>Cumplir con obligaciones legales</li>
            </ul>

            <h2>3. Compartir Información</h2>
            <p>No vendemos su información personal. Podemos compartir información en las siguientes circunstancias:</p>
            <ul>
              <li><strong>Con profesionales:</strong> Para facilitar la conexión de servicios</li>
              <li><strong>Proveedores de servicios:</strong> Para procesar pagos y operaciones</li>
              <li><strong>Cumplimiento legal:</strong> Cuando sea requerido por ley</li>
              <li><strong>Consentimiento:</strong> Cuando usted nos dé su consentimiento explícito</li>
            </ul>

            <h2>4. Seguridad de los Datos</h2>
            <p>Implementamos medidas de seguridad técnicas y organizativas para proteger su información:</p>
            <ul>
              <li>Encriptación de datos sensibles</li>
              <li>Acceso restringido a información personal</li>
              <li>Monitoreo regular de seguridad</li>
              <li>Capacitación del personal en privacidad</li>
            </ul>

            <h2>5. Sus Derechos</h2>
            <p>Usted tiene derecho a:</p>
            <ul>
              <li><strong>Acceso:</strong> Solicitar una copia de su información personal</li>
              <li><strong>Rectificación:</strong> Corregir información inexacta</li>
              <li><strong>Eliminación:</strong> Solicitar la eliminación de su información</li>
              <li><strong>Limitación:</strong> Restringir el procesamiento de su información</li>
              <li><strong>Portabilidad:</strong> Recibir su información en formato estructurado</li>
              <li><strong>Oposición:</strong> Oponerse al procesamiento de su información</li>
            </ul>

            <h2>6. Cookies y Tecnologías Similares</h2>
            <p>Utilizamos cookies y tecnologías similares para:</p>
            <ul>
              <li>Mejorar la funcionalidad del sitio web</li>
              <li>Recordar sus preferencias</li>
              <li>Analizar el uso del sitio</li>
              <li>Personalizar su experiencia</li>
            </ul>

            <h2>7. Retención de Datos</h2>
            <p>Conservamos su información personal durante el tiempo necesario para:</p>
            <ul>
              <li>Cumplir con los propósitos para los cuales fue recopilada</li>
              <li>Cumplir con obligaciones legales</li>
              <li>Resolver disputas</li>
              <li>Hacer cumplir nuestros acuerdos</li>
            </ul>

            <h2>8. Transferencias Internacionales</h2>
            <p>Su información puede ser transferida y procesada en países fuera de Argentina. Nos aseguramos de que dichas transferencias cumplan con las leyes de protección de datos aplicables.</p>

            <h2>9. Menores de Edad</h2>
            <p>Nuestros servicios no están dirigidos a menores de 18 años. No recopilamos intencionalmente información personal de menores de edad.</p>

            <h2>10. Cambios a esta Política</h2>
            <p>Podemos actualizar esta política de privacidad ocasionalmente. Le notificaremos sobre cambios significativos a través de la plataforma o por email.</p>

            <h2>11. Contacto</h2>
            <p>Para preguntas sobre esta política de privacidad o para ejercer sus derechos, puede contactarnos en:</p>
            <ul>
              <li>Email: privacidad@oficiosya.com</li>
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
.politica-privacidad {
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

.content strong {
  color: var(--text);
}
</style>
