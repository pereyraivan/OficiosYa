// Validadores reutilizables para formularios
export interface ValidationResult {
  isValid: boolean
  message?: string
}

export const validators = {
  // Validar email
  email: (email: string): ValidationResult => {
    if (!email.trim()) {
      return { isValid: false, message: 'El email es obligatorio' }
    }
    
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    if (!emailRegex.test(email)) {
      return { isValid: false, message: 'El email no tiene un formato válido' }
    }
    
    return { isValid: true }
  },

  // Validar contraseña
  password: (password: string): ValidationResult => {
    if (!password) {
      return { isValid: false, message: 'La contraseña es obligatoria' }
    }
    
    if (password.length < 6) {
      return { isValid: false, message: 'La contraseña debe tener al menos 6 caracteres' }
    }
    
    return { isValid: true }
  },

  // Validar confirmación de contraseña
  confirmPassword: (password: string, confirmPassword: string): ValidationResult => {
    if (!confirmPassword) {
      return { isValid: false, message: 'Debes confirmar la contraseña' }
    }
    
    if (password !== confirmPassword) {
      return { isValid: false, message: 'Las contraseñas no coinciden' }
    }
    
    return { isValid: true }
  },

  // Validar campo requerido
  required: (value: string, fieldName: string = 'Este campo'): ValidationResult => {
    if (!value.trim()) {
      return { isValid: false, message: `${fieldName} es obligatorio` }
    }
    
    return { isValid: true }
  },

  // Validar longitud mínima
  minLength: (value: string, minLength: number, fieldName: string = 'Este campo'): ValidationResult => {
    if (value.trim().length < minLength) {
      return { isValid: false, message: `${fieldName} debe tener al menos ${minLength} caracteres` }
    }
    
    return { isValid: true }
  },

  // Validar teléfono (formato básico)
  phone: (phone: string): ValidationResult => {
    if (!phone.trim()) {
      return { isValid: true } // Teléfono es opcional
    }
    
    const phoneRegex = /^[\+]?[0-9\s\-\(\)]{10,}$/
    if (!phoneRegex.test(phone)) {
      return { isValid: false, message: 'El teléfono no tiene un formato válido' }
    }
    
    return { isValid: true }
  },

  // Validar que se seleccione al menos un elemento de un array
  atLeastOne: (array: any[], fieldName: string = 'Debes seleccionar al menos una opción'): ValidationResult => {
    if (array.length === 0) {
      return { isValid: false, message: fieldName }
    }
    
    return { isValid: true }
  },

  // Validar términos y condiciones
  termsAccepted: (accepted: boolean): ValidationResult => {
    if (!accepted) {
      return { isValid: false, message: 'Debes aceptar los términos y condiciones' }
    }
    
    return { isValid: true }
  }
}

// Función helper para validar múltiples campos
export const validateFields = (validations: Record<string, ValidationResult>): Record<string, string> => {
  const errors: Record<string, string> = {}
  
  Object.entries(validations).forEach(([field, result]) => {
    if (!result.isValid && result.message) {
      errors[field] = result.message
    }
  })
  
  return errors
}
