<template>
  <div v-if="isOpen" class="fixed inset-0 z-[9999] flex items-center justify-center p-4 bg-slate-900/80 backdrop-blur-sm animate-fade-in" style="font-family: Inter, 'Noto Sans', sans-serif;">
    <div class="bg-white rounded-3xl shadow-2xl border border-slate-100 max-w-md w-full p-6 sm:p-8 text-left relative transform transition-all animate-scale-up">
      <!-- Icono de advertencia / llave -->
      <div class="w-14 h-14 bg-amber-50 rounded-2xl flex items-center justify-center text-amber-500 mb-5 border border-amber-100 shadow-inner">
        <i class="fas fa-key text-2xl"></i>
      </div>

      <!-- Título y descripción -->
      <h2 class="text-xl sm:text-2xl font-black text-slate-900 tracking-tight leading-tight">
        Cambio de contraseña obligatorio
      </h2>
      <p class="text-xs sm:text-sm text-slate-500 mt-2 leading-relaxed">
        Tu cuenta tiene una contraseña temporal asignada por el administrador (<span class="font-bold text-slate-700">1234</span>). Por seguridad, debés definir una nueva contraseña para poder utilizar la aplicación.
      </p>

      <!-- Formulario -->
      <form @submit.prevent="handleSubmit" class="mt-6 space-y-4">
        <!-- Nueva contraseña -->
        <div class="space-y-1.5">
          <label class="block text-xs font-bold uppercase tracking-wider text-slate-500">
            Nueva Contraseña <span class="text-rose-500">*</span>
          </label>
          <div class="relative">
            <input
              :type="showPassword ? 'text' : 'password'"
              v-model="nuevaPassword"
              required
              placeholder="Ingresá tu nueva clave"
              class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm text-slate-800 placeholder-slate-400 focus:outline-none focus:border-[#2D9CDB] focus:bg-white focus:ring-1 focus:ring-[#2D9CDB] transition-all pr-11"
            />
            <button
              type="button"
              @click="showPassword = !showPassword"
              class="absolute inset-y-0 right-0 pr-3.5 flex items-center text-slate-400 hover:text-slate-600 focus:outline-none"
            >
              <i :class="showPassword ? 'fas fa-eye-slash' : 'fas fa-eye'" class="text-xs"></i>
            </button>
          </div>
        </div>

        <!-- Confirmar contraseña -->
        <div class="space-y-1.5">
          <label class="block text-xs font-bold uppercase tracking-wider text-slate-500">
            Confirmar Contraseña <span class="text-rose-500">*</span>
          </label>
          <div class="relative">
            <input
              :type="showConfirmPassword ? 'text' : 'password'"
              v-model="confirmarPassword"
              required
              placeholder="Repetí tu nueva clave"
              class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm text-slate-800 placeholder-slate-400 focus:outline-none focus:border-[#2D9CDB] focus:bg-white focus:ring-1 focus:ring-[#2D9CDB] transition-all pr-11"
            />
            <button
              type="button"
              @click="showConfirmPassword = !showConfirmPassword"
              class="absolute inset-y-0 right-0 pr-3.5 flex items-center text-slate-400 hover:text-slate-600 focus:outline-none"
            >
              <i :class="showConfirmPassword ? 'fas fa-eye-slash' : 'fas fa-eye'" class="text-xs"></i>
            </button>
          </div>
        </div>

        <!-- Mensaje de error -->
        <div v-if="errorMessage" class="p-3 bg-rose-50 border border-rose-200 rounded-xl flex items-center gap-2 text-rose-700 text-xs font-medium">
          <i class="fas fa-exclamation-circle text-rose-500 shrink-0"></i>
          <span>{{ errorMessage }}</span>
        </div>

        <!-- Botones de Acción -->
        <div class="pt-2 flex flex-col gap-2.5">
          <button
            type="submit"
            :disabled="isLoading"
            class="w-full py-3.5 bg-[#2D9CDB] hover:bg-[#2088c2] text-white rounded-xl text-xs font-bold uppercase tracking-widest shadow-lg shadow-blue-500/20 active:scale-95 transition-all flex items-center justify-center gap-2 disabled:opacity-50"
          >
            <i v-if="isLoading" class="fas fa-circle-notch animate-spin text-xs"></i>
            <span>{{ isLoading ? 'Actualizando...' : 'Guardar y Continuar' }}</span>
          </button>

          <button
            type="button"
            @click="handleLogout"
            class="w-full py-2.5 text-slate-400 hover:text-slate-600 text-xs font-semibold tracking-wide transition-colors"
          >
            Cerrar sesión
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useAuthUser } from '../composables/useAuthUser';
import { API_ENDPOINTS } from '../config/apiConfig';
import { useToast } from '../composables/useToast';

const { user, debeCambiarPassword, setDebeCambiarPassword, logout } = useAuthUser();
const { showToast } = useToast();

const isOpen = computed(() => !!debeCambiarPassword.value);

const nuevaPassword = ref('');
const confirmarPassword = ref('');
const showPassword = ref(false);
const showConfirmPassword = ref(false);
const isLoading = ref(false);
const errorMessage = ref('');

const handleSubmit = async () => {
  errorMessage.value = '';

  const pass = nuevaPassword.value.trim();
  const confirm = confirmarPassword.value.trim();

  if (!pass || !confirm) {
    errorMessage.value = 'Por favor completá ambos campos de contraseña.';
    return;
  }

  if (pass.length < 4) {
    errorMessage.value = 'La nueva contraseña debe tener al menos 4 caracteres.';
    return;
  }

  if (pass === '1234') {
    errorMessage.value = 'La nueva contraseña no puede ser la temporal 1234.';
    return;
  }

  if (pass !== confirm) {
    errorMessage.value = 'Las contraseñas no coinciden.';
    return;
  }

  isLoading.value = true;
  try {
    const userId = user.value?.id || user.value?.sub;
    if (!userId) {
      throw new Error('No se pudo identificar al usuario actual.');
    }

    const response = await fetch(API_ENDPOINTS.usuarios.cambiarPassword(), {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        usuarioId: userId,
        nuevaPassword: pass
      })
    });

    const data = await response.json().catch(() => ({}));

    if (!response.ok) {
      throw new Error(data.message || 'Error al actualizar la contraseña.');
    }

    setDebeCambiarPassword(false);
    nuevaPassword.value = '';
    confirmarPassword.value = '';
    showToast('¡Contraseña cambiada exitosamente! Ya podés usar la app.', 'success');
  } catch (err) {
    console.error('Error al cambiar contraseña obligatoria:', err);
    errorMessage.value = err.message || 'Ocurrió un error al actualizar la contraseña.';
  } finally {
    isLoading.value = false;
  }
};

const handleLogout = () => {
  logout();
};
</script>

<style scoped>
.animate-fade-in {
  animation: fadeIn 0.25s ease-out forwards;
}

.animate-scale-up {
  animation: scaleUp 0.25s cubic-bezier(0.16, 1, 0.3, 1) forwards;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes scaleUp {
  from {
    opacity: 0;
    transform: scale(0.95);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}
</style>
