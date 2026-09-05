<template>
    <div class="p-4 h-full text-[#101518]" style="font-family: Inter, 'Noto Sans', sans-serif;">
        <!-- Título -->
        <h1 class="text-4xl font-bold text-left mb-7">Configuración</h1>

        <!-- Campos agrupados en card -->
        <div class="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden mb-6">
            <div v-for="(valor, label, index) in formData" :key="label" 
                :class="['px-4 py-4', index < Object.keys(formData).length - 1 ? 'border-b border-gray-100' : '']">
                <label class="block text-xs text-gray-500 mb-2">{{ label }}</label>
                
                <div class="relative">
                    <!-- INPUT para Teléfono y Contraseña -->
                    <input :type="label === 'Contraseña' ? 'password' : 'text'" v-model="form[label]"
                        :readonly="!editableFields[label]" :disabled="!editableFields[label]"
                        class="block w-full px-3 py-2.5 text-base font-semibold text-[#101518] border border-gray-200 rounded-lg disabled:bg-gray-50 disabled:cursor-not-allowed focus:ring-2 focus:ring-blue-500 focus:border-blue-500 pr-10" />

                    <!-- Lápiz -->
                    <button type="button" @click="enableEdit(label)"
                        class="absolute inset-y-0 right-2 flex items-center px-2 rounded-lg hover:bg-gray-100 transition-colors focus:outline-none">
                        <i class="fas fa-pen text-sm text-gray-400 hover:text-blue-600"></i>
                    </button>
                </div>
            </div>
            
            <!-- Notificaciones -->
            <div class="px-4 py-4 flex items-center justify-between">
                <div>
                    <p class="text-xs text-gray-500 mb-1">Notificaciones</p>
                    <p class="text-base font-semibold text-[#101518]">{{ form.notificaciones ? 'Activadas' : 'Desactivadas' }}</p>
                </div>
                <label class="relative inline-flex items-center cursor-pointer">
                    <input id="notificaciones" type="checkbox" v-model="form.notificaciones" class="sr-only peer" />
                    <div class="w-12 h-7 bg-gray-200 rounded-full transition-colors shadow-inner"
                        :class="form.notificaciones ? 'bg-blue-600' : 'bg-gray-300'"></div>
                    <div class="absolute left-1 top-1 w-5 h-5 bg-white rounded-full transition-transform shadow-sm"
                        :class="form.notificaciones ? 'translate-x-5' : ''"></div>
                </label>
            </div>
        </div>

        <!-- Botón guardar -->
        <button type="button" :disabled="!hasChanges" @click="guardarCambios"
            class="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-3 px-4 rounded-xl transition duration-200 disabled:opacity-50 disabled:cursor-not-allowed disabled:hover:bg-blue-600 shadow-sm">
            Guardar cambios
        </button>

        <!-- Toast de éxito -->
        <transition name="fade">
            <div v-if="showSuccess" class="fixed bottom-20 inset-x-0 flex justify-center z-50 px-4">
                <div class="flex items-center w-full max-w-xs p-4 text-sm text-white rounded-lg shadow-lg" role="alert"
                    style="background-color: #1DB954;">
                    <i class="fas fa-check-circle text-white me-2"></i>
                    <div>Cambios guardados correctamente.</div>
                </div>
            </div>
        </transition>
    </div>
</template>

<script>
import { useAuthUser } from '../../composables/useAuthUser';
import { API_ENDPOINTS } from '../../config/apiConfig';

export default {
    name: 'ConfigUser',
    setup() {
        const { user, setDebeCambiarPassword } = useAuthUser();
        return { user, setDebeCambiarPassword };
    },
    data() {
        return {
            form: {
                Teléfono: '+54 9 11 2345-6789',
                Contraseña: '••••••••',
                notificaciones: true
            },
            original: {},
            editableFields: {
                Teléfono: false,
                Contraseña: false
            },
            showSuccess: false
        }
    },
    computed: {
        formData() {
            return {
                Teléfono: this.form.Teléfono,
                Contraseña: this.form.Contraseña
            }
        },
        hasChanges() {
            return (
                this.form.Teléfono !== this.original.Teléfono ||
                this.form.Contraseña !== this.original.Contraseña ||
                this.form.notificaciones !== this.original.notificaciones
            )
        }
    },
    methods: {
        enableEdit(label) {
            if (label === 'Contraseña' && this.form.Contraseña === '••••••••') {
                this.form.Contraseña = '';
            }
            this.editableFields[label] = true
        },
        async guardarCambios() {
            try {
                if (this.editableFields['Contraseña'] && this.form.Contraseña && this.form.Contraseña !== this.original.Contraseña) {
                    const userId = this.user?.id || this.user?.sub || 1;
                    const response = await fetch(API_ENDPOINTS.usuarios.cambiarPassword(), {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({
                            usuarioId: userId,
                            nuevaPassword: this.form.Contraseña.trim()
                        })
                    });
                    if (!response.ok) {
                        const err = await response.json().catch(() => ({}));
                        throw new Error(err.message || 'Error al actualizar contraseña.');
                    }
                    this.setDebeCambiarPassword(false);
                }

                this.original = { ...this.form }
                this.editableFields = {
                    Teléfono: false,
                    Contraseña: false
                }
                this.showSuccess = true
                setTimeout(() => {
                    this.showSuccess = false
                }, 3000)
            } catch (error) {
                console.error("Error al guardar cambios:", error);
                alert(error.message || 'Error al guardar cambios.');
            }
        },
        onChange() { }
    },
    mounted() {
        this.original = { ...this.form }
    }
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
    transition: opacity 0.4s ease;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}
</style>
