import api from './api'
import type { Profesional, BuscarProfesionalesRequest, RegistrarProfesionalRequest, ApiResponse } from '../types'

export const profesionalService = {
  // Registrar profesional
  async registrar(datos: RegistrarProfesionalRequest): Promise<Profesional> {
    const response = await api.post<ApiResponse<Profesional>>('/profesionales/registrar', datos)
    return response.data.data
  },

  // Buscar profesionales
  async buscar(criterios: BuscarProfesionalesRequest): Promise<Profesional[]> {
    const response = await api.get<ApiResponse<Profesional[]>>('/profesionales/buscar', {
      params: criterios
    })
    return response.data.data
  },

  // Obtener profesionales destacados
  async obtenerDestacados(): Promise<Profesional[]> {
    const response = await api.get<ApiResponse<Profesional[]>>('/profesionales/destacados')
    return response.data.data
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
  }
}
