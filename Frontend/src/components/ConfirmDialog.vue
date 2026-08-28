<template>
  <transition name="modal-fade">
    <div v-if="confirmState.show" class="fixed inset-0 z-[9999] grid place-items-center p-4">
      <!-- Backdrop overlay -->
      <div class="absolute inset-0 bg-slate-900/60 backdrop-blur-sm transition-opacity" @click="handleCancel"></div>
      
      <!-- Modal Content Card -->
      <div 
        class="relative w-full max-w-sm rounded-[28px] bg-white p-8 shadow-2xl animate-slide-up text-center border border-slate-100"
        style="font-family: Inter, 'Noto Sans', sans-serif;"
      >
        <!-- Top Icon -->
        <div class="w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-5 text-2xl shadow-inner transition-transform duration-300" :class="iconBgClass">
          <i :class="iconClass"></i>
        </div>
        
        <!-- Header & message -->
        <h3 class="text-xl font-black text-center text-slate-900 tracking-tight mb-2">{{ confirmState.title }}</h3>
        <p class="text-xs text-slate-500 text-center mt-2 px-2 leading-relaxed opacity-80 whitespace-pre-line">{{ confirmState.message }}</p>
        
        <!-- Action Buttons -->
        <div class="mt-7 flex flex-col gap-2.5">
          <button 
            type="button" 
            @click="handleConfirm" 
            class="w-full py-3.5 text-white font-bold rounded-xl shadow-lg active:scale-[0.98] transition-all text-sm uppercase tracking-wider flex items-center justify-center gap-2"
            :class="confirmBtnClass"
          >
            {{ confirmState.confirmText }}
          </button>
          <button 
            type="button" 
            @click="handleCancel" 
            class="w-full py-2.5 text-xs font-bold text-slate-400 hover:text-slate-600 uppercase tracking-widest transition-colors text-center"
          >
            {{ confirmState.cancelText }}
          </button>
        </div>
      </div>
    </div>
  </transition>
</template>

<script>
import { computed } from 'vue'
import { useConfirm } from '../composables/useConfirm'

export default {
  name: 'ConfirmDialog',
  setup() {
    const { confirmState, handleConfirm, handleCancel } = useConfirm()

    const iconBgClass = computed(() => {
      if (confirmState.value.type === 'danger') return 'bg-rose-50 text-rose-500 border border-rose-100'
      if (confirmState.value.type === 'warning') return 'bg-slate-100 text-slate-600 border border-slate-200'
      return 'bg-blue-50 text-[#2D9CDB] border border-blue-100'
    })

    const iconClass = computed(() => {
      if (confirmState.value.type === 'danger') return 'fas fa-trash-alt'
      if (confirmState.value.type === 'warning') return 'fas fa-eye-slash'
      return 'fas fa-info-circle'
    })

    const confirmBtnClass = computed(() => {
      if (confirmState.value.type === 'danger') {
        return 'bg-rose-500 hover:bg-rose-600 shadow-rose-500/10'
      }
      if (confirmState.value.type === 'warning') {
        return 'bg-slate-800 hover:bg-slate-900 shadow-slate-800/10'
      }
      return 'bg-[#2D9CDB] hover:bg-[#2088c2] shadow-[#2D9CDB]/10'
    })

    return {
      confirmState,
      handleConfirm,
      handleCancel,
      iconBgClass,
      iconClass,
      confirmBtnClass
    }
  }
}
</script>

<style scoped>
.modal-fade-enter-active, .modal-fade-leave-active {
  transition: opacity 0.25s ease;
}
.modal-fade-enter-from, .modal-fade-leave-to {
  opacity: 0;
}

@keyframes slide-up {
  from {
    transform: translateY(20px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.animate-slide-up {
  animation: slide-up 0.4s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}
</style>
