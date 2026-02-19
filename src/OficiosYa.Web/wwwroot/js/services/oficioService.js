import { apiService } from './api.js';
export class OficioService {
    constructor() {
        this.baseEndpoint = '/oficios';
    }
    async obtenerOficios() {
        return apiService.get(this.baseEndpoint);
    }
    async obtenerOficio(id) {
        return apiService.get(`${this.baseEndpoint}/${id}`);
    }
    async buscarProfesionales(filters = {}) {
        const params = new URLSearchParams();
        if (filters.oficioId)
            params.append('oficioId', filters.oficioId.toString());
        if (filters.ciudad)
            params.append('ciudad', filters.ciudad);
        if (filters.precioMin)
            params.append('precioMin', filters.precioMin.toString());
        if (filters.precioMax)
            params.append('precioMax', filters.precioMax.toString());
        if (filters.calificacionMin)
            params.append('calificacionMin', filters.calificacionMin.toString());
        if (filters.disponible !== undefined)
            params.append('disponible', filters.disponible.toString());
        const queryString = params.toString();
        const endpoint = queryString ? `${this.baseEndpoint}/buscar?${queryString}` : `${this.baseEndpoint}/buscar`;
        return apiService.get(endpoint);
    }
    async obtenerProfesionalesDestacados() {
        return apiService.get(`${this.baseEndpoint}/destacados`);
    }
    async obtenerProfesional(id) {
        return apiService.get(`${this.baseEndpoint}/profesional/${id}`);
    }
}
// Instancia global del servicio de oficios
export const oficioService = new OficioService();
//# sourceMappingURL=oficioService.js.map