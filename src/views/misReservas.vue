<template>
    <div class="p-4">
        <h1 class="text-4xl font-bold mb-7">Reservas</h1>
        <!-- LISTA -->
        <section v-for="(t, i) in turnos" :key="t.id"
            class="relative rounded-xl bg-blue-100 p-4 shadow-sm mb-4 text-left space-y-1">
            <!-- botón menú -->
            <button @click.stop="toggleMenu(i)"
                class="absolute top-3 right-2 w-9 h-9 inline-flex items-center justify-center rounded-lg hover:bg-white/60"
                :aria-expanded="(showMenuIndex === i).toString()" aria-haspopup="menu">
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
                    <i class="fas fa-ban"></i> Cancelar
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
            showMenuIndex: null,   // solo un menú abierto
            showModal: false,      // modal único
            selectedId: null,      // id del turno a operar
        }
    },
    computed: {
        turnoSeleccionado() {
            return this.turnos.find(t => t.id === this.selectedId) || null
        },
    },
    mounted() {
        // cerrar menú al click fuera (simple)
        document.addEventListener('click', this.closeMenuOnOutside)
    },
    unmounted() {
        document.removeEventListener('click', this.closeMenuOnOutside)
    },
    methods: {
        toggleMenu(i) {
            this.showMenuIndex = this.showMenuIndex === i ? null : i
        },
        closeMenuOnOutside(e) {
            // si hay menú abierto y el click no viene de un botón de menú, cerramos
            // (esto es básico: si querés 100% robusto, chequeá target.closest por selector)
            if (this.showMenuIndex !== null && !e.target.closest('[role="menu"],[aria-haspopup="menu"]')) {
                this.showMenuIndex = null
            }
        },
        openModal(id) {
            this.selectedId = id
            this.showModal = true
            this.showMenuIndex = null
        },
        closeModal() {
            this.showModal = false
            this.selectedId = null
        },
        confirmCancel() {
            const t = this.turnoSeleccionado
            // TODO: cancelar turno con t.id (API)
            this.closeModal()
        },
    },
}
</script>
