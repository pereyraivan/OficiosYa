// Servicio base para llamadas a la API
export class ApiService {
    constructor(baseUrl = '/api') {
        this.token = null;
        this.baseUrl = baseUrl;
        this.loadToken();
    }
    loadToken() {
        this.token = localStorage.getItem('authToken');
    }
    setToken(token) {
        this.token = token;
        localStorage.setItem('authToken', token);
    }
    clearToken() {
        this.token = null;
        localStorage.removeItem('authToken');
    }
    async request(endpoint, options = {}) {
        const url = `${this.baseUrl}${endpoint}`;
        const defaultHeaders = {
            'Content-Type': 'application/json',
        };
        if (this.token) {
            defaultHeaders['Authorization'] = `Bearer ${this.token}`;
        }
        const config = {
            ...options,
            headers: {
                ...defaultHeaders,
                ...options.headers,
            },
        };
        try {
            const response = await fetch(url, config);
            if (!response.ok) {
                const errorData = await response.json().catch(() => ({}));
                throw new Error(errorData.message || `HTTP error! status: ${response.status}`);
            }
            // Si la respuesta está vacía, retornar un objeto vacío
            const text = await response.text();
            if (!text) {
                return {};
            }
            return JSON.parse(text);
        }
        catch (error) {
            console.error('API request failed:', error);
            throw error;
        }
    }
    async get(endpoint) {
        return this.request(endpoint, { method: 'GET' });
    }
    async post(endpoint, data) {
        return this.request(endpoint, {
            method: 'POST',
            body: data ? JSON.stringify(data) : undefined,
        });
    }
    async put(endpoint, data) {
        return this.request(endpoint, {
            method: 'PUT',
            body: data ? JSON.stringify(data) : undefined,
        });
    }
    async delete(endpoint) {
        return this.request(endpoint, { method: 'DELETE' });
    }
}
// Instancia global del servicio API
export const apiService = new ApiService();
//# sourceMappingURL=api.js.map