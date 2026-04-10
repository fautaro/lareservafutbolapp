<template>
    <div class="p-4 h-full text-[#101518]" style="font-family: Inter, 'Noto Sans', sans-serif;">
        <!-- Título -->
        <h1 class="text-4xl font-bold text-left mb-7">Mi perfil</h1>

        <!-- Card de perfil mejorada -->
        <div class="bg-white rounded-2xl shadow-md overflow-hidden mb-6 border border-gray-100">
            <div class="p-8">
                <div class="flex flex-col sm:flex-row items-center gap-6 text-center sm:text-left">
                    <!-- Foto con indicador -->
                    <div class="relative flex-shrink-0">
                        <img src="https://i.ibb.co/7J6gkj7s/28003-1740766555.webp" alt="Foto de perfil"
                            class="w-32 h-32 rounded-full object-cover ring-4 ring-blue-50 shadow-sm" />
                        <div
                            class="absolute bottom-1 right-2 bg-green-500 w-8 h-8 rounded-full border-[4px] border-white">
                        </div>
                    </div>

                    <!-- Datos principales -->
                    <div class="flex-1">
                        <h2 class="text-3xl font-extrabold text-[#101518] mb-2">{{ profile.nombre || 'Cargando...' }}
                        </h2>
                        <div class="flex flex-wrap items-center justify-center sm:justify-start gap-3">
                            <span
                                class="px-3 py-1 bg-blue-100 text-blue-700 text-[11px] font-black uppercase rounded-lg border border-blue-200 tracking-wider">
                                {{ profile.tipoUsuario || 'Usuario' }}
                            </span>
                            <div class="flex items-center text-gray-400">
                                <i class="fas fa-calendar-alt mr-2 text-xs"></i>
                                <p class="text-sm font-medium">Miembro desde {{ formattedDate }}</p>
                            </div>
                        </div>
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
                    <p class="text-base font-semibold text-[#101518] truncate">{{ profile.nombre || '-' }}</p>
                </div>

                <!-- Email -->
                <div class="px-4 py-4 border-b border-gray-100">
                    <p class="text-xs text-gray-500 mb-1">Correo electrónico</p>
                    <p class="text-base font-semibold text-[#101518] truncate">{{ profile.email || '-' }}</p>
                </div>

                <!-- Teléfono -->
                <div class="px-4 py-4 border-b border-gray-100">
                    <p class="text-xs text-gray-500 mb-1">Teléfono</p>
                    <p class="text-base font-semibold text-[#101518]">{{ profile.telefono || 'No especificado' }}</p>
                </div>

                <!-- Fecha de registro -->
                <div class="px-4 py-4">
                    <p class="text-xs text-gray-500 mb-1">Fecha de registro</p>
                    <p class="text-base font-semibold text-[#101518]">{{ fullDate }}</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { useAuthUser } from '../../composables/useAuthUser';
import { API_ENDPOINTS } from '../../config/apiConfig';
import { startLoader, stopLoader } from '../../services/globalLoader';

export default {
    name: 'UserProfile',
    setup() {
        const { user } = useAuthUser();
        return { user };
    },
    data() {
        return {
            profile: {},
            loading: true
        }
    },
    computed: {
        formattedDate() {
            if (!this.profile.fechaRegistro) return '...';
            const date = new Date(this.profile.fechaRegistro);
            const months = ['enero', 'febrero', 'marzo', 'abril', 'mayo', 'junio', 'julio', 'agosto', 'septiembre', 'octubre', 'noviembre', 'diciembre'];
            return `${months[date.getMonth()]} ${date.getFullYear()}`;
        },
        fullDate() {
            if (!this.profile.fechaRegistro) return '-';
            const date = new Date(this.profile.fechaRegistro);
            return date.toLocaleDateString('es-AR');
        }
    },
    async mounted() {
        await this.loadProfile();
    },
    methods: {
        async loadProfile() {
            startLoader();
            try {
                const userId = this.user?.sub || 1; // Default to user 1 (Dueño) as per user's example
                const response = await fetch(API_ENDPOINTS.usuarios.getProfile(userId));
                if (!response.ok) throw new Error('Error al cargar perfil');
                this.profile = await response.json();
            } catch (error) {
                console.error("Error loading profile:", error);
            } finally {
                this.loading = false;
                stopLoader();
            }
        }
    }
}
</script>
