import api from './api'
import type { Profesional, BuscarProfesionalesRequest, RegistrarProfesionalRequest, RegistrarProfesionalApiResponse, ApiResponse, CategoriaServicio, ZonaCoberturaDto, SuscripcionActiva, SuscripcionPendiente, SolicitarSuscripcionRequest, SolicitarSuscripcionApiResponse, Resena, CrearResenaRequest } from '../types'

export const profesionalService = {
  // Validar imágenes (moderación) antes de subir. Lanza si no pasan.
  async validarImagenes(imagenesBase64: string[]): Promise<{ aprobado: boolean }> {
    const response = await api.post<{ aprobado: boolean }>('/profesionales/validar-imagenes', { imagenesBase64 })
    return response.data
  },

  // Registrar profesional (201: profesional + opcional instruccionesTransferencia si plan de pago por transferencia)
  async registrar(datos: RegistrarProfesionalRequest): Promise<RegistrarProfesionalApiResponse> {
    const response = await api.post<RegistrarProfesionalApiResponse>('/profesionales/registrar', datos)
    return response.data
  },

  // Obtener categorías disponibles
  async obtenerCategorias(tipo?: 'Oficiante' | 'Profesional'): Promise<CategoriaServicio[]> {
    try {
      const params = tipo ? { tipo } : {}
      const response = await api.get<CategoriaServicio[]>('/profesionales/categorias', { params })
      
      // El backend devuelve directamente un array
      if (Array.isArray(response.data)) {
        return response.data
      }
      
      // Si viene envuelto en un objeto
      if (response.data && typeof response.data === 'object' && 'data' in response.data) {
        return (response.data as any).data || []
      }
      
      return []
    } catch (error: any) {
      console.error('Error al obtener categorías:', error)
      
      // Capturar la respuesta del servidor si existe
      const responseData = error.response?.data
      const status = error.response?.status
      
      console.error('Detalles del error:', {
        message: error.message,
        responseData: responseData,
        status: status,
        tipo: tipo,
        fullError: error
      })
      
      // Crear un error más descriptivo
      let errorMessage = 'Error al cargar las categorías disponibles'
      
      // Intentar obtener el mensaje del backend
      if (responseData) {
        // Si el backend devuelve un mensaje específico
        if (responseData.message) {
          errorMessage = responseData.message
        } else if (responseData.error) {
          errorMessage = responseData.error
        } else if (typeof responseData === 'string') {
          errorMessage = responseData
        }
        
        // Si el error menciona TipoServicios o hay una solución sugerida
        if (responseData.message?.includes('TipoServicios') || 
            responseData.solution) {
          console.warn('Error de base de datos detectado. La tabla TipoServicios puede no existir o estar en otro esquema.')
          errorMessage = 'Error de configuración de base de datos. Verifica que la tabla TipoServicios existe en el esquema dbo.'
        }
      } else if (error.message) {
        errorMessage = error.message
      }
      
      // Si es un error 500 sin respuesta, puede ser un problema de conexión o de base de datos
      if (status === 500 && !responseData) {
        errorMessage = 'Error del servidor. Verifica que la base de datos esté configurada correctamente.'
      }
      
      // Crear un nuevo error con el mensaje mejorado
      const enhancedError = new Error(errorMessage)
      ;(enhancedError as any).response = error.response
      ;(enhancedError as any).responseData = responseData
      ;(enhancedError as any).status = status
      ;(enhancedError as any).originalError = error
      throw enhancedError
    }
  },

  // Buscar profesionales
  async buscar(criterios: BuscarProfesionalesRequest): Promise<Profesional[]> {
    const response = await api.get<ApiResponse<Profesional[]>>('/profesionales/buscar', {
      params: criterios
    })
    return response.data.data
  },

  // Obtener profesionales destacados
  async obtenerDestacados(tipo?: 'Oficiante' | 'Profesional'): Promise<Profesional[]> {
    try {
      const params = tipo ? { tipo } : {}
      const response = await api.get<ApiResponse<Profesional[]>>('/profesionales/destacados', { params })
      // El backend devuelve { success, data }
      if (response.data && typeof response.data === 'object' && 'data' in response.data) {
        return response.data.data || []
      }
      // Si viene directamente como array
      return Array.isArray(response.data) ? response.data : []
    } catch (error: any) {
      return []
    }
  },

  /** Perfiles agregados recientemente (por fecha de registro). */
  async obtenerRecientes(limite: number = 12): Promise<Profesional[]> {
    try {
      const response = await api.get<ApiResponse<Profesional[]>>('/profesionales/recientes', { params: { limite } })
      if (response.data && typeof response.data === 'object' && 'data' in response.data) {
        return response.data.data || []
      }
      return Array.isArray(response.data) ? response.data : []
    } catch (error: any) {
      return []
    }
  },

  /** Registra un contacto (clic en Contactar) para conteo y planes. Opcional: tipoContacto 'WhatsApp' | 'Email'. */
  async registrarContacto(profesionalId: number, tipoContacto?: 'WhatsApp' | 'Email'): Promise<void> {
    await api.post(`/profesionales/${profesionalId}/registrar-contacto`, {
      tipoContacto: tipoContacto ?? 'WhatsApp'
    })
  },

  /** Cantidad de contactos recibidos por el profesional en un período (para panel o planes). */
  async obtenerCantidadContactos(profesionalId: number, desde?: string, hasta?: string): Promise<{ total: number }> {
    const params: Record<string, string> = {}
    if (desde) params.desde = desde
    if (hasta) params.hasta = hasta
    const response = await api.get<{ success: boolean; total: number }>(
      `/profesionales/${profesionalId}/contactos/cantidad`,
      { params }
    )
    return { total: response.data?.total ?? 0 }
  },

  // Obtener profesional por ID (acepta respuesta con { data } o el objeto directo)
  async obtenerPorId(id: number): Promise<Profesional> {
    const response = await api.get(`/profesionales/${id}`)
    const body = response.data
    const data = body?.data ?? body
    if (!data) {
      throw new Error('No se recibieron datos del profesional')
    }
    return data as Profesional
  },

  // Obtener profesionales por oficio
  async obtenerPorOficio(oficio: string): Promise<Profesional[]> {
    const response = await api.get<ApiResponse<Profesional[]>>(`/profesionales/oficio/${oficio}`)
    return response.data.data
  },

  // Obtener profesionales cerca de una ubicación
  async obtenerCercaDe(ciudad: string, radio: number = 10): Promise<Profesional[]> {
    const response = await api.get<ApiResponse<Profesional[]>>('/profesionales/cerca', {
      params: { ciudad, radio }
    })
    return response.data.data
  },

  // Obtener planes de suscripción
  async obtenerPlanesSuscripcion(): Promise<any[]> {
    try {
      const response = await api.get<any[]>('/profesionales/planes-suscripcion')
      return Array.isArray(response.data) ? response.data : []
    } catch (error: any) {
      return []
    }
  },

  /** Promo de lanzamiento (Premium gratis N días). */
  async obtenerLanzamientoPromo(): Promise<{
    activa: boolean
    planNombre?: string | null
    diasGratis?: number
    fechaFinUtc?: string | null
    mensaje?: string | null
  }> {
    try {
      const response = await api.get('/config/lanzamiento-promo')
      return response.data ?? { activa: false }
    } catch {
      return { activa: false }
    }
  },

  /** Indica si el profesional autenticado puede postularse a empleos (Pro/Premium). */
  async yoPuedoPostularEmpleos(): Promise<{ puedePostularEmpleos: boolean; planActual: string }> {
    const response = await api.get<{ puedePostularEmpleos: boolean; planActual: string }>('/profesionales/yo-puedo-postular-empleos')
    return response.data ?? { puedePostularEmpleos: false, planActual: 'Free' }
  },

  /** Perfil del profesional autenticado (para el panel). Requiere token. */
  async obtenerMiPerfil(): Promise<Profesional & {
    suscripcionActiva?: SuscripcionActiva | null
    suscripcionPendiente?: SuscripcionPendiente | null
    planActual?: string
  }> {
    const response = await api.get<{ success: boolean; data: any }>('/profesionales/mi-perfil')
    const data = response.data?.data ?? response.data
    if (!data) throw new Error('No se recibieron datos del perfil.')
    return data as Profesional & {
      suscripcionActiva?: SuscripcionActiva | null
      suscripcionPendiente?: SuscripcionPendiente | null
      planActual?: string
    }
  },

  /** Solicitar plan de pago (transferencia) para profesional autenticado. */
  async solicitarSuscripcion(datos: SolicitarSuscripcionRequest): Promise<SolicitarSuscripcionApiResponse> {
    const response = await api.post<SolicitarSuscripcionApiResponse>('/profesionales/solicitar-suscripcion', datos)
    return response.data
  },

  /** Actualizar perfil del profesional (descripción, disponible, foto, zonas, categorías). */
  async actualizarMiPerfil(payload: {
    descripcion?: string
    disponible?: boolean
    fotoPerfilBase64?: string
    zonasCobertura?: ZonaCoberturaDto[]
    oficios?: string[]
  }): Promise<void> {
    await api.put('/profesionales/mi-perfil', payload)
  },

  /** Listar reseñas de un profesional. */
  async listarResenas(profesionalId: number): Promise<Resena[]> {
    const response = await api.get<Resena[]>(`/profesionales/${profesionalId}/resenas`)
    return Array.isArray(response.data) ? response.data : []
  },

  /** Crear reseña para un profesional (con o sin login). */
  async crearResena(profesionalId: number, datos: CrearResenaRequest): Promise<Resena> {
    const response = await api.post<Resena>(`/profesionales/${profesionalId}/resenas`, datos)
    return response.data
  }
}
