<template>
    <div class="p-4 min-h-screen text-[#101518] font-sans">
        <!-- Título -->
        <h1 class="text-4xl font-bold text-left mb-10">Configuración</h1>

        <!-- Campos -->
        <div class="space-y-6">
            <div v-for="(valor, label) in formData" :key="label">
                <label class="block text-sm font-medium text-gray-700 mb-1">{{ label }}</label>

                <div class="relative">
                    <!-- SELECT para Ciudad -->
                    <select v-if="label === 'Ciudad'" v-model="form[label]" :disabled="!editableFields[label]"
                        class="block w-full px-3 py-2 text-sm text-gray-900 border rounded-lg appearance-none bg-white disabled:bg-gray-100 disabled:cursor-not-allowed focus:ring-blue-500 focus:border-blue-500">
                        <option disabled value="">Seleccionar ciudad</option>
                        <option value="Viedma">Viedma</option>
                        <option value="Patagones">Patagones</option>
                    </select>

                    <!-- INPUT para Teléfono y Contraseña -->
                    <input v-else :type="label === 'Contraseña' ? 'password' : 'text'" v-model="form[label]"
                        :readonly="!editableFields[label]" :disabled="!editableFields[label]"
                        class="block w-full px-3 py-2 text-sm text-gray-900 border rounded-lg disabled:bg-gray-100 disabled:cursor-not-allowed focus:ring-blue-500 focus:border-blue-500" />

                    <!-- Lápiz -->
                    <button type="button" @click="enableEdit(label)"
                        class="absolute inset-y-0 right-2 flex items-center px-1 rounded hover:bg-gray-100 focus:outline-none focus:ring-0 focus:bg-transparent">
                        <i class="fas fa-pen text-sm text-gray-500"></i>
                    </button>



                </div>
            </div>
            <div class="flex items-center justify-between pt-2">
                <span class="text-sm text-gray-700 flex items-center gap-2">
                    Notificaciones
                </span>
                <label class="relative inline-flex items-center cursor-pointer">
                    <input id="notificaciones" type="checkbox" v-model="form.notificaciones" class="sr-only peer" />
                    <div class="w-11 h-6 bg-gray-200 rounded-full transition-colors"
                        :class="form.notificaciones ? 'bg-[#2D9CDB]' : 'bg-gray-200'"></div>
                    <div class="absolute left-1 top-1 w-4 h-4 bg-white rounded-full transition-transform"
                        :class="form.notificaciones ? 'translate-x-5' : ''"></div>
                </label>
            </div>

        </div>

        <!-- Botón guardar -->
        <button type="button" :disabled="!hasChanges" @click="guardarCambios"
            class="w-full mt-10 text-white font-medium py-2 rounded transition disabled:opacity-50 disabled:cursor-not-allowed"
            style="background-color: #2D9CDB;" @mouseover="hoverAzul = true" @mouseleave="hoverAzul = false"
            :style="{ backgroundColor: hoverAzul ? '#249BCF' : '#2D9CDB' }">
            Guardar cambios
        </button>

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
export default {
    name: 'ConfigUser',
    data() {
        return {
            form: {
                Teléfono: '+54 9 11 2345-6789',
                Contraseña: '********',
                Ciudad: 'Viedma',
                notificaciones: true
            },
            original: {},
            editableFields: {
                Teléfono: false,
                Contraseña: false,
                Ciudad: false
            },
            showSuccess: false
        }
    },
    computed: {
        formData() {
            return {
                Teléfono: this.form.Teléfono,
                Contraseña: this.form.Contraseña,
                Ciudad: this.form.Ciudad
            }
        },
        hasChanges() {
            return (
                this.form.Teléfono !== this.original.Teléfono ||
                this.form.Contraseña !== this.original.Contraseña ||
                this.form.Ciudad !== this.original.Ciudad ||
                this.form.notificaciones !== this.original.notificaciones
            )
        }
    },
    methods: {
        enableEdit(label) {
            this.editableFields[label] = true
        },
        guardarCambios() {
            this.original = { ...this.form }
            this.editableFields = {
                Teléfono: false,
                Contraseña: false,
                Ciudad: false
            }
            this.showSuccess = true
            setTimeout(() => {
                this.showSuccess = false
            }, 3000)
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
