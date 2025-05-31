<template>
    <div>
        <!-- Botón que abre el drawer -->
        <button @click="toggleDrawer"
            class="flex items-center gap-1 text-base font-medium bg-transparent text-black p-0 m-0 border-none shadow-none focus:outline-none">
            <i class="fas fa-map-marker-alt text-sm"></i>
            <span>{{ selectedCity.nombre }}</span>
            <i class="fas fa-caret-down text-sm"></i>
        </button>

        <!-- Backdrop oscuro -->
        <transition name="fade">
            <div v-if="drawerVisible" class="fixed inset-0 bg-black bg-opacity-50 z-40" @click.self="closeDrawer">
            </div>
        </transition>

        <!-- Drawer inferior -->
        <transition name="slide-up">
            <div v-if="drawerVisible"
                class="fixed bottom-0 left-0 w-full text-black bg-white rounded-t-2xl shadow-lg z-50 p-6 min-h-[28vh]">
                <h3 class="text-lg font-semibold mb-2">Seleccioná tu ciudad</h3>
                <ul>
                    <li v-for="city in cities" :key="city.id" @click="selectCity(city)"
                        class="py-2 border-b cursor-pointer hover:bg-gray-100">
                        {{ city.nombre }}
                    </li>
                </ul>
            </div>
        </transition>
    </div>
</template>

<script>
export default {
    name: 'CitySelectorDrawer',
    data() {
        return {
            drawerVisible: false,
            selectedCity: { id: 1, nombre: 'Viedma' },
            cities: [
                { id: 1, nombre: 'Viedma' },
                { id: 2, nombre: 'Patagones' }
            ]
        }
    },
    methods: {
        toggleDrawer() {
            this.drawerVisible = !this.drawerVisible;
        },
        closeDrawer() {
            this.drawerVisible = false;
        },
        selectCity(city) {
            this.selectedCity = city;
            this.closeDrawer();
            console.log(city)
            this.$emit('city-selected', city); // emitís el objeto ciudad completo
        }
    }
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
    transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}

.slide-up-enter-active,
.slide-up-leave-active {
    transition: transform 0.3s ease;
}

.slide-up-enter-from,
.slide-up-leave-to {
    transform: translateY(100%);
}
</style>
