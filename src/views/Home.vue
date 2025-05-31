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

    <div>
      <div class="h-5 bg-gray-50"></div>
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
