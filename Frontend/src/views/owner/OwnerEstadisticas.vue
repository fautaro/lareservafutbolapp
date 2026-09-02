<template>
  <div class="min-h-screen bg-[#F8FAFC] pb-24 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">
    <!-- HEADER -->
    <div class="sticky top-0 z-40 bg-[#F8FAFC]/90 backdrop-blur-md px-5 pt-6 pb-4 mb-2">
      <!-- Title -->
      <div class="flex items-end justify-between mb-5">
        <div>
          <span class="text-[10px] font-black tracking-widest text-[#2D9CDB] uppercase mb-1 block">Modo Dueño</span>
          <h1 class="text-[28px] font-extrabold text-slate-900 tracking-tight leading-none">Estadísticas</h1>
        </div>
      </div>

      <!-- Date Selector Row -->
      <div class="flex flex-col gap-3 mb-4">
        <!-- Period Selector Filter -->
        <div class="flex overflow-x-auto gap-2 no-scrollbar pb-1">
          <button 
            v-for="periodo in periodosList" 
            :key="periodo.id"
            @click="selectPeriodo(periodo.id)"
            class="px-4 py-2 rounded-full text-xs font-bold transition-all whitespace-nowrap border"
            :class="selectedPeriodoId === periodo.id ? 'bg-slate-900 text-white border-slate-900 shadow-md' : 'bg-white text-slate-600 border-slate-200 hover:bg-slate-50'"
          >
            {{ periodo.nombre }}
          </button>
        </div>

        <div class="flex items-center justify-between mt-1">
          <h2 class="text-xl font-bold text-slate-800 capitalize leading-tight">
            {{ displayDateRange }}
          </h2>
          
          <div class="flex items-center gap-1">
            <button @click="changeDate(-1)" class="w-8 h-8 flex items-center justify-center text-[#2D9CDB] bg-[#2D9CDB]/10 hover:bg-[#2D9CDB]/20 rounded-full transition-colors active:scale-95">
              <i class="fas fa-chevron-left text-xs"></i>
            </button>
            <button @click="changeDate(1)" class="w-8 h-8 flex items-center justify-center text-[#2D9CDB] bg-[#2D9CDB]/10 hover:bg-[#2D9CDB]/20 rounded-full transition-colors active:scale-95">
              <i class="fas fa-chevron-right text-xs"></i>
            </button>
          </div>
        </div>
      </div>
      
      <!-- Complejos Selector Filter -->
      <div v-if="complejosList.length > 0" class="flex overflow-x-auto gap-2 no-scrollbar pb-1">
        <button 
          @click="selectComplejo('all')"
          class="px-4 py-2 rounded-full text-xs font-bold transition-all whitespace-nowrap border"
          :class="selectedComplejoId === 'all' ? 'bg-[#2D9CDB] text-white border-[#2D9CDB] shadow-md' : 'bg-white text-slate-600 border-slate-200 hover:bg-slate-50'"
        >
          Todos ({{ complejosList.length }})
        </button>
        <button 
          v-for="complejo in complejosList" 
          :key="complejo.id"
          @click="selectComplejo(complejo.id)"
          class="px-4 py-2 rounded-full text-xs font-bold transition-all whitespace-nowrap border"
          :class="selectedComplejoId === complejo.id ? 'bg-[#2D9CDB] text-white border-[#2D9CDB] shadow-md' : 'bg-white text-slate-600 border-slate-200 hover:bg-slate-50'"
        >
          {{ complejo.nombre }}
        </button>
      </div>
    </div>

    <!-- STATES -->
    <div v-if="isLoading" class="flex flex-col items-center justify-center py-20">
      <i class="fas fa-circle-notch animate-spin text-3xl text-[#2D9CDB]"></i>
      <p class="mt-4 font-bold text-slate-500 text-sm">Cargando estadísticas...</p>
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
      <p class="text-slate-500 text-sm mt-2">Agregá tu primer complejo para ver sus estadísticas.</p>
    </div>

    <!-- BODY -->
    <div v-else class="px-5 pb-8 animate-slide-up">
      <!-- KPIs Permanentes -->
      <div class="space-y-5 mb-6">
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
          <div class="bg-white p-4 rounded-2xl border border-slate-100 flex flex-col justify-between shadow-sm">
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
          <div class="bg-white p-4 rounded-2xl border border-slate-100 flex flex-col justify-between shadow-sm">
            <div class="flex items-center justify-between mb-2">
              <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest">Ocupación</span>
              <i class="fas fa-chart-pie text-indigo-500 text-xs"></i>
            </div>
            <div>
              <div class="text-2xl font-black text-slate-900">{{ aggregatedStats.porcentajeOcupacion }}%</div>
              <div class="w-full bg-slate-100 h-1.5 rounded-full overflow-hidden mt-1.5 mb-1">
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
          <div class="bg-white p-4 rounded-2xl border border-slate-100 flex flex-col justify-between shadow-sm">
            <div class="flex items-center justify-between mb-2">
              <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest">Disponibles</span>
              <i class="fas fa-door-open text-emerald-500 text-xs"></i>
            </div>
            <div>
              <div class="text-2xl font-black text-slate-900">{{ aggregatedStats.cantidadHorariosDisponibles }}</div>
              <p class="text-[10px] font-bold text-slate-400 mt-0.5">Horarios libres</p>
            </div>
          </div>

          <!-- Horarios Bloqueados -->
          <div class="bg-white p-4 rounded-2xl border border-slate-100 flex flex-col justify-between shadow-sm">
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
      </div>

      <!-- View Toggle -->
      <div class="flex items-center justify-center mb-6">
        <div class="bg-slate-200/50 p-1 rounded-xl inline-flex relative shadow-inner w-full max-w-sm">
          <button 
            @click="viewMode = 'datos'"
            class="flex-1 py-2.5 text-xs font-bold rounded-lg transition-all z-10"
            :class="viewMode === 'datos' ? 'bg-white text-slate-900 shadow-sm' : 'text-slate-500 hover:text-slate-700'"
          >
            <i class="fas fa-list-ul mr-1.5"></i> Datos
          </button>
          <button 
            @click="viewMode = 'graficos'"
            class="flex-1 py-2.5 text-xs font-bold rounded-lg transition-all z-10"
            :class="viewMode === 'graficos' ? 'bg-white text-slate-900 shadow-sm' : 'text-slate-500 hover:text-slate-700'"
          >
            <i class="fas fa-chart-pie mr-1.5"></i> Gráficos
          </button>
        </div>
      </div>

      <!-- Modo de Vista: Datos -->
      <div v-if="viewMode === 'datos'" class="space-y-5 animate-slide-up">
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
              class="p-3.5 bg-white rounded-2xl border border-slate-100 flex items-center justify-between gap-3 shadow-sm"
            >
              <div class="min-w-0 flex-1">
                <div class="flex items-center gap-2 mb-1">
                  <h4 class="text-xs font-extrabold text-slate-900 truncate">
                    {{ selectedComplejoId === 'all' && cancha.complejoNombre ? `${cancha.complejoNombre} - ` : '' }}{{ cancha.canchaNombre }}
                  </h4>
                  <span v-if="cancha.deporteNombre" class="text-[9px] font-bold px-2 py-0.5 rounded-full bg-slate-100 text-slate-600 shrink-0">
                    {{ cancha.deporteNombre }}
                  </span>
                </div>
                <div class="flex items-center gap-2">
                  <div class="w-20 bg-slate-100 h-1.5 rounded-full overflow-hidden shrink-0">
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
              class="p-3 bg-white rounded-xl border border-slate-100 flex items-center justify-between shadow-sm"
            >
              <div class="flex items-center gap-2.5 min-w-0">
                <div class="w-8 h-8 rounded-lg bg-slate-50 border border-slate-100 flex items-center justify-center text-[#2D9CDB] shrink-0">
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

          <div v-else class="p-4 bg-white rounded-xl border border-slate-100 text-center shadow-sm">
            <p class="text-xs font-bold text-slate-400">Aún no se registran pagos en este período.</p>
          </div>
        </div>
      </div>

      <!-- Modo de Vista: Gráficos -->
      <div v-else-if="viewMode === 'graficos'" class="space-y-4 animate-slide-up">
        
        <div class="bg-white p-4 rounded-2xl border border-slate-100 shadow-sm">
          <div class="flex items-center justify-between mb-4">
            <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest">Horarios con más reservas</span>
          </div>
          <div v-if="hasHorariosData" class="h-52 w-full">
            <Bar :data="horariosChartData" :options="horariosBarOptions" />
          </div>
          <div v-else class="h-52 w-full flex items-center justify-center text-slate-400 text-sm font-bold bg-slate-50 rounded-xl border border-dashed border-slate-200">
            No hay datos para mostrar
          </div>
        </div>

        <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <div class="bg-white p-4 rounded-2xl border border-slate-100 shadow-sm">
            <div class="flex items-center justify-between mb-4">
              <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest">Estado de Cobros</span>
            </div>
            <div v-if="hasPagosData" class="h-40 w-full flex justify-center">
              <Doughnut :data="pagosChartData" :options="donutOptions" />
            </div>
            <div v-else class="h-40 w-full flex items-center justify-center text-slate-400 text-xs font-bold bg-slate-50 rounded-xl border border-dashed border-slate-200">
              No hay datos para mostrar
            </div>
          </div>

          <div class="bg-white p-4 rounded-2xl border border-slate-100 shadow-sm">
            <div class="flex items-center justify-between mb-4">
              <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest">Medios de Pago</span>
            </div>
            <div v-if="hasMediosData" class="h-40 w-full flex justify-center">
              <Doughnut :data="mediosPagoChartData" :options="donutOptions" />
            </div>
            <div v-else class="h-40 w-full flex items-center justify-center text-slate-400 text-xs font-bold bg-slate-50 rounded-xl border border-dashed border-slate-200">
              No hay datos para mostrar
            </div>
          </div>
        </div>

      </div>

    </div>

  </div>
</template>

<script>
import { ref, computed, onMounted, watch } from 'vue'
import { useAuthUser } from '../../composables/useAuthUser'
import { API_ENDPOINTS } from '../../config/apiConfig'
import { Doughnut, Bar } from 'vue-chartjs'
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  ArcElement,
  BarElement,
  CategoryScale,
  LinearScale,
} from 'chart.js'
import ChartDataLabels from 'chartjs-plugin-datalabels'

ChartJS.register(Title, Tooltip, Legend, ArcElement, BarElement, CategoryScale, LinearScale, ChartDataLabels)

export default {
  name: 'OwnerEstadisticas',
  components: { Doughnut, Bar },
  setup() {
    const { user } = useAuthUser()

    const viewMode = ref('datos')

    const periodosList = [
      { id: 'diario', nombre: 'Diario' },
      { id: 'semanal', nombre: 'Semanal' },
      { id: 'mensual', nombre: 'Mensual' },
      { id: 'anual', nombre: 'Anual' }
    ]
    const selectedPeriodoId = ref('diario')

    const currentDate = ref(new Date())
    const selectedComplejoId = ref('all')

    const complejosList = ref([])
    const isLoading = ref(true)
    const error = ref(null)

    const aggregatedStats = ref({
      gananciasConfirmadas: 0,
      gananciasCobradas: 0,
      gananciasPendientesCobro: 0,
      cantidadReservasTotales: 0,
      cantidadReservasConfirmadas: 0,
      cantidadReservasPendientes: 0,
      cantidadHorariosBloqueados: 0,
      cantidadHorariosDisponibles: 0,
      totalTurnosDisponibles: 0,
      porcentajeOcupacion: 0,
      desgloseCanchas: [],
      desgloseMediosPago: []
    })

    const hasPagosData = computed(() => {
      return aggregatedStats.value.gananciasCobradas > 0 || aggregatedStats.value.gananciasPendientesCobro > 0
    })

    const hasMediosData = computed(() => {
      return aggregatedStats.value.desgloseMediosPago && aggregatedStats.value.desgloseMediosPago.length > 0 && aggregatedStats.value.desgloseMediosPago.some(m => m.montoTotal > 0)
    })

    const hasHorariosData = computed(() => {
      return aggregatedStats.value.desgloseHorarios && aggregatedStats.value.desgloseHorarios.length > 0 && aggregatedStats.value.desgloseHorarios.some(h => h.cantidadReservas > 0)
    })

    const dateRange = computed(() => {
      const start = new Date(currentDate.value)
      start.setHours(0, 0, 0, 0)
      let end = new Date(start)

      if (selectedPeriodoId.value === 'diario') {
        // end is same as start
      } else if (selectedPeriodoId.value === 'semanal') {
        const day = start.getDay() // 0 is Sunday
        const diff = start.getDate() - day + (day === 0 ? -6 : 1) // adjust to Monday
        start.setDate(diff)
        end = new Date(start)
        end.setDate(end.getDate() + 6)
      } else if (selectedPeriodoId.value === 'mensual') {
        start.setDate(1)
        end = new Date(start.getFullYear(), start.getMonth() + 1, 0)
      } else if (selectedPeriodoId.value === 'anual') {
        start.setMonth(0, 1)
        end = new Date(start.getFullYear(), 11, 31)
      }
      
      return { start, end }
    })

    const displayDateRange = computed(() => {
      const { start, end } = dateRange.value
      if (selectedPeriodoId.value === 'diario') {
        return start.toLocaleDateString('es-ES', { weekday: 'long', day: 'numeric', month: 'short' }).replace(',', '')
      } else if (selectedPeriodoId.value === 'semanal') {
        return `${start.getDate()} ${start.toLocaleDateString('es-ES', {month: 'short'})} - ${end.getDate()} ${end.toLocaleDateString('es-ES', {month: 'short'})}`
      } else if (selectedPeriodoId.value === 'mensual') {
        return start.toLocaleDateString('es-ES', { month: 'long', year: 'numeric' })
      } else if (selectedPeriodoId.value === 'anual') {
        return start.getFullYear().toString()
      }
    })

    const fetchStats = async () => {
      isLoading.value = true
      error.value = null

      try {
        const ownerId = user.value?.sub || 1

        if (complejosList.value.length === 0) {
          const responseComp = await fetch(API_ENDPOINTS.complejos.getAll(ownerId))
          if (!responseComp.ok) {
            throw new Error('Error al cargar la lista de complejos')
          }
          const dataComp = await responseComp.json()
          complejosList.value = dataComp.complejos || []
        }

        if (complejosList.value.length === 0) {
          isLoading.value = false
          return
        }

        const { start, end } = dateRange.value
        const startStr = start.toLocaleDateString('en-CA')
        const endStr = end.toLocaleDateString('en-CA')

        const cId = selectedComplejoId.value === 'all' ? '' : selectedComplejoId.value

        const res = await fetch(
          API_ENDPOINTS.complejos.getEstadisticas(cId, startStr, endStr, ownerId),
          {
            headers: {
              'Cache-Control': 'no-cache',
              'Pragma': 'no-cache'
            }
          }
        )

        if (!res.ok) {
          throw new Error('Error al obtener estadísticas')
        }

        aggregatedStats.value = await res.json()
      } catch (err) {
        console.error('Error fetching owner general stats:', err)
        error.value = err.message || 'Error al obtener las estadísticas'
      } finally {
        isLoading.value = false
      }
    }

    const selectComplejo = (id) => {
      selectedComplejoId.value = id
    }

    const selectPeriodo = (id) => {
      selectedPeriodoId.value = id
      currentDate.value = new Date() // reset to current date when changing period
    }

    const changeDate = (direction) => {
      const newDate = new Date(currentDate.value)
      if (selectedPeriodoId.value === 'diario') {
        newDate.setDate(newDate.getDate() + direction)
      } else if (selectedPeriodoId.value === 'semanal') {
        newDate.setDate(newDate.getDate() + (direction * 7))
      } else if (selectedPeriodoId.value === 'mensual') {
        newDate.setMonth(newDate.getMonth() + direction)
      } else if (selectedPeriodoId.value === 'anual') {
        newDate.setFullYear(newDate.getFullYear() + direction)
      }
      currentDate.value = newDate
    }

    onMounted(() => {
      fetchStats()
    })

    watch(selectedComplejoId, () => {
      fetchStats()
    })
    
    watch(selectedPeriodoId, () => {
      // fetchStats will be triggered by currentDate watch since selectPeriodo resets currentDate
    })

    watch(currentDate, () => {
      fetchStats()
    })

    const formatMoney = (val) => {
      if (val === null || val === undefined) return '0'
      return Number(val).toLocaleString('es-AR')
    }

    // Chart Data Computeds
    const pagosChartData = computed(() => ({
      labels: ['Cobrado', 'Por Cobrar'],
      datasets: [
        {
          backgroundColor: ['#34D399', '#FCD34D'],
          data: [aggregatedStats.value.gananciasCobradas, aggregatedStats.value.gananciasPendientesCobro],
          borderWidth: 0
        }
      ]
    }))

    const mediosPagoChartData = computed(() => {
      const labels = aggregatedStats.value.desgloseMediosPago.map(m => m.medioPagoNombre)
      const data = aggregatedStats.value.desgloseMediosPago.map(m => m.montoTotal)
      const colors = ['#2D9CDB', '#F472B6', '#A78BFA', '#FBBF24', '#34D399', '#F87171']
      return {
        labels: labels,
        datasets: [
          {
            backgroundColor: colors,
            data: data,
            borderWidth: 0
          }
        ]
      }
    })

    const horariosChartData = computed(() => {
      const horarios = aggregatedStats.value.desgloseHorarios || []
      const labels = horarios.map(h => h.horario)
      const data = horarios.map(h => h.cantidadReservas)
      
      return {
        labels: labels,
        datasets: [
          {
            label: 'Reservas',
            backgroundColor: '#8B5CF6',
            data: data,
            borderRadius: 6
          }
        ]
      }
    })
    
    const donutOptions = {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: { position: 'bottom', labels: { boxWidth: 12, font: { family: 'Inter' } } },
        datalabels: {
          color: '#ffffff',
          font: { weight: 'bold', size: 11, family: 'Inter' },
          formatter: (value, ctx) => {
            let sum = 0;
            let dataArr = ctx.chart.data.datasets[0].data;
            dataArr.forEach(data => {
                sum += Number(data);
            });
            if (sum === 0) return '';
            let percentage = (value * 100 / sum).toFixed(1) + "%";
            return value > 0 ? percentage : '';
          }
        }
      }
    }

    const horariosBarOptions = {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: { display: false },
        datalabels: {
          color: '#64748b',
          anchor: 'end',
          align: 'top',
          font: { weight: 'bold', size: 10, family: 'Inter' },
          formatter: (value, ctx) => {
            let sum = 0;
            let dataArr = ctx.chart.data.datasets[0].data;
            dataArr.forEach(data => { sum += Number(data); });
            if (sum === 0 || value === 0) return '';
            let percentage = (value * 100 / sum).toFixed(1) + "%";
            return percentage;
          },
          display: function(context) {
            return context.dataset.data[context.dataIndex] > 0;
          }
        }
      },
      scales: {
        y: { beginAtZero: true, grid: { borderDash: [4, 4] } },
        x: { grid: { display: false } }
      },
      layout: {
        padding: {
          top: 25 // Add padding so the labels don't get cut off
        }
      }
    }

    return {
      viewMode,
      periodosList,
      selectedPeriodoId,
      currentDate,
      selectedComplejoId,
      complejosList,
      isLoading,
      error,
      displayDateRange,
      selectComplejo,
      selectPeriodo,
      changeDate,
      formatMoney,
      aggregatedStats,
      pagosChartData,
      mediosPagoChartData,
      horariosChartData,
      donutOptions,
      horariosBarOptions,
      hasPagosData,
      hasMediosData,
      hasHorariosData
    }
  }
}
</script>

<style scoped>
.animate-slide-up {
  animation: slideUp 0.3s cubic-bezier(0.16, 1, 0.3, 1) both;
}

@keyframes slideUp {
  from { transform: translateY(10px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}

.no-scrollbar::-webkit-scrollbar {
  display: none;
}
.no-scrollbar {
  -ms-overflow-style: none;
  scrollbar-width: none;
}
</style>
