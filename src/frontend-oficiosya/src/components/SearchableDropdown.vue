<template>
  <div class="searchable-dropdown" ref="containerRef">
    <div class="search-field">
      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
        <path d="M3 7h18M3 12h18M3 17h18" stroke="currentColor" stroke-width="2" stroke-linecap="round"/>
      </svg>
      <input
        ref="inputRef"
        v-model="searchText"
        type="text"
        :placeholder="!selectedValue && defaultOption ? defaultOption : placeholder"
        class="search-input"
        :disabled="disabled"
        @input="onInput"
        @focus="onFocus"
        @blur="onBlur"
        @keydown.arrow-down.prevent="navigateDown"
        @keydown.arrow-up.prevent="navigateUp"
        @keydown.enter.prevent="selectCurrent"
        @keydown.escape="closeDropdown"
      />
      <svg 
        width="12" 
        height="12" 
        viewBox="0 0 12 12" 
        fill="none" 
        xmlns="http://www.w3.org/2000/svg"
        class="dropdown-arrow"
      >
        <path fill="currentColor" d="M6 9L1 4h10z"/>
      </svg>
    </div>
    
    <!-- Lista de opciones -->
    <div 
      v-if="showDropdown && filteredOptions.length > 0" 
      class="dropdown-options"
      ref="dropdownRef"
    >
      <div
        v-for="(option, index) in filteredOptions"
        :key="option"
        class="dropdown-option"
        :class="{ 'active': index === selectedIndex, 'selected': option === selectedValue }"
        @click="selectOption(option)"
        @mouseenter="selectedIndex = index"
      >
        {{ option }}
      </div>
    </div>
    
    <div 
      v-if="showDropdown && filteredOptions.length === 0 && searchText.length > 0" 
      class="dropdown-options sin-resultados"
    >
      <div class="dropdown-option">No se encontraron resultados</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch, onMounted, onUnmounted, nextTick } from 'vue'

interface Props {
  modelValue: string
  options: string[]
  placeholder?: string
  disabled?: boolean
  defaultOption?: string
}

const props = withDefaults(defineProps<Props>(), {
  placeholder: 'Buscar...',
  disabled: false,
  defaultOption: ''
})

const emit = defineEmits<{
  'update:modelValue': [value: string]
}>()

const inputRef = ref<HTMLInputElement | null>(null)
const containerRef = ref<HTMLElement | null>(null)
const dropdownRef = ref<HTMLElement | null>(null)
const searchText = ref('')
const showDropdown = ref(false)
const selectedIndex = ref(-1)
const selectedValue = ref(props.modelValue)

// Función para normalizar texto (remover acentos para búsqueda más flexible)
const normalizeText = (text: string): string => {
  return text
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '') // Remover diacríticos
    .trim()
}

// Filtrar opciones basado en el texto de búsqueda
const filteredOptions = computed(() => {
  if (!searchText.value.trim()) {
    return props.options
  }
  
  const searchNormalized = normalizeText(searchText.value)
  return props.options.filter(option => 
    normalizeText(option).includes(searchNormalized) || 
    option.toLowerCase().includes(searchText.value.toLowerCase().trim())
  )
})

// Sincronizar con el valor del modelo
watch(() => props.modelValue, (newValue) => {
  selectedValue.value = newValue
  if (newValue) {
    searchText.value = newValue
  } else {
    searchText.value = ''
  }
}, { immediate: true })

// Sincronizar con las opciones
watch(() => props.options, () => {
  if (selectedValue.value && !props.options.includes(selectedValue.value)) {
    selectedValue.value = ''
    searchText.value = ''
    emit('update:modelValue', '')
  }
})

const onInput = () => {
  if (!searchText.value) {
    selectedValue.value = ''
    emit('update:modelValue', '')
  }
  showDropdown.value = true
  selectedIndex.value = -1
}

const onFocus = () => {
  if (!props.disabled) {
    showDropdown.value = true
    // Si hay un valor seleccionado, mostrarlo en el input
    if (selectedValue.value) {
      searchText.value = selectedValue.value
    }
  }
}

const onBlur = (event: FocusEvent) => {
  // Esperar un poco para permitir que el click en una opción se procese
  setTimeout(() => {
    const relatedTarget = event.relatedTarget as HTMLElement
    if (!containerRef.value?.contains(relatedTarget)) {
      closeDropdown()
      // Restaurar el valor seleccionado en el input
      if (selectedValue.value) {
        searchText.value = selectedValue.value
      } else {
        searchText.value = ''
      }
    }
  }, 200)
}

const navigateDown = () => {
  if (filteredOptions.value.length === 0) return
  selectedIndex.value = Math.min(selectedIndex.value + 1, filteredOptions.value.length - 1)
  scrollToSelected()
}

const navigateUp = () => {
  if (filteredOptions.value.length === 0) return
  selectedIndex.value = Math.max(selectedIndex.value - 1, -1)
  scrollToSelected()
}

const selectCurrent = () => {
  if (selectedIndex.value >= 0 && selectedIndex.value < filteredOptions.value.length) {
    selectOption(filteredOptions.value[selectedIndex.value])
  } else if (filteredOptions.value.length === 1) {
    // Si solo hay una opción, seleccionarla automáticamente
    selectOption(filteredOptions.value[0])
  }
}

const selectOption = (option: string) => {
  // Si se selecciona la opción por defecto, enviar cadena vacía
  const valueToEmit = option === props.defaultOption ? '' : option
  selectedValue.value = valueToEmit
  searchText.value = option === props.defaultOption ? '' : option
  emit('update:modelValue', valueToEmit)
  closeDropdown()
  inputRef.value?.blur()
}

const clearSelection = () => {
  selectedValue.value = ''
  searchText.value = ''
  emit('update:modelValue', '')
  inputRef.value?.focus()
  showDropdown.value = true
}

const closeDropdown = () => {
  showDropdown.value = false
  selectedIndex.value = -1
}

const scrollToSelected = async () => {
  await nextTick()
  if (dropdownRef.value && selectedIndex.value >= 0) {
    const options = dropdownRef.value.querySelectorAll('.dropdown-option')
    if (options[selectedIndex.value]) {
      options[selectedIndex.value].scrollIntoView({
        block: 'nearest',
        behavior: 'smooth'
      })
    }
  }
}

// Cerrar dropdown al hacer click fuera
const handleClickOutside = (event: MouseEvent) => {
  if (containerRef.value && !containerRef.value.contains(event.target as Node)) {
    closeDropdown()
  }
}

onMounted(() => {
  document.addEventListener('click', handleClickOutside)
})

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside)
})
</script>

<style scoped>
.searchable-dropdown {
  position: relative;
  width: 100%;
  flex: 1;
}

.search-field {
  display: flex;
  align-items: center;
  padding: 12px 16px;
  gap: 8px;
  flex: 1;
  color: #64748b;
  position: relative;
  width: 100%;
}

.search-field svg:first-child {
  color: #64748b;
  flex-shrink: 0;
}

.search-input {
  border: none;
  outline: none;
  background: transparent;
  color: #0f172a;
  font-size: 14px;
  width: 100%;
  padding: 0;
  padding-left: 0;
}

.search-input::placeholder {
  color: #94a3b8;
}

.search-input:disabled {
  cursor: not-allowed;
  opacity: 0.6;
}

.dropdown-arrow {
  color: #64748b;
  flex-shrink: 0;
  pointer-events: none;
}

.dropdown-options {
  position: absolute;
  top: 100%;
  left: 0;
  right: 0;
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  max-height: 300px;
  overflow-y: auto;
  z-index: 1000;
  margin-top: 4px;
}

.dropdown-option {
  padding: 12px 16px;
  cursor: pointer;
  color: #0f172a;
  font-size: 14px;
  transition: background-color 0.2s;
  border-bottom: 1px solid #f1f5f9;
}

.dropdown-option:last-child {
  border-bottom: none;
}

.dropdown-option:hover,
.dropdown-option.active {
  background-color: #f8fafc;
}

.dropdown-option.selected {
  background-color: #eff6ff;
  color: #2563eb;
  font-weight: 500;
}

.dropdown-options.sin-resultados .dropdown-option {
  color: #94a3b8;
  cursor: default;
  text-align: center;
}

/* Scrollbar personalizado */
.dropdown-options::-webkit-scrollbar {
  width: 6px;
}

.dropdown-options::-webkit-scrollbar-track {
  background: #f1f5f9;
  border-radius: 4px;
}

.dropdown-options::-webkit-scrollbar-thumb {
  background: #cbd5e1;
  border-radius: 4px;
}

.dropdown-options::-webkit-scrollbar-thumb:hover {
  background: #94a3b8;
}
</style>

