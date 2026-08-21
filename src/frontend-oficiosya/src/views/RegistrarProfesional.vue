<template>
  <div class="registrar-profesional">
    <!-- Formulario de Registro de Profesional con Wizard (el menú superior es el AppNav global) -->
    <section class="section" style="background: linear-gradient(135deg, #f0fdf4 0%, #ecfdf5 50%, #f0f9ff 100%); min-height: calc(100vh - 80px);">
      <div class="container" style="max-width: 800px; padding-top: 40px;">
        <div class="card" style="padding: 40px; background: rgba(255, 255, 255, 0.95); backdrop-filter: blur(10px); border: 1px solid rgba(34, 197, 94, 0.1);">
          <h2 style="text-align: center; margin-bottom: 10px;">Crear cuenta de profesional/oficiante</h2>
          <p style="text-align: center; color: var(--muted); margin-bottom: 30px;">
            Registrate como profesional u oficiante para empezar a recibir trabajos
          </p>

          <Wizard
            :steps="wizardSteps"
            :can-proceed="true"
            :validate-before-next="validarPasoActual"
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

                <!-- Mensaje de error (ej. imagen no aprobada por moderación) -->
                <div v-if="errores.general" class="error error-general-box" style="margin-bottom: 12px;">
                  {{ errores.general }}
                </div>
                <!-- Foto de perfil (opcional) -->
                <div class="form-group">
                  <label>Foto de perfil (opcional)</label>
                  <input
                    ref="fileInputFotoPerfil"
                    type="file"
                    accept="image/jpeg,image/png,image/webp,image/gif"
                    class="input-file"
                    @change="onFotoPerfilChange"
                  />
                  <div v-if="fotoPerfilBase64" class="foto-perfil-preview">
                    <img :src="fotoPerfilBase64" alt="Vista previa" />
                    <button type="button" class="btn-quitar-foto" @click="quitarFotoPerfil">Quitar foto</button>
                  </div>
                </div>
              </div>
            </template>

            <!-- Paso 2: Datos del Oficio/Profesión -->
            <template #step-2>
              <div class="step-content">
                <h3 style="margin-bottom: 24px; color: var(--text);">Datos del Oficio/Profesión</h3>
                <div v-if="errores.general || errores.oficios" class="error error-general-box" style="margin-bottom: 16px;">
                  <p v-if="errores.general">{{ errores.general }}</p>
                  <p v-if="errores.oficios">{{ errores.oficios }}</p>
                </div>
                <!-- Selección de Tipo de Servicio -->
                <div class="form-group" :class="{ 'error-border-wrap': errores.general && tipoServicio === null }">
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
                  <label class="label-oficios">
                    <span>Servicios que ofrecés *</span>
                    <span v-if="formulario?.oficios?.length" class="oficios-seleccionados-inline">
                      {{ formulario.oficios.join(', ') }}
                    </span>
                  </label>
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
                    :max-zonas="maxZonasCoberturaActual"
                  />
                  <p
                    v-if="!restriccionZonasPorPlanActiva"
                    class="aviso-zonas-free"
                  >
                    Si al final elegís el plan <strong>Free</strong>, solo vas a poder mantener <strong>1</strong> zona de cobertura.
                    Podés seleccionar varias ahora y luego ajustar.
                  </p>
                  <p
                    v-else-if="restriccionZonasPorPlanActiva && mensajeFreeZonas"
                    class="aviso-zonas-free"
                  >
                    {{ mensajeFreeZonas }}
                  </p>
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
                
                <!-- Mensajes de error de campos obligatorios (paso 3) -->
                <div v-if="errores.plan || errores.metodoPago || errores.aceptaTerminos" class="error error-general-box" style="margin-bottom: 16px;">
                  <p v-if="errores.plan">{{ errores.plan }}</p>
                  <p v-if="errores.metodoPago">{{ errores.metodoPago }}</p>
                  <p v-if="errores.aceptaTerminos">{{ errores.aceptaTerminos }}</p>
                </div>
                <!-- Selección de Plan -->
                <div class="form-group" style="margin-bottom: 24px;">
                  <label style="margin-bottom: 12px; display: block; font-weight: 500;">
                    Seleccioná tu plan *
                  </label>
                  <div v-if="cargandoPlanes" style="padding: 20px; text-align: center;">
                    <span class="small">Cargando planes...</span>
                  </div>
                  <div v-else class="planes-container" :class="{ 'error-border-wrap': errores.plan }">
                    <div 
                      v-for="plan in planesDisponibles" 
                      :key="plan.id"
                      class="plan-option"
                      :class="{ 'selected': planSeleccionado?.id === plan.id }"
                      @click="seleccionarPlan(plan)"
                    >
                      <div class="plan-header">
                        <h4>{{ plan.nombre }}</h4>
                        <div class="plan-badge" v-if="normalizeText(plan.nombre) === 'pro'">⭐ Más elegido</div>
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
                <span v-if="errores.plan" class="error">{{ errores.plan }}</span>

                <div
                  v-if="mensajeFreeZonas"
                  class="error error-general-box"
                  style="margin: 12px 0 16px 0; text-align: center;"
                >
                  {{ mensajeFreeZonas }}
                </div>

                <p style="color: #64748b; margin-bottom: 24px; text-align: center;">
                  Los planes de pago se abonan por <strong>transferencia</strong> (MISMO / CVU o alias).
                </p>

                <div
                  v-if="metodoPagoSeleccionado === 'transferencia'"
                  class="payment-methods"
                  :class="{ 'error-border': errores.metodoPago }"
                >
                  <div class="payment-method selected" style="cursor: default;">
                    <div class="payment-icon">🏦</div>
                    <div class="payment-info">
                      <h4>Transferencia bancaria o billetera</h4>
                      <p>Al finalizar el registro te mostraremos el <strong>código único</strong> para el concepto y los CVU/alias donde transferir.</p>
                    </div>
                    <div class="payment-check"><span>✓</span></div>
                  </div>
                </div>
                <span v-if="errores.metodoPago" class="error">{{ errores.metodoPago }}</span>

                <div v-if="metodoPagoSeleccionado === 'transferencia'" class="payment-info">
                  <div class="info-box">
                    <h4>💳 Cómo sigue</h4>
                    <div class="info-content">
                      <p><strong>Monto a transferir (estimado):</strong> ${{ montoPago.toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) }}</p>
                      <p style="margin-top: 12px; color: #64748b; font-size: 0.9rem;">
                        El monto y el código <strong>OY-…</strong> definitivos te los damos al confirmar el registro, para que coincida con tu suscripción en el sistema.
                      </p>
                    </div>
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

            <!-- Paso 4: Fotos de trabajos realizados (opcional) -->
            <template #step-4>
              <div class="step-content">
                <!-- Mensaje de error (moderación, etc.) visible en este paso para que el usuario pueda cambiar la imagen -->
                <div v-if="errores.general" ref="errorGeneralRef" class="error error-general-box" role="alert">
                  {{ errores.general }}
                  <p class="error-hint">Podés quitar la imagen problemática y elegir otra, luego volvé a intentar.</p>
                </div>
                <h3 style="margin-bottom: 16px; color: var(--text);">Fotos de trabajos realizados</h3>
                <p style="color: #64748b; margin-bottom: 20px; font-size: 0.95rem;">
                  Subí fotos de trabajos que hayas realizado (opcional). Se mostrarán en tu perfil para que los clientes vean tu experiencia.
                </p>
                <div class="form-group">
                  <input
                    type="file"
                    accept="image/jpeg,image/png,image/webp,image/gif"
                    multiple
                    class="input-file"
                    @change="onFotosTrabajoChange"
                  />
                </div>
                <div v-if="imagenesTrabajoBase64.length > 0" class="fotos-preview">
                  <div v-for="(img, i) in imagenesTrabajoBase64" :key="i" class="foto-preview-item">
                    <img :src="img" alt="Trabajo" class="foto-preview-img" />
                    <button type="button" class="foto-remove" @click="quitarFotoTrabajo(i)" title="Quitar">×</button>
                  </div>
                </div>
                <p v-else class="small" style="color: #94a3b8;">Aún no agregaste fotos. Podés continuar sin subir ninguna.</p>
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

    <!-- Instrucciones de pago por transferencia (código OY- y CVU/alias) tras registro exitoso -->
    <div
      v-if="modalTransferenciaAbierto && instruccionesTransferencia"
      class="modal-pago-overlay"
      role="dialog"
      aria-modal="true"
      aria-labelledby="modal-pago-titulo"
    >
      <div class="modal-pago-card">
        <h3 id="modal-pago-titulo" class="modal-pago-titulo">Pagá tu suscripción por transferencia</h3>
        <p class="modal-pago-intro">
          Tu cuenta ya está creada. La suscripción queda <strong>pendiente</strong> hasta que acreditemos el pago en nuestras cuentas.
        </p>
        <div class="modal-pago-bloque">
          <span class="modal-pago-label">Código para el concepto</span>
          <div class="modal-pago-fila">
            <code class="modal-pago-codigo">{{ instruccionesTransferencia.codigoReferencia }}</code>
            <button type="button" class="btn-modal-copiar" @click="copiarAlPortapapeles(instruccionesTransferencia.codigoReferencia)">Copiar</button>
          </div>
          <p class="modal-pago-hint">{{ instruccionesTransferencia.mensajeConcepto }}</p>
        </div>
        <div class="modal-pago-bloque">
          <span class="modal-pago-label">Monto exacto</span>
          <p class="modal-pago-monto">${{ instruccionesTransferencia.montoArs.toLocaleString('es-AR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) }}</p>
        </div>
        <div v-if="instruccionesTransferencia.cuentas?.length" class="modal-pago-cuentas">
          <span class="modal-pago-label">Podés transferir a cualquiera de estas cuentas</span>
          <div
            v-for="(c, idx) in instruccionesTransferencia.cuentas"
            :key="idx"
            class="modal-pago-cuenta"
          >
            <strong>{{ c.etiqueta }}</strong>
            <div v-if="c.titular" class="modal-pago-detalle">Titular: {{ c.titular }}</div>
            <div class="modal-pago-fila">
              <code class="modal-pago-cvu">{{ c.cvuAlias }}</code>
              <button type="button" class="btn-modal-copiar" @click="copiarAlPortapapeles(c.cvuAlias)">Copiar</button>
            </div>
          </div>
        </div>
        <p v-else class="modal-pago-aviso-config">
          El administrador aún no cargó los CVU/alias en el servidor. Guardá el código de referencia; cuando tengas los datos de pago, transferí el monto indicado con ese código en el concepto.
        </p>
        <button type="button" class="btn-modal-ok" @click="cerrarModalTransferenciaIrHome">Entendido, ir al inicio</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch, nextTick } from 'vue'
import { useRouter } from 'vue-router'
import { profesionalService } from '../services/profesionalService'
import { useProfessionalRegistrationForm } from '../composables/useForm'
import { PROVINCIAS_ARGENTINA, MESSAGES, ROUTES } from '../utils/constants'
import CiudadAutocomplete from '../components/CiudadAutocomplete.vue'
import MapaZonasCobertura from '../components/MapaZonasCobertura.vue'
import Wizard from '../components/Wizard.vue'
import type { CategoriaServicio, InstruccionesPagoTransferencia } from '../types'
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
// Se activa cuando entrás al paso de "Pago" (paso 3), porque el plan es lo último.
// Antes de eso, dejamos seleccionar varias zonas aunque el plan tenga un valor por defecto.
const restriccionZonasPorPlanActiva = ref(false)
const fileInput = ref<HTMLInputElement | null>(null)
const busquedaServicios = ref('')
const errorGeneralRef = ref<HTMLElement | null>(null)

const metodoPagoSeleccionado = ref<string | null>(null)
const montoPago = ref(5000) // Se calculará dinámicamente
const modalTransferenciaAbierto = ref(false)
const instruccionesTransferencia = ref<InstruccionesPagoTransferencia | null>(null)
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

// Foto de perfil (opcional, paso 1)
const fotoPerfilBase64 = ref<string | null>(null)
const fileInputFotoPerfil = ref<HTMLInputElement | null>(null)

// Fotos de trabajos realizados (opcional)
const imagenesTrabajoBase64 = ref<string[]>([])

// Wizard steps
const wizardSteps = [
  { label: 'Datos Personales' },
  { label: 'Oficio/Profesión' },
  { label: 'Pago' },
  { label: 'Fotos (opcional)' }
]

const MENSAJE_OBLIGATORIO = 'Campo obligatorio'
const MENSAJE_ZONAS_FREE = 'Con el plan Free solo podés tener 1 zona de cobertura. Eliminá las demás o seleccioná otro plan.'

/** Valida el paso actual al intentar avanzar. Marca en rojo los campos vacíos y retorna false si hay errores. */
function validarPasoActual (step: number): boolean {
  if (step === 1) {
    let valido = true
    if (!formulario?.nombre?.trim()) { setError('nombre', MENSAJE_OBLIGATORIO); valido = false } else { setError('nombre', '') }
    if (!formulario?.apellido?.trim()) { setError('apellido', MENSAJE_OBLIGATORIO); valido = false } else { setError('apellido', '') }
    if (!formulario?.email?.trim()) { setError('email', MENSAJE_OBLIGATORIO); valido = false } else { setError('email', '') }
    if (!formulario?.password) { setError('password', MENSAJE_OBLIGATORIO); valido = false } else { setError('password', '') }
    if (!formulario?.confirmarPassword) { setError('confirmarPassword', MENSAJE_OBLIGATORIO); valido = false } else { setError('confirmarPassword', '') }
    if (!formulario?.provincia?.trim()) { setError('provincia', MENSAJE_OBLIGATORIO); valido = false } else { setError('provincia', '') }
    if (!formulario?.ciudad?.trim()) { setError('ciudad', MENSAJE_OBLIGATORIO); valido = false } else { setError('ciudad', '') }
    return valido
  }
  if (step === 2) {
    let valido = true
    if (tipoServicio.value === null) {
      setError('general', 'Seleccioná el tipo de servicio (Oficiante o Profesional).')
      valido = false
    } else {
      setError('general', '')
    }
    if (!formulario?.oficios?.length) {
      setError('oficios', 'Debes seleccionar al menos un oficio o categoría.')
      valido = false
    } else {
      setError('oficios', '')
    }
    return valido
  }
  if (step === 3) {
    let valido = true
    if (!planSeleccionado.value) {
      setError('plan', 'Seleccioná un plan de suscripción.')
      valido = false
    } else {
      setError('plan', '')
    }

    // Restricción de zonas: si el plan es Free, solo se permite 1 zona.
    // El plan se elige al final del wizard, por eso recién se valida acá.
    const planNormalizado = normalizeText((planSeleccionado.value?.nombre ?? '').toString())
    const zonasCount = Array.isArray(formulario?.zonasCobertura) ? formulario.zonasCobertura.length : 0
    if (planNormalizado === 'free' && zonasCount > 1) {
      // La UI ya muestra el mensaje en el paso 3; acá solo bloqueamos avanzar.
      setError('general', '')
      valido = false
    } else {
      // Limpiar error si ya no aplica
      setError('general', '')
    }

    if (planNormalizado !== 'free' && !metodoPagoSeleccionado.value) {
      setError('metodoPago', 'Seleccioná un método de pago.')
      valido = false
    } else {
      setError('metodoPago', '')
    }
    if (!formulario?.aceptaTerminos) {
      setError('aceptaTerminos', 'Debes aceptar los términos y condiciones y la política de privacidad.')
      valido = false
    } else {
      setError('aceptaTerminos', '')
    }
    return valido
  }
  return true // Paso 4: sin campos obligatorios
}

// Función para normalizar texto (remover acentos para búsqueda más flexible)
const normalizeText = (text: string): string => {
  return text
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '') // Remover diacríticos
    .trim()
}

const maxZonasCoberturaActual = computed(() => {
  // Antes de entrar al paso 3 (Pago) no imponemos límites por plan.
  if (!restriccionZonasPorPlanActiva.value) return 99999
  if (!planSeleccionado.value) return 99999
  const planNormalizado = normalizeText(planSeleccionado.value?.nombre ?? '')
  return planNormalizado === 'free' ? 1 : 99999
})

const mensajeFreeZonas = computed(() => {
  if (!planSeleccionado.value) return ''
  const planNormalizado = normalizeText(planSeleccionado.value?.nombre ?? '')
  const zonasCount = Array.isArray(formulario?.zonasCobertura) ? formulario.zonasCobertura.length : 0
  if (planNormalizado === 'free' && zonasCount > 1) return MENSAJE_ZONAS_FREE
  return ''
})

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
  // Seleccionar plan básico por defecto
  if (planesDisponibles.value.length > 0) {
    const planBasico = planesDisponibles.value.find((p: any) => {
      const n = normalizeText(p?.nombre ?? '')
      return n === 'basica' || n === 'basico'
    })
    seleccionarPlan(planBasico || planesDisponibles.value[0])
  }
})

// Cargar planes de suscripción
const cargarPlanes = async () => {
  cargandoPlanes.value = true
  try {
    const planes = await profesionalService.obtenerPlanesSuscripcion()
    const order: Record<string, number> = { free: 0, basica: 1, basico: 1, pro: 2, premium: 3 }
    planesDisponibles.value = (planes || []).sort((a: any, b: any) =>
      (order[normalizeText(a?.nombre ?? '')] ?? 99) - (order[normalizeText(b?.nombre ?? '')] ?? 99)
    )
    
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
  const n = normalizeText(plan?.nombre ?? '')
  metodoPagoSeleccionado.value = n === 'free' ? null : 'transferencia'
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
  setError('email', '') // limpiar error previo
  try {
    const { useAuthStore } = await import('../stores/auth')
    const authStore = useAuthStore()
    const disponible = await authStore.verificarEmail(formulario.email)
    if (disponible === false) {
      setError('email', 'Este email ya está registrado')
    }
  } catch {
    // Error de red o del servidor: no mostramos "ya registrado" para no confundir
    setError('email', 'No se pudo verificar el email. Revisá la conexión o intentá más tarde.')
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
  // Al volver a pasos 1–2 no forzar límite de zonas por plan hasta que vuelva a “Pago”.
  restriccionZonasPorPlanActiva.value = step >= 3
}

// Watcher para actualizar monto cuando cambia la duración
watch([planSeleccionado, mesesSuscripcion], () => {
  actualizarMontoPago()
})

async function copiarAlPortapapeles (texto: string) {
  try {
    await navigator.clipboard.writeText(texto)
    alert('Copiado al portapapeles.')
  } catch {
    alert('No se pudo copiar. Seleccioná el texto manualmente.')
  }
}

function cerrarModalTransferenciaIrHome () {
  modalTransferenciaAbierto.value = false
  instruccionesTransferencia.value = null
  router.push(ROUTES.HOME)
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

      const nombrePlanNormalizado = normalizeText(planSeleccionado.value?.nombre ?? '')
      const requierePago = nombrePlanNormalizado !== 'free'

      // Validar método de pago
      if (requierePago && !metodoPagoSeleccionado.value) {
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
        planSeleccionado: planSeleccionado.value?.nombre,
        metodoPago: requierePago ? (metodoPagoSeleccionado.value ?? 'transferencia') : undefined,
        mesesSuscripcion: requierePago ? mesesSuscripcion.value : 1,
        fotoPerfilBase64: fotoPerfilBase64.value || undefined,
        imagenesTrabajoBase64: imagenesTrabajoBase64.value?.length ? imagenesTrabajoBase64.value : undefined
      }

      const result = await profesionalService.registrar(datosProfesional)
      const inst = result.instruccionesTransferencia

      if (inst?.codigoReferencia) {
        instruccionesTransferencia.value = inst
        modalTransferenciaAbierto.value = true
      } else {
        alert(MESSAGES.SUCCESS.PROFESSIONAL_REGISTERED)
        router.push(ROUTES.HOME)
      }
    } catch (error: any) {
      const errorMessage = error.response?.data?.message ||
                          error.response?.data?.error ||
                          error.message ||
                          MESSAGES.ERROR.GENERAL
      setError('general', errorMessage)
      nextTick(() => {
        errorGeneralRef.value?.scrollIntoView({ behavior: 'smooth', block: 'center' })
      })
      console.error('Error al registrar profesional:', errorMessage, error.response?.data)
    }
  }
}

const MAX_FOTOS_TRABAJO = 10
const MAX_SIZE_MB = 3

function readFileAsDataUrl (file: File): Promise<string> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader()
    reader.onload = () => resolve((reader.result as string) || '')
    reader.onerror = () => reject(reader.error)
    reader.readAsDataURL(file)
  })
}

async function onFotoPerfilChange (e: Event) {
  const input = e.target as HTMLInputElement
  const file = input.files?.[0]
  if (!file || !file.type.startsWith('image/')) return
  if (file.size > MAX_SIZE_MB * 1024 * 1024) {
    alert(`La imagen no debe superar ${MAX_SIZE_MB} MB.`)
    input.value = ''
    return
  }
  const dataUrl = await readFileAsDataUrl(file)
  if (!dataUrl) return
  try {
    await profesionalService.validarImagenes([dataUrl])
    fotoPerfilBase64.value = dataUrl
    setError('general', '')
  } catch (err: any) {
    const msg = err?.response?.data?.message ?? err?.message ?? 'La imagen no cumple las normas de la comunidad.'
    setError('general', msg)
  }
  input.value = ''
}

function quitarFotoPerfil () {
  fotoPerfilBase64.value = null
  if (fileInputFotoPerfil.value) fileInputFotoPerfil.value.value = ''
  setError('general', '')
}

async function onFotosTrabajoChange (e: Event) {
  const input = e.target as HTMLInputElement
  const files = input.files
  if (!files?.length) return
  const remaining = MAX_FOTOS_TRABAJO - imagenesTrabajoBase64.value.length
  if (remaining <= 0) {
    alert(`Podés subir hasta ${MAX_FOTOS_TRABAJO} fotos.`)
    input.value = ''
    return
  }
  const toAdd = Math.min(remaining, files.length)
  for (let i = 0; i < toAdd; i++) {
    const file = files[i]
    if (!file.type.startsWith('image/')) continue
    if (file.size > MAX_SIZE_MB * 1024 * 1024) {
      alert(`La imagen ${file.name} supera ${MAX_SIZE_MB} MB.`)
      continue
    }
    try {
      const dataUrl = await readFileAsDataUrl(file)
      if (!dataUrl) continue
      await profesionalService.validarImagenes([dataUrl])
      imagenesTrabajoBase64.value.push(dataUrl)
      setError('general', '')
    } catch (err: any) {
      const msg = err?.response?.data?.message ?? err?.message ?? `La imagen "${file.name}" no cumple las normas de la comunidad.`
      setError('general', msg)
    }
  }
  input.value = ''
}

function quitarFotoTrabajo (index: number) {
  imagenesTrabajoBase64.value.splice(index, 1)
  setError('general', '')
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

const isImageType = (file: File) => file.type === 'image/jpeg' || file.type === 'image/jpg' || file.type === 'image/png'

async function addDocumentoIfValid (file: File): Promise<boolean> {
  if (!validateFile(file)) return false
  if (isImageType(file)) {
    try {
      const dataUrl = await readFileAsDataUrl(file)
      await profesionalService.validarImagenes([dataUrl])
    } catch (err: any) {
      const msg = err?.response?.data?.message ?? err?.message ?? `La imagen "${file.name}" no cumple las normas de la comunidad.`
      setError('documentos', msg)
      return false
    }
  }
  setError('documentos', '')
  documentos.value.push({ nombre: file.name, archivo: file, tamano: formatFileSize(file.size) })
  return true
}

async function handleFileChange (event: Event) {
  const target = event.target as HTMLInputElement
  if (target.files && target.files.length > 0) {
    for (const file of Array.from(target.files)) {
      await addDocumentoIfValid(file)
    }
  }
  isDragging.value = false
  target.value = ''
}

async function handleFileDrop (event: DragEvent) {
  const files = event.dataTransfer?.files
  if (files && files.length > 0) {
    for (const file of Array.from(files)) {
      await addDocumentoIfValid(file)
    }
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

.label-oficios {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  align-items: baseline;
  margin-bottom: 12px;
}

.oficios-seleccionados-inline {
  font-size: 0.85rem;
  color: var(--brand, #22c55e);
  font-weight: 500;
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

.payment-methods.error-border {
  padding: 4px;
  border-radius: 12px;
  border: 2px solid #ef4444;
  background: #fef2f2;
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

.error-general-box {
  text-align: center;
  margin-bottom: 16px;
  padding: 12px 16px;
  background: #fef2f2;
  border: 1px solid #fecaca;
  border-radius: 8px;
}

.error-general-box .error-hint {
  margin: 8px 0 0;
  font-size: 0.8rem;
  color: #b91c1c;
}

.error-border-wrap {
  padding: 8px;
  border-radius: 8px;
  border: 2px solid #ef4444;
  background: #fef2f2;
}

.aviso-zonas-free {
  font-size: 0.82rem;
  color: #b91c1c;
  background: #fef2f2;
  border: 1px solid #fecaca;
  border-radius: 8px;
  padding: 10px 12px;
  margin: -8px 0 16px 0;
  text-align: center;
  line-height: 1.35;
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

/* Paso 4: Fotos de trabajos */
.input-file {
  width: 100%;
  padding: 12px;
  border: 2px dashed #cbd5e1;
  border-radius: 8px;
  background: #f8fafc;
  cursor: pointer;
  font-size: 14px;
}
.input-file:hover {
  border-color: var(--brand);
  background: #f0f9ff;
}
.fotos-preview {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(100px, 1fr));
  gap: 12px;
  margin-top: 16px;
}
.foto-preview-item {
  position: relative;
  aspect-ratio: 1;
  border-radius: 8px;
  overflow: hidden;
  border: 1px solid #e2e8f0;
}
.foto-preview-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}
.foto-remove {
  position: absolute;
  top: 4px;
  right: 4px;
  width: 24px;
  height: 24px;
  border-radius: 50%;
  border: none;
  background: rgba(0,0,0,0.6);
  color: white;
  cursor: pointer;
  font-size: 18px;
  line-height: 1;
  padding: 0;
}
.foto-remove:hover {
  background: #ef4444;
}

.foto-perfil-preview {
  margin-top: 12px;
  display: flex;
  align-items: center;
  gap: 16px;
  flex-wrap: wrap;
}
.foto-perfil-preview img {
  width: 80px;
  height: 80px;
  object-fit: cover;
  border-radius: 50%;
  border: 2px solid #e2e8f0;
}
.btn-quitar-foto {
  padding: 6px 12px;
  font-size: 13px;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  background: #f8fafc;
  color: #64748b;
  cursor: pointer;
}
.btn-quitar-foto:hover {
  background: #fee2e2;
  color: #dc2626;
  border-color: #fecaca;
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

.modal-pago-overlay {
  position: fixed;
  inset: 0;
  z-index: 2000;
  background: rgba(15, 23, 42, 0.55);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
  overflow-y: auto;
}

.modal-pago-card {
  background: #fff;
  border-radius: 16px;
  max-width: 480px;
  width: 100%;
  padding: 24px;
  box-shadow: 0 20px 50px rgba(0, 0, 0, 0.2);
}

.modal-pago-titulo {
  margin: 0 0 12px;
  font-size: 1.25rem;
  color: var(--text, #0f172a);
}

.modal-pago-intro {
  margin: 0 0 20px;
  font-size: 0.95rem;
  color: #475569;
  line-height: 1.45;
}

.modal-pago-bloque {
  margin-bottom: 18px;
}

.modal-pago-label {
  display: block;
  font-size: 0.8rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.04em;
  color: #64748b;
  margin-bottom: 8px;
}

.modal-pago-fila {
  display: flex;
  align-items: center;
  gap: 10px;
  flex-wrap: wrap;
}

.modal-pago-codigo,
.modal-pago-cvu {
  flex: 1;
  min-width: 0;
  font-size: 0.95rem;
  padding: 10px 12px;
  background: #f1f5f9;
  border-radius: 8px;
  word-break: break-all;
}

.modal-pago-monto {
  margin: 0;
  font-size: 1.35rem;
  font-weight: 700;
  color: var(--brand, #059669);
}

.modal-pago-hint {
  margin: 10px 0 0;
  font-size: 0.85rem;
  color: #64748b;
  line-height: 1.4;
}

.modal-pago-cuentas {
  margin-bottom: 20px;
}

.modal-pago-cuenta {
  margin-top: 14px;
  padding: 12px;
  border: 1px solid #e2e8f0;
  border-radius: 10px;
  background: #fafafa;
}

.modal-pago-detalle {
  font-size: 0.88rem;
  color: #475569;
  margin: 6px 0 8px;
}

.modal-pago-aviso-config {
  font-size: 0.88rem;
  color: #b45309;
  background: #fffbeb;
  border: 1px solid #fde68a;
  border-radius: 8px;
  padding: 12px;
  margin-bottom: 16px;
}

.btn-modal-copiar {
  flex-shrink: 0;
  padding: 8px 14px;
  border-radius: 8px;
  border: 1px solid #cbd5e1;
  background: #fff;
  font-size: 0.85rem;
  cursor: pointer;
}

.btn-modal-copiar:hover {
  border-color: var(--brand, #059669);
  color: var(--brand, #059669);
}

.btn-modal-ok {
  width: 100%;
  margin-top: 8px;
  padding: 14px;
  border: none;
  border-radius: 10px;
  background: var(--brand, #059669);
  color: #fff;
  font-weight: 600;
  font-size: 1rem;
  cursor: pointer;
}

.btn-modal-ok:hover {
  filter: brightness(1.05);
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
