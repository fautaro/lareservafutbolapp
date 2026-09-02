<template>
  <div class="relative min-h-screen flex flex-col bg-gray-50 overflow-x-hidden"
    style='font-family: Inter, "Noto Sans", sans-serif;'>

    <!-- Global Loader (Teleported to body for absolute priority) -->
    <Teleport to="body">
      <transition name="fade">
        <div v-if="loading"
          class="fixed inset-0 z-[9999] flex items-center justify-center bg-gray-50/90 backdrop-blur-md">
          <div class="flex flex-col items-center">
            <img :src="logo" alt="Logo" class="w-36 h-36 animate-bounce" />
          </div>
        </div>
      </transition>
    </Teleport>

    <main class="relative flex-1 overflow-y-auto pb-32" style="padding-top: max(12px, env(safe-area-inset-top, 0px));">
      <router-view v-slot="{ Component, route }">
        <transition name="fade" mode="out-in" appear>
          <component :is="Component" :key="route.fullPath" />
        </transition>
      </router-view>
    </main>

    <!-- Footer flotante estilo Apple -->
    <div class="fixed left-4 right-4 z-40 mx-auto max-w-md flex justify-between rounded-full bg-white/85 backdrop-blur-xl p-1.5 shadow-[0_8px_32px_rgba(0,0,0,0.12)] border border-gray-200/60"
      :class="loading ? 'pointer-events-none opacity-50 select-none' : ''"
      style="bottom: calc(1rem + env(safe-area-inset-bottom, 0px));">
      
      <router-link to="/" class="flex flex-1 flex-col items-center justify-center py-2 mx-0.5 gap-1 rounded-[1.25rem] transition-all duration-300 active:scale-95"
        :class="route.name === 'Home' ? 'text-[#101518] bg-black/5 shadow-[inset_0_1px_2px_rgba(0,0,0,0.03)]' : 'text-[#5c748a] hover:bg-black/[0.02]'">
        <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" fill="currentColor" viewBox="0 0 256 256">
          <path
            d="M224,115.55V208a16,16,0,0,1-16,16H168a16,16,0,0,1-16-16V168a8,8,0,0,0-8-8H112a8,8,0,0,0-8,8v40a16,16,0,0,1-16,16H48a16,16,0,0,1-16-16V115.55a16,16,0,0,1,5.17-11.78l80-75.48.11-.11a16,16,0,0,1,21.53,0,1.14,1.14,0,0,0,.11.11l80,75.48A16,16,0,0,1,224,115.55Z" />
        </svg>
        <p class="text-[10px] font-semibold leading-none tracking-wide">Inicio</p>
      </router-link>

      <router-link to="/reservas" class="flex flex-1 flex-col items-center justify-center py-2 mx-0.5 gap-1 rounded-[1.25rem] transition-all duration-300 active:scale-95"
        :class="route.name === 'MisReservas' ? 'text-[#101518] bg-black/5 shadow-[inset_0_1px_2px_rgba(0,0,0,0.03)]' : 'text-[#5c748a] hover:bg-black/[0.02]'">
        <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" fill="currentColor" viewBox="0 0 256 256">
          <path
            d="M208,32H184V24a8,8,0,0,0-16,0v8H88V24a8,8,0,0,0-16,0v8H48A16,16,0,0,0,32,48V208a16,16,0,0,0,16,16H208a16,16,0,0,0,16-16V48A16,16,0,0,0,208,32ZM72,48v8a8,8,0,0,0,16,0V48h80v8a8,8,0,0,0,16,0V48h24V80H48V48ZM208,208H48V96H208V208Zm-96-88v64a8,8,0,0,1-16,0V132.94l-4.42,2.22a8,8,0,0,1-7.16-14.32l16-8A8,8,0,0,1,112,120Zm59.16,30.45L152,176h16a8,8,0,0,1,0,16H136a8,8,0,0,1-6.4-12.8l28.78-38.37A8,8,0,1,0,145.07,132a8,8,0,1,1-13.85-8A24,24,0,0,1,176,136,23.76,23.76,0,0,1,171.16,150.45Z" />
        </svg>
        <p class="text-[10px] font-semibold leading-none tracking-wide">Reservas</p>
      </router-link>

      <router-link v-if="isOwner()" to="/owner/estadisticas" class="flex flex-1 flex-col items-center justify-center py-2 mx-0.5 gap-1 rounded-[1.25rem] transition-all duration-300 active:scale-95"
        :class="route.name === 'OwnerEstadisticas' ? 'text-[#101518] bg-black/5 shadow-[inset_0_1px_2px_rgba(0,0,0,0.03)]' : 'text-[#5c748a] hover:bg-black/[0.02]'">
        <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" fill="currentColor" viewBox="0 0 256 256">
          <path d="M224,200h-8V40a8,8,0,0,0-8-8H152a8,8,0,0,0-8,8V200H112V88a8,8,0,0,0-8-8H48a8,8,0,0,0-8,8V200H32a8,8,0,0,0,0,16H224a8,8,0,0,0,0-16ZM160,48h40V200H160ZM56,96H96V200H56Z"></path>
        </svg>
        <p class="text-[10px] font-semibold leading-none tracking-wide">Estadísticas</p>
      </router-link>

      <router-link to="/menu" class="flex flex-1 flex-col items-center justify-center py-2 mx-0.5 gap-1 rounded-[1.25rem] transition-all duration-300 active:scale-95"
        :class="route.name === 'Menu' ? 'text-[#101518] bg-black/5 shadow-[inset_0_1px_2px_rgba(0,0,0,0.03)]' : 'text-[#5c748a] hover:bg-black/[0.02]'">
        <svg xmlns="http://www.w3.org/2000/svg" width="24px" height="24px" fill="currentColor" viewBox="0 0 256 256">
          <path
            d="M224,128a8,8,0,0,1-8,8H40a8,8,0,0,1,0-16H216A8,8,0,0,1,224,128ZM40,72H216a8,8,0,0,0,0-16H40a8,8,0,0,0,0,16ZM216,184H40a8,8,0,0,0,0,16H216a8,8,0,0,0,0-16Z" />
        </svg>
        <p class="text-[10px] font-semibold leading-none tracking-wide">Menú</p>
      </router-link>
    </div>
  </div>
</template>

<script>
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import { useGlobalLoader } from '../services/globalLoader'
import { useRole } from '../composables/useRole'
import logo from '../assets/logo.svg'

export default {
  name: 'DefaultLayout',
  setup() {
    const route = useRoute()
    const { loading } = useGlobalLoader()
    const { isOwner } = useRole()
    return {
      route,
      loading,
      isOwner
    }
  },
  data() {
    return {
      logo,
    }
  },
}
</script>