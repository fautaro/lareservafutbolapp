<template>
    <div>
        <!-- Botón Disparador (Estilo Minimalista & Ghost) -->
        <button @click="toggleDrawer"
            class="group flex items-center gap-2 px-2 py-1.5 rounded-xl transition-all active:scale-95 focus:outline-none">
            <div class="flex items-center justify-center transition-transform group-hover:scale-110">
                <i class="fas fa-location-dot text-[#2D9CDB] text-sm"></i>
            </div>
            <div class="flex flex-col items-start leading-tight">
                <span class="text-[10px] font-black text-slate-400 uppercase tracking-wider mb-0.5">Ciudad</span>
                <div class="flex items-center gap-1.5">
                    <span class="text-[13px] font-bold text-[#121212]">{{ selectedCity.nombre }}</span>
                    <i
                        class="fas fa-chevron-down text-[9px] text-slate-300 group-hover:text-[#2D9CDB] transition-colors"></i>
                </div>
            </div>
        </button>

        <!-- Teleport para asegurar que el drawer esté fuera del contexto del header sticky/animado -->
        <Teleport to="body">
            <!-- Backdrop oscuro con desenfoque -->
            <transition name="fade">
                <div v-if="drawerVisible" class="fixed inset-0 bg-black/40 backdrop-blur-[2px] z-[1000]"
                    @click.self="closeDrawer">
                </div>
            </transition>

            <!-- Drawer inferior modernizado -->
            <transition name="slide-up">
                <div v-if="drawerVisible"
                    class="fixed bottom-0 left-0 w-full text-[#121212] bg-white rounded-t-[32px] shadow-2xl z-[1001] p-8 pb-10 min-h-[35vh]"
                    style="font-family: 'Noto Sans', sans-serif;">
                    <!-- Handle de arrastre (visual) -->
                    <div class="w-12 h-1.5 bg-slate-100 rounded-full mx-auto mb-8"></div>

                    <div class="flex items-center justify-between mb-6 px-2">
                        <div class="space-y-1">
                            <h3 class="text-xl font-black tracking-tight text-slate-900">¿Dónde estás?</h3>
                            <p class="text-xs text-slate-400 font-medium">Seleccioná tu ciudad para ver complejos
                                cercanos</p>
                        </div>
                    </div>

                    <div class="space-y-3">
                        <button v-for="city in cities" :key="city.id" @click="selectCity(city)"
                            class="w-full text-left p-5 rounded-2xl cursor-pointer transition-all flex items-center justify-between group relative overflow-hidden"
                            :class="selectedCity.id === city.id
                                ? 'bg-[#2D9CDB]/10 text-[#2D9CDB]'
                                : 'bg-slate-50 hover:bg-slate-100 text-slate-600'">
                            <div class="flex items-center gap-4">
                                <div class="w-10 h-10 rounded-xl flex items-center justify-center transition-colors"
                                    :class="selectedCity.id === city.id ? 'bg-white shadow-sm' : 'bg-white/50'">
                                    <i class="fas fa-city text-base"
                                        :class="selectedCity.id === city.id ? 'text-[#2D9CDB]' : 'text-slate-300'"></i>
                                </div>
                                <span class="text-base font-bold">{{ city.nombre }}</span>
                            </div>

                            <div v-if="selectedCity.id === city.id"
                                class="w-6 h-6 rounded-full bg-[#2D9CDB] flex items-center justify-center text-white shadow-lg shadow-[#2D9CDB]/20">
                                <i class="fas fa-check text-[10px]"></i>
                            </div>
                            <i v-else
                                class="fas fa-chevron-right text-xs text-slate-300 group-hover:translate-x-1 transition-transform"></i>
                        </button>
                    </div>
                </div>
            </transition>
        </Teleport>
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
            this.$emit('city-selected', city);
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
    transition: transform 0.5s cubic-bezier(0.32, 0.72, 0, 1);
}

.slide-up-enter-from,
.slide-up-leave-to {
    transform: translateY(100%);
}
</style>
