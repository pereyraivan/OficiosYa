export interface Usuario {
    id: number;
    nombre: string;
    apellido: string;
    email: string;
    telefono?: string;
    direccion?: string;
    ciudad?: string;
    provincia?: string;
    codigoPostal?: string;
    fechaRegistro: string;
    emailVerificado: boolean;
    activo: boolean;
}
export interface RegistrarUsuarioRequest {
    nombre: string;
    apellido: string;
    email: string;
    password: string;
    telefono?: string;
    direccion?: string;
    ciudad?: string;
    provincia?: string;
    codigoPostal?: string;
}
export interface LoginRequest {
    email: string;
    password: string;
}
export interface LoginResponse {
    success: boolean;
    token?: string;
    usuario?: Usuario;
    message?: string;
}
export interface Oficio {
    id: number;
    nombre: string;
    descripcion?: string;
    icono?: string;
    color?: string;
    activo: boolean;
    fechaCreacion: string;
}
export interface Profesional {
    id: number;
    usuarioId: number;
    descripcion?: string;
    fotoPerfil?: string;
    calificacionPromedio: number;
    totalResenas: number;
    disponible: boolean;
    fechaRegistro: string;
    activo: boolean;
    usuario?: Usuario;
    profesionalOficios?: ProfesionalOficio[];
}
export interface ProfesionalOficio {
    id: number;
    profesionalId: number;
    oficioId: number;
    descripcionEspecializada?: string;
    precioHora: number;
    anosExperiencia: number;
    activo: boolean;
    fechaRegistro: string;
    profesional?: Profesional;
    oficio?: Oficio;
}
export interface ApiResponse<T> {
    success: boolean;
    data?: T;
    message?: string;
    errors?: string[];
}
export interface SearchFilters {
    oficioId?: number;
    ciudad?: string;
    precioMin?: number;
    precioMax?: number;
    calificacionMin?: number;
    disponible?: boolean;
}
//# sourceMappingURL=index.d.ts.map