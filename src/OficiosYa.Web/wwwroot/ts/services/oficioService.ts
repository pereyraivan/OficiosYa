import { apiService } from './api';
import { Oficio, Profesional, SearchFilters } from '../types';

export class OficioService {
  private baseEndpoint = '/oficios';

  async obtenerOficios(): Promise<Oficio[]> {
    return apiService.get<Oficio[]>(this.baseEndpoint);
  }

  async obtenerOficio(id: number): Promise<Oficio> {
    return apiService.get<Oficio>(`${this.baseEndpoint}/${id}`);
  }

  async buscarProfesionales(filters: SearchFilters = {}): Promise<Profesional[]> {
    const params = new URLSearchParams();
    
    if (filters.oficioId) params.append('oficioId', filters.oficioId.toString());
    if (filters.ciudad) params.append('ciudad', filters.ciudad);
    if (filters.precioMin) params.append('precioMin', filters.precioMin.toString());
    if (filters.precioMax) params.append('precioMax', filters.precioMax.toString());
    if (filters.calificacionMin) params.append('calificacionMin', filters.calificacionMin.toString());
    if (filters.disponible !== undefined) params.append('disponible', filters.disponible.toString());

    const queryString = params.toString();
    const endpoint = queryString ? `${this.baseEndpoint}/buscar?${queryString}` : `${this.baseEndpoint}/buscar`;
    
    return apiService.get<Profesional[]>(endpoint);
  }

  async obtenerProfesionalesDestacados(): Promise<Profesional[]> {
    return apiService.get<Profesional[]>(`${this.baseEndpoint}/destacados`);
  }

  async obtenerProfesional(id: number): Promise<Profesional> {
    return apiService.get<Profesional>(`${this.baseEndpoint}/profesional/${id}`);
  }
}

// Instancia global del servicio de oficios
export const oficioService = new OficioService();
