<template>
  <div class="politica-privacidad">
    <section class="section legales-section">
      <div class="container legales-container">
        <div class="card legales-card">
          <h1 class="legales-titulo">Política de Privacidad</h1>
          <div class="legales-fecha">
            <p>Última actualización: {{ fechaActualizacion }}</p>
          </div>

          <div class="content">
            <h2>1. Responsable del Tratamiento</h2>
            <p>
              El responsable del tratamiento de sus datos personales en el marco de la plataforma TuOficioPro
              (en adelante, “TuOficioPro” o “nosotros”) es:
            </p>
            <ul>
              <li><strong>Denominación:</strong> TuOficioPro</li>
              <li><strong>Domicilio:</strong> San Miguel de Tucumán, Tucumán, Argentina</li>
              <li><strong>Email de contacto:</strong> <strong>{{ contactEmail }}</strong></li>
              <li><strong>Sitio web:</strong> {{ siteDomain }}</li>
            </ul>
            <p>
              Cuando se disponga de razón social y CUIT del titular, se publicarán en esta sección.
              Esta política se enmarca en la <strong>Ley 25.326 de Protección de Datos Personales</strong> (Argentina)
              y en las disposiciones de la Dirección Nacional de Protección de Datos Personales (DNPDP).
            </p>

            <h2>2. Información que Recopilamos</h2>
            <p>Recopilamos la información que usted nos proporciona al registrarse y usar la plataforma, incluyendo:</p>
            <ul>
              <li><strong>Datos de registro y cuenta:</strong> Nombre, apellido, email, teléfono, contraseña (almacenada de forma segura), dirección, ciudad, provincia, código postal.</li>
              <li><strong>Datos de perfil profesional (si aplica):</strong> Descripción, oficios o categorías, zonas de cobertura, fotos de perfil y de trabajos, certificados o documentos que suba.</li>
              <li><strong>Datos de uso:</strong> Interacciones con la plataforma, búsquedas, perfiles visitados, reseñas que publique, contactos realizados.</li>
              <li><strong>Datos de suscripción y pago (si aplica):</strong> Plan elegido, duración, método de pago indicado, comprobantes o referencias de transferencia.</li>
              <li><strong>Datos técnicos:</strong> Dirección IP, tipo de navegador, dispositivo y registros de acceso necesarios para seguridad y funcionamiento.</li>
            </ul>

            <h2>3. Finalidades del Tratamiento</h2>
            <p>Utilizamos sus datos para:</p>
            <ul>
              <li>Gestionar su cuenta y permitir el uso de la plataforma como intermediario entre quienes buscan y quienes ofrecen servicios.</li>
              <li>Mostrar perfiles de profesionales a los usuarios (nombre, oficios, zona, descripción, fotos, reseñas, etc.) según la configuración de visibilidad.</li>
              <li>Gestionar suscripciones, pagos, facturación y solicitudes de arrepentimiento o baja cuando corresponda.</li>
              <li>Enviar comunicaciones transaccionales (recuperación de contraseña, confirmaciones, avisos de cuenta).</li>
              <li>Comunicarnos con usted para soporte y, con su consentimiento, novedades o promociones.</li>
              <li>Garantizar la seguridad, prevenir fraudes, moderar contenido y cumplir obligaciones legales.</li>
              <li>Mejorar nuestros servicios mediante estadísticas agregadas y anónimas.</li>
            </ul>

            <h2>4. Base Legal y Consentimiento</h2>
            <p>
              El tratamiento se basa en su consentimiento (al registrarse y aceptar esta política), en la ejecución
              del contrato de uso de la plataforma y, cuando corresponda, en el cumplimiento de obligaciones legales
              o en el interés legítimo de TuOficioPro (por ejemplo, seguridad y prevención de fraude).
              Para envíos comerciales solicitamos su consentimiento y puede revocarlo en cualquier momento escribiendo a
              <strong>{{ contactEmail }}</strong>.
            </p>

            <h2>5. Compartición de Datos</h2>
            <p><strong>No vendemos sus datos personales.</strong> Podemos compartir información:</p>
            <ul>
              <li><strong>Entre usuarios de la plataforma:</strong> Para que la intermediación funcione (por ejemplo, que los clientes vean el perfil, contacto o reseñas de los profesionales).</li>
              <li><strong>Con proveedores de servicios:</strong> Hosting (Somee u otro proveedor de alojamiento web), servicio de correo electrónico (Gmail/Google SMTP u equivalente), y herramientas técnicas necesarias para operar la plataforma, siempre con obligaciones de confidencialidad y uso limitado.</li>
              <li><strong>Por obligación legal:</strong> Cuando lo exija la ley o una autoridad competente.</li>
              <li><strong>Con su consentimiento:</strong> En otros casos que le informemos y para los que solicitemos su autorización expresa.</li>
            </ul>

            <h2>6. Seguridad y Retención</h2>
            <p>
              Aplicamos medidas técnicas y organizativas para proteger sus datos (acceso restringido, contraseñas
              hasheadas, HTTPS cuando esté disponible, buenas prácticas de desarrollo). Conservamos la información
              mientras sea necesaria para las finalidades indicadas, la relación contractual activa y el cumplimiento
              de obligaciones legales, contables o de defensa de derechos; después se elimina o anonimiza cuando ya no
              sea necesaria.
            </p>

            <h2>7. Sus Derechos (Ley 25.326)</h2>
            <p>Usted tiene derecho a:</p>
            <ul>
              <li><strong>Acceso:</strong> Conocer qué datos tenemos sobre usted y obtener una copia.</li>
              <li><strong>Rectificación:</strong> Corregir datos inexactos o incompletos.</li>
              <li><strong>Supresión:</strong> Solicitar la eliminación de sus datos cuando la ley lo permita.</li>
              <li><strong>Oposición y limitación:</strong> Oponerse a ciertos tratamientos o solicitar su limitación en los casos previstos por la ley.</li>
            </ul>
            <p>
              Para ejercer estos derechos, escriba a <strong>{{ contactEmail }}</strong> indicando su solicitud
              y un medio para confirmar su identidad. También puede acudir a la
              <strong>Dirección Nacional de Protección de Datos Personales (DNPDP)</strong>:
              <a href="https://www.argentina.gob.ar/justicia/datospersonales" target="_blank" rel="noopener">argentina.gob.ar/justicia/datospersonales</a>.
            </p>

            <h2>8. Cookies y Tecnologías Similares</h2>
            <p>TuOficioPro utiliza cookies y tecnologías similares para:</p>
            <ul>
              <li><strong>Cookies técnicas (necesarias):</strong> Mantener la sesión iniciada, recordar preferencias básicas y garantizar el funcionamiento seguro del sitio. No requieren consentimiento previo.</li>
              <li><strong>Cookies analíticas o de marketing (si se implementan):</strong> Medir uso del sitio o campañas publicitarias. Solo se activarán con su consentimiento expreso cuando corresponda.</li>
            </ul>
            <p>
              Puede configurar su navegador para rechazar cookies; parte del sitio (por ejemplo, el inicio de sesión)
              podría dejar de funcionar correctamente. Actualmente TuOficioPro no utiliza Google Analytics ni Meta Pixel;
              si se incorporan en el futuro, se actualizará esta política y se solicitará consentimiento cuando la ley lo exija.
            </p>

            <h2>9. Menores</h2>
            <p>
              La plataforma no está dirigida a menores de 18 años. No recopilamos datos de menores de forma voluntaria;
              si tomamos conocimiento de que se han proporcionado datos de un menor, procederemos a eliminar dicha información.
            </p>

            <h2>10. Arrepentimiento, baja y reclamos</h2>
            <p>
              Para ejercer el derecho de arrepentimiento o solicitar la baja de un servicio contratado con TuOficioPro,
              puede utilizar el formulario en
              <RouterLink to="/arrepentimiento-y-baja">Arrepentimiento y baja de servicio</RouterLink>
              o escribir a <strong>{{ contactEmail }}</strong>.
            </p>

            <h2>11. Cambios a esta Política</h2>
            <p>
              Podemos actualizar esta Política de Privacidad. Los cambios se publicarán en esta página con la nueva
              fecha de “Última actualización”. Le recomendamos revisarla periódicamente. Para cambios sustanciales
              podremos notificar por email o mediante un aviso destacado en la plataforma.
            </p>

            <h2>12. Contacto</h2>
            <p>Para consultas sobre privacidad o para ejercer sus derechos:</p>
            <ul>
              <li>Email: <strong>{{ contactEmail }}</strong></li>
              <li>Dirección: San Miguel de Tucumán, Tucumán, Argentina</li>
            </ul>
          </div>

          <div class="legales-acciones">
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
import { ROUTES, APP_CONFIG } from '../utils/constants'

const router = useRouter()
const fechaActualizacion = ref('27 de agosto de 2026')
const contactEmail = APP_CONFIG.CONTACT_EMAIL
const siteDomain = APP_CONFIG.SITE_DOMAIN

const irAHome = () => {
  router.push(ROUTES.HOME)
}
</script>

<style scoped>
.politica-privacidad {
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
  padding-top: 0;
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

.legales-acciones {
  text-align: center;
  margin-top: 40px;
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

.content a {
  color: var(--primary, #2563eb);
}

@media (max-width: 600px) {
  .legales-section {
    padding: 12px 0 48px;
  }

  .legales-container {
    max-width: 100%;
    padding-left: 10px;
    padding-right: 10px;
  }

  .legales-card {
    padding: 18px 14px;
    border-radius: 12px;
  }

  .legales-titulo {
    font-size: 1.35rem;
    margin-bottom: 16px;
  }

  .legales-fecha {
    margin-bottom: 20px;
    font-size: 0.875rem;
  }

  .content h2 {
    font-size: 1.05rem;
    margin-top: 22px;
  }
}
</style>
