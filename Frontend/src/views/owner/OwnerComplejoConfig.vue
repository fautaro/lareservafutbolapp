<template>
  <div class="min-h-screen bg-[#F8FAFC] pb-32 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">
    <div class="animate-fade-in text-left">
      <!-- TOP NAVIGATION BAR -->
      <nav class="sticky top-4 z-50 bg-white/95 backdrop-blur-md shadow-sm border border-slate-100 mx-2 rounded-2xl px-3 py-3 flex items-center justify-between">
        <button @click="goBack" class="w-10 h-10 flex items-center justify-center text-slate-400 hover:text-slate-700 bg-slate-50 hover:bg-slate-100 rounded-xl transition-colors">
          <i class="fas fa-chevron-left text-sm"></i>
        </button>
        <h1 class="text-[13px] font-bold text-slate-700 tracking-tight leading-none text-center truncate px-2">
          Configuración: {{ complejo?.nombre || 'Cargando...' }}
        </h1>
        <div class="w-10"></div>
      </nav>

      <div v-if="isLoading" class="flex justify-center py-20">
        <i class="fas fa-circle-notch animate-spin text-3xl text-[#2D9CDB]"></i>
      </div>

      <div v-else-if="complejo">
        <!-- DETALLES DEL COMPLEJO -->
        <section class="mt-6 px-4">
          <div class="bg-white rounded-2xl p-5 border border-slate-100 shadow-sm space-y-4">
            <!-- Fila superior: Imagen, Nombre y Visibilidad -->
            <div class="flex items-center justify-between gap-3">
              <div class="flex items-center gap-3.5 min-w-0">
                <div class="relative w-14 h-14 shrink-0">
                  <div class="w-14 h-14 rounded-xl overflow-hidden bg-slate-100">
                    <img v-if="complejo.imagen" :src="complejo.imagen" class="w-full h-full object-cover" />
                    <div v-else class="w-full h-full flex items-center justify-center text-slate-300">
                      <i class="fas fa-image text-lg"></i>
                    </div>
                  </div>
                  <button @click="openImagenModal" class="absolute -bottom-1 -right-1 w-6 h-6 rounded-full bg-white border border-slate-200 shadow-sm flex items-center justify-center text-slate-500 hover:text-[#2D9CDB] hover:bg-slate-50 transition-colors" title="Editar imagen">
                    <i class="fas fa-pencil-alt text-[9px]"></i>
                  </button>
                </div>
                <div class="min-w-0">
                  <h2 class="font-bold text-slate-800 text-lg leading-tight truncate">{{ complejo.nombre }}</h2>
                  <span class="text-xs text-slate-400 block mt-0.5">{{ complejo.categoria || 'Complejo deportivo' }}</span>
                </div>
              </div>

              <div class="flex items-center gap-2 shrink-0">
                <span class="text-[10px] font-black uppercase tracking-wider px-2.5 py-1 rounded-full" :class="complejo.estado ? 'bg-emerald-50 text-emerald-600 border border-emerald-100' : 'bg-rose-50 text-rose-500 border border-rose-100'">
                  {{ complejo.estado ? 'Visible' : 'Oculto' }}
                </span>
                <button @click="toggleComplejoEstado" class="text-[11px] font-bold text-slate-500 hover:text-[#2D9CDB] bg-slate-50 hover:bg-slate-100 border border-slate-200 rounded-lg px-2.5 py-1.5 transition-colors">
                  {{ complejo.estado ? 'Ocultar' : 'Mostrar' }}
                </button>
              </div>
            </div>

            <!-- Fila inferior: Ubicación completa sin compresión -->
            <div class="pt-3 border-t border-slate-100 flex items-center justify-between gap-3">
              <div class="flex items-center gap-2 min-w-0 flex-1">
                <i class="fas fa-map-marker-alt text-[#2D9CDB] shrink-0 text-sm"></i>
                <span class="text-xs font-medium text-slate-700 truncate">
                  {{ complejo.direccion || 'Sin dirección' }} <span class="text-slate-400 font-normal">• {{ complejo.ciudadNombre }}</span>
                </span>
              </div>
              <button @click="openDireccionModal" class="text-xs font-bold text-[#2D9CDB] hover:text-[#2088c2] bg-blue-50/60 hover:bg-blue-50 border border-blue-100 px-3 py-1.5 rounded-lg transition-colors flex items-center gap-1.5 shrink-0" title="Editar dirección y ciudad">
                <i class="fas fa-pencil-alt text-[9px]"></i>
                <span>Editar</span>
              </button>
            </div>
          </div>
        </section>

        <!-- CANCHAS LIST -->
        <section class="mt-8 px-4">
          <div class="flex items-center justify-between mb-4">
            <h3 class="font-bold text-slate-800">Canchas ({{ complejo.canchas?.length || 0 }})</h3>
            <button @click="openModal" class="text-xs font-bold text-[#2D9CDB] bg-blue-50 px-3 py-1.5 rounded-lg hover:bg-blue-100 transition-colors">
              <i class="fas fa-plus mr-1"></i> Agregar
            </button>
          </div>

          <div v-if="complejo.canchas?.length === 0" class="bg-white rounded-2xl p-8 text-center border border-slate-100 border-dashed">
            <div class="w-12 h-12 bg-slate-50 rounded-full flex items-center justify-center mx-auto mb-3">
              <i class="fas fa-futbol text-slate-300"></i>
            </div>
            <p class="text-sm font-bold text-slate-600">No hay canchas registradas</p>
            <p class="text-xs text-slate-400 mt-1">Agregá tu primera cancha para empezar a recibir reservas.</p>
          </div>

          <div v-else class="space-y-3">
            <div v-for="cancha in complejo.canchas" :key="cancha.id" class="bg-white rounded-2xl border border-slate-100 p-4 shadow-sm space-y-3" :class="{'opacity-75 bg-slate-50/50': !cancha.estado}">
              <!-- Fila superior: Ícono, Nombre completo de la cancha y Estado -->
              <div class="flex items-center justify-between gap-3">
                <div class="flex items-center gap-2.5 min-w-0 flex-1">
                  <div class="w-8 h-8 rounded-lg bg-blue-50 text-[#2D9CDB] flex items-center justify-center shrink-0 font-bold text-xs">
                    <i class="fas fa-futbol"></i>
                  </div>
                  <div class="min-w-0 flex-1">
                    <h4 class="font-bold text-slate-800 text-sm leading-tight truncate">{{ cancha.nombre }}</h4>
                    <p class="text-[11px] text-slate-400 mt-0.5">{{ cancha.tipoCancha || 'Sin tipo' }}</p>
                  </div>
                </div>
                <span class="text-[9px] font-black uppercase tracking-wider px-2 py-0.5 rounded-full shrink-0" :class="cancha.estado ? 'bg-emerald-50 text-emerald-600 border border-emerald-100' : 'bg-rose-50 text-rose-500 border border-rose-100'">
                  {{ cancha.estado ? 'Activa' : 'Inactiva' }}
                </span>
              </div>

              <!-- Fila inferior: Precio y Botones de acción -->
              <div class="flex items-center justify-between pt-2.5 border-t border-slate-100/80">
                <div class="flex items-center gap-1.5">
                  <div>
                    <span class="text-[10px] text-slate-400 block leading-none">Precio</span>
                    <span class="font-bold text-slate-800 text-sm">${{ cancha.precioHora || 0 }} <span class="text-[10px] font-normal text-slate-400">/h</span></span>
                  </div>
                  <button @click="openPrecioModal(cancha)" class="w-6 h-6 flex items-center justify-center rounded-lg bg-slate-50 text-slate-400 hover:bg-slate-100 hover:text-[#2D9CDB] border border-slate-200 transition-colors ml-1" title="Editar precio">
                    <i class="fas fa-pencil-alt text-[9px]"></i>
                  </button>
                </div>

                <div class="flex items-center gap-2">
                  <button @click="openHorariosModal(cancha)" class="text-xs font-bold text-slate-600 bg-slate-100 hover:bg-slate-200 px-3 py-1.5 rounded-xl transition-colors flex items-center gap-1.5">
                    <i class="far fa-clock text-slate-400"></i>
                    <span>Horarios</span>
                  </button>
                  <button @click="toggleCanchaEstado(cancha)" class="text-xs font-bold transition-colors border border-slate-200 rounded-xl px-3 py-1.5" :class="cancha.estado ? 'text-slate-400 hover:text-rose-500 hover:border-rose-200 bg-white' : 'text-emerald-600 bg-emerald-50 hover:bg-emerald-100 border-emerald-200'">
                    {{ cancha.estado ? 'Desactivar' : 'Activar' }}
                  </button>
                </div>
              </div>
            </div>
          </div>
        </section>
      </div>

      <!-- EDIT IMAGEN MODAL -->
      <div v-if="showImagenModal" class="fixed inset-0 z-[60] flex items-end sm:items-center justify-center">
        <div class="absolute inset-0 bg-slate-900/40 backdrop-blur-sm" @click="closeImagenModal"></div>
        <div class="relative bg-white w-full sm:w-96 rounded-t-3xl sm:rounded-3xl p-6 animate-slide-up-fast">
          <div class="flex justify-between items-center mb-5">
            <div>
              <h3 class="font-bold text-slate-800 text-lg">Editar Imagen del Complejo</h3>
              <p class="text-xs font-bold text-slate-400 mt-1 uppercase tracking-wider">{{ complejo?.nombre }}</p>
            </div>
            <button @click="closeImagenModal" class="w-8 h-8 flex items-center justify-center rounded-full bg-slate-100 text-slate-500 hover:bg-slate-200">
              <i class="fas fa-times"></i>
            </button>
          </div>

          <form @submit.prevent="submitImagen" class="space-y-4">
            <div>
              <label class="text-[11px] font-bold text-slate-500 uppercase tracking-wider">URL de la Imagen</label>
              <input v-model="imagenForm.url" type="url" required @input="imagenValidationError = null; hasPreviewError = false" class="mt-1 w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB]" placeholder="https://ejemplo.com/imagen.png">
              <p class="text-[10px] text-slate-400 mt-1">La URL debe ser pública y terminar en .png, .jpg o .jpeg</p>
            </div>

            <!-- Previsualización -->
            <div v-if="imagenForm.url && !imagenValidationError" class="mt-2">
              <label class="text-[11px] font-bold text-slate-500 uppercase tracking-wider">Previsualización</label>
              <div class="mt-1 w-full h-32 rounded-xl overflow-hidden bg-slate-50 border border-slate-200 flex items-center justify-center">
                <img :src="imagenForm.url" class="w-full h-full object-cover" @error="onPreviewError" />
              </div>
              <p v-if="hasPreviewError" class="text-xs text-rose-500 mt-1"><i class="fas fa-exclamation-circle mr-1"></i> No se pudo cargar la imagen desde la URL provista.</p>
            </div>

            <button type="submit" :disabled="isSubmittingImagen" class="w-full py-4 bg-[#2D9CDB] text-white rounded-xl font-bold text-sm hover:bg-[#2088c2] transition-colors mt-2 disabled:opacity-50">
              {{ isSubmittingImagen ? 'Actualizando...' : 'Actualizar Imagen' }}
            </button>
          </form>
        </div>
      </div>

      <!-- EDIT DIRECCION MODAL -->
      <div v-if="showDireccionModal" class="fixed inset-0 z-[60] flex items-end sm:items-center justify-center">
        <div class="absolute inset-0 bg-slate-900/40 backdrop-blur-sm" @click="closeDireccionModal"></div>
        <div class="relative bg-white w-full sm:w-96 rounded-t-3xl sm:rounded-3xl p-6 animate-slide-up-fast">
          <div class="flex justify-between items-center mb-5">
            <div>
              <h3 class="font-bold text-slate-800 text-lg">Editar Ubicación</h3>
              <p class="text-xs font-bold text-slate-400 mt-1 uppercase tracking-wider">{{ complejo?.nombre }}</p>
            </div>
            <button @click="closeDireccionModal" class="w-8 h-8 flex items-center justify-center rounded-full bg-slate-100 text-slate-500 hover:bg-slate-200">
              <i class="fas fa-times"></i>
            </button>
          </div>

          <form @submit.prevent="submitDireccion" class="space-y-4">
            <div>
              <label class="text-[11px] font-bold text-slate-500 uppercase tracking-wider">Ciudad</label>
              <select v-model="direccionForm.ciudadId" required class="mt-1 w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB]">
                <option value="" disabled>Seleccioná una ciudad</option>
                <option v-for="c in ciudades" :key="c.id" :value="c.id">{{ c.nombre }}</option>
              </select>
            </div>

            <div>
              <label class="text-[11px] font-bold text-slate-500 uppercase tracking-wider">Dirección</label>
              <input v-model="direccionForm.direccion" type="text" max="200" class="mt-1 w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB]" placeholder="Ej. Av. de los Deportes 1234">
              <p class="text-[10px] text-slate-400 mt-1">Opcional. Máximo 200 caracteres.</p>
            </div>

            <button type="submit" :disabled="isSubmittingDireccion" class="w-full py-4 bg-[#2D9CDB] text-white rounded-xl font-bold text-sm hover:bg-[#2088c2] transition-colors mt-2 disabled:opacity-50">
              {{ isSubmittingDireccion ? 'Actualizando...' : 'Actualizar Ubicación' }}
            </button>
          </form>
        </div>
      </div>

      <!-- ADD CANCHA MODAL -->
      <div v-if="showModal" class="fixed inset-0 z-[60] flex items-end sm:items-center justify-center">
        <div class="absolute inset-0 bg-slate-900/40 backdrop-blur-sm" @click="closeModal"></div>
        <div class="relative bg-white w-full sm:w-96 rounded-t-3xl sm:rounded-3xl p-6 animate-slide-up-fast">
          <div class="flex justify-between items-center mb-5">
            <h3 class="font-bold text-slate-800 text-lg">Nueva Cancha</h3>
            <button @click="closeModal" class="w-8 h-8 flex items-center justify-center rounded-full bg-slate-100 text-slate-500 hover:bg-slate-200">
              <i class="fas fa-times"></i>
            </button>
          </div>

          <form @submit.prevent="submitCancha" class="space-y-4">
            <div>
              <label class="text-[11px] font-bold text-slate-500 uppercase tracking-wider">Nombre / Nro</label>
              <input v-model="canchaForm.nombre" type="text" required class="mt-1 w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB]" placeholder="Ej. Cancha 1">
            </div>

            <div>
              <label class="text-[11px] font-bold text-slate-500 uppercase tracking-wider">Tipo de Cancha</label>
              <select v-model="canchaForm.tipoCanchaId" required class="mt-1 w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB]">
                <option value="" disabled>Seleccioná un tipo</option>
                <option v-for="t in tiposCancha" :key="t.id" :value="t.id">{{ t.nombre }}</option>
              </select>
            </div>

            <div>
              <label class="text-[11px] font-bold text-slate-500 uppercase tracking-wider">Precio por Hora (ARS)</label>
              <input v-model="canchaForm.precioHora" type="number" required class="mt-1 w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB]" placeholder="Ej. 10000">
            </div>

            <button type="submit" :disabled="isSubmitting" class="w-full py-4 bg-[#2D9CDB] text-white rounded-xl font-bold text-sm hover:bg-[#2088c2] transition-colors mt-2 disabled:opacity-50">
              {{ isSubmitting ? 'Guardando...' : 'Guardar Cancha' }}
            </button>
          </form>
        </div>
      </div>

      <!-- EDIT PRECIO MODAL -->
      <div v-if="showPrecioModal" class="fixed inset-0 z-[60] flex items-end sm:items-center justify-center">
        <div class="absolute inset-0 bg-slate-900/40 backdrop-blur-sm" @click="closePrecioModal"></div>
        <div class="relative bg-white w-full sm:w-96 rounded-t-3xl sm:rounded-3xl p-6 animate-slide-up-fast">
          <div class="flex justify-between items-center mb-5">
            <div>
              <h3 class="font-bold text-slate-800 text-lg">Editar Precio</h3>
              <p class="text-xs font-bold text-slate-400 mt-1 uppercase tracking-wider">{{ activeCancha?.nombre }}</p>
            </div>
            <button @click="closePrecioModal" class="w-8 h-8 flex items-center justify-center rounded-full bg-slate-100 text-slate-500 hover:bg-slate-200">
              <i class="fas fa-times"></i>
            </button>
          </div>

          <form @submit.prevent="submitPrecio" class="space-y-4">
            <div>
              <label class="text-[11px] font-bold text-slate-500 uppercase tracking-wider">Precio por Hora (ARS)</label>
              <input v-model="precioForm.precioHora" type="number" required min="0" step="0.01" class="mt-1 w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:outline-none focus:border-[#2D9CDB]" placeholder="Ej. 10000">
            </div>

            <button type="submit" :disabled="isSubmittingPrecio" class="w-full py-4 bg-[#2D9CDB] text-white rounded-xl font-bold text-sm hover:bg-[#2088c2] transition-colors mt-2 disabled:opacity-50">
              {{ isSubmittingPrecio ? 'Actualizando...' : 'Actualizar Precio' }}
            </button>
          </form>
        </div>
      </div>

      <!-- GESTION DE HORARIOS MODAL -->
      <div v-if="showHorariosModal" class="fixed inset-0 z-[70] flex items-end sm:items-center justify-center">
        <div class="absolute inset-0 bg-slate-900/60 backdrop-blur-sm" @click="closeHorariosModal"></div>
        <div class="relative bg-white w-full max-w-lg sm:rounded-3xl rounded-t-3xl h-[85vh] sm:h-auto max-h-[90vh] flex flex-col animate-slide-up-fast shadow-2xl">
          
          <div class="px-6 py-5 border-b border-slate-100 flex justify-between items-center bg-white rounded-t-3xl shrink-0">
            <div>
              <h3 class="font-black text-slate-800 text-lg leading-none">Horarios Disponibles</h3>
              <p class="text-xs font-bold text-slate-400 mt-1 uppercase tracking-wider">{{ activeCancha?.nombre }}</p>
            </div>
            <button @click="closeHorariosModal" class="w-8 h-8 flex items-center justify-center rounded-full bg-slate-50 text-slate-500 hover:bg-slate-100 border border-slate-200 transition-colors">
              <i class="fas fa-times"></i>
            </button>
          </div>

          <div class="flex-1 overflow-y-auto p-6 bg-slate-50">
            <form @submit.prevent="submitHorario" class="bg-white p-4 rounded-2xl border border-slate-200 shadow-sm mb-6 space-y-4">
              <h4 class="text-[11px] font-black text-slate-400 uppercase tracking-widest mb-2 border-b border-slate-100 pb-2">Agregar Franja</h4>
              <div class="grid grid-cols-3 gap-3">
                <div class="col-span-3">
                  <label class="text-[10px] font-bold text-slate-500 uppercase tracking-wider block mb-1">Día</label>
                  <select v-model="horarioForm.diaSemana" required class="w-full bg-slate-50 border border-slate-200 rounded-xl px-3 py-2 text-sm focus:outline-none focus:border-[#2D9CDB] focus:ring-1 focus:ring-[#2D9CDB]">
                    <option value="" disabled>Seleccionar</option>
                    <option v-for="d in diasSemana" :key="d.id" :value="d.id">{{ d.nombre }}</option>
                  </select>
                </div>
                <div class="col-span-1">
                  <label class="text-[10px] font-bold text-slate-500 uppercase tracking-wider block mb-1">Inicio</label>
                  <input v-model="horarioForm.horaInicio" type="time" required class="w-full bg-slate-50 border border-slate-200 rounded-xl px-2 py-2 text-sm focus:outline-none focus:border-[#2D9CDB]">
                </div>
                <div class="col-span-1">
                  <label class="text-[10px] font-bold text-slate-500 uppercase tracking-wider block mb-1">Fin</label>
                  <input v-model="horarioForm.horaFin" type="time" required class="w-full bg-slate-50 border border-slate-200 rounded-xl px-2 py-2 text-sm focus:outline-none focus:border-[#2D9CDB]">
                </div>
                <div class="col-span-1 flex items-end">
                  <button type="submit" :disabled="isSubmittingHorario" class="w-full py-2 bg-[#2D9CDB] text-white rounded-xl font-bold text-sm shadow-md shadow-[#2D9CDB]/20 hover:bg-blue-600 transition-colors disabled:opacity-50">
                    <i class="fas fa-plus"></i>
                  </button>
                </div>
              </div>
            </form>

            <div v-if="isHorariosLoading" class="flex justify-center py-10">
              <i class="fas fa-circle-notch animate-spin text-2xl text-[#2D9CDB]"></i>
            </div>
            
            <div v-else-if="horariosCancha.length === 0" class="text-center py-10">
              <div class="w-12 h-12 bg-white border border-slate-200 rounded-full flex items-center justify-center mx-auto mb-3 shadow-sm text-slate-300">
                <i class="fas fa-calendar-times"></i>
              </div>
              <p class="text-sm font-bold text-slate-500">No hay horarios cargados</p>
            </div>

            <div v-else class="space-y-3">
              <h4 class="text-[11px] font-black text-slate-400 uppercase tracking-widest mb-3 px-1">Franjas Registradas</h4>
              <div v-for="h in horariosCancha" :key="h.id" class="bg-white p-4 rounded-xl border border-slate-200 shadow-sm flex items-center justify-between group">
                <div class="flex items-center gap-3">
                  <div class="w-10 h-10 bg-blue-50 text-[#2D9CDB] rounded-xl flex items-center justify-center font-bold text-xs">
                    {{ getDiaNombre(h.diaSemana).substring(0,3) }}
                  </div>
                  <div>
                    <p class="font-bold text-slate-800 text-sm leading-none">{{ getDiaNombre(h.diaSemana) }}</p>
                    <p class="text-[11px] font-bold text-slate-400 mt-1 uppercase tracking-widest">{{ formatTime(h.horaInicio) }} - {{ formatTime(h.horaFin) }} hs</p>
                  </div>
                </div>
                <button @click="deleteHorario(h.id)" class="w-8 h-8 rounded-full bg-slate-50 text-slate-400 hover:bg-red-50 hover:text-red-500 transition-colors border border-slate-100 flex items-center justify-center">
                  <i class="fas fa-trash-alt text-xs"></i>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

    </div>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthUser } from '../../composables/useAuthUser'
import { API_ENDPOINTS } from '../../config/apiConfig'
import { useToast } from '../../composables/useToast'
import { useConfirm } from '../../composables/useConfirm'

export default {
  name: 'OwnerComplejoConfig',
  setup() {
    const router = useRouter()
    const route = useRoute()
    const { user } = useAuthUser()
    const { showToast } = useToast()
    const { requireConfirm } = useConfirm()
    const complejoId = route.params.id

    const isLoading = ref(true)
    const complejo = ref(null)
    const ciudades = ref([])
    
    // Cancha Modal state
    const showModal = ref(false)
    const isSubmitting = ref(false)
    const tiposCancha = ref([])
    const canchaForm = ref({
      nombre: '',
      tipoCanchaId: '',
      precioHora: null
    })
    // Precio Modal state
    const showPrecioModal = ref(false)
    const isSubmittingPrecio = ref(false)
    const precioForm = ref({
      precioHora: null
    })

    // Horarios Modal state
    const showHorariosModal = ref(false)
    const activeCancha = ref(null)
    const horariosCancha = ref([])
    const isHorariosLoading = ref(false)
    const isSubmittingHorario = ref(false)
    const horarioForm = ref({
      diaSemana: '',
      horaInicio: '',
      horaFin: ''
    })

    const diasSemana = [
      { id: 1, nombre: 'Lunes' },
      { id: 2, nombre: 'Martes' },
      { id: 3, nombre: 'Miércoles' },
      { id: 4, nombre: 'Jueves' },
      { id: 5, nombre: 'Viernes' },
      { id: 6, nombre: 'Sábado' },
      { id: 0, nombre: 'Domingo' }
    ]

    const getDiaNombre = (id) => diasSemana.find(d => d.id === id)?.nombre || ''
    const formatTime = (time) => time ? time.substring(0,5) : ''

    const fetchConfig = async () => {
      try {
        const ownerId = user.value?.sub || 1
        const response = await fetch(API_ENDPOINTS.complejos.getConfig(complejoId, ownerId))
        if (response.ok) {
          complejo.value = await response.json()
        }
      } catch (error) {
        console.error('Error fetching config:', error)
      } finally {
        isLoading.value = false
      }
    }

    const fetchRefData = async () => {
      try {
        const response = await fetch(API_ENDPOINTS.referenceData.getAll())
        if (response.ok) {
          const data = await response.json()
          tiposCancha.value = data.tiposCancha || []
          ciudades.value = data.ciudades || []
        }
      } catch (error) {
        console.error('Error fetching ref data:', error)
      }
    }

    const openModal = () => { showModal.value = true }
    const closeModal = () => {
      showModal.value = false
      canchaForm.value = { nombre: '', tipoCanchaId: '', precioHora: null }
    }

    const submitCancha = async () => {
      isSubmitting.value = true
      try {
        const payload = {
          tipoCanchaId: parseInt(canchaForm.value.tipoCanchaId),
          nombre: canchaForm.value.nombre,
          precioHora: parseFloat(canchaForm.value.precioHora)
        }

        const response = await fetch(API_ENDPOINTS.complejos.createCancha(complejoId), {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload)
        })

        if (response.ok) {
          closeModal()
          await fetchConfig() 
        } else {
          showToast('Error al crear la cancha', 'error')
        }
      } catch (error) {
        console.error(error)
        showToast('Ocurrió un error', 'error')
      } finally {
        isSubmitting.value = false
      }
    }

    const openPrecioModal = (cancha) => {
      activeCancha.value = cancha
      precioForm.value.precioHora = cancha.precioHora
      showPrecioModal.value = true
    }

    const closePrecioModal = () => {
      showPrecioModal.value = false
      activeCancha.value = null
      precioForm.value.precioHora = null
    }

    const submitPrecio = async () => {
      if (!activeCancha.value) return
      isSubmittingPrecio.value = true
      try {
        const currentUserId = user.value?.sub || 1
        const response = await fetch(API_ENDPOINTS.canchas.updatePrecio(activeCancha.value.id), {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            canchaId: activeCancha.value.id,
            usuarioId: currentUserId,
            precioHora: parseFloat(precioForm.value.precioHora)
          })
        })

        if (response.ok) {
          showToast('Precio actualizado correctamente', 'success')
          closePrecioModal()
          await fetchConfig()
        } else {
          showToast('Error al actualizar el precio', 'error')
        }
      } catch (error) {
        console.error(error)
        showToast('Ocurrió un error', 'error')
      } finally {
        isSubmittingPrecio.value = false
      }
    }

    // --- HORARIOS LOGIC ---
    const fetchHorariosCancha = async (canchaId) => {
      isHorariosLoading.value = true
      try {
        const response = await fetch(API_ENDPOINTS.canchas.getHorarios(canchaId))
        if (response.ok) {
          horariosCancha.value = await response.json()
        } else {
          horariosCancha.value = []
        }
      } catch (e) {
        console.error(e)
        horariosCancha.value = []
      } finally {
        isHorariosLoading.value = false
      }
    }

    const openHorariosModal = async (cancha) => {
      activeCancha.value = cancha
      showHorariosModal.value = true
      await fetchHorariosCancha(cancha.id)
    }

    const closeHorariosModal = () => {
      showHorariosModal.value = false
      activeCancha.value = null
      horariosCancha.value = []
      horarioForm.value = { diaSemana: '', horaInicio: '', horaFin: '' }
    }

    const submitHorario = async () => {
      isSubmittingHorario.value = true
      try {
        const inicio = horarioForm.value.horaInicio.length === 5 ? `${horarioForm.value.horaInicio}:00` : horarioForm.value.horaInicio;
        const fin = horarioForm.value.horaFin.length === 5 ? `${horarioForm.value.horaFin}:00` : horarioForm.value.horaFin;

        const payload = {
          diaSemana: parseInt(horarioForm.value.diaSemana),
          horaInicio: inicio,
          horaFin: fin
        }
        
        const response = await fetch(API_ENDPOINTS.canchas.createHorario(activeCancha.value.id), {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(payload)
        })

        if (response.ok) {
          horarioForm.value = { diaSemana: '', horaInicio: '', horaFin: '' }
          await fetchHorariosCancha(activeCancha.value.id)
        } else {
          showToast('Error al agregar horario', 'error')
        }
      } catch (error) {
        console.error(error)
        showToast('Ocurrió un error', 'error')
      } finally {
        isSubmittingHorario.value = false
      }
    }

    const deleteHorario = async (horarioId) => {
      const confirmed = await requireConfirm({
        title: 'Eliminar Horario',
        message: '¿Estás seguro de que deseas eliminar este horario?',
        confirmText: 'Eliminar',
        cancelText: 'Cancelar',
        type: 'danger'
      })
      if (!confirmed) return;
      try {
        const response = await fetch(API_ENDPOINTS.canchas.deleteHorario(activeCancha.value.id, horarioId), {
          method: 'DELETE'
        })
        if (response.ok) {
          await fetchHorariosCancha(activeCancha.value.id)
        } else {
          showToast('Error al eliminar horario', 'error')
        }
      } catch (error) {
        console.error(error)
      }
    }

    const toggleComplejoEstado = async () => {
      if (!complejo.value) return
      const nuevoEstado = !complejo.value.estado
      const title = nuevoEstado ? 'Mostrar Complejo' : 'Ocultar Complejo'
      const message = nuevoEstado 
        ? '¿Habilitar la visibilidad de este complejo en el modo jugador?' 
        : '¿Ocultar este complejo? Los jugadores no podrán verlo ni reservar.'
      
      const confirmed = await requireConfirm({
        title,
        message,
        confirmText: nuevoEstado ? 'Mostrar' : 'Ocultar',
        cancelText: 'Cancelar',
        type: nuevoEstado ? 'info' : 'warning'
      })
      if (!confirmed) return

      try {
        const currentUserId = user.value?.sub || 1
        const response = await fetch(API_ENDPOINTS.complejos.updateEstado(complejoId), {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            complejoId: parseInt(complejoId),
            usuarioId: currentUserId,
            estado: nuevoEstado
          })
        })

        if (response.ok) {
          complejo.value.estado = nuevoEstado
        } else {
          showToast('Error al cambiar el estado del complejo', 'error')
        }
      } catch (error) {
        console.error('Error toggling complejo estado:', error)
        showToast('Ocurrió un error', 'error')
      }
    }

    const toggleCanchaEstado = async (cancha) => {
      const nuevoEstado = !cancha.estado
      const title = nuevoEstado ? 'Habilitar Cancha' : 'Desactivar Cancha'
      const message = nuevoEstado 
        ? '¿Habilitar esta cancha?' 
        : '¿Desactivar esta cancha? No se mostrará a los jugadores.'
      
      const confirmed = await requireConfirm({
        title,
        message,
        confirmText: nuevoEstado ? 'Habilitar' : 'Desactivar',
        cancelText: 'Cancelar',
        type: nuevoEstado ? 'info' : 'warning'
      })
      if (!confirmed) return

      try {
        const currentUserId = user.value?.sub || 1
        const response = await fetch(API_ENDPOINTS.canchas.updateEstado(cancha.id), {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            canchaId: cancha.id,
            usuarioId: currentUserId,
            estado: nuevoEstado
          })
        })

        if (response.ok) {
          cancha.estado = nuevoEstado
        } else {
          showToast('Error al cambiar el estado de la cancha', 'error')
        }
      } catch (error) {
        console.error('Error toggling cancha estado:', error)
        showToast('Ocurrió un error', 'error')
      }
    }

    // --- EDIT IMAGEN LOGIC ---
    const showImagenModal = ref(false)
    const isSubmittingImagen = ref(false)
    const imagenValidationError = ref(null)
    const hasPreviewError = ref(false)
    const imagenForm = ref({
      url: ''
    })

    const openImagenModal = () => {
      imagenForm.value.url = complejo.value?.imagen || ''
      imagenValidationError.value = null
      hasPreviewError.value = false
      showImagenModal.value = true
    }

    const closeImagenModal = () => {
      showImagenModal.value = false
      imagenForm.value.url = ''
      imagenValidationError.value = null
      hasPreviewError.value = false
    }

    const onPreviewError = () => {
      hasPreviewError.value = true
    }

    const validateImagenUrl = (url) => {
      if (!url) return 'La URL es requerida'
      try {
        const parsedUrl = new URL(url)
        const pathname = parsedUrl.pathname.toLowerCase()
        if (!pathname.endsWith('.png') && !pathname.endsWith('.jpg') && !pathname.endsWith('.jpeg')) {
          return 'La URL debe terminar en .png, .jpg o .jpeg (puede incluir parámetros de consulta)'
        }
        return null
      } catch (e) {
        return 'Por favor ingresá una URL válida que empiece con http:// o https://'
      }
    }

    const submitImagen = async () => {
      const error = validateImagenUrl(imagenForm.value.url)
      if (error) {
        imagenValidationError.value = error
        showToast(error, 'error')
        return
      }

      isSubmittingImagen.value = true
      try {
        const currentUserId = user.value?.sub || 1
        const response = await fetch(API_ENDPOINTS.complejos.updateImagen(complejoId), {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            complejoId: parseInt(complejoId),
            usuarioId: currentUserId,
            imagenUrl: imagenForm.value.url
          })
        })

        if (response.ok) {
          showToast('Imagen del complejo actualizada correctamente', 'success')
          closeImagenModal()
          await fetchConfig()
        } else {
          const errData = await response.json().catch(() => ({}))
          showToast(errData.message || 'Error al actualizar la imagen del complejo', 'error')
        }
      } catch (error) {
        console.error(error)
        showToast('Ocurrió un error al actualizar la imagen', 'error')
      } finally {
        isSubmittingImagen.value = false
      }
    }

    // --- EDIT DIRECCION LOGIC ---
    const showDireccionModal = ref(false)
    const isSubmittingDireccion = ref(false)
    const direccionForm = ref({
      ciudadId: '',
      direccion: ''
    })

    const openDireccionModal = () => {
      direccionForm.value.ciudadId = complejo.value?.ciudadId || ''
      direccionForm.value.direccion = complejo.value?.direccion || ''
      showDireccionModal.value = true
    }

    const closeDireccionModal = () => {
      showDireccionModal.value = false
      direccionForm.value.ciudadId = ''
      direccionForm.value.direccion = ''
    }

    const submitDireccion = async () => {
      if (!direccionForm.value.ciudadId) {
        showToast('La ciudad es requerida.', 'error')
        return
      }

      if (direccionForm.value.direccion && direccionForm.value.direccion.length > 200) {
        showToast('La dirección no puede exceder los 200 caracteres.', 'error')
        return
      }

      isSubmittingDireccion.value = true
      try {
        const currentUserId = user.value?.sub || 1
        const response = await fetch(API_ENDPOINTS.complejos.updateDireccion(complejoId), {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            complejoId: parseInt(complejoId),
            usuarioId: currentUserId,
            ciudadId: parseInt(direccionForm.value.ciudadId),
            direccion: direccionForm.value.direccion
          })
        })

        if (response.ok) {
          showToast('Ubicación del complejo actualizada correctamente', 'success')
          closeDireccionModal()
          await fetchConfig()
        } else {
          const errData = await response.json().catch(() => ({}))
          showToast(errData.message || 'Error al actualizar la ubicación del complejo', 'error')
        }
      } catch (error) {
        console.error(error)
        showToast('Ocurrió un error al actualizar la ubicación', 'error')
      } finally {
        isSubmittingDireccion.value = false
      }
    }

    const goBack = () => {
      router.push({ name: 'Home' })
    }

    onMounted(() => {
      fetchConfig()
      fetchRefData()
    })

    return {
      isLoading,
      complejo,
      // Canchas
      showModal,
      isSubmitting,
      tiposCancha,
      canchaForm,
      openModal,
      closeModal,
      submitCancha,
      toggleCanchaEstado,
      // Edición precio
      showPrecioModal,
      isSubmittingPrecio,
      precioForm,
      openPrecioModal,
      closePrecioModal,
      submitPrecio,
      // Horarios
      showHorariosModal,
      activeCancha,
      horariosCancha,
      isHorariosLoading,
      isSubmittingHorario,
      horarioForm,
      diasSemana,
      openHorariosModal,
      closeHorariosModal,
      submitHorario,
      deleteHorario,
      getDiaNombre,
      formatTime,
      // Edición imagen
      showImagenModal,
      isSubmittingImagen,
      imagenValidationError,
      hasPreviewError,
      imagenForm,
      openImagenModal,
      closeImagenModal,
      onPreviewError,
      submitImagen,
      // Edición dirección
      ciudades,
      showDireccionModal,
      isSubmittingDireccion,
      direccionForm,
      openDireccionModal,
      closeDireccionModal,
      submitDireccion,
      // Common
      toggleComplejoEstado,
      goBack
    }
  }
}
</script>

<style scoped>
.animate-fade-in { animation: fadeIn 0.4s ease-out; }
.animate-slide-up-fast { animation: slideUp 0.3s cubic-bezier(0.16, 1, 0.3, 1); }

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}
@keyframes slideUp {
  from { transform: translateY(100%); }
  to { transform: translateY(0); }
}
</style>
