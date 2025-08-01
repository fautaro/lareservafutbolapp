<template>
  <div class="p-4 h-full text-[#101518] space-y-8" style="font-family: Inter, 'Noto Sans', sans-serif;">
    <!-- Título -->
    <h1 class="text-4xl font-bold text-left mb-7">Menú</h1>

    <!-- Lista de opciones -->
    <div class="flex flex-col gap-6">
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
    </div>
  </div>
</template>
