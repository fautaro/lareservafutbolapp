<template>
  <div class="min-h-screen bg-[#F8FAFC] pb-2 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">
    
    <!-- HEADER -->
    <div class="bg-white rounded-[24px] shadow-sm sticky top-4 z-40 overflow-hidden mx-1 pb-4 border border-slate-100/50 mt-4 px-4 pt-4">
      <div class="flex items-center justify-between mb-4">
        <button @click="$router.back()" class="w-10 h-10 flex items-center justify-center bg-slate-50 border border-slate-100 rounded-full text-slate-400 hover:text-slate-600 hover:bg-slate-100 transition-all">
          <i class="fas fa-chevron-left text-sm"></i>
        </button>
        <div class="text-center w-full px-2">
          <h1 class="text-[11px] font-black uppercase tracking-widest text-slate-400 mb-0.5">Agenda</h1>
          <p class="text-base font-bold text-slate-900 leading-tight tracking-tight truncate">
             {{ agendaData ? agendaData.complejoNombre : 'Cargando...' }}
          </p>
        </div>
        <button class="w-10 h-10 flex items-center justify-center bg-[#2D9CDB]/5 border border-[#2D9CDB]/20 rounded-full text-[#2D9CDB] hover:bg-[#2D9CDB]/10 transition-all shrink-0">
          <i class="fas fa-ellipsis-v text-xs"></i>
        </button>
      </div>

      <!-- DATE Selector -->
      <div class="flex items-center justify-between bg-slate-50 border border-slate-100/50 rounded-2xl p-2 mb-3">
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

      <!-- Canchas Selector Tabs (If multiple courts) -->
      <div v-if="agendaData && agendaData.canchas.length > 1" class="flex overflow-x-auto gap-2 no-scrollbar pb-1">
        <button 
          v-for="cancha in agendaData.canchas" 
          :key="cancha.canchaId"
          @click="selectedCanchaId = cancha.canchaId"
          class="px-4 py-2 rounded-xl text-xs font-bold transition-colors whitespace-nowrap"
          :class="selectedCanchaId === cancha.canchaId ? 'bg-slate-900 text-white' : 'bg-slate-50 text-slate-500 border border-slate-100'"
        >
          {{ cancha.canchaNombre }}
        </button>
      </div>
    </div>

    <!-- STATES -->
    <div v-if="isLoading" class="flex flex-col items-center justify-center py-20">
      <i class="fas fa-circle-notch animate-spin text-3xl text-[#2D9CDB]"></i>
      <p class="mt-4 font-bold text-slate-500 text-sm">Cargando agenda...</p>
    </div>

    <div v-else-if="error" class="px-4 py-10 text-center">
      <div class="w-16 h-16 bg-red-50 text-red-500 rounded-full flex items-center justify-center mx-auto mb-4 text-2xl">
         <i class="fas fa-exclamation-triangle"></i>
      </div>
      <h3 class="font-bold text-slate-900 text-lg">{{ error }}</h3>
      <p class="text-slate-500 text-sm mt-2">Revisá tu conexión o intentá nuevamente más tarde.</p>
    </div>

    <div v-else-if="isForbidden" class="px-4 py-10 text-center">
      <div class="w-16 h-16 bg-orange-50 text-orange-500 rounded-full flex items-center justify-center mx-auto mb-4 text-2xl">
         <i class="fas fa-lock"></i>
      </div>
      <h3 class="font-bold text-slate-900 text-lg">Acceso Denegado</h3>
      <p class="text-slate-500 text-sm mt-2">No tenés permisos para ver la agenda de este complejo.</p>
    </div>

    <div v-else-if="!selectedCanchaInfo || selectedCanchaInfo.turnos.length === 0" class="px-4 py-10 text-center">
      <div class="w-16 h-16 bg-slate-100 text-slate-400 rounded-full flex items-center justify-center mx-auto mb-4 text-2xl">
         <i class="fas fa-calendar-times"></i>
      </div>
      <h3 class="font-bold text-slate-900 text-lg">Sin horarios</h3>
      <p class="text-slate-500 text-sm mt-2">La cancha no tiene horarios configurados para este día.</p>
    </div>

    <!-- CALENDAR BODY -->
    <div v-else class="relative px-4 pt-6">
      
      <!-- TIMELINE VIEW -->
      <div class="flex flex-col space-y-0.5">
        <div v-for="turno in selectedCanchaInfo.turnos" :key="turno.horaInicio" class="relative group min-h-[80px] flex gap-4">
          
          <!-- Hour Label -->
          <div class="w-12 pt-0.5 flex flex-col items-end shrink-0">
            <span class="text-[11px] font-bold text-slate-400 tabular-nums">{{ formatTime(turno.horaInicio) }}</span>
          </div>

          <!-- Slot Container -->
          <div class="flex-1 border-t border-slate-100 relative pb-4 min-w-0">
            
            <!-- Turn Content -->
            <div 
              class="w-full h-[calc(100%-4px)] rounded-2xl p-4 flex flex-col justify-between transition-all duration-300 shadow-sm border animate-slide-in"
              :class="turno.estado === 'reserved' 
                ? 'bg-[#2D9CDB] border-[#2D9CDB] text-white shadow-blue-100' 
                : 'bg-white border-slate-200 text-slate-900 hover:border-[#2D9CDB]/30'"
              @click="handleTurnClick(turno)"
            >
              <div class="flex justify-between items-start gap-2">
                <div class="min-w-0">
                  <div class="flex items-center gap-1.5 mb-1">
                    <i class="fas fa-clock text-[10px]" :class="turno.estado === 'reserved' ? 'text-white/70' : 'text-slate-400'"></i>
                    <span class="text-[10px] font-black uppercase tracking-widest opacity-80 whitespace-nowrap">
                      {{ formatTime(turno.horaInicio) }} - {{ formatTime(turno.horaFin) }}
                    </span>
                  </div>
                  <h4 class="text-sm font-bold tracking-tight truncate">
                    {{ turno.estado === 'reserved' ? turno.jugadorNombre : 'Espacio Disponible' }}
                  </h4>
                </div>
                
                <div v-if="turno.estado === 'reserved'" class="w-8 h-8 rounded-full bg-white/20 flex items-center justify-center shrink-0">
                  <i class="fas fa-check text-xs"></i>
                </div>
                <div v-else class="text-[10px] font-black text-[#2D9CDB] uppercase tracking-widest pt-1 shrink-0">
                  Libre
                </div>
              </div>

              <!-- Footer info (subtle) -->
              <div class="flex items-center justify-between mt-2 pt-2 border-t" :class="turno.estado === 'reserved' ? 'border-white/10' : 'border-slate-50'">
                 <span class="text-[9px] font-bold opacity-70 uppercase tracking-wide">
                    <template v-if="turno.estado === 'reserved'">
                        {{ turno.estadoPago === 'Pagado' ? 'Pagado' : 'Pago Pendiente' }}
                        <span v-if="turno.montoTotal" class="ml-1">${{ turno.montoTotal }}</span>
                    </template>
                    <template v-else>Tap para bloquear</template>
                 </span>
                 <i class="fas fa-chevron-right text-[10px] opacity-40"></i>
              </div>
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
            <div class="w-16 h-16 rounded-full flex items-center justify-center text-2xl shadow-inner" :class="selectedTurn.estado === 'reserved' ? 'bg-blue-50 text-[#2D9CDB]' : 'bg-emerald-50 text-emerald-500'">
               <i :class="selectedTurn.estado === 'reserved' ? 'fas fa-user-check' : 'fas fa-calendar-plus'"></i>
            </div>
          </div>

          <div class="text-center mb-6">
            <h3 class="text-xl font-extrabold text-slate-900 tracking-tight">{{ selectedTurn.estado === 'reserved' ? selectedTurn.jugadorNombre : 'Espacio Libre' }}</h3>
            <p class="text-sm font-medium text-slate-500 mt-1 capitalize">{{ displayDay }} • {{ formatTime(selectedTurn.horaInicio) }} hs</p>
          </div>

          <div class="grid grid-cols-2 gap-4 mb-8">
            <div class="bg-slate-50 p-4 rounded-2xl border border-slate-100 text-center flex flex-col justify-center">
              <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest block mb-1">Estado</span>
              <span class="text-sm font-bold text-slate-900 capitalize leading-tight">{{ selectedTurn.estado === 'reserved' ? 'Reservado' : 'Disponible' }}</span>
            </div>
            <div class="bg-slate-50 p-4 rounded-2xl border border-slate-100 text-center flex flex-col justify-center">
              <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest block mb-1">Total</span>
              <span class="text-sm font-bold text-slate-900 leading-tight">{{ selectedTurn.montoTotal ? `$${selectedTurn.montoTotal}` : '-' }}</span>
            </div>
          </div>

          <div class="flex flex-col gap-3">
            <button class="w-full py-3.5 bg-[#2D9CDB] text-white font-bold rounded-xl shadow-lg shadow-blue-200 active:scale-95 transition-all text-sm uppercase tracking-widest">
              {{ selectedTurn.estado === 'reserved' ? 'Ver detalle reserva' : 'Reservar manualmente' }}
            </button>
            <button class="w-full py-3.5 bg-red-50 text-red-500 font-bold rounded-xl active:scale-95 transition-all text-sm uppercase tracking-widest">
              {{ selectedTurn.estado === 'reserved' ? 'Cancelar Turno' : 'Bloquear Horario' }}
            </button>
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuthUser } from '../../composables/useAuthUser'
import { API_ENDPOINTS } from '../../config/apiConfig'

export default {
  name: 'OwnerAgenda',
  setup() {
    const route = useRoute()
    const router = useRouter()
    const { user } = useAuthUser()

    const complejoId = route.params.id

    const currentDate = ref(new Date())
    const selectedTurn = ref(null)
    const selectedCanchaId = ref(null)

    const agendaData = ref(null)
    const isLoading = ref(true)
    const error = ref(null)
    const isForbidden = ref(false)

    const fetchAgenda = async () => {
      isLoading.value = true
      error.value = null
      isForbidden.value = false
      agendaData.value = null

      try {
        // Obtenemos el ID del usuario simulado (o 1 por defecto)
        const currentUserId = user.value?.sub || 1

        // Format Date to YYYY-MM-DD local time
        const dateStr = currentDate.value.toLocaleDateString('en-CA')
        
        const response = await fetch(API_ENDPOINTS.complejos.getAgenda(complejoId, dateStr, currentUserId))

        if (response.status === 403 || response.status === 401) {
            isForbidden.value = true
            return
        }

        if (!response.ok) {
            throw new Error(response.status === 404 ? 'Complejo no encontrado' : 'Error del servidor')
        }

        const data = await response.json()
        agendaData.value = data

        if (data.canchas && data.canchas.length > 0) {
            // Keep selected cancha if still valid, otherwise select the first one
            if (!selectedCanchaId.value || !data.canchas.find(c => c.canchaId === selectedCanchaId.value)) {
                selectedCanchaId.value = data.canchas[0].canchaId
            }
        }
      } catch (err) {
        console.error(err)
        error.value = err.message || 'Error al obtener la agenda'
      } finally {
        isLoading.value = false
      }
    }

    onMounted(() => {
        fetchAgenda()
    })

    watch(currentDate, () => {
        fetchAgenda()
    })

    const selectedCanchaInfo = computed(() => {
        if (!agendaData.value || !selectedCanchaId.value) return null
        return agendaData.value.canchas.find(c => c.canchaId === selectedCanchaId.value)
    })

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

    const formatTime = (timeStr) => {
        // timeStr usually comes as "HH:MM:SS"
        if (!timeStr) return ''
        return timeStr.substring(0, 5)
    }

    return {
      agendaData,
      selectedCanchaId,
      selectedCanchaInfo,
      isLoading,
      error,
      isForbidden,
      currentDate,
      displayMonth,
      displayDay,
      changeDate,
      selectedTurn,
      handleTurnClick,
      formatTime
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
