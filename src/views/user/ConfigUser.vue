    <style>

    /* en style scoped o global */
    .fade-enter-active,
    .fade-leave-active {
        transition: opacity 0.4s ease;
    }

    .fade-enter-from,
    .fade-leave-to {
        opacity: 0;
    }

select {
    -webkit-appearance: none;
    -moz-appearance: none;
    appearance: none;
    padding: 2px 30px 2px 2px;
    border: none;
}
</style>
<template>
    <div class="p-4 min-h-screen text-[#101518]" style="font-family: Inter, 'Noto Sans', sans-serif;">
        <!-- Título -->
        <h1 class="text-4xl font-bold text-left mb-7">Configuración</h1>

        <!-- Campos -->
        <div class="space-y-6">
            <div v-for="(valor, label) in formData" :key="label">
                <label class="block text-sm text-gray-600 mb-1">{{ label }}</label>
                <div class="relative">
                    <!-- SELECT para Ciudad -->
                    <select v-if="label === 'Ciudad'" v-model="form[label]" :disabled="!editableFields[label]" :class="[
                        'w-full rounded pl-3 pr-10 py-2 text-[#101518] appearance-none transition',
                        editableFields[label]
                            ? 'border border-blue-400 bg-white focus:outline-none focus:ring-2 focus:ring-blue-400'
                            : 'bg-gray-100 border border-gray-200 cursor-default'
                    ]"
                        style="background-image: url('data:image/svg+xml;utf8,<svg fill=\'%235c748a\' height=\'20\' viewBox=\'0 0 20 20\' width=\'20\' xmlns=\'http://www.w3.org/2000/svg\'><path d=\'M5.5 7.5l4.5 4.5 4.5-4.5z\'/></svg>'); background-repeat: no-repeat; background-position: right 0.75rem center; background-size: 1rem;">
                        <option disabled value="">Seleccionar ciudad</option>
                        <option value="Viedma">Viedma</option>
                        <option value="Buenos Aires">Buenos Aires</option>
                        <option value="Córdoba">Córdoba</option>
                        <option value="Mendoza">Mendoza</option>
                    </select>
                    <!-- INPUT para el resto -->
                    <input v-else :type="label === 'Contraseña' ? 'password' : 'text'" v-model="form[label]"
                        :readonly="!editableFields[label]" :disabled="!editableFields[label]" :class="[
                            'w-full rounded pl-3 pr-10 py-2 text-[#101518] transition',
                            editableFields[label]
                                ? 'border border-blue-400 bg-white focus:outline-none focus:ring-2 focus:ring-blue-400'
                                : 'bg-gray-100 border border-gray-200 cursor-default'
                        ]" />

                    <!-- Lápiz -->
                    <div type="button" @click="enableEdit(label)"
                        class="absolute right-1.5 top-1/2 -translate-y-1/2 p-1 rounded-full hover:bg-gray-200 transition">
                        <i class="fas fa-pen text-xs text-gray-500"></i>
                    </div>
                </div>
            </div>



            <!-- Notificaciones -->
            <div class="flex items-center justify-between pt-2">
                <label class="text-sm text-gray-600">Notificaciones</label>
                <label class="relative inline-flex items-center cursor-pointer">
                    <input type="checkbox" v-model="form.notificaciones" class="sr-only peer" @change="onChange" />
                    <div class="w-11 h-6 bg-gray-300 rounded-full peer peer-checked:bg-blue-600 transition-colors">
                    </div>
                    <div
                        class="absolute left-1 top-1 w-4 h-4 bg-white rounded-full transition-transform peer-checked:translate-x-5">
                    </div>
                </label>
            </div>
        </div>

        <!-- Botón guardar -->
        <button type="button" :disabled="!hasChanges"
            class="w-full bg-blue-600 text-white font-medium py-2 rounded mt-10 transition"
            :class="{ 'opacity-50 cursor-not-allowed': !hasChanges, 'hover:bg-blue-700': hasChanges }"
            @click="guardarCambios">
            Guardar cambios
        </button>
        <!-- Toast tipo iOS -->
        <transition name="fade">
            <div v-if="showSuccess"
                class="fixed bottom-20 left-1/2 -translate-x-1/2 z-50 rounded-xl bg-green-500 text-white px-5 py-3 shadow-lg text-sm font-medium">
                ✅ Cambios guardados correctamente.
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
            // Solo los campos editables, sin notificaciones
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
        onChange() {
            // Marca cambio al tocar el switch
        }
    },
    mounted() {
        this.original = { ...this.form }
    }
}
</script>
