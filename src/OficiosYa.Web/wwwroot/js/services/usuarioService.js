import { apiService } from './api';
export class UsuarioService {
    constructor() {
        this.baseEndpoint = '/usuarios';
    }
    async registrarUsuario(request) {
        return apiService.post(`${this.baseEndpoint}/registrar`, request);
    }
    async login(request) {
        const response = await apiService.post(`${this.baseEndpoint}/login`, request);
        // Si el login es exitoso, guardar el token
        if (response.success && response.token) {
            apiService.setToken(response.token);
        }
        return response;
    }
    async obtenerUsuario(id) {
        return apiService.get(`${this.baseEndpoint}/${id}`);
    }
    async verificarEmailDisponible(email) {
        return apiService.get(`${this.baseEndpoint}/verificar-email?email=${encodeURIComponent(email)}`);
    }
    logout() {
        apiService.clearToken();
    }
    isLoggedIn() {
        return apiService['token'] !== null;
    }
}
// Instancia global del servicio de usuarios
export const usuarioService = new UsuarioService();
//# sourceMappingURL=usuarioService.js.map