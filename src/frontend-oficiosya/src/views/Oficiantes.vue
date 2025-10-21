<template>
  <div class="oficiantes">
    <!-- Header -->
    <header>
      <div class="container nav">
        <div class="logo" @click="irAInicio" style="cursor: pointer;">
          <div class="logo-badge">🛠️</div>
          <span>OficiosYa</span>
        </div>
        
        <!-- Menú hamburguesa para móviles -->
        <button class="menu-toggle" @click="toggleMenu" :class="{ active: menuOpen }">
          <span></span>
          <span></span>
          <span></span>
        </button>
        
        <!-- Navegación -->
        <nav class="navlinks" :class="{ active: menuOpen }">
          <a href="#" @click="irAInicio; closeMenu()">Inicio</a>
          <a href="#" @click="irAOficiantes; closeMenu()" class="active">Oficiantes</a>
          <a href="#" @click="irAProfesionales; closeMenu()">Profesionales</a>
          <a href="#como-funciona" @click="closeMenu">Cómo funciona</a>
          <a href="#" @click="closeMenu">Ayuda</a>
          <button class="cta" @click="irARegistro(); closeMenu()">Registrate</button>
        </nav>
      </div>
    </header>

    <!-- Hero Section -->
    <section class="hero" style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 80px 0;">
      <div class="container">
        <div style="text-align: center; max-width: 800px; margin: 0 auto;">
          <h1 style="font-size: 3rem; margin-bottom: 20px; font-weight: 700;">
            🧰 Oficiantes
          </h1>
          <p style="font-size: 1.2rem; margin-bottom: 30px; opacity: 0.9;">
            Encontrá los mejores oficiantes para trabajos técnicos y manuales en tu zona
          </p>
          <div style="display: flex; gap: 16px; justify-content: center; flex-wrap: wrap;">
            <button class="cta" style="background: white; color: #667eea; border: none;" @click="irARegistro()">
              Registrate como Oficiante
            </button>
            <button class="cta" style="background: transparent; color: white; border: 2px solid white;" @click="buscarOficiantes">
              Buscar Oficiantes
            </button>
          </div>
        </div>
      </div>
    </section>

    <!-- Categorías de Oficiantes -->
    <section class="section" style="padding: 80px 0; background: #f8fafc;">
      <div class="container">
        <div style="text-align: center; margin-bottom: 60px;">
          <h2 style="font-size: 2.5rem; margin-bottom: 20px; color: var(--text);">
            Categorías de Oficiantes
          </h2>
          <p style="font-size: 1.1rem; color: var(--muted); max-width: 600px; margin: 0 auto;">
            Explorá las diferentes especialidades de oficiantes disponibles
          </p>
        </div>

        <div class="categorias-grid">
          <div
            v-for="categoria in CATEGORIAS_SERVICIO.OFICIANTE"
            :key="categoria.nombre"
            class="categoria-card"
            @click="buscarPorCategoria(categoria.nombre)"
          >
            <div class="categoria-icon">{{ categoria.icono }}</div>
            <h3>{{ categoria.nombre }}</h3>
            <p>{{ categoria.descripcion }}</p>
            <div class="categoria-arrow">→</div>
          </div>
        </div>
      </div>
    </section>

    <!-- Cómo funciona -->
    <section class="section" style="padding: 80px 0; background: white;">
      <div class="container">
        <div style="text-align: center; margin-bottom: 60px;">
          <h2 style="font-size: 2.5rem; margin-bottom: 20px; color: var(--text);">
            ¿Cómo funciona?
          </h2>
          <p style="font-size: 1.1rem; color: var(--muted); max-width: 600px; margin: 0 auto;">
            Encontrar el oficiante perfecto es fácil y rápido
          </p>
        </div>

        <div class="pasos-grid">
          <div class="paso-card">
            <div class="paso-numero">1</div>
            <h3>Buscar</h3>
            <p>Encontrá oficiantes en tu zona por categoría o especialidad</p>
          </div>
          <div class="paso-card">
            <div class="paso-numero">2</div>
            <h3>Comparar</h3>
            <p>Revisá perfiles, reseñas y precios de diferentes oficiantes</p>
          </div>
          <div class="paso-card">
            <div class="paso-numero">3</div>
            <h3>Contactar</h3>
            <p>Elegí el mejor oficiante y contactalo directamente</p>
          </div>
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
          <div class="small" style="margin-top:8px"><a href="#" style="color:inherit">Oficiantes</a></div>
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
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { CATEGORIAS_SERVICIO } from '../utils/constants'

const router = useRouter()

// Estado del menú móvil
const menuOpen = ref(false)

// Métodos de navegación
const irAInicio = () => {
  router.push('/')
}

const irAOficiantes = () => {
  router.push('/oficiantes')
}

const irAProfesionales = () => {
  // Por ahora redirigir a inicio, luego crear vista de profesionales
  router.push('/')
}

const irARegistro = () => {
  router.push('/registrar-profesional')
}

const buscarOficiantes = () => {
  // Scroll a la sección de categorías
  const categoriasSection = document.querySelector('.categorias-grid')
  if (categoriasSection) {
    categoriasSection.scrollIntoView({ behavior: 'smooth' })
  }
}

const buscarPorCategoria = (categoria: string) => {
  // Por ahora solo mostrar un alert, luego implementar búsqueda real
  alert(`Buscando oficiantes de: ${categoria}`)
}

const toggleMenu = () => {
  menuOpen.value = !menuOpen.value
}

const closeMenu = () => {
  menuOpen.value = false
}
</script>

<style scoped>
.oficiantes {
  min-height: 100vh;
}

.logo {
  cursor: pointer;
}

.navlinks a.active {
  color: var(--brand);
  font-weight: 600;
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
  background: var(--text);
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
  color: var(--text);
  font-weight: 500;
  transition: color 0.2s ease;
}

.navlinks a:hover {
  color: var(--brand);
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

/* Grid de categorías */
.categorias-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 24px;
  margin-top: 40px;
}

.categoria-card {
  background: white;
  border-radius: 16px;
  padding: 32px;
  text-align: center;
  cursor: pointer;
  transition: all 0.3s ease;
  border: 2px solid transparent;
  position: relative;
  overflow: hidden;
}

.categoria-card:hover {
  transform: translateY(-8px);
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
  border-color: var(--brand);
}

.categoria-card:hover .categoria-arrow {
  transform: translateX(8px);
}

.categoria-icon {
  font-size: 3rem;
  margin-bottom: 16px;
  display: block;
}

.categoria-card h3 {
  font-size: 1.5rem;
  margin-bottom: 12px;
  color: var(--text);
  font-weight: 600;
}

.categoria-card p {
  color: var(--muted);
  line-height: 1.6;
  margin-bottom: 20px;
}

.categoria-arrow {
  position: absolute;
  bottom: 20px;
  right: 20px;
  font-size: 1.5rem;
  color: var(--brand);
  transition: transform 0.3s ease;
}

/* Grid de pasos */
.pasos-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 40px;
  margin-top: 40px;
}

.paso-card {
  text-align: center;
  padding: 32px 20px;
}

.paso-numero {
  width: 60px;
  height: 60px;
  background: var(--brand);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  font-weight: 700;
  margin: 0 auto 20px;
}

.paso-card h3 {
  font-size: 1.3rem;
  margin-bottom: 12px;
  color: var(--text);
  font-weight: 600;
}

.paso-card p {
  color: var(--muted);
  line-height: 1.6;
}

/* Responsive */
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
    background: white;
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
  
  .categorias-grid {
    grid-template-columns: 1fr;
  }
  
  .pasos-grid {
    grid-template-columns: 1fr;
  }
  
  .hero h1 {
    font-size: 2rem !important;
  }
  
  .hero p {
    font-size: 1rem !important;
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
