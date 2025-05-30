<template>
  <div
    class="relative flex size-full min-h-screen flex-col bg-gray-50 justify-between group/design-root overflow-x-hidden"
    style='font-family: Inter, "Noto Sans", sans-serif;'>
<div class="pb-28">
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

      <div class="w-full px-4 py-3">
        <div class="flex gap-4 justify-start">
          <!-- Botón Fútbol -->
          <button
            class="w-32 h-24 flex flex-col items-start justify-between rounded-xl bg-blue-600 text-white text-lg font-semibold shadow-md p-3">
            <i class="fas fa-futbol text-2xl"></i>
            <span>Fútbol</span>
          </button>

          <!-- Botón Pádel -->
          <button
            class="w-32 h-24 flex flex-col items-start justify-between rounded-xl bg-gray-200 text-black text-lg font-semibold shadow-md p-3">
            <i class="fas fa-table-tennis text-2xl"></i>
            <span>Pádel</span>
          </button>
        </div>
      </div>


      <h3 class="text-[#101518] text-lg font-bold leading-tight tracking-[-0.015em] px-4 pb-2 pt-4 mt-3 text-left">
        Complejos</h3>
      <div class="px-4 py-2">
        <div class="rounded-xl overflow-hidden shadow-md bg-white">
          <!-- Imagen con overlay -->
          <div class="relative">
            <img class="w-full h-40 object-cover rounded-t-xl"
              src="https://lh3.googleusercontent.com/aida-public/AB6AXuBxsSA2oJ8uLiqA_UxyIMpcGzCjG7A9rphyx23t9I9LbUPu6sQ7NNrpOna63iSus18XypxnKWFFmMRJ0DgIE4naaULFZa6-9ymIJynVhMcgvYNjOOuXILs491p2a77Blc19DnV23LxVVtaxUfIAIGy1iWwVuQsdXGqrjux3kIzCGg6onm7tAax5AjvYHo5uG2ez_xQdjhXCYQu1F2Kx4D42wln9jj_WgeITaboIKKq5SKKJad1XP_Nf6q92EQccRuIhjGJKO4geabhD"
              alt="Complejo" />
            <!-- Overlay de precio -->
            <div
              class="absolute top-2 left-2 bg-black bg-opacity-60 text-white text-xs px-2 py-1 rounded-md leading-none">
              <span class="block">Desde</span>
              <span class="font-semibold">$45.000</span>
            </div>
          </div>

          <!-- Footer de la card -->
          <div class="px-4 py-3">
            <div class="flex items-center justify-between">
              <h3 class="text-[#101518] text-base font-bold">Complejo La Canchita</h3>
              <span class="text-xs bg-blue-100 text-blue-800 font-medium px-2 py-1 rounded-full">Fútbol 5</span>
            </div>
          </div>
        </div>
      </div>

      <div class="px-4 py-2">
        <div class="rounded-xl overflow-hidden shadow-md bg-white">
          <!-- Imagen con overlay -->
          <div class="relative">
            <img class="w-full h-40 object-cover rounded-t-xl"
              src="https://media.lmneuquen.com/p/8123b6c6e157febeab803965b5d7743f/adjuntos/198/imagenes/007/626/0007626446/770x0/smart/world-padel-center-patagonia.png"
              alt="Complejo de Pádel" />
            <!-- Overlay de precio -->
            <div
              class="absolute top-2 left-2 bg-black bg-opacity-60 text-white text-xs px-2 py-1 rounded-md leading-none">
              <span class="block">Desde</span>
              <span class="font-semibold">$45.000</span>
            </div>
          </div>

          <!-- Footer de la card -->
          <div class="px-4 py-3">
            <div class="flex items-center justify-between">
              <h3 class="text-[#101518] text-base font-bold">Complejo Padel Pro</h3>
              <span class="text-xs bg-green-100 text-green-800 font-medium px-2 py-1 rounded-full">Pádel</span>
            </div>
          </div>
        </div>
      </div>
      <div class="px-4 py-2">
        <div class="rounded-xl overflow-hidden shadow-md bg-white">
          <!-- Imagen con overlay -->
          <div class="relative">
            <img class="w-full h-40 object-cover rounded-t-xl"
              src="https://www.hoysejuega.com/uploads/Modules/ImagenesComplejos/800_600_complejo-el-estadio-4.jpeg"
              alt="Cancha de Fútbol 11" />
            <!-- Overlay de precio -->
            <div
              class="absolute top-2 left-2 bg-black bg-opacity-60 text-white text-xs px-2 py-1 rounded-md leading-none">
              <span class="block">Desde</span>
              <span class="font-semibold">$65.000</span>
            </div>
          </div>

          <!-- Footer de la card -->
          <div class="px-4 py-3">
            <div class="flex items-center justify-between">
              <h3 class="text-[#101518] text-base font-bold">Complejo Estadio Sur</h3>
              <span class="text-xs bg-indigo-100 text-indigo-800 font-medium px-2 py-1 rounded-full">Fútbol 11</span>
            </div>
          </div>
        </div>
      </div>
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
      mostrarSelector: false,
      localidadSeleccionada: 'Localidad',
      localidades: ['Buenos Aires', 'Córdoba', 'Rosario', 'Mendoza']
    }
  },
  methods: {
    seleccionarLocalidad(loc) {
      this.localidadSeleccionada = loc
      this.mostrarSelector = false
    }
  }
}
</script>

<style>
.slide-up-enter-active,
.slide-up-leave-active {
  transition: transform 0.3s ease;
}

.slide-up-enter-from,
.slide-up-leave-to {
  transform: translateY(100%);
}
</style>
