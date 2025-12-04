<template>
  <div class="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 via-white to-green-50 p-4" style="font-family: Inter, 'Noto Sans', sans-serif;">
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
              <path d="M10 2a6 6 0 00-6 6v3.586l-.707.707A1 1 0 004 14h12a1 1 0 00.707-1.707L16 11.586V8a6 6 0 00-6-6zM10 18a3 3 0 01-3-3h6a3 3 0 01-3 3z"/>
            </svg>
          </div>
          <div class="flex-1 border-t border-gray-200"></div>
        </div>

        <!-- Botón de login -->
        <button
          @click="loginWithAuth0" 
          class="w-full text-white font-semibold py-4 px-6 rounded-xl transition-all duration-300 ease-in-out transform hover:scale-[1.02] hover:shadow-lg active:scale-[0.98] flex items-center justify-center gap-3 text-base"
          style="background: linear-gradient(to right, #2563eb, #1d4ed8);"
          @mouseover="$event.target.style.background = 'linear-gradient(to right, #1d4ed8, #1e40af)'"
          @mouseout="$event.target.style.background = 'linear-gradient(to right, #2563eb, #1d4ed8)'"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 16l-4-4m0 0l4-4m-4 4h14m-5 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h7a3 3 0 013 3v1"/>
          </svg>
          Ingresar
        </button>
      </div>
    </div>
  </div>
</template>


<script setup>
import { ref, onMounted } from 'vue';
import { useAuth0 } from '@auth0/auth0-vue';
import { useRouter, useRoute } from 'vue-router';

const { loginWithRedirect, isLoading: auth0IsLoading, error: auth0Error } = useAuth0();
const router = useRouter();
const route = useRoute();
const isLoading = ref(false);
const error = ref(null);

// Manejar redirección si viene con código de autorización
onMounted(() => {
  // El SDK de Auth0 maneja automáticamente el callback
  // Si el usuario ya está autenticado, redirigir a home
});

const loginWithAuth0 = async () => {
  try {
    isLoading.value = true;
    error.value = null;
    
    await loginWithRedirect({
      appState: { returnTo: '/' }
    });
  } catch (err) {
    console.error('Error en login:', err);
    error.value = 'Error al iniciar sesión. Por favor, intenta de nuevo.';
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
  0%, 100% {
    opacity: 1;
  }
  50% {
    opacity: 0.9;
  }
}
</style>
