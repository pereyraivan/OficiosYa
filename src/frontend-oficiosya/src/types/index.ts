// Tipos para Usuario
export interface Usuario {
  id: number
  nombre: string
  email: string
  telefono?: string
  fechaRegistro: string
  esProfesional: boolean
  activo: boolean
}

export interface RegistrarUsuarioRequest {
  nombre: string
  email: string
  contraseña: string
  telefono?: string
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
  fotoPerfil?: string
}

export interface LoginRequest {
  email: string
  contraseña: string
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
  oficio: string
  zona: string
  radio?: number
}

// Tipos para Reseñas
export interface Resena {
  id: number
  profesionalId: number
  clienteId: number
  puntuacion: number
  comentario?: string
  fechaCreacion: string
  cliente: Usuario
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
