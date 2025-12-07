<style></style>

<template>
  <div class="relative min-h-screen">
    <!-- Aviso de error -->
    <div v-if="loadError" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-40 z-50">
      <div class="bg-white rounded-lg shadow-lg p-6 text-center max-w-sm w-full">
        <h2 class="text-xl font-bold mb-2 text-red-600">No se pudo cargar la información</h2>
        <p class="mb-4">Ocurrió un inconveniente al obtener los datos. Por favor, intenta nuevamente.</p>
        <button @click="retryLoad" class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">Reintentar</button>
      </div>
    </div>
    <!-- Contenido Principal -->
    <div v-if="!loading && !loadError" key="main-content">
        <div class="flex items-center bg-gray-50 p-4 pb-2 justify-between">
          <h1 class="text-[#101518] text-3xl font-bold leading-tight tracking-[-0.025em]">
            La reserva
          </h1>
          <CitySelectorDrawer @city-selected="onCitySelected" />
        </div>

        <div class="px-4 pt-2 mt-3 font-bold leading-tight tracking-[-0.025em] text-left">
          <h2 class="text-[#101518] text-lg font-semibold tracking-[-0.015em]">Deportes</h2>
        </div>

        <div class="w-full px-4 py-3">
          <div class="flex gap-4 justify-start">
            <button
              v-for="deporte in deportes"
              :key="deporte.id"
              @click="toggleDeporte(deporte.nombre)"
              :class="[
                'w-32 h-24 flex flex-col items-start justify-between rounded-xl text-lg font-semibold shadow-md p-3 transition-colors duration-200',
                deporteSeleccionado === deporte.nombre ? 'bg-blue-600 text-white' : 'bg-gray-200 text-black'
              ]"
            >
              <i :class="['text-2xl', deporte.icon]"></i>
              <span>{{ deporte.nombre }}</span>
            </button>
          </div>
        </div>

        <h3 class="text-[#101518] text-lg font-bold leading-tight tracking-[-0.015em] px-4 pb-2 pt-4 mt-3 text-left">
          Complejos
        </h3>

        <transition name="fade" mode="out-in">
          <div :key="deporteSeleccionado">
            <div class="px-4 py-2" v-for="complejo in complejosFiltrados" :key="complejo.id">
              <div class="rounded-xl overflow-hidden shadow-md bg-white cursor-pointer" @click="nuevaReserva(complejo.id)">
                <div class="relative">
                  <img class="w-full h-40 object-cover rounded-t-xl" :src="complejo.imagen" :alt="complejo.nombre" />
                  <div class="absolute top-2 left-2 bg-black bg-opacity-60 text-white text-xs px-2 py-1 rounded-md leading-none">
                    <span class="block">Desde</span>
                    <span class="font-semibold">{{ complejo.precio }}</span>
                  </div>
                </div>
                <div class="px-4 py-3">
                  <div class="flex items-center justify-between">
                    <h3 class="text-[#101518] text-sm font-bold truncate w-0 flex-1 text-left">
                      {{ complejo.nombre }}
                    </h3>
                    <span
                      :class="[
                        'text-xs font-medium px-2 py-1 rounded-full',
                        complejo.deportePillBg,
                        complejo.deportePillText
                      ]"
                    >
                      {{ complejo.categoria }}
                    </span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </transition>

        <div class="h-5 bg-gray-50"></div>
      </div>
  </div>
</template>

<script>
import CitySelectorDrawer from '../components/Home/CitySelectorDrawer.vue'
import { useGlobalLoader, startLoader, stopLoader } from '../services/globalLoader' // ruta relativa
import { API_ENDPOINTS } from '../config/apiConfig'

export default {
  name: 'Home',
  components: { CitySelectorDrawer },
  setup() {
    const { loading } = useGlobalLoader()
    return { loading }
  },

  data() {
    return {
      ciudadSeleccionada: { id: 1, nombre: 'Viedma' },
      deporteSeleccionado: null,
      deportes: [],
      complejos: [],
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
        const response = await fetch(API_ENDPOINTS.complejos.getAll())
        const data = await response.json()
        this.deportes = data.deportes
        this.complejos = data.complejos
      } catch (e) {
        console.error('Error cargando los datos:', e)
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
