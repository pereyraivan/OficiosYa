<template>
  <div ref="wizardRootRef" class="wizard">
    <!-- Indicador de pasos -->
    <div class="wizard-steps">
      <div 
        v-for="(step, index) in steps" 
        :key="index"
        class="step-indicator"
        :class="{ 
          'active': currentStep === index + 1,
          'completed': currentStep > index + 1
        }"
      >
        <div class="step-number">
          <span v-if="currentStep > index + 1">✓</span>
          <span v-else>{{ index + 1 }}</span>
        </div>
        <div class="step-label">{{ step.label }}</div>
      </div>
    </div>

    <!-- Contenido del paso actual -->
    <div class="wizard-content">
      <slot :name="`step-${currentStep}`" :step="currentStep" :next="nextStep" :prev="prevStep" :goTo="goToStep" />
    </div>

    <!-- Navegación -->
    <div class="wizard-navigation">
      <button
        v-if="currentStep > 1"
        type="button"
        class="btn-secondary"
        @click="prevStep"
      >
        ← Anterior
      </button>
      <div class="spacer"></div>
      <button
        v-if="currentStep < totalSteps"
        type="button"
        class="btn-primary"
        @click="nextStep"
        :disabled="!canProceed"
      >
        Siguiente →
      </button>
      <button
        v-else
        type="button"
        class="btn-primary"
        @click="onFinishClick"
        :disabled="!canProceed"
      >
        Finalizar
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, nextTick } from 'vue'

interface Step {
  label: string
}

interface Props {
  steps: Step[]
  canProceed?: boolean
  /** Si está definida, se llama al hacer clic en Siguiente/Finalizar. Si retorna false (o Promise<false>), no se avanza. */
  validateBeforeNext?: (step: number) => boolean | Promise<boolean>
}

const props = withDefaults(defineProps<Props>(), {
  canProceed: true
})

const emit = defineEmits<{
  finish: []
  stepChange: [step: number]
}>()

const currentStep = ref(1)
const totalSteps = computed(() => props.steps.length)
const wizardRootRef = ref<HTMLElement | null>(null)

function scrollToStepTop () {
  nextTick(() => {
    if (wizardRootRef.value) {
      wizardRootRef.value.scrollIntoView({ behavior: 'smooth', block: 'start' })
    } else {
      window.scrollTo({ top: 0, behavior: 'smooth' })
    }
  })
}

const nextStep = async () => {
  if (currentStep.value >= totalSteps.value) return
  if (props.validateBeforeNext) {
    const ok = await Promise.resolve(props.validateBeforeNext(currentStep.value))
    if (!ok) return
  }
  currentStep.value++
  emit('stepChange', currentStep.value)
  scrollToStepTop()
}

const onFinishClick = async () => {
  if (props.validateBeforeNext) {
    const ok = await Promise.resolve(props.validateBeforeNext(currentStep.value))
    if (!ok) return
  }
  emit('finish')
}

const prevStep = () => {
  if (currentStep.value > 1) {
    currentStep.value--
    emit('stepChange', currentStep.value)
    scrollToStepTop()
  }
}

const goToStep = (step: number) => {
  if (step >= 1 && step <= totalSteps.value) {
    currentStep.value = step
    emit('stepChange', currentStep.value)
    scrollToStepTop()
  }
}

defineExpose({
  currentStep,
  nextStep,
  prevStep,
  goToStep
})
</script>

<style scoped>
.wizard {
  width: 100%;
}

.wizard-steps {
  display: flex;
  justify-content: space-between;
  margin-bottom: 40px;
  position: relative;
  padding: 0 20px;
}

.wizard-steps::before {
  content: '';
  position: absolute;
  top: 20px;
  left: 10%;
  right: 10%;
  height: 2px;
  background: #e2e8f0;
  z-index: 0;
}

.step-indicator {
  display: flex;
  flex-direction: column;
  align-items: center;
  position: relative;
  z-index: 1;
  flex: 1;
}

.step-number {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: white;
  border: 2px solid #e2e8f0;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
  color: #94a3b8;
  transition: all 0.3s ease;
  margin-bottom: 8px;
}

.step-indicator.active .step-number {
  background: var(--brand);
  border-color: var(--brand);
  color: white;
  transform: scale(1.1);
}

.step-indicator.completed .step-number {
  background: #22c55e;
  border-color: #22c55e;
  color: white;
}

.step-label {
  font-size: 0.85rem;
  color: #64748b;
  text-align: center;
  font-weight: 500;
}

.step-indicator.active .step-label {
  color: var(--brand);
  font-weight: 600;
}

.step-indicator.completed .step-label {
  color: #22c55e;
}

.wizard-content {
  min-height: 400px;
  margin-bottom: 30px;
}

.wizard-navigation {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-top: 30px;
  border-top: 1px solid #e2e8f0;
}

.spacer {
  flex: 1;
}

.btn-primary,
.btn-secondary {
  padding: 12px 24px;
  border-radius: 8px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
  border: none;
  font-size: 14px;
}

.btn-primary {
  background: var(--brand);
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: var(--brand-600);
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(59, 130, 246, 0.3);
}

.btn-primary:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-secondary {
  background: white;
  color: var(--text);
  border: 2px solid #e2e8f0;
}

.btn-secondary:hover {
  border-color: #cbd5e1;
  background: #f8fafc;
}

/* Responsive */
@media (max-width: 768px) {
  .wizard-steps {
    padding: 0 10px;
  }

  .step-number {
    width: 32px;
    height: 32px;
    font-size: 0.9rem;
  }

  .step-label {
    font-size: 0.75rem;
  }

  .wizard-content {
    min-height: 300px;
  }

  .btn-primary,
  .btn-secondary {
    padding: 10px 16px;
    font-size: 13px;
  }
}
</style>

