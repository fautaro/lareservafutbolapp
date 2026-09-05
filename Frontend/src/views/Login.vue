<template>
  <div class="min-h-screen flex items-center justify-center p-4"
    style="font-family: Inter, 'Noto Sans', sans-serif; background-color: #F9FAFB;">
    <div class="w-full max-w-md">
      <!-- Card principal -->
      <div class="bg-white rounded-2xl shadow-xl p-8 border border-gray-100">
        <!-- Logo centrado -->
        <div class="text-center mb-8">
          <div class="flex justify-center mb-4">
            <img src="/src/assets/logo.svg" alt="Logo La Reserva" class="w-24 h-24 drop-shadow-lg" />
          </div>
          <h1 class="text-3xl font-bold text-gray-800 mb-2">
            La Reserva Fútbol
          </h1>
          <p class="text-gray-500 text-sm">Reserva tu cancha con facilidad</p>
        </div>

        <!-- Divider decorativo -->
        <div class="flex items-center mb-8">
          <div class="flex-1 border-t border-gray-200"></div>
          <div class="px-4">
            <svg class="w-5 h-5 text-gray-400" fill="currentColor" viewBox="0 0 20 20">
              <path
                d="M10 2a6 6 0 00-6 6v3.586l-.707.707A1 1 0 004 14h12a1 1 0 00.707-1.707L16 11.586V8a6 6 0 00-6-6zM10 18a3 3 0 01-3-3h6a3 3 0 01-3 3z" />
            </svg>
          </div>
          <div class="flex-1 border-t border-gray-200"></div>
        </div>

        <!-- Formulario de Login Local (Email / Password) -->
        <form @submit.prevent="loginWithEmailPassword" class="space-y-4 mb-6">
          <div class="space-y-1.5 text-left">
            <label class="block text-xs font-bold uppercase tracking-wider text-slate-500">
              Correo Electrónico
            </label>
            <input
              v-model="email"
              type="email"
              required
              placeholder="tu@email.com"
              class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm text-slate-800 placeholder-slate-400 focus:outline-none focus:border-[#2D9CDB] focus:bg-white focus:ring-1 focus:ring-[#2D9CDB] transition-all"
            />
          </div>

          <div class="space-y-1.5 text-left">
            <label class="block text-xs font-bold uppercase tracking-wider text-slate-500">
              Contraseña
            </label>
            <div class="relative">
              <input
                :type="showPassword ? 'text' : 'password'"
                v-model="password"
                required
                placeholder="Ingresá tu contraseña"
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

          <!-- Mensaje de error -->
          <div v-if="error" class="p-3 bg-rose-50 border border-rose-200 rounded-xl flex items-center gap-2 text-rose-700 text-xs font-medium text-left">
            <i class="fas fa-exclamation-circle text-rose-500 shrink-0"></i>
            <span>{{ error }}</span>
          </div>

          <!-- Botón Iniciar Sesión -->
          <button
            type="submit"
            :disabled="isLoading"
            class="w-full text-white font-semibold py-3.5 px-6 rounded-xl transition-all duration-300 ease-in-out transform hover:scale-[1.01] hover:shadow-lg active:scale-[0.98] flex items-center justify-center gap-3 text-sm"
            style="background: #2D9CDB;"
          >
            <i v-if="isLoading" class="fas fa-circle-notch animate-spin text-sm"></i>
            <span>{{ isLoading ? 'Ingresando...' : 'Iniciar Sesión' }}</span>
          </button>
        </form>

        <!-- Divider decorativo -->
        <div class="flex items-center mb-6">
          <div class="flex-1 border-t border-gray-200"></div>
          <div class="px-3 text-xs uppercase font-bold text-gray-400">
            o con Auth0
          </div>
          <div class="flex-1 border-t border-gray-200"></div>
        </div>

        <!-- Botón de login con Auth0 -->
        <button @click="loginWithAuth0"
          type="button"
          class="w-full bg-slate-50 hover:bg-slate-100 text-slate-700 font-semibold py-3 px-6 rounded-xl border border-slate-200 transition-all duration-200 active:scale-[0.98] flex items-center justify-center gap-2 text-xs uppercase tracking-wider">
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
              d="M11 16l-4-4m0 0l4-4m-4 4h14m-5 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h7a3 3 0 013 3v1" />
          </svg>
          Ingresar con Auth0
        </button>
      </div>
    </div>
  </div>
</template>
<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthUser } from '../composables/useAuthUser';
import { auth0Config } from '../config/auth0Config';
import { API_ENDPOINTS } from '../config/apiConfig';

const { login: loginRedirect, setLocalSession } = useAuthUser();
const router = useRouter();

const email = ref('');
const password = ref('');
const showPassword = ref(false);
const isLoading = ref(false);
const error = ref(null);

const loginWithEmailPassword = async () => {
  if (!email.value.trim() || !password.value) {
    error.value = 'Por favor ingresá tu email y contraseña.';
    return;
  }

  try {
    isLoading.value = true;
    error.value = null;

    const response = await fetch(API_ENDPOINTS.usuarios.login(), {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        email: email.value.trim(),
        password: password.value
      })
    });

    const data = await response.json().catch(() => ({}));

    if (!response.ok) {
      throw new Error(data.message || 'Credenciales inválidas o error de autenticación.');
    }

    // Guardar sesión local (incluye 'debeCambiarPassword')
    setLocalSession(data);

    // Redireccionar al home o ruta correspondiente
    router.push('/');
  } catch (err) {
    console.error('Error en login:', err);
    error.value = err.message || 'Error al iniciar sesión.';
  } finally {
    isLoading.value = false;
  }
};

const loginWithAuth0 = async () => {
  try {
    isLoading.value = true;
    error.value = null;

    if (auth0Config.loginDisabled) {
      router.push('/');
      return;
    }

    await loginRedirect({
      appState: { returnTo: '/' }
    });
  } catch (err) {
    console.error('Error en login Auth0:', err);
    error.value = 'Error al iniciar sesión con Auth0. Por favor, intenta de nuevo.';
    isLoading.value = false;
  }
};
</script>

<style scoped>
@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}

.animate-spin {
  animation: spin 1s linear infinite;
}

@keyframes pulse-subtle {

  0%,
  100% {
    opacity: 1;
  }

  50% {
    opacity: 0.9;
  }
}
</style>
