<template>
    <div class="p-4 min-h-screen text-[#101518]" style="font-family: Inter, 'Noto Sans', sans-serif;">
        <!-- Título -->
        <h1 class="text-4xl font-bold text-left mb-7">Configuración</h1>

        <!-- Campos -->
        <div class="space-y-6">
            <!-- Campo reusable -->
            <div v-for="(valor, label) in campos" :key="label">
                <label class="block text-sm text-gray-600 mb-1">{{ label }}</label>
                <div class="relative">
                    <input type="text" :value="valor" readonly
                        class="w-full border border-gray-300 rounded pl-3 pr-10 py-2 bg-white text-[#101518] focus:outline-none" />
                    <div type="button"
                        class="absolute right-2 top-1/2 -translate-y-1/2 p-1 rounded-full hover:bg-gray-200 transition">
                        <i class="fas fa-pen text-xs text-gray-500"></i>
                    </div>
                </div>
            </div>

            <!-- Notificaciones -->
            <div class="flex items-center justify-between pt-2">
                <label class="text-sm text-gray-600">Notificaciones</label>
                <label class="relative inline-flex items-center cursor-pointer">
                    <input type="checkbox" v-model="form.notificaciones" class="sr-only peer">
                    <div class="w-11 h-6 bg-gray-300 rounded-full peer peer-checked:bg-blue-600 transition-colors">
                    </div>
                    <div
                        class="absolute left-1 top-1 w-4 h-4 bg-white rounded-full transition-transform peer-checked:translate-x-5">
                    </div>
                </label>
            </div>
        </div>
        <!-- Botón guardar -->
        <button type="button"
            class="w-full bg-blue-600 text-white font-medium py-2 rounded mt-10 hover:bg-blue-700 transition">
            Guardar cambios
        </button>
    </div>
</template>

<script>
export default {
    name: 'ConfigUser',
    data() {
        return {
            form: {
                notificaciones: true,
                // otros campos...
            },
            campos: {
                'Teléfono': '+54 9 11 2345-6789',
                'Contraseña': '********',
                'Ciudad': 'Viedma'
            }
        }
    }
}
</script>
