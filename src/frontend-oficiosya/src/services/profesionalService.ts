import api from './api'
import type { Profesional, BuscarProfesionalesRequest, RegistrarProfesionalRequest, ApiResponse, CategoriaServicio } from '../types'

export const profesionalService = {
  // Registrar profesional
  async registrar(datos: RegistrarProfesionalRequest): Promise<Profesional> {
    const response = await api.post<Profesional>('/profesionales/registrar', datos)
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

  // Obtener profesional por ID
  async obtenerPorId(id: number): Promise<Profesional> {
    const response = await api.get<ApiResponse<Profesional>>(`/profesionales/${id}`)
    return response.data.data
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
  }
}
