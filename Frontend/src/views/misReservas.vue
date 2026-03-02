<style scoped>
.fade-enter-active,
.fade-leave-active {
    transition: opacity .25s ease;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}
</style>

<template>
    <div class="p-4">
        <h1 class="text-4xl font-bold mb-7">Reservas</h1>

        <h2 class="text-sm font-semibold text-gray-500 uppercase tracking-wide text-left mb-4">
            Partidos pendientes
        </h2>

        <!-- Toast estilo Flowbite -->
        <transition name="fade">
            <div v-if="showSuccess" class="fixed bottom-20 inset-x-0 flex justify-center z-50 px-4">
                <div class="flex items-center w-full max-w-xs p-4 text-sm text-white rounded-lg shadow-lg" role="alert"
                    style="background-color: #1DB954;">
                    <i class="fas fa-check-circle text-white me-2"></i>
                    <div>La reserva ha sido cancelada.</div>
                </div>
            </div>
        </transition>


        <!-- Aviso cuando no hay turnos -->
        <div v-if="turnos.length === 0" class="rounded-xl border border-gray-200 bg-gray-50 p-4 text-sm text-gray-600">
            <div class="flex items-center gap-2">
                <i class="fas fa-info-circle text-gray-500"></i>
                <span>No hay turnos reservados.</span>
            </div>
        </div>

        <!-- LISTA con transición para fade-out al eliminar -->
        <transition-group v-else name="fade" tag="div">
            <section v-for="(t, i) in turnos" :key="t.id"
                class="relative rounded-xl bg-blue-100 p-4 shadow-sm mb-4 text-left space-y-1">
                <!-- overlay loader mientras cancela -->
                <div v-if="cancellingId === t.id"
                    class="absolute inset-0 z-40 grid place-items-center rounded-xl bg-white/70 backdrop-blur-[1px]">
                    <div class="inline-flex items-center gap-2 text-sm text-gray-700">
                        <svg class="h-5 w-5 animate-spin" viewBox="0 0 24 24" fill="none" aria-hidden="true">
                            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" />
                            <path class="opacity-75" fill="currentColor" d="M4 12a 8 8 0 0 1 8-8v4A4 4 0 0 0 4 12z" />
                        </svg>
                        Cancelando turno…
                    </div>
                </div>

                <!-- botón menú -->
                <button @click.stop="toggleMenu(i)"
                    class="absolute top-3 right-2 w-9 h-9 inline-flex items-center justify-center rounded-lg hover:bg-white/60"
                    :aria-expanded="(showMenuIndex === i).toString()" aria-haspopup="menu"
                    :disabled="cancellingId === t.id">
                    <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 256 256">
                        <circle cx="128" cy="64" r="10" />
                        <circle cx="128" cy="128" r="10" />
                        <circle cx="128" cy="192" r="10" />
                    </svg>
                </button>

                <!-- menú -->
                <div v-if="showMenuIndex === i"
                    class="absolute right-2 top-12 z-50 w-40 rounded-lg border border-gray-200 bg-white shadow-lg"
                    role="menu">
                    <button
                        class="flex w-full items-center gap-2 px-4 py-2 text-left text-sm text-red-600 hover:bg-red-50 rounded-lg"
                        @click="openModal(t.id)">
                        <i class="fas fa-ban"></i>
                        Cancelar
                    </button>
                </div>

                <!-- contenido -->
                <div class="pr-8">
                    <h3 class="text-base font-bold text-gray-900 mb-2 leading-tight">{{ t.complejo }}</h3>
                    <div class="space-y-1">
                        <div class="flex items-center gap-2 text-gray-600">
                            <i class="fas fa-futbol text-[10px] w-4 mt-0.5"></i>
                            <span class="text-sm font-medium">{{ t.cancha }}</span>
                        </div>
                        <div class="flex items-center gap-2 text-gray-600">
                            <i class="fas fa-calendar-day text-[10px] w-4 mt-0.5"></i>
                            <span class="text-sm">{{ t.fecha }}</span>
                        </div>
                        <div class="flex items-center gap-2 text-gray-600">
                            <i class="fas fa-clock text-[10px] w-4 mt-0.5"></i>
                            <span class="text-sm">{{ t.hora }} a {{ t.horaFin }} hs</span>
                        </div>
                    </div>
                </div>
                <div class="mt-3 pt-2 border-t border-blue-200/50 flex justify-between items-center">
                    <p class="inline-flex items-center gap-2 text-xs font-bold"
                        :class="t.estado === 'Confirmado' ? 'text-green-700' : 'text-amber-700'">
                        <i :class="t.estado === 'Confirmado' ? 'fas fa-check-circle' : 'fas fa-hourglass-half'"></i>
                        {{ t.estado }}
                    </p>
                </div>
            </section>
        </transition-group>

        <h2 class="mt-8 text-sm font-semibold text-gray-500 uppercase tracking-wide text-left mb-4">
            Turnos antiguos
        </h2>

        <!-- Aviso cuando no hay turnos antiguos -->
        <div v-if="turnosAntiguos.length === 0"
            class="rounded-xl border border-gray-200 bg-gray-50 p-4 text-sm text-gray-600">
            <div class="flex items-center gap-2">
                <i class="fas fa-info-circle text-gray-500"></i>
                <span>No hay turnos antiguos.</span>
            </div>
        </div>

        <!-- Lista con transición -->
        <transition-group v-else name="fade" tag="div">
            <section v-for="ta in turnosAntiguos" :key="ta.id"
                class="relative rounded-xl bg-gray-50 border border-gray-100 p-4 shadow-sm mb-4 text-left">
                <div class="pr-2">
                    <h3 class="text-sm font-bold text-gray-600 mb-2 leading-tight">{{ ta.complejo }}</h3>
                    <div class="space-y-0.5 opacity-75">
                        <div class="flex items-center gap-2 text-xs text-gray-500">
                            <i class="fas fa-futbol text-[9px] w-3 text-center"></i> <span>{{ ta.cancha }}</span>
                        </div>
                        <div class="flex items-center gap-2 text-xs text-gray-500">
                            <i class="fas fa-calendar text-[9px] w-3 text-center"></i> <span>{{ ta.fecha }}</span>
                        </div>
                        <div class="flex items-center gap-2 text-xs text-gray-500">
                            <i class="fas fa-clock text-[9px] w-3 text-center"></i> <span>{{ ta.hora }} a {{ ta.horaFin
                            }} hs</span>
                        </div>
                    </div>
                </div>
                <div class="mt-2 pt-2 border-t border-gray-100 flex items-center justify-between">
                    <p
                        class="inline-flex items-center gap-2 text-[10px] font-bold text-gray-400 uppercase tracking-widest">
                        <i class="fas fa-flag-checkered"></i>
                        FINALIZADO
                    </p>
                </div>
            </section>
        </transition-group>

        <teleport to="body">
            <div v-if="showModal" class="fixed inset-0 z-[2100] grid place-items-center p-4">
                <div class="absolute inset-0 bg-black/60 backdrop-blur-sm" @click="closeModal"></div>

                <div class="relative w-full max-w-sm rounded-2xl bg-white p-6 shadow-2xl animate-slide-up">
                    <div
                        class="w-14 h-14 bg-red-50 rounded-full flex items-center justify-center text-red-500 mx-auto mb-4">
                        <i class="fas fa-trash-can text-xl"></i>
                    </div>
                    <h3 class="text-xl font-black text-center text-gray-900">¿Cancelar reserva?</h3>
                    <p class="text-sm text-gray-500 text-center mt-1">Se perderá el turno seleccionado.</p>

                    <div v-if="turnoSeleccionado"
                        class="mt-5 bg-gray-50 rounded-xl p-4 border border-gray-100 space-y-2 text-left">
                        <p class="text-[10px] uppercase font-black text-gray-400 tracking-widest">Detalles</p>
                        <p class="text-sm font-bold text-gray-800 leading-tight">{{ turnoSeleccionado.complejo }}</p>
                        <div class="text-xs text-gray-600 space-y-1">
                            <div class="flex items-center gap-2"><i class="fas fa-futbol w-3 text-center"></i> {{
                                turnoSeleccionado.cancha }}</div>
                            <div class="flex items-center gap-2"><i class="fas fa-calendar-day w-3 text-center"></i> {{
                                turnoSeleccionado.fecha }}</div>
                            <div class="flex items-center gap-2"><i class="fas fa-clock w-3 text-center"></i> {{
                                turnoSeleccionado.hora }} a {{ turnoSeleccionado.horaFin }} hs</div>
                        </div>
                    </div>

                    <div class="mt-8 flex flex-col gap-2">
                        <button
                            class="w-full py-4 bg-red-600 text-white font-black rounded-xl shadow-lg shadow-red-100 active:scale-95 transition-all"
                            @click="confirmCancel">
                            SÍ, CANCELAR TURNO
                        </button>
                        <button class="w-full py-3 text-sm font-bold text-gray-400 hover:text-gray-600"
                            @click="closeModal">
                            No, volver atrás
                        </button>
                    </div>
                </div>
            </div>
        </teleport>
    </div>
</template>

<script>
import { startLoader, stopLoader } from '../services/globalLoader';
import { useAuthUser } from '../composables/useAuthUser';
import { API_ENDPOINTS } from '../config/apiConfig';

export default {
    name: 'Reservas',
    setup() {
        const { user } = useAuthUser();
        return { user };
    },
    data() {
        return {
            turnos: [],
            turnosAntiguos: [],
            showMenuIndex: null,
            showModal: false,
            selectedId: null,
            cancellingId: null,
            showSuccess: false,
            successTimer: null,
            _loaderTimer: null,
        }
    },

    mounted() {
        this.loadData()
    },

    computed: {
        sinTurnos() { return this.turnos.length === 0 },
        turnoSeleccionado() { return this.turnos.find(t => t.id === this.selectedId) || null },
    },

    beforeUnmount() {
        if (this._loaderTimer) clearTimeout(this._loaderTimer)
        if (this.successTimer) clearTimeout(this.successTimer)
    },

    methods: {
        async loadData() {
            startLoader();
            try {
                const userId = this.user?.sub || 1;
                const response = await fetch(API_ENDPOINTS.reservas.getUserReservations(userId));
                if (!response.ok) throw new Error('Error al cargar reservas');

                const data = await response.json();
                this.turnos = data.partidosPendientes;
                this.turnosAntiguos = data.turnosAntiguos;
            } catch (error) {
                console.error("Error loading reservations:", error);
            } finally {
                stopLoader();
            }
        },

        toggleMenu(i) {
            if (this.cancellingId !== null) return
            this.showMenuIndex = this.showMenuIndex === i ? null : i
        },
        openModal(id) {
            this.selectedId = id
            this.showModal = true
            this.showMenuIndex = null
        },
        closeModal() { this.showModal = false },

        async confirmCancel() {
            const id = this.selectedId;
            this.showModal = false;
            this.cancellingId = id;

            try {
                const response = await fetch(API_ENDPOINTS.reservas.cancelReservation(id), {
                    method: 'DELETE'
                });

                if (!response.ok) throw new Error('Error al cancelar reserva');

                // Aplicamos el retraso visual para que se vea el loader de cancelación
                setTimeout(() => {
                    this.turnos = this.turnos.filter(t => t.id !== id);
                    this.cancellingId = null;
                    this.selectedId = null;
                    this.showSuccess = true;

                    if (this.successTimer) clearTimeout(this.successTimer);
                    this.successTimer = setTimeout(() => (this.showSuccess = false), 3000);
                }, 1000);

            } catch (error) {
                console.error("Error cancelling reservation:", error);
                this.cancellingId = null;
            }
        },
    },
}
</script>
