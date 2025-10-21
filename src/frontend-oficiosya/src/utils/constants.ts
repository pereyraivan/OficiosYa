// Constantes reutilizables de la aplicación
export const CATEGORIAS_SERVICIO = {
  OFICIANTE: [
    { nombre: 'Plomero', icono: '🔧', descripcion: 'Instalación y reparación de sistemas de plomería' },
    { nombre: 'Electricista', icono: '⚡', descripcion: 'Instalaciones y reparaciones eléctricas' },
    { nombre: 'Albañil', icono: '🧱', descripcion: 'Construcción y reparación de estructuras' },
    { nombre: 'Gasista', icono: '🔥', descripcion: 'Instalación y reparación de sistemas de gas' },
    { nombre: 'Jardinero', icono: '🌿', descripcion: 'Mantenimiento de jardines y espacios verdes' },
    { nombre: 'Pintor', icono: '🎨', descripcion: 'Pintura de interiores y exteriores' },
    { nombre: 'Cerrajero', icono: '🔒', descripcion: 'Apertura de cerraduras y copias de llaves' },
    { nombre: 'Carpintero', icono: '🪚', descripcion: 'Trabajos en madera y muebles' },
    { nombre: 'Herrero', icono: '⚒️', descripcion: 'Trabajos en metal y herrería' },
    { nombre: 'Técnico en Aire Acondicionado', icono: '❄️', descripcion: 'Instalación y reparación de sistemas de climatización' },
    { nombre: 'Técnico en Refrigeración', icono: '🧊', descripcion: 'Reparación de heladeras y freezers' },
    { nombre: 'Limpieza', icono: '🧹', descripcion: 'Servicios de limpieza doméstica y comercial' }
  ],
  PROFESIONAL: [
    { nombre: 'Abogado', icono: '⚖️', descripcion: 'Servicios legales y asesoramiento jurídico' },
    { nombre: 'Contador', icono: '📊', descripcion: 'Servicios contables y fiscales' },
    { nombre: 'Médico', icono: '🩺', descripcion: 'Servicios de salud y atención médica' },
    { nombre: 'Psicólogo', icono: '🧠', descripcion: 'Servicios de salud mental y terapia' },
    { nombre: 'Dentista', icono: '🦷', descripcion: 'Servicios de salud dental' },
    { nombre: 'Veterinario', icono: '🐕', descripcion: 'Servicios de salud animal' },
    { nombre: 'Arquitecto', icono: '🏗️', descripcion: 'Diseño arquitectónico y planificación' },
    { nombre: 'Ingeniero', icono: '⚙️', descripcion: 'Servicios de ingeniería y consultoría técnica' },
    { nombre: 'Diseñador Gráfico', icono: '🎨', descripcion: 'Servicios de diseño visual y comunicación' },
    { nombre: 'Traductor', icono: '🌐', descripcion: 'Servicios de traducción e interpretación' },
    { nombre: 'Coach', icono: '🎯', descripcion: 'Servicios de coaching y desarrollo personal' },
    { nombre: 'Nutricionista', icono: '🥗', descripcion: 'Servicios de nutrición y alimentación saludable' }
  ]
} as const

// Mantener compatibilidad con código existente
export const OFICIOS_DISPONIBLES = [
  ...CATEGORIAS_SERVICIO.OFICIANTE.map(c => c.nombre),
  ...CATEGORIAS_SERVICIO.PROFESIONAL.map(c => c.nombre)
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
  OFICIANTES: '/oficiantes',
  PROFESIONALES: '/profesionales',
  LOGIN: '/login',
  REGISTER: '/registrar',
  REGISTER_PROFESSIONAL: '/registrar-profesional',
  PROFILE: '/perfil',
  DASHBOARD: '/dashboard',
  TERMINOS: '/terminos-y-condiciones',
  POLITICA: '/politica-de-privacidad'
} as const
