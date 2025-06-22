<script setup>
import { ref, onMounted, onUnmounted } from 'vue'

const showMenu = ref(false)
const showModal = ref(false)
const menuRef = ref(null)

const toggleMenu = () => {
    showMenu.value = !showMenu.value
}

const handleClickOutside = (event) => {
    if (menuRef.value && !menuRef.value.contains(event.target)) {
        showMenu.value = false
    }
}

onMounted(() => {
    document.addEventListener('click', handleClickOutside)
})
onUnmounted(() => {
    document.removeEventListener('click', handleClickOutside)
})

const openModal = () => {
    showModal.value = true
    showMenu.value = false
}
const closeModal = () => {
    showModal.value = false
}
</script>

<template>
    <div class="p-4 text-gray-700 space-y-6">
        <h1 class="text-2xl font-bold text-[#101518] text-left">Mis Reservas</h1>

        <h2 class="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-2 text-left">
            Partidos pendientes
        </h2>

        <div class="relative rounded-xl bg-blue-100 p-4 shadow-sm text-left space-y-1">
            <!-- Botón tres puntitos -->
            <button @click.stop="toggleMenu"
                class="absolute top-[18px] right-2 appearance-none bg-transparent border-none p-0 m-0 text-gray-500 hover:text-gray-700"
                aria-label="Opciones">
                <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6" fill="currentColor" viewBox="0 0 256 256">
                    <circle cx="128" cy="64" r="10" />
                    <circle cx="128" cy="128" r="10" />
                    <circle cx="128" cy="192" r="10" />
                </svg>
            </button>

            <!-- Dropdown -->
            <div v-if="showMenu" ref="menuRef"
                class="absolute right-2 top-12 w-32 bg-white rounded-lg shadow-lg z-50 border border-gray-200">
                <button
                    class="block w-full px-4 py-2 text-sm text-left text-red-600 hover:bg-red-50 rounded-lg appearance-none bg-transparent border-none m-0 p-0 focus:outline-none"
                    style="background-color: transparent;" @click="openModal">
                    Cancelar
                </button>
            </div>

            <p class="text-base font-semibold text-[#101518]">Complejo Deportivo Norte</p>
            <p class="text-sm text-gray-700">Cancha 1 - 01/01/2026 - 13:00</p>

            <p class="text-lg font-medium text-green-700 flex items-center gap-2 mt-2">
                <i class="fas fa-circle-check text-lg"></i>
                Confirmado
            </p>
        </div>
    </div>
    <!-- Modal  -->
    <teleport to="body">
        <div v-if="showModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-[999]">
            <div class="bg-white rounded-lg p-6 w-full max-w-sm mx-auto text-center shadow-xl space-y-4">
                <h3 class="text-lg font-semibold text-[#101518]">¿Seguro que desea cancelar?</h3>
                <div class="text-sm text-gray-700 space-y-1">
                    <p><strong>Complejo:</strong> Complejo Deportivo Norte</p>
                    <p><strong>Turno:</strong> Cancha 1 - 01/01/2026 - 13:00</p>
                </div>
                <div class="flex justify-center gap-4 pt-2">
                    <button class="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700" @click="closeModal">
                        Confirmar
                    </button>
                    <button class="px-4 py-2 bg-gray-200 text-gray-800 rounded hover:bg-gray-300" @click="closeModal">
                        Cerrar
                    </button>
                </div>
            </div>
        </div>
    </teleport>
</template>
