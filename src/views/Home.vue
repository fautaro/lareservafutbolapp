<style></style>

<template>
  <div class="relative min-h-screen">
    <!-- Contenido Principal -->
    <transition name="fade" mode="out-in">
      <div v-if="!loading" key="main-content">
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
    </transition>
  </div>
</template>

<script>
import CitySelectorDrawer from '../components/Home/CitySelectorDrawer.vue'
import { useGlobalLoader, startLoader, stopLoader } from '../services/globalLoader' // ruta relativa

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
      _loaderTimer: null,
    }
  },

  created() {
    this.loadData()
  },

  beforeUnmount() {
    if (this._loaderTimer) clearTimeout(this._loaderTimer)
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
      try {
        const response = await fetch(`${import.meta.env.BASE_URL}data/data.json`)
        const data = await response.json()
        this.deportes = data.deportes
        this.complejos = data.complejos
      } catch (e) {
        console.error('Error cargando los datos:', e)
      } finally {
        this._loaderTimer = setTimeout(() => {
          stopLoader()
          this._loaderTimer = null
        }, 1000)
      }
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
