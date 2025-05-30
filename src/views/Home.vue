<style>
.v-enter-active,
.v-leave-active {
  transition: opacity 0.5s ease;
}

.v-enter-from,
.v-leave-to {
  opacity: 0;
}
</style>

<template>
  <div class="min-h-screen flex flex-col bg-gray-50 overflow-x-hidden"
    style='font-family: Inter, "Noto Sans", sans-serif;'>
    <div class="flex-1 pb-28">
      <div class="flex items-center bg-gray-50 p-4 pb-2 justify-between">
        <!-- Título alineado a la izquierda -->
        <h1 class="text-[#101518] text-3xl font-bold leading-tight tracking-[-0.025em]">
          La reserva
        </h1>

        <!-- Selector de ciudad -->
        <button
          class="flex items-center gap-1 text-base font-medium bg-transparent text-black p-0 m-0 border-none shadow-none focus:outline-none">
          <!-- Icono de ubicación -->
          <i class="fas fa-map-marker-alt text-sm"></i>

          <!-- Nombre de la ciudad -->
          <span>Viedma</span>

          <!-- Icono caret hacia abajo -->
          <i class="fas fa-caret-down text-sm"></i>
        </button>

      </div>
      <!-- Subtítulo de sección -->
      <div class="px-4 pt-2 mt-3 font-bold leading-tight tracking-[-0.025em] text-left">
        <h2 class="text-[#101518] text-lg font-semibold tracking-[-0.015em]">Deportes</h2>
      </div>

      <!-- Botones de deportes -->
      <div class="w-full px-4 py-3">
        <div class="flex gap-4 justify-start">
          <button v-for="deporte in deportes" :key="deporte.id" @click="toggleDeporte(deporte.nombre)" :class="[
            'w-32 h-24 flex flex-col items-start justify-between rounded-xl text-lg font-semibold shadow-md p-3 transition-colors duration-200',
            deporteSeleccionado === deporte.nombre ? 'bg-blue-600 text-white' : 'bg-gray-200 text-black'
          ]">
            <i :class="['text-2xl', deporte.icon]"></i>
            <span>{{ deporte.nombre }}</span>
          </button>

        </div>
      </div>


      <h3 class="text-[#101518] text-lg font-bold leading-tight tracking-[-0.015em] px-4 pb-2 pt-4 mt-3 text-left">
        Complejos</h3>
      <transition name="fade" mode="out-in">
        <div :key="deporteSeleccionado">
          <div class="px-4 py-2" v-for="complejo in complejosFiltrados" :key="complejo.id">
            <div class="rounded-xl overflow-hidden shadow-md bg-white">
              <div class="relative">
                <img class="w-full h-40 object-cover rounded-t-xl" :src="complejo.imagen" :alt="complejo.nombre" />
                <div
                  class="absolute top-2 left-2 bg-black bg-opacity-60 text-white text-xs px-2 py-1 rounded-md leading-none">
                  <span class="block">Desde</span>
                  <span class="font-semibold">{{ complejo.precio }}</span>
                </div>
              </div>
              <div class="px-4 py-3">
                <div class="flex items-center justify-between">
                  <h3 class="text-[#101518] text-base font-bold">
                    {{ complejo.nombre }}
                  </h3>
                  <span :class="[
                    'text-xs font-medium px-2 py-1 rounded-full',
                    complejo.deportePillBg,
                    complejo.deportePillText
                  ]">
                    {{ complejo.categoria }}
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </transition>

    </div>
    <div>
      <div class="fixed bottom-0 left-0 right-0 z-50 flex gap-2 border-t border-[#eaedf1] bg-gray-50 px-4 pb-3 pt-2">
        <a class="just flex flex-1 flex-col items-center justify-end gap-1 rounded-full text-[#101518]" href="#">
          <div class="text-[#101518] flex h-8 items-center justify-center" data-icon="House" data-size="24px"
            data-weight="fill">
            <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" fill="currentColor"
              viewBox="0 0 256 256">
              <path
                d="M224,115.55V208a16,16,0,0,1-16,16H168a16,16,0,0,1-16-16V168a8,8,0,0,0-8-8H112a8,8,0,0,0-8,8v40a16,16,0,0,1-16,16H48a16,16,0,0,1-16-16V115.55a16,16,0,0,1,5.17-11.78l80-75.48.11-.11a16,16,0,0,1,21.53,0,1.14,1.14,0,0,0,.11.11l80,75.48A16,16,0,0,1,224,115.55Z">
              </path>
            </svg>
          </div>
          <p class="text-[#101518] text-xs font-medium leading-normal tracking-[0.015em]">Inicio</p>
        </a>
        <a class="just flex flex-1 flex-col items-center justify-end gap-1 text-[#5c748a]" href="#">
          <div class="text-[#5c748a] flex h-8 items-center justify-center" data-icon="Calendar" data-size="24px"
            data-weight="regular">
            <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" fill="currentColor"
              viewBox="0 0 256 256">
              <path
                d="M208,32H184V24a8,8,0,0,0-16,0v8H88V24a8,8,0,0,0-16,0v8H48A16,16,0,0,0,32,48V208a16,16,0,0,0,16,16H208a16,16,0,0,0,16-16V48A16,16,0,0,0,208,32ZM72,48v8a8,8,0,0,0,16,0V48h80v8a8,8,0,0,0,16,0V48h24V80H48V48ZM208,208H48V96H208V208Zm-96-88v64a8,8,0,0,1-16,0V132.94l-4.42,2.22a8,8,0,0,1-7.16-14.32l16-8A8,8,0,0,1,112,120Zm59.16,30.45L152,176h16a8,8,0,0,1,0,16H136a8,8,0,0,1-6.4-12.8l28.78-38.37A8,8,0,1,0,145.07,132a8,8,0,1,1-13.85-8A24,24,0,0,1,176,136,23.76,23.76,0,0,1,171.16,150.45Z">
              </path>
            </svg>
          </div>
          <p class="text-[#5c748a] text-xs font-medium leading-normal tracking-[0.015em]">Mis reservas</p>
        </a>
        <a class="just flex flex-1 flex-col items-center justify-end gap-1 text-[#5c748a]" href="#">
          <div class="text-[#5c748a] flex h-8 items-center justify-center" data-icon="List" data-size="24px"
            data-weight="regular">
            <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" fill="currentColor"
              viewBox="0 0 256 256">
              <path
                d="M224,128a8,8,0,0,1-8,8H40a8,8,0,0,1,0-16H216A8,8,0,0,1,224,128ZM40,72H216a8,8,0,0,0,0-16H40a8,8,0,0,0,0,16ZM216,184H40a8,8,0,0,0,0,16H216a8,8,0,0,0,0-16Z">
              </path>
            </svg>
          </div>
          <p class="text-[#5c748a] text-xs font-medium leading-normal tracking-[0.015em]">Menú</p>
        </a>
      </div>
      <div class="h-5 bg-gray-50"></div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      deporteSeleccionado: null,
      deportes: [],
      complejos: []
    };
  },
  created() {
    this.init();
  },
  computed: {
    complejosFiltrados() {
      if (!this.deporteSeleccionado) return this.complejos;
      return this.complejos.filter(
        c => c.categoria.toLowerCase().includes(this.deporteSeleccionado.toLowerCase())
      );
    }
  },
  methods: {
    init() {
      this.loadData();
    },
    loadData() {
      const response = {
        deportes: [
          {
            id: 1,
            nombre: 'Fútbol',
            icon: 'fas fa-futbol',
            bgClass: 'bg-blue-600',
            textClass: 'text-white'
          },
          {
            id: 2,
            nombre: 'Pádel',
            icon: 'fas fa-table-tennis',
            bgClass: 'bg-gray-200',
            textClass: 'text-black'
          }
        ],
        complejos: [
          {
            id: 1,
            nombre: 'Complejo La Canchita',
            precio: '$45.000',
            imagen:
              'https://lh3.googleusercontent.com/aida-public/AB6AXuBxsSA2oJ8uLiqA_UxyIMpcGzCjG7A9rphyx23t9I9LbUPu6sQ7NNrpOna63iSus18XypxnKWFFmMRJ0DgIE4naaULFZa6-9ymIJynVhMcgvYNjOOuXILs491p2a77Blc19DnV23LxVVtaxUfIAIGy1iWwVuQsdXGqrjux3kIzCGg6onm7tAax5AjvYHo5uG2ez_xQdjhXCYQu1F2Kx4D42wln9jj_WgeITaboIKKq5SKKJad1XP_Nf6q92EQccRuIhjGJKO4geabhD',
            categoria: 'Fútbol 5',
            deportePillBg: 'bg-blue-100',
            deportePillText: 'text-blue-800'
          },
          {
            id: 2,
            nombre: 'Complejo Padel Pro',
            precio: '$45.000',
            imagen:
              'https://media.lmneuquen.com/p/8123b6c6e157febeab803965b5d7743f/adjuntos/198/imagenes/007/626/0007626446/770x0/smart/world-padel-center-patagonia.png',
            categoria: 'Pádel',
            deportePillBg: 'bg-green-100',
            deportePillText: 'text-green-800'
          },
          {
            id: 3,
            nombre: 'Complejo Estadio Sur',
            precio: '$65.000',
            imagen:
              'https://www.hoysejuega.com/uploads/Modules/ImagenesComplejos/800_600_complejo-el-estadio-4.jpeg',
            categoria: 'Fútbol 11',
            deportePillBg: 'bg-indigo-100',
            deportePillText: 'text-indigo-800'
          }
        ]
      };

      this.deportes = response.deportes;
      this.complejos = response.complejos;
    },
    toggleDeporte(nombre) {
      this.deporteSeleccionado =
        this.deporteSeleccionado === nombre ? null : nombre;
    }
  }
};
</script>
