// Tipos para Usuario
export interface Usuario {
  id: number
  nombre: string
  email: string
  telefono?: string
  fechaRegistro: string
  esProfesional: boolean
  esAdmin?: boolean
  activo: boolean
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

export interface RegistrarProfesionalRequest {
  nombre: string
  apellido: string
  email: string
  password: string
  telefono?: string
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
  oficios: Oficio[]
  certificados: Certificado[]
  resenas: Resena[]
}

export interface BuscarProfesionalesRequest {
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
  comentario: string
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

// Tipos para Oficios
export interface Oficio {
  id: number
  nombre: string
  descripcion?: string
  icono?: string
  color?: string
  activo: boolean
  fechaCreacion: string
}

export interface CrearOficioRequest {
  nombre: string
  descripcion?: string
  icono?: string
  color?: string
}

export interface ActualizarOficioRequest {
  id: number
  nombre: string
  descripcion?: string
  icono?: string
  color?: string
  activo: boolean
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
