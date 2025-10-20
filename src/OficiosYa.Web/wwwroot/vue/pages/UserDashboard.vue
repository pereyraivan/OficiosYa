<template>
  <div class="dashboard-page">
    <div class="container" style="padding: 40px 0;">
      <div class="row">
        <!-- Sidebar -->
        <div class="col-md-3">
          <div class="card">
            <div class="card-body">
              <div class="user-info">
                <div class="user-avatar">
                  <i class="fas fa-user fa-2x"></i>
                </div>
                <h4>{{ user?.nombre }} {{ user?.apellido }}</h4>
                <p class="text-muted">{{ user?.email }}</p>
              </div>
              
              <nav class="dashboard-nav">
                <a href="#" :class="{ active: activeTab === 'profile' }" @click="activeTab = 'profile'">
                  <i class="fas fa-user me-2"></i>
                  Mi Perfil
                </a>
                <a href="#" :class="{ active: activeTab === 'services' }" @click="activeTab = 'services'">
                  <i class="fas fa-tools me-2"></i>
                  Mis Servicios
                </a>
                <a href="#" :class="{ active: activeTab === 'bookings' }" @click="activeTab = 'bookings'">
                  <i class="fas fa-calendar me-2"></i>
                  Reservas
                </a>
                <a href="#" :class="{ active: activeTab === 'reviews' }" @click="activeTab = 'reviews'">
                  <i class="fas fa-star me-2"></i>
                  Reseñas
                </a>
                <a href="#" :class="{ active: activeTab === 'settings' }" @click="activeTab = 'settings'">
                  <i class="fas fa-cog me-2"></i>
                  Configuración
                </a>
              </nav>
            </div>
          </div>
        </div>

        <!-- Contenido principal -->
        <div class="col-md-9">
          <!-- Mi Perfil -->
          <div v-if="activeTab === 'profile'" class="tab-content">
            <div class="card">
              <div class="card-header">
                <h3>Mi Perfil</h3>
              </div>
              <div class="card-body">
                <form @submit.prevent="updateProfile">
                  <div class="row">
                    <div class="col-md-6">
                      <div class="form-group">
                        <label for="nombre">Nombre</label>
                        <input v-model="profileForm.nombre" type="text" class="form-control" required>
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="form-group">
                        <label for="apellido">Apellido</label>
                        <input v-model="profileForm.apellido" type="text" class="form-control" required>
                      </div>
                    </div>
                  </div>
                  
                  <div class="form-group">
                    <label for="email">Email</label>
                    <input v-model="profileForm.email" type="email" class="form-control" required>
                  </div>
                  
                  <div class="form-group">
                    <label for="telefono">Teléfono</label>
                    <input v-model="profileForm.telefono" type="tel" class="form-control">
                  </div>
                  
                  <div class="form-group">
                    <label for="direccion">Dirección</label>
                    <input v-model="profileForm.direccion" type="text" class="form-control">
                  </div>
                  
                  <div class="row">
                    <div class="col-md-6">
                      <div class="form-group">
                        <label for="ciudad">Ciudad</label>
                        <input v-model="profileForm.ciudad" type="text" class="form-control">
                      </div>
                    </div>
                    <div class="col-md-6">
                      <div class="form-group">
                        <label for="provincia">Provincia</label>
                        <input v-model="profileForm.provincia" type="text" class="form-control">
                      </div>
                    </div>
                  </div>
                  
                  <button type="submit" class="btn btn-primary" :disabled="loading">
                    {{ loading ? 'Guardando...' : 'Guardar Cambios' }}
                  </button>
                </form>
              </div>
            </div>
          </div>

          <!-- Mis Servicios -->
          <div v-if="activeTab === 'services'" class="tab-content">
            <div class="card">
              <div class="card-header d-flex justify-content-between align-items-center">
                <h3>Mis Servicios</h3>
                <button class="btn btn-primary" @click="showAddService = true">
                  <i class="fas fa-plus me-2"></i>
                  Agregar Servicio
                </button>
              </div>
              <div class="card-body">
                <div v-if="servicios.length > 0" class="services-list">
                  <div v-for="servicio in servicios" :key="servicio.id" class="service-item">
                    <div class="service-info">
                      <h5>{{ getOficioName(servicio.oficioId) }}</h5>
                      <p>{{ servicio.descripcionEspecializada || 'Sin descripción' }}</p>
                      <div class="service-meta">
                        <span class="price">${{ servicio.precioHora }}/hora</span>
                        <span class="experience">{{ servicio.anosExperiencia }} años de experiencia</span>
                      </div>
                    </div>
                    <div class="service-actions">
                      <button class="btn btn-sm btn-outline-primary" @click="editService(servicio)">
                        <i class="fas fa-edit"></i>
                      </button>
                      <button class="btn btn-sm btn-outline-danger" @click="deleteService(servicio.id)">
                        <i class="fas fa-trash"></i>
                      </button>
                    </div>
                  </div>
                </div>
                <div v-else class="text-center py-4">
                  <i class="fas fa-tools fa-3x text-muted mb-3"></i>
                  <h4>No tienes servicios registrados</h4>
                  <p class="text-muted">Agrega tus servicios para que los clientes puedan encontrarte</p>
                  <button class="btn btn-primary" @click="showAddService = true">
                    Agregar mi primer servicio
                  </button>
                </div>
              </div>
            </div>
          </div>

          <!-- Reservas -->
          <div v-if="activeTab === 'bookings'" class="tab-content">
            <div class="card">
              <div class="card-header">
                <h3>Mis Reservas</h3>
              </div>
              <div class="card-body">
                <div v-if="reservas.length > 0" class="bookings-list">
                  <div v-for="reserva in reservas" :key="reserva.id" class="booking-item">
                    <div class="booking-info">
                      <h5>{{ reserva.cliente?.nombre }} {{ reserva.cliente?.apellido }}</h5>
                      <p>{{ reserva.servicio?.nombre }}</p>
                      <div class="booking-meta">
                        <span class="date">{{ formatDate(reserva.fecha) }}</span>
                        <span class="status" :class="reserva.estado">{{ reserva.estado }}</span>
                      </div>
                    </div>
                    <div class="booking-actions">
                      <button class="btn btn-sm btn-primary" @click="viewBooking(reserva)">
                        Ver detalles
                      </button>
                    </div>
                  </div>
                </div>
                <div v-else class="text-center py-4">
                  <i class="fas fa-calendar fa-3x text-muted mb-3"></i>
                  <h4>No tienes reservas</h4>
                  <p class="text-muted">Las reservas de tus clientes aparecerán aquí</p>
                </div>
              </div>
            </div>
          </div>

          <!-- Reseñas -->
          <div v-if="activeTab === 'reviews'" class="tab-content">
            <div class="card">
              <div class="card-header">
                <h3>Reseñas de mis clientes</h3>
              </div>
              <div class="card-body">
                <div v-if="resenas.length > 0" class="reviews-list">
                  <div v-for="resena in resenas" :key="resena.id" class="review-item">
                    <div class="review-header">
                      <div class="reviewer-info">
                        <div class="reviewer-avatar">
                          {{ resena.cliente?.nombre?.charAt(0) }}
                        </div>
                        <div>
                          <strong>{{ resena.cliente?.nombre }} {{ resena.cliente?.apellido }}</strong>
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
                  <i class="fas fa-star fa-3x text-muted mb-3"></i>
                  <h4>No tienes reseñas</h4>
                  <p class="text-muted">Las reseñas de tus clientes aparecerán aquí</p>
                </div>
              </div>
            </div>
          </div>

          <!-- Configuración -->
          <div v-if="activeTab === 'settings'" class="tab-content">
            <div class="card">
              <div class="card-header">
                <h3>Configuración</h3>
              </div>
              <div class="card-body">
                <div class="settings-section">
                  <h4>Notificaciones</h4>
                  <div class="form-check">
                    <input v-model="settings.emailNotifications" type="checkbox" class="form-check-input">
                    <label class="form-check-label">Recibir notificaciones por email</label>
                  </div>
                  <div class="form-check">
                    <input v-model="settings.smsNotifications" type="checkbox" class="form-check-input">
                    <label class="form-check-label">Recibir notificaciones por SMS</label>
                  </div>
                </div>
                
                <div class="settings-section">
                  <h4>Privacidad</h4>
                  <div class="form-check">
                    <input v-model="settings.profilePublic" type="checkbox" class="form-check-input">
                    <label class="form-check-label">Perfil público</label>
                  </div>
                </div>
                
                <button class="btn btn-primary" @click="saveSettings">
                  Guardar configuración
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, reactive, onMounted } from 'vue'
import { useAuth } from '../composables/useApi'

export default {
  name: 'UserDashboard',
  setup() {
    const { user, loading } = useAuth()
    
    const activeTab = ref('profile')
    const showAddService = ref(false)
    
    const profileForm = reactive({
      nombre: '',
      apellido: '',
      email: '',
      telefono: '',
      direccion: '',
      ciudad: '',
      provincia: ''
    })
    
    const servicios = ref([])
    const reservas = ref([])
    const resenas = ref([])
    
    const settings = reactive({
      emailNotifications: true,
      smsNotifications: false,
      profilePublic: true
    })

    onMounted(async () => {
      if (user.value) {
        // Cargar datos del usuario
        Object.assign(profileForm, user.value)
        await loadServicios()
        await loadReservas()
        await loadResenas()
      }
    })

    const loadServicios = async () => {
      // Simular carga de servicios
      servicios.value = []
    }

    const loadReservas = async () => {
      // Simular carga de reservas
      reservas.value = []
    }

    const loadResenas = async () => {
      // Simular carga de reseñas
      resenas.value = []
    }

    const updateProfile = async () => {
      // Implementar actualización de perfil
      console.log('Updating profile:', profileForm)
    }

    const editService = (servicio) => {
      console.log('Edit service:', servicio)
    }

    const deleteService = (id) => {
      console.log('Delete service:', id)
    }

    const viewBooking = (reserva) => {
      console.log('View booking:', reserva)
    }

    const saveSettings = () => {
      console.log('Save settings:', settings)
    }

    const getOficioName = (oficioId) => {
      // Simular obtención del nombre del oficio
      return 'Servicio'
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

    return {
      user,
      loading,
      activeTab,
      showAddService,
      profileForm,
      servicios,
      reservas,
      resenas,
      settings,
      updateProfile,
      editService,
      deleteService,
      viewBooking,
      saveSettings,
      getOficioName,
      generateStars,
      formatDate
    }
  }
}
</script>

<style scoped>
.dashboard-page {
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

.card-header {
  padding: 1.5rem 1.5rem 0 1.5rem;
  border-bottom: 1px solid #e5e7eb;
  margin-bottom: 1.5rem;
}

.card-body {
  padding: 1.5rem;
}

.user-info {
  text-align: center;
  margin-bottom: 2rem;
}

.user-avatar {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  background: linear-gradient(135deg, #22c55e, #06b6d4);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 1rem;
}

.dashboard-nav {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.dashboard-nav a {
  padding: 12px 16px;
  border-radius: 12px;
  color: #6b7280;
  text-decoration: none;
  transition: all 0.2s;
  display: flex;
  align-items: center;
}

.dashboard-nav a:hover {
  background: #f8fafc;
  color: #22c55e;
}

.dashboard-nav a.active {
  background: #22c55e;
  color: white;
}

.tab-content {
  animation: fadeIn 0.3s ease-in;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}

.form-group {
  margin-bottom: 1rem;
}

.form-control {
  padding: 12px 16px;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  font-size: 14px;
  width: 100%;
}

.form-control:focus {
  outline: none;
  border-color: #22c55e;
  box-shadow: 0 0 0 3px rgba(34,197,94,0.1);
}

.form-check {
  margin-bottom: 1rem;
}

.form-check-input {
  margin-right: 0.5rem;
}

.btn {
  padding: 12px 24px;
  border-radius: 12px;
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

.btn-outline-danger {
  background: transparent;
  border: 1px solid #ef4444;
  color: #ef4444;
}

.btn-outline-danger:hover {
  background: #ef4444;
  color: white;
}

.btn-sm {
  padding: 8px 16px;
  font-size: 0.875em;
}

.services-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.service-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  background: #f8fafc;
}

.service-info h5 {
  margin: 0 0 0.5rem 0;
  font-weight: 600;
}

.service-meta {
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

.service-actions {
  display: flex;
  gap: 0.5rem;
}

.bookings-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.booking-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  background: #f8fafc;
}

.booking-meta {
  display: flex;
  gap: 1rem;
  margin-top: 0.5rem;
}

.status {
  padding: 4px 8px;
  border-radius: 8px;
  font-size: 0.875em;
  font-weight: 600;
}

.status.pendiente {
  background: #fef3c7;
  color: #92400e;
}

.status.confirmada {
  background: #d1fae5;
  color: #065f46;
}

.status.cancelada {
  background: #fee2e2;
  color: #991b1b;
}

.reviews-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.review-item {
  padding: 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
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

.stars {
  color: #f59e0b;
  font-size: 1.2em;
}

.settings-section {
  margin-bottom: 2rem;
  padding-bottom: 1.5rem;
  border-bottom: 1px solid #e5e7eb;
}

.settings-section:last-child {
  border-bottom: none;
  margin-bottom: 0;
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

.col-md-3 {
  flex: 0 0 25%;
  max-width: 25%;
}

.col-md-6 {
  flex: 0 0 50%;
  max-width: 50%;
}

.col-md-9 {
  flex: 0 0 75%;
  max-width: 75%;
}

.d-flex {
  display: flex;
}

.justify-content-between {
  justify-content: space-between;
}

.align-items-center {
  align-items: center;
}

.text-center {
  text-align: center;
}

.py-4 {
  padding-top: 1.5rem;
  padding-bottom: 1.5rem;
}

.text-muted {
  color: #6b7280;
}

.mb-3 {
  margin-bottom: 1rem;
}

.me-2 {
  margin-right: 0.5rem;
}

/* Responsive */
@media (max-width: 768px) {
  .row {
    flex-direction: column;
  }
  
  .col-md-3,
  .col-md-9 {
    flex: 0 0 100%;
    max-width: 100%;
  }
  
  .service-item,
  .booking-item {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }
  
  .service-actions {
    width: 100%;
    justify-content: flex-end;
  }
}
</style>
