<template>
  <div class="registrar-profesional">
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

    <!-- Formulario de Registro de Profesional -->
    <section class="section" style="background: #f8fafc; min-height: calc(100vh - 80px);">
      <div class="container" style="max-width: 600px; padding-top: 40px;">
        <div class="card" style="padding: 40px;">
          <h2 style="text-align: center; margin-bottom: 10px;">Crear cuenta de profesional</h2>
          <p style="text-align: center; color: var(--muted); margin-bottom: 30px;">
            Registrate como profesional para empezar a recibir trabajos
          </p>

          <form @submit.prevent="registrarProfesional">
            <!-- Información Personal -->
            <div style="margin-bottom: 30px;">
              <h3 style="margin-bottom: 20px; color: var(--text);">Información Personal</h3>
              
              <!-- Nombre y Apellido -->
              <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 16px; margin-bottom: 16px;">
                <div class="form-group">
                  <label for="nombre">Nombre *</label>
                  <input
                    id="nombre"
                    v-model="formulario.nombre"
                    type="text"
                    required
                    :class="{ 'error': errores.nombre }"
                    placeholder="Tu nombre"
                  />
                  <span v-if="errores.nombre" class="error">{{ errores.nombre }}</span>
                </div>

                <div class="form-group">
                  <label for="apellido">Apellido *</label>
                  <input
                    id="apellido"
                    v-model="formulario.apellido"
                    type="text"
                    required
                    :class="{ 'error': errores.apellido }"
                    placeholder="Tu apellido"
                  />
                  <span v-if="errores.apellido" class="error">{{ errores.apellido }}</span>
                </div>
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
              <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 16px;">
                <div class="form-group">
                  <label for="password">Contraseña *</label>
                  <input
                    id="password"
                    v-model="formulario.password"
                    type="password"
                    required
                    :class="{ 'error': errores.password }"
                    placeholder="Mínimo 6 caracteres"
                  />
                  <span v-if="errores.password" class="error">{{ errores.password }}</span>
                </div>

                <div class="form-group">
                  <label for="confirmarPassword">Confirmar contraseña *</label>
                  <input
                    id="confirmarPassword"
                    v-model="formulario.confirmarPassword"
                    type="password"
                    required
                    :class="{ 'error': errores.confirmarPassword }"
                    placeholder="Repetí tu contraseña"
                  />
                  <span v-if="errores.confirmarPassword" class="error">{{ errores.confirmarPassword }}</span>
                </div>
              </div>
            </div>

            <!-- Información de Ubicación -->
            <div style="margin-bottom: 30px;">
              <h3 style="margin-bottom: 20px; color: var(--text);">Ubicación</h3>
              
              <div class="form-group">
                <label for="direccion">Dirección (opcional)</label>
                <input
                  id="direccion"
                  v-model="formulario.direccion"
                  type="text"
                  placeholder="Calle, número, piso, depto"
                />
              </div>

              <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 16px;">
                <div class="form-group">
                  <label for="ciudad">Ciudad *</label>
                  <input
                    id="ciudad"
                    v-model="formulario.ciudad"
                    type="text"
                    required
                    :class="{ 'error': errores.ciudad }"
                    placeholder="Ej: San Miguel de Tucumán"
                  />
                  <span v-if="errores.ciudad" class="error">{{ errores.ciudad }}</span>
                </div>

                <div class="form-group">
                  <label for="provincia">Provincia *</label>
                  <select
                    id="provincia"
                    v-model="formulario.provincia"
                    required
                    :class="{ 'error': errores.provincia }"
                    style="width: 100%; padding: 12px; border: 1px solid #e2e8f0; border-radius: 6px; font-size: 14px;"
                  >
                    <option value="">Seleccionar provincia</option>
                    <option v-for="provincia in PROVINCIAS_ARGENTINA" :key="provincia" :value="provincia">
                      {{ provincia }}
                    </option>
                  </select>
                  <span v-if="errores.provincia" class="error">{{ errores.provincia }}</span>
                </div>
              </div>

              <div class="form-group">
                <label for="codigoPostal">Código Postal (opcional)</label>
                <input
                  id="codigoPostal"
                  v-model="formulario.codigoPostal"
                  type="text"
                  placeholder="4000"
                />
              </div>
            </div>

            <!-- Información Profesional -->
            <div style="margin-bottom: 30px;">
              <h3 style="margin-bottom: 20px; color: var(--text);">Información Profesional</h3>
              
              <div class="form-group">
                <label for="descripcion">Descripción profesional (opcional)</label>
                <textarea
                  id="descripcion"
                  v-model="formulario.descripcion"
                  rows="4"
                  placeholder="Contanos sobre tu experiencia, especialidades, años trabajando..."
                ></textarea>
              </div>

              <!-- Selección de Tipo de Servicio -->
              <div class="form-group">
                <label style="margin-bottom: 12px; display: block; font-weight: 500;">
                  ¿Qué tipo de servicio ofrecés? *
                </label>
                <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 16px; margin-bottom: 20px;">
                  <div
                    class="tipo-servicio-option"
                    :class="{ selected: tipoServicio === 'oficiante' }"
                    @click="tipoServicio = 'oficiante'"
                  >
                    <div class="tipo-icon">🧰</div>
                    <div class="tipo-content">
                      <h3>Soy Oficiante</h3>
                      <p>Plomero, Electricista, Albañil, etc.</p>
                    </div>
                  </div>
                  <div
                    class="tipo-servicio-option"
                    :class="{ selected: tipoServicio === 'profesional' }"
                    @click="tipoServicio = 'profesional'"
                  >
                    <div class="tipo-icon">💼</div>
                    <div class="tipo-content">
                      <h3>Soy Profesional</h3>
                      <p>Abogado, Contador, Médico, etc.</p>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Selección de Oficios/Categorías -->
              <div v-if="tipoServicio" class="form-group">
                <label>Servicios que ofrecés *</label>
                <div style="display: grid; grid-template-columns: repeat(2, 1fr); gap: 8px; margin-top: 8px;">
                  <label v-for="categoria in categoriasDisponibles" :key="categoria.nombre" 
                         :style="`display: flex; align-items: center; gap: 8px; cursor: pointer; padding: 8px; border: 1px solid #e2e8f0; border-radius: 6px; background: ${formulario.oficios.includes(categoria.nombre) ? '#e0f2fe' : 'white'};`">
                    <input
                      v-model="formulario.oficios"
                      type="checkbox"
                      :value="categoria.nombre"
                    />
                    <span class="oficio-icon">{{ categoria.icono }}</span>
                    <span>{{ categoria.nombre }}</span>
                  </label>
                </div>
                <span v-if="errores.oficios" class="error">{{ errores.oficios }}</span>
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
                <span style="font-size: 14px; line-height: 1.4; flex: 1;">
                  Acepto los <a href="#" @click.prevent="abrirTerminos" style="color: var(--brand); text-decoration: underline;">términos y condiciones</a> y la <a href="#" @click.prevent="abrirPolitica" style="color: var(--brand); text-decoration: underline;">política de privacidad</a>
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
              @click="registrarProfesional"
            >
              {{ cargando ? MESSAGES.LOADING.CREATING_ACCOUNT : 'Crear cuenta de profesional' }}
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
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { profesionalService } from '../services/profesionalService'
import { useProfessionalRegistrationForm } from '../composables/useForm'
import { CATEGORIAS_SERVICIO, PROVINCIAS_ARGENTINA, MESSAGES, ROUTES } from '../utils/constants'

const router = useRouter()

// Usar el composable para manejar el formulario
const {
  form: formulario,
  errors: errores,
  loading: cargando,
  isValid,
  handleSubmit,
  updateField,
  setError
} = useProfessionalRegistrationForm()

// Estado adicional
const emailVerificando = ref(false)
const tipoServicio = ref<'oficiante' | 'profesional' | null>(null)

// Computed para categorías disponibles según el tipo seleccionado
const categoriasDisponibles = computed(() => {
  if (tipoServicio.value === 'oficiante') {
    return CATEGORIAS_SERVICIO.OFICIANTE
  } else if (tipoServicio.value === 'profesional') {
    return CATEGORIAS_SERVICIO.PROFESIONAL
  }
  return []
})

// Métodos
const verificarEmail = async () => {
  if (!formulario.email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formulario.email)) {
    return
  }

  emailVerificando.value = true
  try {
    // Aquí podrías implementar la verificación de email si tienes un endpoint
    // const disponible = await authStore.verificarEmail(formulario.email)
    // if (!disponible) {
    //   errores.email = 'Este email ya está registrado'
    // }
  } catch (error) {
    console.error('Error verificando email:', error)
  } finally {
    emailVerificando.value = false
  }
}

const registrarProfesional = async () => {
  const success = await handleSubmit()
  
  if (success) {
    try {
      const datosProfesional = {
        nombre: formulario.nombre.trim(),
        apellido: formulario.apellido.trim(),
        email: formulario.email.trim(),
        password: formulario.password,
        telefono: formulario.telefono.trim() || undefined,
        direccion: formulario.direccion.trim() || undefined,
        ciudad: formulario.ciudad.trim(),
        provincia: formulario.provincia.trim(),
        codigoPostal: formulario.codigoPostal.trim() || undefined,
        descripcion: formulario.descripcion.trim() || undefined,
        oficios: formulario.oficios
      }

      const profesional = await profesionalService.registrar(datosProfesional)
      
      // Registro exitoso
      alert(MESSAGES.SUCCESS.PROFESSIONAL_REGISTERED)
      router.push(ROUTES.HOME)
    } catch (error: any) {
      console.error('Error en registro:', error)
      setError('general', error.response?.data?.message || MESSAGES.ERROR.GENERAL)
    }
  }
}

const irAHome = () => {
  router.push(ROUTES.HOME)
}

const irALogin = () => {
  router.push(ROUTES.LOGIN)
}

const abrirTerminos = () => {
  // Abrir términos y condiciones en nueva pestaña
  window.open(ROUTES.TERMINOS, '_blank')
}

const abrirPolitica = () => {
  // Abrir política de privacidad en nueva pestaña
  window.open(ROUTES.POLITICA, '_blank')
}
</script>

<style scoped>
.registrar-profesional {
  min-height: 100vh;
}

.logo {
  cursor: pointer;
}

input[type="checkbox"] {
  margin: 0;
}

input.error, textarea.error {
  border-color: #ef4444;
}

.cta:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

textarea {
  width: 100%;
  padding: 12px;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  font-size: 14px;
  font-family: inherit;
  resize: vertical;
  min-height: 80px;
}

textarea:focus {
  outline: none;
  border-color: var(--brand);
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

/* Estilos para opciones de tipo de servicio */
.tipo-servicio-option {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 20px;
  border: 2px solid #e2e8f0;
  border-radius: 12px;
  cursor: pointer;
  transition: all 0.2s ease;
  background: white;
}

.tipo-servicio-option:hover {
  border-color: #cbd5e1;
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.tipo-servicio-option.selected {
  border-color: var(--brand);
  background: #f0f9ff;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.tipo-icon {
  font-size: 2.5rem;
  line-height: 1;
}

.tipo-content h3 {
  margin: 0 0 4px 0;
  font-size: 1.1rem;
  font-weight: 600;
  color: var(--text);
}

.tipo-content p {
  margin: 0;
  font-size: 0.9rem;
  color: #64748b;
  line-height: 1.4;
}

.oficio-icon {
  font-size: 1.2rem;
  margin-right: 4px;
}
</style>
