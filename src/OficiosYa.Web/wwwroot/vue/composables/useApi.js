import { ref, reactive } from 'vue'

// Servicio base para llamadas a la API
class ApiService {
  constructor(baseUrl = '/api') {
    this.baseUrl = baseUrl
    this.token = localStorage.getItem('authToken')
  }

  setToken(token) {
    this.token = token
    localStorage.setItem('authToken', token)
  }

  clearToken() {
    this.token = null
    localStorage.removeItem('authToken')
  }

  async request(endpoint, options = {}) {
    const url = `${this.baseUrl}${endpoint}`
    
    const defaultHeaders = {
      'Content-Type': 'application/json',
    }

    if (this.token) {
      defaultHeaders['Authorization'] = `Bearer ${this.token}`
    }

    const config = {
      ...options,
      headers: {
        ...defaultHeaders,
        ...options.headers,
      },
    }

    try {
      const response = await fetch(url, config)
      
      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}))
        throw new Error(errorData.message || `HTTP error! status: ${response.status}`)
      }

      const text = await response.text()
      if (!text) {
        return {}
      }

      return JSON.parse(text)
    } catch (error) {
      console.error('API request failed:', error)
      throw error
    }
  }

  async get(endpoint) {
    return this.request(endpoint, { method: 'GET' })
  }

  async post(endpoint, data) {
    return this.request(endpoint, {
      method: 'POST',
      body: data ? JSON.stringify(data) : undefined,
    })
  }

  async put(endpoint, data) {
    return this.request(endpoint, {
      method: 'PUT',
      body: data ? JSON.stringify(data) : undefined,
    })
  }

  async delete(endpoint) {
    return this.request(endpoint, { method: 'DELETE' })
  }
}

// Instancia global del servicio API
const apiService = new ApiService()

// Composable para usar la API
export function useApi() {
  const loading = ref(false)
  const error = ref(null)

  const execute = async (apiCall) => {
    loading.value = true
    error.value = null
    
    try {
      const result = await apiCall()
      return result
    } catch (err) {
      error.value = err.message
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    apiService,
    loading,
    error,
    execute
  }
}

// Composable para autenticación
export function useAuth() {
  const { apiService, loading, error, execute } = useApi()
  const user = ref(null)
  const isLoggedIn = ref(!!apiService.token)

  const login = async (credentials) => {
    return execute(async () => {
      const response = await apiService.post('/usuarios/login', credentials)
      
      if (response.success && response.token) {
        apiService.setToken(response.token)
        user.value = response.usuario
        isLoggedIn.value = true
      }
      
      return response
    })
  }

  const register = async (userData) => {
    return execute(async () => {
      const response = await apiService.post('/usuarios/registrar', userData)
      return response
    })
  }

  const logout = () => {
    apiService.clearToken()
    user.value = null
    isLoggedIn.value = false
  }

  return {
    user,
    isLoggedIn,
    loading,
    error,
    login,
    register,
    logout
  }
}

// Composable para oficios
export function useOficios() {
  const { apiService, loading, error, execute } = useApi()
  const oficios = ref([])

  const loadOficios = async () => {
    return execute(async () => {
      const data = await apiService.get('/oficios')
      oficios.value = data
      return data
    })
  }

  const searchProfessionals = async (filters = {}) => {
    return execute(async () => {
      const params = new URLSearchParams()
      
      Object.keys(filters).forEach(key => {
        if (filters[key] !== undefined && filters[key] !== '') {
          params.append(key, filters[key])
        }
      })

      const queryString = params.toString()
      const endpoint = queryString ? `/oficios/buscar?${queryString}` : '/oficios/buscar'
      
      return await apiService.get(endpoint)
    })
  }

  return {
    oficios,
    loading,
    error,
    loadOficios,
    searchProfessionals
  }
}
