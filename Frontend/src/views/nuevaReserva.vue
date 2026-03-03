<template>
    <div class="nueva-reserva-wrapper text-[#101518]" style="font-family: Inter, 'Noto Sans', sans-serif;">
        <!-- Botón de cerrar -->
        <div class="absolute top-4 right-4 z-10">
            <div @click="onCerrarClick" class="text-gray-500 hover:text-red-500 transition-colors">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24"
                    stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                </svg>
            </div>
        </div>
        <!-- Modal de confirmación al salir -->
        <div v-if="mostrarModalConfirmacion"
            class="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-[999] p-4">
            <div class="bg-white rounded-2xl p-6 max-w-sm w-full shadow-2xl text-center space-y-4 animate-slide-up">
                <div class="w-16 h-16 bg-red-50 rounded-full flex items-center justify-center mx-auto text-red-500">
                    <i class="fas fa-exclamation-triangle text-2xl"></i>
                </div>
                <h2 class="text-xl font-bold text-gray-800">¿Querés salir?</h2>
                <p class="text-sm text-gray-500">Se perderán los datos de tu reserva actual.</p>
                <div class="flex gap-3 pt-2">
                    <button @click="mostrarModalConfirmacion = false"
                        class="flex-1 py-3 bg-gray-100 rounded-xl font-semibold text-gray-600 hover:bg-gray-200 transition-colors">Volver</button>
                    <button @click="volverAInicio"
                        class="flex-1 py-3 bg-red-600 text-white rounded-xl font-bold hover:bg-red-700 transition-colors">Salir</button>
                </div>
            </div>
        </div>

        <!-- Pantalla de Éxito (Estilo PedidosYa) -->
        <Transition name="fade">
            <div v-if="reservaExitosa"
                class="fixed inset-0 z-[2000] bg-white flex flex-col items-center justify-center p-6 text-center">
                <div class="mb-6 relative">
                    <div
                        class="w-32 h-32 bg-green-100 rounded-full flex items-center justify-center text-green-500 animate-bounce-short">
                        <i class="fas fa-check text-6xl"></i>
                    </div>
                    <div class="absolute inset-0 w-32 h-32 bg-green-400 rounded-full opacity-0 animate-ping-once"></div>
                </div>
                <h2 class="text-3xl font-black text-gray-900 mb-2">¡Reserva confirmada!</h2>
                <p class="text-gray-500 text-lg mb-8 max-w-xs">Tu turno ha sido agendado con éxito. Ya podés verlo en
                    tus
                    reservas.</p>
                <div class="w-full max-w-xs space-y-4">
                    <div class="bg-gray-50 rounded-2xl p-4 border border-gray-100 flex flex-col gap-1 items-start">
                        <span class="text-[10px] uppercase font-black text-gray-400 tracking-widest">Complejo</span>
                        <span class="text-sm font-bold text-gray-800">{{ complejo.nombre }}</span>
                    </div>
                    <button @click="$router.push({ name: 'MisReservas' })"
                        class="w-full py-4 bg-green-500 hover:bg-green-600 text-white font-bold rounded-2xl shadow-xl shadow-green-100 transition-all transform active:scale-95">
                        VER MIS RESERVAS
                    </button>
                </div>
            </div>
        </Transition>

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
                    <h3 class="text-base font-semibold text-gray-700 mb-2">Elegí el deporte:</h3>
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

                <!-- Selector de Canchas -->
                <div v-if="canchasFiltradas.length" class="mb-6">
                    <h3 class="text-base font-semibold text-gray-700 mb-2">Elegí la cancha:</h3>
                    <div class="flex gap-3 overflow-x-auto pb-2 no-scrollbar flex-nowrap">
                        <button v-for="cancha in canchasFiltradas" :key="cancha.id"
                            @click="seleccionarCancha(cancha.id)" :class="[
                                'min-w-[140px] px-4 py-3 rounded-xl text-sm font-bold border-2 transition-all duration-200 whitespace-nowrap text-center flex-shrink-0',
                                cancha.id === canchaSeleccionadaId ? 'border-blue-600 bg-blue-50 text-blue-700 shadow-sm' : 'border-gray-200 bg-white text-gray-600'
                            ]">
                            {{ cancha.nombre }}
                        </button>
                    </div>
                </div>

                <!-- Selector de Medio de Pago -->
                <div v-if="mediosPago.length" class="mb-6">
                    <h3 class="text-base font-semibold text-gray-700 mb-2">Elegí el medio de pago:</h3>
                    <div class="flex gap-3 overflow-x-auto pb-2 no-scrollbar flex-nowrap">
                        <button v-for="mp in mediosPago" :key="mp.id" @click="medioPagoSeleccionadoId = mp.id" :class="[
                            'min-w-[120px] px-4 py-3 rounded-xl text-sm font-bold border-2 transition-all duration-200 whitespace-nowrap text-center flex-shrink-0 flex flex-col items-center gap-1',
                            mp.id === medioPagoSeleccionadoId ? 'border-blue-600 bg-blue-50 text-blue-700 shadow-sm' : 'border-gray-200 bg-white text-gray-600'
                        ]">
                            <i :class="mp.icono || 'fas fa-wallet'" class="text-base mb-1"></i>
                            {{ mp.nombre }}
                        </button>
                    </div>
                </div>

                <!-- Horarios disponibles -->
                <div v-if="horarios.length" class="mb-6">
                    <h3 class="text-base font-semibold text-gray-700 mb-2">Horarios disponibles</h3>
                    <div class="flex flex-col gap-3">
                        <div v-for="(hora, i) in horarios" :key="i" @click="preConfirmarReserva(hora)"
                            class="w-full h-20 px-5 py-4 rounded-2xl bg-white shadow-sm border border-gray-100 flex justify-between items-center transition-all hover:border-blue-400 hover:shadow-md cursor-pointer group active:scale-[0.98]">
                            <div class="flex flex-col">
                                <span class="text-base font-bold text-gray-900 leading-tight">{{ hora.rango }}</span>
                                <span class="text-xs text-blue-500 font-semibold mt-0.5">Disponible</span>
                            </div>
                            <div class="flex items-center gap-4">
                                <div class="text-right flex flex-col">
                                    <span class="text-lg font-bold text-[#2D9CDB]">${{
                                        hora.precioHora?.toLocaleString('es-AR') }}</span>
                                    <span class="text-[10px] text-gray-400 uppercase font-bold tracking-tighter">por
                                        turno</span>
                                </div>
                                <div
                                    class="w-8 h-8 rounded-full bg-gray-50 flex items-center justify-center group-hover:bg-blue-50 transition-colors">
                                    <i
                                        class="fas fa-chevron-right text-gray-300 group-hover:text-[#2D9CDB] text-xs"></i>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Mensaje cuando no hay horarios en la cancha seleccionada -->
                <div v-else-if="loaded && canchaSeleccionadaId" class="my-10 text-center px-4">
                    <div class="bg-gray-50 rounded-2xl p-8 border-2 border-dashed border-gray-200">
                        <div class="w-16 h-16 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
                            <i class="fas fa-clock text-gray-400 text-2xl"></i>
                        </div>
                        <h4 class="text-gray-800 font-bold mb-1">No hay horarios disponibles</h4>
                        <p class="text-gray-500 text-sm">Probá seleccionando otro día o deporte para ver más opciones.
                        </p>
                    </div>
                </div>

                <!-- Modal de Pre-confirmación de Reserva -->
                <div v-if="mostrarConfirmacionReserva"
                    class="fixed inset-0 bg-black/60 backdrop-blur-sm flex items-end sm:items-center justify-center z-[1000] p-0 sm:p-4 transition-all duration-300">
                    <div
                        class="bg-white w-full max-w-md rounded-t-3xl sm:rounded-3xl shadow-2xl overflow-hidden transform transition-all animate-slide-up">
                        <!-- Header Card -->
                        <div class="bg-blue-600 p-5 text-white relative">
                            <button @click="mostrarConfirmacionReserva = false"
                                class="absolute top-4 right-4 text-white/80 hover:text-white">
                                <i class="fas fa-times text-xl"></i>
                            </button>
                            <h2 class="text-xl font-bold mb-0.5">Confirmar Turno</h2>
                            <p class="text-blue-100 text-xs">Casi listo, revisá los datos de tu reserva</p>
                        </div>

                        <!-- Detalles -->
                        <div class="p-5 space-y-4">
                            <!-- Sección Complejo (Centrada) -->
                            <div class="flex flex-col items-center text-center">
                                <div
                                    class="w-12 h-12 bg-blue-50 rounded-xl flex items-center justify-center text-blue-600 shadow-sm mb-2">
                                    <i class="fas fa-building text-xl"></i>
                                </div>
                                <h3 class="text-xl font-black text-gray-900 leading-tight mb-0.5">{{ complejo.nombre }}
                                </h3>
                                <p class="text-sm text-gray-500 px-4">{{ complejo.direccion }}</p>
                            </div>

                            <!-- Divisor sutil -->
                            <div class="border-t border-gray-100 w-full"></div>

                            <!-- Otros Detalles (En una sola línea) -->
                            <div class="space-y-2 pt-2">
                                <!-- Fecha -->
                                <div class="flex items-center gap-3 py-2 border-b border-gray-50">
                                    <div
                                        class="w-9 h-9 bg-emerald-50 rounded-lg flex items-center justify-center text-emerald-600 flex-shrink-0">
                                        <i class="fas fa-calendar-day text-xs"></i>
                                    </div>
                                    <div class="flex-1 flex justify-between items-center">
                                        <span
                                            class="text-xs text-gray-400 font-extrabold uppercase tracking-widest">Fecha</span>
                                        <span class="font-bold text-gray-800 text-base capitalize">{{
                                            formattedSelectedDate }}</span>
                                    </div>
                                </div>

                                <!-- Horario -->
                                <div class="flex items-center gap-3 py-2 border-b border-gray-50">
                                    <div
                                        class="w-9 h-9 bg-indigo-50 rounded-lg flex items-center justify-center text-indigo-600 flex-shrink-0">
                                        <i class="fas fa-clock text-xs"></i>
                                    </div>
                                    <div class="flex-1 flex justify-between items-center">
                                        <span
                                            class="text-xs text-gray-400 font-extrabold uppercase tracking-widest">Horario</span>
                                        <span class="font-bold text-indigo-700 text-base">{{ horarioSeleccionado?.rango
                                        }}</span>
                                    </div>
                                </div>

                                <!-- Cancha -->
                                <div class="flex items-center gap-3 py-2 border-b border-gray-50">
                                    <div
                                        class="w-9 h-9 bg-amber-50 rounded-lg flex items-center justify-center text-amber-600 flex-shrink-0">
                                        <i :class="getDeporteIcono()" class="text-xs"></i>
                                    </div>
                                    <div class="flex-1 flex justify-between items-center">
                                        <span
                                            class="text-xs text-gray-400 font-extrabold uppercase tracking-widest">Cancha</span>
                                        <span class="font-bold text-gray-800 text-base">{{ getFullCanchaNombre()
                                        }}</span>
                                    </div>
                                </div>

                                <!-- Precio -->
                                <div class="flex items-center gap-3 py-2 border-b border-gray-50">
                                    <div
                                        class="w-9 h-9 bg-red-50 rounded-lg flex items-center justify-center text-red-600 flex-shrink-0">
                                        <i class="fas fa-money-bill-wave text-xs"></i>
                                    </div>
                                    <div class="flex-1 flex justify-between items-center">
                                        <span
                                            class="text-xs text-gray-400 font-extrabold uppercase tracking-widest">Precio</span>
                                        <span class="font-bold text-gray-900 text-base">${{
                                            horarioSeleccionado?.precioHora?.toLocaleString('es-AR') }}</span>
                                    </div>
                                </div>

                                <!-- Medio de Pago Summary -->
                                <div class="flex items-center gap-3 py-2">
                                    <div
                                        class="w-9 h-9 bg-purple-50 rounded-lg flex items-center justify-center text-purple-600 flex-shrink-0">
                                        <i :class="getMedioPagoIcono()" class="text-xs"></i>
                                    </div>
                                    <div class="flex-1 flex justify-between items-center">
                                        <span
                                            class="text-xs text-gray-400 font-extrabold uppercase tracking-widest">Pago</span>
                                        <span class="font-bold text-gray-800 text-base">{{ getMedioPagoNombre()
                                            }}</span>
                                    </div>
                                </div>
                            </div>

                            <!-- Aviso -->
                            <div class="bg-amber-50 border border-amber-100 rounded-xl p-3 flex gap-3">
                                <i class="fas fa-info-circle text-amber-500 mt-0.5 text-xs"></i>
                                <p class="text-xs text-amber-800 leading-relaxed font-medium">
                                    Al confirmar, tu solicitud se enviará al complejo. Podrás ver el estado en la
                                    sección 'Mis Reservas'.
                                </p>
                            </div>

                            <!-- Botones -->
                            <div class="flex flex-col gap-2 pt-1">
                                <button @click="confirmarReservaFinal"
                                    class="w-full py-3.5 bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-xl shadow-lg transition-all active:scale-95">
                                    CONFIRMAR RESERVA
                                </button>
                                <button @click="mostrarConfirmacionReserva = false"
                                    class="w-full py-2.5 bg-white text-gray-500 font-semibold rounded-xl border border-gray-100 hover:bg-gray-50 transition-all text-sm">
                                    Cancelar
                                </button>
                            </div>
                        </div>
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
import { useAuthUser } from '../composables/useAuthUser';

export default {
    name: 'NuevaReserva',
    setup() {
        const { user } = useAuthUser();
        return { user };
    },
    data() {
        return {
            mostrarModalConfirmacion: false,
            mostrarConfirmacionReserva: false,
            horarioSeleccionado: null,
            nextRoute: null,
            salidaConfirmada: false,
            deporteSeleccionado: 'futbol5',
            canchaSeleccionadaId: null,
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
            mediosPago: [],
            medioPagoSeleccionadoId: null,
            loaded: false,
            reservaExitosa: false
        }
    },
    computed: {
        horarios() {
            if (!this.diaSeleccionado || !this.rawHorariosData.length || !this.canchaSeleccionadaId) return [];
            const dayData = this.rawHorariosData.find(d => d.fecha.startsWith(this.diaSeleccionado));
            if (!dayData) return [];

            return dayData.horarios
                .filter(h => h.canchaId === this.canchaSeleccionadaId)
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
        },
        canchasFiltradas() {
            if (!this.diaSeleccionado || !this.deporteSeleccionado || !this.rawHorariosData.length) return [];
            const dayData = this.rawHorariosData.find(d => d.fecha.startsWith(this.diaSeleccionado));
            if (!dayData) return [];

            const canchasMap = new Map();
            dayData.horarios.forEach(h => {
                const normalizedApiSport = h.tipoCancha.toLowerCase().replace(/ /g, '');
                if (normalizedApiSport === this.deporteSeleccionado.toLowerCase()) {
                    canchasMap.set(h.canchaId, h.canchaNombre);
                }
            });

            return Array.from(canchasMap, ([id, nombre]) => ({ id, nombre }));
        },
        formattedSelectedDate() {
            if (!this.diaSeleccionado) return '';
            const date = new Date(this.diaSeleccionado + 'T12:00:00'); // Use mid-day to avoid TZ issues
            const options = { weekday: 'long', day: 'numeric', month: 'long' };
            return date.toLocaleDateString('es-AR', options);
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
                    this.resetSelection();
                }

                // Fetch Medios de Pago
                const mpResponse = await fetch(API_ENDPOINTS.medioPagos.getAll());
                if (mpResponse.ok) {
                    this.mediosPago = await mpResponse.json();
                    if (this.mediosPago.length > 0) {
                        this.medioPagoSeleccionadoId = this.mediosPago[0].id;
                    }
                }
            } catch (error) {
                console.error("Error fetching schedules:", error);
            } finally {
                this.loaded = true;
                stopLoader();
            }
        },
        seleccionarDia(fecha) {
            this.diaSeleccionado = fecha;
            this.resetSelection();
        },
        seleccionarDeporte(tipo) {
            this.deporteSeleccionado = tipo;
            this.canchaSeleccionadaId = null;
            // Seleccionamos la primera cancha disponible automáticamente
            this.$nextTick(() => {
                if (this.canchasFiltradas.length > 0) {
                    this.canchaSeleccionadaId = this.canchasFiltradas[0].id;
                }
            });
        },
        seleccionarCancha(id) {
            this.canchaSeleccionadaId = id;
        },
        resetSelection() {
            this.canchaSeleccionadaId = null;
            if (this.deportesFiltradosPorDia.length > 0) {
                this.deporteSeleccionado = this.deportesFiltradosPorDia[0].tipo;
                this.$nextTick(() => {
                    if (this.canchasFiltradas.length > 0) {
                        this.canchaSeleccionadaId = this.canchasFiltradas[0].id;
                    }
                });
            }
        },
        volverAInicio() {
            this.mostrarModalConfirmacion = false
            this.salidaConfirmada = true
            this.$router.push(this.nextRoute)
        },
        onCerrarClick() {
            this.mostrarModalConfirmacion = true
            this.nextRoute = { name: 'Home' }
        },
        preConfirmarReserva(hora) {
            this.horarioSeleccionado = hora;
            this.mostrarConfirmacionReserva = true;
        },
        getDeporteNombre() {
            return this.deportesDisponibles.find(d => d.tipo === this.deporteSeleccionado)?.nombre || 'Cancha';
        },
        getDeporteIcono() {
            return this.deportesDisponibles.find(d => d.tipo === this.deporteSeleccionado)?.icono || 'fas fa-futbol';
        },
        getFullCanchaNombre() {
            const cancha = this.canchasFiltradas.find(c => c.id === this.canchaSeleccionadaId);
            const deporte = this.getDeporteNombre();
            return cancha ? `${cancha.nombre} (${deporte})` : deporte;
        },
        getMedioPagoNombre() {
            return this.mediosPago.find(m => m.id === this.medioPagoSeleccionadoId)?.nombre || 'No seleccionado';
        },
        getMedioPagoIcono() {
            return this.mediosPago.find(m => m.id === this.medioPagoSeleccionadoId)?.icono || 'fas fa-wallet';
        },
        async confirmarReservaFinal() {
            startLoader();
            try {
                const reservaData = {
                    usuarioId: this.user?.sub || 1, // Fallback al id 1 para pruebas como UserProfile
                    complejoId: this.complejo.id,
                    canchaId: this.horarioSeleccionado.canchaId,
                    fecha: this.diaSeleccionado,
                    horaInicio: this.horarioSeleccionado.horaInicio,
                    horaFin: this.horarioSeleccionado.horaFin,
                    montoTotal: this.horarioSeleccionado.precioHora,
                    medioPagoId: this.medioPagoSeleccionadoId
                };

                const response = await fetch(API_ENDPOINTS.reservas.create, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(reservaData)
                });

                let result;
                try {
                    const contentType = response.headers.get("content-type");
                    if (contentType && contentType.indexOf("application/json") !== -1) {
                        result = await response.json();
                    } else {
                        // Si no es JSON (como un error 500 con texto), manejamos el error
                        const text = await response.text();
                        console.error("Respuesta no-JSON recibida:", text);
                        throw new Error("El servidor respondió con un error inesperado.");
                    }
                } catch (e) {
                    if (!response.ok && !result) {
                        throw new Error(`Error en el servidor (${response.status}).`);
                    }
                    throw e;
                }

                if (!response.ok) {
                    throw new Error(result?.message || 'Error al confirmar reserva');
                }

                // Si tiene éxito
                this.reservaExitosa = true;

                // Redirigir automáticamente después de unos segundos si el usuario no pulsa el botón
                setTimeout(() => {
                    if (this.reservaExitosa) {
                        this.salidaConfirmada = true;
                        this.$router.push({ name: 'MisReservas' });
                    }
                }, 4000);
            } catch (error) {
                console.error("Error al confirmar reserva:", error);
                alert(error.message || 'Hubo un error al procesar tu reserva. Intentalo de nuevo.');
            } finally {
                stopLoader();
                this.mostrarConfirmacionReserva = false;
            }
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

@keyframes slide-up {
    from {
        transform: translateY(100%);
        opacity: 0;
    }

    to {
        transform: translateY(0);
        opacity: 1;
    }
}

.animate-slide-up {
    animation: slide-up 0.4s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}

.animate-ping-once {
    animation: ping 1s cubic-bezier(0, 0, 0.2, 1) forwards;
}

@keyframes ping {

    75%,
    100% {
        transform: scale(2);
        opacity: 0;
    }
}

.animate-bounce-short {
    animation: bounce 0.5s ease-in-out;
}

@keyframes bounce {

    0%,
    100% {
        transform: translateY(0);
    }

    50% {
        transform: translateY(-10px);
    }
}

.fade-enter-active,
.fade-leave-active {
    transition: opacity 0.5s ease;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}
</style>
