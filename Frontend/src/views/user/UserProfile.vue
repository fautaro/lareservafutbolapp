<template>
    <div class="p-4 h-full text-[#101518]" style="font-family: Inter, 'Noto Sans', sans-serif;">
        <!-- Título -->
        <h1 class="text-4xl font-bold text-left mb-7">Mi perfil</h1>

        <!-- Card de perfil mejorada -->
        <div class="bg-white rounded-2xl shadow-md overflow-hidden mb-6 border border-gray-100">
            <div class="p-6">
                <div class="flex items-center gap-4">
                    <div class="relative">
                        <img src="https://i.ibb.co/7J6gkj7s/28003-1740766555.webp" alt="Foto de perfil"
                            class="w-24 h-24 rounded-full object-cover ring-4 ring-blue-100 shadow-sm" />
                        <div class="absolute -bottom-1 -right-1 bg-green-500 w-7 h-7 rounded-full border-[3px] border-white"></div>
                    </div>
                    <div class="flex-1">
                        <h2 class="text-2xl font-bold text-[#101518] mb-1">Juan Perez</h2>
                        <p class="text-sm text-gray-500">Miembro desde marzo 2024</p>
                    </div>
                </div>
            </div>
        </div>

        <!-- Información del usuario -->
        <div class="space-y-3">
            <h3 class="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-4">Información personal</h3>
            
            <!-- Card agrupada con divisores -->
            <div class="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden">
                <!-- Nombre -->
                <div class="px-4 py-4 border-b border-gray-100">
                    <p class="text-xs text-gray-500 mb-1">Nombre completo</p>
                    <p class="text-base font-semibold text-[#101518] truncate">Juan Perez</p>
                </div>

                <!-- Email -->
                <div class="px-4 py-4 border-b border-gray-100">
                    <p class="text-xs text-gray-500 mb-1">Correo electrónico</p>
                    <p class="text-base font-semibold text-[#101518] truncate">test@email.com</p>
                </div>

                <!-- Teléfono -->
                <div class="px-4 py-4 border-b border-gray-100">
                    <p class="text-xs text-gray-500 mb-1">Teléfono</p>
                    <p class="text-base font-semibold text-[#101518]">+54 9 11 2345-6789</p>
                </div>

                <!-- Fecha de registro -->
                <div class="px-4 py-4">
                    <p class="text-xs text-gray-500 mb-1">Fecha de registro</p>
                    <p class="text-base font-semibold text-[#101518]">15/03/2024</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: 'UserProfile'
}
</script>
