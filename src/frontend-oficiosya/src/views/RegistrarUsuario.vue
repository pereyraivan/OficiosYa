<template>
  <div class="registrar-usuario">
    <!-- Header -->
    <header>
      <div class="container nav">
        <div class="logo" @click="irAHome">
          <div class="logo-badge">🛠️</div>
          <span>OficiosYa</span>
        </div>
        <nav class="navlinks">
          <a href="#" @click="irAHome">Inicio</a>
          <a href="#" @click="irALogin">Iniciar Sesión</a>
        </nav>
      </div>
    </header>

    <!-- Formulario de Registro -->
    <section class="section" style="background: #f8fafc; min-height: calc(100vh - 80px);">
      <div class="container" style="max-width: 500px; padding-top: 40px;">
        <div class="card" style="padding: 40px;">
          <h2 style="text-align: center; margin-bottom: 30px;">Crear cuenta</h2>
          <p style="text-align: center; color: var(--muted); margin-bottom: 30px;">
            Registrate para encontrar profesionales o publicar tus servicios
          </p>

          <form @submit.prevent="registrarUsuario">
            <!-- Nombre -->
            <div class="form-group">
              <label for="nombre">Nombre completo *</label>
              <input
                id="nombre"
                v-model="formulario.nombre"
                type="text"
                required
                :class="{ 'error': errores.nombre }"
                placeholder="Tu nombre completo"
              />
              <span v-if="errores.nombre" class="error">{{ errores.nombre }}</span>
            </div>

            <!-- Email -->
            <div class="form-group">
              <label for="email">Email *</label>
              <input
                id="email"
                v-model="formulario.email"
                type="email"
                required
                :class="{ 'error': errores.email }"
                placeholder="tu@email.com"
                @blur="verificarEmail"
              />
              <span v-if="errores.email" class="error">{{ errores.email }}</span>
              <span v-if="emailVerificando" class="small">Verificando email...</span>
            </div>

            <!-- Teléfono -->
            <div class="form-group">
              <label for="telefono">Teléfono (opcional)</label>
              <input
                id="telefono"
                v-model="formulario.telefono"
                type="tel"
                placeholder="+54 9 381 123-4567"
              />
            </div>

            <!-- Contraseña -->
            <div class="form-group">
              <label for="contraseña">Contraseña *</label>
              <input
                id="contraseña"
                v-model="formulario.contraseña"
                type="password"
                required
                :class="{ 'error': errores.contraseña }"
                placeholder="Mínimo 6 caracteres"
              />
              <span v-if="errores.contraseña" class="error">{{ errores.contraseña }}</span>
            </div>

            <!-- Confirmar Contraseña -->
            <div class="form-group">
              <label for="confirmarContraseña">Confirmar contraseña *</label>
              <input
                id="confirmarContraseña"
                v-model="formulario.confirmarContraseña"
                type="password"
                required
                :class="{ 'error': errores.confirmarContraseña }"
                placeholder="Repetí tu contraseña"
              />
              <span v-if="errores.confirmarContraseña" class="error">{{ errores.confirmarContraseña }}</span>
            </div>

            <!-- Tipo de cuenta -->
            <div class="form-group">
              <label>Tipo de cuenta *</label>
              <div style="display: flex; gap: 16px; margin-top: 8px;">
                <label style="display: flex; align-items: center; gap: 8px; cursor: pointer;">
                  <input
                    v-model="formulario.esProfesional"
                    type="radio"
                    :value="false"
                  />
                  <span>Cliente</span>
                </label>
                <label style="display: flex; align-items: center; gap: 8px; cursor: pointer;">
                  <input
                    v-model="formulario.esProfesional"
                    type="radio"
                    :value="true"
                  />
                  <span>Profesional</span>
                </label>
              </div>
            </div>

            <!-- Términos y condiciones -->
            <div class="form-group">
              <label style="display: flex; align-items: flex-start; gap: 8px; cursor: pointer;">
                <input
                  v-model="formulario.aceptaTerminos"
                  type="checkbox"
                  required
                />
                <span style="font-size: 14px;">
                  Acepto los <a href="#" style="color: var(--brand);">términos y condiciones</a> 
                  y la <a href="#" style="color: var(--brand);">política de privacidad</a>
                </span>
              </label>
              <span v-if="errores.aceptaTerminos" class="error">{{ errores.aceptaTerminos }}</span>
            </div>

            <!-- Mensaje de error general -->
            <div v-if="errores.general" class="error" style="text-align: center; margin-bottom: 16px;">
              {{ errores.general }}
            </div>

            <!-- Botón de registro -->
            <button
              type="submit"
              class="cta"
              style="width: 100%; margin-top: 20px;"
              :disabled="cargando"
              @click="registrarUsuario"
            >
              {{ cargando ? MESSAGES.LOADING.CREATING_ACCOUNT : 'Crear cuenta' }}
            </button>

            <!-- Enlace a login -->
            <div style="text-align: center; margin-top: 20px;">
              <span class="small">¿Ya tenés cuenta? </span>
              <a href="#" @click="irALogin" style="color: var(--brand); font-weight: 500;">
                Iniciar sesión
              </a>
            </div>
          </form>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useUserRegistrationForm } from '../composables/useForm'
import { MESSAGES, ROUTES } from '../utils/constants'

const router = useRouter()
const authStore = useAuthStore()

// Usar el composable para manejar el formulario
const {
  form: formulario,
  errors: errores,
  loading: cargando,
  isValid,
  handleSubmit,
  setError
} = useUserRegistrationForm()

// Estado adicional
const emailVerificando = ref(false)

// Métodos
const verificarEmail = async () => {
  if (!formulario.email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formulario.email)) {
    return
  }

  emailVerificando.value = true
  try {
    const disponible = await authStore.verificarEmail(formulario.email)
    if (!disponible) {
      setError('email', MESSAGES.ERROR.EMAIL_EXISTS)
    }
  } catch (error) {
    console.error('Error verificando email:', error)
  } finally {
    emailVerificando.value = false
  }
}

const registrarUsuario = async () => {
  const success = await handleSubmit()
  
  if (success) {
    try {
      const datosUsuario = {
        nombre: formulario.nombre.trim(),
        email: formulario.email.trim(),
        contraseña: formulario.contraseña,
        telefono: formulario.telefono.trim() || undefined
      }

      const exito = await authStore.registrar(datosUsuario)
      
      if (exito) {
        alert(MESSAGES.SUCCESS.USER_REGISTERED)
        router.push(ROUTES.HOME)
      } else {
        setError('general', MESSAGES.ERROR.GENERAL)
      }
    } catch (error: any) {
      console.error('Error en registro:', error)
      setError('general', error.message || MESSAGES.ERROR.GENERAL)
    }
  }
}

const irAHome = () => {
  router.push(ROUTES.HOME)
}

const irALogin = () => {
  router.push(ROUTES.LOGIN)
}
</script>

<style scoped>
.registrar-usuario {
  min-height: 100vh;
}

.logo {
  cursor: pointer;
}

input[type="radio"] {
  margin: 0;
}

input[type="checkbox"] {
  margin: 0;
}

input.error {
  border-color: #ef4444;
}

.cta:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>
