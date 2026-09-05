<template>
  <transition name="toast-slide">
    <div
      v-if="toastState.show"
      class="fixed bottom-24 inset-x-0 flex justify-center z-[3500] px-4 pointer-events-none"
    >
      <div
        class="pointer-events-auto flex items-center gap-3 px-5 py-3.5 bg-slate-900 text-white rounded-2xl shadow-2xl border border-slate-800 max-w-sm sm:max-w-md animate-slide-up"
        role="alert"
      >
        <div class="flex items-center justify-center shrink-0">
          <i :class="[iconClass, iconColorClass]" class="text-base"></i>
        </div>
        <span class="text-xs sm:text-sm font-bold text-white leading-tight">
          {{ toastState.message }}
        </span>
        <button
          type="button"
          @click="hideToast"
          class="ml-1.5 text-slate-400 hover:text-white transition-colors text-xs p-1 -mr-1"
          title="Cerrar"
        >
          <i class="fas fa-times"></i>
        </button>
      </div>
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
      const msg = (toastState.value.message || '').toLowerCase()
      if (msg.includes('baja')) return 'fas fa-user-minus'
      if (msg.includes('reactiv')) return 'fas fa-rotate-left'
      if (toastState.value.type === 'success') return 'fas fa-check-circle'
      if (toastState.value.type === 'info') return 'fas fa-info-circle'
      return 'fas fa-exclamation-circle'
    })

    const iconColorClass = computed(() => {
      const msg = (toastState.value.message || '').toLowerCase()
      if (msg.includes('baja')) return 'text-rose-400'
      if (toastState.value.type === 'success') return 'text-green-400'
      if (toastState.value.type === 'info') return 'text-sky-400'
      return 'text-rose-400'
    })

    return {
      toastState,
      hideToast,
      iconClass,
      iconColorClass
    }
  }
}
</script>

<style scoped>
.toast-slide-enter-active,
.toast-slide-leave-active {
  transition: all 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

.toast-slide-enter-from {
  opacity: 0;
  transform: translateY(16px) scale(0.95);
}

.toast-slide-leave-to {
  opacity: 0;
  transform: translateY(16px) scale(0.95);
}

@keyframes slide-up {
  from {
    transform: translateY(16px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.animate-slide-up {
  animation: slide-up 0.3s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}
</style>
