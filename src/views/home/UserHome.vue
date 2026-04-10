<template>
  <div class="min-h-screen bg-[#F8FAFC] pb-32 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">

    <!-- ERROR STATE -->
    <div v-if="loadError"
      class="fixed inset-0 flex items-center justify-center bg-slate-900/60 backdrop-blur-sm z-[2000] p-6 text-left">
      <div class="bg-white rounded-2xl shadow-2xl p-8 text-center max-w-sm w-full animate-slide-up">
        <div class="w-16 h-16 bg-red-50 rounded-full flex items-center justify-center text-red-500 mx-auto mb-5">
          <i class="fas fa-wifi text-2xl"></i>
        </div>
        <h2 class="text-xl font-bold mb-2 text-slate-900">Problema de conexión</h2>
        <p class="text-slate-500 mb-6 text-sm leading-relaxed">No pudimos obtener la información de los complejos. Por
          favor, revisá tu conexión e intentá de nuevo.</p>
        <button @click="retryLoad"
          class="w-full bg-slate-900 text-white py-4 rounded-xl font-bold uppercase tracking-widest text-[10px] shadow-lg active:scale-95 transition-all">
          Reintentar ahora
        </button>
      </div>
    </div>

    <!-- MAIN APP FLOW -->
    <div v-if="!loading && !loadError" class="animate-fade-in text-left">

      <!-- TOP NAVIGATION BAR (SLIM & CLEAN) -->
      <nav
        class="sticky top-4 z-50 bg-white/95 backdrop-blur-md shadow-sm border border-slate-100 mx-2 rounded-2xl px-3 py-3 flex items-center justify-between">
        <div class="flex flex-col items-center gap-1 min-w-[80px]">
          <svg class="h-8 w-auto" viewBox="0 0 52 40" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M0 0.19043V39.8487H30.8524L24.8072 29.1348H10.1606V0.19043H0Z" fill="#2D9CDB" />
            <path
              d="M37.7816 0.00362063C45.0797 -0.165953 51.306 5.64486 51.8218 12.4088C52.25 18.0208 48.8028 23.5772 43.2304 25.941L51.8563 39.849H38.6107L29.3726 26.1281H15.4243V17.5764H32.7911C33.1173 17.5808 34.5456 17.5662 35.7469 16.4493C36.7024 15.5605 36.6765 14.4714 36.7513 14.0884C36.7211 13.761 36.859 12.5404 35.8388 11.5624C34.5212 10.2994 32.8371 10.4982 32.6057 10.5289H15.4272V0.192198C15.4272 0.192198 37.3203 0.0138535 37.7816 0.00362063Z"
              fill="#2D9CDB" />
          </svg>
          <h1 class="text-[13px] font-bold text-[#2D9CDB] tracking-tight leading-none">
            La Reserva
          </h1>
        </div>
        <CitySelectorDrawer @city-selected="onCitySelected" />
      </nav>

      <!-- DISCOVERY SECTION: SPORTS -->
      <section class="mt-10 px-1">
        <div class="flex items-center justify-between mb-2">
          <h2 class="text-base font-bold text-slate-900 tracking-tight">Categorías</h2>
          <button v-if="deporteSeleccionado" @click="deporteSeleccionado = null"
            class="text-[10px] font-bold text-blue-600 uppercase tracking-widest">Ver todos</button>
        </div>

        <div class="tabs-container flex overflow-x-auto scroll-smooth gap-4 pt-1 pb-6 -mx-1 px-1 no-scrollbar">
          <button v-for="deporte in deportes" :key="deporte.id" @click="toggleDeporte(deporte.nombre)"
            class="flex-shrink-0 w-28 h-28 rounded-[24px] transition-all duration-300 flex flex-col items-center justify-center gap-3 border text-center shadow-sm"
            :class="deporteSeleccionado === deporte.nombre
              ? 'bg-[#2D9CDB] text-white border-[#2D9CDB] shadow-[#2D9CDB]/25 shadow-lg scale-[1.03] translate-y-[-1px]'
              : 'bg-white text-[#B3B3B3] border-slate-100 hover:border-slate-200'">
            <div class="w-12 h-12 rounded-full flex items-center justify-center transition-colors shadow-inner"
              :class="deporteSeleccionado === deporte.nombre ? 'bg-white/20' : 'bg-slate-50 text-[#B3B3B3]'">
              <i :class="['text-xl', deporte.icon]"></i>
            </div>
            <span class="text-[11px] font-black uppercase tracking-wider">{{ deporte.nombre }}</span>
          </button>
        </div>
      </section>





      <!-- MAIN FEED: COMPLEXES -->
      <section class="mt-2 px-1">
        <div class="flex items-center justify-between mb-6">
          <div class="space-y-0.5">
            <h2 class="text-xl font-bold text-slate-900 tracking-tight">
              {{ deporteSeleccionado ? 'Lo mejor en ' + deporteSeleccionado : 'Complejos disponibles' }}
            </h2>
            <p class="text-[11px] text-slate-400 font-medium">Encontrá el lugar ideal para tu próximo partido</p>
          </div>
        </div>

        <transition-group name="fade" tag="div" class="space-y-8">
          <div v-for="complejo in complejosFiltrados" :key="complejo.id" class="animate-slide-up">
            <div
              class="group relative bg-white rounded-2xl overflow-hidden shadow-sm border border-slate-100 hover:shadow-xl transition-all duration-500 cursor-pointer"
              @click="nuevaReserva(complejo.id)">
              <!-- Image Container -->
              <div class="relative aspect-[16/10] overflow-hidden">
                <img class="w-full h-full object-cover transition-transform duration-700 group-hover:scale-110"
                  :src="complejo.imagen" :alt="complejo.nombre" />
                <!-- Gradient Overlay -->
                <div class="absolute inset-0 bg-gradient-to-t from-slate-900/60 to-transparent opacity-60"></div>

                <!-- Floating Labels -->
                <div class="absolute top-4 left-4">
                  <span
                    class="bg-white/95 backdrop-blur-md text-[11px] font-black px-3.5 py-1.5 rounded-xl shadow-lg border border-white/50"
                    :class="complejo.deportePillText || 'text-[#2D9CDB]'"
                    :style="complejo.deportePillText ? '' : 'color: #2D9CDB;'">
                    {{ complejo.categoria }}
                  </span>
                </div>

                <div class="absolute bottom-5 left-5 right-5 flex items-end justify-between">
                  <div class="space-y-1">
                    <h3 class="text-2xl font-bold text-white tracking-tight drop-shadow-md">{{ complejo.nombre }}</h3>
                    <div class="flex items-center gap-1.5 text-white/90">
                      <i class="fas fa-location-dot text-[10px]"></i>
                      <span class="text-xs font-medium">{{ ciudadSeleccionada?.nombre }}</span>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Footer Details -->
              <div class="px-5 py-5 flex items-center justify-between bg-white border-t border-slate-50">
                <div class="flex items-center gap-4">
                  <div class="flex flex-col">
                    <span
                      class="text-[9px] font-black text-slate-300 uppercase tracking-widest leading-none mb-1">Precio x
                      Hora</span>
                    <div class="flex items-baseline gap-1">
                      <span class="text-base font-bold text-slate-900">{{ complejo.precio }}</span>
                      <span class="text-[10px] font-bold text-slate-400">ARS</span>
                    </div>
                  </div>
                </div>

                <div
                  class="flex items-center gap-2 text-[#2D9CDB] font-bold text-xs uppercase tracking-widest group-hover:translate-x-1 transition-transform">
                  <span>Reservar</span>
                  <i class="fas fa-chevron-right text-[10px]"></i>
                </div>
              </div>
            </div>
          </div>

          <!-- EMPTY STATE REFINED -->
          <div v-if="complejosFiltrados.length === 0" key="empty"
            class="flex flex-col items-center justify-center py-20 text-center px-10">
            <div class="w-24 h-24 bg-slate-50 rounded-full flex items-center justify-center mb-6">
              <i class="fas fa-ghost text-slate-200 text-4xl"></i>
            </div>
            <h3 class="text-lg font-bold text-slate-900">Ups, no hay resultados</h3>
            <p class="text-sm text-slate-400 mt-2 leading-relaxed">No encontramos complejos disponibles para tu
              selección en este momento.</p>
            <button @click="deporteSeleccionado = null"
              class="mt-8 px-8 py-3 bg-slate-100 text-slate-600 rounded-xl text-xs font-bold uppercase tracking-widest hover:bg-slate-200 transition-colors">
              Limpiar filtros
            </button>
          </div>
        </transition-group>
      </section>

      <!-- DECORATIVE BOTTOM SPACER -->
      <div class="h-10"></div>
    </div>
  </div>
</template>

<script>
import CitySelectorDrawer from '../../components/Home/CitySelectorDrawer.vue'
import { useGlobalLoader, startLoader, stopLoader } from '../../services/globalLoader'
import { API_ENDPOINTS } from '../../config/apiConfig'
import logo from '../../assets/logo.svg'

export default {
  name: 'Home',
  components: { CitySelectorDrawer },
  setup() {
    const { loading } = useGlobalLoader()
    return { loading }
  },

  data() {
    return {
      logo,
      ciudadSeleccionada: { id: 1, nombre: 'Viedma' },
      deporteSeleccionado: null,
      deportes: [],
      complejos: [],
      proximaReserva: null,
      loadError: false,
    }
  },

  created() {
    this.loadData()
  },

  computed: {
    complejosFiltrados() {
      return this.complejos.filter(
        c =>
          (!this.deporteSeleccionado ||
            c.categoria.toLowerCase().includes(this.deporteSeleccionado?.toLowerCase())) &&
          (!this.ciudadSeleccionada || c.ciudad_Id === this.ciudadSeleccionada.id)
      )
    },
  },
  methods: {
    async loadData() {
      startLoader()
      this.loadError = false
      try {
        const userId = 1 // Simulado: Debería venir del auth
        const [complejosRes, proximaResRes] = await Promise.all([
          fetch(API_ENDPOINTS.complejos.getAll()),
          fetch(API_ENDPOINTS.reservas.getNext(userId))
        ])

        const complexesData = await complejosRes.json()
        let nextReservaData = null

        // Manejo robusto del caso vacío (204 No Content o cuerpo vacío)
        if (proximaResRes.status === 200) {
          const text = await proximaResRes.text()
          nextReservaData = text ? JSON.parse(text) : null
        }

        this.deportes = complexesData.deportes
        this.complejos = complexesData.complejos
        this.proximaReserva = nextReservaData
      } catch (e) {
        console.error('Error cargando los datos:', e)
        // No mostramos error total si solo falló la proxima reserva, 
        // pero aquí complejosRes también podría haber fallado.
        this.loadError = true
      } finally {
        stopLoader()
      }
    },
    retryLoad() {
      this.loadError = false
      this.loadData()
    },
    toggleDeporte(nombre) {
      this.deporteSeleccionado = this.deporteSeleccionado === nombre ? null : nombre
    },
    onCitySelected(ciudad) {
      this.ciudadSeleccionada = ciudad
    },
    nuevaReserva(complejoId) {
      this.$router.push({ name: 'NuevaReserva', query: { id: complejoId } })
    },
  },
}
</script>

<style scoped>
.no-scrollbar::-webkit-scrollbar {
  display: none;
}

.no-scrollbar {
  -ms-overflow-style: none;
  scrollbar-width: none;
}

.animate-fade-in {
  animation: fadeIn 0.6s cubic-bezier(0.16, 1, 0.3, 1);
}

.animate-slide-up {
  animation: slideUp 0.5s cubic-bezier(0.16, 1, 0.3, 1);
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }

  to {
    opacity: 1;
  }
}

@keyframes slideUp {
  from {
    transform: translateY(30px);
    opacity: 0;
  }

  to {
    transform: translateY(0);
    opacity: 1;
  }
}

.fade-enter-active,
.fade-leave-active {
  transition: all 0.5s cubic-bezier(0.16, 1, 0.3, 1);
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: scale(0.95) translateY(10px);
}
</style>
