<!-- src/views/MisReservas.vue -->
<template>
  <div class="p-4 text-gray-700 space-y-6">
    <h1 class="text-2xl font-bold text-[#101518] text-left">Mis Reservas</h1>

    <h2 class="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-2 text-left">
      Partidos pendientes
    </h2>

    <div class="relative rounded-xl bg-blue-100 p-4 shadow-sm text-left space-y-1">
      <!-- Botón de tres puntitos verticales corregido -->
      <button
        class="absolute top-[18px] right-2 appearance-none bg-transparent border-none p-0 m-0 text-gray-500 hover:text-gray-700"
        aria-label="Opciones"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="currentColor" viewBox="0 0 256 256">
          <circle cx="128" cy="64" r="10"/>
          <circle cx="128" cy="128" r="10"/>
          <circle cx="128" cy="192" r="10"/>
        </svg>
      </button>

      <p class="text-base font-semibold text-[#101518]">Complejo Deportivo Norte</p>
      <p class="text-sm text-gray-700">Cancha 1 - 01/01/2026 - 13:00</p>

 <p class="text-lg font-medium text-green-700 flex items-center gap-2 mt-2">
  <i class="fas fa-circle-check text-lg"></i>
  Confirmado
</p>

    </div>
  </div>
</template>
