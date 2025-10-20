import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Usuario } from '../types'
import { usuarioService } from '../services/usuarioService'

export const useAuthStore = defineStore('auth', () => {
  // Estado
  const usuario = ref<Usuario | null>(null)
  const token = ref<string | null>(localStorage.getItem('auth_token'))
  const cargando = ref(false)

  // Getters
  const estaAutenticado = computed(() => !!token.value && !!usuario.value)
  const esProfesional = computed(() => usuario.value?.esProfesional || false)

  // Actions
  const login = async (email: string, contraseña: string) => {
    cargando.value = true
    try {
      const resultado = await usuarioService.login({ email, contraseña })
      usuario.value = resultado.usuario
      token.value = resultado.token
      localStorage.setItem('auth_token', resultado.token)
      return true
    } catch (error) {
      console.error('Error en login:', error)
      return false
    } finally {
      cargando.value = false
    }
  }

  const registrar = async (datosUsuario: any) => {
    cargando.value = true
    try {
      const nuevoUsuario = await usuarioService.registrar(datosUsuario)
      usuario.value = nuevoUsuario
      return true
    } catch (error) {
      console.error('Error en registro:', error)
      return false
    } finally {
      cargando.value = false
    }
  }

  const logout = () => {
    usuario.value = null
    token.value = null
    localStorage.removeItem('auth_token')
  }

  const cargarPerfil = async () => {
    if (!token.value) return

    cargando.value = true
    try {
      const perfil = await usuarioService.obtenerPerfil()
      usuario.value = perfil
    } catch (error) {
      console.error('Error cargando perfil:', error)
      logout()
    } finally {
      cargando.value = false
    }
  }

  return {
    // Estado
    usuario,
    token,
    cargando,
    // Getters
    estaAutenticado,
    esProfesional,
    // Actions
    login,
    registrar,
    logout,
    cargarPerfil
  }
})
