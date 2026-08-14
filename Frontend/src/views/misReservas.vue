<style scoped>
.fade-enter-active,
.fade-leave-active {
    transition: all 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
    transform: translateY(10px);
}

/* Glass effect for the tab bar */
.tabs-container {
    background: rgba(255, 255, 255, 0.9);
    backdrop-filter: blur(10px);
}

.reserva-card {
    transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
}

@keyframes slide-up {
    from {
        transform: translateY(20px);
        opacity: 0;
    }

    to {
        transform: translateY(0);
        opacity: 1;
    }
}

.animate-slide-up {
    animation: slide-up 0.4s ease forwards;
}

.tab-indicator {
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}
</style>

<template>
    <div class="min-h-screen bg-gray-50 pb-24 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">
        <!-- Header Island (Título + Tabs) -->
        <div
            class="bg-white rounded-[24px] shadow-md sticky top-4 z-40 overflow-hidden mx-1 pb-2 border border-slate-100/50">
            <!-- Título -->
            <div class="px-4 pt-8 pb-5">
                <h1 class="text-3xl font-bold tracking-tight text-slate-900 leading-none">
                    {{ isOwner() ? 'Reservas Recibidas' : 'Mis Reservas' }}
                </h1>
                <p class="text-slate-400 text-sm font-medium mt-2">
                    {{ isOwner() ? 'Controlá los turnos agendados por tus clientes.' : 'Gestioná tus turnos y seguí tu historial.' }}
                </p>
            </div>

            <!-- Tabs Selector -->
            <div class="tabs-container flex overflow-x-auto scroll-smooth p-1 pb-3 px-2 gap-2">
                <button v-for="tab in filteredTabs" :key="tab.id" @click="activeTab = tab.id"
                    class="flex-shrink-0 py-4 px-4 text-xs font-bold uppercase tracking-widest transition-all duration-300 relative rounded-xl flex items-center justify-center gap-2"
                    :class="activeTab === tab.id ? 'text-[#2D9CDB] bg-[#2D9CDB]/5' : 'text-slate-400 hover:text-slate-600'">
                    {{ tab.label }}
                    <!-- Contador numérico -->
                    <span v-if="getTabCount(tab.id) > 0"
                        class="inline-flex items-center justify-center w-5 h-5 text-[11px] font-black rounded-full transition-colors leading-[0]"
                        :class="activeTab === tab.id ? 'bg-[#2D9CDB] text-white' : 'bg-slate-200 text-slate-500'">
                        {{ getTabCount(tab.id) }}</span>
                    <div v-if="activeTab === tab.id"
                        class="absolute bottom-1 left-1/2 -translate-x-1/2 w-6 h-1 bg-[#2D9CDB] rounded-full tab-indicator">
                    </div>
                </button>
            </div>
        </div>

        <!-- Capa invisible para cerrar menús al hacer clic fuera -->
        <div v-if="showMenuId" class="fixed inset-0 z-40" @click="showMenuId = null"></div>

        <div class="px-1 pt-8 sm:px-2">
            <!-- Toast de Éxito -->
            <transition name="fade">
                <div v-if="showSuccess"
                    class="fixed bottom-24 inset-x-0 flex justify-center z-50 px-4 pointer-events-none">
                    <div class="flex items-center gap-3 p-4 bg-slate-900 text-white rounded-2xl shadow-2xl">
                        <i class="fas fa-check-circle text-green-400"></i>
                        <span class="text-sm font-bold">Reserva cancelada</span>
                    </div>
                </div>
            </transition>

            <!-- CONTENIDO DE TABS -->
            <transition name="fade" mode="out-in">
                <!-- TAB 1: POR CONFIRMAR -->
                <div v-if="activeTab === 'pendientes'" key="pendientes" class="space-y-4">
                    <div v-if="porConfirmar.length === 0"
                        class="flex flex-col items-center justify-center py-24 text-center">
                        <div
                            class="w-20 h-20 bg-slate-100 rounded-full flex items-center justify-center mb-6 text-slate-300">
                            <i class="fas fa-hourglass-start text-3xl"></i>
                        </div>
                        <h3 class="font-bold text-slate-800">Nada pendiente</h3>
                        <p class="text-sm text-slate-400 max-w-[200px] mx-auto mt-1">Los turnos que esperan aprobación
                            del complejo aparecerán acá.</p>
                    </div>

                    <transition-group v-else name="fade" tag="div" class="space-y-4">
                        <div v-for="t in porConfirmar" :key="t.id"
                            class="reserva-card relative rounded-[28px] bg-white border border-slate-100 p-5 shadow-sm hover:shadow-md animate-slide-up"
                            :class="{ 'z-50': showMenuId === t.id }">

                            <div v-if="cancellingId === t.id"
                                class="absolute inset-0 z-40 grid place-items-center bg-white/90 backdrop-blur-sm rounded-[28px]">
                                <i class="fas fa-circle-notch animate-spin text-[#2D9CDB] text-2xl"></i>
                            </div>

                            <!-- Blur wrapper for card content -->
                            <div :class="{ 'blur-[2px] opacity-40 scale-[0.98] pointer-events-none': showMenuId === t.id }"
                                class="transition-all duration-500">
                                <!-- Header: Complejo -->
                                <div class="flex justify-between items-center mb-5 pr-10">
                                    <div class="flex items-center gap-3 min-w-0">
                                        <div
                                            class="w-8 h-8 rounded-lg bg-slate-50 flex items-center justify-center text-slate-400 shrink-0 border border-slate-100/50">
                                            <i class="fas fa-building text-xs"></i>
                                        </div>
                                        <h3 class="text-lg font-bold text-slate-900 truncate tracking-tight">{{
                                            t.complejo
                                        }}</h3>
                                    </div>
                                </div>

                                <!-- Body: Compact Rows -->
                                <div class="flex flex-col gap-2.5 mb-6">
                                    <!-- Cancha -->
                                    <div class="flex items-center gap-4 py-1">
                                        <div
                                            class="w-9 h-9 rounded-full bg-[#2D9CDB]/5 flex items-center justify-center text-[#2D9CDB] border border-[#2D9CDB]/20 shrink-0">
                                            <i class="fas fa-futbol text-xs"></i>
                                        </div>
                                        <div class="flex flex-col text-left">
                                            <span
                                                class="text-[10px] font-bold text-slate-400 uppercase tracking-tight">Cancha</span>
                                            <span class="text-sm font-bold text-slate-700 leading-tight">{{ t.cancha
                                            }}</span>
                                            <span
                                                class="text-[10px] font-medium text-slate-300 uppercase tracking-wider leading-none mt-0.5">{{
                                                    t.deporte }}</span>
                                        </div>
                                    </div>

                                    <!-- Fecha -->
                                    <div class="flex items-center gap-4 py-1">
                                        <div
                                            class="w-9 h-9 rounded-full bg-indigo-50/50 flex items-center justify-center text-indigo-500 border border-indigo-100/30 shrink-0">
                                            <i class="fas fa-calendar-day text-xs"></i>
                                        </div>
                                        <div class="flex flex-col text-left">
                                            <span
                                                class="text-[10px] font-bold text-slate-400 uppercase tracking-tight">Fecha</span>
                                            <span class="text-sm font-bold text-slate-700 leading-tight">{{ t.fecha
                                            }}</span>
                                        </div>
                                    </div>

                                    <!-- Hora -->
                                    <div class="flex items-center gap-4 py-1">
                                        <div
                                            class="w-9 h-9 rounded-full bg-emerald-50/50 flex items-center justify-center text-emerald-500 border border-emerald-100/30 shrink-0">
                                            <i class="fas fa-clock text-xs"></i>
                                        </div>
                                        <div class="flex flex-col text-left">
                                            <span
                                                class="text-[10px] font-bold text-slate-400 uppercase tracking-tight">Hora</span>
                                            <span class="text-sm font-bold text-slate-700 leading-tight">{{ t.hora }}
                                                hs</span>
                                        </div>
                                    </div>
                                </div>

                                <!-- Footer -->
                                <div class="flex items-center justify-between pt-5 border-t border-slate-50 gap-4">
                                    <div class="flex flex-col text-left">
                                        <span
                                            class="text-[10px] font-bold text-slate-300 uppercase tracking-widest mb-1">Medio
                                            de Pago</span>
                                        <span
                                            class="text-xs font-bold text-slate-600 bg-slate-50 px-2 py-1 rounded-lg border border-slate-100">
                                            {{ t.medioPago }}
                                        </span>
                                    </div>
                                    <div class="flex flex-col text-right">
                                        <span
                                            class="text-[10px] font-bold text-slate-300 uppercase tracking-widest mb-1">Total</span>
                                        <span class="text-xl font-bold text-[#2D9CDB] leading-none">${{
                                            t.precio.toLocaleString('es-AR') }}</span>
                                    </div>
                                </div>
                            </div>

                            <!-- Menu: Extracted from blur wrapper to stay clear -->
                            <div class="absolute top-7 right-7 z-50">
                                <button @click.stop="toggleMenu(t.id)"
                                    class="w-9 h-9 flex items-center justify-center text-slate-300 hover:text-slate-600 hover:bg-slate-50 rounded-xl transition-all"
                                    :class="{ 'text-[#2D9CDB] bg-[#2D9CDB]/5 shadow-sm': showMenuId === t.id }">
                                    <i class="fas fa-ellipsis-v text-xs"></i>
                                </button>
                                <transition name="fade">
                                    <div v-if="showMenuId === t.id"
                                        class="absolute right-0 mt-2 w-52 bg-white border border-slate-100 rounded-3xl shadow-2xl z-50 overflow-hidden ring-1 ring-slate-200/50 py-1">
                                        <button
                                            class="w-full px-5 py-3.5 text-left text-[11px] font-bold text-slate-600 hover:bg-slate-50 flex items-center gap-3 transition-colors">
                                            <i class="fas fa-share-alt text-slate-400 w-4"></i>
                                            Compartir
                                        </button>
                                        <button
                                            class="w-full px-5 py-3.5 text-left text-[11px] font-bold text-slate-600 hover:bg-slate-50 flex items-center gap-3 transition-colors">
                                            <i class="fas fa-calendar-plus text-slate-400 w-4"></i>
                                            Agregar al calendario
                                        </button>
                                        <button
                                            class="w-full px-5 py-3.5 text-left text-[11px] font-bold text-slate-600 hover:bg-slate-50 flex items-center gap-3 transition-colors">
                                            <i class="fas fa-location-arrow text-slate-400 w-4"></i>
                                            Como llego
                                        </button>
                                        <button
                                            class="w-full px-5 py-3.5 text-left text-[11px] font-bold text-slate-600 hover:bg-slate-50 flex items-center gap-3 transition-colors">
                                            <i class="fas fa-phone-alt text-slate-400 w-4"></i>
                                            Teléfono Complejo
                                        </button>

                                        <div class="h-px bg-slate-100 mx-3 my-1"></div>

                                        <button @click.stop="openModal(t.id)"
                                            class="w-full px-5 py-4 text-left text-[11px] font-extrabold text-red-500 hover:bg-red-50 flex items-center gap-3 transition-colors text-xs">
                                            <i class="fas fa-trash-alt w-4"></i>
                                            CANCELAR RESERVA
                                        </button>
                                    </div>
                                </transition>
                            </div>
                        </div>
                    </transition-group>
                </div>

                <!-- TAB 2: CONFIRMADAS -->
                <div v-else-if="activeTab === 'confirmadas'" key="confirmadas" class="space-y-4">
                    <div v-if="confirmadas.length === 0"
                        class="flex flex-col items-center justify-center py-24 text-center">
                        <div
                            class="w-20 h-20 bg-slate-100 rounded-full flex items-center justify-center mb-6 text-slate-300">
                            <i class="fas fa-calendar-check text-3xl"></i>
                        </div>
                        <h3 class="font-bold text-slate-800">
                            {{ isOwner() ? 'Sin reservas próximas' : 'Sin partidos próximos' }}
                        </h3>
                        <p class="text-sm text-slate-400 max-w-[240px] mx-auto mt-1">
                            {{ isOwner() ? 'Cuando los jugadores reserven tus canchas aparecerán acá.' : '¡Reservá hoy y empezá a jugar!' }}
                        </p>
                        <button v-if="!isOwner()" @click="$router.push({ name: 'Home' })"
                            class="mt-6 px-6 py-3 bg-[#2D9CDB] text-white rounded-2xl font-bold text-xs uppercase tracking-widest shadow-xl shadow-[#2D9CDB]/20 transition-all active:scale-95">
                            Explorar Canchas
                        </button>
                        <button v-else @click="$router.push({ name: 'Home' })"
                            class="mt-6 px-6 py-3 bg-[#2D9CDB] text-white rounded-2xl font-bold text-xs uppercase tracking-widest shadow-xl shadow-[#2D9CDB]/20 transition-all active:scale-95">
                            Ver Mis Canchas
                        </button>
                    </div>

                    <transition-group v-else name="fade" tag="div" class="space-y-4">
                        <div v-for="t in confirmadas" :key="t.id"
                            class="reserva-card relative rounded-[28px] bg-white border border-slate-100 p-5 shadow-sm hover:shadow-md animate-slide-up"
                            :class="{ 'z-50': showMenuId === t.id }">

                            <!-- Blur wrapper for card content -->
                            <div :class="{ 'blur-[2px] opacity-40 scale-[0.98] pointer-events-none': showMenuId === t.id }"
                                class="transition-all duration-500">
                                <!-- Header: Complejo -->
                                <div class="flex justify-between items-center mb-5 pr-10">
                                    <div class="flex items-center gap-3 min-w-0">
                                        <div
                                            class="w-8 h-8 rounded-lg bg-slate-50 flex items-center justify-center text-slate-400 shrink-0 border border-slate-100/50">
                                            <i class="fas fa-building text-xs"></i>
                                        </div>
                                        <h3 class="text-lg font-bold text-slate-900 truncate tracking-tight">{{
                                            t.complejo
                                        }}</h3>
                                    </div>
                                </div>

                                <!-- Body: Compact Rows -->
                                <div class="flex flex-col gap-2.5 mb-6">
                                    <!-- Cancha -->
                                    <div class="flex items-center gap-4 py-1">
                                        <div
                                            class="w-9 h-9 rounded-full bg-[#2D9CDB]/5 flex items-center justify-center text-[#2D9CDB] border border-[#2D9CDB]/20 shrink-0">
                                            <i class="fas fa-futbol text-xs"></i>
                                        </div>
                                        <div class="flex flex-col text-left">
                                            <span
                                                class="text-[10px] font-bold text-slate-400 uppercase tracking-tight">Cancha</span>
                                            <span class="text-sm font-bold text-slate-700 leading-tight">{{ t.cancha
                                            }}</span>
                                            <span
                                                class="text-[10px] font-medium text-slate-300 uppercase tracking-wider leading-none mt-0.5">{{
                                                    t.deporte }}</span>
                                        </div>
                                    </div>

                                    <!-- Fecha -->
                                    <div class="flex items-center gap-4 py-1">
                                        <div
                                            class="w-9 h-9 rounded-full bg-indigo-50/50 flex items-center justify-center text-indigo-500 border border-indigo-100/30 shrink-0">
                                            <i class="fas fa-calendar-day text-xs"></i>
                                        </div>
                                        <div class="flex flex-col text-left">
                                            <span
                                                class="text-[10px] font-bold text-slate-400 uppercase tracking-tight">Fecha</span>
                                            <span class="text-sm font-bold text-slate-700 leading-tight">{{ t.fecha
                                            }}</span>
                                        </div>
                                    </div>

                                    <!-- Hora -->
                                    <div class="flex items-center gap-4 py-1">
                                        <div
                                            class="w-9 h-9 rounded-full bg-emerald-50/50 flex items-center justify-center text-emerald-500 border border-emerald-100/30 shrink-0">
                                            <i class="fas fa-clock text-xs"></i>
                                        </div>
                                        <div class="flex flex-col text-left">
                                            <span
                                                class="text-[10px] font-bold text-slate-400 uppercase tracking-tight">Hora</span>
                                            <span class="text-sm font-bold text-slate-700 leading-tight">{{ t.hora }}
                                                hs</span>
                                        </div>
                                    </div>
                                </div>

                                <!-- Footer -->
                                <div class="flex items-center justify-between pt-5 border-t border-slate-50 gap-4">
                                    <div class="flex flex-col text-left">
                                        <span
                                            class="text-[10px] font-bold text-slate-300 uppercase tracking-widest mb-1">Medio
                                            de Pago</span>
                                        <span
                                            class="text-xs font-bold text-slate-600 bg-slate-50 px-2 py-1 rounded-lg border border-slate-100">
                                            {{ t.medioPago }}
                                        </span>
                                    </div>
                                    <div class="flex flex-col text-right">
                                        <span
                                            class="text-[10px] font-bold text-slate-300 uppercase tracking-widest mb-1">Total</span>
                                        <span class="text-xl font-bold text-[#2D9CDB] leading-none">${{
                                            t.precio.toLocaleString('es-AR') }}</span>
                                    </div>
                                </div>
                            </div>

                            <!-- Menu: Extracted from blur wrapper to stay clear -->
                            <div class="absolute top-7 right-7 z-50">
                                <button @click.stop="toggleMenu(t.id)"
                                    class="w-9 h-9 flex items-center justify-center text-slate-300 hover:text-slate-600 hover:bg-slate-50 rounded-xl transition-all"
                                    :class="{ 'text-[#2D9CDB] bg-[#2D9CDB]/5 shadow-sm': showMenuId === t.id }">
                                    <i class="fas fa-ellipsis-v text-xs"></i>
                                </button>
                                <transition name="fade">
                                    <div v-if="showMenuId === t.id"
                                        class="absolute right-0 mt-2 w-52 bg-white border border-slate-100 rounded-3xl shadow-2xl z-50 overflow-hidden ring-1 ring-slate-200/50 py-1">
                                        <button
                                            class="w-full px-5 py-3.5 text-left text-[11px] font-bold text-slate-600 hover:bg-slate-50 flex items-center gap-3 transition-colors">
                                            <i class="fas fa-share-alt text-slate-400 w-4"></i>
                                            Compartir
                                        </button>
                                        <button
                                            class="w-full px-5 py-3.5 text-left text-[11px] font-bold text-slate-600 hover:bg-slate-50 flex items-center gap-3 transition-colors">
                                            <i class="fas fa-calendar-plus text-slate-400 w-4"></i>
                                            Agregar al calendario
                                        </button>
                                        <button
                                            class="w-full px-5 py-3.5 text-left text-[11px] font-bold text-slate-600 hover:bg-slate-50 flex items-center gap-3 transition-colors">
                                            <i class="fas fa-location-arrow text-slate-400 w-4"></i>
                                            Como llego
                                        </button>
                                        <button
                                            class="w-full px-5 py-3.5 text-left text-[11px] font-bold text-slate-600 hover:bg-slate-50 flex items-center gap-3 transition-colors">
                                            <i class="fas fa-phone-alt text-slate-400 w-4"></i>
                                            Teléfono Complejo
                                        </button>

                                        <div class="h-px bg-slate-100 mx-3 my-1"></div>

                                        <button @click.stop="openModal(t.id)"
                                            class="w-full px-5 py-4 text-left text-[11px] font-extrabold text-red-500 hover:bg-red-50 flex items-center gap-3 transition-colors text-xs">
                                            <i class="fas fa-trash-alt w-4"></i>
                                            CANCELAR TURNO
                                        </button>
                                    </div>
                                </transition>
                            </div>
                        </div>
                    </transition-group>
                </div>

                <!-- TAB 3: HISTORIAL -->
                <div v-else-if="activeTab === 'historial'" key="historial" class="space-y-4">
                    <div v-if="turnosAntiguos.length === 0"
                        class="flex flex-col items-center justify-center py-24 text-center opacity-30">
                        <i class="fas fa-history text-4xl mb-4"></i>
                        <p class="font-bold">Historial vacío</p>
                    </div>

                    <div v-for="ta in turnosAntiguos" :key="ta.id"
                        class="reserva-card relative rounded-[28px] bg-white border border-slate-100 p-5 shadow-sm opacity-80 filter grayscale-[0.2]">

                        <!-- Header Historial: Complejo -->
                        <div class="flex items-center gap-3 mb-5">
                            <div
                                class="w-8 h-8 rounded-lg bg-slate-50 flex items-center justify-center text-slate-300 shrink-0 border border-slate-100/50">
                                <i class="fas fa-building text-xs"></i>
                            </div>
                            <h3 class="text-lg font-bold text-slate-500 truncate tracking-tight">{{ ta.complejo }}</h3>
                        </div>

                        <!-- Body Historial: Compact Rows -->
                        <div class="flex flex-col gap-2.5 mb-6">
                            <!-- Cancha -->
                            <div class="flex items-center gap-4 py-1">
                                <div
                                    class="w-9 h-9 rounded-full bg-slate-50 flex items-center justify-center text-slate-400 border border-slate-100 shrink-0">
                                    <i class="fas fa-futbol text-xs"></i>
                                </div>
                                <div class="flex flex-col text-left">
                                    <span
                                        class="text-[10px] font-bold text-slate-400 uppercase tracking-tight">Cancha</span>
                                    <span class="text-sm font-bold text-slate-500 truncate leading-tight">{{ ta.cancha
                                    }}</span>
                                    <span
                                        class="text-[10px] font-medium text-slate-300 uppercase tracking-wider leading-none mt-0.5">{{
                                            ta.deporte }}</span>
                                </div>
                            </div>

                            <!-- Fecha -->
                            <div class="flex items-center gap-4 py-1">
                                <div
                                    class="w-9 h-9 rounded-full bg-slate-50 flex items-center justify-center text-slate-400 border border-slate-100 shrink-0">
                                    <i class="fas fa-calendar-day text-xs"></i>
                                </div>
                                <div class="flex flex-col text-left">
                                    <span
                                        class="text-[10px] font-bold text-slate-400 uppercase tracking-tight">Fecha</span>
                                    <span class="text-sm font-bold text-slate-500 leading-tight">{{ ta.fecha }}</span>
                                </div>
                            </div>

                            <!-- Hora -->
                            <div class="flex items-center gap-4 py-1">
                                <div
                                    class="w-9 h-9 rounded-full bg-slate-50 flex items-center justify-center text-slate-400 border border-slate-100 shrink-0">
                                    <i class="fas fa-clock text-xs"></i>
                                </div>
                                <div class="flex flex-col text-left">
                                    <span
                                        class="text-[10px] font-bold text-slate-400 uppercase tracking-tight">Hora</span>
                                    <span class="text-sm font-bold text-slate-500 leading-tight">{{ ta.hora }} hs</span>
                                </div>
                            </div>
                        </div>

                        <!-- Footer Historial -->
                        <div class="flex items-center justify-between pt-5 border-t border-slate-50 gap-4">
                            <div class="flex flex-col text-left">
                                <span class="text-[10px] font-bold text-slate-300 uppercase tracking-widest mb-1">Medio
                                    de Pago</span>
                                <span class="text-xs font-bold text-slate-400">{{ ta.medioPago }}</span>
                            </div>
                            <div class="flex flex-col text-right">
                                <span
                                    class="text-[10px] font-bold text-slate-300 uppercase tracking-widest mb-1">Total</span>
                                <span class="text-lg font-bold text-slate-400">${{ ta.precio.toLocaleString('es-AR')
                                }}</span>
                            </div>
                        </div>
                    </div>
                </div>
            </transition>
        </div>

        <!-- Modal de Cancelación -->
        <teleport to="body">
            <div v-if="showModal" class="fixed inset-0 z-[2100] grid place-items-center p-4">
                <div class="absolute inset-0 bg-slate-900/60 backdrop-blur-sm" @click="closeModal"></div>

                <div class="relative w-full max-w-sm rounded-[24px] bg-white p-8 shadow-2xl animate-slide-up"
                    style="font-family: Inter, 'Noto Sans', sans-serif;">
                    <div
                        class="w-16 h-16 bg-red-50 rounded-full flex items-center justify-center text-red-500 mx-auto mb-5">
                        <i class="fas fa-calendar-xmark text-2xl"></i>
                    </div>
                    <h3 class="text-xl font-bold text-center text-slate-900">¿Confirmás la cancelación?</h3>
                    <p class="text-xs text-slate-500 text-center mt-2 px-4 leading-relaxed opacity-80">Esta acción no se
                        puede
                        deshacer y el turno quedará disponible para otros.</p>

                    <div v-if="turnoSeleccionado"
                        class="mt-8 bg-slate-50 rounded-2xl p-6 border border-slate-100 space-y-5 text-left">
                        <div class="flex flex-col items-center text-center gap-2">
                            <p class="text-xl font-bold text-slate-900 leading-tight tracking-tight">{{
                                turnoSeleccionado.complejo }}</p>
                            <div class="flex justify-center">
                                <span
                                    class="text-[10px] font-bold text-[#2D9CDB] bg-[#2D9CDB]/5 px-3 py-1 rounded-lg border border-[#2D9CDB]/20 uppercase tracking-widest">
                                    {{ turnoSeleccionado.cancha }}
                                </span>
                            </div>
                        </div>

                        <div class="space-y-3 pt-4 border-t border-slate-200">
                            <div class="flex items-center gap-4">
                                <div
                                    class="w-10 h-10 rounded-xl bg-white shadow-sm flex items-center justify-center text-slate-400 border border-slate-100">
                                    <i class="fas fa-calendar-day text-sm"></i>
                                </div>
                                <div class="flex flex-col">
                                    <span class="text-[10px] uppercase font-bold text-slate-400 tracking-wider">Fecha
                                        del turno</span>
                                    <span class="text-base font-bold text-slate-800">{{ turnoSeleccionado.fecha
                                    }}</span>
                                </div>
                            </div>

                            <div class="flex items-center gap-4">
                                <div
                                    class="w-10 h-10 rounded-xl bg-white shadow-sm flex items-center justify-center text-slate-400 border border-slate-100">
                                    <i class="fas fa-clock text-sm"></i>
                                </div>
                                <div class="flex flex-col">
                                    <span
                                        class="text-[9px] uppercase font-bold text-slate-400 tracking-wider">Horario</span>
                                    <span class="text-base font-bold text-slate-800">{{ turnoSeleccionado.hora }} a {{
                                        turnoSeleccionado.horaFin }} hs</span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="mt-7 flex flex-col gap-2">
                        <button
                            class="w-full py-3.5 bg-slate-900 text-white font-bold rounded-xl shadow-xl shadow-slate-200 active:scale-95 transition-all text-sm uppercase tracking-wide"
                            @click="confirmCancel">
                            Confirmar Cancelación
                        </button>
                        <button
                            class="w-full py-2 text-xs font-bold text-slate-300 uppercase tracking-widest hover:text-slate-500"
                            @click="closeModal">
                            Mantener turno
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
import { useRole } from '../composables/useRole';
import { API_ENDPOINTS } from '../config/apiConfig';

export default {
    name: 'Reservas',
    setup() {
        const { user } = useAuthUser();
        const { isOwner } = useRole();
        return { user, isOwner };
    },
    data() {
        return {
            turnos: [],
            turnosAntiguos: [],
            showMenuId: null,
            showModal: false,
            selectedId: null,
            cancellingId: null,
            showSuccess: false,
            successTimer: null,
            _loaderTimer: null,
            activeTab: 'confirmadas',
            tabs: [
                { id: 'pendientes', label: 'A Confirmar' },
                { id: 'confirmadas', label: 'Próximas' },
                { id: 'historial', label: 'Pasadas' }
            ]
        }
    },

    mounted() {
        this.loadData()
    },

    computed: {
        porConfirmar() {
            return this.turnos.filter(t => t.estado.toLowerCase().includes('pendiente'));
        },
        confirmadas() {
            return this.turnos.filter(t => t.estado.toLowerCase().includes('confirmado'));
        },
        filteredTabs() {
            if (this.isOwner()) {
                return this.tabs.filter(t => t.id !== 'pendientes');
            }
            return this.tabs;
        },
        turnoSeleccionado() {
            return this.turnos.find(t => t.id === this.selectedId) ||
                this.turnosAntiguos.find(t => t.id === this.selectedId) ||
                null;
        },
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

        openModal(id) {
            this.selectedId = id
            this.showModal = true
            this.showMenuId = null // Cerramos el menú al abrir el modal
        },
        closeModal() { this.showModal = false },

        toggleMenu(id) {
            this.showMenuId = this.showMenuId === id ? null : id;
        },

        async confirmCancel() {
            const id = this.selectedId;
            this.showModal = false;
            this.cancellingId = id;

            startLoader();

            try {
                // LLAMADA AL BACKEND PARA CANCELAR EL TURNO DEFINITIVAMENTE
                const response = await fetch(API_ENDPOINTS.reservas.cancelReservation(id), {
                    method: 'DELETE',
                    headers: { 'Content-Type': 'application/json' }
                });

                if (!response.ok) {
                    const errorText = await response.text();
                    throw new Error(`Error del servidor: ${errorText}`);
                }

                // Pausa mínima para fluidez visual y luego recargamos los datos desde backend
                await new Promise(resolve => setTimeout(resolve, 300));
                await this.loadData();

                this.cancellingId = null;
                this.selectedId = null;
                this.showSuccess = true;

                if (this.successTimer) clearTimeout(this.successTimer);
                this.successTimer = setTimeout(() => (this.showSuccess = false), 3000);

            } catch (error) {
                console.error("Error al cancelar turno en el backend:", error);
                this.cancellingId = null;
                alert('No se pudo cancelar el turno. Verificá tu conexión o intentá nuevamente.');
            } finally {
                stopLoader();
            }
        },

        getTabCount(tabId) {
            if (tabId === 'pendientes') return this.porConfirmar.length;
            if (tabId === 'confirmadas') return this.confirmadas.length;
            if (tabId === 'historial') return this.turnosAntiguos.length;
            return 0;
        }
    },
}
</script>

<style scoped>
.tabs-container {
    -ms-overflow-style: none;
    /* Internet Explorer 10+ */
    scrollbar-width: none;
    /* Firefox */
}

.tabs-container::-webkit-scrollbar {
    display: none;
    /* Safari and Chrome */
}

.reserva-card {
    transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.fade-enter-active,
.fade-leave-active {
    transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}
</style>
