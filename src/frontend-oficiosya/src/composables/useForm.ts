import { ref, reactive, computed } from 'vue'
import { validators, validateFields, type ValidationResult } from '../utils/validators'

export interface FormField {
  value: any
  validator?: (value: any) => ValidationResult
  required?: boolean
}

export interface UseFormOptions {
  initialValues: Record<string, any>
  validations?: Record<string, (value: any) => ValidationResult>
  onSubmit?: (values: Record<string, any>) => Promise<void> | void
}

export function useForm(options: UseFormOptions) {
  const { initialValues, validations = {}, onSubmit } = options
  
  // Estado del formulario
  const form = reactive<Record<string, any>>({ ...initialValues })
  const errors = reactive<Record<string, string>>({})
  const loading = ref(false)
  const submitted = ref(false)

  // Computed para verificar si el formulario es válido
  const isValid = computed(() => {
    return Object.keys(errors).length === 0 && 
           Object.values(form).some(value => value !== '' && value !== null && value !== undefined)
  })

  // Función para limpiar errores
  const clearErrors = () => {
    Object.keys(errors).forEach(key => {
      errors[key] = ''
    })
  }

  // Función para validar un campo específico
  const validateField = (fieldName: string, value: any): boolean => {
    const validator = validations[fieldName]
    if (!validator) return true

    const result = validator(value)
    if (!result.isValid && result.message) {
      errors[fieldName] = result.message
      return false
    } else {
      errors[fieldName] = ''
      return true
    }
  }

  // Función para validar todo el formulario
  const validateForm = (): boolean => {
    clearErrors()
    
    const validationResults: Record<string, ValidationResult> = {}
    
    Object.entries(validations).forEach(([field, validator]) => {
      validationResults[field] = validator(form[field])
    })
    
    const newErrors = validateFields(validationResults)
    Object.assign(errors, newErrors)
    
    return Object.keys(newErrors).length === 0
  }

  // Función para establecer un error manualmente
  const setError = (field: string, message: string) => {
    errors[field] = message
  }

  // Función para establecer múltiples errores
  const setErrors = (newErrors: Record<string, string>) => {
    Object.assign(errors, newErrors)
  }

  // Función para resetear el formulario
  const resetForm = () => {
    Object.keys(form).forEach(key => {
      form[key] = initialValues[key] || ''
    })
    clearErrors()
    submitted.value = false
  }

  // Función para manejar el envío del formulario
  const handleSubmit = async (event?: Event) => {
    if (event) {
      event.preventDefault()
    }
    
    submitted.value = true
    
    if (!validateForm()) {
      return false
    }

    if (onSubmit) {
      loading.value = true
      try {
        await onSubmit(form)
        return true
      } catch (error: any) {
        console.error('Error en submit:', error)
        setError('general', error.message || 'Ha ocurrido un error')
        return false
      } finally {
        loading.value = false
      }
    }
    
    return true
  }

  // Función para actualizar un campo
  const updateField = (field: string, value: any) => {
    form[field] = value
    
    // Validar automáticamente si ya se envió el formulario
    if (submitted.value) {
      validateField(field, value)
    }
  }

  return {
    // Estado
    form,
    errors,
    loading,
    submitted,
    isValid,
    
    // Métodos
    clearErrors,
    validateField,
    validateForm,
    setError,
    setErrors,
    resetForm,
    handleSubmit,
    updateField
  }
}

// Composable específico para formularios de registro de usuarios
export function useUserRegistrationForm() {
  return useForm({
    initialValues: {
      nombre: '',
      email: '',
      telefono: '',
      contraseña: '',
      confirmarContraseña: '',
      esProfesional: false,
      aceptaTerminos: false
    },
    validations: {
      nombre: (value) => validators.minLength(value, 2, 'El nombre'),
      email: (value) => validators.email(value),
      telefono: (value) => validators.phone(value),
      contraseña: (value) => validators.password(value),
      confirmarContraseña: (value, form) => validators.confirmPassword(form.contraseña, value),
      aceptaTerminos: (value) => validators.termsAccepted(value)
    }
  })
}

// Composable específico para formularios de registro de profesionales
export function useProfessionalRegistrationForm() {
  return useForm({
    initialValues: {
      nombre: '',
      apellido: '',
      email: '',
      telefono: '',
      password: '',
      confirmarPassword: '',
      direccion: '',
      ciudad: '',
      provincia: '',
      codigoPostal: '',
      descripcion: '',
      oficios: [],
      aceptaTerminos: false
    },
    validations: {
      nombre: (value) => validators.minLength(value, 2, 'El nombre'),
      apellido: (value) => validators.minLength(value, 2, 'El apellido'),
      email: (value) => validators.email(value),
      telefono: (value) => validators.phone(value),
      password: (value) => validators.password(value),
      confirmarPassword: (value, form) => validators.confirmPassword(form.password, value),
      ciudad: (value) => validators.required(value, 'La ciudad'),
      provincia: (value) => validators.required(value, 'La provincia'),
      oficios: (value) => validators.atLeastOne(value, 'Debes seleccionar al menos un oficio'),
      aceptaTerminos: (value) => validators.termsAccepted(value)
    }
  })
}
