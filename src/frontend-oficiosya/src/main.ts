import { createApp } from 'vue'
import { createPinia } from 'pinia'
import router from './router'
import App from './App.vue'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'
import './assets/css/main.css'
import './assets/css/custom-bootstrap.css'

// Limpiar FAB estático del HTML (si quedó de builds viejos) — Vue lo monta flotante
document.getElementById('oy-support-host')?.remove()
document.getElementById('oy-wa-fab-static')?.remove()
document.querySelectorAll('body > #oy-support-fab').forEach((el) => el.remove())

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.mount('#app')
