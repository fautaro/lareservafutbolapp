<template>
    <div class="px-4 pt-6 pb-28">
        <!-- Datos del complejo -->
        <div class="mb-6">
            <h2 class="text-lg font-semibold text-gray-800">{{ complejo.nombre }}</h2>
            <p class="text-sm text-gray-600">{{ complejo.direccion }}</p>
        </div>

        <!-- Selector de días -->
        <div class="mb-6">
            <h3 class="text-base font-semibold text-gray-700 mb-2">Elegí un día</h3>
            <div class="flex gap-3 overflow-x-auto pb-2 no-scrollbar">
                <button v-for="(dia, index) in diasDisponibles" :key="index" @click="seleccionarDia(dia.fechaExacta)"
                    :class="[
                        'w-[110px] h-[80px] flex-shrink-0 flex flex-col items-center justify-center rounded-xl text-center border font-medium transition-colors duration-200',
                        dia.fechaExacta === diaSeleccionado
                            ? 'bg-green-500 text-white border-green-600'
                            : 'bg-white text-gray-700 border-gray-300'
                    ]">
                    <div class="text-sm font-semibold leading-tight capitalize">
                        {{ dia.dia }}
                    </div>
                    <div class="text-sm leading-tight">
                        {{ dia.fecha }}
                    </div>
                </button>
            </div>
        </div>



        <!-- Horarios disponibles -->
        <div v-if="horarios.length" class="mb-6">
            <h3 class="text-base font-semibold text-gray-700 mb-2">Horarios disponibles</h3>
            <div class="flex flex-col gap-3">
                <div v-for="(hora, i) in horarios" :key="i"
                    class="w-full h-[72px] px-4 py-3 rounded-xl bg-white shadow border border-gray-200 text-sm flex justify-between items-center">
                    <span class="font-medium text-gray-700">{{ hora.rango }}</span>
                    <span class="text-blue-600 font-semibold text-xs">Disponible</span>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: 'NuevaReserva',
    data() {
        return {
            complejo: {
                id: 1,
                nombre: 'Tercer Tiempo',
                direccion: 'Del Laurel 236, Viedma',
                imagen: 'https://lh3.googleusercontent.com/p/AF1QipM9N9YYOEDzYw3ejjlq9tdIhq0KHZsmb2EFNFW1=w426-h240-k-no'
            },
            diasDisponibles: [
                { dia: 'jueves', fecha: '03-07', fechaExacta: '2025-07-03' },
                { dia: 'viernes', fecha: '04-07', fechaExacta: '2025-07-04' },
                { dia: 'sábado', fecha: '05-07', fechaExacta: '2025-07-05' },
                { dia: 'domingo', fecha: '06-07', fechaExacta: '2025-07-06' },
                { dia: 'lunes', fecha: '07-07', fechaExacta: '2025-07-07' },
                { dia: 'martes', fecha: '08-07', fechaExacta: '2025-07-08' },
                { dia: 'miércoles', fecha: '09-07', fechaExacta: '2025-07-09' }
            ],
            diaSeleccionado: '2025-07-03',
            horariosPorDia: {
                '2025-07-03': [{ rango: '13 a 14hs' }, { rango: '14 a 15hs' }],
                '2025-07-04': [{ rango: '16 a 17hs' }, { rango: '17 a 18hs' }],
                '2025-07-05': [{ rango: '15 a 16hs' }]
            }
        }
    },
    computed: {
        horarios() {
            return this.horariosPorDia[this.diaSeleccionado] || []
        }
    },
    created() {
        const complejoId = this.$route.query.id
        console.log(complejoId)

        // Simulación: podrías llamar a una API con este ID
        // this.cargarDatosDelComplejo(complejoId)
    },
    methods: {
        seleccionarDia(fecha) {
            this.diaSeleccionado = fecha
        }
    }
}
</script>

<style scoped>
.no-scrollbar::-webkit-scrollbar {
    display: none;
}

.no-scrollbar {
    -ms-overflow-style: none;
    scrollbar-width: none;
}
</style>
