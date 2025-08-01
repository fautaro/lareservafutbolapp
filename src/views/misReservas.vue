<template>
    <div class="p-4 text-[#101518] font-sans">
        <h1 class="text-4xl font-bold mb-7 text-left">Reservas</h1>

        <h2 class="text-sm font-semibold text-gray-500 uppercase tracking-wide text-left mb-4">
            Partidos pendientes
        </h2>

        <!-- Lista de turnos -->
        <section v-for="(t, i) in turnos" :key="t.id"
            class="relative rounded-xl bg-blue-100 p-4 shadow-sm text-left space-y-1 mb-4">
            <!-- Botón tres puntos -->
            <button ref="menuButtonRef" @click.stop="toggleMenu(i)" @keydown.down.prevent="focusFirstItem(i)"
                class="absolute top-[18px] right-2 inline-flex items-center justify-center w-9 h-9 rounded-lg hover:bg-white/60 text-gray-600 focus:outline-none focus:ring-4 focus:ring-blue-200"
                aria-haspopup="menu" :aria-expanded="(showMenuIndex === i).toString()" aria-label="Opciones">
                <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="currentColor" viewBox="0 0 256 256"
                    aria-hidden="true">
                    <circle cx="128" cy="64" r="10" />
                    <circle cx="128" cy="128" r="10" />
                    <circle cx="128" cy="192" r="10" />
                </svg>
            </button>

            <!-- Dropdown -->
            <div v-if="showMenuIndex === i" ref="menuRef"
                class="absolute right-2 top-12 z-50 w-40 origin-top-right rounded-lg border border-gray-200 bg-white shadow-lg focus:outline-none"
                role="menu" aria-label="Acciones de la reserva" @keydown.esc.stop.prevent="closeMenu"
                @keydown.tab="closeMenu">
                <button ref="firstItemRef" role="menuitem"
                    class="flex w-full items-center gap-2 px-4 py-2 text-left text-sm text-red-600 hover:bg-red-50 rounded-lg focus:outline-none focus:bg-red-50"
                    @click="openModal(i)">
                    <i class="fas fa-ban"></i>
                    Cancelar
                </button>
            </div>

            <!-- Contenido -->
            <p class="text-base font-semibold">{{ t.complejo }}</p>
            <p class="text-sm text-gray-700">
                {{ t.cancha }} · {{ t.fecha }} · {{ t.hora }}
            </p>

            <!-- Estado -->
            <p class="mt-2 inline-flex items-center gap-2 text-lg font-medium"
                :class="t.estado === 'Confirmado' ? 'text-green-700' : 'text-yellow-700'">
                <i :class="t.estado === 'Confirmado' ? 'fas fa-circle-check' : 'fas fa-hourglass-half'"></i>
                {{ t.estado }}
            </p>
        </section>
        <h2 class="mt-8 text-sm font-semibold text-gray-500 uppercase tracking-wide text-left mb-4">
            Turnos pasados
        </h2>

        <!-- Lista de turnos pasados (cards grises) -->
        <section v-for="tp in turnosPasados" :key="tp.id"
            class="relative rounded-xl bg-gray-100 p-4 shadow-sm text-left space-y-1 mb-4">
            <!-- Contenido -->
            <p class="text-base font-semibold">{{ tp.complejo }}</p>
            <p class="text-sm text-gray-700">
                {{ tp.cancha }} · {{ tp.fecha }} · {{ tp.hora }}
            </p>

            <!-- Estado + resultado -->
            <div class="mt-2 flex items-center justify-between">
                <p class="inline-flex items-center gap-2 text-sm font-medium text-gray-700">
                    <i class="fas fa-flag-checkered"></i>
                    {{ tp.estado }}
                </p>
                <p class="text-sm font-semibold text-[#101518]">Resultado: {{ tp.resultado }}</p>
            </div>
        </section>
        <!-- Modal -->
        <teleport to="body">
            <div v-if="showModalIndex !== null" class="fixed inset-0 z-[999] flex items-center justify-center"
                @keydown.esc.prevent="closeModal">
                <div class="absolute inset-0 bg-black/50" @click="closeModal" aria-hidden="true"></div>

                <div ref="modalRef"
                    class="relative w-full max-w-sm mx-4 rounded-lg bg-white p-6 shadow-xl focus:outline-none focus:ring-4 focus:ring-blue-200"
                    role="dialog" aria-modal="true" aria-labelledby="modal-title">
                    <h3 id="modal-title" class="text-lg font-semibold">¿Seguro que desea cancelar?</h3>

                    <div v-if="turnoSeleccionado" class="mt-3 text-sm text-gray-700 space-y-1">
                        <p><strong>Complejo:</strong> {{ turnoSeleccionado.complejo }}</p>
                        <p>
                            <strong>Turno:</strong> {{ turnoSeleccionado.cancha }} ·
                            {{ turnoSeleccionado.fecha }} · {{ turnoSeleccionado.hora }}
                        </p>
                    </div>

                    <div class="mt-5 flex justify-end gap-3">
                        <button
                            class="inline-flex items-center justify-center rounded-lg bg-[#EB5757] px-4 py-2 text-sm font-medium text-white hover:brightness-95 focus:outline-none focus:ring-4 focus:ring-rose-200"
                            @click="confirmCancel">
                            Confirmar
                        </button>
                        <button
                            class="inline-flex items-center justify-center rounded-lg bg-gray-100 px-4 py-2 text-sm font-medium text-gray-800 hover:bg-gray-200 focus:outline-none focus:ring-4 focus:ring-gray-200"
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
            // listado de turnos a iterar
            turnos: [
                {
                    id: 1,
                    complejo: 'Complejo Deportivo Norte',
                    cancha: 'Cancha 1',
                    fecha: '01/01/2026',
                    hora: '13:00',
                    estado: 'Confirmado',
                },
                {
                    id: 2,
                    complejo: 'Club Parque Sur',
                    cancha: 'Cancha 2',
                    fecha: '05/01/2026',
                    hora: '20:30',
                    estado: 'Pendiente',
                },
            ],
            turnosPasados: [
                {
                    id: 101,
                    complejo: 'Complejo Deportivo Norte',
                    cancha: 'Cancha 3',
                    fecha: '12/12/2025',
                    hora: '18:00',
                    estado: 'Finalizado',
                    resultado: '2 - 1',
                },
                {
                    id: 102,
                    complejo: 'Club Parque Sur',
                    cancha: 'Cancha 1',
                    fecha: '22/12/2025',
                    hora: '20:00',
                    estado: 'Finalizado',
                    resultado: '0 - 0',
                },
            ],
            showMenuIndex: null,   // índice del turno con menú abierto
            showModalIndex: null,  // índice del turno con modal abierto
        }
    },
    computed: {
        turnoSeleccionado() {
            if (this.showModalIndex === null) return null
            return this.turnos[this.showModalIndex] || null
        },
    },
    mounted() {
        document.addEventListener('click', this.handleClickOutside)
    },
    unmounted() {
        document.removeEventListener('click', this.handleClickOutside)
    },
    methods: {
        toggleMenu(i) {
            this.showMenuIndex = this.showMenuIndex === i ? null : i
            this.$nextTick(() => {
                // enfocar primer ítem si se abrió
                if (this.showMenuIndex === i) {
                    this.$refs.firstItemRef?.focus?.()
                }
            })
        },
        closeMenu() {
            this.showMenuIndex = null
            // devolver foco al botón del menú (si existe)
            this.$refs.menuButtonRef?.focus?.()
        },
        handleClickOutside(e) {
            // Si hay un menú abierto y el click fue fuera del menú, cerrarlo
            const menuEl = this.$refs.menuRef
            const btnEl = this.$refs.menuButtonRef
            const clickedInsideMenu = Array.isArray(menuEl)
                ? menuEl.some(el => el && el.contains(e.target))
                : menuEl?.contains?.(e.target)
            const clickedButton = Array.isArray(btnEl)
                ? btnEl.some(el => el && el.contains(e.target))
                : btnEl?.contains?.(e.target)

            if (this.showMenuIndex !== null && !clickedInsideMenu && !clickedButton) {
                this.closeMenu()
            }
        },
        focusFirstItem(i) {
            if (this.showMenuIndex === i) {
                this.$refs.firstItemRef?.focus?.()
            } else {
                this.toggleMenu(i)
            }
        },
        openModal(i) {
            this.showModalIndex = i
            this.closeMenu()
            this.$nextTick(() => this.$refs.modalRef?.focus?.())
        },
        closeModal() {
            this.showModalIndex = null
        },
        confirmCancel() {
            const t = this.turnoSeleccionado
            // TODO: llamada a API para cancelar (usar t.id)
            // Por ahora cerramos modal:
            this.showModalIndex = null
        },
    },
}
</script>
