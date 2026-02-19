import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import Oficiantes from '../views/Oficiantes.vue'
import RegistrarUsuario from '../views/RegistrarUsuario.vue'
import RegistrarProfesional from '../views/RegistrarProfesional.vue'
import Login from '../views/Login.vue'
import TerminosCondiciones from '../views/TerminosCondiciones.vue'
import PoliticaPrivacidad from '../views/PoliticaPrivacidad.vue'
import GestionarOficios from '../views/admin/GestionarOficios.vue'
import GestionarCategorias from '../views/admin/GestionarCategorias.vue'
import { useAuthStore } from '../stores/auth'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/oficiantes',
    name: 'Oficiantes',
    component: Oficiantes
  },
  {
    path: '/registrar',
    name: 'RegistrarUsuario',
    component: RegistrarUsuario
  },
  {
    path: '/registrar-profesional',
    name: 'RegistrarProfesional',
    component: RegistrarProfesional
  },
  {
    path: '/login',
    name: 'Login',
    component: Login
  },
  {
    path: '/terminos-y-condiciones',
    name: 'TerminosCondiciones',
    component: TerminosCondiciones
  },
  {
    path: '/politica-de-privacidad',
    name: 'PoliticaPrivacidad',
    component: PoliticaPrivacidad
  },
  {
    path: '/admin/oficios',
    name: 'GestionarOficios',
    component: GestionarOficios,
    meta: { requiresAdmin: true }
  },
  {
    path: '/admin/categorias',
    name: 'GestionarCategorias',
    component: GestionarCategorias,
    meta: { requiresAdmin: true }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// Guard de navegación para rutas de admin
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  
  if (to.meta.requiresAdmin) {
    if (!authStore.estaAutenticado) {
      next({ name: 'Login', query: { redirect: to.fullPath } })
    } else if (!authStore.esAdmin) {
      alert('No tienes permisos para acceder a esta sección')
      next({ name: 'Home' })
    } else {
      next()
    }
  } else {
    next()
  }
})

export default router
