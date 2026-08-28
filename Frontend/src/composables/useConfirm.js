import { ref } from 'vue'

const confirmState = ref({
  show: false,
  title: 'Confirmación',
  message: '',
  confirmText: 'Aceptar',
  cancelText: 'Cancelar',
  type: 'info', // 'danger' | 'warning' | 'info'
  resolve: null
})

export const useConfirm = () => {
  const requireConfirm = (options = {}) => {
    return new Promise((resolve) => {
      confirmState.value = {
        show: true,
        title: options.title || '¿Estás seguro?',
        message: options.message || '',
        confirmText: options.confirmText || 'Confirmar',
        cancelText: options.cancelText || 'Cancelar',
        type: options.type || 'info',
        resolve
      }
    })
  }

  const handleConfirm = () => {
    if (confirmState.value.resolve) {
      confirmState.value.resolve(true)
    }
    confirmState.value.show = false
  }

  const handleCancel = () => {
    if (confirmState.value.resolve) {
      confirmState.value.resolve(false)
    }
    confirmState.value.show = false
  }

  return {
    confirmState,
    requireConfirm,
    handleConfirm,
    handleCancel
  }
}
