<template>
  <div class="home">
    <!-- Header -->
    <header>
      <div class="container nav">
        <div class="logo" @click="irAInicio" style="cursor: pointer;">
          <div class="logo-badge">🛠️</div>
          <span>OficiosYa</span>
        </div>
        
        <!-- Menú hamburguesa para móviles -->
        <button 
          class="menu-toggle" 
          type="button" 
          @click="toggleMenu"
          :class="{ 'active': menuOpen }"
        >
          <span></span>
          <span></span>
          <span></span>
        </button>
        
        <!-- Navegación -->
        <nav class="navlinks" :class="{ 'active': menuOpen }">
          <a href="#inicio" @click="closeMenu">Inicio</a>
          <a href="#" @click="cambiarTipoBusqueda('oficiante'); closeMenu()" :class="{ 'active': tipoBusqueda === 'oficiante' }">Oficiantes</a>
          <a href="#" @click="cambiarTipoBusqueda('profesional'); closeMenu()" :class="{ 'active': tipoBusqueda === 'profesional' }">Profesionales</a>
          <a href="#como-funciona" @click="closeMenu">Cómo funciona</a>
          <a href="#" @click="closeMenu">Ayuda</a>
          <button class="cta" @click="irARegistro(); closeMenu()">Registrate</button>
        </nav>
      </div>
    </header>

    <!-- Hero Section -->
    <section class="hero bg-primary text-white py-5" id="inicio">
      <div class="container">
        <div class="row align-items-center min-vh-50">
          <div class="col-lg-6">
            <h1 class="display-4 fw-bold mb-4">
              Encontrá al {{ tipoBusqueda === 'oficiante' ? 'oficiante' : 'profesional' }} que necesitás, <br> en tu zona
            </h1>
            <p class="lead mb-4" v-if="tipoBusqueda === 'oficiante'">
              Electricistas, plomeros, albañiles, gasistas, jardineros y mucho más. Compará perfiles, reseñas y contactá directo.
            </p>
            <p class="lead mb-4" v-else>
              Abogados, contadores, médicos, psicólogos y otros profesionales especializados. Compará perfiles, reseñas y contactá directo.
            </p>
            
            <form @submit.prevent="buscarProfesionales" class="search">
              <div class="chip">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path d="M3 7h18M3 12h18M3 17h18" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
                </svg>
                <select v-model="busqueda.oficio" style="border:0;outline:none;width:100%;background:transparent">
                  <option v-for="opcion in opcionesDropdown" :key="opcion" :value="opcion">
                    {{ opcion }}
                  </option>
                </select>
              </div>
              <div class="chip">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <path d="M12 21s8-6.1 8-12a8 8 0 1 0-16 0c0 5.9 8 12 8 12Z" stroke="currentColor" stroke-width="2"/>
                  <circle cx="12" cy="9" r="3" stroke="currentColor" stroke-width="2"/>
                </svg>
                <input 
                  v-model="busqueda.zona" 
                  type="text" 
                  placeholder="Tu zona (ej: Yerba Buena)"
                  style="border:0;outline:none;width:100%;background:transparent"
                >
              </div>
              <button type="submit" class="btn btn-light">
                Buscar
              </button>
            </form>
            
            <div class="mt-3 small">
              <strong>Tip:</strong> pronto podrás <b>dibujar tu zona</b> en el mapa para resultados más precisos.
            </div>
          </div>
          <div class="col-lg-6 text-center">
            <div class="hero-card">
              <span class="badge mb-3">Nuevos perfiles verificados</span>
              <h3 class="mb-3">Profesionales cerca tuyo</h3>
              <div class="row g-3">
                <div v-for="profesional in profesionalesDestacados" :key="profesional.id" class="col-6">
                  <div class="d-flex align-items-center">
                    <div class="avatar me-2"></div>
                    <div>
                      <strong class="small">{{ profesional.usuario.nombre }}</strong>
                      <div class="small text-muted">San Miguel de Tucumán</div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Categorías -->
    <section class="section py-5 bg-light" id="categorias">
      <div class="container">
        <h2 class="text-center mb-3">Explorá categorías populares</h2>
        <p class="text-center text-muted mb-5">Elegí una categoría para ver profesionales cerca de tu ubicación.</p>
        <div class="row g-4">
          <div class="col-lg-3 col-md-4 col-sm-6" v-for="categoria in categorias" :key="categoria.nombre">
            <div class="card cat h-100 text-center">
              <div class="card-body d-flex flex-column align-items-center">
                <div class="cat-icon mb-3">{{ categoria.icono }}</div>
                <h5 class="card-title">{{ categoria.nombre }}</h5>
                <p class="card-text text-muted">{{ categoria.descripcion }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Cómo funciona -->
    <section class="section py-5" id="como-funciona" style="background:#f8fafc">
      <div class="container">
        <h2 class="text-center mb-5">¿Cómo funciona?</h2>
        <div class="row g-4">
          <div class="col-lg-4 col-md-6" v-for="paso in pasos" :key="paso.numero">
            <div class="step text-center">
              <div class="step-badge" :style="paso.badgeStyle">{{ paso.numero }}</div>
              <h4 class="mb-3">{{ paso.titulo }}</h4>
              <p class="text-muted">{{ paso.descripcion }}</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Profesionales destacados -->
    <section class="section py-5">
      <div class="container">
        <h2 class="text-center mb-3">Profesionales destacados</h2>
        <p class="text-center text-muted mb-5">Perfiles completos, con certificaciones y excelentes reseñas.</p>
        <div class="row g-4">
          <div class="col-lg-4 col-md-6" v-for="profesional in profesionalesDestacados" :key="profesional.id">
            <div class="pro card h-100">
              <div class="card-body">
                <div class="pro-head d-flex align-items-center mb-3">
                  <div class="avatar me-3"></div>
                  <div>
                    <h6 class="card-title mb-1">{{ profesional.usuario.nombre }}</h6>
                    <small class="text-muted">San Miguel de Tucumán · Tucumán</small>
                  </div>
                </div>
                <div class="mb-2">
                  <span class="stars">★★★★★</span> 
                  <span class="ms-1">{{ profesional.calificacionPromedio.toFixed(1) }} ({{ profesional.resenas.length }})</span>
                </div>
                <div class="small text-muted">
                  Certificaciones: {{ profesional.certificados.length }}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Testimonios -->
    <section class="section py-5 bg-light">
      <div class="container">
        <h2 class="text-center mb-5">Lo que dicen nuestros usuarios</h2>
        <div class="row g-4">
          <div class="col-lg-4 col-md-6" v-for="testimonio in testimonios" :key="testimonio.id">
            <div class="testi card h-100">
              <div class="card-body">
                <blockquote class="blockquote mb-0">
                  <p class="mb-3">"{{ testimonio.comentario }}"</p>
                  <footer class="blockquote-footer">
                    <cite title="Source Title">{{ testimonio.autor }}</cite>
                  </footer>
                </blockquote>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- CTA profesional -->
    <section class="section py-5">
      <div class="container">
        <div class="cta-block">
          <div>
            <h2 style="margin:0 0 6px">¿Sos profesional? Empezá a recibir trabajos</h2>
            <div class="small" style="color: white;">Registrate gratis, subí tus certificaciones y elegí tu zona de trabajo.</div>
          </div>
          <button class="btn btn-light btn-lg" @click="irARegistro">
            Crear cuenta de profesional
          </button>
        </div>
      </div>
    </section>

    <!-- Footer -->
    <footer>
      <div class="container footer">
        <div>
          <div class="logo" style="color:white; cursor: pointer;" @click="irAInicio">
            <div class="logo-badge">🛠️</div>
            <span>OficiosYa</span>
          </div>
          <p class="small" style="margin-top:8px">Conectamos personas con profesionales de confianza en su zona.</p>
        </div>
        <div>
          <strong>Explorar</strong>
          <div class="small" style="margin-top:8px"><a href="#" @click="irAOficiantes" style="color:inherit">Oficiantes</a></div>
          <div class="small"><a href="#" style="color:inherit">Profesionales</a></div>
        </div>
        <div>
          <strong>Ayuda</strong>
          <div class="small" style="margin-top:8px"><a href="#" style="color:inherit">Centro de ayuda</a></div>
          <div class="small"><a href="#" style="color:inherit">Contacto</a></div>
        </div>
        <div>
          <strong>Legal</strong>
          <div class="small" style="margin-top:8px"><a href="#" style="color:inherit">Términos</a></div>
          <div class="small"><a href="#" style="color:inherit">Privacidad</a></div>
        </div>
      </div>
      <div class="container" style="padding:12px 0;border-top:1px solid #1f2937;color:#94a3b8">
        <div class="small">© 2025 OficiosYa</div>
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { profesionalService } from '../services/profesionalService'
import { CATEGORIAS_SERVICIO } from '../utils/constants'
import type { Profesional } from '../types'

const router = useRouter()

// Estado reactivo
const busqueda = ref({
  oficio: 'Electricista',
  zona: ''
})

const profesionalesDestacados = ref<Profesional[]>([])
const menuOpen = ref(false)
const tipoBusqueda = ref<'oficiante' | 'profesional'>('oficiante')

// Opciones del dropdown según el tipo de búsqueda
const opcionesDropdown = computed(() => {
  if (tipoBusqueda.value === 'oficiante') {
    return CATEGORIAS_SERVICIO.OFICIANTE.map(c => c.nombre)
  } else {
    return CATEGORIAS_SERVICIO.PROFESIONAL.map(c => c.nombre)
  }
})

// Datos estáticos
const categorias = ref([
  { nombre: 'Electricista', descripcion: 'Instalaciones y urgencias', icono: '⚡' },
  { nombre: 'Plomería', descripcion: 'Pérdidas, caños, bombas', icono: '🚰' },
  { nombre: 'Construcción', descripcion: 'Obras y refacciones', icono: '🧱' },
  { nombre: 'Gasista', descripcion: 'Habilitaciones y reparaciones', icono: '🔥' },
  { nombre: 'Pintura', descripcion: 'Interior y exterior', icono: '🎨' },
  { nombre: 'Jardinería', descripcion: 'Mantenimiento y riego', icono: '🌿' },
  { nombre: 'Cerrajería', descripcion: 'Aperturas y copias', icono: '🔒' },
  { nombre: 'Limpieza', descripcion: 'Hogar y oficinas', icono: '🧹' }
])

const pasos = ref([
  {
    numero: 1,
    titulo: 'Paso 1',
    descripcion: 'Usá el buscador o elegí una categoría. Podrás filtrar por distancia, precio y valoración.',
    badgeStyle: 'background:#e0f2fe;color:#075985'
  },
  {
    numero: 2,
    titulo: 'Paso 2',
    descripcion: 'Mirá fotos de trabajos, certificaciones y reseñas reales de clientes.',
    badgeStyle: 'background:#dcfce7;color:#14532d'
  },
  {
    numero: 3,
    titulo: 'Paso 3',
    descripcion: 'Enviá un mensaje o pedí presupuesto sin compromiso. ¡Listo!',
    badgeStyle: 'background:#fef9c3;color:#854d0e'
  }
])

const testimonios = ref([
  {
    id: 1,
    comentario: 'Conseguí electricista en 5 minutos y resolvió el mismo día.',
    autor: 'Sofía, SM de Tucumán'
  },
  {
    id: 2,
    comentario: 'Me encantó poder ver reseñas y fotos de trabajos reales.',
    autor: 'Martín, Yerba Buena'
  },
  {
    id: 3,
    comentario: 'Como profesional, recibo pedidos todas las semanas.',
    autor: 'Daniela, Plomería'
  }
])

// Métodos
const buscarProfesionales = async () => {
  try {
    const resultados = await profesionalService.buscar({
      oficio: busqueda.value.oficio,
      zona: busqueda.value.zona
    })
    console.log('Resultados de búsqueda:', resultados)
    // Aquí podrías navegar a una página de resultados o mostrar un modal
  } catch (error) {
    console.error('Error en búsqueda:', error)
  }
}

const irARegistro = () => {
  router.push('/registrar-profesional')
}

const irAInicio = () => {
  document.getElementById('inicio')?.scrollIntoView({ behavior: 'smooth' })
}

const irAOficiantes = () => {
  router.push('/oficiantes')
}

const toggleMenu = () => {
  menuOpen.value = !menuOpen.value
}

const closeMenu = () => {
  menuOpen.value = false
}

const cambiarTipoBusqueda = (tipo: 'oficiante' | 'profesional') => {
  tipoBusqueda.value = tipo
  // Cambiar el oficio seleccionado al primer elemento de la nueva categoría
  busqueda.value.oficio = opcionesDropdown.value[0]
}

const cargarProfesionalesDestacados = async () => {
  try {
    profesionalesDestacados.value = await profesionalService.obtenerDestacados()
  } catch (error) {
    console.error('Error cargando profesionales destacados:', error)
    // Datos de ejemplo si falla la API
    profesionalesDestacados.value = [
      {
        id: 1,
        usuarioId: 1,
        descripcion: 'Electricista matriculada',
        calificacionPromedio: 4.9,
        totalResenas: 0,
        disponible: true,
        fechaRegistro: '2024-01-01',
        activo: true,
        usuario: { id: 1, nombre: 'María López', email: 'maria@email.com', fechaRegistro: '2024-01-01', esProfesional: true, activo: true },
        oficios: [],
        certificados: [],
        resenas: []
      },
      {
        id: 2,
        usuarioId: 2,
        descripcion: 'Plomería y gas',
        calificacionPromedio: 4.6,
        totalResenas: 0,
        disponible: true,
        fechaRegistro: '2024-01-01',
        activo: true,
        usuario: { id: 2, nombre: 'Diego Pérez', email: 'diego@email.com', fechaRegistro: '2024-01-01', esProfesional: true, activo: true },
        oficios: [],
        certificados: [],
        resenas: []
      }
    ]
  }
}

// Lifecycle
onMounted(() => {
  cargarProfesionalesDestacados()
})
</script>

<style scoped>
/* Estilos personalizados que mantienen el diseño original */
.avatar {
  width: 40px;
  height: 40px;
  background: linear-gradient(135deg, #e2e8f0, #94a3b8);
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.min-vh-50 {
  min-height: 50vh;
}

/* Estilos para el menú hamburguesa */
.menu-toggle {
  display: none;
  flex-direction: column;
  justify-content: space-around;
  width: 30px;
  height: 30px;
  background: transparent;
  border: none;
  cursor: pointer;
  padding: 0;
  z-index: 1001;
}

.menu-toggle span {
  width: 100%;
  height: 3px;
  background: white;
  border-radius: 2px;
  transition: all 0.3s ease;
  transform-origin: center;
}

.menu-toggle.active span:nth-child(1) {
  transform: rotate(45deg) translate(6px, 6px);
}

.menu-toggle.active span:nth-child(2) {
  opacity: 0;
}

.menu-toggle.active span:nth-child(3) {
  transform: rotate(-45deg) translate(6px, -6px);
}

/* Navegación responsive */
.navlinks {
  display: flex;
  align-items: center;
  gap: 24px;
}

.navlinks a {
  text-decoration: none;
  color: white;
  font-weight: 500;
  transition: color 0.2s ease;
}

.navlinks a:hover {
  color: var(--brand);
}

.navlinks a.active {
  color: var(--brand);
  font-weight: 600;
}

.navlinks .cta {
  background: var(--brand);
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
}

.navlinks .cta:hover {
  background: #2563eb;
  transform: translateY(-1px);
}

/* Mantener el logo badge original */
.logo-badge {
  width: 34px;
  height: 34px;
  border-radius: 12px;
  background: linear-gradient(135deg, var(--brand), #06b6d4);
  display: grid;
  place-items: center;
  color: white;
  font-weight: 900;
}

/* Mantener el estilo de las estrellas */
.stars {
  color: #f59e0b;
}

/* Mantener el estilo de los pasos */
.step-badge {
  width: 60px;
  height: 60px;
  background: var(--brand);
  color: white;
  border-radius: 50%;
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 1.8rem;
  font-weight: 700;
  margin: 0 auto 20px auto;
}

/* Mantener el estilo de las categorías */
.cat-icon {
  width: 42px;
  height: 42px;
  border-radius: 12px;
  display: grid;
  place-items: center;
  background: #f1f5f9;
  font-size: 1.5rem;
}

/* Mantener el estilo del hero card */
.hero-card {
  background: #ffffff;
  color: var(--text);
  border-radius: var(--radius-lg);
  padding: 20px;
  box-shadow: var(--shadow);
}

/* Mantener el estilo del badge */
.badge {
  display: inline-block;
  padding: 6px 10px;
  border-radius: 999px;
  background: #10b9811a;
  color: #065f46;
  font-weight: 600;
  font-size: 12px;
}

/* Mantener el estilo del CTA block */
.cta-block {
  background: linear-gradient(135deg, #22c55e, #06b6d4);
  border-radius: var(--radius-lg);
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
  transition: background-color 0.2s;
}

.cta-block button:hover {
  background: #1e293b;
}

/* Mantener el estilo del footer */
footer {
  background: var(--bg-soft);
  color: #cbd5e1;
}

.footer {
  display: grid;
  grid-template-columns: 2fr 1fr 1fr 1fr;
  gap: 16px;
  padding: 32px 0;
}

.small {
  font-size: 13px;
  color: #94a3b8;
}

/* Mantener el estilo del header */
header {
  position: sticky;
  top: 0;
  z-index: 10;
  background: var(--bg);
  backdrop-filter: saturate(180%) blur(10px);
  border-bottom: 1px solid #ffffff40;
}

.nav {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 14px 0;
}

.logo {
  display: flex;
  align-items: center;
  gap: 10px;
  font-weight: 800;
  color: white;
}

/* Responsive design */
@media (max-width: 768px) {
  .menu-toggle {
    display: flex;
  }
  
  .navlinks {
    position: fixed;
    top: 0;
    right: -100%;
    width: 100%;
    height: 100vh;
    background: var(--bg);
    flex-direction: column;
    justify-content: center;
    align-items: center;
    gap: 32px;
    transition: right 0.3s ease;
    z-index: 1000;
    box-shadow: -4px 0 20px rgba(0, 0, 0, 0.1);
  }
  
  .navlinks.active {
    right: 0;
  }
  
  .navlinks a {
    font-size: 1.2rem;
    font-weight: 600;
  }
  
  .navlinks .cta {
    font-size: 1.1rem;
    padding: 16px 32px;
  }
  
  /* Ajustes para el header en móviles */
  .nav {
    position: relative;
    z-index: 1002;
  }
  
  .logo {
    z-index: 1003;
  }
}

@media (max-width: 480px) {
  .navlinks {
    gap: 24px;
  }
  
  .navlinks a {
    font-size: 1.1rem;
  }
  
  .navlinks .cta {
    font-size: 1rem;
    padding: 14px 28px;
  }
}
</style>
