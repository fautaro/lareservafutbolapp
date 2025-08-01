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
                    <div>La reserva ha sido cancelada correctamente.</div>
                </div>
            </div>
        </transition>


        <!-- Aviso cuando no hay turnos -->
        <div v-if="turnos.length === 0" class="rounded-xl border border-gray-200 bg-gray-50 p-4 text-sm text-gray-600">
            <div class="flex items-center gap-2">
                <i class="fas fa-info-circle text-gray-500"></i>
                <span>No hay turnos reservados actualmente.</span>
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
                <p class="text-base font-semibold">{{ t.complejo }}</p>
                <p class="text-sm text-gray-700">{{ t.cancha }} · {{ t.fecha }} · {{ t.hora }}</p>
                <p class="mt-2 inline-flex items-center gap-2 text-lg font-medium"
                    :class="t.estado === 'Confirmado' ? 'text-green-700' : 'text-yellow-700'">
                    <i :class="t.estado === 'Confirmado' ? 'fas fa-circle-check' : 'fas fa-hourglass-half'"></i>
                    {{ t.estado }}
                </p>
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
                class="relative rounded-xl bg-gray-200 p-4 shadow-sm mb-4 text-left space-y-1">
                <p class="text-base font-semibold">{{ ta.complejo }}</p>
                <p class="text-sm text-gray-700">{{ ta.cancha }} · {{ ta.fecha }} · {{ ta.hora }}</p>

                <div class="mt-2 flex items-center justify-between">
                    <p class="inline-flex items-center gap-2 text-sm font-medium text-gray-700">
                        <i class="fas fa-flag-checkered"></i>
                        {{ ta.estado }}
                    </p>
                    <p class="text-sm font-semibold text-[#101518]">
                        Resultado: {{ ta.resultado }}
                    </p>
                </div>
            </section>
        </transition-group>

        <!-- MODAL ÚNICO -->
        <teleport to="body">
            <div v-if="showModal" class="fixed inset-0 z-[999] grid place-items-center p-4">
                <div class="absolute inset-0 bg-black/50" @click="closeModal"></div>

                <div class="relative w-full max-w-sm rounded-lg bg-white p-6 shadow-xl">
                    <h3 class="text-lg font-semibold">¿Seguro que desea cancelar?</h3>

                    <div v-if="turnoSeleccionado" class="mt-3 text-sm text-gray-700 space-y-1">
                        <p><strong>Complejo:</strong> {{ turnoSeleccionado.complejo }}</p>
                        <p><strong>Turno:</strong> {{ turnoSeleccionado.cancha }} · {{ turnoSeleccionado.fecha }} · {{
                            turnoSeleccionado.hora }}</p>
                    </div>

                    <div class="mt-5 flex justify-end gap-3">
                        <button
                            class="rounded-lg bg-[#EB5757] px-4 py-2 text-sm font-medium text-white hover:brightness-95 focus:outline-none"
                            @click="confirmCancel">
                            Confirmar
                        </button>
                        <button
                            class="rounded-lg bg-gray-100 px-4 py-2 text-sm font-medium text-gray-800 hover:bg-gray-200"
                            @click="closeModal">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </teleport>
    </div>
</template>

<script>
export default {
    name: 'Reservas',
    data() {
        return {
            turnos: [
                { id: 1, complejo: 'Complejo Deportivo Norte', cancha: 'Cancha 1', fecha: '01/01/2026', hora: '13:00', estado: 'Confirmado' },
                { id: 2, complejo: 'Club Parque Sur', cancha: 'Cancha 2', fecha: '05/01/2026', hora: '20:30', estado: 'Pendiente' },
            ],
            turnosAntiguos: [
                {
                    id: 201,
                    complejo: 'Polideportivo Oeste',
                    cancha: 'Cancha 4',
                    fecha: '10/10/2025',
                    hora: '19:00',
                    estado: 'Finalizado',
                    resultado: '1 - 3',
                },
                {
                    id: 202,
                    complejo: 'Club Central',
                    cancha: 'Cancha 2',
                    fecha: '02/11/2025',
                    hora: '21:00',
                    estado: 'Finalizado',
                    resultado: '2 - 2',
                }],
            showMenuIndex: null,
            showModal: false,
            selectedId: null,

            // loader por card
            cancellingId: null,
            showSuccess: false,
            successTimer: null,
        }
    },
    computed: {
        sinTurnos() {
            return this.turnos.length === 0
        },
        turnoSeleccionado() {
            return this.turnos.find(t => t.id === this.selectedId) || null
        },
    },
    beforeUnmount() {
        if (this.toastTimer) clearTimeout(this.toastTimer)
    },
    methods: {
        toggleMenu(i) {
            if (this.cancellingId !== null) return
            this.showMenuIndex = this.showMenuIndex === i ? null : i
        },
        openModal(id) {
            this.selectedId = id
            this.showModal = true
            this.showMenuIndex = null
        },
        closeModal() {
            this.showModal = false
        },
        confirmCancel() {
            const id = this.selectedId
            this.showModal = false
            this.cancellingId = id

            setTimeout(() => {
                this.turnos = this.turnos.filter(t => t.id !== id)
                this.cancellingId = null
                this.selectedId = null
                this.showSuccess = true

                if (this.successTimer) clearTimeout(this.successTimer)
                this.successTimer = setTimeout(() => (this.showSuccess = false), 3000)
            }, 2000)
        },
    },
}
</script>
