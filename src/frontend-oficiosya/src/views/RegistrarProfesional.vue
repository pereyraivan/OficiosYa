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

    <!-- Formulario de Registro de Profesional con Wizard -->
    <section class="section" style="background: linear-gradient(135deg, #f0fdf4 0%, #ecfdf5 50%, #f0f9ff 100%); min-height: calc(100vh - 80px);">
      <div class="container" style="max-width: 800px; padding-top: 40px;">
        <div class="card" style="padding: 40px; background: rgba(255, 255, 255, 0.95); backdrop-filter: blur(10px); border: 1px solid rgba(34, 197, 94, 0.1);">
          <h2 style="text-align: center; margin-bottom: 10px;">Crear cuenta de profesional/oficiante</h2>
          <p style="text-align: center; color: var(--muted); margin-bottom: 30px;">
            Registrate como profesional u oficiante para empezar a recibir trabajos
          </p>

          <Wizard
            :steps="wizardSteps"
            :can-proceed="canProceedToNext"
            @finish="registrarProfesional"
            @step-change="onStepChange"
          >
            <!-- Paso 1: Datos Personales -->
            <template #step-1>
              <div class="step-content">
                <h3 style="margin-bottom: 24px; color: var(--text);">Datos Personales</h3>
                
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
                      @blur="validateField('password', formulario.password)"
                      @input="validatePasswordOnChange"
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
                      @blur="validateField('confirmarPassword', formulario.confirmarPassword)"
                      @input="validateConfirmPassword"
                    />
                    <span v-if="errores.confirmarPassword" class="error">{{ errores.confirmarPassword }}</span>
                  </div>
                </div>

                <!-- Dirección -->
                <div class="form-group">
                  <label for="direccion">Dirección (opcional)</label>
                  <input
                    id="direccion"
                    v-model="formulario.direccion"
                    type="text"
                    placeholder="Calle, número, piso, depto"
                  />
                </div>

                <!-- Provincia (primero) -->
                <div class="form-group">
                  <label for="provincia">Provincia *</label>
                  <select
                    id="provincia"
                    v-model="formulario.provincia"
                    required
                    :class="{ 'error': errores.provincia }"
                    style="width: 100%; padding: 12px; border: 1px solid #e2e8f0; border-radius: 6px; font-size: 14px;"
                    @change="onProvinciaChange"
                  >
                    <option value="">Seleccionar provincia</option>
                    <option v-for="provincia in PROVINCIAS_ARGENTINA" :key="provincia" :value="provincia">
                      {{ provincia }}
                    </option>
                  </select>
                  <span v-if="errores.provincia" class="error">{{ errores.provincia }}</span>
                </div>

                <!-- Ciudad (después de provincia) -->
                <div class="form-group">
                  <label for="ciudad">Ciudad *</label>
                  <CiudadAutocomplete
                    id="ciudad"
                    v-model="formulario.ciudad"
                    :placeholder="formulario.provincia ? 'Ej: San Miguel de Tucumán' : 'Primero selecciona una provincia'"
                    :provincia="formulario.provincia"
                    :disabled="!formulario.provincia"
                    :class="{ 'error': errores.ciudad }"
                    @ciudad-seleccionada="onCiudadSeleccionada"
                  />
                  <span v-if="errores.ciudad" class="error">{{ errores.ciudad }}</span>
                  <span v-if="!formulario.provincia" class="small" style="color: #64748b; margin-top: 4px; display: block;">
                    Primero debes seleccionar una provincia
                  </span>
                </div>

                <!-- Código Postal -->
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
            </template>

            <!-- Paso 2: Datos del Oficio/Profesión -->
            <template #step-2>
              <div class="step-content">
                <h3 style="margin-bottom: 24px; color: var(--text);">Datos del Oficio/Profesión</h3>
                
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
                  <div v-if="cargandoCategorias" style="padding: 20px; text-align: center;">
                    <span class="small">Cargando categorías...</span>
                  </div>
                  <template v-else>
                    <div v-if="categoriasDisponibles.length === 0" style="padding: 20px; text-align: center;">
                      <span class="small" style="color: #ef4444;">
                        {{ errores && errores.general && (errores.general.includes('categorías') || errores.general.includes('Error')) 
                          ? errores.general 
                          : 'No hay categorías disponibles para este tipo. Por favor, intenta recargar la página o contacta al administrador.' }}
                      </span>
                      <button 
                        v-if="!cargandoCategorias"
                        @click="cargarCategorias(tipoServicio === 'oficiante' ? 'Oficiante' : 'Profesional')"
                        style="margin-top: 10px; padding: 8px 16px; background: var(--brand); color: white; border: none; border-radius: 4px; cursor: pointer;"
                      >
                        Reintentar
                      </button>
                    </div>
                    <div v-else class="categorias-container">
                      <!-- Buscador de servicios -->
                      <div class="buscador-servicios" style="margin-bottom: 16px;">
                        <div style="position: relative;">
                          <svg 
                            width="18" 
                            height="18" 
                            viewBox="0 0 24 24" 
                            fill="none" 
                            xmlns="http://www.w3.org/2000/svg"
                            style="position: absolute; left: 12px; top: 50%; transform: translateY(-50%); color: #64748b; pointer-events: none;"
                          >
                            <circle cx="11" cy="11" r="8" stroke="currentColor" stroke-width="2"/>
                            <path d="m21 21-4.35-4.35" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
                          </svg>
                          <input
                            v-model="busquedaServicios"
                            type="text"
                            placeholder="Buscar servicios (ej: Plomero, Electricista, Médico...)"
                            class="input-busqueda-servicios"
                          />
                          <button
                            v-if="busquedaServicios"
                            @click="busquedaServicios = ''"
                            style="position: absolute; right: 12px; top: 50%; transform: translateY(-50%); background: none; border: none; color: #64748b; cursor: pointer; padding: 4px;"
                            type="button"
                          >
                            ✕
                          </button>
                        </div>
                        <div v-if="busquedaServicios && categoriasFiltradas.length === 0" class="sin-resultados-busqueda" style="margin-top: 8px; padding: 12px; background: #f8fafc; border-radius: 6px; text-align: center; color: #64748b; font-size: 14px;">
                          No se encontraron servicios que coincidan con "{{ busquedaServicios }}"
                        </div>
                      </div>
                      
                      <!-- Grid de categorías filtradas -->
                      <div v-if="categoriasFiltradas.length > 0" class="categorias-grid">
                        <div 
                          v-for="categoria in categoriasFiltradas" 
                          :key="`cat-${categoria.id}-${categoria.nombre}`" 
                          class="categoria-checkbox"
                          :class="{ 'selected': formulario?.oficios && Array.isArray(formulario.oficios) && formulario.oficios.includes(categoria.nombre) }"
                          @click="toggleCategoria(categoria.nombre)">
                          <span>{{ categoria.nombre }}</span>
                        </div>
                      </div>
                      <div v-else-if="!busquedaServicios" class="categorias-grid">
                        <div 
                          v-for="categoria in categoriasDisponibles" 
                          :key="`cat-${categoria.id}-${categoria.nombre}`" 
                          class="categoria-checkbox"
                          :class="{ 'selected': formulario?.oficios && Array.isArray(formulario.oficios) && formulario.oficios.includes(categoria.nombre) }"
                          @click="toggleCategoria(categoria.nombre)">
                          <span>{{ categoria.nombre }}</span>
                        </div>
                      </div>
                    </div>
                  </template>
                  <span v-if="errores.oficios" class="error">{{ errores.oficios }}</span>
                </div>

                <!-- Zonas de Cobertura -->
                <div class="form-group" style="margin-top: 24px;">
                  <label style="margin-bottom: 12px; display: block; font-weight: 500;">
                    Zonas de Cobertura
                  </label>
                  <MapaZonasCobertura
                    v-model="formulario.zonasCobertura"
                    :provincia="formulario.provincia"
                    :show-map="false"
                  />
                </div>

                <!-- Descripción Profesional -->
                <div class="form-group">
                  <label for="descripcion">Descripción profesional (opcional)</label>
                  <textarea
                    id="descripcion"
                    v-model="formulario.descripcion"
                    rows="4"
                    placeholder="Contanos sobre tu experiencia, especialidades, años trabajando..."
                  ></textarea>
                </div>

                <!-- Carga de Certificados -->
                <div class="form-group">
                  <label style="margin-bottom: 12px; display: block; font-weight: 500;">
                    Certificados y Documentos (Opcional)
                  </label>
                  <p style="font-size: 0.9rem; color: #64748b; margin-bottom: 16px;">
                    Subí tus certificados, títulos o documentos que acrediten tu experiencia profesional
                  </p>

                  <!-- Lista de documentos cargados -->
                  <div v-if="documentos.length > 0" class="documentos-list">
                    <div v-for="(documento, index) in documentos" :key="index" class="documento-item">
                      <div class="documento-info">
                        <span class="documento-icon">📄</span>
                        <span class="documento-nombre">{{ documento.nombre }}</span>
                        <span class="documento-tamano">({{ documento.tamano }})</span>
                      </div>
                      <button 
                        type="button" 
                        class="btn-eliminar"
                        @click="eliminarDocumento(index)"
                      >
                        ✕
                      </button>
                    </div>
                  </div>

                  <!-- Input para cargar documentos -->
                  <div class="upload-area" 
                       :class="{ 'dragging': isDragging }"
                       @dragover.prevent="isDragging = true"
                       @dragleave.prevent="isDragging = false"
                       @drop.prevent="handleFileDrop">
                    <input
                      type="file"
                      ref="fileInput"
                      accept=".pdf,.jpg,.jpeg,.png,.doc,.docx"
                      multiple
                      style="display: none;"
                      @change="handleFileChange"
                    />
                    <div class="upload-content">
                      <div class="upload-icon">📎</div>
                      <div class="upload-text">
                        <p>Arrastrá archivos aquí o <span class="upload-link" @click="abrirSelectorArchivos">hacé clic para seleccionar</span></p>
                        <p class="upload-hint">Formatos: PDF, JPG, PNG, DOC, DOCX (máx. 5MB cada uno)</p>
                      </div>
                    </div>
                  </div>
                  <span v-if="errores.documentos" class="error">{{ errores.documentos }}</span>
                </div>
              </div>
            </template>

            <!-- Paso 3: Métodos de Pago -->
            <template #step-3>
              <div class="step-content">
                <h3 style="margin-bottom: 24px; color: var(--text);">Suscripción y Pago</h3>
                
                <!-- Selección de Plan -->
                <div class="form-group" style="margin-bottom: 24px;">
                  <label style="margin-bottom: 12px; display: block; font-weight: 500;">
                    Seleccioná tu plan *
                  </label>
                  <div v-if="cargandoPlanes" style="padding: 20px; text-align: center;">
                    <span class="small">Cargando planes...</span>
                  </div>
                  <div v-else class="planes-container">
                    <div 
                      v-for="plan in planesDisponibles" 
                      :key="plan.id"
                      class="plan-option"
                      :class="{ 'selected': planSeleccionado?.id === plan.id }"
                      @click="seleccionarPlan(plan)"
                    >
                      <div class="plan-header">
                        <h4>{{ plan.nombre }}</h4>
                        <div class="plan-badge" v-if="plan.nombre === 'Premium'">⭐ Popular</div>
                      </div>
                      <p class="plan-descripcion">{{ plan.descripcion }}</p>
                      <div class="plan-precio">
                        <span class="precio-mensual">${{ plan.precioMensual.toLocaleString('es-AR') }}/mes</span>
                      </div>
                    </div>
                  </div>
                </div>

                <!-- Selección de Duración -->
                <div v-if="planSeleccionado" class="form-group" style="margin-bottom: 24px;">
                  <label style="margin-bottom: 12px; display: block; font-weight: 500;">
                    Duración de la suscripción *
                  </label>
                  <div class="duracion-options">
                    <div 
                      v-for="opcion in opcionesDuracion" 
                      :key="opcion.meses"
                      class="duracion-option"
                      :class="{ 'selected': mesesSuscripcion === opcion.meses }"
                      @click="seleccionarDuracion(opcion.meses)"
                    >
                      <div class="duracion-label">{{ opcion.label }}</div>
                      <div class="duracion-precio">
                        <span class="precio-total">${{ calcularPrecio(opcion.meses).toLocaleString('es-AR') }}</span>
                        <span v-if="opcion.descuento > 0" class="descuento-badge">
                          {{ opcion.descuento }}% OFF
                        </span>
                      </div>
                      <div v-if="opcion.descuento > 0" class="precio-sin-descuento">
                        ${{ calcularPrecioSinDescuento(opcion.meses).toLocaleString('es-AR') }}
                      </div>
                    </div>
                  </div>
                </div>

                <p style="color: #64748b; margin-bottom: 24px; text-align: center;">
                  Seleccioná tu método de pago preferido
                </p>

                <div class="payment-methods">
                  <div 
                    v-for="metodo in metodosPago" 
                    :key="metodo.id"
                    class="payment-method"
                    :class="{ 'selected': metodoPagoSeleccionado === metodo.id }"
                    @click="seleccionarMetodoPago(metodo.id)"
                  >
                    <div class="payment-icon">{{ metodo.icono }}</div>
                    <div class="payment-info">
                      <h4>{{ metodo.nombre }}</h4>
                      <p>{{ metodo.descripcion }}</p>
                    </div>
                    <div class="payment-check">
                      <span v-if="metodoPagoSeleccionado === metodo.id">✓</span>
                    </div>
                  </div>
                </div>

                <!-- Cupón de Pago (para Rapipago/Pagos Fáciles) -->
                <div v-if="metodoPagoSeleccionado === 'rapipago' || metodoPagoSeleccionado === 'pagosfaciles'" class="payment-coupon">
                  <div class="coupon-container">
                    <div class="coupon-header">
                      <h4>📄 Cupón de Pago</h4>
                      <p>Presentá este cupón en cualquier sucursal de {{ metodoPagoSeleccionado === 'rapipago' ? 'Rapipago' : 'Pagos Fáciles' }}</p>
                    </div>
                    <div class="coupon-content">
                      <div class="coupon-field">
                        <span class="coupon-label">Código de Pago:</span>
                        <span class="coupon-value">{{ codigoPago }}</span>
                      </div>
                      <div class="coupon-field">
                        <span class="coupon-label">Monto:</span>
                        <span class="coupon-value">${{ montoPago.toFixed(2) }}</span>
                      </div>
                      <div class="coupon-field">
                        <span class="coupon-label">Vencimiento:</span>
                        <span class="coupon-value">{{ fechaVencimiento }}</span>
                      </div>
                    </div>
                    <button type="button" class="btn-print" @click="imprimirCupon">
                      🖨️ Imprimir Cupón
                    </button>
                  </div>
                </div>

                <!-- Información de Transferencia -->
                <div v-if="metodoPagoSeleccionado === 'transferencia'" class="payment-info">
                  <div class="info-box">
                    <h4>💳 Datos para Transferencia</h4>
                    <div class="info-content">
                      <p><strong>CBU:</strong> 1234567890123456789012</p>
                      <p><strong>Alias:</strong> OFICIOSYA.SUSCRIPCION</p>
                      <p><strong>Razón Social:</strong> OficiosYa S.A.</p>
                      <p><strong>Monto:</strong> ${{ montoPago.toFixed(2) }}</p>
                      <p style="margin-top: 12px; color: #64748b; font-size: 0.9rem;">
                        Una vez realizada la transferencia, enviá el comprobante a pagos@oficiosya.com
                      </p>
                    </div>
                  </div>
                </div>

                <!-- Información de Tarjeta -->
                <div v-if="metodoPagoSeleccionado === 'tarjeta'" class="payment-info">
                  <div class="info-box">
                    <p style="color: #64748b; text-align: center;">
                      El pago con tarjeta de crédito será procesado de forma segura en el siguiente paso.
                    </p>
                    <p style="color: #64748b; text-align: center; margin-top: 12px;">
                      <strong>Monto:</strong> ${{ montoPago.toFixed(2) }}
                    </p>
                  </div>
                </div>

                <!-- Términos y condiciones -->
                <div class="form-group" style="margin-top: 30px;">
                  <div class="terms-container">
                    <label class="terms-label">
                      <input
                        v-model="formulario.aceptaTerminos"
                        type="checkbox"
                        required
                        class="terms-checkbox"
                      />
                      <span class="terms-text">
                        Acepto los 
                        <a href="#" @click.prevent="abrirTerminos" class="terms-link">términos y condiciones</a> 
                        y la 
                        <a href="#" @click.prevent="abrirPolitica" class="terms-link">política de privacidad</a>
                      </span>
                    </label>
                  </div>
                  <span v-if="errores.aceptaTerminos" class="error">{{ errores.aceptaTerminos }}</span>
                </div>

                <!-- Mensaje de error general -->
                <div v-if="errores.general" class="error" style="text-align: center; margin-bottom: 16px;">
                  {{ errores.general }}
                </div>
              </div>
            </template>
          </Wizard>

          <!-- Enlace a login -->
          <div style="text-align: center; margin-top: 20px;">
            <span class="small">¿Ya tenés cuenta? </span>
            <a href="#" @click="irALogin" style="color: var(--brand); font-weight: 500;">
              Iniciar sesión
            </a>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { profesionalService } from '../services/profesionalService'
import { useProfessionalRegistrationForm } from '../composables/useForm'
import { PROVINCIAS_ARGENTINA, MESSAGES, ROUTES } from '../utils/constants'
import CiudadAutocomplete from '../components/CiudadAutocomplete.vue'
import MapaZonasCobertura from '../components/MapaZonasCobertura.vue'
import Wizard from '../components/Wizard.vue'
import type { CategoriaServicio } from '../types'
import type { CiudadSugerida } from '../services/geocodingService'

const router = useRouter()

// Usar el composable para manejar el formulario
const {
  form: formulario,
  errors: errores,
  handleSubmit,
  setError,
  validateField
} = useProfessionalRegistrationForm()

// Asegurar que formulario.oficios esté inicializado como array
if (!formulario.oficios || !Array.isArray(formulario.oficios)) {
  formulario.oficios = []
}

// Estado adicional
const emailVerificando = ref(false)
const tipoServicio = ref<'oficiante' | 'profesional' | null>(null)
const documentos = ref<Array<{ nombre: string; archivo: File; tamano: string }>>([])
const isDragging = ref(false)
const categoriasCargadas = ref<CategoriaServicio[]>([])
const cargandoCategorias = ref(false)
const currentStep = ref(1)
const fileInput = ref<HTMLInputElement | null>(null)
const busquedaServicios = ref('')

// Métodos de pago
const metodosPago = [
  {
    id: 'transferencia',
    nombre: 'Transferencia Bancaria',
    descripcion: 'Transferí desde tu banco',
    icono: '🏦'
  },
  {
    id: 'tarjeta',
    nombre: 'Tarjeta de Crédito',
    descripcion: 'Visa, Mastercard, Amex',
    icono: '💳'
  },
  {
    id: 'rapipago',
    nombre: 'Rapipago',
    descripcion: 'Pagá en cualquier sucursal',
    icono: '🏪'
  },
  {
    id: 'pagosfaciles',
    nombre: 'Pagos Fáciles',
    descripcion: 'Pagá en cualquier sucursal',
    icono: '🏬'
  }
]

const metodoPagoSeleccionado = ref<string | null>(null)
const montoPago = ref(5000) // Se calculará dinámicamente
const codigoPago = ref('')
const fechaVencimiento = ref('')
const planesDisponibles = ref<any[]>([])
const cargandoPlanes = ref(false)
const planSeleccionado = ref<any>(null)
const mesesSuscripcion = ref(1)

// Opciones de duración
const opcionesDuracion = computed(() => {
  if (!planSeleccionado.value) return []
  
  return [
    { meses: 1, label: '1 Mes', descuento: 0 },
    { meses: 3, label: '3 Meses', descuento: planSeleccionado.value.descuentoTrimestral || 0 },
    { meses: 6, label: '6 Meses', descuento: planSeleccionado.value.descuentoSemestral || 0 },
    { meses: 12, label: '12 Meses', descuento: planSeleccionado.value.descuentoAnual || 0 }
  ]
})

// Generar código de pago único
const generarCodigoPago = () => {
  const timestamp = Date.now()
  const random = Math.floor(Math.random() * 10000)
  codigoPago.value = `OFY-${timestamp}-${random.toString().padStart(4, '0')}`
  
  // Fecha de vencimiento: 7 días desde hoy
  const fecha = new Date()
  fecha.setDate(fecha.getDate() + 7)
  fechaVencimiento.value = fecha.toLocaleDateString('es-AR', { 
    year: 'numeric', 
    month: 'long', 
    day: 'numeric' 
  })
}

// Wizard steps
const wizardSteps = [
  { label: 'Datos Personales' },
  { label: 'Oficio/Profesión' },
  { label: 'Pago' }
]

// Validar si se puede avanzar al siguiente paso
const canProceedToNext = computed(() => {
  if (currentStep.value === 1) {
    // Validar datos personales
    return formulario && formulario.nombre && 
           formulario.apellido && 
           formulario.email && 
           formulario.password && 
           formulario.confirmarPassword &&
           formulario.ciudad && 
           formulario.provincia &&
           errores && !errores.nombre &&
           !errores.apellido &&
           !errores.email &&
           !errores.password &&
           !errores.confirmarPassword &&
           !errores.ciudad &&
           !errores.provincia
  } else if (currentStep.value === 2) {
    // Validar datos del oficio
    return tipoServicio.value !== null && 
           formulario?.oficios && Array.isArray(formulario.oficios) && 
           formulario.oficios.length > 0
  } else if (currentStep.value === 3) {
    // Validar plan, método de pago y términos
    return planSeleccionado.value !== null &&
           metodoPagoSeleccionado.value !== null && 
           formulario?.aceptaTerminos === true
  }
  return false
})

// Función para normalizar texto (remover acentos para búsqueda más flexible)
const normalizeText = (text: string): string => {
  return text
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '') // Remover diacríticos
    .trim()
}

// Computed para categorías disponibles según el tipo seleccionado
const categoriasDisponibles = computed(() => {
  if (!tipoServicio.value) {
    return []
  }
  
  // Filtrar categorías por el tipo seleccionado
  const tipoBuscado = tipoServicio.value === 'oficiante' ? 'Oficiante' : 'Profesional'
  return categoriasCargadas.value.filter(cat => 
    cat.tipo && cat.tipo.toLowerCase() === tipoBuscado.toLowerCase()
  )
})

// Computed para categorías filtradas por búsqueda
const categoriasFiltradas = computed(() => {
  if (!busquedaServicios.value.trim()) {
    return categoriasDisponibles.value
  }
  
  const busquedaNormalizada = normalizeText(busquedaServicios.value)
  return categoriasDisponibles.value.filter(categoria => {
    const nombreNormalizado = normalizeText(categoria.nombre)
    const descripcionNormalizada = categoria.descripcion ? normalizeText(categoria.descripcion) : ''
    const grupoNormalizado = categoria.grupo ? normalizeText(categoria.grupo) : ''
    
    return nombreNormalizado.includes(busquedaNormalizada) ||
           descripcionNormalizada.includes(busquedaNormalizada) ||
           grupoNormalizado.includes(busquedaNormalizada)
  })
})

// Función para cargar categorías
const cargarCategorias = async (tipo?: 'Oficiante' | 'Profesional') => {
  if (!tipo) {
    categoriasCargadas.value = []
    return
  }

  cargandoCategorias.value = true
  // Limpiar errores previos
  if (errores?.general && errores.general.includes('categorías')) {
    setError('general', '')
  }
  
  try {
    const categorias = await profesionalService.obtenerCategorias(tipo)
    categoriasCargadas.value = categorias || []
    
    // Si no hay categorías y hay un tipo seleccionado, mostrar mensaje
    if (categoriasCargadas.value.length === 0 && tipo) {
      console.warn(`No se encontraron categorías para el tipo: ${tipo}`)
      setError('general', `No se encontraron categorías disponibles para el tipo "${tipo}". Por favor, contacta al administrador.`)
    }
  } catch (error: any) {
    console.error('Error al cargar categorías:', error)
    let mensajeError = 'Error al cargar las categorías disponibles'
    
    if (error.response?.data) {
      // Si el error viene del backend con información específica
      if (error.response.data.message) {
        mensajeError = error.response.data.message
      } else if (error.response.data.error) {
        mensajeError = error.response.data.error
      }
      
      // Si el error menciona TipoServicios, dar instrucciones más claras
      if (error.response.data.message?.includes('TipoServicios') || 
          error.response.data.solution) {
        mensajeError = 'Error de configuración de base de datos. Por favor, contacta al administrador del sistema.'
        console.error('Error de base de datos:', error.response.data)
      }
    } else if (error.message) {
      mensajeError = error.message
    }
    
    if (errores && setError) {
      setError('general', mensajeError)
    }
    categoriasCargadas.value = []
  } finally {
    cargandoCategorias.value = false
  }
}

// Cargar planes al montar el componente (no cargar categorías hasta que se seleccione un tipo)
onMounted(async () => {
  await cargarPlanes()
  generarCodigoPago()
  // Seleccionar plan básico por defecto
  if (planesDisponibles.value.length > 0) {
    seleccionarPlan(planesDisponibles.value[0])
  }
})

// Cargar planes de suscripción
const cargarPlanes = async () => {
  cargandoPlanes.value = true
  try {
    const planes = await profesionalService.obtenerPlanesSuscripcion()
    planesDisponibles.value = planes || []
    
    // Si no hay planes, mostrar mensaje pero no bloquear el formulario
    if (planesDisponibles.value.length === 0) {
      console.warn('No se encontraron planes de suscripción disponibles')
    }
    
    // Si no hay planes, no es crítico, solo log
    if (planesDisponibles.value.length === 0) {
      console.warn('No se encontraron planes de suscripción')
    }
  } catch (error: any) {
    console.error('Error al cargar planes de suscripción:', error)
    // No establecer error general para planes, solo log
    // Los planes son opcionales y no deberían bloquear el registro
    planesDisponibles.value = []
  } finally {
    cargandoPlanes.value = false
  }
}

// Seleccionar plan
const seleccionarPlan = (plan: any) => {
  planSeleccionado.value = plan
  mesesSuscripcion.value = 1 // Resetear a 1 mes cuando cambia el plan
  actualizarMontoPago()
}

// Seleccionar duración
const seleccionarDuracion = (meses: number) => {
  mesesSuscripcion.value = meses
  actualizarMontoPago()
}

// Calcular precio con descuento
const calcularPrecio = (meses: number): number => {
  if (!planSeleccionado.value) return 0
  
  const precioBase = meses === 1 ? planSeleccionado.value.precioMensual :
                     meses === 3 ? planSeleccionado.value.precioTrimestral :
                     meses === 6 ? planSeleccionado.value.precioSemestral :
                     meses === 12 ? planSeleccionado.value.precioAnual :
                     planSeleccionado.value.precioMensual * meses
  
  return precioBase
}

// Calcular precio sin descuento (para mostrar el ahorro)
const calcularPrecioSinDescuento = (meses: number): number => {
  if (!planSeleccionado.value) return 0
  return planSeleccionado.value.precioMensual * meses
}

// Actualizar monto de pago
const actualizarMontoPago = () => {
  montoPago.value = calcularPrecio(mesesSuscripcion.value)
}

// Watcher para recargar categorías cuando cambia el tipo de servicio
watch(tipoServicio, async (nuevoTipo) => {
  // Limpiar búsqueda cuando cambia el tipo de servicio
  busquedaServicios.value = ''
  // Limpiar selección anterior
  if (formulario && Array.isArray(formulario.oficios)) {
    formulario.oficios = []
  }
  
  // Limpiar errores de oficios
  if (errores && errores.oficios) {
    setError('oficios', '')
  }
  
  if (nuevoTipo) {
    const tipo = nuevoTipo === 'oficiante' ? 'Oficiante' : 'Profesional'
    // Cargar categorías del nuevo tipo
    await cargarCategorias(tipo)
  } else {
    // Si se deselecciona el tipo, limpiar categorías
    categoriasCargadas.value = []
  }
}, { immediate: false })

// Métodos
const verificarEmail = async () => {
  if (!formulario.email || !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formulario.email)) {
    return
  }

  emailVerificando.value = true
  try {
    const { useAuthStore } = await import('../stores/auth')
    const authStore = useAuthStore()
    const disponible = await authStore.verificarEmail(formulario.email)
    if (!disponible) {
      setError('email', 'Este email ya está registrado')
    }
  } catch (error) {
    // Error silencioso
  } finally {
    emailVerificando.value = false
  }
}

const onProvinciaChange = () => {
  // Limpiar la ciudad cuando se cambia la provincia
  formulario.ciudad = ''
  // Limpiar el error de ciudad si existe
  if (errores.ciudad) {
    setError('ciudad', '')
  }
}

const onCiudadSeleccionada = (ciudad: CiudadSugerida) => {
  // Ya no necesitamos actualizar la provincia desde la ciudad
  // porque ahora la provincia se selecciona primero
  // Pero podemos validar que la ciudad pertenezca a la provincia seleccionada
  if (formulario.provincia && ciudad.provincia) {
    const provinciaEncontrada = PROVINCIAS_ARGENTINA.find(
      p => p.toLowerCase().includes(ciudad.provincia!.toLowerCase()) ||
           ciudad.provincia!.toLowerCase().includes(p.toLowerCase())
    )
    // Si la ciudad no pertenece a la provincia seleccionada, mostrar error
    if (provinciaEncontrada && provinciaEncontrada !== formulario.provincia) {
      setError('ciudad', 'La ciudad seleccionada no pertenece a la provincia elegida')
      formulario.ciudad = ''
    }
  }
}

const onStepChange = (step: number) => {
  currentStep.value = step
}

const seleccionarMetodoPago = (metodoId: string) => {
  metodoPagoSeleccionado.value = metodoId
  if (metodoId === 'rapipago' || metodoId === 'pagosfaciles') {
    generarCodigoPago()
  }
}

// Watcher para actualizar monto cuando cambia la duración
watch([planSeleccionado, mesesSuscripcion], () => {
  actualizarMontoPago()
})

const imprimirCupon = () => {
  window.print()
}

const registrarProfesional = async () => {
  const success = await handleSubmit()
  
  if (success) {
    try {
      // Validar que se haya seleccionado al menos un oficio
      if (!formulario?.oficios || !Array.isArray(formulario.oficios) || formulario.oficios.length === 0) {
        setError('oficios', 'Debes seleccionar al menos un oficio o categoría')
        return
      }

      // Validar método de pago
      if (!metodoPagoSeleccionado.value) {
        setError('general', 'Debes seleccionar un método de pago')
        return
      }

      // Preparar datos para enviar
      const datosProfesional = {
        nombre: formulario.nombre.trim(),
        apellido: formulario.apellido.trim(),
        email: formulario.email.trim(),
        password: formulario.password,
        telefono: formulario.telefono?.trim() || undefined,
        direccion: formulario.direccion?.trim() || undefined,
        ciudad: formulario.ciudad.trim(),
        provincia: formulario.provincia.trim(),
        codigoPostal: formulario.codigoPostal?.trim() || undefined,
        descripcion: formulario.descripcion?.trim() || undefined,
        oficios: formulario?.oficios || [],
        zonasCobertura: formulario?.zonasCobertura && formulario.zonasCobertura.length > 0
          ? formulario.zonasCobertura.map((z: any) => ({
              localidad: z.localidad,
              provincia: z.provincia,
              departamento: z.departamento,
              latitud: z.latitud,
              longitud: z.longitud,
              codigoPostal: z.codigoPostal
            }))
          : undefined,
        metodoPago: metodoPagoSeleccionado.value,
        codigoPago: (metodoPagoSeleccionado.value === 'rapipago' || metodoPagoSeleccionado.value === 'pagosfaciles') 
          ? codigoPago.value 
          : undefined,
        mesesSuscripcion: mesesSuscripcion.value
      }

      await profesionalService.registrar(datosProfesional)
      
      // Registro exitoso
      alert(MESSAGES.SUCCESS.PROFESSIONAL_REGISTERED)
      router.push(ROUTES.HOME)
    } catch (error: any) {
      const errorMessage = error.response?.data?.message || 
                          error.response?.data?.error || 
                          error.message || 
                          MESSAGES.ERROR.GENERAL
      setError('general', errorMessage)
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
  window.open(ROUTES.TERMINOS, '_blank')
}

const abrirPolitica = () => {
  window.open(ROUTES.POLITICA, '_blank')
}

// Funciones para manejo de documentos
const formatFileSize = (bytes: number): string => {
  if (bytes === 0) return '0 Bytes'
  const k = 1024
  const sizes = ['Bytes', 'KB', 'MB', 'GB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i]
}

const validateFile = (file: File): boolean => {
  const validTypes = ['application/pdf', 'image/jpeg', 'image/jpg', 'image/png', 
                      'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document']
  const maxSize = 5 * 1024 * 1024 // 5MB

  if (!validTypes.includes(file.type)) {
    alert('Tipo de archivo no válido. Solo se permiten PDF, JPG, PNG, DOC, DOCX')
    return false
  }

  if (file.size > maxSize) {
    alert('El archivo es demasiado grande. El tamaño máximo es 5MB')
    return false
  }

  return true
}

const handleFileChange = (event: Event) => {
  const target = event.target as HTMLInputElement
  if (target.files && target.files.length > 0) {
    Array.from(target.files).forEach(file => {
      if (validateFile(file)) {
        documentos.value.push({
          nombre: file.name,
          archivo: file,
          tamano: formatFileSize(file.size)
        })
      }
    })
  }
  isDragging.value = false
}

const handleFileDrop = (event: DragEvent) => {
  const files = event.dataTransfer?.files
  if (files && files.length > 0) {
    Array.from(files).forEach(file => {
      if (validateFile(file)) {
        documentos.value.push({
          nombre: file.name,
          archivo: file,
          tamano: formatFileSize(file.size)
        })
      }
    })
  }
  isDragging.value = false
}

const eliminarDocumento = (index: number) => {
  documentos.value.splice(index, 1)
}

// Toggle categoría seleccionada
const toggleCategoria = (nombreCategoria: string) => {
  // Asegurarse de que formulario y formulario.oficios existan
  if (!formulario) {
    console.error('Formulario no está inicializado')
    return
  }
  
  // Asegurarse de que formulario.oficios sea un array
  if (!Array.isArray(formulario.oficios)) {
    formulario.oficios = []
  }
  
  const index = formulario.oficios.indexOf(nombreCategoria)
  if (index === -1) {
    formulario.oficios.push(nombreCategoria)
  } else {
    formulario.oficios.splice(index, 1)
  }
}

// Validar contraseña cuando cambia
const validatePasswordOnChange = () => {
  validateField('password', formulario.password)
  if (formulario.confirmarPassword) {
    validateConfirmPassword()
  }
}

// Validar confirmación de contraseña
const validateConfirmPassword = () => {
  validateField('confirmarPassword', formulario.confirmarPassword)
}

// Abrir selector de archivos
const abrirSelectorArchivos = () => {
  fileInput.value?.click()
}
</script>

<style scoped>
/* Estilos existentes del formulario */
.registrar-profesional {
  min-height: 100vh;
}

.logo {
  cursor: pointer;
}

.step-content {
  animation: fadeIn 0.3s ease;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.categorias-container {
  margin-top: 8px;
  max-height: 300px;
  overflow-y: auto;
  overflow-x: hidden;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  padding: 8px;
  background: #f8fafc;
}

.categorias-container::-webkit-scrollbar {
  width: 8px;
}

.categorias-container::-webkit-scrollbar-track {
  background: #f1f5f9;
  border-radius: 4px;
}

.categorias-container::-webkit-scrollbar-thumb {
  background: #cbd5e1;
  border-radius: 4px;
}

.categorias-container::-webkit-scrollbar-thumb:hover {
  background: #94a3b8;
}

/* Estilos para el buscador de servicios */
.buscador-servicios {
  margin-bottom: 16px;
}

.buscador-servicios > div {
  position: relative;
}

.input-busqueda-servicios {
  width: 100%;
  padding: 12px 40px 12px 40px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  font-size: 14px;
  transition: border-color 0.2s, box-shadow 0.2s;
  outline: none;
}

.input-busqueda-servicios:focus {
  border-color: var(--brand);
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.input-busqueda-servicios::placeholder {
  color: #94a3b8;
}

.sin-resultados-busqueda {
  margin-top: 8px;
  padding: 12px;
  background: #f8fafc;
  border-radius: 6px;
  text-align: center;
  color: #64748b;
  font-size: 14px;
}

.categorias-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 8px;
}

.categoria-checkbox {
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  padding: 12px;
  border: 2px solid #e2e8f0;
  border-radius: 6px;
  background: white;
  transition: all 0.2s;
  min-height: 44px;
  text-align: center;
  font-weight: 500;
  color: var(--text);
}

.categoria-checkbox:hover {
  border-color: #3b82f6;
  background: #f0f9ff;
  transform: translateY(-1px);
  box-shadow: 0 2px 4px rgba(59, 130, 246, 0.2);
}

.categoria-checkbox.selected {
  background: #3b82f6;
  border-color: #3b82f6;
  color: white;
  font-weight: 600;
  box-shadow: 0 2px 8px rgba(59, 130, 246, 0.3);
}

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

/* Estilos para métodos de pago */
.payment-methods {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 16px;
  margin-bottom: 24px;
}

.payment-method {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 20px;
  border: 2px solid #e2e8f0;
  border-radius: 12px;
  cursor: pointer;
  transition: all 0.2s ease;
  background: white;
  position: relative;
}

.payment-method:hover {
  border-color: #cbd5e1;
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.payment-method.selected {
  border-color: var(--brand);
  background: #f0f9ff;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.payment-icon {
  font-size: 2.5rem;
  line-height: 1;
}

.payment-info {
  flex: 1;
}

.payment-info h4 {
  margin: 0 0 4px 0;
  font-size: 1rem;
  font-weight: 600;
  color: var(--text);
}

.payment-info p {
  margin: 0;
  font-size: 0.85rem;
  color: #64748b;
}

.payment-check {
  width: 24px;
  height: 24px;
  border-radius: 50%;
  background: var(--brand);
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  font-size: 0.9rem;
}

.payment-method:not(.selected) .payment-check {
  display: none;
}

/* Estilos para cupón de pago */
.payment-coupon {
  margin-top: 24px;
}

.coupon-container {
  border: 2px dashed #cbd5e1;
  border-radius: 12px;
  padding: 24px;
  background: #f8fafc;
  text-align: center;
}

.coupon-header h4 {
  margin: 0 0 8px 0;
  font-size: 1.2rem;
  color: var(--text);
}

.coupon-header p {
  margin: 0 0 20px 0;
  color: #64748b;
  font-size: 0.9rem;
}

.coupon-content {
  background: white;
  border-radius: 8px;
  padding: 20px;
  margin-bottom: 16px;
}

.coupon-field {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px 0;
  border-bottom: 1px solid #e2e8f0;
}

.coupon-field:last-child {
  border-bottom: none;
}

.coupon-label {
  font-weight: 500;
  color: #64748b;
}

.coupon-value {
  font-weight: 600;
  color: var(--text);
  font-size: 1.1rem;
}

.btn-print {
  background: var(--brand);
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 8px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-print:hover {
  background: var(--brand-600);
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(59, 130, 246, 0.3);
}

/* Estilos para información de pago */
.payment-info {
  margin-top: 24px;
}

.info-box {
  border: 1px solid #e2e8f0;
  border-radius: 12px;
  padding: 20px;
  background: #f8fafc;
}

.info-box h4 {
  margin: 0 0 16px 0;
  font-size: 1.1rem;
  color: var(--text);
}

.info-content p {
  margin: 8px 0;
  color: var(--text);
}

/* Estilos para documentos */
.documentos-list {
  margin-bottom: 16px;
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.documento-item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 16px;
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
}

.documento-info {
  display: flex;
  align-items: center;
  gap: 12px;
  flex: 1;
}

.documento-icon {
  font-size: 1.5rem;
}

.documento-nombre {
  font-size: 0.9rem;
  color: var(--text);
  font-weight: 500;
}

.documento-tamano {
  font-size: 0.85rem;
  color: #64748b;
}

.btn-eliminar {
  background: #ef4444;
  color: white;
  border: none;
  border-radius: 50%;
  width: 28px;
  height: 28px;
  cursor: pointer;
  font-size: 1.2rem;
  line-height: 1;
}

.upload-area {
  border: 2px dashed #cbd5e1;
  border-radius: 12px;
  padding: 32px;
  text-align: center;
  background: #f8fafc;
  cursor: pointer;
}

.upload-area.dragging {
  border-color: var(--brand);
  background: #e0f2fe;
}

.upload-content {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 16px;
}

.upload-icon {
  font-size: 3rem;
}

.upload-link {
  color: var(--brand);
  text-decoration: underline;
  cursor: pointer;
}

.terms-container {
  margin: 20px 0;
}

.terms-label {
  display: flex;
  align-items: flex-start;
  gap: 12px;
  cursor: pointer;
}

.terms-checkbox {
  margin-top: 2px;
  width: 18px;
  height: 18px;
  accent-color: var(--brand);
}

.terms-link {
  color: var(--brand);
  text-decoration: underline;
}

input.error, textarea.error {
  border-color: #ef4444;
}

.error {
  color: #ef4444;
  font-size: 0.85rem;
  margin-top: 4px;
  display: block;
}

/* Estilos para planes de suscripción */
.planes-container {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 16px;
  margin-bottom: 20px;
}

.plan-option {
  border: 2px solid #e2e8f0;
  border-radius: 12px;
  padding: 20px;
  cursor: pointer;
  transition: all 0.2s ease;
  background: white;
  position: relative;
}

.plan-option:hover {
  border-color: #cbd5e1;
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.plan-option.selected {
  border-color: var(--brand);
  background: #f0f9ff;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.plan-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 8px;
}

.plan-header h4 {
  margin: 0;
  font-size: 1.2rem;
  font-weight: 600;
  color: var(--text);
}

.plan-badge {
  background: #fbbf24;
  color: white;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 0.75rem;
  font-weight: 600;
}

.plan-descripcion {
  margin: 0 0 12px 0;
  font-size: 0.9rem;
  color: #64748b;
  line-height: 1.4;
}

.plan-precio {
  margin-top: 12px;
}

.precio-mensual {
  font-size: 1.5rem;
  font-weight: 700;
  color: var(--brand);
}

/* Estilos para opciones de duración */
.duracion-options {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(140px, 1fr));
  gap: 12px;
}

.duracion-option {
  border: 2px solid #e2e8f0;
  border-radius: 8px;
  padding: 16px;
  cursor: pointer;
  transition: all 0.2s ease;
  background: white;
  text-align: center;
}

.duracion-option:hover {
  border-color: #cbd5e1;
  transform: translateY(-2px);
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.duracion-option.selected {
  border-color: var(--brand);
  background: #f0f9ff;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.duracion-label {
  font-weight: 600;
  color: var(--text);
  margin-bottom: 8px;
  display: block;
}

.duracion-precio {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 4px;
  margin-bottom: 4px;
}

.precio-total {
  font-size: 1.3rem;
  font-weight: 700;
  color: var(--brand);
}

.descuento-badge {
  background: #22c55e;
  color: white;
  padding: 2px 8px;
  border-radius: 4px;
  font-size: 0.75rem;
  font-weight: 600;
}

.precio-sin-descuento {
  font-size: 0.85rem;
  color: #94a3b8;
  text-decoration: line-through;
}

/* Responsive */
@media (max-width: 768px) {
  .categorias-grid {
    grid-template-columns: 1fr;
  }
  
  .tipo-servicio-option {
    flex-direction: column;
    text-align: center;
  }
  
  .payment-methods {
    grid-template-columns: 1fr;
  }
  
  .coupon-field {
    flex-direction: column;
    align-items: flex-start;
    gap: 4px;
  }

  .planes-container {
    grid-template-columns: 1fr;
  }

  .duracion-options {
    grid-template-columns: repeat(2, 1fr);
  }
}
</style>
