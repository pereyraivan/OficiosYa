import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import Oficiantes from '../views/Oficiantes.vue'
import RegistrarUsuario from '../views/RegistrarUsuario.vue'
import RegistrarProfesional from '../views/RegistrarProfesional.vue'
import Login from '../views/Login.vue'
import TerminosCondiciones from '../views/TerminosCondiciones.vue'
import PoliticaPrivacidad from '../views/PoliticaPrivacidad.vue'

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
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
