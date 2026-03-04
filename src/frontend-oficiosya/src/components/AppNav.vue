<template>
  <header class="app-nav-header">
    <div class="container nav">
      <div class="logo" @click="irAInicio" style="cursor: pointer;">
        <div class="logo-badge">🛠️</div>
        <span>OficiosYa</span>
      </div>

      <button
        class="menu-toggle"
        type="button"
        @click="toggleMenu"
        :class="{ active: menuOpen }"
      >
        <span></span>
        <span></span>
        <span></span>
      </button>

      <nav class="navlinks" :class="{ active: menuOpen }">
        <router-link to="/" class="nav-link" :class="{ active: isActiveInicio }">Inicio</router-link>
        <router-link :to="{ path: '/', query: { tipo: 'oficiante' } }" class="nav-link" :class="{ active: isActiveOficiantes }">Oficiantes</router-link>
        <router-link :to="{ path: '/', query: { tipo: 'profesional' } }" class="nav-link" :class="{ active: isActiveProfesionales }">Profesionales</router-link>
        <div class="nav-dropdown" :class="{ open: dropdownOpen, active: isActiveEmpleos || isActive('/publicar-trabajo') }" @mouseenter="dropdownOpen = true" @mouseleave="dropdownOpen = false">
          <button type="button" class="nav-dropdown-trigger nav-link" @click="toggleDropdown" aria-haspopup="true" :aria-expanded="dropdownOpen">
            Empleos
          </button>
          <div class="nav-dropdown-menu" @click.stop>
            <router-link to="/empleos" class="nav-link" :class="{ active: isActiveEmpleos }">Buscar empleos</router-link>
            <router-link to="/publicar-trabajo" class="nav-link" :class="{ active: isActive('/publicar-trabajo') }">Publicar empleo</router-link>
          </div>
        </div>
        <router-link to="/dejar-resena" class="nav-link" :class="{ active: route.path === '/dejar-resena' || route.path.startsWith('/dejar-resena/') }">Dejá tu reseña</router-link>
        <router-link to="/#como-funciona" class="nav-link">Cómo funciona</router-link>
        <router-link v-if="authStore.estaAutenticado" to="/mi-panel" class="nav-link" :class="{ active: isActive('/mi-panel') }">Mi panel</router-link>
        <template v-if="authStore.estaAutenticado">
          <button type="button" class="cta-outline" @click="cerrarSesion(); closeMenu()">Cerrar sesión</button>
        </template>
        <template v-else>
          <router-link to="/login" class="nav-link nav-link-login">Iniciar sesión</router-link>
          <button type="button" class="cta" @click="irARegistro(); closeMenu()">Regístrate</button>
        </template>
      </nav>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const menuOpen = ref(false)
const dropdownOpen = ref(false)

function toggleMenu () {
  menuOpen.value = !menuOpen.value
  if (!menuOpen.value) dropdownOpen.value = false
}

function closeMenu () {
  menuOpen.value = false
  dropdownOpen.value = false
}

function toggleDropdown () {
  dropdownOpen.value = !dropdownOpen.value
}

function closeDropdown () {
  dropdownOpen.value = false
}

// Cerrar menú al cambiar de ruta (evita conflictos con router-link y el error parentNode)
watch(() => route.path, () => {
  closeMenu()
})

function isActive (path: string) {
  if (path === '/') return route.path === '/' && !route.query.tipo
  return route.path === path
}

const isActiveInicio = computed(() => route.path === '/' && !route.query.tipo)
const isActiveOficiantes = computed(() => route.path === '/' && route.query.tipo === 'oficiante')
const isActiveProfesionales = computed(() => route.path === '/' && route.query.tipo === 'profesional')

const isActiveEmpleos = computed(() => route.path === '/empleos' || route.path.startsWith('/empleos/'))

function irA (path: string) {
  if (path === '/') {
    router.push('/')
    return
  }
  router.push(path)
}

function irAInicio () {
  router.push('/')
}

function irAOficiantes () {
  router.push({ path: '/', query: { tipo: 'oficiante' } })
}

function irAProfesionales () {
  router.push({ path: '/', query: { tipo: 'profesional' } })
}

function irALogin () {
  router.push('/login')
}

function irARegistro () {
  router.push('/registrar-profesional')
}

function cerrarSesion () {
  authStore.logout()
  router.push('/')
}
</script>

<style scoped>
.app-nav-header {
  position: sticky;
  top: 0;
  z-index: 100;
  background: white;
  backdrop-filter: saturate(180%) blur(10px);
  border-bottom: 2px solid var(--brand);
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
  color: #0f172a;
}

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
  background: #0f172a;
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

.navlinks {
  display: flex;
  align-items: center;
  gap: 16px;
}

.nav-link {
  text-decoration: none;
  color: #0f172a;
  font-weight: 500;
  transition: color 0.2s ease, border-color 0.2s ease, background 0.2s ease;
  padding: 8px 12px;
  border-radius: 8px;
  border: 2px solid transparent;
  background: transparent;
  cursor: pointer;
  font-size: inherit;
  font-family: inherit;
}

.navlinks a.nav-link:hover {
  color: var(--brand);
}

.navlinks a.nav-link.active,
.nav-dropdown.active .nav-dropdown-trigger {
  color: var(--brand);
  font-weight: 600;
  border-color: var(--brand);
  background: transparent;
}

/* Dropdown Empleos */
.nav-dropdown {
  position: relative;
}

.nav-dropdown-trigger {
  display: inline-flex;
  align-items: center;
  gap: 4px;
}

.nav-dropdown-trigger::after {
  content: '';
  width: 0;
  height: 0;
  border-left: 4px solid transparent;
  border-right: 4px solid transparent;
  border-top: 5px solid currentColor;
  margin-left: 2px;
}

.nav-dropdown.open .nav-dropdown-trigger::after {
  transform: rotate(180deg);
}

.nav-dropdown-menu {
  display: none;
  position: absolute;
  top: 100%;
  left: 0;
  margin-top: 4px;
  min-width: 180px;
  flex-direction: column;
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
  padding: 8px 0;
  z-index: 100;
}

.nav-dropdown:hover .nav-dropdown-menu,
.nav-dropdown.open .nav-dropdown-menu {
  display: flex;
}

.nav-dropdown-menu .nav-link {
  padding: 10px 16px;
  border: none;
  border-radius: 0;
  white-space: nowrap;
}

.nav-dropdown-menu .nav-link:hover {
  background: #f0fdf4;
}

.nav-dropdown-menu .nav-link.active {
  background: rgba(34, 197, 94, 0.08);
  border: none;
  border-left: 3px solid var(--brand);
  border-radius: 0;
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

.nav-link-login {
  text-decoration: none;
  color: var(--text);
  font-weight: 500;
  padding: 8px 0;
}

.nav-link-login:hover {
  color: var(--brand);
}

.navlinks .cta-outline {
  background: transparent;
  color: #475569;
  border: 2px solid #e2e8f0;
  padding: 10px 20px;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s ease;
}

.navlinks .cta-outline:hover {
  background: var(--brand);
  color: white;
  border-color: var(--brand);
}

@media (max-width: 768px) {
  .menu-toggle {
    display: flex;
  }

  .navlinks {
    position: fixed;
    top: 0;
    right: -100%;
    width: 100%;
    max-width: 320px;
    height: 100vh;
    background: linear-gradient(180deg, #f0fdf4 0%, #ecfdf5 50%, #f0f9ff 100%);
    flex-direction: column;
    justify-content: center;
    align-items: center;
    gap: 32px;
    transition: right 0.3s ease;
    z-index: 1000;
    box-shadow: -4px 0 20px rgba(0, 0, 0, 0.12);
    padding: 80px 24px 40px;
  }

  .navlinks.active {
    right: 0;
  }

  .navlinks a.nav-link,
  .nav-dropdown-trigger {
    font-size: 1.2rem;
    font-weight: 600;
    color: #0f172a;
  }

  .navlinks .nav-dropdown .nav-link {
    color: #0f172a;
  }

  .navlinks .cta-outline {
    color: #475569;
  }

  .nav-dropdown-menu {
    position: static;
    margin-top: 8px;
    margin-bottom: 8px;
    min-width: 100%;
    box-shadow: none;
    border: 1px solid #e5e7eb;
  }

  .navlinks .cta {
    font-size: 1.1rem;
    padding: 16px 32px;
  }

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
