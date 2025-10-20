// Constantes reutilizables de la aplicación
export const OFICIOS_DISPONIBLES = [
  'Electricista',
  'Plomero',
  'Albañil',
  'Gasista',
  'Jardinero',
  'Pintor',
  'Cerrajero',
  'Limpieza',
  'Carpintero',
  'Herrero',
  'Técnico en Aire Acondicionado',
  'Técnico en Refrigeración'
] as const

export const PROVINCIAS_ARGENTINA = [
  'Buenos Aires',
  'CABA',
  'Catamarca',
  'Chaco',
  'Chubut',
  'Córdoba',
  'Corrientes',
  'Entre Ríos',
  'Formosa',
  'Jujuy',
  'La Pampa',
  'La Rioja',
  'Mendoza',
  'Misiones',
  'Neuquén',
  'Río Negro',
  'Salta',
  'San Juan',
  'San Luis',
  'Santa Cruz',
  'Santa Fe',
  'Santiago del Estero',
  'Tierra del Fuego',
  'Tucumán'
] as const

export const CIUDADES_TUCUMAN = [
  'San Miguel de Tucumán',
  'Yerba Buena',
  'Tafí Viejo',
  'Concepción',
  'Aguilares',
  'Monteros',
  'Famaillá',
  'Simoca',
  'La Cocha',
  'Graneros',
  'Burruyacú',
  'Trancas',
  'Chicligasta',
  'Leales',
  'Cruz Alta',
  'Capital',
  'Tafí del Valle',
  'Juan Bautista Alberdi',
  'La Banda del Río Salí',
  'Banda del Río Salí'
] as const

// Configuración de la aplicación
export const APP_CONFIG = {
  API_BASE_URL: import.meta.env.VITE_API_URL || 'http://localhost:5000/api',
  APP_NAME: 'OficiosYa',
  APP_VERSION: '1.0.0',
  MIN_PASSWORD_LENGTH: 6,
  MAX_FILE_SIZE: 5 * 1024 * 1024, // 5MB
  SUPPORTED_IMAGE_TYPES: ['image/jpeg', 'image/png', 'image/webp']
} as const

// Mensajes de la aplicación
export const MESSAGES = {
  SUCCESS: {
    USER_REGISTERED: '¡Cuenta creada exitosamente!',
    PROFESSIONAL_REGISTERED: '¡Cuenta de profesional creada exitosamente!',
    PROFILE_UPDATED: 'Perfil actualizado correctamente',
    PASSWORD_CHANGED: 'Contraseña cambiada correctamente'
  },
  ERROR: {
    GENERAL: 'Ha ocurrido un error. Intenta nuevamente.',
    NETWORK: 'Error de conexión. Verifica tu internet.',
    VALIDATION: 'Por favor, corrige los errores en el formulario.',
    EMAIL_EXISTS: 'Este email ya está registrado',
    INVALID_CREDENTIALS: 'Email o contraseña incorrectos',
    FILE_TOO_LARGE: 'El archivo es demasiado grande',
    INVALID_FILE_TYPE: 'Tipo de archivo no válido'
  },
  LOADING: {
    CREATING_ACCOUNT: 'Creando cuenta...',
    LOGGING_IN: 'Iniciando sesión...',
    LOADING: 'Cargando...',
    SAVING: 'Guardando...'
  }
} as const

// Rutas de la aplicación
export const ROUTES = {
  HOME: '/',
  LOGIN: '/login',
  REGISTER: '/registrar',
  REGISTER_PROFESSIONAL: '/registrar-profesional',
  PROFILE: '/perfil',
  DASHBOARD: '/dashboard'
} as const
