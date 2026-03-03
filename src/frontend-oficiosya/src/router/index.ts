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
import PerfilProfesional from '../views/PerfilProfesional.vue'
import PublicarTrabajo from '../views/PublicarTrabajo.vue'
import EmpleosPublicados from '../views/EmpleosPublicados.vue'
import DetalleEmpleo from '../views/DetalleEmpleo.vue'
import PanelProfesional from '../views/PanelProfesional.vue'
import DejarResena from '../views/DejarResena.vue'
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
    path: '/publicar-trabajo',
    name: 'PublicarTrabajo',
    component: PublicarTrabajo
  },
  {
    path: '/empleos',
    name: 'EmpleosPublicados',
    component: EmpleosPublicados
  },
  {
    path: '/empleos/:id',
    name: 'DetalleEmpleo',
    component: DetalleEmpleo
  },
  {
    path: '/profesional/:id',
    name: 'PerfilProfesional',
    component: PerfilProfesional
  },
  {
    path: '/dejar-resena',
    name: 'DejarResena',
    component: DejarResena
  },
  {
    path: '/dejar-resena/:id',
    name: 'DejarResenaId',
    component: DejarResena
  },
  {
    path: '/r/:id',
    name: 'LinkCortoResena',
    redirect: (to) => ({ name: 'DejarResenaId', params: { id: to.params.id } })
  },
  {
    path: '/mi-panel',
    name: 'PanelProfesional',
    component: PanelProfesional
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
  routes,
  scrollBehavior (to, _from, savedPosition) {
    if (to.hash) {
      return { el: to.hash, behavior: 'smooth' }
    }
    if (savedPosition) return savedPosition
    return { top: 0 }
  }
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
