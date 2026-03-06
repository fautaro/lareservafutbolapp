<template>
  <div class="p-4 h-full text-[#101518] space-y-8" style="font-family: Inter, 'Noto Sans', sans-serif;">
    <h1 class="text-4xl font-bold text-left mb-7">Menú</h1>
    <div class="flex flex-col gap-6">
      <template v-if="isAuthenticated">
        <router-link :to="{ name: 'UserProfile' }"
          class="no-underline text-inherit flex items-center gap-3 border-b border-gray-200 pb-3 text-lg font-medium">
          <i class="fas fa-user w-5 text-[#5c748a]"></i>
          Mi perfil
        </router-link>
        <router-link :to="{ name: 'ConfigUser' }"
          class="no-underline text-inherit flex items-center gap-3 border-b border-gray-200 pb-3 text-lg font-medium">
          <i class="fas fa-cog w-5 text-[#5c748a]"></i>
          Configuración
        </router-link>
        <router-link :to="{ name: 'Help' }"
          class="no-underline text-inherit flex items-center gap-3 border-b border-gray-200 pb-3 text-lg font-medium">
          <i class="fas fa-circle-question w-5 text-[#5c748a]"></i>
          Ayuda
        </router-link>
        <button @click="logoutWithAuth0"
          class="w-full bg-red-600 hover:bg-red-700 text-white font-bold py-3 px-4 rounded-lg transition duration-300 ease-in-out transform hover:scale-105 mt-2">
          <i class="fas fa-sign-out-alt mr-2"></i>
          Cerrar sesión
        </button>
      </template>
      <template v-else>
        <button @click="loginWithAuth0"
          class="w-full bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-4 rounded-lg transition duration-300 ease-in-out transform hover:scale-105">
          <i class="fas fa-sign-in-alt mr-2"></i>
          Ingresar
        </button>
      </template>
    </div>
  </div>
</template>

<script setup>
import { useAuthUser } from '../composables/useAuthUser';
const { isAuthenticated, login, logout } = useAuthUser();
const loginWithAuth0 = () => login();
const logoutWithAuth0 = () => logout({ logoutParams: { returnTo: window.location.origin } });
</script>
