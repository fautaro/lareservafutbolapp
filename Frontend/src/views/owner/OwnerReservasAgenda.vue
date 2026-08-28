<template>
  <div class="min-h-screen bg-[#F8FAFC] pb-24 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">
    
    <!-- HEADER -->
    <div class="bg-white rounded-[24px] shadow-sm sticky top-4 z-40 overflow-hidden mx-1 pb-4 border border-slate-100/50 mt-4 px-4 pt-4">
      <div class="flex items-center justify-between mb-4">
        <div class="w-10 h-10 flex items-center justify-center bg-[#2D9CDB]/10 text-[#2D9CDB] rounded-full shrink-0">
          <i class="fas fa-calendar-alt text-sm"></i>
        </div>
        <div class="text-center w-full px-2">
          <h1 class="text-[11px] font-black uppercase tracking-widest text-slate-400 mb-0.5">Modo Dueño</h1>
          <p class="text-base font-bold text-slate-900 leading-tight tracking-tight truncate">
            Agenda de Reservas
          </p>
        </div>
        <button 
          @click="openStatsModal" 
          class="w-10 h-10 flex items-center justify-center bg-[#2D9CDB]/5 border border-[#2D9CDB]/20 rounded-full text-[#2D9CDB] hover:bg-[#2D9CDB]/10 active:scale-95 transition-all shrink-0 cursor-pointer"
          title="Ver estadísticas del día"
        >
          <i class="fas fa-chart-line text-xs"></i>
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

      <!-- Complejos Selector Filter -->
      <div v-if="complejosList.length > 0" class="flex overflow-x-auto gap-2 no-scrollbar pb-2">
        <button 
          @click="selectComplejo('all')"
          class="px-3.5 py-1.5 rounded-xl text-xs font-bold transition-all whitespace-nowrap flex items-center gap-1.5"
          :class="selectedComplejoId === 'all' ? 'bg-[#2D9CDB] text-white shadow-md shadow-[#2D9CDB]/20' : 'bg-slate-50 text-slate-500 border border-slate-100 hover:bg-slate-100'"
        >
          <i class="fas fa-layer-group text-[10px]"></i>
          Todos ({{ complejosList.length }})
        </button>
        <button 
          v-for="complejo in complejosList" 
          :key="complejo.id"
          @click="selectComplejo(complejo.id)"
          class="px-3.5 py-1.5 rounded-xl text-xs font-bold transition-all whitespace-nowrap flex items-center gap-1.5"
          :class="selectedComplejoId === complejo.id ? 'bg-[#2D9CDB] text-white shadow-md shadow-[#2D9CDB]/20' : 'bg-slate-50 text-slate-500 border border-slate-100 hover:bg-slate-100'"
        >
          <i class="fas fa-building text-[10px]"></i>
          {{ complejo.nombre }}
        </button>
      </div>

      <!-- Canchas Selector Tabs (Filtered by selected complejo) -->
      <div v-if="availableCanchas.length > 1" class="flex overflow-x-auto gap-2 no-scrollbar pt-1 border-t border-slate-100">
        <button 
          @click="selectedCanchaId = 'all'"
          class="px-3 py-1.5 rounded-xl text-[11px] font-bold transition-colors whitespace-nowrap"
          :class="selectedCanchaId === 'all' ? 'bg-slate-900 text-white' : 'bg-slate-50 text-slate-500 border border-slate-100 hover:bg-slate-100'"
        >
          Todas las canchas ({{ availableCanchas.length }})
        </button>
        <button 
          v-for="cancha in availableCanchas" 
          :key="cancha.canchaId"
          @click="selectedCanchaId = cancha.canchaId"
          class="px-3 py-1.5 rounded-xl text-[11px] font-bold transition-colors whitespace-nowrap"
          :class="selectedCanchaId === cancha.canchaId ? 'bg-slate-900 text-white' : 'bg-slate-50 text-slate-500 border border-slate-100 hover:bg-slate-100'"
        >
          {{ selectedComplejoId === 'all' ? `${cancha.complejoNombre} - ${cancha.canchaNombre}` : cancha.canchaNombre }}
        </button>
      </div>
    </div>

    <!-- STATES -->
    <div v-if="isLoading" class="flex flex-col items-center justify-center py-20">
      <i class="fas fa-circle-notch animate-spin text-3xl text-[#2D9CDB]"></i>
      <p class="mt-4 font-bold text-slate-500 text-sm">Cargando agenda de complejos...</p>
    </div>

    <div v-else-if="error" class="px-4 py-10 text-center">
      <div class="w-16 h-16 bg-red-50 text-red-500 rounded-full flex items-center justify-center mx-auto mb-4 text-2xl">
         <i class="fas fa-exclamation-triangle"></i>
      </div>
      <h3 class="font-bold text-slate-900 text-lg">{{ error }}</h3>
      <p class="text-slate-500 text-sm mt-2">Revisá tu conexión o intentá nuevamente más tarde.</p>
    </div>

    <div v-else-if="complejosList.length === 0" class="px-4 py-16 text-center">
      <div class="w-20 h-20 bg-slate-100 text-slate-400 rounded-full flex items-center justify-center mx-auto mb-4 text-3xl">
         <i class="fas fa-building"></i>
      </div>
      <h3 class="font-bold text-slate-900 text-lg">No tenés complejos registrados</h3>
      <p class="text-slate-500 text-sm mt-2">Agregá tu primer complejo para gestionar sus reservas y turnos.</p>
      <button @click="$router.push({ name: 'OwnerComplejoCreate' })" class="mt-6 px-6 py-3 bg-[#2D9CDB] text-white rounded-2xl font-bold text-xs uppercase tracking-widest shadow-xl shadow-[#2D9CDB]/20 transition-all active:scale-95">
        Crear Complejo
      </button>
    </div>

    <div v-else-if="displayedCanchas.length === 0" class="px-4 py-10 text-center">
      <div class="w-16 h-16 bg-slate-100 text-slate-400 rounded-full flex items-center justify-center mx-auto mb-4 text-2xl">
         <i class="fas fa-calendar-times"></i>
      </div>
      <h3 class="font-bold text-slate-900 text-lg">Sin canchas u horarios</h3>
      <p class="text-slate-500 text-sm mt-2">No se encontraron canchas ni horarios configurados para el filtro seleccionado.</p>
    </div>

    <!-- CALENDAR BODY -->
    <div v-else class="relative px-4 pt-4 space-y-6">
      
      <!-- CANCHA SECTIONS -->
      <div v-for="cancha in displayedCanchas" :key="cancha.canchaId" class="bg-white rounded-3xl p-5 border border-slate-100 shadow-sm animate-slide-up">
        
        <!-- Cancha Title Header -->
        <div class="flex items-center justify-between pb-4 border-b border-slate-100 mb-4">
          <div class="flex items-center gap-3 min-w-0">
            <div class="w-9 h-9 rounded-xl bg-[#2D9CDB]/10 text-[#2D9CDB] flex items-center justify-center shrink-0 border border-[#2D9CDB]/20">
              <i class="fas fa-futbol text-xs"></i>
            </div>
            <div class="min-w-0">
              <span v-if="selectedComplejoId === 'all'" class="text-[10px] font-black uppercase tracking-wider text-slate-400 block truncate">
                {{ cancha.complejoNombre }}
              </span>
              <h3 class="text-base font-extrabold text-slate-900 truncate tracking-tight">
                {{ cancha.canchaNombre }}
              </h3>
              <span v-if="cancha.deporteNombre" class="text-[10px] font-bold text-[#2D9CDB] uppercase tracking-widest">
                {{ cancha.deporteNombre }}
              </span>
            </div>
          </div>

          <div class="text-right shrink-0">
            <span class="text-[10px] font-bold px-2.5 py-1 rounded-lg bg-slate-50 text-slate-600 border border-slate-100">
              {{ cancha.turnos ? cancha.turnos.filter(t => t.estado === 'reserved').length : 0 }} reservado(s)
            </span>
          </div>
        </div>

        <!-- TIMELINE VIEW -->
        <div v-if="cancha.turnos && cancha.turnos.length > 0" class="flex flex-col space-y-0.5">
          <div v-for="turno in cancha.turnos" :key="turno.horaInicio" class="relative group min-h-[75px] flex gap-3 sm:gap-4">
            
            <!-- Hour Label -->
            <div class="w-11 sm:w-12 pt-0.5 flex flex-col items-end shrink-0">
              <span class="text-[11px] font-bold text-slate-400 tabular-nums">{{ formatTime(turno.horaInicio) }}</span>
            </div>

            <!-- Slot Container -->
            <div class="flex-1 border-t border-slate-100 relative pb-3 min-w-0">
              
              <!-- Turn Content Card -->
              <div 
                class="w-full h-[calc(100%-4px)] rounded-2xl p-3.5 sm:p-4 flex flex-col justify-between transition-all duration-300 shadow-sm border cursor-pointer hover:scale-[1.01]"
                :class="turno.estado === 'reserved' 
                  ? 'bg-[#2D9CDB] border-[#2D9CDB] text-white shadow-blue-100' 
                  : turno.estado === 'blocked' 
                  ? 'bg-slate-100 border-slate-200 text-slate-500' 
                  : 'bg-white border-slate-200 text-slate-900 hover:border-[#2D9CDB]/40'"
                @click="handleTurnClick(turno, cancha)"
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
                      {{ turno.estado === 'reserved' ? turno.jugadorNombre : turno.estado === 'blocked' ? 'Horario Bloqueado' : 'Espacio Disponible' }}
                    </h4>
                  </div>
                  
                  <div v-if="turno.estado === 'reserved'" class="w-7 h-7 sm:w-8 sm:h-8 rounded-full bg-white/20 flex items-center justify-center shrink-0">
                    <i class="fas fa-check text-xs"></i>
                  </div>
                  <div v-else-if="turno.estado === 'blocked'" class="w-7 h-7 sm:w-8 sm:h-8 rounded-full bg-slate-200 flex items-center justify-center shrink-0">
                    <i class="fas fa-ban text-xs text-slate-400"></i>
                  </div>
                  <div v-else class="text-[10px] font-black text-[#2D9CDB] uppercase tracking-widest pt-1 shrink-0">
                    Libre
                  </div>
                </div>

                <!-- Footer info -->
                <div class="flex items-center justify-between mt-2 pt-2 border-t" :class="turno.estado === 'reserved' ? 'border-white/10' : 'border-slate-50'">
                   <span class="text-[9px] font-bold opacity-70 uppercase tracking-wide">
                      <template v-if="turno.estado === 'reserved'">
                          {{ (turno.estadoPago === 'Pagado' || turno.estadoPago === 'pagado') ? 'Pagado' : 'Pago Pendiente' }}
                          <span v-if="turno.montoTotal" class="ml-1">${{ formatMoney(turno.montoTotal) }}</span>
                      </template>
                      <template v-else-if="turno.estado === 'blocked'">Tap para desbloquear</template>
                      <template v-else>Tap para bloquear</template>
                   </span>
                   <i class="fas fa-chevron-right text-[10px] opacity-40"></i>
                </div>
              </div>

            </div>
          </div>
        </div>

        <div v-else class="py-6 text-center text-slate-400 text-xs">
          Sin horarios disponibles para este día.
        </div>

      </div>
    </div>

    <!-- MODAL FOR TURN -->
    <transition name="fade">
      <div v-if="selectedTurn" class="fixed inset-0 z-[2000] grid place-items-center p-4">
        <div class="absolute inset-0 bg-slate-900/60 backdrop-blur-sm" @click="closeModal"></div>
        
        <div class="relative w-full max-w-sm bg-white rounded-[28px] shadow-2xl overflow-hidden">

          <!-- ========== ALERTS (Top Banner) ========== -->
          <div v-if="errorMessage" class="bg-red-50 p-4 border-b border-red-100 flex items-start gap-3">
             <i class="fas fa-exclamation-circle text-red-500 mt-0.5"></i>
             <p class="text-sm font-bold text-red-600 leading-tight">{{ errorMessage }}</p>
          </div>
          <div v-if="successMessage" class="bg-emerald-50 p-4 border-b border-emerald-100 flex items-start gap-3">
             <i class="fas fa-check-circle text-emerald-500 mt-0.5"></i>
             <p class="text-sm font-bold text-emerald-600 leading-tight">{{ successMessage }}</p>
          </div>

          <!-- ========== SUMMARY VIEW ========== -->
          <div v-if="modalView === 'summary'" class="p-8 animate-slide-up">
            <div class="flex items-center justify-center mb-6">
              <div class="w-16 h-16 rounded-full flex items-center justify-center text-2xl shadow-inner" :class="selectedTurn.estado === 'reserved' ? 'bg-blue-50 text-[#2D9CDB]' : selectedTurn.estado === 'blocked' ? 'bg-slate-100 text-slate-400' : 'bg-emerald-50 text-emerald-500'">
                 <i :class="selectedTurn.estado === 'reserved' ? 'fas fa-user-check' : selectedTurn.estado === 'blocked' ? 'fas fa-ban' : 'fas fa-calendar-plus'"></i>
              </div>
            </div>

            <div class="text-center mb-6">
              <span v-if="selectedTurnContext?.complejoNombre" class="text-[10px] font-black text-slate-400 uppercase tracking-widest block mb-1">
                {{ selectedTurnContext.complejoNombre }} • {{ selectedTurnContext.canchaNombre }}
              </span>
              <h3 class="text-xl font-extrabold text-slate-900 tracking-tight">{{ selectedTurn.estado === 'reserved' ? selectedTurn.jugadorNombre : selectedTurn.estado === 'blocked' ? 'Horario Bloqueado' : 'Espacio Libre' }}</h3>
              <p class="text-sm font-medium text-slate-500 mt-1 capitalize">{{ displayDay }} • {{ formatTime(selectedTurn.horaInicio) }} hs</p>
            </div>

            <div class="grid grid-cols-2 gap-4 mb-8">
              <div class="bg-slate-50 p-4 rounded-2xl border border-slate-100 text-center flex flex-col justify-center">
                <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest block mb-1">Estado</span>
                <span class="text-sm font-bold text-slate-900 capitalize leading-tight">{{ selectedTurn.estado === 'reserved' ? 'Reservado' : selectedTurn.estado === 'blocked' ? 'Bloqueado' : 'Disponible' }}</span>
              </div>
              <div class="bg-slate-50 p-4 rounded-2xl border border-slate-100 text-center flex flex-col justify-center">
                <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest block mb-1">Total</span>
                <span class="text-sm font-bold text-slate-900 leading-tight">{{ selectedTurn.montoTotal ? `$${formatMoney(selectedTurn.montoTotal)}` : '-' }}</span>
              </div>
            </div>

            <!-- Botones para turno RESERVADO -->
            <div v-if="selectedTurn.estado === 'reserved'" class="flex flex-col gap-3">
              <button @click="openReservaDetail" class="w-full py-3.5 bg-[#2D9CDB] text-white font-bold rounded-xl shadow-lg shadow-blue-200 active:scale-95 transition-all text-sm uppercase tracking-widest">
                Ver detalle reserva
              </button>
              <button @click="initCancel" class="w-full py-3.5 bg-red-50 text-red-500 font-bold rounded-xl active:scale-95 transition-all text-sm uppercase tracking-widest">
                Cancelar Turno
              </button>
            </div>

            <!-- Botones para turno BLOQUEADO -->
            <div v-else-if="selectedTurn.estado === 'blocked'" class="flex flex-col gap-3">
              <button @click="initCancel" class="w-full py-3.5 bg-slate-100 text-slate-700 font-bold rounded-xl active:scale-95 transition-all text-sm uppercase tracking-widest">
                Desbloquear Horario
              </button>
            </div>

            <!-- Botones para turno DISPONIBLE -->
            <div v-else class="flex flex-col gap-3">
              <button @click="executeBlock" class="w-full py-3.5 bg-red-50 text-red-500 font-bold rounded-xl active:scale-95 transition-all text-sm uppercase tracking-widest flex justify-center items-center gap-2" :disabled="isBlocking">
                <i v-if="isBlocking" class="fas fa-circle-notch animate-spin"></i>
                <span>{{ isBlocking ? 'Bloqueando...' : 'Bloquear Horario' }}</span>
              </button>
            </div>
          </div>

          <!-- ========== CONFIRM CANCEL VIEW ========== -->
          <div v-else-if="modalView === 'confirm-cancel'" class="p-8 animate-slide-in text-center">
            <div class="w-20 h-20 bg-red-50 text-red-500 rounded-full flex items-center justify-center mx-auto mb-6 shadow-inner text-3xl">
              <i class="fas fa-exclamation-triangle"></i>
            </div>
            <h3 class="text-xl font-extrabold text-slate-900 tracking-tight mb-2">{{ selectedTurn.estado === 'blocked' ? '¿Desbloquear horario?' : '¿Cancelar turno?' }}</h3>
            <p v-if="selectedTurn.estado === 'blocked'" class="text-sm text-slate-500 mb-8 leading-relaxed">
               Estás a punto de desbloquear este horario para las <strong class="text-slate-700">{{ formatTime(selectedTurn.horaInicio) }} hs</strong>.<br/><br/>Volverá a estar disponible para reservas.
            </p>
            <p v-else class="text-sm text-slate-500 mb-8 leading-relaxed">
               Estás a punto de cancelar la reserva de <strong class="text-slate-700">{{ selectedTurn.jugadorNombre }}</strong> para las <strong class="text-slate-700">{{ formatTime(selectedTurn.horaInicio) }} hs</strong>.<br/><br/>Esta acción no se puede deshacer.
            </p>
            
            <div class="flex flex-col gap-3">
              <button @click="executeCancel" :disabled="isCancelling" class="w-full py-3.5 bg-red-500 text-white font-bold rounded-xl shadow-lg shadow-red-200 active:scale-95 transition-all text-sm uppercase tracking-widest disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2">
                <i v-if="isCancelling" class="fas fa-circle-notch animate-spin"></i>
                <span>{{ isCancelling ? (selectedTurn.estado === 'blocked' ? 'Desbloqueando...' : 'Cancelando...') : (selectedTurn.estado === 'blocked' ? 'Sí, desbloquear' : 'Sí, cancelar turno') }}</span>
              </button>
              <button @click="modalView = 'summary'" :disabled="isCancelling" class="w-full py-3.5 bg-slate-100 text-slate-700 font-bold rounded-xl active:scale-95 transition-all text-sm uppercase tracking-widest disabled:opacity-50 disabled:cursor-not-allowed">
                No, volver
              </button>
            </div>
          </div>

          <!-- ========== DETAIL VIEW (Reserva) ========== -->
          <div v-else-if="modalView === 'detail'" class="animate-slide-in">
            <!-- Header -->
            <div class="bg-[#2D9CDB] px-6 pt-6 pb-8 text-white">
              <button @click="modalView = 'summary'" class="w-9 h-9 rounded-full bg-white/20 flex items-center justify-center mb-4 active:scale-90 transition-transform">
                <i class="fas fa-arrow-left text-sm"></i>
              </button>
              <h3 class="text-lg font-extrabold tracking-tight">Detalle de Reserva</h3>
              <p class="text-sm font-medium text-white/70 mt-0.5 capitalize">{{ displayDay }} • {{ formatTime(selectedTurn.horaInicio) }} - {{ formatTime(selectedTurn.horaFin) }}</p>
              <p v-if="selectedTurn.reservaId" class="text-[10px] font-black text-white/40 uppercase tracking-widest mt-2">ID #{{ selectedTurn.reservaId }}</p>
            </div>

            <!-- Body -->
            <div class="px-6 pt-5 pb-6 -mt-4 bg-white rounded-t-[20px] relative">
              <!-- Jugador -->
              <div class="mb-5">
                <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest block mb-2">Jugador</span>
                <div class="bg-slate-50 rounded-2xl border border-slate-100 p-4">
                  <div class="flex items-center gap-3">
                    <div class="w-10 h-10 rounded-full bg-[#2D9CDB]/10 text-[#2D9CDB] flex items-center justify-center shrink-0">
                      <i class="fas fa-user text-sm"></i>
                    </div>
                    <div class="min-w-0">
                      <p class="text-sm font-bold text-slate-900 truncate">{{ selectedTurn.jugadorNombre || '-' }}</p>
                      <p v-if="selectedTurn.jugadorTelefono" class="text-xs text-slate-500 truncate">{{ selectedTurn.jugadorTelefono }}</p>
                      <p v-if="selectedTurn.jugadorEmail" class="text-xs text-slate-400 truncate">{{ selectedTurn.jugadorEmail }}</p>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Info Grid -->
              <div class="grid grid-cols-2 gap-3 mb-5">
                <div class="bg-slate-50 p-3.5 rounded-2xl border border-slate-100 text-center">
                  <span class="text-[9px] font-black text-slate-400 uppercase tracking-widest block mb-1">Pago</span>
                  <span class="text-xs font-bold leading-tight" :class="(selectedTurn.estadoPago === 'pagado' || selectedTurn.estadoPago === 'Pagado') ? 'text-emerald-600' : 'text-amber-600'">
                    {{ (selectedTurn.estadoPago === 'pagado' || selectedTurn.estadoPago === 'Pagado') ? 'Pagado' : 'Pendiente' }}
                  </span>
                </div>
                <div class="bg-slate-50 p-3.5 rounded-2xl border border-slate-100 text-center">
                  <span class="text-[9px] font-black text-slate-400 uppercase tracking-widest block mb-1">Total</span>
                  <span class="text-xs font-bold text-slate-900 leading-tight">{{ selectedTurn.montoTotal ? `$${formatMoney(selectedTurn.montoTotal)}` : '-' }}</span>
                </div>
                <div class="bg-slate-50 p-3.5 rounded-2xl border border-slate-100 text-center">
                  <span class="text-[9px] font-black text-slate-400 uppercase tracking-widest block mb-1">Medio Pago</span>
                  <span class="text-xs font-bold text-slate-900 leading-tight">{{ selectedTurn.medioPagoNombre || '-' }}</span>
                </div>
                <div class="bg-slate-50 p-3.5 rounded-2xl border border-slate-100 text-center">
                  <span class="text-[9px] font-black text-slate-400 uppercase tracking-widest block mb-1">Confirmada</span>
                  <span class="text-xs font-bold leading-tight" :class="selectedTurn.confirmada ? 'text-emerald-600' : 'text-amber-600'">
                    {{ selectedTurn.confirmada ? 'Sí' : 'No' }}
                  </span>
                </div>
              </div>

              <!-- Estado Reserva -->
              <div v-if="selectedTurn.estadoReserva" class="mb-5">
                <div class="bg-slate-50 p-3.5 rounded-2xl border border-slate-100 flex items-center gap-3">
                  <div class="w-8 h-8 rounded-full flex items-center justify-center shrink-0"
                    :class="selectedTurn.estadoReserva === 'Confirmado' ? 'bg-emerald-100 text-emerald-600' : 'bg-amber-100 text-amber-600'">
                    <i :class="selectedTurn.estadoReserva === 'Confirmado' ? 'fas fa-check' : 'fas fa-clock'" class="text-xs"></i>
                  </div>
                  <div>
                    <span class="text-[9px] font-black text-slate-400 uppercase tracking-widest block">Estado Reserva</span>
                    <span class="text-xs font-bold text-slate-900">{{ selectedTurn.estadoReserva }}</span>
                  </div>
                </div>
              </div>

              <!-- Fecha creación -->
              <div v-if="selectedTurn.fechaReserva" class="mb-6">
                <div class="bg-slate-50 p-3.5 rounded-2xl border border-slate-100 flex items-center gap-3">
                  <div class="w-8 h-8 rounded-full bg-slate-200/60 text-slate-500 flex items-center justify-center shrink-0">
                    <i class="fas fa-calendar-alt text-xs"></i>
                  </div>
                  <div>
                    <span class="text-[9px] font-black text-slate-400 uppercase tracking-widest block">Creada</span>
                    <span class="text-xs font-bold text-slate-900">{{ formatFechaReserva(selectedTurn.fechaReserva) }}</span>
                  </div>
                </div>
              </div>

              <!-- Volver button -->
              <button @click="modalView = 'summary'" class="w-full py-3.5 bg-slate-100 text-slate-700 font-bold rounded-xl active:scale-95 transition-all text-sm uppercase tracking-widest">
                <i class="fas fa-arrow-left mr-2 text-xs"></i> Volver
              </button>
            </div>
          </div>

        </div>
      </div>
    </transition>

    <!-- MODAL ESTADISTICAS DEL DIA (BOTTOM SHEET / MODAL) -->
    <transition name="fade">
      <div v-if="isStatsModalOpen" class="fixed inset-0 z-[2000] flex items-end sm:items-center justify-center p-0 sm:p-4">
        <div class="absolute inset-0 bg-slate-900/60 backdrop-blur-sm" @click="closeStatsModal"></div>
        
        <div class="relative w-full max-w-lg bg-white rounded-t-[32px] sm:rounded-[28px] shadow-2xl overflow-hidden max-h-[90vh] flex flex-col animate-slide-up z-10">
          
          <!-- Drag Handle (Mobile) -->
          <div class="w-12 h-1.5 bg-slate-200 rounded-full mx-auto mt-3 mb-1 sm:hidden"></div>

          <!-- Header -->
          <div class="px-5 py-3.5 border-b border-slate-100/80 flex items-center justify-between gap-3 bg-white">
            <div class="flex items-center gap-3 min-w-0 flex-1">
              <div class="w-10 h-10 rounded-2xl bg-[#2D9CDB]/10 text-[#2D9CDB] flex items-center justify-center shrink-0">
                <i class="fas fa-chart-line text-sm"></i>
              </div>
              <div class="min-w-0 flex-1">
                <div class="flex items-center gap-1.5">
                  <h3 class="text-base font-extrabold text-slate-900 tracking-tight leading-tight truncate">Estadísticas del Día</h3>
                  <span class="text-[9px] font-black uppercase tracking-widest px-2 py-0.5 rounded-md bg-[#2D9CDB]/10 text-[#2D9CDB] capitalize shrink-0">
                    {{ displayMonth }}
                  </span>
                </div>
                <p class="text-xs text-slate-500 capitalize truncate mt-0.5">
                  {{ displayDay }} • {{ selectedComplejoId === 'all' ? 'Todos los Complejos' : currentComplejoNombre }}
                </p>
              </div>
            </div>

            <button 
              @click="closeStatsModal" 
              class="w-10 h-10 flex items-center justify-center rounded-full bg-slate-50 border border-slate-200/70 text-slate-400 hover:text-slate-700 hover:bg-slate-100 transition-all active:scale-90 shrink-0 cursor-pointer"
              title="Cerrar"
            >
              <i class="fas fa-times text-xs"></i>
            </button>
          </div>

          <!-- Body (Scrollable) -->
          <div class="px-5 pt-4 pb-8 overflow-y-auto space-y-5 no-scrollbar">

            <!-- Hero Card: Ganancias Confirmadas -->
            <div class="relative overflow-hidden rounded-2xl bg-gradient-to-br from-slate-900 to-slate-800 p-5 text-white shadow-lg">
              <div class="flex items-center justify-between mb-2">
                <div class="flex items-center gap-2">
                  <i class="fas fa-sack-dollar text-emerald-400 text-xs"></i>
                  <span class="text-[10px] font-black uppercase tracking-widest text-slate-300">Ganancias Confirmadas</span>
                </div>
                <span class="text-[9px] font-bold px-2 py-0.5 rounded-full bg-emerald-500/20 text-emerald-300 border border-emerald-500/30">
                  {{ aggregatedStats.cantidadReservasConfirmadas }} {{ aggregatedStats.cantidadReservasConfirmadas === 1 ? 'partido' : 'partidos' }}
                </span>
              </div>

              <div class="text-3xl font-black tracking-tight text-white mb-4">
                ${{ formatMoney(aggregatedStats.gananciasConfirmadas) }}
              </div>

              <!-- Breakdown pills -->
              <div class="grid grid-cols-2 gap-2 pt-3 border-t border-white/10">
                <div class="bg-white/5 rounded-xl p-2.5 border border-white/5">
                  <div class="flex items-center gap-1.5 text-[9px] font-black uppercase tracking-widest text-slate-400 mb-1">
                    <span class="w-2 h-2 rounded-full bg-emerald-400"></span>
                    Cobrado
                  </div>
                  <div class="text-sm font-bold text-emerald-400">
                    ${{ formatMoney(aggregatedStats.gananciasCobradas) }}
                  </div>
                </div>

                <div class="bg-white/5 rounded-xl p-2.5 border border-white/5">
                  <div class="flex items-center gap-1.5 text-[9px] font-black uppercase tracking-widest text-slate-400 mb-1">
                    <span class="w-2 h-2 rounded-full bg-amber-400"></span>
                    Por Cobrar
                  </div>
                  <div class="text-sm font-bold text-amber-300">
                    ${{ formatMoney(aggregatedStats.gananciasPendientesCobro) }}
                  </div>
                </div>
              </div>
            </div>

            <!-- Operational Metrics (2x2 Grid) -->
            <div class="grid grid-cols-2 gap-3">
              <!-- Reservas Totales -->
              <div class="bg-slate-50 p-4 rounded-2xl border border-slate-100 flex flex-col justify-between">
                <div class="flex items-center justify-between mb-2">
                  <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest">Reservas</span>
                  <i class="fas fa-calendar-check text-[#2D9CDB] text-xs"></i>
                </div>
                <div>
                  <div class="text-2xl font-black text-slate-900">{{ aggregatedStats.cantidadReservasTotales }}</div>
                  <p class="text-[10px] font-bold text-slate-400 mt-0.5">
                    {{ aggregatedStats.cantidadReservasConfirmadas }} confirmadas • {{ aggregatedStats.cantidadReservasPendientes }} pendientes
                  </p>
                </div>
              </div>

              <!-- Ocupación -->
              <div class="bg-slate-50 p-4 rounded-2xl border border-slate-100 flex flex-col justify-between">
                <div class="flex items-center justify-between mb-2">
                  <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest">Ocupación</span>
                  <i class="fas fa-chart-pie text-indigo-500 text-xs"></i>
                </div>
                <div>
                  <div class="text-2xl font-black text-slate-900">{{ aggregatedStats.porcentajeOcupacion }}%</div>
                  <div class="w-full bg-slate-200 h-1.5 rounded-full overflow-hidden mt-1.5 mb-1">
                    <div 
                      class="bg-[#2D9CDB] h-full rounded-full transition-all duration-500" 
                      :style="{ width: `${aggregatedStats.porcentajeOcupacion}%` }"
                    ></div>
                  </div>
                  <p class="text-[10px] font-bold text-slate-400">
                    {{ aggregatedStats.cantidadReservasTotales }} de {{ aggregatedStats.totalTurnosDisponibles }} turnos
                  </p>
                </div>
              </div>

              <!-- Turnos Disponibles -->
              <div class="bg-slate-50 p-4 rounded-2xl border border-slate-100 flex flex-col justify-between">
                <div class="flex items-center justify-between mb-2">
                  <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest">Disponibles</span>
                  <i class="fas fa-door-open text-emerald-500 text-xs"></i>
                </div>
                <div>
                  <div class="text-2xl font-black text-slate-900">{{ aggregatedStats.cantidadHorariosDisponibles }}</div>
                  <p class="text-[10px] font-bold text-slate-400 mt-0.5">Horarios libres hoy</p>
                </div>
              </div>

              <!-- Horarios Bloqueados -->
              <div class="bg-slate-50 p-4 rounded-2xl border border-slate-100 flex flex-col justify-between">
                <div class="flex items-center justify-between mb-2">
                  <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest">Bloqueados</span>
                  <i class="fas fa-ban text-slate-400 text-xs"></i>
                </div>
                <div>
                  <div class="text-2xl font-black text-slate-900">{{ aggregatedStats.cantidadHorariosBloqueados }}</div>
                  <p class="text-[10px] font-bold text-slate-400 mt-0.5">Horarios no habilitados</p>
                </div>
              </div>
            </div>

            <!-- Desglose por Cancha -->
            <div v-if="aggregatedStats.desgloseCanchas && aggregatedStats.desgloseCanchas.length > 0" class="space-y-2.5">
              <div class="flex items-center justify-between px-1">
                <span class="text-[10px] font-black uppercase tracking-widest text-slate-400">
                  Rendimiento por Cancha ({{ aggregatedStats.desgloseCanchas.length }})
                </span>
              </div>

              <div class="space-y-2">
                <div 
                  v-for="cancha in aggregatedStats.desgloseCanchas" 
                  :key="cancha.canchaId"
                  class="p-3.5 bg-slate-50 rounded-2xl border border-slate-100 flex items-center justify-between gap-3"
                >
                  <div class="min-w-0 flex-1">
                    <div class="flex items-center gap-2 mb-1">
                      <h4 class="text-xs font-extrabold text-slate-900 truncate">
                        {{ cancha.complejoNombre ? `${cancha.complejoNombre} - ` : '' }}{{ cancha.canchaNombre }}
                      </h4>
                      <span v-if="cancha.deporteNombre" class="text-[9px] font-bold px-2 py-0.5 rounded-full bg-slate-200 text-slate-600 shrink-0">
                        {{ cancha.deporteNombre }}
                      </span>
                    </div>
                    <div class="flex items-center gap-2">
                      <div class="w-20 bg-slate-200 h-1.5 rounded-full overflow-hidden shrink-0">
                        <div 
                          class="bg-[#2D9CDB] h-full rounded-full" 
                          :style="{ width: `${cancha.porcentajeOcupacion}%` }"
                        ></div>
                      </div>
                      <span class="text-[10px] text-slate-500 font-bold">
                        {{ cancha.reservasCount }}/{{ cancha.totalTurnos }} turnos ({{ cancha.porcentajeOcupacion }}%)
                      </span>
                    </div>
                  </div>

                  <div class="text-right shrink-0">
                    <span class="text-[9px] font-black text-slate-400 uppercase tracking-widest block leading-none mb-0.5">Ingresos</span>
                    <span class="text-sm font-extrabold text-slate-900">
                      ${{ formatMoney(cancha.ingresos) }}
                    </span>
                  </div>
                </div>
              </div>
            </div>

            <!-- Desglose por Medios de Pago -->
            <div class="space-y-2.5">
              <div class="flex items-center justify-between px-1">
                <span class="text-[10px] font-black uppercase tracking-widest text-slate-400">
                  Medios de Pago
                </span>
              </div>

              <div v-if="aggregatedStats.desgloseMediosPago && aggregatedStats.desgloseMediosPago.length > 0" class="grid grid-cols-1 sm:grid-cols-2 gap-2">
                <div 
                  v-for="(medio, idx) in aggregatedStats.desgloseMediosPago" 
                  :key="idx"
                  class="p-3 bg-slate-50 rounded-xl border border-slate-100 flex items-center justify-between"
                >
                  <div class="flex items-center gap-2.5 min-w-0">
                    <div class="w-8 h-8 rounded-lg bg-white shadow-sm flex items-center justify-center text-[#2D9CDB] shrink-0">
                      <i class="fas fa-wallet text-xs"></i>
                    </div>
                    <div class="truncate">
                      <p class="text-xs font-bold text-slate-900 truncate">{{ medio.medioPagoNombre }}</p>
                      <p class="text-[10px] text-slate-400">{{ medio.cantidadReservas }} {{ medio.cantidadReservas === 1 ? 'partido' : 'partidos' }}</p>
                    </div>
                  </div>
                  <div class="text-right shrink-0 font-extrabold text-xs text-slate-900 ml-2">
                    ${{ formatMoney(medio.montoTotal) }}
                  </div>
                </div>
              </div>

              <div v-else class="p-4 bg-slate-50 rounded-xl border border-slate-100 text-center">
                <p class="text-xs font-bold text-slate-400">Aún no se registran pagos en las reservas de hoy.</p>
              </div>
            </div>

          </div>

        </div>
      </div>
    </transition>

  </div>
</template>

<script>
import { ref, computed, onMounted, watch } from 'vue'
import { useAuthUser } from '../../composables/useAuthUser'
import { API_ENDPOINTS } from '../../config/apiConfig'

export default {
  name: 'OwnerReservasAgenda',
  setup() {
    const { user } = useAuthUser()

    const currentDate = ref(new Date())
    const selectedComplejoId = ref('all') // 'all' | number
    const selectedCanchaId = ref('all') // 'all' | number

    const complejosList = ref([])
    const agendasMap = ref({}) // { [complejoId]: agendaData }
    const isLoading = ref(true)
    const error = ref(null)

    const selectedTurn = ref(null)
    const selectedTurnContext = ref(null) // { complejoId, complejoNombre, canchaId, canchaNombre }
    const modalView = ref('summary') // 'summary' | 'detail' | 'confirm-cancel'
    const isCancelling = ref(false)
    const isBlocking = ref(false)
    const errorMessage = ref('')
    const successMessage = ref('')
    const isStatsModalOpen = ref(false)

    const fetchAllData = async () => {
      isLoading.value = true
      error.value = null

      try {
        const ownerId = user.value?.sub || 1
        // 1. Fetch all complexes of the owner
        const responseComp = await fetch(API_ENDPOINTS.complejos.getAll(ownerId))
        if (!responseComp.ok) {
          throw new Error('Error al cargar la lista de complejos')
        }
        const dataComp = await responseComp.json()
        complejosList.value = dataComp.complejos || []

        if (complejosList.value.length === 0) {
          agendasMap.value = {}
          isLoading.value = false
          return
        }

        // 2. Fetch agenda for all complexes for the selected date
        const dateStr = currentDate.value.toLocaleDateString('en-CA')
        const today = new Date()
        today.setHours(0, 0, 0, 0)
        const selectedDate = new Date(currentDate.value)
        selectedDate.setHours(0, 0, 0, 0)
        const isPastDate = selectedDate < today

        const newAgendas = {}
        await Promise.all(
          complejosList.value.map(async (c) => {
            try {
              const res = await fetch(
                API_ENDPOINTS.complejos.getAgenda(c.id, dateStr, ownerId, isPastDate),
                {
                  headers: {
                    'Cache-Control': 'no-cache',
                    'Pragma': 'no-cache'
                  }
                }
              )
              if (res.ok) {
                const agenda = await res.json()
                newAgendas[c.id] = agenda
              }
            } catch (err) {
              console.error(`Error loading agenda for complejo ${c.id}:`, err)
            }
          })
        )

        agendasMap.value = newAgendas
      } catch (err) {
        console.error('Error fetching owner general agenda:', err)
        error.value = err.message || 'Error al obtener la agenda de reservas'
      } finally {
        isLoading.value = false
      }
    }

    const selectComplejo = (id) => {
      selectedComplejoId.value = id
      selectedCanchaId.value = 'all'
    }

    const changeDate = (days) => {
      const newDate = new Date(currentDate.value)
      newDate.setDate(newDate.getDate() + days)
      currentDate.value = newDate
    }

    onMounted(() => {
      fetchAllData()
    })

    watch(currentDate, () => {
      fetchAllData()
    })

    const displayMonth = computed(() => {
      return currentDate.value.toLocaleString('es-ES', { month: 'long' })
    })

    const displayDay = computed(() => {
      const options = { weekday: 'long', day: 'numeric', month: 'short' }
      return currentDate.value.toLocaleDateString('es-ES', options).replace(',', '')
    })

    const currentComplejoNombre = computed(() => {
      if (selectedComplejoId.value === 'all') return 'Todos los Complejos'
      const found = complejosList.value.find(c => c.id === selectedComplejoId.value)
      return found ? found.nombre : ''
    })

    // Available canchas list according to selected complejo
    const availableCanchas = computed(() => {
      const list = []
      const targetComplejos = selectedComplejoId.value === 'all'
        ? complejosList.value
        : complejosList.value.filter(c => c.id === selectedComplejoId.value)

      targetComplejos.forEach(comp => {
        const agenda = agendasMap.value[comp.id]
        if (agenda && agenda.canchas) {
          agenda.canchas.forEach(cancha => {
            list.push({
              canchaId: cancha.canchaId,
              canchaNombre: cancha.canchaNombre,
              deporteNombre: cancha.deporteNombre,
              complejoId: comp.id,
              complejoNombre: comp.nombre
            })
          })
        }
      })
      return list
    })

    // Displayed canchas with merged turnos
    const displayedCanchas = computed(() => {
      const result = []
      const targetComplejos = selectedComplejoId.value === 'all'
        ? complejosList.value
        : complejosList.value.filter(c => c.id === selectedComplejoId.value)

      targetComplejos.forEach(comp => {
        const agenda = agendasMap.value[comp.id]
        if (agenda && agenda.canchas) {
          agenda.canchas.forEach(cancha => {
            if (selectedCanchaId.value === 'all' || selectedCanchaId.value === cancha.canchaId) {
              const mergedTurnos = []
              let currentMerged = null

              if (cancha.turnos) {
                cancha.turnos.forEach(t => {
                  if (t.estado === 'reserved' && t.reservaId) {
                    if (currentMerged && currentMerged.reservaId === t.reservaId && currentMerged.estado === 'reserved') {
                      currentMerged.horaFin = t.horaFin
                    } else {
                      currentMerged = { ...t, complejoId: comp.id, complejoNombre: comp.nombre, canchaId: cancha.canchaId, canchaNombre: cancha.canchaNombre }
                      mergedTurnos.push(currentMerged)
                    }
                  } else if (t.estado === 'blocked' && t.reservaId) {
                    if (currentMerged && currentMerged.reservaId === t.reservaId && currentMerged.estado === 'blocked') {
                      currentMerged.horaFin = t.horaFin
                    } else {
                      currentMerged = { ...t, complejoId: comp.id, complejoNombre: comp.nombre, canchaId: cancha.canchaId, canchaNombre: cancha.canchaNombre }
                      mergedTurnos.push(currentMerged)
                    }
                  } else {
                    currentMerged = null
                    mergedTurnos.push({ ...t, complejoId: comp.id, complejoNombre: comp.nombre, canchaId: cancha.canchaId, canchaNombre: cancha.canchaNombre })
                  }
                })
              }

              result.push({
                ...cancha,
                complejoId: comp.id,
                complejoNombre: comp.nombre,
                turnos: mergedTurnos
              })
            }
          })
        }
      })

      return result
    })

    // Turn click and modal management
    const handleTurnClick = (turn, canchaContext) => {
      selectedTurn.value = turn
      selectedTurnContext.value = {
        complejoId: turn.complejoId || canchaContext.complejoId,
        complejoNombre: turn.complejoNombre || canchaContext.complejoNombre,
        canchaId: turn.canchaId || canchaContext.canchaId,
        canchaNombre: turn.canchaNombre || canchaContext.canchaNombre
      }
      modalView.value = 'summary'
      errorMessage.value = ''
      successMessage.value = ''
    }

    const closeModal = () => {
      if (isCancelling.value) return
      selectedTurn.value = null
      selectedTurnContext.value = null
      modalView.value = 'summary'
      errorMessage.value = ''
      successMessage.value = ''
    }

    const openReservaDetail = () => {
      modalView.value = 'detail'
      errorMessage.value = ''
    }

    const initCancel = () => {
      errorMessage.value = ''
      successMessage.value = ''
      
      if (!selectedTurn.value) {
        errorMessage.value = 'No hay un turno seleccionado.'
        return
      }
      if (!selectedTurn.value.reservaId) {
        errorMessage.value = 'Error: este turno no tiene un ID de reserva válido asociado.'
        return
      }
      if (selectedTurn.value.estadoReserva === 'Eliminado') {
        errorMessage.value = 'Este turno ya se encuentra cancelado.'
        return
      }

      modalView.value = 'confirm-cancel'
    }

    const executeCancel = async () => {
      errorMessage.value = ''
      isCancelling.value = true
      
      try {
        const response = await fetch(API_ENDPOINTS.reservas.cancelReservation(selectedTurn.value.reservaId), {
          method: 'DELETE'
        })
        if (!response.ok) {
          throw new Error('No se pudo cancelar el turno')
        }
        
        successMessage.value = 'Turno cancelado correctamente.'
        setTimeout(() => {
          closeModal()
          fetchAllData()
        }, 1500)
      } catch (err) {
        console.error(err)
        errorMessage.value = err.message || 'Error al cancelar el turno'
      } finally {
        isCancelling.value = false
      }
    }

    const executeBlock = async () => {
      errorMessage.value = ''
      isBlocking.value = true
      
      try {
        const currentUserId = user.value?.sub || 1
        const dateStr = currentDate.value.toLocaleDateString('en-CA')

        const payload = {
          usuarioId: currentUserId,
          complejoId: parseInt(selectedTurnContext.value?.complejoId || selectedTurn.value.complejoId),
          canchaId: selectedTurn.value.canchaId || selectedTurnContext.value?.canchaId,
          fecha: dateStr,
          horaInicio: selectedTurn.value.horaInicio,
          horaFin: selectedTurn.value.horaFin
        }

        const response = await fetch(API_ENDPOINTS.reservas.block, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(payload)
        })

        if (!response.ok) {
          throw new Error('No se pudo bloquear el horario')
        }
        
        successMessage.value = 'Horario bloqueado correctamente.'
        setTimeout(() => {
          closeModal()
          fetchAllData()
        }, 1500)
      } catch (err) {
        console.error(err)
        errorMessage.value = err.message || 'Error al bloquear'
      } finally {
        isBlocking.value = false
      }
    }

    const formatTime = (timeStr) => {
      if (!timeStr) return ''
      return timeStr.substring(0, 5)
    }

    const formatFechaReserva = (fechaStr) => {
      if (!fechaStr) return '-'
      const date = new Date(fechaStr)
      return date.toLocaleDateString('es-AR', {
        day: 'numeric',
        month: 'short',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
      })
    }

    const formatMoney = (val) => {
      if (val === null || val === undefined) return '0'
      return Number(val).toLocaleString('es-AR')
    }

    const openStatsModal = () => {
      isStatsModalOpen.value = true
    }

    const closeStatsModal = () => {
      isStatsModalOpen.value = false
    }

    // Aggregated statistics calculation (across all complexes or for the selected complex)
    const aggregatedStats = computed(() => {
      let gananciasConfirmadas = 0
      let gananciasCobradas = 0
      let gananciasPendientesCobro = 0
      let totalReservas = 0
      let confirmadas = 0
      let pendientes = 0
      let bloqueados = 0
      let disponibles = 0
      let totalTurnos = 0
      const canchasMap = []
      const mediosMap = {}

      const processedReservas = new Set()

      const targetComplejos = selectedComplejoId.value === 'all'
        ? complejosList.value
        : complejosList.value.filter(c => c.id === selectedComplejoId.value)

      targetComplejos.forEach(comp => {
        const agenda = agendasMap.value[comp.id]
        if (agenda && agenda.canchas) {
          agenda.canchas.forEach(c => {
            let canchaReservas = 0
            let canchaIngresos = 0
            const canchaTurnos = c.turnos ? c.turnos.length : 0
            totalTurnos += canchaTurnos

            if (c.turnos) {
              c.turnos.forEach(t => {
                if (t.estado === 'reserved') {
                  let isNewReserva = true
                  if (t.reservaId) {
                    if (processedReservas.has(t.reservaId)) {
                      isNewReserva = false
                    } else {
                      processedReservas.add(t.reservaId)
                    }
                  }

                  if (isNewReserva) {
                    totalReservas++
                    const monto = Number(t.montoTotal) || 0
                    const isConf = t.confirmada || t.estadoReserva === 'Confirmado'
                    if (isConf) {
                      confirmadas++
                      gananciasConfirmadas += monto
                      canchaIngresos += monto
                      if (t.estadoPago === 'pagado' || t.estadoPago === 'Pagado') {
                        gananciasCobradas += monto
                      } else {
                        gananciasPendientesCobro += monto
                      }

                      const medio = t.medioPagoNombre || 'Efectivo / En complejo'
                      if (!mediosMap[medio]) {
                        mediosMap[medio] = { medioPagoNombre: medio, cantidadReservas: 0, montoTotal: 0 }
                      }
                      mediosMap[medio].cantidadReservas++
                      mediosMap[medio].montoTotal += monto
                    } else {
                      pendientes++
                    }
                  }
                  canchaReservas++
                } else if (t.estado === 'blocked') {
                  let isNewBlock = true
                  if (t.reservaId) {
                    if (processedReservas.has(t.reservaId)) {
                      isNewBlock = false
                    } else {
                      processedReservas.add(t.reservaId)
                    }
                  }
                  if (isNewBlock) {
                    bloqueados++
                  }
                } else {
                  disponibles++
                }
              })
            }

            const ocupacion = canchaTurnos > 0 ? Math.round((canchaReservas / canchaTurnos) * 1000) / 10 : 0
            canchasMap.push({
              canchaId: c.canchaId,
              canchaNombre: c.canchaNombre,
              complejoNombre: selectedComplejoId.value === 'all' ? comp.nombre : '',
              deporteNombre: c.deporteNombre || '',
              reservasCount: canchaReservas,
              totalTurnos: canchaTurnos,
              porcentajeOcupacion: Math.min(100, ocupacion),
              ingresos: canchaIngresos
            })
          })
        }
      })

      const ocupacionGral = totalTurnos > 0 ? Math.round((totalReservas / totalTurnos) * 1000) / 10 : 0

      return {
        gananciasConfirmadas,
        gananciasCobradas,
        gananciasPendientesCobro,
        cantidadReservasTotales: totalReservas,
        cantidadReservasConfirmadas: confirmadas,
        cantidadReservasPendientes: pendientes,
        cantidadHorariosBloqueados: bloqueados,
        cantidadHorariosDisponibles: disponibles,
        totalTurnosDisponibles: totalTurnos,
        porcentajeOcupacion: Math.min(100, ocupacionGral),
        desgloseCanchas: canchasMap,
        desgloseMediosPago: Object.values(mediosMap).sort((a, b) => b.montoTotal - a.montoTotal)
      }
    })

    return {
      currentDate,
      selectedComplejoId,
      selectedCanchaId,
      complejosList,
      isLoading,
      error,
      displayMonth,
      displayDay,
      currentComplejoNombre,
      availableCanchas,
      displayedCanchas,
      selectComplejo,
      changeDate,
      selectedTurn,
      selectedTurnContext,
      modalView,
      isCancelling,
      isBlocking,
      errorMessage,
      successMessage,
      handleTurnClick,
      closeModal,
      openReservaDetail,
      initCancel,
      executeCancel,
      executeBlock,
      formatTime,
      formatFechaReserva,
      formatMoney,
      isStatsModalOpen,
      openStatsModal,
      closeStatsModal,
      aggregatedStats
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
