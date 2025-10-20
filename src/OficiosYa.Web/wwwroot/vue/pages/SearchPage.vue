<template>
  <div class="search-page">
    <div class="container" style="padding: 40px 0;">
      <div class="row">
        <!-- Filtros de búsqueda -->
        <div class="col-md-4">
          <div class="card" style="position: sticky; top: 100px;">
            <div class="card-body">
              <h4>Filtros de búsqueda</h4>
              <form @submit.prevent="search">
                <div class="form-group">
                  <label for="oficio">Oficio</label>
                  <select v-model="filters.oficioId" class="form-control">
                    <option value="">Todos los oficios</option>
                    <option v-for="oficio in oficios" :key="oficio.id" :value="oficio.id">
                      {{ oficio.nombre }}
                    </option>
                  </select>
                </div>
                
                <div class="form-group">
                  <label for="ciudad">Ciudad</label>
                  <input 
                    v-model="filters.ciudad" 
                    type="text" 
                    class="form-control" 
                    placeholder="Ej: San Miguel de Tucumán"
                  >
                </div>
                
                <div class="form-group">
                  <label for="precioMin">Precio mínimo por hora</label>
                  <input 
                    v-model.number="filters.precioMin" 
                    type="number" 
                    class="form-control" 
                    placeholder="0" 
                    min="0"
                  >
                </div>
                
                <div class="form-group">
                  <label for="precioMax">Precio máximo por hora</label>
                  <input 
                    v-model.number="filters.precioMax" 
                    type="number" 
                    class="form-control" 
                    placeholder="10000" 
                    min="0"
                  >
                </div>
                
                <div class="form-group">
                  <label for="calificacionMin">Calificación mínima</label>
                  <select v-model="filters.calificacionMin" class="form-control">
                    <option value="">Cualquier calificación</option>
                    <option value="4">4+ estrellas</option>
                    <option value="4.5">4.5+ estrellas</option>
                    <option value="5">5 estrellas</option>
                  </select>
                </div>
                
                <div class="form-group">
                  <div class="form-check">
                    <input 
                      v-model="filters.disponible" 
                      type="checkbox" 
                      class="form-check-input"
                    >
                    <label class="form-check-label">Solo disponibles ahora</label>
                  </div>
                </div>
                
                <button type="submit" class="btn btn-primary" :disabled="loading" style="width: 100%;">
                  {{ loading ? 'Buscando...' : 'Buscar' }}
                </button>
              </form>
            </div>
          </div>
        </div>
        
        <!-- Resultados -->
        <div class="col-md-8">
          <div class="d-flex justify-content-between align-items-center mb-4">
            <h2>Profesionales encontrados</h2>
            <div class="small text-muted">
              {{ resultados.length }} resultados
            </div>
          </div>
          
          <!-- Loading -->
          <div v-if="loading" class="text-center py-5">
            <div class="spinner"></div>
            <p class="mt-3">Buscando profesionales...</p>
          </div>
          
          <!-- Error -->
          <div v-else-if="error" class="alert alert-danger">
            <h4>Error</h4>
            <p>{{ error }}</p>
          </div>
          
          <!-- Sin resultados -->
          <div v-else-if="resultados.length === 0" class="text-center py-5">
            <i class="fas fa-search fa-3x text-muted mb-3"></i>
            <h4>No se encontraron profesionales</h4>
            <p class="text-muted">Intenta ajustar los filtros de búsqueda.</p>
          </div>
          
          <!-- Resultados -->
          <div v-else class="row">
            <div v-for="profesional in resultados" :key="profesional.id" class="col-md-6 col-lg-4 mb-4">
              <div class="card h-100">
                <div v-if="profesional.fotoPerfil" class="card-img-top" style="height: 200px; background-image: url(profesional.fotoPerfil); background-size: cover; background-position: center;"></div>
                <div v-else class="card-img-top bg-light d-flex align-items-center justify-content-center" style="height: 200px;">
                  <i class="fas fa-user fa-3x text-muted"></i>
                </div>
                
                <div class="card-body d-flex flex-column">
                  <h5 class="card-title">{{ profesional.usuario?.nombre }} {{ profesional.usuario?.apellido }}</h5>
                  <p class="card-text text-muted">{{ getOficiosNames(profesional.profesionalOficios) }}</p>
                  <p class="card-text">{{ profesional.descripcion || 'Sin descripción' }}</p>
                  
                  <div class="mt-auto">
                    <div class="d-flex justify-content-between align-items-center mb-2">
                      <div class="rating">
                        <span class="stars">{{ generateStars(profesional.calificacionPromedio) }}</span>
                        <span class="ms-1">({{ profesional.totalResenas }})</span>
                      </div>
                      <span class="badge bg-success">${{ getPrecioPromedio(profesional.profesionalOficios) }}/h</span>
                    </div>
                    
                    <div class="d-flex gap-2">
                      <button 
                        class="btn btn-primary btn-sm flex-fill" 
                        @click="verPerfil(profesional.id)"
                      >
                        Ver Perfil
                      </button>
                      <button 
                        class="btn btn-outline-primary btn-sm flex-fill" 
                        @click="contactar(profesional.id)"
                      >
                        Contactar
                      </button>
                    </div>
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
import { ref, reactive, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useOficios } from '../composables/useApi'

export default {
  name: 'SearchPage',
  setup() {
    const route = useRoute()
    const router = useRouter()
    const { oficios, loading, error, loadOficios, searchProfessionals } = useOficios()
    
    const resultados = ref([])
    const filters = reactive({
      oficioId: '',
      ciudad: '',
      precioMin: null,
      precioMax: null,
      calificacionMin: '',
      disponible: false
    })

    onMounted(async () => {
      await loadOficios()
      
      // Aplicar filtros de la URL
      if (route.query.oficioId) filters.oficioId = route.query.oficioId
      if (route.query.ciudad) filters.ciudad = route.query.ciudad
      if (route.query.precioMin) filters.precioMin = parseInt(route.query.precioMin)
      if (route.query.precioMax) filters.precioMax = parseInt(route.query.precioMax)
      if (route.query.calificacionMin) filters.calificacionMin = route.query.calificacionMin
      if (route.query.disponible) filters.disponible = route.query.disponible === 'true'
      
      // Realizar búsqueda inicial
      await search()
    })

    const search = async () => {
      try {
        // Limpiar filtros vacíos
        const cleanFilters = Object.fromEntries(
          Object.entries(filters).filter(([_, value]) => 
            value !== '' && value !== null && value !== undefined
          )
        )
        
        const data = await searchProfessionals(cleanFilters)
        resultados.value = data || []
        
        // Actualizar URL
        router.replace({
          query: cleanFilters
        })
      } catch (err) {
        console.error('Search error:', err)
      }
    }

    const getOficiosNames = (profesionalOficios) => {
      if (!profesionalOficios || !oficios.value) return 'Sin oficios'
      
      return profesionalOficios
        .map(po => {
          const oficio = oficios.value.find(o => o.id === po.oficioId)
          return oficio?.nombre
        })
        .filter(Boolean)
        .join(', ')
    }

    const getPrecioPromedio = (profesionalOficios) => {
      if (!profesionalOficios || profesionalOficios.length === 0) return '0'
      
      const promedio = profesionalOficios.reduce((sum, po) => sum + po.precioHora, 0) / profesionalOficios.length
      return promedio.toFixed(2)
    }

    const generateStars = (calificacion) => {
      const stars = Math.round(calificacion)
      const fullStars = '★'.repeat(stars)
      const emptyStars = '☆'.repeat(5 - stars)
      return fullStars + emptyStars
    }

    const verPerfil = (profesionalId) => {
      router.push(`/profesional/${profesionalId}`)
    }

    const contactar = (profesionalId) => {
      // Implementar lógica de contacto
      alert(`Contactando al profesional ${profesionalId}`)
    }

    return {
      oficios,
      resultados,
      filters,
      loading,
      error,
      search,
      getOficiosNames,
      getPrecioPromedio,
      generateStars,
      verPerfil,
      contactar
    }
  }
}
</script>

<style scoped>
.search-page {
  min-height: 100vh;
  background: #f8fafc;
}

.card {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 18px;
  box-shadow: 0 10px 30px rgba(2,6,23,.12);
  transition: transform .12s ease;
}

.card:hover {
  transform: translateY(-2px);
}

.form-group {
  margin-bottom: 16px;
}

.form-control {
  padding: 12px 16px;
  border: 1px solid #e5e7eb;
  border-radius: 18px;
  font-size: 14px;
  width: 100%;
}

.form-control:focus {
  outline: none;
  border-color: #22c55e;
  box-shadow: 0 0 0 3px rgba(34,197,94,0.1);
}

.btn {
  padding: 12px 24px;
  border-radius: 18px;
  border: none;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.2s;
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
  border: 1px solid #e5e7eb;
  color: #0f172a;
}

.btn-outline-primary:hover {
  background: #f8fafc;
}

.stars {
  color: #f59e0b;
}

.badge {
  padding: 4px 8px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: 600;
}

.bg-success {
  background: #22c55e !important;
  color: white;
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
  gap: 20px;
}

.col-md-4 {
  flex: 0 0 33.333333%;
  max-width: 33.333333%;
}

.col-md-8 {
  flex: 0 0 66.666667%;
  max-width: 66.666667%;
}

.col-lg-4 {
  flex: 0 0 33.333333%;
  max-width: 33.333333%;
}

.col-md-6 {
  flex: 0 0 50%;
  max-width: 50%;
}

.mb-4 {
  margin-bottom: 1.5rem;
}

.mb-2 {
  margin-bottom: 0.5rem;
}

.mt-3 {
  margin-top: 1rem;
}

.mt-auto {
  margin-top: auto;
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

.py-5 {
  padding-top: 3rem;
  padding-bottom: 3rem;
}

.py-3 {
  padding-top: 1rem;
  padding-bottom: 1rem;
}

.card-body {
  padding: 1.25rem;
}

.card-img-top {
  border-top-left-radius: 18px;
  border-top-right-radius: 18px;
}

.h-100 {
  height: 100%;
}

.flex-fill {
  flex: 1 1 auto;
}

.gap-2 {
  gap: 0.5rem;
}

.small {
  font-size: 13px;
  color: #94a3b8;
}

.text-muted {
  color: #6b7280;
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
  
  .col-md-4,
  .col-md-8 {
    flex: 0 0 100%;
    max-width: 100%;
  }
  
  .col-md-6 {
    flex: 0 0 100%;
    max-width: 100%;
  }
}
</style>
