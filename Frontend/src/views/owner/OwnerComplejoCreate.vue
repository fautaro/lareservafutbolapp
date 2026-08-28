<template>
  <div class="min-h-screen bg-[#F8FAFC] pb-32 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">
    <div class="animate-fade-in text-left">
      <!-- TOP NAVIGATION BAR -->
      <nav class="sticky top-4 z-50 bg-white/95 backdrop-blur-md shadow-sm border border-slate-100 mx-2 rounded-2xl px-3 py-3 flex items-center justify-between">
        <button @click="goBack" class="w-10 h-10 flex items-center justify-center text-slate-400 hover:text-slate-700 bg-slate-50 hover:bg-slate-100 rounded-xl transition-colors">
          <i class="fas fa-chevron-left text-sm"></i>
        </button>
        <h1 class="text-[13px] font-bold text-slate-700 tracking-tight leading-none">
          Nuevo Complejo
        </h1>
        <div class="w-10"></div>
      </nav>

      <section class="mt-6 px-4">
        <div v-if="isLoading" class="flex justify-center py-10">
          <i class="fas fa-circle-notch animate-spin text-2xl text-[#2D9CDB]"></i>
        </div>

        <form v-else @submit.prevent="submitForm" class="space-y-5 bg-white p-5 rounded-2xl shadow-sm border border-slate-100">
          <div class="space-y-1">
            <label class="text-xs font-bold text-slate-500 uppercase tracking-wider">Nombre del Complejo</label>
            <input v-model="form.nombre" type="text" required class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB] focus:ring-1 focus:ring-[#2D9CDB]" placeholder="Ej. La Reserva Fútbol">
          </div>

          <div class="space-y-1">
            <label class="text-xs font-bold text-slate-500 uppercase tracking-wider">Dirección</label>
            <input v-model="form.direccion" type="text" class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB] focus:ring-1 focus:ring-[#2D9CDB]" placeholder="Ej. Av. Siempreviva 742">
          </div>

          <div class="space-y-1">
            <label class="text-xs font-bold text-slate-500 uppercase tracking-wider">Ciudad</label>
            <select v-model="form.ciudadId" required class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB] focus:ring-1 focus:ring-[#2D9CDB]">
              <option value="" disabled>Seleccioná una ciudad</option>
              <option v-for="c in ciudades" :key="c.id" :value="c.id">{{ c.nombre }}</option>
            </select>
          </div>

          <div class="space-y-1">
            <label class="text-xs font-bold text-slate-500 uppercase tracking-wider">Deporte Principal</label>
            <select v-model="form.deporteId" class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB] focus:ring-1 focus:ring-[#2D9CDB]">
              <option value="" disabled>Seleccioná un deporte</option>
              <option v-for="d in deportes" :key="d.id" :value="d.id">{{ d.nombre }}</option>
            </select>
          </div>

          <div class="space-y-1">
            <label class="text-xs font-bold text-slate-500 uppercase tracking-wider">Precio Desde (ARS)</label>
            <input v-model="form.precio" type="number" class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB] focus:ring-1 focus:ring-[#2D9CDB]" placeholder="Ej. 15000">
          </div>

          <div class="space-y-1">
            <label class="text-xs font-bold text-slate-500 uppercase tracking-wider">URL Imagen (Opcional)</label>
            <input v-model="form.imagen" type="url" class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB] focus:ring-1 focus:ring-[#2D9CDB]" placeholder="https://...">
          </div>

          <button type="submit" :disabled="isSubmitting" class="w-full py-4 bg-[#2D9CDB] text-white rounded-xl font-bold text-sm hover:bg-[#2088c2] transition-colors mt-6 disabled:opacity-50">
            {{ isSubmitting ? 'Guardando...' : 'Crear Complejo' }}
          </button>
        </form>
      </section>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthUser } from '../../composables/useAuthUser'
import { API_ENDPOINTS } from '../../config/apiConfig'
import { useToast } from '../../composables/useToast'

export default {
  name: 'OwnerComplejoCreate',
  setup() {
    const router = useRouter()
    const { user } = useAuthUser()
    const { showToast } = useToast()
    const isLoading = ref(true)
    const isSubmitting = ref(false)
    
    const ciudades = ref([])
    const deportes = ref([])
    
    const form = ref({
      nombre: '',
      direccion: '',
      ciudadId: '',
      deporteId: '',
      precio: null,
      imagen: ''
    })

    const fetchRefData = async () => {
      try {
        const response = await fetch(API_ENDPOINTS.referenceData.getAll())
        if (response.ok) {
          const data = await response.json()
          ciudades.value = data.ciudades || []
          deportes.value = data.deportes || []
        }
      } catch (error) {
        console.error('Error fetching reference data:', error)
      } finally {
        isLoading.value = false
      }
    }

    const goBack = () => {
      router.push({ name: 'Home' })
    }

    const submitForm = async () => {
      isSubmitting.value = true
      try {
        const payload = {
          duenoId: user.value?.sub || 1,
          nombre: form.value.nombre,
          direccion: form.value.direccion,
          ciudadId: parseInt(form.value.ciudadId),
          deporteId: form.value.deporteId ? parseInt(form.value.deporteId) : null,
          precio: form.value.precio ? parseFloat(form.value.precio) : null,
          imagen: form.value.imagen || null
        }

        const response = await fetch(API_ENDPOINTS.complejos.create(), {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(payload)
        })

        if (response.ok) {
          router.push({ name: 'Home' })
        } else {
          showToast('Error al crear el complejo', 'error')
        }
      } catch (error) {
        console.error(error)
        showToast('Ocurrió un error', 'error')
      } finally {
        isSubmitting.value = false
      }
    }

    onMounted(() => {
      fetchRefData()
    })

    return {
      isLoading,
      isSubmitting,
      ciudades,
      deportes,
      form,
      goBack,
      submitForm
    }
  }
}
</script>

<style scoped>
.animate-fade-in {
  animation: fadeIn 0.4s ease-out;
}
@keyframes fadeIn {
  from { opacity: 0; transform: translateY(10px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
