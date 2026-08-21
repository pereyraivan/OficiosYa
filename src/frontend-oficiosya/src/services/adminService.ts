import api from './api'
import type { CategoriaServicio, CrearCategoriaServicioRequest, ActualizarCategoriaServicioRequest, SuscripcionPendienteAdmin, ConfirmarSuscripcionAdminResponse, ProfesionalAdmin, ActualizarProfesionalAdminRequest } from '../types'
import { useAuthStore } from '../stores/auth'

const getAdminHeaders = () => {
  const authStore = useAuthStore()
  const email = authStore.usuario?.email
  return email ? { 'X-Admin-Email': email } : {}
}

export const adminService = {
  // ========== CATEGORÍAS DE SERVICIO ==========

  async obtenerCategoriaServicios(tipo?: 'Oficiante' | 'Profesional', soloActivos: boolean = true): Promise<CategoriaServicio[]> {
    const params: any = { soloActivos }
    if (tipo) params.tipo = tipo

    const response = await api.get<CategoriaServicio[]>('/admin/categoriaservicios', {
      params,
      headers: getAdminHeaders()
    })
    return response.data
  },

  async obtenerCategoriaServicio(id: number): Promise<CategoriaServicio> {
    const response = await api.get<CategoriaServicio>(`/admin/categoriaservicios/${id}`, {
      headers: getAdminHeaders()
    })
    return response.data
  },

  async crearCategoriaServicio(datos: CrearCategoriaServicioRequest): Promise<CategoriaServicio> {
    const response = await api.post<CategoriaServicio>('/admin/categoriaservicios', datos, {
      headers: getAdminHeaders()
    })
    return response.data
  },

  async actualizarCategoriaServicio(id: number, datos: ActualizarCategoriaServicioRequest): Promise<CategoriaServicio> {
    const response = await api.put<CategoriaServicio>(`/admin/categoriaservicios/${id}`, datos, {
      headers: getAdminHeaders()
    })
    return response.data
  },

  async eliminarCategoriaServicio(id: number): Promise<void> {
    await api.delete(`/admin/categoriaservicios/${id}`, {
      headers: getAdminHeaders()
    })
  },

  // ========== SUSCRIPCIONES / PAGOS ==========

  async obtenerSuscripcionesPendientes(): Promise<SuscripcionPendienteAdmin[]> {
    const response = await api.get<SuscripcionPendienteAdmin[]>('/admin/suscripciones/pendientes', {
      headers: getAdminHeaders()
    })
    return response.data
  },

  async confirmarSuscripcion(id: number): Promise<ConfirmarSuscripcionAdminResponse> {
    const response = await api.post<ConfirmarSuscripcionAdminResponse>(
      `/admin/suscripciones/${id}/confirmar`,
      {},
      { headers: getAdminHeaders() }
    )
    return response.data
  },

  async rechazarSuscripcion(id: number): Promise<{ message: string; id: number }> {
    const response = await api.post<{ message: string; id: number }>(
      `/admin/suscripciones/${id}/rechazar`,
      {},
      { headers: getAdminHeaders() }
    )
    return response.data
  },

  // ========== PROFESIONALES / OFICIANTES ==========

  async obtenerProfesionales(opts?: {
    tipo?: 'Oficiante' | 'Profesional'
    soloActivos?: boolean
  }): Promise<ProfesionalAdmin[]> {
    const params: Record<string, string | boolean> = {}
    if (opts?.tipo) params.tipo = opts.tipo
    if (opts?.soloActivos != null) params.soloActivos = opts.soloActivos

    const response = await api.get<ProfesionalAdmin[]>('/admin/profesionales', {
      params,
      headers: getAdminHeaders()
    })
    return Array.isArray(response.data) ? response.data : []
  },

  async obtenerProfesional(id: number): Promise<ProfesionalAdmin> {
    const response = await api.get<ProfesionalAdmin>(`/admin/profesionales/${id}`, {
      headers: getAdminHeaders()
    })
    return response.data
  },

  async actualizarProfesional(id: number, datos: ActualizarProfesionalAdminRequest): Promise<ProfesionalAdmin> {
    const response = await api.put<ProfesionalAdmin>(`/admin/profesionales/${id}`, datos, {
      headers: getAdminHeaders()
    })
    return response.data
  },

  async eliminarProfesional(id: number): Promise<{ message: string; id: number }> {
    const response = await api.delete<{ message: string; id: number }>(`/admin/profesionales/${id}`, {
      headers: getAdminHeaders()
    })
    return response.data
  }
}
