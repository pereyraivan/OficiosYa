<template>
  <div class="profile-page">
    <div class="container" style="padding: 40px 0;">
      <!-- Loading -->
      <div v-if="loading" class="text-center py-5">
        <div class="spinner"></div>
        <p class="mt-3">Cargando perfil...</p>
      </div>

      <!-- Error -->
      <div v-else-if="error" class="alert alert-danger">
        <h4>Error</h4>
        <p>{{ error }}</p>
      </div>

      <!-- Perfil del profesional -->
      <div v-else-if="profesional" class="row">
        <!-- Información principal -->
        <div class="col-md-8">
          <div class="card mb-4">
            <div class="card-body">
              <div class="d-flex align-items-start gap-4">
                <!-- Avatar -->
                <div class="avatar-large">
                  <img v-if="profesional.fotoPerfil" :src="profesional.fotoPerfil" :alt="profesional.usuario?.nombre" class="avatar-img">
                  <i v-else class="fas fa-user fa-3x"></i>
                </div>
                
                <!-- Información básica -->
                <div class="flex-grow-1">
                  <h1 class="mb-2">{{ profesional.usuario?.nombre }} {{ profesional.usuario?.apellido }}</h1>
                  <div class="rating mb-2">
                    <span class="stars">{{ generateStars(profesional.calificacionPromedio) }}</span>
                    <span class="ms-2">{{ profesional.calificacionPromedio.toFixed(1) }} ({{ profesional.totalResenas }} reseñas)</span>
                  </div>
                  <p class="text-muted mb-3">{{ profesional.descripcion || 'Sin descripción disponible' }}</p>
                  
                  <!-- Ubicación -->
                  <div class="location mb-3">
                    <i class="fas fa-map-marker-alt me-2"></i>
                    <span>{{ profesional.usuario?.ciudad }}, {{ profesional.usuario?.provincia }}</span>
                  </div>
                  
                  <!-- Disponibilidad -->
                  <div class="availability">
                    <span :class="['badge', profesional.disponible ? 'bg-success' : 'bg-secondary']">
                      {{ profesional.disponible ? 'Disponible ahora' : 'No disponible' }}
                    </span>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Servicios ofrecidos -->
          <div class="card mb-4">
            <div class="card-body">
              <h3 class="mb-3">Servicios ofrecidos</h3>
              <div v-if="profesional.profesionalOficios?.length > 0" class="services-grid">
                <div v-for="servicio in profesional.profesionalOficios" :key="servicio.id" class="service-card">
                  <div class="service-icon">{{ getOficioIcon(servicio.oficioId) }}</div>
                  <div class="service-info">
                    <h5>{{ getOficioName(servicio.oficioId) }}</h5>
                    <p class="service-description">{{ servicio.descripcionEspecializada || 'Servicio profesional' }}</p>
                    <div class="service-details">
                      <span class="price">${{ servicio.precioHora }}/hora</span>
                      <span class="experience">{{ servicio.anosExperiencia }} años de experiencia</span>
                    </div>
                  </div>
                </div>
              </div>
              <div v-else class="text-center py-4">
                <p class="text-muted">No hay servicios disponibles</p>
              </div>
            </div>
          </div>

          <!-- Reseñas -->
          <div class="card">
            <div class="card-body">
              <h3 class="mb-3">Reseñas de clientes</h3>
              <div v-if="resenas.length > 0">
                <div v-for="resena in resenas" :key="resena.id" class="review-card">
                  <div class="review-header">
                    <div class="reviewer-info">
                      <div class="reviewer-avatar">
                        {{ resena.usuario?.nombre?.charAt(0) }}
                      </div>
                      <div>
                        <strong>{{ resena.usuario?.nombre }} {{ resena.usuario?.apellido }}</strong>
                        <div class="review-date">{{ formatDate(resena.fechaCreacion) }}</div>
                      </div>
                    </div>
                    <div class="review-rating">
                      <span class="stars">{{ generateStars(resena.calificacion) }}</span>
                    </div>
                  </div>
                  <p class="review-comment">{{ resena.comentario }}</p>
                </div>
              </div>
              <div v-else class="text-center py-4">
                <p class="text-muted">No hay reseñas disponibles</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Sidebar -->
        <div class="col-md-4">
          <!-- Acciones -->
          <div class="card mb-4">
            <div class="card-body">
              <h4>Contactar</h4>
              <div class="d-grid gap-2">
                <button class="btn btn-primary" @click="contactar">
                  <i class="fas fa-envelope me-2"></i>
                  Enviar mensaje
                </button>
                <button class="btn btn-outline-primary" @click="solicitarPresupuesto">
                  <i class="fas fa-calculator me-2"></i>
                  Solicitar presupuesto
                </button>
                <button class="btn btn-outline-secondary" @click="favoritos">
                  <i class="fas fa-heart me-2"></i>
                  Agregar a favoritos
                </button>
              </div>
            </div>
          </div>

          <!-- Información de contacto -->
          <div class="card mb-4">
            <div class="card-body">
              <h4>Información de contacto</h4>
              <div class="contact-info">
                <div v-if="profesional.usuario?.telefono" class="contact-item">
                  <i class="fas fa-phone me-2"></i>
                  <span>{{ profesional.usuario.telefono }}</span>
                </div>
                <div class="contact-item">
                  <i class="fas fa-envelope me-2"></i>
                  <span>{{ profesional.usuario?.email }}</span>
                </div>
                <div v-if="profesional.usuario?.direccion" class="contact-item">
                  <i class="fas fa-map-marker-alt me-2"></i>
                  <span>{{ profesional.usuario.direccion }}</span>
                </div>
              </div>
            </div>
          </div>

          <!-- Certificaciones -->
          <div v-if="certificados.length > 0" class="card">
            <div class="card-body">
              <h4>Certificaciones</h4>
              <div class="certifications">
                <div v-for="cert in certificados" :key="cert.id" class="certification-item">
                  <div class="cert-icon">
                    <i class="fas fa-certificate"></i>
                  </div>
                  <div class="cert-info">
                    <h6>{{ cert.titulo }}</h6>
                    <p class="small">{{ cert.institucion }}</p>
                    <span v-if="cert.verificado" class="badge bg-success">Verificado</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useOficios } from '../composables/useApi'

export default {
  name: 'ProfessionalProfile',
  props: {
    id: {
      type: String,
      required: true
    }
  },
  setup(props) {
    const route = useRoute()
    const { oficios, loading, error, apiService } = useOficios()
    
    const profesional = ref(null)
    const resenas = ref([])
    const certificados = ref([])

    onMounted(async () => {
      await loadProfesional()
      await loadResenas()
      await loadCertificados()
    })

    const loadProfesional = async () => {
      try {
        const data = await apiService.get(`/oficios/profesional/${props.id}`)
        profesional.value = data
      } catch (err) {
        console.error('Error loading profesional:', err)
      }
    }

    const loadResenas = async () => {
      try {
        const data = await apiService.get(`/oficios/profesional/${props.id}/resenas`)
        resenas.value = data || []
      } catch (err) {
        console.error('Error loading resenas:', err)
      }
    }

    const loadCertificados = async () => {
      try {
        const data = await apiService.get(`/oficios/profesional/${props.id}/certificados`)
        certificados.value = data || []
      } catch (err) {
        console.error('Error loading certificados:', err)
      }
    }

    const getOficioName = (oficioId) => {
      const oficio = oficios.value.find(o => o.id === oficioId)
      return oficio?.nombre || 'Servicio'
    }

    const getOficioIcon = (oficioId) => {
      const oficio = oficios.value.find(o => o.id === oficioId)
      return oficio?.icono || '🔧'
    }

    const generateStars = (rating) => {
      const stars = Math.round(rating)
      const fullStars = '★'.repeat(stars)
      const emptyStars = '☆'.repeat(5 - stars)
      return fullStars + emptyStars
    }

    const formatDate = (dateString) => {
      const date = new Date(dateString)
      return date.toLocaleDateString('es-ES', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    }

    const contactar = () => {
      alert('Funcionalidad de contacto en desarrollo')
    }

    const solicitarPresupuesto = () => {
      alert('Funcionalidad de presupuesto en desarrollo')
    }

    const favoritos = () => {
      alert('Funcionalidad de favoritos en desarrollo')
    }

    return {
      profesional,
      resenas,
      certificados,
      oficios,
      loading,
      error,
      getOficioName,
      getOficioIcon,
      generateStars,
      formatDate,
      contactar,
      solicitarPresupuesto,
      favoritos
    }
  }
}
</script>

<style scoped>
.profile-page {
  min-height: 100vh;
  background: #f8fafc;
}

.card {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 18px;
  box-shadow: 0 10px 30px rgba(2,6,23,.12);
  margin-bottom: 1.5rem;
}

.card-body {
  padding: 1.5rem;
}

.avatar-large {
  width: 120px;
  height: 120px;
  border-radius: 50%;
  background: linear-gradient(135deg, #e2e8f0, #94a3b8);
  display: flex;
  align-items: center;
  justify-content: center;
  overflow: hidden;
}

.avatar-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.rating {
  display: flex;
  align-items: center;
}

.stars {
  color: #f59e0b;
  font-size: 1.2em;
}

.location {
  color: #6b7280;
}

.badge {
  padding: 6px 12px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: 600;
}

.bg-success {
  background: #22c55e !important;
  color: white;
}

.bg-secondary {
  background: #6b7280 !important;
  color: white;
}

.services-grid {
  display: grid;
  gap: 1rem;
}

.service-card {
  display: flex;
  gap: 1rem;
  padding: 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  background: #f8fafc;
}

.service-icon {
  font-size: 2rem;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 60px;
  height: 60px;
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.service-info {
  flex: 1;
}

.service-details {
  display: flex;
  gap: 1rem;
  margin-top: 0.5rem;
}

.price {
  font-weight: 600;
  color: #22c55e;
}

.experience {
  color: #6b7280;
  font-size: 0.9em;
}

.review-card {
  padding: 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  margin-bottom: 1rem;
  background: #f8fafc;
}

.review-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 0.5rem;
}

.reviewer-info {
  display: flex;
  gap: 0.75rem;
  align-items: center;
}

.reviewer-avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: #22c55e;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
}

.review-date {
  font-size: 0.875em;
  color: #6b7280;
}

.review-comment {
  margin: 0;
  line-height: 1.5;
}

.contact-info {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.contact-item {
  display: flex;
  align-items: center;
  color: #6b7280;
}

.certifications {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.certification-item {
  display: flex;
  gap: 1rem;
  align-items: center;
  padding: 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  background: #f8fafc;
}

.cert-icon {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  background: #22c55e;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.2em;
}

.cert-info h6 {
  margin: 0 0 0.25rem 0;
  font-weight: 600;
}

.btn {
  padding: 12px 24px;
  border-radius: 18px;
  border: none;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.2s;
  text-decoration: none;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.btn-primary {
  background: #22c55e;
  color: white;
}

.btn-primary:hover {
  background: #16a34a;
}

.btn-outline-primary {
  background: transparent;
  border: 1px solid #22c55e;
  color: #22c55e;
}

.btn-outline-primary:hover {
  background: #22c55e;
  color: white;
}

.btn-outline-secondary {
  background: transparent;
  border: 1px solid #6b7280;
  color: #6b7280;
}

.btn-outline-secondary:hover {
  background: #6b7280;
  color: white;
}

.d-grid {
  display: grid;
}

.gap-2 {
  gap: 0.5rem;
}

.spinner {
  width: 40px;
  height: 40px;
  border: 4px solid #f3f3f3;
  border-top: 4px solid #22c55e;
  border-radius: 50%;
  animation: spin 1s linear infinite;
  margin: 0 auto;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 20px;
}

.row {
  display: flex;
  gap: 2rem;
}

.col-md-8 {
  flex: 0 0 66.666667%;
  max-width: 66.666667%;
}

.col-md-4 {
  flex: 0 0 33.333333%;
  max-width: 33.333333%;
}

.mb-2 {
  margin-bottom: 0.5rem;
}

.mb-3 {
  margin-bottom: 1rem;
}

.mb-4 {
  margin-bottom: 1.5rem;
}

.mt-3 {
  margin-top: 1rem;
}

.d-flex {
  display: flex;
}

.align-items-start {
  align-items: flex-start;
}

.align-items-center {
  align-items: center;
}

.justify-content-between {
  justify-content: space-between;
}

.flex-grow-1 {
  flex-grow: 1;
}

.gap-4 {
  gap: 1.5rem;
}

.text-center {
  text-align: center;
}

.py-5 {
  padding-top: 3rem;
  padding-bottom: 3rem;
}

.py-4 {
  padding-top: 1.5rem;
  padding-bottom: 1.5rem;
}

.text-muted {
  color: #6b7280;
}

.small {
  font-size: 0.875em;
}

.alert {
  padding: 12px 16px;
  border-radius: 18px;
  margin: 16px 0;
}

.alert-danger {
  background: #fef2f2;
  color: #991b1b;
  border: 1px solid #fecaca;
}

/* Responsive */
@media (max-width: 768px) {
  .row {
    flex-direction: column;
  }
  
  .col-md-8,
  .col-md-4 {
    flex: 0 0 100%;
    max-width: 100%;
  }
  
  .d-flex.gap-4 {
    flex-direction: column;
    text-align: center;
  }
  
  .avatar-large {
    align-self: center;
  }
}
</style>
