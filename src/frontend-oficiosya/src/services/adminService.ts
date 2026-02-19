import api from './api'
import type { Oficio, CategoriaServicio, CrearOficioRequest, ActualizarOficioRequest, CrearCategoriaServicioRequest, ActualizarCategoriaServicioRequest } from '../types'
import { useAuthStore } from '../stores/auth'

// Función helper para agregar el header de admin
const getAdminHeaders = () => {
  const authStore = useAuthStore()
  const email = authStore.usuario?.email
  return email ? { 'X-Admin-Email': email } : {}
}

export const adminService = {
  // ========== OFICIOS ==========
  
  async obtenerOficios(soloActivos: boolean = true): Promise<Oficio[]> {
    const response = await api.get<Oficio[]>('/admin/oficios', {
      params: { soloActivos },
      headers: getAdminHeaders()
    })
    return response.data
  },

  async obtenerOficio(id: number): Promise<Oficio> {
    const response = await api.get<Oficio>(`/admin/oficios/${id}`, {
      headers: getAdminHeaders()
    })
    return response.data
  },

  async crearOficio(datos: CrearOficioRequest): Promise<Oficio> {
    const response = await api.post<Oficio>('/admin/oficios', datos, {
      headers: getAdminHeaders()
    })
    return response.data
  },

  async actualizarOficio(id: number, datos: ActualizarOficioRequest): Promise<Oficio> {
    const response = await api.put<Oficio>(`/admin/oficios/${id}`, datos, {
      headers: getAdminHeaders()
    })
    return response.data
  },

  async eliminarOficio(id: number): Promise<void> {
    await api.delete(`/admin/oficios/${id}`, {
      headers: getAdminHeaders()
    })
  },

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
  }
}

