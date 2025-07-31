<template>
    <!-- Título -->
<div class="p-4 text-gray-700">
  <h1 class="text-4xl font-bold text-[#101518] text-left mb-7">Mi cuenta</h1>
</div>

    <div class="p-4 text-[#101518] space-y-6" style="font-family: Inter, 'Noto Sans', sans-serif;">
        <!-- Perfil -->
        <div class="flex flex-col items-center space-y-4">
            <img src="https://i.ibb.co/7J6gkj7s/28003-1740766555.webp" alt="Foto de perfil"
                class="w-32 h-32 rounded-full object-cover border border-gray-300" />
            <p class="text-xl font-semibold">Juan Perez</p>
        </div>

        <!-- Datos de usuario -->
        <div class="space-y-4">
            <div class="border-b pb-2">
                <p class="text-sm text-gray-500">Usuario</p>
                <p class="text-base">fautaro123</p>
            </div>
            <div class="border-b pb-2">
                <p class="text-sm text-gray-500">Email</p>
                <p class="text-base">fautaro@email.com</p>
            </div>
            <div class="border-b pb-2">
                <p class="text-sm text-gray-500">Fecha de registro</p>
                <p class="text-base">15 de marzo de 2024</p>
            </div>
            <div class="border-b pb-2">
                <p class="text-sm text-gray-500">Teléfono</p>
                <p class="text-base">+54 9 11 2345-6789</p>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: 'UserProfile'
}
</script>
