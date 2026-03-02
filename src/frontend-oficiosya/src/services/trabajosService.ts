import api from './api'

export interface PublicarTrabajoRequest {
  descripcion: string
  telefono?: string
  email: string
  categoriaIds: number[]
}

export interface TrabajoListItem {
  id: number
  descripcionResumen: string
  descripcion: string
  estado: string
  fechaCreacion: string
  categorias: { id: number; nombre: string }[]
}

export interface TrabajoDetalle extends TrabajoListItem {
  telefono?: string
  email: string
}

export interface ListarTrabajosParams {
  estado?: string
  categoriaIds?: number[]
  page?: number
  pageSize?: number
}

export interface ListarTrabajosResult {
  items: TrabajoListItem[]
  total: number
  page: number
  pageSize: number
  totalPaginas: number
}

export const trabajosService = {
  async publicar(data: PublicarTrabajoRequest): Promise<{ id: number }> {
    const response = await api.post<{ success: boolean; data: { id: number } }>('/trabajos', {
      descripcion: data.descripcion,
      telefono: data.telefono || undefined,
      email: data.email,
      categoriaIds: data.categoriaIds
    })
    return response.data.data
  },

  async listar(params: ListarTrabajosParams = {}): Promise<ListarTrabajosResult> {
    const searchParams: Record<string, string | number> = {
      page: params.page ?? 1,
      pageSize: params.pageSize ?? 20
    }
    if (params.estado != null) searchParams.estado = params.estado
    if (params.categoriaIds?.length) searchParams.categoriaIds = params.categoriaIds.join(',')
    const response = await api.get<{ success: boolean; data: ListarTrabajosResult }>('/trabajos', {
      params: searchParams
    })
    return response.data.data
  },

  async obtenerPorId(id: number): Promise<TrabajoDetalle> {
    const response = await api.get<{ success: boolean; data: TrabajoDetalle }>(`/trabajos/${id}`)
    return response.data.data
  }
}
