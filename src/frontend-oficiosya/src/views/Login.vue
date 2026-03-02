<template>
  <div class="login">
    <!-- Header -->
    <header>
      <div class="container nav">
        <div class="logo" @click="irAHome">
          <div class="logo-badge">🛠️</div>
          <span>OficiosYa</span>
        </div>
        <nav class="navlinks">
          <a href="#" @click="irAHome">Inicio</a>
        </nav>
      </div>
    </header>

    <!-- Formulario de Login -->
    <section class="section" style="background: #f8fafc; min-height: calc(100vh - 80px);">
      <div class="container" style="max-width: 400px; padding-top: 40px;">
        <div class="card" style="padding: 40px;">
          <h2 style="text-align: center; margin-bottom: 30px;">Iniciar sesión</h2>
          <p style="text-align: center; color: var(--muted); margin-bottom: 30px;">
            Ingresá a tu cuenta para continuar
          </p>

          <form @submit.prevent="iniciarSesion">
            <!-- Email -->
            <div class="form-group">
              <label for="email">Email</label>
              <input
                id="email"
                v-model="formulario.email"
                type="email"
                required
                :class="{ 'error': errores.email }"
                placeholder="tu@email.com"
              />
              <span v-if="errores.email" class="error">{{ errores.email }}</span>
            </div>

            <!-- Contraseña -->
            <div class="form-group">
              <label for="contraseña">Contraseña</label>
              <input
                id="contraseña"
                v-model="formulario.contraseña"
                type="password"
                required
                :class="{ 'error': errores.contraseña }"
                placeholder="Tu contraseña"
              />
              <span v-if="errores.contraseña" class="error">{{ errores.contraseña }}</span>
            </div>

            <!-- Recordarme -->
            <div style="margin-bottom: 20px;">
              <label class="remember-me-label">
                <input
                  v-model="formulario.recordarme"
                  type="checkbox"
                  class="remember-me-checkbox"
                />
                <span class="remember-me-text">Recordarme</span>
              </label>
            </div>

            <!-- Mensaje de error general -->
            <div v-if="errores.general" class="error" style="text-align: center; margin-bottom: 16px;">
              {{ errores.general }}
            </div>

            <!-- Botón de login -->
            <button
              type="submit"
              class="cta"
              style="width: 100%; margin-top: 20px;"
              :disabled="cargando"
            >
              {{ cargando ? 'Iniciando sesión...' : 'Iniciar sesión' }}
            </button>

            <!-- Enlace a registro -->
            <div style="text-align: center; margin-top: 20px;">
              <span class="small">¿No tenés cuenta? </span>
              <a href="#" @click="irARegistro" style="color: var(--brand); font-weight: 500;">
                Registrate como profesional u oficiante
              </a>
            </div>

            <!-- Enlace a recuperar contraseña -->
            <div style="text-align: center; margin-top: 10px;">
              <a href="#" style="color: var(--muted); font-size: 14px;">
                ¿Olvidaste tu contraseña?
              </a>
            </div>
          </form>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const authStore = useAuthStore()

// Estado del formulario
const formulario = reactive({
  email: '',
  contraseña: '',
  recordarme: false
})

// Estado de la UI
const cargando = ref(false)

// Errores
const errores = reactive({
  email: '',
  contraseña: '',
  general: ''
})

// Métodos
const limpiarErrores = () => {
  Object.keys(errores).forEach(key => {
    errores[key as keyof typeof errores] = ''
  })
}

const validarFormulario = () => {
  limpiarErrores()
  let esValido = true

  // Validar email
  if (!formulario.email.trim()) {
    errores.email = 'El email es obligatorio'
    esValido = false
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formulario.email)) {
    errores.email = 'El email no tiene un formato válido'
    esValido = false
  }

  // Validar contraseña
  if (!formulario.contraseña) {
    errores.contraseña = 'La contraseña es obligatoria'
    esValido = false
  }

  return esValido
}

const iniciarSesion = async () => {
  if (!validarFormulario()) {
    return
  }

  cargando.value = true
  limpiarErrores()

  try {
    // El store de auth espera email y password
    const exito = await authStore.login(formulario.email.trim(), formulario.contraseña)
    
    if (exito) {
      // Login exitoso
      alert('¡Bienvenido!')
      router.push('/')
    } else {
      errores.general = 'Email o contraseña incorrectos'
    }
  } catch (error: any) {
    errores.general = error.response?.data?.message || error.message || 'Error al iniciar sesión. Intenta nuevamente.'
  } finally {
    cargando.value = false
  }
}

const irAHome = () => {
  router.push('/')
}

const irARegistro = () => {
  router.push('/registrar-profesional')
}
</script>

<style scoped>
.login {
  min-height: 100vh;
}

.logo {
  cursor: pointer;
}

input.error {
  border-color: #ef4444;
}

.cta:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* Estilos para Recordarme */
.remember-me-label {
  display: flex;
  align-items: center;
  gap: 10px;
  cursor: pointer;
  user-select: none;
}

.remember-me-checkbox {
  margin: 0;
  width: 18px;
  height: 18px;
  cursor: pointer;
  accent-color: var(--brand);
  flex-shrink: 0;
}

.remember-me-text {
  font-size: 14px;
  color: var(--text);
  font-weight: 500;
}
</style>
