<template>
  <div class="min-h-screen bg-[#F8FAFC] pb-32 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">
    
    <!-- MAIN APP FLOW -->
    <div class="animate-fade-in text-left">

      <!-- TOP NAVIGATION BAR -->
      <nav class="sticky top-4 z-50 bg-white/95 backdrop-blur-md shadow-sm border border-slate-100 mx-2 rounded-2xl px-3 py-3 flex items-center justify-between">
        <div class="flex flex-col items-center gap-1 min-w-[80px]">
          <svg class="h-8 w-auto" viewBox="0 0 52 40" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M0 0.19043V39.8487H30.8524L24.8072 29.1348H10.1606V0.19043H0Z" fill="#2D9CDB" />
            <path d="M37.7816 0.00362063C45.0797 -0.165953 51.306 5.64486 51.8218 12.4088C52.25 18.0208 48.8028 23.5772 43.2304 25.941L51.8563 39.849H38.6107L29.3726 26.1281H15.4243V17.5764H32.7911C33.1173 17.5808 34.5456 17.5662 35.7469 16.4493C36.7024 15.5605 36.6765 14.4714 36.7513 14.0884C36.7211 13.761 36.859 12.5404 35.8388 11.5624C34.5212 10.2994 32.8371 10.4982 32.6057 10.5289H15.4272V0.192198C15.4272 0.192198 37.3203 0.0138535 37.7816 0.00362063Z" fill="#2D9CDB" />
          </svg>
          <h1 class="text-[13px] font-bold text-[#2D9CDB] tracking-tight leading-none">
            Modo Dueño
          </h1>
        </div>
      </nav>

      <!-- OWNER STATS / GREETING -->
      <section class="mt-8 px-4">
        <div class="space-y-0.5">
          <h2 class="text-xl font-bold text-slate-900 tracking-tight">Mis Complejos</h2>
          <p class="text-[11px] text-slate-400 font-medium">Gestioná tus complejos y revisá sus agendas</p>
        </div>
      </section>

      <!-- COMPLEJOS LIST -->
      <section class="mt-6 px-1">
        <!-- LOADING STATE -->
        <div v-if="isLoading" class="flex flex-col items-center justify-center py-20">
          <i class="fas fa-circle-notch animate-spin text-3xl text-[#2D9CDB]"></i>
          <p class="mt-4 font-bold text-slate-500 text-sm">Cargando complejos...</p>
        </div>

        <div v-else class="space-y-6">
          <div v-for="complejo in complejos" :key="complejo.id" class="animate-slide-up">
            <div class="group relative bg-white rounded-2xl overflow-hidden shadow-sm border border-slate-100 hover:shadow-xl transition-all duration-500 cursor-pointer" @click="verAgenda(complejo.id)">
              
              <!-- Image Container (Simplified from Complex card) -->
              <div class="relative aspect-[16/10] overflow-hidden">
                <img class="w-full h-full object-cover transition-transform duration-700 group-hover:scale-110" :src="complejo.imagen || 'https://images.unsplash.com/photo-1544919982-b61976f0ba4a?q=80&w=800&auto=format&fit=crop'" :alt="complejo.nombre" />
                <!-- Gradient Overlay -->
                <div class="absolute inset-0 bg-gradient-to-t from-slate-900/60 to-transparent opacity-60"></div>
                
                <!-- Floating Labels -->
                <div class="absolute top-4 left-4">
                  <span v-if="complejo.categoria" class="bg-white/95 backdrop-blur-md text-[11px] font-black px-3.5 py-1.5 rounded-xl shadow-lg border border-white/50" :class="complejo.deportePillText">
                    {{ complejo.categoria }}
                  </span>
                </div>

                <div class="absolute bottom-5 left-5 right-5 flex items-end justify-between">
                  <div class="space-y-1">
                    <h3 class="text-2xl font-bold text-white tracking-tight drop-shadow-md">{{ complejo.nombre }}</h3>
                  </div>
                </div>
              </div>

              <!-- Footer Details -->
              <div class="px-5 py-5 flex items-center justify-between bg-white border-t border-slate-50">
                <div class="flex items-center gap-4">
                  <div class="flex flex-col">
                    <span class="text-[9px] font-black text-slate-300 uppercase tracking-widest leading-none mb-1">Precio x Hora (Desde)</span>
                    <div class="flex items-baseline gap-1">
                      <span class="text-base font-bold text-slate-900">{{ complejo.precio || '-' }}</span>
                      <span v-if="complejo.precio" class="text-[10px] font-bold text-slate-400">ARS</span>
                    </div>
                  </div>
                </div>

                <div class="flex items-center gap-2 text-[#2D9CDB] font-bold text-xs uppercase tracking-widest group-hover:translate-x-1 transition-transform">
                  <span>Ver Agenda</span>
                  <i class="fas fa-chevron-right text-[10px]"></i>
                </div>
              </div>
            </div>
          </div>

          <!-- EMPTY STATE REFINED -->
          <div v-if="complejos.length === 0" class="flex flex-col items-center justify-center py-20 text-center px-10">
            <div class="w-24 h-24 bg-slate-50 rounded-full flex items-center justify-center mb-6">
              <i class="fas fa-plus text-slate-200 text-4xl"></i>
            </div>
            <h3 class="text-lg font-bold text-slate-900">No tenés complejos</h3>
            <p class="text-sm text-slate-400 mt-2 leading-relaxed">Empezá agregando tu primer complejo para recibir reservas.</p>
            <button class="mt-8 px-8 py-3 bg-slate-100 text-slate-600 rounded-xl text-xs font-bold uppercase tracking-widest hover:bg-slate-200 transition-colors">
              Agregar Complejo
            </button>
          </div>
        </div>
      </section>

      <!-- QUICK ACTIONS -->
      <section v-if="complejos.length > 0 && !isLoading" class="mt-10 px-4">
         <button class="w-full py-4 border-2 border-dashed border-slate-200 rounded-2xl flex items-center justify-center gap-3 text-slate-400 hover:text-[#2D9CDB] hover:border-[#2D9CDB] transition-colors group">
            <i class="fas fa-plus text-sm group-hover:scale-110 transition-transform"></i>
            <span class="text-xs font-bold uppercase tracking-widest">Agregar otro complejo</span>
         </button>
      </section>

      <div class="h-10"></div>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthUser } from '../../composables/useAuthUser'
import { API_ENDPOINTS } from '../../config/apiConfig'

export default {
  name: 'OwnerHome',
  setup() {
    const router = useRouter()
    const { user } = useAuthUser()
    const complejos = ref([])
    const isLoading = ref(true)

    const fetchComplejos = async () => {
      isLoading.value = true
      try {
        const ownerId = user.value?.sub || 1 // Fallback para dev
        const response = await fetch(API_ENDPOINTS.complejos.getAll(ownerId))
        
        if (!response.ok) throw new Error('Error al cargar los complejos')
        
        const data = await response.json()
        complejos.value = data.complejos || []
      } catch (error) {
        console.error('Error fetching owner complejos:', error)
      } finally {
        isLoading.value = false
      }
    }

    const verAgenda = (complejoId) => {
      router.push({ name: 'OwnerAgenda', params: { id: complejoId } })
    }

    onMounted(() => {
      fetchComplejos()
    })

    return {
      complejos,
      isLoading,
      verAgenda
    }
  }
}
</script>

<style scoped>
.animate-fade-in {
  animation: fadeIn 0.6s cubic-bezier(0.16, 1, 0.3, 1);
}

.animate-slide-up {
  animation: slideUp 0.5s cubic-bezier(0.16, 1, 0.3, 1);
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

@keyframes slideUp {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}
</style>
