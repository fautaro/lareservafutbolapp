<template>
    <!-- Botón de cerrar -->
    <div class="absolute top-4 right-4 z-10">
        <div @click="mostrarModalConfirmacion = true" class="text-gray-500 hover:text-red-500 transition-colors">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24"
                stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
        </div>
    </div>

    <!-- Modal de confirmación -->
    <div v-if="mostrarModalConfirmacion"
        class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-[999]">
        <div class="bg-white rounded-xl p-6 mx-4 max-w-sm w-full shadow-xl text-center space-y-4">
            <h2 class="text-lg font-semibold text-gray-800">¿Volver a la pantalla principal?</h2>
            <p class="text-sm text-gray-600">Se perderán los datos seleccionados.</p>
            <div class="flex justify-center gap-3 pt-2">
                <button @click="mostrarModalConfirmacion = false"
                    class="text-sm px-4 py-2 bg-gray-200 rounded hover:bg-gray-300 text-gray-800">Cancelar</button>
                <button @click="volverAInicio"
                    class="text-sm px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700">Salir</button>
            </div>
        </div>
    </div>


    <div class="px-4 pt-6 pb-28">
        <!-- Datos del complejo -->
        <div class="mb-6">
            <h1 class="text-2xl font-bold text-gray-900">{{ complejo.nombre }}</h1>
            <p class="text-base text-gray-500">{{ complejo.direccion }}</p>
        </div>


        <!-- Selector de días -->
        <div class="mb-6">
            <h3 class="text-base font-semibold text-gray-700 mb-2">Elegí un día</h3>
            <div class="flex gap-3 overflow-x-auto pb-2 no-scrollbar">
                <button v-for="(dia, index) in diasDisponibles" :key="index" @click="seleccionarDia(dia.fechaExacta)"
                    :class="[
                        'w-[110px] h-[80px] flex-shrink-0 flex flex-col items-center justify-center rounded-xl text-center text-base font-semibold shadow-md p-3 transition-colors duration-200',
                        dia.fechaExacta === diaSeleccionado ? 'bg-blue-600 text-white' : 'bg-gray-200 text-black'
                    ]">
                    <div class="text-sm leading-tight capitalize">
                        {{ dia.dia }}
                    </div>
                    <div class="text-sm leading-tight">
                        {{ dia.fecha }}
                    </div>
                </button>
            </div>
        </div>


        <!-- Selector de deportes -->
        <div class="mb-6">
            <h3 class="text-base font-semibold text-gray-700 mb-2">Canchas disponibles:</h3>
            <div class="flex gap-2 overflow-x-auto pb-2 no-scrollbar">
                <button v-for="(deporte, index) in deportesDisponibles" :key="index"
                    @click="seleccionarDeporte(deporte.tipo)" :class="[
                        'text-sm px-4 py-2 rounded-full whitespace-nowrap transition-colors duration-200',
                        deporte.tipo === deporteSeleccionado ? 'bg-blue-600 text-white' : 'bg-gray-200 text-gray-800'
                    ]">
                    {{ deporte.nombre }}
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
        <div v-else class="text-sm text-center text-gray-500 mb-6">
            No hay canchas disponibles para los horarios seleccionados.
        </div>
    </div>
</template>

<script>
export default {
    name: 'NuevaReserva',
    data() {
        return {
            mostrarModalConfirmacion: false,
            deporteSeleccionado: 'futbol5',
            complejo: {
                id: 1,
                nombre: 'Tercer Tiempo',
                direccion: 'Del Laurel 236, Viedma',
                imagen: 'https://lh3.googleusercontent.com/p/AF1QipM9N9YYOEDzYw3ejjlq9tdIhq0KHZsmb2EFNFW1=w426-h240-k-no'
            },
            deportesDisponibles: [
                { tipo: 'futbol5', nombre: 'Fútbol 5' },
                { tipo: 'futbol7', nombre: 'Fútbol 7' },
                { tipo: 'futbol11', nombre: 'Fútbol 11' },
                { tipo: 'padel', nombre: 'Pádel' }
            ],
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
                '2025-07-03': {
                    futbol5: [{ rango: '13 a 14hs' }, { rango: '14 a 15hs' }],
                    futbol7: [{ rango: '15 a 16hs' }]
                },
                '2025-07-04': {
                    futbol5: [{ rango: '16 a 17hs' }],
                    padel: [{ rango: '17 a 18hs' }]
                }
            }
        }
    },
    computed: {
        horarios() {
            const horariosDelDia = this.horariosPorDia[this.diaSeleccionado] || {};
            return horariosDelDia[this.deporteSeleccionado] || [];
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
        },
        seleccionarDeporte(tipo) {
            this.deporteSeleccionado = tipo;
        },
        volverAInicio() {
            this.mostrarModalConfirmacion = false;
            this.$router.push('/');
        },
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
