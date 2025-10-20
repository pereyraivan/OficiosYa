import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import RegistrarUsuario from '../views/RegistrarUsuario.vue'
import RegistrarProfesional from '../views/RegistrarProfesional.vue'
import Login from '../views/Login.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
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
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
