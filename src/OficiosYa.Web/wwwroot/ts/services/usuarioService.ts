import { apiService } from './api';
import { Usuario, RegistrarUsuarioRequest, LoginRequest, LoginResponse } from '../types';

export class UsuarioService {
  private baseEndpoint = '/usuarios';

  async registrarUsuario(request: RegistrarUsuarioRequest): Promise<Usuario> {
    return apiService.post<Usuario>(`${this.baseEndpoint}/registrar`, request);
  }

  async login(request: LoginRequest): Promise<LoginResponse> {
    const response = await apiService.post<LoginResponse>(`${this.baseEndpoint}/login`, request);
    
    // Si el login es exitoso, guardar el token
    if (response.success && response.token) {
      apiService.setToken(response.token);
    }
    
    return response;
  }

  async obtenerUsuario(id: number): Promise<Usuario> {
    return apiService.get<Usuario>(`${this.baseEndpoint}/${id}`);
  }

  async verificarEmailDisponible(email: string): Promise<{ disponible: boolean }> {
    return apiService.get<{ disponible: boolean }>(`${this.baseEndpoint}/verificar-email?email=${encodeURIComponent(email)}`);
  }

  logout(): void {
    apiService.clearToken();
  }

  isLoggedIn(): boolean {
    return apiService['token'] !== null;
  }
}

// Instancia global del servicio de usuarios
export const usuarioService = new UsuarioService();
