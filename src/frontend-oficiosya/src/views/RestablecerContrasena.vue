<template>
  <div class="restablecer-contrasena">
    <section class="section" style="background: #f8fafc; min-height: calc(100vh - 60px);">
      <div class="container" style="max-width: 400px; padding-top: 40px;">
        <div class="card" style="padding: 40px;">
          <h2 style="text-align: center; margin-bottom: 16px;">Nueva contraseña</h2>
          <p style="text-align: center; color: var(--muted); margin-bottom: 30px;">
            Elegí una contraseña nueva para tu cuenta.
          </p>

          <div v-if="exito" class="success-box">
            <p>{{ mensajeExito }}</p>
            <button type="button" class="cta" style="width: 100%; margin-top: 16px;" @click="irALogin">
              Ir al login
            </button>
          </div>

          <form v-else @submit.prevent="restablecer">
            <div class="form-group">
              <label for="nueva">Nueva contraseña</label>
              <div class="password-field">
                <input
                  id="nueva"
                  v-model="formulario.nuevaContrasena"
                  :type="mostrarNueva ? 'text' : 'password'"
                  required
                  minlength="6"
                  :class="{ error: errores.nuevaContrasena }"
                  placeholder="Mínimo 6 caracteres"
                  autocomplete="new-password"
                />
                <button
                  type="button"
                  class="password-toggle"
                  :aria-label="mostrarNueva ? 'Ocultar contraseña' : 'Mostrar contraseña'"
                  @click="mostrarNueva = !mostrarNueva"
                >
                  <svg v-if="mostrarNueva" xmlns="http://www.w3.org/2000/svg" width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" aria-hidden="true">
                    <path d="M5 11c2.8-3.2 5.2-4.5 7-4.5s4.2 1.3 7 4.5" />
                    <circle cx="12" cy="13.5" r="1.4" fill="currentColor" stroke="none" />
                    <line x1="6" y1="18" x2="18" y2="6" />
                  </svg>
                  <svg v-else xmlns="http://www.w3.org/2000/svg" width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" aria-hidden="true">
                    <path d="M5 11c2.8-3.2 5.2-4.5 7-4.5s4.2 1.3 7 4.5" />
                    <circle cx="12" cy="13.5" r="1.4" fill="currentColor" stroke="none" />
                  </svg>
                </button>
              </div>
              <span v-if="errores.nuevaContrasena" class="error">{{ errores.nuevaContrasena }}</span>
            </div>

            <div class="form-group">
              <label for="confirmar">Confirmar contraseña</label>
              <div class="password-field">
                <input
                  id="confirmar"
                  v-model="formulario.confirmarContrasena"
                  :type="mostrarConfirmar ? 'text' : 'password'"
                  required
                  :class="{ error: errores.confirmarContrasena }"
                  placeholder="Repetí la contraseña"
                  autocomplete="new-password"
                />
                <button
                  type="button"
                  class="password-toggle"
                  :aria-label="mostrarConfirmar ? 'Ocultar contraseña' : 'Mostrar contraseña'"
                  @click="mostrarConfirmar = !mostrarConfirmar"
                >
                  <svg v-if="mostrarConfirmar" xmlns="http://www.w3.org/2000/svg" width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" aria-hidden="true">
                    <path d="M5 11c2.8-3.2 5.2-4.5 7-4.5s4.2 1.3 7 4.5" />
                    <circle cx="12" cy="13.5" r="1.4" fill="currentColor" stroke="none" />
                    <line x1="6" y1="18" x2="18" y2="6" />
                  </svg>
                  <svg v-else xmlns="http://www.w3.org/2000/svg" width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.6" stroke-linecap="round" aria-hidden="true">
                    <path d="M5 11c2.8-3.2 5.2-4.5 7-4.5s4.2 1.3 7 4.5" />
                    <circle cx="12" cy="13.5" r="1.4" fill="currentColor" stroke="none" />
                  </svg>
                </button>
              </div>
              <span v-if="errores.confirmarContrasena" class="error">{{ errores.confirmarContrasena }}</span>
            </div>

            <div v-if="errores.general" class="error" style="text-align: center; margin-bottom: 16px;">
              {{ errores.general }}
            </div>

            <div v-if="!tokenValido" class="error" style="text-align: center; margin-bottom: 16px;">
              El enlace no es válido. Solicitá uno nuevo desde el login.
            </div>

            <button
              type="submit"
              class="cta"
              style="width: 100%; margin-top: 20px;"
              :disabled="cargando || !tokenValido"
            >
              {{ cargando ? 'Guardando...' : 'Restablecer contraseña' }}
            </button>

            <div style="text-align: center; margin-top: 20px;">
              <router-link to="/olvide-contrasena" style="color: var(--brand); font-size: 14px;">
                Solicitar nuevo enlace
              </router-link>
            </div>
          </form>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { usuarioService } from '../services/usuarioService'

const route = useRoute()
const router = useRouter()

const token = computed(() => (route.query.token as string) || '')
const tokenValido = computed(() => token.value.trim().length > 0)

const formulario = reactive({
  nuevaContrasena: '',
  confirmarContrasena: ''
})

const errores = reactive({
  nuevaContrasena: '',
  confirmarContrasena: '',
  general: ''
})

const cargando = ref(false)
const exito = ref(false)
const mensajeExito = ref('')
const mostrarNueva = ref(false)
const mostrarConfirmar = ref(false)

const limpiarErrores = () => {
  errores.nuevaContrasena = ''
  errores.confirmarContrasena = ''
  errores.general = ''
}

const validar = () => {
  limpiarErrores()
  let ok = true

  if (formulario.nuevaContrasena.length < 6) {
    errores.nuevaContrasena = 'Mínimo 6 caracteres'
    ok = false
  }
  if (formulario.nuevaContrasena !== formulario.confirmarContrasena) {
    errores.confirmarContrasena = 'Las contraseñas no coinciden'
    ok = false
  }

  return ok
}

const restablecer = async () => {
  if (!validar() || !tokenValido.value) return

  cargando.value = true
  limpiarErrores()

  try {
    const respuesta = await usuarioService.restablecerContrasena({
      token: token.value.trim(),
      nuevaContrasena: formulario.nuevaContrasena,
      confirmarContrasena: formulario.confirmarContrasena
    })
    mensajeExito.value = respuesta.message
    exito.value = true
  } catch (error: any) {
    errores.general =
      error.response?.data?.message || error.message || 'No se pudo restablecer la contraseña.'
  } finally {
    cargando.value = false
  }
}

const irALogin = () => {
  router.push('/login')
}
</script>

<style scoped>
input.error {
  border-color: #ef4444;
}

.cta:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.success-box {
  text-align: center;
  color: var(--text);
  background: #ecfdf5;
  border: 1px solid #a7f3d0;
  border-radius: 8px;
  padding: 20px;
}

.password-field {
  position: relative;
}

.password-field input {
  padding-right: 44px;
}

.password-toggle {
  position: absolute;
  top: 50%;
  right: 10px;
  transform: translateY(-50%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 4px;
  border: none;
  background: transparent;
  color: #9ca3af;
  cursor: pointer;
  border-radius: 6px;
  line-height: 0;
  font-size: inherit;
}

.password-toggle:hover {
  color: #6b7280;
}
</style>
