import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'

// Importar componentes de páginas
import HomePage from './pages/HomePage.vue'
import SearchPage from './pages/SearchPage.vue'
import ProfessionalProfile from './pages/ProfessionalProfile.vue'
import UserDashboard from './pages/UserDashboard.vue'

// Configurar rutas
const routes = [
  { path: '/', component: HomePage },
  { path: '/buscar', component: SearchPage },
  { path: '/profesional/:id', component: ProfessionalProfile, props: true },
  { path: '/dashboard', component: UserDashboard }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// Crear la aplicación Vue
const app = createApp(App)

// Usar el router
app.use(router)

// Montar la aplicación
app.mount('#vue-app')
