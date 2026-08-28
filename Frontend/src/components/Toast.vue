<template>
  <transition name="toast-fade">
    <div v-if="toastState.show" class="fixed top-6 left-1/2 -translate-x-1/2 z-[3000] max-w-sm w-[90%] bg-white/95 backdrop-blur shadow-2xl border rounded-2xl p-4 flex items-start gap-3">
      <div class="w-10 h-10 rounded-full flex items-center justify-center shrink-0" :class="iconClass">
        <i :class="icon"></i>
      </div>
      <div class="flex-1 min-w-0 pt-0.5">
        <p class="text-xs font-black uppercase tracking-wider text-slate-400 mb-0.5">{{ title }}</p>
        <p class="text-sm font-bold text-slate-800 leading-snug">{{ toastState.message }}</p>
      </div>
      <button @click="hideToast" class="w-6 h-6 flex items-center justify-center rounded-full hover:bg-slate-100 text-slate-400 hover:text-slate-600 transition-colors">
        <i class="fas fa-times text-xs"></i>
      </button>
    </div>
  </transition>
</template>

<script>
import { computed } from 'vue'
import { useToast } from '../composables/useToast'

export default {
  name: 'Toast',
  setup() {
    const { toastState, hideToast } = useToast()

    const iconClass = computed(() => {
      if (toastState.value.type === 'success') return 'bg-emerald-50 text-emerald-500 border border-emerald-100'
      if (toastState.value.type === 'info') return 'bg-blue-50 text-blue-500 border border-blue-100'
      return 'bg-rose-50 text-rose-500 border border-rose-100'
    })

    const icon = computed(() => {
      if (toastState.value.type === 'success') return 'fas fa-check-circle'
      if (toastState.value.type === 'info') return 'fas fa-info-circle'
      return 'fas fa-exclamation-circle'
    })

    const title = computed(() => {
      if (toastState.value.type === 'success') return 'Éxito'
      if (toastState.value.type === 'info') return 'Info'
      return 'Alerta'
    })

    return {
      toastState,
      hideToast,
      iconClass,
      icon,
      title
    }
  }
}
</script>

<style scoped>
.toast-fade-enter-active, .toast-fade-leave-active {
  transition: all 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}
.toast-fade-enter-from {
  opacity: 0;
  transform: translate(-50%, -20px);
}
.toast-fade-leave-to {
  opacity: 0;
  transform: translate(-50%, -20px);
}
</style>
