<template>
  <div class="min-h-screen bg-[#F8FAFC] pb-32 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">
    <div class="animate-fade-in text-left">
      <!-- ══ TOP NAVIGATION BAR ══ -->
      <nav class="sticky top-4 z-50 bg-white/95 backdrop-blur-md shadow-sm border border-slate-100 mx-2 rounded-2xl px-3 py-3 flex items-center justify-between">
        <button
          type="button"
          @click="goBack"
          class="w-10 h-10 flex items-center justify-center text-slate-400 hover:text-slate-700 bg-slate-50 hover:bg-slate-100 rounded-xl transition-all active:scale-95"
          title="Volver al listado"
        >
          <i class="fas fa-chevron-left text-sm"></i>
        </button>

        <h1 class="text-[13px] font-bold text-slate-700 tracking-tight leading-none">
          Alta de Usuario
        </h1>

        <div class="w-10"></div>
      </nav>

      <!-- ══ CONTENIDO DEL FORMULARIO ══ -->
      <section class="mt-6 px-3 sm:px-4 max-w-xl mx-auto">
        <!-- Header de sección (Estilo Modo Jugador) -->
        <div class="mb-5 px-1">
          <h2 class="text-2xl sm:text-3xl font-bold tracking-tight text-slate-900 leading-none">
            Nuevo Usuario
          </h2>
          <p class="text-slate-400 text-xs sm:text-sm font-medium mt-2">
            Completá los datos personales y roles para dar de alta un usuario en el sistema.
          </p>
        </div>

        <!-- Formulario -->
        <form @submit.prevent="submitCreateUsuario" class="bg-white rounded-[24px] border border-slate-100 p-5 sm:p-6 shadow-sm space-y-5">
          <!-- Nombre -->
          <div class="space-y-1.5">
            <label class="block text-xs font-bold uppercase tracking-wider text-slate-500">
              Nombre <span class="text-rose-500">*</span>
            </label>
            <input
              v-model="form.nombre"
              type="text"
              required
              placeholder="Ej: Lionel"
              class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm text-slate-800 placeholder-slate-400 focus:outline-none focus:border-[#2D9CDB] focus:bg-white focus:ring-1 focus:ring-[#2D9CDB] transition-all"
            />
          </div>

          <!-- Apellido -->
          <div class="space-y-1.5">
            <label class="block text-xs font-bold uppercase tracking-wider text-slate-500">
              Apellido <span class="text-rose-500">*</span>
            </label>
            <input
              v-model="form.apellido"
              type="text"
              required
              placeholder="Ej: Messi"
              class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm text-slate-800 placeholder-slate-400 focus:outline-none focus:border-[#2D9CDB] focus:bg-white focus:ring-1 focus:ring-[#2D9CDB] transition-all"
            />
          </div>

          <!-- DNI -->
          <div class="space-y-1.5">
            <label class="block text-xs font-bold uppercase tracking-wider text-slate-500">
              DNI <span class="text-rose-500">*</span>
            </label>
            <input
              v-model="form.dni"
              type="text"
              required
              placeholder="Ej: 33123456"
              class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm text-slate-800 placeholder-slate-400 focus:outline-none focus:border-[#2D9CDB] focus:bg-white focus:ring-1 focus:ring-[#2D9CDB] transition-all"
            />
          </div>

          <!-- Email -->
          <div class="space-y-1.5">
            <label class="block text-xs font-bold uppercase tracking-wider text-slate-500">
              Email <span class="text-rose-500">*</span>
            </label>
            <input
              v-model="form.email"
              type="email"
              required
              placeholder="Ej: usuario@reserva.com"
              class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm text-slate-800 placeholder-slate-400 focus:outline-none focus:border-[#2D9CDB] focus:bg-white focus:ring-1 focus:ring-[#2D9CDB] transition-all"
            />
          </div>

          <!-- Roles / Permisos (Switches estilo Modo Jugador / iOS) -->
          <div class="space-y-1.5 pt-1">
            <label class="block text-xs font-bold uppercase tracking-wider text-slate-500">
              Roles y Permisos
            </label>
            <div class="bg-slate-50/80 rounded-2xl border border-slate-200/70 divide-y divide-slate-200/60 overflow-hidden">
              <!-- Switch ¿Es dueño? -->
              <div class="p-4 flex items-center justify-between gap-3">
                <div class="space-y-0.5 min-w-0">
                  <span class="text-sm font-bold text-slate-800 block leading-tight">¿Es dueño?</span>
                  <span class="text-xs text-slate-400 block leading-normal">Habilita la gestión de complejos deportivos</span>
                </div>
                <label class="relative inline-flex items-center cursor-pointer shrink-0">
                  <input type="checkbox" v-model="form.esDueno" class="sr-only peer" />
                  <div
                    class="w-12 h-7 bg-slate-200 rounded-full transition-colors shadow-inner"
                    :class="form.esDueno ? 'bg-[#2D9CDB]' : 'bg-slate-300'"
                  ></div>
                  <div
                    class="absolute left-1 top-1 w-5 h-5 bg-white rounded-full transition-transform shadow-sm"
                    :class="form.esDueno ? 'translate-x-5' : ''"
                  ></div>
                </label>
              </div>

              <!-- Switch ¿Es administrador? -->
              <div class="p-4 flex items-center justify-between gap-3">
                <div class="space-y-0.5 min-w-0">
                  <span class="text-sm font-bold text-slate-800 block leading-tight">¿Es administrador?</span>
                  <span class="text-xs text-slate-400 block leading-normal">Asigna permisos de administración y gestión para personal</span>
                </div>
                <label class="relative inline-flex items-center cursor-pointer shrink-0">
                  <input type="checkbox" v-model="form.esAdmin" class="sr-only peer" />
                  <div
                    class="w-12 h-7 bg-slate-200 rounded-full transition-colors shadow-inner"
                    :class="form.esAdmin ? 'bg-purple-600' : 'bg-slate-300'"
                  ></div>
                  <div
                    class="absolute left-1 top-1 w-5 h-5 bg-white rounded-full transition-transform shadow-sm"
                    :class="form.esAdmin ? 'translate-x-5' : ''"
                  ></div>
                </label>
              </div>
            </div>
          </div>

          <!-- Botones de Acción -->
          <div class="pt-4 flex flex-col sm:flex-row gap-3">
            <button
              type="button"
              @click="goBack"
              class="w-full sm:flex-1 py-3.5 bg-slate-100 hover:bg-slate-200 text-slate-600 rounded-xl text-xs font-bold uppercase tracking-widest transition-all active:scale-95"
            >
              Cancelar
            </button>
            <button
              type="submit"
              :disabled="isSubmitting"
              class="w-full sm:flex-1 py-3.5 bg-[#2D9CDB] hover:bg-[#2088c2] text-white rounded-xl text-xs font-bold uppercase tracking-widest shadow-lg shadow-blue-500/20 active:scale-95 transition-all flex items-center justify-center gap-2 disabled:opacity-50"
            >
              <i v-if="isSubmitting" class="fas fa-circle-notch animate-spin text-xs"></i>
              <span>{{ isSubmitting ? 'Guardando...' : 'Crear Usuario' }}</span>
            </button>
          </div>
        </form>
      </section>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { API_ENDPOINTS } from '../../config/apiConfig';
import { useToast } from '../../composables/useToast';

const router = useRouter();
const { showToast } = useToast();

const isSubmitting = ref(false);
const form = ref({
  nombre: '',
  apellido: '',
  dni: '',
  email: '',
  esDueno: false,
  esAdmin: false
});

const goBack = () => {
  router.push({ name: 'AdminUsuarios' });
};

const submitCreateUsuario = async () => {
  if (!form.value.nombre.trim() || !form.value.apellido.trim() || !form.value.dni.trim() || !form.value.email.trim()) {
    showToast('Por favor completá todos los campos requeridos', 'warning');
    return;
  }

  isSubmitting.value = true;
  try {
    const payload = {
      nombre: form.value.nombre.trim(),
      apellido: form.value.apellido.trim(),
      dni: form.value.dni.trim(),
      email: form.value.email.trim(),
      esDueno: form.value.esDueno,
      esAdmin: form.value.esAdmin
    };

    const response = await fetch(API_ENDPOINTS.usuarios.create(), {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload)
    });

    if (!response.ok) {
      const err = await response.json().catch(() => ({}));
      throw new Error(err.message || 'Error al dar de alta el usuario');
    }

    showToast('Usuario creado exitosamente', 'success');
    router.push({ name: 'AdminUsuarios' });
  } catch (error) {
    console.error('Error creando usuario:', error);
    showToast(error.message || 'Error al crear el usuario', 'error');
  } finally {
    isSubmitting.value = false;
  }
};
</script>

<style scoped>
.animate-fade-in {
  animation: fadeIn 0.3s ease-out forwards;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(4px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>
