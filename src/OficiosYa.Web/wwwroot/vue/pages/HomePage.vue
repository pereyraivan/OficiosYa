<template>
  <div>
    <!-- Hero Section -->
    <section class="hero" id="inicio">
      <div class="container hero-wrap">
        <div>
          <h1>Encontrá al profesional que necesitás, <br> en tu zona</h1>
          <p>Electricistas, plomeros, albañiles, gasistas, jardineros y mucho más. Compará perfiles, reseñas y contactá directo.</p>
          
          <!-- Búsqueda dinámica -->
          <form class="search" @submit.prevent="handleSearch">
            <div class="chip">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M3 7h18M3 12h18M3 17h18" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
              </svg>
              <select v-model="searchFilters.oficio" style="border:0;outline:none;width:100%;background:transparent">
                <option value="">Todos los oficios</option>
                <option v-for="oficio in oficios" :key="oficio.id" :value="oficio.id">
                  {{ oficio.nombre }}
                </option>
              </select>
            </div>
            <div class="chip">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M12 21s8-6.1 8-12a8 8 0 1 0-16 0c0 5.9 8 12 8 12Z" stroke="currentColor" stroke-width="2"/>
                <circle cx="12" cy="9" r="3" stroke="currentColor" stroke-width="2"/>
              </svg>
              <input 
                v-model="searchFilters.ciudad" 
                placeholder="Ej: San Miguel de Tucumán" 
                style="border:0;outline:none;width:100%;background:transparent" 
              />
            </div>
            <button type="submit">Buscar</button>
          </form>
          
          <div style="margin-top:14px" class="small">
            Tip: pronto podrás <b>dibujar tu zona</b> en el mapa para resultados más precisos.
          </div>
        </div>
        
        <!-- Profesionales destacados -->
        <div class="hero-card">
          <span class="badge">Nuevos perfiles verificados</span>
          <h3 style="margin-top:10px">Profesionales cerca tuyo</h3>
          <div v-if="loading" class="text-center py-3">
            <div class="spinner"></div>
          </div>
          <div v-else-if="destacados.length > 0" style="display:grid;grid-template-columns:repeat(2,1fr);gap:12px">
            <div v-for="prof in destacados.slice(0, 4)" :key="prof.id" class="pro">
              <div class="pro-head">
                <div class="avatar"></div>
                <div>
                  <strong>{{ prof.usuario?.nombre }} {{ prof.usuario?.apellido }}</strong>
                  <div class="small">{{ getOficioName(prof.profesionalOficios?.[0]?.oficioId) }} — {{ prof.usuario?.ciudad }}</div>
                </div>
              </div>
              <div class="small" style="margin-top:8px">
                <span class="stars">{{ generateStars(prof.calificacionPromedio) }}</span> 
                ({{ prof.totalResenas }} reseñas)
              </div>
            </div>
          </div>
          <div v-else class="text-center py-3">
            <p class="small text-muted">No hay profesionales destacados disponibles</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Categorías -->
    <section class="section" id="categorias">
      <div class="container">
        <h2>Explorá categorías populares</h2>
        <p class="muted">Elegí una categoría para ver profesionales cerca de tu ubicación.</p>
        <div class="grid" style="margin-top:16px">
          <div 
            v-for="oficio in oficios" 
            :key="oficio.id" 
            class="card cat" 
            @click="filterByOficio(oficio.id)"
          >
            <div class="cat-icon">{{ oficio.icono || '🔧' }}</div>
            <div>
              <strong>{{ oficio.nombre }}</strong>
              <div class="small">{{ oficio.descripcion }}</div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Cómo funciona -->
    <section class="section" id="como-funciona" style="background:#f8fafc">
      <div class="container">
        <h2>¿Cómo funciona?</h2>
        <div class="steps" style="margin-top:12px">
          <div class="step">
            <div class="badge" style="background:#e0f2fe;color:#075985">Paso 1</div>
            <h3>Buscá por oficio y zona</h3>
            <p class="muted">Usá el buscador o elegí una categoría. Podrás filtrar por distancia, precio y valoración.</p>
          </div>
          <div class="step">
            <div class="badge" style="background:#dcfce7;color:#14532d">Paso 2</div>
            <h3>Compará perfiles verificados</h3>
            <p class="muted">Mirá fotos de trabajos, certificaciones y reseñas reales de clientes.</p>
          </div>
          <div class="step">
            <div class="badge" style="background:#fef9c3;color:#854d0e">Paso 3</div>
            <h3>Contactá y contratá</h3>
            <p class="muted">Enviá un mensaje o pedí presupuesto sin compromiso. ¡Listo!</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Testimonios -->
    <section class="section" style="background:#f8fafc">
      <div class="container">
        <h2>Lo que dicen nuestros usuarios</h2>
        <div class="testis" style="margin-top:16px">
          <blockquote class="testi">
            "Conseguí electricista en 5 minutos y resolvió el mismo día."
            <div class="small" style="margin-top:6px">— Sofía, SM de Tucumán</div>
          </blockquote>
          <blockquote class="testi">
            "Me encantó poder ver reseñas y fotos de trabajos reales."
            <div class="small" style="margin-top:6px">— Martín, Yerba Buena</div>
          </blockquote>
          <blockquote class="testi">
            "Como profesional, recibo pedidos todas las semanas."
            <div class="small" style="margin-top:6px">— Daniela, Plomería</div>
          </blockquote>
        </div>
      </div>
    </section>

    <!-- CTA profesional -->
    <section class="section">
      <div class="container">
        <div class="cta-block">
          <div>
            <h2 style="margin:0 0 6px">¿Sos profesional? Empezá a recibir trabajos</h2>
            <div class="small" style="color: white;">Registrate gratis, subí tus certificaciones y elegí tu zona de trabajo.</div>
          </div>
          <button @click="goToRegister">Crear cuenta de profesional</button>
        </div>
      </div>
    </section>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useOficios } from '../composables/useApi'

export default {
  name: 'HomePage',
  setup() {
    const router = useRouter()
    const { oficios, loading, loadOficios, searchProfessionals } = useOficios()
    
    const destacados = ref([])
    const searchFilters = ref({
      oficio: '',
      ciudad: ''
    })

    onMounted(async () => {
      await loadOficios()
      await loadDestacados()
    })

    const loadDestacados = async () => {
      try {
        const data = await searchProfessionals({ destacado: true })
        destacados.value = data || []
      } catch (error) {
        console.error('Error loading destacados:', error)
      }
    }

    const handleSearch = () => {
      const filters = { ...searchFilters.value }
      if (filters.oficio) filters.oficioId = filters.oficio
      delete filters.oficio
      
      router.push({
        path: '/buscar',
        query: filters
      })
    }

    const filterByOficio = (oficioId) => {
      searchFilters.value.oficio = oficioId
      handleSearch()
    }

    const getOficioName = (oficioId) => {
      const oficio = oficios.value.find(o => o.id === oficioId)
      return oficio?.nombre || 'Profesional'
    }

    const generateStars = (rating) => {
      const stars = Math.round(rating)
      const fullStars = '★'.repeat(stars)
      const emptyStars = '☆'.repeat(5 - stars)
      return fullStars + emptyStars
    }

    const goToRegister = () => {
      window.location.href = '/Profesional/Registrar'
    }

    return {
      oficios,
      destacados,
      searchFilters,
      loading,
      handleSearch,
      filterByOficio,
      getOficioName,
      generateStars,
      goToRegister
    }
  }
}
</script>

<style scoped>
/* Estilos específicos de la página de inicio */
.hero {
  background: radial-gradient(1200px 600px at 70% -10%, #22c55e1a 0, transparent 60%), 
              radial-gradient(900px 500px at -10% -10%, #06b6d41a 0, transparent 60%), 
              #0b1220;
  color: white;
}

.hero-wrap {
  display: grid;
  grid-template-columns: 1.2fr .8fr;
  gap: 40px;
  align-items: center;
  padding: 60px 0;
}

.hero h1 {
  font-size: clamp(28px, 4vw, 46px);
  line-height: 1.08;
  margin: 0 0 12px;
}

.hero p {
  color: #cbd5e1;
  margin: 0 0 26px;
  font-size: clamp(15px, 2.2vw, 18px);
}

.search {
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
  background: #ffffff;
  padding: 10px;
  border-radius: 999px;
  box-shadow: 0 8px 30px rgba(0,0,0,.25);
}

.chip {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 10px 14px;
  border-radius: 999px;
  border: 1px solid #e5e7eb;
  background: #fff;
  flex: 1;
  min-width: 180px;
}

.search button {
  padding: 12px 18px;
  border-radius: 999px;
  background: #22c55e;
  color: #fff;
  border: 0;
  flex: 0 0 auto;
  cursor: pointer;
}

.hero-card {
  background: #ffffff;
  color: #0f172a;
  border-radius: 24px;
  padding: 20px;
  box-shadow: 0 10px 30px rgba(2,6,23,.12);
}

.badge {
  display: inline-block;
  padding: 6px 10px;
  border-radius: 999px;
  background: #10b9811a;
  color: #065f46;
  font-weight: 600;
  font-size: 12px;
}

.section {
  padding: 56px 0;
}

.section h2 {
  font-size: clamp(22px, 3.2vw, 32px);
  margin: 0 0 18px;
}

.muted {
  color: #64748b;
}

.grid {
  display: grid;
  grid-template-columns: repeat(6, 1fr);
  gap: 16px;
}

.card {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 18px;
  padding: 16px;
  box-shadow: 0 10px 30px rgba(2,6,23,.12);
  transition: transform .12s ease;
  cursor: pointer;
}

.card:hover {
  transform: translateY(-2px);
}

.cat {
  display: flex;
  gap: 12px;
  align-items: center;
}

.cat-icon {
  width: 42px;
  height: 42px;
  border-radius: 12px;
  display: grid;
  place-items: center;
  background: #f1f5f9;
}

.steps {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 18px;
}

.step {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  border-radius: 18px;
  padding: 18px;
  box-shadow: 0 10px 30px rgba(2,6,23,.12);
}

.step h3 {
  margin: 10px 0 6px;
}

.pros {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 16px;
}

.pro {
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 18px;
  padding: 16px;
  box-shadow: 0 10px 30px rgba(2,6,23,.12);
}

.pro-head {
  display: flex;
  align-items: center;
  gap: 12px;
}

.avatar {
  width: 44px;
  height: 44px;
  border-radius: 50%;
  background: linear-gradient(135deg, #e2e8f0, #94a3b8);
}

.stars {
  color: #f59e0b;
}

.testis {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 16px;
}

.testi {
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 18px;
  padding: 16px;
  box-shadow: 0 10px 30px rgba(2,6,23,.12);
}

.cta-block {
  background: linear-gradient(135deg, #22c55e, #06b6d4);
  border-radius: 24px;
  padding: 28px;
  color: white;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 18px;
}

.cta-block button {
  background: #0f172a;
  color: #fff;
  border: 0;
  border-radius: 999px;
  padding: 12px 18px;
  cursor: pointer;
}

.spinner {
  width: 20px;
  height: 20px;
  border: 2px solid #f3f3f3;
  border-top: 2px solid #22c55e;
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

.small {
  font-size: 13px;
  color: #94a3b8;
}

/* Responsive */
@media (max-width: 1000px) {
  .hero-wrap {
    grid-template-columns: 1fr;
  }
  .grid {
    grid-template-columns: repeat(3, 1fr);
  }
  .pros {
    grid-template-columns: repeat(2, 1fr);
  }
  .steps {
    grid-template-columns: 1fr;
  }
  .testis {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 600px) {
  .grid {
    grid-template-columns: repeat(2, 1fr);
  }
  .pros {
    grid-template-columns: 1fr;
  }
  .search {
    border-radius: 18px;
  }
}
</style>
