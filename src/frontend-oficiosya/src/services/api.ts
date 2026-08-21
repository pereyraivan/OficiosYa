import axios from 'axios'
import type { ApiResponse, ErrorResponse } from '../types'

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5230/api'

// Configuración base de axios
const api = axios.create({
  baseURL: API_BASE_URL,
  timeout: 60000, // 60 segundos para operaciones complejas (registro, carga de datos, etc.)
  headers: {
    'Content-Type': 'application/json; charset=utf-8',
    'Accept': 'application/json'
  },
  responseType: 'json',
  responseEncoding: 'utf8'
})

// Interceptor para requests
api.interceptors.request.use(
  (config) => {
    // Agregar token de autenticación si existe
    const token = localStorage.getItem('auth_token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Interceptor para responses
api.interceptors.response.use(
  (response) => {
    return response
  },
  (error) => {
    // Manejar errores de autenticación (no redirigir si el 401 viene del login)
    if (error.response?.status === 401 && !error.config?.url?.includes('/login')) {
      localStorage.removeItem('auth_token')
      window.location.href = '/login'
    }
    
    // Si el error no tiene response, es un error de red
    if (!error.response) {
      const networkError: ErrorResponse = {
        message: error.message || 'Error de conexión. Verifica que el backend esté corriendo.',
        errors: undefined
      }
      return Promise.reject(networkError)
    }
    
    // Formatear errores de la API (incluye validación ModelState de ASP.NET)
    const data = error.response?.data
    let message = data?.message as string | undefined
    const validationErrors = data?.errors as Record<string, string[]> | undefined
    if (!message && validationErrors) {
      const firstField = Object.keys(validationErrors)[0]
      if (firstField && validationErrors[firstField]?.[0]) {
        message = validationErrors[firstField][0]
      }
    }

    const errorResponse: ErrorResponse = {
      message: message || `Error ${error.response.status}: ${error.response.statusText}`,
      errors: validationErrors ?? data?.errors
    }
    
    return Promise.reject(errorResponse)
  }
)

export default api
