<template>
    <div class="nueva-reserva-wrapper">
        <!-- Botón de cerrar -->
        <div class="absolute top-4 right-4 z-10">
            <div @click="onCerrarClick" class="text-gray-500 hover:text-red-500 transition-colors">
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
                <h2 class="text-lg font-semibold text-gray-800">Está seguro que desea salir?</h2>
                <p class="text-sm text-gray-600">Se perderán los datos seleccionados.</p>
                <div class="flex justify-center gap-3 pt-2">
                    <button @click="mostrarModalConfirmacion = false"
                        class="text-sm px-4 py-2 bg-gray-200 rounded hover:bg-gray-300 text-gray-800">Cancelar</button>
                    <button @click="volverAInicio"
                        class="text-sm px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700">Salir</button>
                </div>
            </div>
        </div>


        <div class="w-full sm:px-2 pt-6 pb-20">
            <!-- Datos del complejo -->
            <div class="mb-6">
                <h1 class="text-2xl font-bold text-gray-900">{{ complejo.nombre }}</h1>
                <p class="text-base text-gray-500">{{ complejo.direccion }}</p>
            </div>

            <div v-if="loaded && rawHorariosData.length === 0" class="my-10 text-center px-4">
                <div class="bg-gray-50 rounded-2xl p-8 border-2 border-dashed border-gray-200">
                    <div class="w-16 h-16 bg-red-50 rounded-full flex items-center justify-center mx-auto mb-4">
                        <i class="fas fa-calendar-times text-red-400 text-2xl"></i>
                    </div>
                    <h4 class="text-gray-800 font-bold mb-1">Complejo sin disponibilidad</h4>
                    <p class="text-gray-500 text-sm">Este complejo no tiene horarios cargados por el momento. Intentá
                        con otro complejo cercano.</p>
                    <button @click="$router.push({ name: 'Home' })"
                        class="mt-4 text-blue-600 font-semibold text-sm hover:underline">
                        Volver al inicio
                    </button>
                </div>
            </div>

            <template v-else-if="loaded">
                <!-- Selector de días -->
                <div v-if="diasDisponibles.length" class="mb-6">
                    <h3 class="text-base font-semibold text-gray-700 mb-2">Elegí un día</h3>
                    <div class="flex gap-3 overflow-x-auto pb-2 no-scrollbar">
                        <button v-for="(dia, index) in diasDisponibles" :key="index"
                            @click="seleccionarDia(dia.fechaExacta)" :class="[
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
                <div v-if="deportesFiltradosPorDia.length" class="mb-6">
                    <h3 class="text-base font-semibold text-gray-700 mb-2">Canchas disponibles:</h3>
                    <div class="flex gap-2 overflow-x-auto pb-2 no-scrollbar">
                        <button v-for="(deporte, index) in deportesFiltradosPorDia" :key="index"
                            @click="seleccionarDeporte(deporte.tipo)" :class="[
                                'flex items-center gap-2 text-sm px-4 py-2 rounded-full whitespace-nowrap transition-colors duration-200',
                                deporte.tipo === deporteSeleccionado ? 'bg-blue-600 text-white' : 'bg-gray-200 text-gray-800'
                            ]">
                            <i :class="deporte.icono" class="text-base"></i>
                            {{ deporte.nombre }}
                        </button>
                    </div>
                </div>

                <!-- Horarios disponibles -->
                <div v-if="horarios.length" class="mb-6">
                    <h3 class="text-base font-semibold text-gray-700 mb-2">Horarios disponibles</h3>
                    <div class="flex flex-col gap-3">
                        <div v-for="(hora, i) in horarios" :key="i"
                            class="w-full h-[72px] px-4 py-3 rounded-xl bg-white shadow border border-gray-200 text-sm flex justify-between items-center transition-all hover:border-blue-300 cursor-pointer group">
                            <span class="font-medium text-gray-700">{{ hora.rango }}</span>
                            <div class="flex items-center gap-2">
                                <span
                                    class="text-blue-600 font-semibold text-xs bg-blue-50 px-2 py-1 rounded">Disponible</span>
                                <i
                                    class="fas fa-chevron-right text-gray-300 group-hover:text-blue-400 transition-colors"></i>
                            </div>
                        </div>
                    </div>
                </div>
                <div v-else class="my-10 text-center px-4">
                    <div class="bg-gray-50 rounded-2xl p-8 border-2 border-dashed border-gray-200">
                        <div class="w-16 h-16 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
                            <i class="fas fa-clock text-gray-400 text-2xl"></i>
                        </div>
                        <h4 class="text-gray-800 font-bold mb-1">No hay horarios disponibles</h4>
                        <p class="text-gray-500 text-sm">Probá seleccionando otro día o deporte para ver más opciones.
                        </p>
                    </div>
                </div>
            </template>

            <!-- Skeleton Loading -->
            <div v-else class="my-10 text-center px-4 flex flex-col items-center">
                <div class="animate-pulse flex flex-col items-center w-full">
                    <div class="h-4 bg-gray-200 rounded w-1/2 mb-4"></div>
                    <div class="space-y-3 w-full">
                        <div class="h-20 bg-gray-100 rounded-xl" v-for="n in 3" :key="n"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>

<script>
import { API_ENDPOINTS } from '../config/apiConfig';
import { startLoader, stopLoader } from '../services/globalLoader';

export default {
    name: 'NuevaReserva',
    data() {
        return {
            mostrarModalConfirmacion: false,
            nextRoute: null,
            salidaConfirmada: false,
            deporteSeleccionado: 'futbol5',
            complejo: {
                id: null,
                nombre: '',
                direccion: '',
                imagen: ''
            },
            deportesDisponibles: [
                { tipo: 'futbol5', nombre: 'Fútbol 5', icono: 'fas fa-futbol' },
                { tipo: 'futbol7', nombre: 'Fútbol 7', icono: 'fas fa-futbol' },
                { tipo: 'futbol11', nombre: 'Fútbol 11', icono: 'fas fa-futbol' },
                { tipo: 'padel', nombre: 'Pádel', icono: 'fas fa-table-tennis' }
            ],
            diasDisponibles: [],
            diaSeleccionado: null,
            rawHorariosData: [],
            loaded: false
        }
    },
    computed: {
        horarios() {
            if (!this.diaSeleccionado || !this.rawHorariosData.length) return [];
            const dayData = this.rawHorariosData.find(d => d.fecha.startsWith(this.diaSeleccionado));
            if (!dayData) return [];

            // Filtramos por el deporte seleccionado. 
            // Normalizamos nombres (sacamos espacios y pasamos a lowercase para comparar)
            return dayData.horarios
                .filter(h => {
                    const normalizedApiSport = h.tipoCancha.toLowerCase().replace(/ /g, '');
                    return normalizedApiSport === this.deporteSeleccionado.toLowerCase();
                })
                .map(h => ({
                    ...h,
                    rango: `${h.horaInicio.substring(0, 5)} a ${h.horaFin.substring(0, 5)}hs`
                }));
        },
        deportesFiltradosPorDia() {
            if (!this.diaSeleccionado || !this.rawHorariosData.length) return [];
            const dayData = this.rawHorariosData.find(d => d.fecha.startsWith(this.diaSeleccionado));
            if (!dayData) return [];

            // Obtenemos los deportes que realmente tienen horarios este día
            const deportesConHorario = [...new Set(dayData.horarios.map(h => h.tipoCancha.toLowerCase().replace(/ /g, '')))];

            return this.deportesDisponibles.filter(d => deportesConHorario.includes(d.tipo.toLowerCase()));
        }
    },
    created() {
        const complejoId = this.$route.query.id || 1; // Usamos 1 como fallback si no hay ID
        this.fetchHorarios(complejoId);
    },
    methods: {
        async fetchHorarios(complejoId) {
            startLoader();
            try {
                const response = await fetch(API_ENDPOINTS.reservas.getHorariosDisponibles(complejoId));
                if (!response.ok) throw new Error('Error al cargar horarios');

                const data = await response.json();
                this.rawHorariosData = data.horariosPorDia;

                // Actualizamos los datos del complejo con la info real de la API
                if (data.complejo) {
                    this.complejo = data.complejo;
                }

                // Generamos los días disponibles para el selector
                this.diasDisponibles = this.rawHorariosData.map(d => {
                    const date = new Date(d.fecha);
                    const nombresDias = ['domingo', 'lunes', 'martes', 'miércoles', 'jueves', 'viernes', 'sábado'];
                    return {
                        dia: nombresDias[date.getUTCDay()],
                        fecha: `${date.getUTCDate()}/${date.getUTCMonth() + 1}`,
                        fechaExacta: d.fecha.split('T')[0]
                    };
                });

                if (this.diasDisponibles.length > 0) {
                    this.diaSeleccionado = this.diasDisponibles[0].fechaExacta;
                }
            } catch (error) {
                console.error("Error fetching schedules:", error);
            } finally {
                this.loaded = true;
                stopLoader();
            }
        },
        seleccionarDia(fecha) {
            this.diaSeleccionado = fecha
        },
        seleccionarDeporte(tipo) {
            this.deporteSeleccionado = tipo;
        },
        volverAInicio() {
            this.mostrarModalConfirmacion = false
            this.salidaConfirmada = true
            this.$router.push(this.nextRoute)
        },
        onCerrarClick() {
            this.mostrarModalConfirmacion = true
            this.nextRoute = { name: 'Home' }
        }

    },
    beforeRouteLeave(to, from, next) {
        if (this.salidaConfirmada) {
            next()
            return
        }
        this.mostrarModalConfirmacion = true
        this.nextRoute = {
            name: to.name,
            params: to.params,
            query: to.query
        }
        next(false)
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
