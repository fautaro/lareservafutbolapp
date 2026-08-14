<template>
  <div class="min-h-screen bg-[#F8FAFC] pb-2 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">
    
    <!-- HEADER -->
    <div class="bg-white rounded-[24px] shadow-sm sticky top-4 z-40 overflow-hidden mx-1 pb-4 border border-slate-100/50 mt-4 px-4 pt-4">
      <div class="flex items-center justify-between mb-4">
        <button @click="$router.back()" class="w-10 h-10 flex items-center justify-center bg-slate-50 border border-slate-100 rounded-full text-slate-400 hover:text-slate-600 hover:bg-slate-100 transition-all">
          <i class="fas fa-chevron-left text-sm"></i>
        </button>
        <div class="text-center">
          <h1 class="text-[11px] font-black uppercase tracking-widest text-slate-400 mb-0.5">Agenda</h1>
          <p class="text-base font-bold text-slate-900 leading-tight tracking-tight">{{ canchaNombre }}</p>
        </div>
        <button class="w-10 h-10 flex items-center justify-center bg-[#2D9CDB]/5 border border-[#2D9CDB]/20 rounded-full text-[#2D9CDB] hover:bg-[#2D9CDB]/10 transition-all">
          <i class="fas fa-ellipsis-v text-xs"></i>
        </button>
      </div>

      <!-- DATE Selector -->
      <div class="flex items-center justify-between bg-slate-50 border border-slate-100/50 rounded-2xl p-2">
        <button @click="changeDate(-1)" class="w-10 h-10 flex items-center justify-center text-slate-400 hover:text-slate-900 transition-colors bg-white rounded-xl shadow-sm border border-slate-100">
          <i class="fas fa-chevron-left text-xs"></i>
        </button>
        <div class="flex flex-col items-center">
          <span class="text-[10px] font-black text-[#2D9CDB] uppercase tracking-widest">{{ displayMonth }}</span>
          <span class="text-sm font-bold text-slate-900 capitalize">{{ displayDay }}</span>
        </div>
        <button @click="changeDate(1)" class="w-10 h-10 flex items-center justify-center text-slate-400 hover:text-slate-900 transition-colors bg-white rounded-xl shadow-sm border border-slate-100">
          <i class="fas fa-chevron-right text-xs"></i>
        </button>
      </div>
    </div>

    <!-- CALENDAR BODY -->
    <div class="relative px-4 pt-6">
      
      <!-- TIMELINE VIEW -->
      <div class="flex flex-col space-y-0.5">
        <div v-for="hour in workingHours" :key="hour" class="relative group min-h-[80px] flex gap-4">
          
          <!-- Hour Label -->
          <div class="w-12 pt-0.5 flex flex-col items-end">
            <span class="text-[11px] font-bold text-slate-400 tabular-nums">{{ hour }}:00</span>
          </div>

          <!-- Slot Container -->
          <div class="flex-1 border-t border-slate-100 relative pb-4">
            
            <!-- Turn Content -->
            <div v-if="getTurnAt(hour)" 
              class="w-full h-[calc(100%-4px)] rounded-2xl p-4 flex flex-col justify-between transition-all duration-300 shadow-sm border animate-slide-in"
              :class="getTurnAt(hour).status === 'reserved' 
                ? 'bg-[#2D9CDB] border-[#2D9CDB] text-white shadow-blue-100' 
                : 'bg-white border-slate-200 text-slate-900 hover:border-[#2D9CDB]/30'"
              @click="handleTurnClick(getTurnAt(hour))"
            >
              <div class="flex justify-between items-start">
                <div>
                  <div class="flex items-center gap-1.5 mb-1">
                    <i class="fas fa-clock text-[10px]" :class="getTurnAt(hour).status === 'reserved' ? 'text-white/70' : 'text-slate-400'"></i>
                    <span class="text-[10px] font-black uppercase tracking-widest opacity-80">
                      {{ hour }}:00 - {{ hour + 1 }}:00
                    </span>
                  </div>
                  <h4 class="text-sm font-bold tracking-tight">
                    {{ getTurnAt(hour).status === 'reserved' ? getTurnAt(hour).user : 'Espacio Disponible' }}
                  </h4>
                </div>
                
                <div v-if="getTurnAt(hour).status === 'reserved'" class="w-8 h-8 rounded-full bg-white/20 flex items-center justify-center">
                  <i class="fas fa-check text-xs"></i>
                </div>
                <div v-else class="text-[10px] font-black text-[#2D9CDB] uppercase tracking-widest">
                  Libre
                </div>
              </div>

              <!-- Footer info (subtle) -->
              <div class="flex items-center justify-between mt-2 pt-2 border-t" :class="getTurnAt(hour).status === 'reserved' ? 'border-white/10' : 'border-slate-50'">
                 <span class="text-[9px] font-bold opacity-70 uppercase tracking-wide">
                    {{ getTurnAt(hour).status === 'reserved' ? 'Pago Pendiente' : 'Tap para bloquear' }}
                 </span>
                 <i class="fas fa-chevron-right text-[10px] opacity-40"></i>
              </div>
            </div>

            <!-- Empty slot visual (if no turn defined) -->
            <div v-else class="w-full h-[calc(100%-8px)] rounded-2xl border-2 border-dotted border-slate-100 bg-slate-50/50 flex items-center justify-center text-slate-300">
               <span class="text-[10px] font-bold uppercase tracking-widest">Fuera de horario</span>
            </div>

          </div>
        </div>
      </div>
    </div>



    <!-- SIMPLE MODAL FOR INFO -->
    <transition name="fade">
      <div v-if="selectedTurn" class="fixed inset-0 z-[2000] grid place-items-center p-4">
        <div class="absolute inset-0 bg-slate-900/60 backdrop-blur-sm" @click="selectedTurn = null"></div>
        
        <div class="relative w-full max-w-sm bg-white rounded-[28px] p-8 animate-slide-up shadow-2xl">
          <div class="flex items-center justify-center mb-6">
            <div class="w-16 h-16 rounded-full flex items-center justify-center text-2xl shadow-inner" :class="selectedTurn.status === 'reserved' ? 'bg-blue-50 text-[#2D9CDB]' : 'bg-emerald-50 text-emerald-500'">
               <i :class="selectedTurn.status === 'reserved' ? 'fas fa-user-check' : 'fas fa-calendar-plus'"></i>
            </div>
          </div>

          <div class="text-center mb-6">
            <h3 class="text-xl font-extrabold text-slate-900 tracking-tight">{{ selectedTurn.status === 'reserved' ? selectedTurn.user : 'Espacio Libre' }}</h3>
            <p class="text-sm font-medium text-slate-500 mt-1 capitalize">{{ displayDay }} • {{ selectedTurn.start.split(' ')[1] }} hs</p>
          </div>

          <div class="grid grid-cols-2 gap-4 mb-8">
            <div class="bg-slate-50 p-4 rounded-2xl border border-slate-100 text-center">
              <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest block mb-1">Estado</span>
              <span class="text-sm font-bold text-slate-900 capitalize">{{ selectedTurn.status === 'reserved' ? 'Reservado' : 'Disponible' }}</span>
            </div>
            <div class="bg-slate-50 p-4 rounded-2xl border border-slate-100 text-center">
              <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest block mb-1">Duración</span>
              <span class="text-sm font-bold text-slate-900">60 minutos</span>
            </div>
          </div>

          <div class="flex flex-col gap-3">
            <button class="w-full py-3.5 bg-[#2D9CDB] text-white font-bold rounded-xl shadow-lg shadow-blue-200 active:scale-95 transition-all text-sm uppercase tracking-widest">
              {{ selectedTurn.status === 'reserved' ? 'Ver detalle reserva' : 'Reservar manualmente' }}
            </button>
            <button class="w-full py-3.5 bg-red-50 text-red-500 font-bold rounded-xl active:scale-95 transition-all text-sm uppercase tracking-widest">
              {{ selectedTurn.status === 'reserved' ? 'Cancelar Turno' : 'Bloquear Horario' }}
            </button>
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
import { ref, computed } from 'vue'

export default {
  name: 'OwnerAgenda',
  setup() {
    const canchaNombre = ref('Cancha Principal - El Fortín')
    const currentDate = ref(new Date('2026-04-10'))
    const selectedTurn = ref(null)

    // Horario de atención: 08:00 a 24:00 (00:00 del día siguiente)
    const workingHours = [8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23]

    // MOCK DATA
    const mockTurns = ref([
      {
        start: "2026-04-10 18:00",
        end: "2026-04-10 19:00",
        status: "available"
      },
      {
        start: "2026-04-10 19:00",
        end: "2026-04-10 20:00",
        status: "reserved",
        user: "Juan Pérez"
      },
      {
        start: "2026-04-10 20:00",
        end: "2026-04-10 21:00",
        status: "available"
      },
      {
        start: "2026-04-10 21:00",
        end: "2026-04-10 22:00",
        status: "reserved",
        user: "Carlos Gómez"
      }
    ])

    // Helper to get turn at specific hour
    const getTurnAt = (hour) => {
      const timeStr = `${hour.toString().padStart(2, '0')}:00`
      return mockTurns.value.find(t => t.start.includes(timeStr))
    }

    const displayMonth = computed(() => {
      return currentDate.value.toLocaleString('es-ES', { month: 'long' })
    })

    const displayDay = computed(() => {
      const options = { weekday: 'long', day: 'numeric', month: 'short' }
      return currentDate.value.toLocaleDateString('es-ES', options).replace(',', '')
    })

    const changeDate = (days) => {
      const newDate = new Date(currentDate.value)
      newDate.setDate(newDate.getDate() + days)
      currentDate.value = newDate
    }

    const handleTurnClick = (turn) => {
      selectedTurn.value = turn
    }

    return {
      canchaNombre,
      currentDate,
      workingHours,
      displayMonth,
      displayDay,
      changeDate,
      getTurnAt,
      selectedTurn,
      handleTurnClick
    }
  }
}
</script>

<style scoped>
.animate-slide-in {
  animation: slideIn 0.4s cubic-bezier(0.16, 1, 0.3, 1) both;
}

.animate-slide-up {
  animation: slideUp 0.4s cubic-bezier(0.16, 1, 0.3, 1) both;
}

.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}

@keyframes slideIn {
  from { transform: translateX(10px); opacity: 0; }
  to { transform: translateX(0); opacity: 1; }
}

@keyframes slideUp {
  from { transform: translateY(100%); }
  to { transform: translateY(0); }
}

/* Hide scrollbar but keep functionality */
.no-scrollbar::-webkit-scrollbar {
  display: none;
}
.no-scrollbar {
  -ms-overflow-style: none;
  scrollbar-width: none;
}
</style>
