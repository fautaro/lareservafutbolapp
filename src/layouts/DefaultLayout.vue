<!-- src/layouts/DefaultLayout.vue -->
<template>
  <div class="relative min-h-screen flex flex-col bg-gray-50 overflow-x-hidden"
    style='font-family: Inter, "Noto Sans", sans-serif;'>

    <!-- Loader -->
    <div v-if="loading" class="absolute inset-0 flex items-center justify-center z-50 bg-white/80">
      <img :src="logo" alt="Logo" class="w-36 h-36 animate-bounce" />
    </div>

    <!-- Contenido dinámico de la vista -->
    <main class="flex-1 overflow-y-auto pb-28" style="padding-top: max(12px, env(safe-area-inset-top, 0px));">
      <router-view />
    </main>

    <!-- Footer fijo -->
    <div
      class="fixed bottom-0 left-0 right-0 z-40 flex gap-2 border-t border-[#eaedf1] bg-gray-50 px-4 pt-2"
      :class="loading ? 'pointer-events-none opacity-50' : ''"
      style="padding-bottom: calc(6px + env(safe-area-inset-bottom, 0px) * 0.4);"
    >
 <router-link to="/" class="just flex flex-1 flex-col items-center justify-end gap-1"
    :class="route.name === 'Home' ? 'text-[#101518]' : 'text-[#5c748a]'">
    <div class="flex h-8 items-center justify-center">
      <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" fill="currentColor" viewBox="0 0 256 256">
        <path d="M224,115.55V208a16,16,0,0,1-16,16H168a16,16,0,0,1-16-16V168a8,8,0,0,0-8-8H112a8,8,0,0,0-8,8v40a16,16,0,0,1-16,16H48a16,16,0,0,1-16-16V115.55a16,16,0,0,1,5.17-11.78l80-75.48.11-.11a16,16,0,0,1,21.53,0,1.14,1.14,0,0,0,.11.11l80,75.48A16,16,0,0,1,224,115.55Z"/>
      </svg>
    </div>
    <p class="text-xs font-medium leading-normal tracking-[0.015em]">Inicio</p>
  </router-link>

  <router-link to="/reservas" class="just flex flex-1 flex-col items-center justify-end gap-1"
    :class="route.name === 'MisReservas' ? 'text-[#101518]' : 'text-[#5c748a]'">
    <div class="flex h-8 items-center justify-center">
      <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" fill="currentColor" viewBox="0 0 256 256">
        <path d="M208,32H184V24a8,8,0,0,0-16,0v8H88V24a8,8,0,0,0-16,0v8H48A16,16,0,0,0,32,48V208a16,16,0,0,0,16,16H208a16,16,0,0,0,16-16V48A16,16,0,0,0,208,32ZM72,48v8a8,8,0,0,0,16,0V48h80v8a8,8,0,0,0,16,0V48h24V80H48V48ZM208,208H48V96H208V208Zm-96-88v64a8,8,0,0,1-16,0V132.94l-4.42,2.22a8,8,0,0,1-7.16-14.32l16-8A8,8,0,0,1,112,120Zm59.16,30.45L152,176h16a8,8,0,0,1,0,16H136a8,8,0,0,1-6.4-12.8l28.78-38.37A8,8,0,1,0,145.07,132a8,8,0,1,1-13.85-8A24,24,0,0,1,176,136,23.76,23.76,0,0,1,171.16,150.45Z"/>
      </svg>
    </div>
    <p class="text-xs font-medium leading-normal tracking-[0.015em]">Reservas</p>
  </router-link>

  <router-link to="/menu" class="just flex flex-1 flex-col items-center justify-end gap-1"
    :class="route.name === 'Menu' ? 'text-[#101518]' : 'text-[#5c748a]'">
    <div class="flex h-8 items-center justify-center">
      <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" fill="currentColor" viewBox="0 0 256 256">
        <path d="M224,128a8,8,0,0,1-8,8H40a8,8,0,0,1,0-16H216A8,8,0,0,1,224,128ZM40,72H216a8,8,0,0,0,0-16H40a8,8,0,0,0,0,16ZM216,184H40a8,8,0,0,0,0,16H216a8,8,0,0,0,0-16Z"/>
      </svg>
    </div>
    <p class="text-xs font-medium leading-normal tracking-[0.015em]">Menú</p>
  </router-link>
    </div>
  </div>
</template>

<script>
import { useRoute } from 'vue-router'
import { useGlobalLoader } from '../services/globalLoader' // ruta relativa desde layouts/
import logo from '../assets/logo.svg'

export default {
  name: 'DefaultLayout',
  setup() {
    const route = useRoute()
    const { loading } = useGlobalLoader()
    return { route, loading }
  },
  data() {
    return {
      logo, 
    }
  },
}
</script>
