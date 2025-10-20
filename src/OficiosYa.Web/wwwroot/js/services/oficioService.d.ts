import { Oficio, Profesional, SearchFilters } from '../types';
export declare class OficioService {
    private baseEndpoint;
    obtenerOficios(): Promise<Oficio[]>;
    obtenerOficio(id: number): Promise<Oficio>;
    buscarProfesionales(filters?: SearchFilters): Promise<Profesional[]>;
    obtenerProfesionalesDestacados(): Promise<Profesional[]>;
    obtenerProfesional(id: number): Promise<Profesional>;
}
export declare const oficioService: OficioService;
//# sourceMappingURL=oficioService.d.ts.map