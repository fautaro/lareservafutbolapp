    <style>
        .btn-logout {
            background-color: #EB5757;
        }

        .btn-logout:hover {
            background-color: #D94444;
        }
    </style>
<template>
    <div class="max-w-4xl mx-auto text-[#101518] font-sans space-y-8">
        <!-- Título -->
        <h1 class="text-4xl text-left font-bold mb-10">Mi cuenta</h1>

        <!-- Sección de perfil -->
        <section class="flex flex-col items-center gap-4">
            <img src="https://i.ibb.co/7J6gkj7s/28003-1740766555.webp" alt="Foto de perfil"
                class="w-32 h-32 rounded-full object-cover border border-gray-300" />
            <h2 class="text-2xl font-semibold">Juan Perez</h2>
        </section>

        <!-- Datos del usuario -->
        <section class="border border-gray-100 rounded-lg divide-y divide-gray-200">
            <div class="flex items-center justify-between px-6 py-3">
                <div class="flex items-center gap-2">
                    <i class="fas fa-user text-gray-500"></i>
                    <span class="text-sm text-gray-500">Nombre</span>
                </div>
                <span class="text-base font-medium text-[#101518]">Juan Perez</span>
            </div>

            <div class="flex items-center justify-between px-6 py-3">
                <div class="flex items-center gap-2">
                    <i class="fas fa-envelope text-gray-500"></i>
                    <span class="text-sm text-gray-500">Email</span>
                </div>
                <span class="text-base font-medium text-[#101518]">test@email.com</span>
            </div>

            <div class="flex items-center justify-between px-6 py-3">
                <div class="flex items-center gap-2">
                    <i class="fas fa-calendar-alt text-gray-500"></i>
                    <span class="text-sm text-gray-500">Fecha de registro</span>
                </div>
                <span class="text-base font-medium text-[#101518]">15/03/2024</span>
            </div>

            <div class="flex items-center justify-between px-6 py-3">
                <div class="flex items-center gap-2">
                    <i class="fas fa-phone text-gray-500"></i>
                    <span class="text-sm text-gray-500">Teléfono</span>
                </div>
                <span class="text-base font-medium text-[#101518]">+54 9 11 2345-6789</span>
            </div>
        </section>
    </div>
</template>

<script>
export default {
    name: 'UserProfile'
}
</script>
