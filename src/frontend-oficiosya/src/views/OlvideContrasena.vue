<template>
  <div class="olvide-contrasena">
    <section class="section" style="background: #f8fafc; min-height: calc(100vh - 60px);">
      <div class="container" style="max-width: 400px; padding-top: 40px;">
        <div class="card" style="padding: 40px;">
          <h2 style="text-align: center; margin-bottom: 16px;">¿Olvidaste tu contraseña?</h2>
          <p style="text-align: center; color: var(--muted); margin-bottom: 30px;">
            Ingresá tu email y te enviaremos un enlace para restablecerla.
          </p>

          <div v-if="enviado" class="success-box">
            <p>{{ mensajeExito }}</p>
            <button type="button" class="cta" style="width: 100%; margin-top: 16px;" @click="irALogin">
              Volver al login
            </button>
          </div>

          <form v-else @submit.prevent="enviarSolicitud">
            <div class="form-group">
              <label for="email">Email</label>
              <input
                id="email"
                v-model="email"
                type="email"
                required
                :class="{ error: errorEmail }"
                placeholder="tu@email.com"
                autocomplete="email"
              />
              <span v-if="errorEmail" class="error">{{ errorEmail }}</span>
            </div>

            <div v-if="errorGeneral" class="error" style="text-align: center; margin-bottom: 16px;">
              {{ errorGeneral }}
            </div>

            <button type="submit" class="cta" style="width: 100%; margin-top: 20px;" :disabled="cargando">
              {{ cargando ? 'Enviando...' : 'Enviar enlace de recuperación' }}
            </button>

            <div style="text-align: center; margin-top: 20px;">
              <a href="#" style="color: var(--brand); font-size: 14px;" @click.prevent="irALogin">
                ← Volver al login
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
import { usuarioService } from '../services/usuarioService'

const router = useRouter()

const email = ref('')
const cargando = ref(false)
const enviado = ref(false)
const mensajeExito = ref('')
const errorEmail = ref('')
const errorGeneral = ref('')

const enviarSolicitud = async () => {
  errorEmail.value = ''
  errorGeneral.value = ''

  const valor = email.value.trim()
  if (!valor) {
    errorEmail.value = 'El email es obligatorio'
    return
  }
  if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(valor)) {
    errorEmail.value = 'El email no tiene un formato válido'
    return
  }

  cargando.value = true
  try {
    const respuesta = await usuarioService.solicitarRecuperacionContrasena(valor)
    mensajeExito.value = respuesta.message
    enviado.value = true
  } catch (error: any) {
    errorGeneral.value =
      error.response?.data?.message || error.message || 'No se pudo enviar la solicitud. Intentá de nuevo.'
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
</style>
