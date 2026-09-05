<template>
  <div class="min-h-screen bg-[#F8FAFC] pb-32 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">
    <div class="animate-fade-in text-left">
      <!-- ══ HEADER / TOP NAVIGATION (Estilo Modo Jugador) ══ -->
      <nav class="sticky top-4 z-50 bg-white/95 backdrop-blur-md shadow-sm border border-slate-100 mx-2 mb-6 rounded-2xl px-3 py-3 flex items-center justify-between">
        <div class="flex flex-col items-center gap-1 min-w-[80px]">
          <svg class="h-8 w-auto" viewBox="0 0 52 40" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M0 0.19043V39.8487H30.8524L24.8072 29.1348H10.1606V0.19043H0Z" fill="#2D9CDB" />
            <path
              d="M37.7816 0.00362063C45.0797 -0.165953 51.306 5.64486 51.8218 12.4088C52.25 18.0208 48.8028 23.5772 43.2304 25.941L51.8563 39.849H38.6107L29.3726 26.1281H15.4243V17.5764H32.7911C33.1173 17.5808 34.5456 17.5662 35.7469 16.4493C36.7024 15.5605 36.6765 14.4714 36.7513 14.0884C36.7211 13.761 36.859 12.5404 35.8388 11.5624C34.5212 10.2994 32.8371 10.4982 32.6057 10.5289H15.4272V0.192198C15.4272 0.192198 37.3203 0.0138535 37.7816 0.00362063Z"
              fill="#2D9CDB"
            />
          </svg>
          <h1 class="text-[13px] font-bold text-[#2D9CDB] tracking-tight leading-none">
            La Reserva
          </h1>
        </div>

        <button
          type="button"
          @click="goToCreateUsuario"
          class="flex items-center gap-2 bg-[#2D9CDB] hover:bg-[#2088c2] text-white px-3.5 py-2 rounded-xl text-xs font-bold uppercase tracking-wider shadow-md shadow-blue-500/20 active:scale-95 transition-all"
        >
          <i class="fas fa-user-plus text-xs"></i>
          <span>Nuevo Usuario</span>
        </button>
      </nav>

      <!-- ══ ERROR STATE ══ -->
      <Teleport to="body">
        <transition name="modal-fade">
          <div
            v-if="loadError"
            class="fixed inset-0 z-[9999] flex items-center justify-center p-6 text-left"
          >
            <!-- Fullscreen Backdrop Overlay -->
            <div class="absolute inset-0 bg-slate-900/60 backdrop-blur-sm transition-opacity"></div>

            <!-- Modal Content Card -->
            <div
              class="relative bg-white rounded-2xl shadow-2xl p-8 text-center max-w-sm w-full animate-slide-up border border-slate-100"
              style="font-family: Inter, 'Noto Sans', sans-serif;"
            >
              <div class="w-16 h-16 bg-red-50 rounded-full flex items-center justify-center text-red-500 mx-auto mb-5 text-2xl shadow-inner">
                <i class="fas fa-wifi"></i>
              </div>
              <h2 class="text-xl font-bold mb-2 text-slate-900">Problema de conexión</h2>
              <p class="text-slate-500 mb-6 text-sm leading-relaxed">
                No pudimos obtener la información de los usuarios. Por favor, revisá tu conexión e intentá de nuevo.
              </p>
              <button
                type="button"
                @click="retryLoad"
                class="w-full bg-slate-900 hover:bg-slate-800 text-white py-4 rounded-xl font-bold uppercase tracking-widest text-[10px] shadow-lg active:scale-95 transition-all cursor-pointer"
              >
                Reintentar ahora
              </button>
            </div>
          </div>
        </transition>
      </Teleport>

      <!-- ══ CONTENIDO PRINCIPAL ══ -->
      <main class="mt-4 space-y-4">
        <!-- ── Tabs Selector con Contadores (Estilo UserReservas) ── -->
        <div class="tabs-container flex overflow-x-auto scroll-smooth p-1 pb-2 px-3 gap-2 no-scrollbar">
          <button
            v-for="tab in filterTabs"
            :key="tab.id"
            @click="activeFilter = tab.id"
            class="flex-shrink-0 py-3 px-4 text-xs font-bold uppercase tracking-widest transition-all duration-300 relative rounded-xl flex items-center justify-center gap-2"
            :class="activeFilter === tab.id ? 'text-[#2D9CDB] bg-[#2D9CDB]/10 font-black' : 'text-slate-400 hover:text-slate-600 bg-white border border-slate-100/80'"
          >
            {{ tab.label }}
            <span
              class="inline-flex items-center justify-center min-w-[20px] h-5 px-1.5 text-[11px] font-black rounded-full transition-colors leading-none"
              :class="activeFilter === tab.id ? 'bg-[#2D9CDB] text-white' : 'bg-slate-100 text-slate-500'"
            >
              {{ tab.count }}
            </span>
            <div
              v-if="activeFilter === tab.id"
              class="absolute bottom-1 left-1/2 -translate-x-1/2 w-6 h-1 bg-[#2D9CDB] rounded-full"
            ></div>
          </button>
        </div>

        <!-- ── Buscador ── -->
        <div class="px-3">
          <div class="relative bg-white rounded-2xl p-1.5 border border-slate-100 shadow-sm">
            <i class="fas fa-search absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 text-xs"></i>
            <input
              v-model="searchQuery"
              type="text"
              placeholder="Buscar por nombre, DNI o email..."
              class="w-full bg-slate-50 border border-slate-200/80 rounded-xl pl-10 pr-3.5 py-2.5 text-xs text-slate-800 placeholder-slate-400 focus:outline-none focus:border-[#2D9CDB] focus:bg-white focus:ring-1 focus:ring-[#2D9CDB] transition-all"
            />
          </div>
        </div>

        <!-- ── Listado de Usuarios ── -->
        <div class="px-3 pt-2">
          <!-- Loading State -->
          <div v-if="isLoading" class="flex flex-col items-center justify-center py-20">
            <i class="fas fa-circle-notch animate-spin text-3xl text-[#2D9CDB]"></i>
            <p class="mt-4 font-bold text-slate-400 text-xs uppercase tracking-wider">Cargando usuarios...</p>
          </div>

          <!-- Connection Error State Inline -->
          <div v-else-if="loadError" class="bg-white rounded-[24px] border border-slate-100 p-10 text-center shadow-sm">
            <div class="w-20 h-20 bg-red-50 rounded-full flex items-center justify-center mx-auto mb-4 text-red-500 text-3xl">
              <i class="fas fa-wifi"></i>
            </div>
            <h3 class="text-base font-bold text-slate-800 mb-1">Problema de conexión</h3>
            <p class="text-xs text-slate-400 max-w-[260px] mx-auto mb-6 leading-relaxed">No pudimos obtener la información de los usuarios. Por favor, revisá tu conexión e intentá de nuevo.</p>
            <button
              type="button"
              @click="retryLoad"
              class="px-6 py-3 bg-slate-900 hover:bg-slate-800 text-white rounded-xl text-xs font-bold uppercase tracking-widest transition-all active:scale-95 shadow-md shadow-slate-900/10"
            >
              Reintentar ahora
            </button>
          </div>

          <!-- Empty State -->
          <div v-else-if="usuariosFiltrados.length === 0" class="bg-white rounded-[24px] border border-slate-100 p-10 text-center shadow-sm">
            <div class="w-20 h-20 bg-slate-100 rounded-full flex items-center justify-center mx-auto mb-4 text-slate-300 text-3xl">
              <i class="fas fa-users-slash"></i>
            </div>
            <h3 class="text-base font-bold text-slate-800 mb-1">No se encontraron usuarios</h3>
            <p class="text-xs text-slate-400 max-w-[240px] mx-auto">Intentá con otro término de búsqueda o seleccioná otro filtro.</p>
          </div>

          <!-- Lista de Tarjetas (Cards estilo Modo Jugador) -->
          <div v-else class="space-y-3">
            <div
              v-for="u in usuariosFiltrados"
              :key="u.id"
              class="relative rounded-[24px] bg-white border border-slate-100 p-5 shadow-sm hover:shadow-md transition-all space-y-4"
              :class="{ 'opacity-70 bg-slate-50/60 border-dashed': !u.activo }"
            >
              <!-- Header usuario: Avatar + Nombre + Estado badge -->
              <div class="flex items-start justify-between gap-3">
                <div class="flex items-center gap-3.5 min-w-0">
                  <div
                    class="w-11 h-11 rounded-2xl flex items-center justify-center font-black text-sm shrink-0 shadow-inner"
                    :class="!u.activo ? 'bg-slate-100 text-slate-400' : 'bg-[#2D9CDB]/10 text-[#2D9CDB]'"
                  >
                    {{ getInitials(u.nombre, u.apellido) }}
                  </div>
                  <div class="min-w-0">
                    <h3 class="font-bold text-slate-900 text-base leading-tight truncate">
                      {{ u.nombre }} {{ u.apellido || '' }}
                    </h3>
                    <p class="text-xs text-slate-400 font-medium truncate mt-0.5">{{ u.email }}</p>
                  </div>
                </div>

                <!-- Estado badge -->
                <span
                  class="px-3 py-1 text-[10px] font-black uppercase tracking-wider rounded-full border shrink-0"
                  :class="!u.activo ? 'bg-rose-50 text-rose-600 border-rose-200' : 'bg-emerald-50 text-emerald-700 border-emerald-200'"
                >
                  {{ u.activo ? 'Activo' : 'Baja' }}
                </span>
              </div>

              <!-- Datos secundarios: Rol, DNI y Acciones -->
              <div class="flex flex-wrap items-center justify-between gap-3 pt-3 border-t border-slate-50 text-xs">
                <div class="flex items-center gap-2.5">
                  <span
                    class="px-2.5 py-1 rounded-lg text-[10px] font-black uppercase tracking-wider border"
                    :class="getRoleBadgeClass(u.tipoUsuarioNombre)"
                  >
                    <i :class="getRoleIcon(u.tipoUsuarioNombre)" class="mr-1"></i>
                    {{ u.tipoUsuarioNombre || 'Jugador' }}
                  </span>

                  <span v-if="u.dni" class="text-xs text-slate-500 font-medium flex items-center gap-1">
                    <span class="text-slate-400 font-semibold">DNI:</span> {{ u.dni }}
                  </span>
                </div>

                <!-- Botones de Acción: Dar de baja / Reactivar -->
                <div class="flex items-center gap-2 ml-auto">
                  <button
                    v-if="u.activo"
                    type="button"
                    @click="darDeBaja(u)"
                    class="flex items-center gap-1.5 px-3 py-1.5 rounded-xl text-rose-600 hover:bg-rose-50 border border-rose-200 text-xs font-bold transition-all active:scale-95"
                    title="Dar de baja usuario"
                  >
                    <i class="fas fa-user-minus text-[11px]"></i>
                    <span>Dar de baja</span>
                  </button>

                  <button
                    v-else
                    type="button"
                    @click="reactivar(u)"
                    class="flex items-center gap-1.5 px-3 py-1.5 rounded-xl text-emerald-700 hover:bg-emerald-50 border border-emerald-200 text-xs font-bold transition-all active:scale-95"
                    title="Reactivar usuario"
                  >
                    <i class="fas fa-rotate-left text-[11px]"></i>
                    <span>Reactivar</span>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { API_ENDPOINTS } from '../../config/apiConfig';
import { useToast } from '../../composables/useToast';
import { useConfirm } from '../../composables/useConfirm';

const router = useRouter();
const { showToast } = useToast();
const { requireConfirm } = useConfirm();

// Estado
const usuarios = ref([]);
const isLoading = ref(true);
const loadError = ref(false);
const searchQuery = ref('');
const activeFilter = ref('todos'); // 'todos' | 'activos' | 'bajas'

// Navegación a Alta de Usuario independiente
const goToCreateUsuario = () => {
  router.push({ name: 'AdminUserCreate' });
};

// Reintentar carga
const retryLoad = () => {
  loadError.value = false;
  fetchUsuarios();
};

// Cargar usuarios
const fetchUsuarios = async () => {
  isLoading.value = true;
  loadError.value = false;
  try {
    const response = await fetch(API_ENDPOINTS.usuarios.getAll());
    if (!response.ok) throw new Error('Error al obtener el listado de usuarios');
    const data = await response.json();
    usuarios.value = Array.isArray(data) ? data : [];
  } catch (error) {
    console.error('Error fetching usuarios:', error);
    loadError.value = true;
  } finally {
    isLoading.value = false;
  }
};

onMounted(() => {
  fetchUsuarios();
});

// Contadores
const countActivos = computed(() => {
  return usuarios.value.filter((u) => u.activo === true).length;
});

const countBajas = computed(() => {
  return usuarios.value.filter((u) => u.activo === false).length;
});

// Tabs de filtrado con conteo dinámico
const filterTabs = computed(() => [
  { id: 'todos', label: 'Todos', count: usuarios.value.length },
  { id: 'activos', label: 'Activos', count: countActivos.value },
  { id: 'bajas', label: 'Bajas', count: countBajas.value }
]);

// Usuarios filtrados
const usuariosFiltrados = computed(() => {
  return usuarios.value.filter((u) => {
    // Filtro de estado
    if (activeFilter.value === 'activos' && !u.activo) return false;
    if (activeFilter.value === 'bajas' && u.activo) return false;

    // Filtro de búsqueda
    if (searchQuery.value.trim()) {
      const q = searchQuery.value.toLowerCase().trim();
      const matchNombre = (u.nombre || '').toLowerCase().includes(q);
      const matchApellido = (u.apellido || '').toLowerCase().includes(q);
      const matchEmail = (u.email || '').toLowerCase().includes(q);
      const matchDni = (u.dni || '').toLowerCase().includes(q);
      return matchNombre || matchApellido || matchEmail || matchDni;
    }

    return true;
  });
});

// Iniciales para el avatar
const getInitials = (nombre, apellido) => {
  const n = (nombre || '').trim()[0] || '';
  const a = (apellido || '').trim()[0] || '';
  return (n + a).toUpperCase() || 'U';
};

// Badges de roles
const getRoleBadgeClass = (rol) => {
  const r = (rol || '').toLowerCase();
  if (r.includes('admin')) return 'bg-purple-50 text-purple-700 border-purple-200';
  if (r.includes('dueño') && r.includes('jugador')) return 'bg-teal-50 text-teal-800 border-teal-200';
  if (r.includes('dueño') || r.includes('dueno')) return 'bg-amber-50 text-amber-800 border-amber-200';
  return 'bg-blue-50 text-[#2D9CDB] border-blue-200';
};

const getRoleIcon = (rol) => {
  const r = (rol || '').toLowerCase();
  if (r.includes('admin')) return 'fas fa-shield-halved';
  if (r.includes('dueño') && r.includes('jugador')) return 'fas fa-user-gear';
  if (r.includes('dueño') || r.includes('dueno')) return 'fas fa-building';
  return 'fas fa-futbol';
};

// Dar de baja
const darDeBaja = async (usuario) => {
  const confirmado = await requireConfirm({
    title: '¿Dar de baja usuario?',
    message: `¿Confirmás la baja de ${usuario.nombre} ${usuario.apellido || ''}?\nEl usuario permanecerá en el sistema con estado "Baja".`,
    confirmText: 'Dar de baja',
    cancelText: 'Cancelar',
    type: 'danger'
  });

  if (!confirmado) return;

  try {
    const response = await fetch(API_ENDPOINTS.usuarios.baja(usuario.id), {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' }
    });

    if (!response.ok) {
      const err = await response.json().catch(() => ({}));
      throw new Error(err.message || 'Error al dar de baja al usuario');
    }

    showToast('Usuario dado de baja exitosamente', 'success');
    await fetchUsuarios();
  } catch (error) {
    console.error('Error dar de baja:', error);
    showToast(error.message || 'Ocurrió un error al procesar la baja', 'error');
  }
};

// Reactivar
const reactivar = async (usuario) => {
  const confirmado = await requireConfirm({
    title: '¿Reactivar usuario?',
    message: `¿Deseás reactivar a ${usuario.nombre} ${usuario.apellido || ''}?\nSu estado volverá a ser "Activo".`,
    confirmText: 'Reactivar',
    cancelText: 'Cancelar',
    type: 'info'
  });

  if (!confirmado) return;

  try {
    const response = await fetch(API_ENDPOINTS.usuarios.reactivar(usuario.id), {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' }
    });

    if (!response.ok) {
      const err = await response.json().catch(() => ({}));
      throw new Error(err.message || 'Error al reactivar el usuario');
    }

    showToast('Usuario reactivado exitosamente', 'success');
    await fetchUsuarios();
  } catch (error) {
    console.error('Error reactivar:', error);
    showToast(error.message || 'Ocurrió un error al reactivar el usuario', 'error');
  }
};
</script>

<style scoped>
.no-scrollbar::-webkit-scrollbar,
.tabs-container::-webkit-scrollbar {
  display: none;
}

.no-scrollbar,
.tabs-container {
  -ms-overflow-style: none;
  scrollbar-width: none;
}

.animate-fade-in {
  animation: fadeIn 0.3s ease-out forwards;
}

.animate-slide-up {
  animation: slideUp 0.4s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}

.modal-fade-enter-active,
.modal-fade-leave-active {
  transition: opacity 0.25s ease;
}

.modal-fade-enter-from,
.modal-fade-leave-to {
  opacity: 0;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes slideUp {
  from {
    transform: translateY(20px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}
</style>
