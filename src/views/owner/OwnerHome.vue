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
        <div class="px-4 py-2 bg-blue-50 rounded-xl">
           <span class="text-[10px] font-black text-[#2D9CDB] uppercase tracking-widest">Panel Control</span>
        </div>
      </nav>

      <!-- OWNER STATS / GREETING -->
      <section class="mt-8 px-4">
        <h2 class="text-2xl font-bold text-slate-900 tracking-tight">Mis Canchas</h2>
        <p class="text-sm text-slate-400 mt-1">Gestioná tus espacios y revisá la agenda</p>
      </section>

      <!-- CANCHAS LIST -->
      <section class="mt-6 px-1">
        <div class="space-y-6">
          <div v-for="cancha in canchas" :key="cancha.id" class="animate-slide-up">
            <div class="group relative bg-white rounded-2xl overflow-hidden shadow-sm border border-slate-100 hover:shadow-lg transition-all duration-500">
              
              <!-- Image Container (Simplified from Complex card) -->
              <div class="relative aspect-[16/8] overflow-hidden">
                <img class="w-full h-full object-cover" :src="cancha.imagen" :alt="cancha.nombre" />
                <div class="absolute inset-0 bg-gradient-to-t from-slate-900/50 to-transparent opacity-60"></div>
                
                <div class="absolute top-4 left-4">
                  <span class="bg-white/95 backdrop-blur-md text-[10px] font-black px-3 py-1 rounded-lg shadow-sm border border-white/50 text-[#2D9CDB] uppercase tracking-wider">
                    {{ cancha.tipo }}
                  </span>
                </div>

                <div class="absolute bottom-4 left-5">
                  <h3 class="text-xl font-bold text-white tracking-tight drop-shadow-md">{{ cancha.nombre }}</h3>
                </div>
              </div>

              <!-- Footer Actions -->
              <div class="px-5 py-4 flex items-center justify-between bg-white">
                <div class="flex flex-col">
                  <span class="text-[9px] font-black text-slate-300 uppercase tracking-widest leading-none mb-1">Precio x Hora</span>
                  <div class="flex items-baseline gap-1">
                    <span class="text-base font-bold text-slate-900">${{ cancha.precio }}</span>
                    <span class="text-[10px] font-bold text-slate-400">ARS</span>
                  </div>
                </div>

                <button @click="verAgenda(cancha.id)" class="flex items-center gap-2 bg-[#2D9CDB] text-white px-5 py-2.5 rounded-xl font-bold text-[11px] uppercase tracking-widest shadow-lg shadow-blue-200 active:scale-95 transition-all">
                  <span>Ver Agenda</span>
                  <i class="fas fa-calendar-days text-[10px]"></i>
                </button>
              </div>
            </div>
          </div>

          <!-- EMPTY STATE -->
          <div v-if="canchas.length === 0" class="flex flex-col items-center justify-center py-20 text-center px-10">
            <div class="w-20 h-20 bg-slate-50 rounded-full flex items-center justify-center mb-6 text-slate-200">
              <i class="fas fa-plus text-3xl"></i>
            </div>
            <h3 class="text-lg font-bold text-slate-900">No tenés canchas</h3>
            <p class="text-sm text-slate-400 mt-2">Empezá agregando tu primer cancha para recibir reservas.</p>
            <button class="mt-8 px-8 py-3 bg-[#2D9CDB] text-white rounded-xl text-xs font-bold uppercase tracking-widest shadow-lg active:scale-95 transition-all">
              Agregar Cancha
            </button>
          </div>
        </div>
      </section>

      <!-- QUICK ACTIONS -->
      <section v-if="canchas.length > 0" class="mt-10 px-4">
         <button class="w-full py-4 border-2 border-dashed border-slate-200 rounded-2xl flex items-center justify-center gap-3 text-slate-400 hover:text-[#2D9CDB] hover:border-[#2D9CDB] transition-colors group">
            <i class="fas fa-plus text-sm group-hover:scale-110 transition-transform"></i>
            <span class="text-xs font-bold uppercase tracking-widest">Agregar otra cancha</span>
         </button>
      </section>

      <div class="h-10"></div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'OwnerHome',
  data() {
    return {
      canchas: [
        {
          id: 1,
          nombre: 'Cancha Principal - El Fortín',
          tipo: 'Fútbol 5',
          precio: '12000',
          imagen: 'https://images.unsplash.com/photo-1544919982-b61976f0ba4a?q=80&w=800&auto=format&fit=crop'
        },
        {
          id: 2,
          nombre: 'Cancha 2 - Turf Pro',
          tipo: 'Fútbol 7',
          precio: '18000',
          imagen: 'https://images.unsplash.com/photo-1574629810360-7efbbe195018?q=80&w=800&auto=format&fit=crop'
        }
      ]
    }
  },
  methods: {
    verAgenda(canchaId) {
      this.$router.push({ name: 'OwnerAgenda', params: { id: canchaId } })
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
