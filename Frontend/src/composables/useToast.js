import { ref } from 'vue'

const toastState = ref({
  show: false,
  message: '',
  type: 'error', // 'error' | 'success' | 'info'
  duration: 4000,
  timeoutId: null
})

export const useToast = () => {
  const showToast = (message, type = 'error', duration = 4000) => {
    if (toastState.value.timeoutId) {
      clearTimeout(toastState.value.timeoutId)
    }

    toastState.value.message = message
    toastState.value.type = type
    toastState.value.show = true

    toastState.value.timeoutId = setTimeout(() => {
      toastState.value.show = false
    }, duration)
  }

  const hideToast = () => {
    if (toastState.value.timeoutId) {
      clearTimeout(toastState.value.timeoutId)
    }
    toastState.value.show = false
  }

  return {
    toastState,
    showToast,
    hideToast
  }
}
