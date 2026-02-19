import api from './api'
import type { Usuario, RegistrarUsuarioRequest, LoginRequest, ApiResponse } from '../types'

export const usuarioService = {
  // Registrar nuevo usuario
  async registrar(usuario: RegistrarUsuarioRequest): Promise<Usuario> {
    // El backend devuelve directamente el objeto UsuarioResponse
    const response = await api.post<Usuario>('/usuarios/registrar', usuario)
    return response.data
  },

  // Login de usuario
  async login(credenciales: LoginRequest): Promise<{ usuario: Usuario; token: string }> {
    const response = await api.post<ApiResponse<{ usuario: Usuario; token: string }>>('/usuarios/login', credenciales)
    return response.data.data
  },

  // Obtener usuario por ID
  async obtenerPorId(id: number): Promise<Usuario> {
    const response = await api.get<ApiResponse<Usuario>>(`/usuarios/${id}`)
    return response.data.data
  },

  // Obtener perfil del usuario actual
  async obtenerPerfil(): Promise<Usuario> {
    const response = await api.get<ApiResponse<Usuario>>('/usuarios/perfil')
    return response.data.data
  },

  // Actualizar perfil
  async actualizarPerfil(usuario: Partial<Usuario>): Promise<Usuario> {
    const response = await api.put<ApiResponse<Usuario>>('/usuarios/perfil', usuario)
    return response.data.data
  },

  // Verificar si el email está disponible
  async verificarEmail(email: string): Promise<boolean> {
    try {
      const response = await api.get<{ disponible: boolean }>(`/usuarios/verificar-email?email=${encodeURIComponent(email)}`)
      return response.data.disponible
    } catch {
      return false
    }
  }
}
