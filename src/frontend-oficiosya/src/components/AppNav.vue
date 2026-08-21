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
        <div
          class="nav-dropdown"
          :class="{ open: planesDropdownOpen }"
          @mouseenter="planesDropdownOpen = true"
          @mouseleave="planesDropdownOpen = false"
        >
          <button
            type="button"
            class="nav-dropdown-trigger nav-link"
            @click.stop="planesDropdownOpen = !planesDropdownOpen"
            aria-haspopup="true"
            :aria-expanded="planesDropdownOpen"
          >
            Planes
          </button>
          <div class="nav-dropdown-menu planes-dropdown-menu" @click.stop>
            <div v-if="cargandoPlanes" class="planes-dropdown-loading">Cargando planes...</div>
            <template v-else>
              <div v-if="planes.length === 0" class="planes-dropdown-empty">No hay planes disponibles.</div>
              <div
                v-for="plan in planes"
                :key="plan.id || plan.nombre"
                class="planes-dropdown-plan"
                role="button"
                tabindex="0"
                @click="irAPlanes(); closeMenu();"
                @keydown.enter="irAPlanes(); closeMenu();"
              >
                <div class="planes-dropdown-plan-title">
                  <span class="planes-dropdown-plan-nombre">{{ plan.nombre }}</span>
                  <span v-if="plan.precioMensual === 0" class="planes-dropdown-plan-precio">Gratis</span>
                  <span v-else-if="typeof plan.precioMensual === 'number'" class="planes-dropdown-plan-precio">
                    $ {{ formatNum(plan.precioMensual) }} / mes
                  </span>
                </div>
                <div class="planes-dropdown-plan-desc">
                  {{ construirDescripcionPlan(plan) }}
                </div>
              </div>
            </template>
          </div>
        </div>
        <div
          v-if="authStore.esAdmin"
          class="nav-dropdown"
          :class="{ open: adminDropdownOpen, active: isActiveAdmin }"
          @mouseenter="adminDropdownOpen = true"
          @mouseleave="adminDropdownOpen = false"
        >
          <button
            type="button"
            class="nav-dropdown-trigger nav-link"
            @click.stop="adminDropdownOpen = !adminDropdownOpen"
            aria-haspopup="true"
            :aria-expanded="adminDropdownOpen"
          >
            Admin
          </button>
          <div class="nav-dropdown-menu" @click.stop>
            <router-link to="/admin" class="nav-link" :class="{ active: route.path === '/admin' }">Panel admin</router-link>
            <router-link to="/admin/profesionales" class="nav-link" :class="{ active: isActive('/admin/profesionales') }">Profesionales</router-link>
            <router-link to="/admin/suscripciones" class="nav-link" :class="{ active: isActive('/admin/suscripciones') }">Pagos pendientes</router-link>
            <router-link to="/admin/categorias" class="nav-link" :class="{ active: isActive('/admin/categorias') }">Categorías</router-link>
          </div>
        </div>
        <div
          v-if="authStore.estaAutenticado"
          class="nav-dropdown user-dropdown"
          :class="{ open: userDropdownOpen, active: isActive('/mi-panel') }"
        >
          <button
            type="button"
            class="user-avatar-btn"
            @click.stop="toggleUserDropdown"
            aria-haspopup="true"
            :aria-expanded="userDropdownOpen"
            :title="nombreCompleto"
          >
            <span class="user-avatar">{{ inicialesUsuario }}</span>
          </button>
          <div class="user-dropdown-menu" @click.stop>
            <div class="user-dropdown-header">
              <span class="user-avatar user-avatar-lg">{{ inicialesUsuario }}</span>
              <div class="user-dropdown-info">
                <strong>{{ nombreCompleto }}</strong>
                <span>{{ authStore.usuario?.email }}</span>
              </div>
            </div>
            <div class="user-dropdown-divider" />
            <template v-if="authStore.esProfesional">
              <button
                v-for="item in panelMenuItems"
                :key="item.id"
                type="button"
                class="user-dropdown-item"
                @click="irAPanel(item.id)"
              >
                {{ item.label }}
              </button>
            </template>
            <button
              v-else
              type="button"
              class="user-dropdown-item"
              @click="irARegistro(); closeMenu()"
            >
              Crear cuenta profesional
            </button>
            <div class="user-dropdown-divider" />
            <button type="button" class="user-dropdown-item user-dropdown-logout" @click="cerrarSesion(); closeMenu()">
              Cerrar sesión
            </button>
          </div>
        </div>
        <template v-if="!authStore.estaAutenticado">
          <router-link to="/login" class="nav-link nav-link-login">Iniciar sesión</router-link>
          <button type="button" class="cta" @click="irARegistro(); closeMenu()">Regístrate</button>
        </template>
      </nav>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted, onUnmounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { profesionalService } from '../services/profesionalService'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const menuOpen = ref(false)
const dropdownOpen = ref(false)
const planesDropdownOpen = ref(false)
const adminDropdownOpen = ref(false)
const userDropdownOpen = ref(false)

const panelMenuItems = [
  { id: 'perfil', label: 'Perfil' },
  { id: 'suscripcion', label: 'Suscripción' },
  { id: 'zonas', label: 'Zonas' },
  { id: 'categorias', label: 'Categorías' }
] as const

const nombreCompleto = computed(() => {
  const u = authStore.usuario
  if (!u) return 'Usuario'
  return [u.nombre, u.apellido].filter(Boolean).join(' ').trim() || u.email
})

const inicialesUsuario = computed(() => {
  const u = authStore.usuario
  if (!u) return '?'
  const nombre = (u.nombre || '').trim()
  const apellido = (u.apellido || '').trim()
  if (nombre && apellido) return (nombre[0] + apellido[0]).toUpperCase()
  if (nombre) return nombre.slice(0, 2).toUpperCase()
  if (u.email) return u.email[0].toUpperCase()
  return '?'
})
const planes = ref<any[]>([])
const cargandoPlanes = ref(false)

function toggleMenu () {
  menuOpen.value = !menuOpen.value
  if (!menuOpen.value) {
    dropdownOpen.value = false
    planesDropdownOpen.value = false
    adminDropdownOpen.value = false
    userDropdownOpen.value = false
  }
}

function closeMenu () {
  menuOpen.value = false
  dropdownOpen.value = false
  planesDropdownOpen.value = false
  adminDropdownOpen.value = false
  userDropdownOpen.value = false
}

function toggleUserDropdown () {
  userDropdownOpen.value = !userDropdownOpen.value
  if (userDropdownOpen.value) {
    dropdownOpen.value = false
    planesDropdownOpen.value = false
    adminDropdownOpen.value = false
  }
}

function irAPanel (tab: string) {
  closeMenu()
  router.push({ path: '/mi-panel', query: { tab } })
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
const isActiveAdmin = computed(() => route.path.startsWith('/admin'))

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

function irAPlanes () {
  router.push('/planes')
}

function formatNum (n: number) {
  return typeof n === 'number' ? n.toLocaleString('es-AR') : n
}

function normalizarPlanNombre (nombre: unknown): string {
  return (nombre ?? '')
    .toString()
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '') // Quitar tildes
    .trim()
}

function construirDescripcionPlan (plan: any): string {
  const base = plan?.descripcion ? String(plan.descripcion).trim() : ''
  const n = normalizarPlanNombre(plan?.nombre)

  const lineas: string[] = []
  if (n === 'free') {
    lineas.push('Contactos: 5 por mes')
    lineas.push('Zonas de cobertura: hasta 1 zona')
    lineas.push('Postulaciones a empleos: no incluidas')
  } else if (n === 'basica' || n === 'basico') {
    lineas.push('Contactos: 20 por mes')
    lineas.push('Zonas de cobertura: ilimitadas')
    lineas.push('Postulaciones a empleos: no incluidas')
  } else if (n === 'pro') {
    lineas.push('Contactos: ilimitados')
    lineas.push('Zonas de cobertura: ilimitadas')
    lineas.push('Postulaciones a empleos: hasta 5')
  } else if (n === 'premium') {
    lineas.push('Contactos: ilimitados')
    lineas.push('Zonas de cobertura: ilimitadas')
    lineas.push('Postulaciones a empleos: ilimitadas')
  }

  const bloqueIncluye = [
    lineas.length ? 'Incluye:' : null,
    ...lineas.map(l => `• ${l}`),
    'Importante: si tu suscripción vence, volvé automáticamente a Free.'
  ].filter(Boolean)

  if (!base) return bloqueIncluye.join('\n')
  return `${base}\n\n${bloqueIncluye.join('\n')}`
}

async function cargarPlanes () {
  cargandoPlanes.value = true
  try {
    const list = await profesionalService.obtenerPlanesSuscripcion()
    planes.value = Array.isArray(list) ? list : []
  } finally {
    cargandoPlanes.value = false
  }
}

function cerrarDropdownsExternos (event: MouseEvent) {
  const target = event.target as HTMLElement | null
  if (!target?.closest('.user-dropdown')) {
    userDropdownOpen.value = false
  }
}

onMounted(() => {
  cargarPlanes()
  document.addEventListener('click', cerrarDropdownsExternos)
})

onUnmounted(() => {
  document.removeEventListener('click', cerrarDropdownsExternos)
})
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

.nav-dropdown:not(.user-dropdown):hover .nav-dropdown-menu,
.nav-dropdown:not(.user-dropdown).open .nav-dropdown-menu {
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

.planes-dropdown-menu {
  min-width: 320px;
  max-width: 420px;
  max-height: min(70vh, 520px);
  overflow-y: auto;
  overscroll-behavior: contain;
  padding: 10px 0;
}

.planes-dropdown-loading,
.planes-dropdown-empty {
  padding: 10px 16px;
  color: #64748b;
  font-size: 0.95rem;
}

.planes-dropdown-plan {
  padding: 10px 16px;
  cursor: pointer;
  outline: none;
}

.planes-dropdown-plan:hover {
  background: #f0fdf4;
}

.planes-dropdown-plan-title {
  display: flex;
  align-items: baseline;
  justify-content: space-between;
  gap: 10px;
}

.planes-dropdown-plan-nombre {
  font-weight: 700;
  color: #0f172a;
}

.planes-dropdown-plan-precio {
  color: var(--brand);
  font-weight: 600;
  white-space: nowrap;
  font-size: 0.9rem;
}

.planes-dropdown-plan-desc {
  margin-top: 6px;
  color: #64748b;
  font-size: 0.92rem;
  line-height: 1.35;
  white-space: pre-line;
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

/* Menú de usuario */
.user-dropdown {
  margin-left: 4px;
}

.user-avatar-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 2px;
  border: 2px solid transparent;
  background: transparent;
  cursor: pointer;
  border-radius: 50%;
  transition: border-color 0.2s ease, box-shadow 0.2s ease, transform 0.15s ease;
}

.user-avatar-btn:hover,
.user-dropdown.open .user-avatar-btn,
.user-dropdown.active .user-avatar-btn {
  border-color: rgba(34, 197, 94, 0.45);
  box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.12);
}

.user-dropdown.active .user-avatar {
  box-shadow: 0 2px 10px rgba(34, 197, 94, 0.45), 0 0 0 2px rgba(255, 255, 255, 0.9);
}

.user-avatar-btn:active {
  transform: scale(0.96);
}

.user-avatar {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: linear-gradient(135deg, var(--brand) 0%, #06b6d4 100%);
  color: white;
  font-size: 0.8rem;
  font-weight: 800;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  letter-spacing: 0.03em;
  flex-shrink: 0;
  box-shadow: 0 2px 8px rgba(34, 197, 94, 0.35);
  text-shadow: 0 1px 1px rgba(15, 23, 42, 0.15);
}

.user-avatar-lg {
  width: 52px;
  height: 52px;
  font-size: 1.05rem;
  box-shadow: 0 4px 14px rgba(34, 197, 94, 0.3);
}

.user-dropdown-menu {
  display: none;
  position: absolute;
  top: calc(100% + 10px);
  right: 0;
  left: auto;
  min-width: 280px;
  max-width: 320px;
  flex-direction: column;
  background: white;
  border: 1px solid rgba(34, 197, 94, 0.2);
  border-radius: 14px;
  box-shadow: 0 12px 32px rgba(15, 23, 42, 0.14), 0 0 0 1px rgba(34, 197, 94, 0.06);
  padding: 8px 0 10px;
  z-index: 110;
  overflow: hidden;
}

.user-dropdown.open .user-dropdown-menu {
  display: flex;
}

.user-dropdown-header {
  display: flex;
  align-items: center;
  gap: 14px;
  padding: 14px 16px 16px;
  background: linear-gradient(135deg, rgba(34, 197, 94, 0.1) 0%, rgba(6, 182, 212, 0.08) 100%);
  border-bottom: 1px solid rgba(34, 197, 94, 0.12);
}

.user-dropdown-info {
  display: flex;
  flex-direction: column;
  gap: 2px;
  min-width: 0;
}

.user-dropdown-info strong {
  font-size: 0.95rem;
  font-weight: 700;
  color: #0f172a;
  line-height: 1.3;
  word-break: break-word;
}

.user-dropdown-info span {
  font-size: 0.8125rem;
  color: var(--brand-700, #15803d);
  word-break: break-all;
  opacity: 0.85;
}

.user-dropdown-divider {
  height: 1px;
  background: linear-gradient(90deg, transparent, rgba(34, 197, 94, 0.2), transparent);
  margin: 6px 12px;
}

.user-dropdown-item {
  display: block;
  width: 100%;
  text-align: left;
  padding: 10px 16px;
  border: none;
  background: transparent;
  color: #0f172a;
  font-size: 0.9375rem;
  font-weight: 500;
  cursor: pointer;
  font-family: inherit;
  transition: background 0.15s ease, color 0.15s ease, padding-left 0.15s ease;
}

.user-dropdown-item:hover {
  background: linear-gradient(90deg, rgba(34, 197, 94, 0.1), rgba(6, 182, 212, 0.06));
  color: var(--brand-700, #15803d);
  padding-left: 20px;
}

.user-dropdown-logout {
  color: #64748b;
}

.user-dropdown-logout:hover {
  color: #dc2626;
  background: #fef2f2;
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

  .nav-dropdown-menu,
  .user-dropdown-menu {
    position: static;
    margin-top: 8px;
    margin-bottom: 8px;
    min-width: 100%;
    max-width: 100%;
    box-shadow: none;
    border: 1px solid #e5e7eb;
  }

  .user-dropdown {
    width: 100%;
    display: flex;
    flex-direction: column;
    align-items: center;
  }

  .user-dropdown-header {
    flex-direction: column;
    text-align: center;
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
