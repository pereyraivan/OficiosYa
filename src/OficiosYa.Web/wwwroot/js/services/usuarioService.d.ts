import { Usuario, RegistrarUsuarioRequest, LoginRequest, LoginResponse } from '../types';
export declare class UsuarioService {
    private baseEndpoint;
    registrarUsuario(request: RegistrarUsuarioRequest): Promise<Usuario>;
    login(request: LoginRequest): Promise<LoginResponse>;
    obtenerUsuario(id: number): Promise<Usuario>;
    verificarEmailDisponible(email: string): Promise<{
        disponible: boolean;
    }>;
    logout(): void;
    isLoggedIn(): boolean;
}
export declare const usuarioService: UsuarioService;
//# sourceMappingURL=usuarioService.d.ts.map