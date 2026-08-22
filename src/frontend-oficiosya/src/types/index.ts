// Tipos para Usuario
export interface Usuario {
  id: number
  nombre: string
  apellido?: string
  email: string
  telefono?: string
  fechaRegistro?: string
  esProfesional?: boolean
  esAdmin?: boolean
  activo?: boolean
  ciudad?: string
  provincia?: string
  /** ID del perfil profesional cuando el usuario es profesional (viene en /usuarios/perfil). */
  profesionalId?: number
}

export interface RegistrarUsuarioRequest {
  nombre: string
  apellido: string
  email: string
  password: string
  telefono?: string
  direccion?: string
  ciudad?: string
  provincia?: string
  codigoPostal?: string
}

export interface ZonaCoberturaDto {
  localidad: string
  provincia: string
  departamento?: string
  latitud: number
  longitud: number
  codigoPostal?: string
}

export interface InstruccionesTransferenciaCuenta {
  etiqueta: string
  cvuAlias: string
  titular: string
}

export interface InstruccionesPagoTransferencia {
  codigoReferencia: string
  /** Ej. "DNI" o "CUIT/CUIL" */
  etiquetaReferencia?: string
  montoArs: number
  moneda: string
  planNombre: string
  meses: number
  mensajeConcepto: string
  cuentas: InstruccionesTransferenciaCuenta[]
}

export interface RegistrarProfesionalApiResponse {
  profesional: Profesional
  instruccionesTransferencia?: InstruccionesPagoTransferencia | null
}

export interface RegistrarProfesionalRequest {
  nombre: string
  apellido: string
  email: string
  password: string
  telefono?: string
  /** DNI o CUIT/CUIL */
  documento: string
  direccion?: string
  ciudad?: string
  provincia?: string
  codigoPostal?: string
  descripcion?: string
  oficios: string[]
  zonasCobertura?: ZonaCoberturaDto[]
  fotoPerfil?: string
  metodoPago?: string
  codigoPago?: string
  planSeleccionado?: string
  mesesSuscripcion?: number
  /** Opcional. Imágenes de trabajos en base64 (data:image/...;base64,...) */
  imagenesTrabajoBase64?: string[]
}

export interface LoginRequest {
  email: string
  password: string
}

// Tipos para Profesional
export interface Profesional {
  id: number
  usuarioId: number
  descripcion?: string
  fotoPerfil?: string
  calificacionPromedio: number
  totalResenas: number
  disponible: boolean
  fechaRegistro: string
  activo: boolean
  usuario: Usuario
  /** Nombres de categorías (ej. Plomero, Electricista). */
  oficios: string[]
  certificados: Certificado[]
  resenas: Resena[]
}

export interface BuscarProfesionalesRequest {
  termino?: string
  oficio?: string
  zona?: string
  radio?: number
}

// Tipos para Reseñas
export interface Resena {
  id: number
  profesionalId: number
  usuarioId?: number | null
  calificacion: number
  comentario?: string
  fechaCreacion: string
  /** Nombre mostrado (usuario logueado o nombre ingresado por visitante). */
  nombreAutor?: string
  usuario?: { id: number; nombre: string } | null
}

export interface CrearResenaRequest {
  /** Cómo querés que aparezca tu nombre (obligatorio si no estás logueado). */
  autorNombre?: string
  calificacion: number
  comentario?: string
}

// Tipos para Certificados
export interface Certificado {
  id: number
  profesionalId: number
  nombre: string
  institucion: string
  fechaEmision: string
  fechaVencimiento?: string
  archivoUrl?: string
}

// Tipos para Categorías de Servicio
export interface CategoriaServicio {
  id: number
  nombre: string
  descripcion?: string
  tipo: string // 'Oficiante' | 'Profesional'
  grupo?: string // Ej: 'Construcción', 'Salud', 'Hogar y Mantenimiento', etc.
  icono?: string
  activo?: boolean
  fechaCreacion?: string
}

export interface CrearCategoriaServicioRequest {
  nombre: string
  descripcion?: string
  tipo: 'Oficiante' | 'Profesional'
  icono?: string
}

export interface ActualizarCategoriaServicioRequest {
  id: number
  nombre: string
  descripcion?: string
  tipo: 'Oficiante' | 'Profesional'
  icono?: string
  activo: boolean
}

// Suscripción activa del profesional (panel)
export interface SuscripcionActiva {
  tipo: string
  fechaVencimiento: string
  estado: string
}

export interface SuscripcionPendiente {
  id: number
  tipo: string
  precio: number
  codigoPago?: string | null
  estado: string
  fechaRegistro: string
}

export interface SolicitarSuscripcionRequest {
  planSeleccionado: string
  mesesSuscripcion: number
  metodoPago?: string
}

export interface SolicitarSuscripcionApiResponse {
  suscripcionId: number
  yaExistente: boolean
  instruccionesTransferencia?: InstruccionesPagoTransferencia | null
}

export interface SuscripcionPendienteAdmin {
  id: number
  profesionalId: number
  profesionalNombre: string
  profesionalEmail: string
  profesionalTelefono?: string | null
  profesionalDocumento?: string | null
  planSuscripcionId?: number | null
  planNombre: string
  precio: number
  codigoPago?: string | null
  metodoPago?: string | null
  estado: string
  fechaRegistro: string
  fechaInicio: string
  fechaVencimiento: string
  mesesDuracion: number
}

export interface ConfirmarSuscripcionAdminResponse {
  id: number
  estado: string
  pagoConfirmado: boolean
  fechaPago?: string | null
  fechaInicio: string
  fechaVencimiento: string
  mensaje: string
}

/** Listado admin de profesionales / oficiantes */
export interface ProfesionalAdmin {
  id: number
  usuarioId: number
  nombre: string
  apellido: string
  email: string
  telefono?: string | null
  documento?: string | null
  ciudad?: string | null
  provincia?: string | null
  descripcion?: string | null
  tipos: string[]
  categorias: string[]
  planActual: string
  suscripcionEstado?: string | null
  disponible: boolean
  activo: boolean
  calificacionPromedio: number
  totalResenas: number
  fechaRegistro: string
}

export interface ActualizarProfesionalAdminRequest {
  nombre: string
  apellido: string
  email: string
  telefono?: string | null
  documento?: string | null
  ciudad?: string | null
  provincia?: string | null
  descripcion?: string | null
  disponible: boolean
  activo: boolean
  categorias?: string[]
}

// Tipos para API Response
export interface ApiResponse<T> {
  data: T
  message?: string
  success: boolean
}

export interface ErrorResponse {
  message: string
  errors?: Record<string, string[]>
}
