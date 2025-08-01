<template>
    <div class="max-w-4xl mx-auto text-[#101518] font-sans space-y-8">
        <!-- Título -->
        <h1 class="text-4xl text-left font-bold mb-10">Mi cuenta</h1>

        <!-- Sección de perfil -->
        <section class="flex flex-col items-center gap-4">
            <img src="https://i.ibb.co/7J6gkj7s/28003-1740766555.webp" alt="Foto de perfil"
                class="w-32 h-32 rounded-full object-cover border border-gray-300" />
            <h2 class="text-2xl font-semibold">Juan Perez</h2>
            <!-- Label de rol -->
            <p class="text-sm text-gray-600">Rol: <span class="font-medium">{{ rol }}</span></p>
            <!-- Estado de verificación -->
            <span class="px-3 py-1 text-sm rounded-full font-medium"
                :class="cuentaVerificada ? 'bg-green-100 text-green-700' : 'bg-yellow-100 text-yellow-800'">
                {{ cuentaVerificada ? 'Cuenta verificada' : 'Cuenta sin verificar' }}
            </span>
        </section>

        <!-- Datos del usuario -->
        <section class="border border-gray-100 rounded-lg divide-y divide-gray-200">
            <div class="flex items-center justify-between px-6 py-3">
                <div class="flex items-center gap-2">
                    <i class="fas fa-user text-gray-500"></i>
                    <span class="text-sm text-gray-500">Usuario</span>
                </div>
                <span class="text-base font-medium text-[#101518]">fautaro123</span>
            </div>

            <div class="flex items-center justify-between px-6 py-3">
                <div class="flex items-center gap-2">
                    <i class="fas fa-envelope text-gray-500"></i>
                    <span class="text-sm text-gray-500">Email</span>
                </div>
                <span class="text-base font-medium text-[#101518]">fautaro@email.com</span>
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

            <div class="flex items-center justify-between px-6 py-3">
                <div class="flex items-center gap-2">
                    <i class="fas fa-futbol text-gray-500"></i>
                    <span class="text-sm text-gray-500">Partidos jugados</span>
                </div>
                <span class="text-base font-medium text-[#101518]">25</span>
            </div>

            <div class="flex items-center justify-between px-6 py-3">
                <div class="flex items-center gap-2">
                    <i class="fas fa-trophy text-gray-500"></i>
                    <span class="text-sm text-gray-500">Deporte preferido</span>
                </div>
                <span class="text-base font-medium text-[#101518]">Fútbol</span>
            </div>

        </section>
        <!-- Botón cerrar sesión -->
        <div class="pt-6">
            <button @click="cerrarSesion"
                class="w-full sm:w-auto px-5 py-2 text-sm font-medium text-white rounded-lg transition focus:outline-none focus:ring-4"
                style="background-color: #EB5757;" @mouseover="hovering = true" @mouseleave="hovering = false"
                :style="{ backgroundColor: hovering ? '#D94444' : '#EB5757' }">
                Cerrar sesión
            </button>
        </div>

    </div>
</template>

<script>
export default {
    name: 'UserProfile',
    data() {
        return {
            rol: 'Jugador', // o 'Dueño de cancha'
            cuentaVerificada: true // o false
        }
    },
    methods: {
        cerrarSesion() {
            // lógica real de logout
            console.log('Sesión cerrada')
        }
    }
}
</script>
