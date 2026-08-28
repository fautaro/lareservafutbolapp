<template>
  <div class="min-h-screen bg-slate-50 pb-28 text-slate-800" style="font-family: -apple-system, BlinkMacSystemFont, 'SF Pro Text', Inter, sans-serif;">
    <div class="px-3 pt-3 max-w-2xl mx-auto animate-slide-up">

      <!-- ══ HEADER PRINCIPAL ESTILO IOS ══ -->
      <header class="sticky top-2 z-40 bg-white/95 backdrop-blur-md rounded-2xl shadow-sm border border-slate-100 p-4 mb-4">
        <!-- Barra superior: Botón Volver + Título + Badge de Modo -->
        <div class="flex items-center justify-between gap-2 mb-3">
          <div class="flex items-center gap-2.5">
            <button 
              type="button"
              @click="goBack" 
              class="w-9 h-9 rounded-xl bg-slate-100/90 text-slate-600 flex items-center justify-center active:scale-95 transition-transform"
              title="Volver"
            >
              <i class="fas fa-chevron-left text-sm"></i>
            </button>
            <div>
              <h1 class="text-xl sm:text-2xl font-black text-slate-900 leading-tight tracking-tight">Centro de Ayuda</h1>
              <p class="text-[11px] text-slate-400 font-medium">Guías y preguntas frecuentes</p>
            </div>
          </div>

          <!-- Badge con el rol activo (definido en /menu) -->
          <div 
            class="flex items-center gap-1.5 px-2.5 py-1 rounded-full text-[11px] font-extrabold uppercase tracking-wide border shadow-2xs shrink-0"
            :class="isOwnerRole ? 'bg-indigo-50 text-indigo-700 border-indigo-100' : 'bg-sky-50 text-[#2D9CDB] border-sky-100'"
          >
            <i :class="isOwnerRole ? 'fas fa-building text-[10px]' : 'fas fa-futbol text-[10px]'"></i>
            <span>{{ isOwnerRole ? 'Dueño' : 'Jugador' }}</span>
          </div>
        </div>

        <!-- ══ BARRA DE BÚSQUEDA TIPO IOS ══ -->
        <div class="relative mt-2">
          <i class="fas fa-search absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400 text-xs pointer-events-none"></i>
          <input 
            v-model="searchQuery" 
            type="text" 
            placeholder="Buscar en la ayuda (ej. 'cancelar', 'precio', 'agenda')..." 
            class="w-full bg-slate-100/90 border border-transparent focus:border-[#2D9CDB]/40 focus:bg-white rounded-xl pl-9 pr-9 py-2.5 text-base sm:text-sm text-slate-800 placeholder:text-slate-400 focus:outline-none transition-all"
          />
          <button 
            v-if="searchQuery" 
            @click="searchQuery = ''" 
            class="absolute right-2.5 top-1/2 -translate-y-1/2 w-6 h-6 rounded-full bg-slate-200 text-slate-500 hover:text-slate-700 flex items-center justify-center text-[10px]"
            title="Borrar búsqueda"
          >
            <i class="fas fa-times"></i>
          </button>
        </div>

        <!-- ══ FILTRO DE CATEGORÍAS (Scroll horizontal con inercia iOS) ══ -->
        <div class="flex gap-2 overflow-x-auto no-scrollbar pt-3 -mx-1 px-1 scroll-smooth">
          <button 
            type="button"
            @click="selectedCategory = 'all'"
            class="px-3.5 py-2 rounded-xl text-xs font-bold whitespace-nowrap transition-all active:scale-95 shrink-0 flex items-center gap-1.5"
            :class="selectedCategory === 'all' ? 'bg-slate-900 text-white shadow-sm' : 'bg-slate-100 text-slate-600 hover:bg-slate-200/70'"
          >
            <span>Todas</span>
            <span class="text-[10px] opacity-70">({{ totalItemsCount }})</span>
          </button>

          <button 
            type="button"
            v-for="cat in currentCategories" 
            :key="cat.id"
            @click="selectedCategory = cat.id"
            class="px-3 py-2 rounded-xl text-xs font-bold whitespace-nowrap transition-all active:scale-95 shrink-0 flex items-center gap-1.5"
            :class="selectedCategory === cat.id ? 'bg-[#2D9CDB] text-white shadow-sm shadow-blue-300/40' : 'bg-slate-100 text-slate-600 hover:bg-slate-200/70'"
          >
            <i :class="cat.icon" class="text-[11px]"></i>
            <span>{{ cat.name }}</span>
          </button>
        </div>
      </header>

      <!-- ══ BANNER DE CONTEXTO RÁPIDO ══ -->
      <div class="mb-4">
        <div class="bg-white rounded-2xl p-3.5 border border-slate-100 shadow-2xs flex items-center justify-between gap-3">
          <div class="flex items-center gap-3">
            <div 
              class="w-10 h-10 rounded-xl flex items-center justify-center shrink-0"
              :class="isOwnerRole ? 'bg-indigo-50 text-indigo-600' : 'bg-sky-50 text-[#2D9CDB]'"
            >
              <i :class="isOwnerRole ? 'fas fa-chart-pie text-sm' : 'fas fa-futbol text-sm'"></i>
            </div>
            <div>
              <p class="text-xs font-extrabold text-slate-800">
                {{ isOwnerRole ? 'Guías para Administración de Complejos' : 'Guías para Jugadores y Reservas' }}
              </p>
              <p class="text-[11px] text-slate-500 leading-tight">
                {{ isOwnerRole ? 'Aprende a gestionar canchas, precios, agendas y métricas.' : 'Descubre cómo explorar, agendar partidos y gestionar tus turnos.' }}
              </p>
            </div>
          </div>
          <button 
            type="button"
            @click="toggleExpandAll" 
            class="text-[11px] font-bold text-[#2D9CDB] bg-sky-50/80 active:bg-sky-100 border border-sky-100 px-3 py-1.5 rounded-xl transition-colors shrink-0"
          >
            {{ areAllExpanded ? 'Colapsar' : 'Expandir' }}
          </button>
        </div>
      </div>

      <!-- ══ LISTADO DE PREGUNTAS Y GUÍAS (ACORDEONES IOS GROUPED) ══ -->
      <div class="space-y-4">

        <!-- Estado de Sin Resultados -->
        <div v-if="filteredSections.length === 0" class="bg-white rounded-2xl border border-slate-100 p-8 text-center shadow-2xs">
          <div class="w-14 h-14 bg-slate-100 rounded-full flex items-center justify-center mx-auto mb-3 text-slate-400 text-xl">
            <i class="fas fa-search"></i>
          </div>
          <h3 class="font-bold text-slate-800 text-sm">No encontramos resultados para "{{ searchQuery }}"</h3>
          <p class="text-xs text-slate-400 mt-1 max-w-xs mx-auto">Probá con otros términos como 'reserva', 'horario', 'pago', 'cancha' o 'agenda'.</p>
          <button 
            type="button"
            @click="searchQuery = ''; selectedCategory = 'all'" 
            class="mt-4 text-xs font-bold text-[#2D9CDB] bg-sky-50 px-4 py-2 rounded-xl hover:bg-sky-100 transition-colors active:scale-95"
          >
            Restablecer búsqueda
          </button>
        </div>

        <!-- Secciones agrupadas con estilo iOS Grouped Table -->
        <div v-for="section in filteredSections" :key="section.id" class="space-y-1.5">
          
          <!-- Encabezado de Sección -->
          <div class="flex items-center gap-2 px-1 pt-1">
            <div class="w-1.5 h-1.5 rounded-full" :class="isOwnerRole ? 'bg-indigo-500' : 'bg-[#2D9CDB]'"></div>
            <h2 class="text-[11px] font-black uppercase tracking-wider text-slate-400">{{ section.title }}</h2>
            <span class="text-[10px] font-bold text-slate-400">({{ section.items.length }})</span>
          </div>

          <!-- Tarjeta Agrupada -->
          <div class="bg-white rounded-2xl border border-slate-100/90 shadow-2xs overflow-hidden divide-y divide-slate-100">
            <div 
              v-for="item in section.items" 
              :key="item.id" 
              class="transition-colors duration-150"
            >
              <!-- Botón de apertura / cierre (Tap Target ergonómico) -->
              <button 
                type="button"
                @click="toggleItem(item.id)" 
                class="w-full p-3.5 flex items-center gap-3 text-left hover:bg-slate-50/80 active:bg-slate-100/70 transition-colors focus:outline-none"
              >
                <!-- Ícono redondeado -->
                <div class="w-9 h-9 rounded-xl flex items-center justify-center shrink-0" :style="{ background: item.iconBg || 'rgba(45,156,219,0.08)' }">
                  <i :class="[item.icon, 'text-xs']" :style="{ color: item.iconColor || '#2D9CDB' }"></i>
                </div>

                <div class="flex-1 min-w-0 pr-1">
                  <span class="text-[13px] font-bold text-slate-800 leading-snug block">
                    {{ item.question }}
                  </span>
                  <p v-if="item.shortTip" class="text-[11px] text-slate-400 font-normal mt-0.5 line-clamp-1">
                    {{ item.shortTip }}
                  </p>
                </div>

                <!-- Chevron animado -->
                <div class="w-6 h-6 rounded-full bg-slate-50 flex items-center justify-center shrink-0">
                  <i 
                    class="fas fa-chevron-down text-[10px] text-slate-400 transition-transform duration-300 ease-out"
                    :class="{ 'rotate-180 text-[#2D9CDB]': openItems.includes(item.id) }"
                  ></i>
                </div>
              </button>

              <!-- Contenido desplegable -->
              <div 
                v-show="openItems.includes(item.id)" 
                class="px-4 pb-4 pt-1 text-xs text-slate-600 space-y-2.5 animate-fade-in"
              >
                <div class="bg-slate-50/90 rounded-xl p-3.5 border border-slate-100 leading-relaxed text-slate-700 space-y-2">
                  <p v-for="(paragraph, pIdx) in item.answer" :key="pIdx" class="leading-relaxed text-[13px]">
                    {{ paragraph }}
                  </p>

                  <!-- Lista de pasos -->
                  <ul v-if="item.steps && item.steps.length" class="space-y-1.5 mt-2 pt-2 border-t border-slate-200/60">
                    <li v-for="(step, sIdx) in item.steps" :key="sIdx" class="flex items-start gap-2 text-xs">
                      <span class="w-4 h-4 rounded-full bg-emerald-100 text-emerald-700 flex items-center justify-center font-black text-[9px] shrink-0 mt-0.5">
                        {{ sIdx + 1 }}
                      </span>
                      <span class="text-slate-700">{{ step }}</span>
                    </li>
                  </ul>

                  <!-- Tip destacado -->
                  <div v-if="item.tip" class="mt-2 pt-1 flex items-start gap-2 bg-amber-50/90 border border-amber-200/60 rounded-xl p-2.5 text-amber-900 text-xs">
                    <i class="fas fa-lightbulb text-amber-500 mt-0.5 shrink-0"></i>
                    <div>
                      <strong class="font-bold">Consejo: </strong>
                      <span>{{ item.tip }}</span>
                    </div>
                  </div>

                  <!-- Botón de acción rápida -->
                  <div v-if="item.actionRoute" class="pt-2">
                    <router-link 
                      :to="item.actionRoute" 
                      class="inline-flex items-center gap-2 text-xs font-bold text-white bg-[#2D9CDB] hover:bg-[#2088c2] px-3.5 py-2 rounded-xl transition-all shadow-xs active:scale-95"
                    >
                      <i :class="item.actionIcon || 'fas fa-arrow-right'" class="text-[10px]"></i>
                      <span>{{ item.actionLabel || 'Ir a la sección' }}</span>
                    </router-link>
                  </div>
                </div>
              </div>

            </div>
          </div>
        </div>

      </div>

      <!-- ══ ACCIONES RÁPIDAS Y SOPORTE DIRECTO ══ -->
      <div class="mt-8 space-y-3">
        <p class="text-[10px] font-black tracking-widest uppercase text-slate-400 px-1">Atajos & Contacto</p>

        <div class="grid grid-cols-1 sm:grid-cols-2 gap-2.5">
          <!-- Atajo 1: Home / Mis Complejos -->
          <router-link 
            :to="{ name: 'Home' }" 
            class="bg-white rounded-2xl border border-slate-100 p-3.5 flex items-center justify-between gap-3 shadow-2xs hover:bg-slate-50/80 active:scale-[0.99] transition-all"
          >
            <div class="flex items-center gap-3">
              <div 
                class="w-10 h-10 rounded-xl flex items-center justify-center font-bold"
                :class="isOwnerRole ? 'bg-indigo-50 text-indigo-600' : 'bg-sky-50 text-[#2D9CDB]'"
              >
                <i :class="isOwnerRole ? 'fas fa-building' : 'fas fa-futbol'"></i>
              </div>
              <div>
                <h4 class="text-xs font-bold text-slate-800">{{ isOwnerRole ? 'Mis Complejos' : 'Explorar Canchas' }}</h4>
                <p class="text-[11px] text-slate-400">{{ isOwnerRole ? 'Ver agenda y reservas' : 'Buscar turnos disponibles' }}</p>
              </div>
            </div>
            <i class="fas fa-chevron-right text-slate-300 text-xs"></i>
          </router-link>

          <!-- Atajo 2: Configuración -->
          <router-link 
            :to="{ name: 'ConfigUser' }" 
            class="bg-white rounded-2xl border border-slate-100 p-3.5 flex items-center justify-between gap-3 shadow-2xs hover:bg-slate-50/80 active:scale-[0.99] transition-all"
          >
            <div class="flex items-center gap-3">
              <div class="w-10 h-10 rounded-xl bg-purple-50 text-purple-600 flex items-center justify-center font-bold">
                <i class="fas fa-user-gear"></i>
              </div>
              <div>
                <h4 class="text-xs font-bold text-slate-800">Mi Cuenta y Perfil</h4>
                <p class="text-[11px] text-slate-400">Datos y configuración</p>
              </div>
            </div>
            <i class="fas fa-chevron-right text-slate-300 text-xs"></i>
          </router-link>
        </div>

        <!-- Tarjeta de Soporte Directo -->
        <div class="bg-gradient-to-br from-slate-900 to-slate-800 text-white rounded-2xl p-4 shadow-sm mt-3">
          <div class="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-3">
            <div class="flex items-center gap-3">
              <div class="w-10 h-10 rounded-xl bg-white/10 flex items-center justify-center text-white text-lg shrink-0">
                <i class="fas fa-headset"></i>
              </div>
              <div>
                <h3 class="text-xs font-extrabold text-white">¿Necesitás ayuda personalizada?</h3>
                <p class="text-[11px] text-slate-300">Nuestro equipo está listo para responder tus dudas.</p>
              </div>
            </div>

            <a 
              href="mailto:soporte@lareservafutbol.com?subject=Consulta%20desde%20Centro%20de%20Ayuda" 
              class="w-full sm:w-auto text-center px-4 py-2.5 bg-[#2D9CDB] active:bg-[#2088c2] text-white font-bold text-xs rounded-xl shadow-sm transition-all active:scale-95 flex items-center justify-center gap-2"
            >
              <i class="fas fa-envelope text-xs"></i>
              <span>Escribir a Soporte</span>
            </a>
          </div>
        </div>

      </div>

      <!-- Footer sutil -->
      <footer class="text-center text-[11px] text-slate-400 mt-6 mb-2">
        La Reserva Fútbol • Centro de Ayuda {{ currentYear }}
      </footer>

    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useRole } from '../../composables/useRole'

const router = useRouter()
const { currentRole } = useRole()

const isOwnerRole = computed(() => currentRole.value === 'owner')
const currentYear = new Date().getFullYear()

const goBack = () => {
  if (window.history.length > 1) {
    router.back()
  } else {
    router.push({ name: 'Menu' })
  }
}

// Búsqueda y categoría activa
const searchQuery = ref('')
const selectedCategory = ref('all')
const openItems = ref([])

const toggleItem = (id) => {
  const idx = openItems.value.indexOf(id)
  if (idx > -1) {
    openItems.value.splice(idx, 1)
  } else {
    openItems.value.push(id)
  }
}

// Categorías según el modo actual activo en la app
const currentCategories = computed(() => {
  if (isOwnerRole.value) {
    return [
      { id: 'complejos', name: 'Complejos', icon: 'fas fa-building' },
      { id: 'canchas', name: 'Canchas & Precios', icon: 'fas fa-futbol' },
      { id: 'horarios', name: 'Horarios & Franjas', icon: 'fas fa-clock' },
      { id: 'agenda', name: 'Agenda & Turnos', icon: 'fas fa-calendar-alt' },
      { id: 'estadisticas', name: 'Estadísticas & Cobros', icon: 'fas fa-chart-line' }
    ]
  } else {
    return [
      { id: 'explorar', name: 'Explorar', icon: 'fas fa-compass' },
      { id: 'reservar', name: 'Reservas', icon: 'fas fa-calendar-plus' },
      { id: 'mis-reservas', name: 'Mis Partidos', icon: 'fas fa-list-check' },
      { id: 'pagos', name: 'Pagos', icon: 'fas fa-wallet' },
      { id: 'cuenta', name: 'Cuenta & Perfil', icon: 'fas fa-user-shield' }
    ]
  }
})

// Base de Conocimiento para JUGADORES
const playerSections = [
  {
    id: 'explorar',
    title: 'Exploración y Canchas Disponibles',
    items: [
      {
        id: 'p-1',
        category: 'explorar',
        question: '¿Cómo busco canchas y complejos en mi ciudad?',
        shortTip: 'Usá el selector de ciudad y filtros de deportes en la pantalla de inicio.',
        icon: 'fas fa-map-marker-alt',
        iconBg: 'rgba(45,156,219,0.1)',
        iconColor: '#2D9CDB',
        answer: [
          'En la pantalla principal (Inicio), podés pulsar sobre el selector de ciudad ubicado en la barra superior para cambiar tu localidad (ej. Viedma, Carmen de Patagones, etc.).',
          'La app listará únicamente los complejos habilitados en esa localidad. Además, podés filtrar por deporte (Fútbol 5, Fútbol 8, Fútbol 11, Pádel, Tenis) usando el carrusel superior.'
        ],
        steps: [
          'Pulsá en el selector de ciudad en el encabezado superior.',
          'Elegí tu localidad para ver canchas cercanas.',
          'Tocá cualquier categoría de deporte para filtrar los complejos.',
          'Pulsá en la tarjeta del complejo para ver sus detalles y reservar.'
        ],
        tip: 'Si no encontrás resultados, podés limpiar los filtros presionando "Ver todos" o "Limpiar filtros".',
        actionRoute: { name: 'Home' },
        actionLabel: 'Ir al Inicio y Explorar'
      },
      {
        id: 'p-2',
        category: 'explorar',
        question: '¿Qué información puedo ver antes de reservar?',
        shortTip: 'Precios base por hora, fotos reales, dirección y categorías.',
        icon: 'fas fa-eye',
        iconBg: 'rgba(29,185,84,0.1)',
        iconColor: '#1DB954',
        answer: [
          'Cada tarjeta de complejo muestra la fotografía del lugar, la categoría deportiva, la dirección exacta y el precio por hora de referencia.',
          'Al ingresar a un complejo, podrás visualizar todos los días disponibles, las canchas registradas y los turnos horarios libres en tiempo real.'
        ]
      }
    ]
  },
  {
    id: 'reservar',
    title: 'Proceso de Reserva de Turnos',
    items: [
      {
        id: 'p-3',
        category: 'reservar',
        question: '¿Cómo realizo una nueva reserva paso a paso?',
        shortTip: 'Elegí día, deporte, cancha, horario y confirmá en segundos.',
        icon: 'fas fa-calendar-plus',
        iconBg: 'rgba(45,156,219,0.1)',
        iconColor: '#2D9CDB',
        answer: [
          'Reservar es muy sencillo y toma menos de un minuto:',
          '1. En Inicio, tocá sobre el complejo donde querés jugar.'
        ],
        steps: [
          'Seleccioná el día deseado en el carrusel de fechas.',
          'Elegí el deporte o tamaño de cancha (Fútbol 5, 8, etc.).',
          'Seleccioná la cancha específica y el horario disponible que prefieras.',
          'Elegí el medio de pago (Efectivo, Mercado Pago o Transferencia).',
          'Pulsá "Confirmar Reserva" para agendar tu turno de inmediato.'
        ],
        tip: 'Los horarios libres se actualizan al instante para evitar solapamientos con otros jugadores.',
        actionRoute: { name: 'Home' },
        actionLabel: 'Hacer una Reserva'
      },
      {
        id: 'p-4',
        category: 'reservar',
        question: '¿Por qué un horario aparece deshabilitado o no disponible?',
        shortTip: 'Validación atómica para garantizar que no haya turnos duplicados.',
        icon: 'fas fa-clock',
        iconBg: 'rgba(249,115,22,0.1)',
        iconColor: '#F97316',
        answer: [
          'La Reserva cuenta con un motor de validación atómica en tiempo real. Si un horario ya fue reservado por otro usuario, o si el dueño del complejo lo bloqueó para mantenimiento o partido fijo, no estará disponible para selección.',
          'Te recomendamos elegir otro horario libre o consultar en días próximos.'
        ]
      }
    ]
  },
  {
    id: 'mis-reservas',
    title: 'Gestión de Mis Reservas y Cancelaciones',
    items: [
      {
        id: 'p-5',
        category: 'mis-reservas',
        question: '¿Dónde encuentro mis partidos agendados y pasados?',
        shortTip: 'En la sección "Reservas" de la barra de navegación inferior.',
        icon: 'fas fa-list-check',
        iconBg: 'rgba(187,107,217,0.1)',
        iconColor: '#BB6BD9',
        answer: [
          'Al ingresar a "Reservas" en la barra inferior encontrarás tres pestañas ordenadas:',
          '• A Confirmar: Turnos en proceso de validación.',
          '• Próximas: Tus partidos confirmados listos para jugar, con fecha, hora y precio.',
          '• Pasadas / Historial: Registro de todos tus partidos anteriores jugados.'
        ],
        actionRoute: { name: 'MisReservas' },
        actionLabel: 'Ver Mis Reservas'
      },
      {
        id: 'p-6',
        category: 'mis-reservas',
        question: '¿Cómo cancelo una reserva si no puedo asistir?',
        shortTip: 'Abrí el menú de opciones (tres puntos) en la tarjeta de tu reserva.',
        icon: 'fas fa-ban',
        iconBg: 'rgba(239,68,68,0.1)',
        iconColor: '#EF4444',
        answer: [
          'Para cancelar una reserva, ingresá a la sección "Reservas", localizá la tarjeta del partido y tocá el botón de los tres puntos (⋮) en la esquina superior derecha.',
          'Seleccioná la opción "CANCELAR RESERVA" y confirmá la acción en el modal. El horario quedará liberado automáticamente para que otros jugadores puedan utilizarlo.'
        ],
        tip: 'Intentá cancelar con suficiente anticipación para que el complejo y otros jugadores puedan aprovechar el turno.'
      },
      {
        id: 'p-7',
        category: 'mis-reservas',
        question: '¿Puedo obtener la ubicación o teléfono del complejo desde mi reserva?',
        shortTip: 'Accedé al menú de opciones de la tarjeta para ver cómo llegar o llamar.',
        icon: 'fas fa-phone-volume',
        iconBg: 'rgba(16,185,129,0.1)',
        iconColor: '#10B981',
        answer: [
          'Sí. En el menú de cada tarjeta de reserva disponés de opciones directas como "Cómo llego" y "Teléfono Complejo" para comunicarte rápidamente.'
        ]
      }
    ]
  },
  {
    id: 'pagos',
    title: 'Medios de Pago y Cobros',
    items: [
      {
        id: 'p-8',
        category: 'pagos',
        question: '¿Qué medios de pago están disponibles?',
        shortTip: 'Efectivo en el complejo, Mercado Pago o Transferencia bancaria.',
        icon: 'fas fa-wallet',
        iconBg: 'rgba(45,156,219,0.1)',
        iconColor: '#2D9CDB',
        answer: [
          'Al momento de confirmar tu reserva podés seleccionar entre Efectivo (abonás en recepción al llegar al complejo), Mercado Pago o Transferencia Bancaria según la configuración de cada establecimiento.'
        ]
      }
    ]
  },
  {
    id: 'cuenta',
    title: 'Cuenta, Perfil y Configuración',
    items: [
      {
        id: 'p-9',
        category: 'cuenta',
        question: '¿Cómo cambio mi información de contacto o teléfono?',
        shortTip: 'Desde Menú → Mi Perfil o Menú → Configuración.',
        icon: 'fas fa-user-pen',
        iconBg: 'rgba(99,102,241,0.1)',
        iconColor: '#6366F1',
        answer: [
          'Ingresá a "Menú" en la barra inferior y seleccioná "Mi perfil" para revisar tus datos personales registrados, o "Configuración" para editar tu teléfono de contacto y activar o pausar notificaciones.'
        ],
        actionRoute: { name: 'UserProfile' },
        actionLabel: 'Ver Mi Perfil'
      },
      {
        id: 'p-10',
        category: 'cuenta',
        question: '¿Cómo funciona el inicio de sesión y la seguridad?',
        shortTip: 'Autenticación moderna y segura con Auth0.',
        icon: 'fas fa-shield-halved',
        iconBg: 'rgba(29,185,84,0.1)',
        iconColor: '#1DB954',
        answer: [
          'Tus credenciales y datos de usuario están protegidos mediante Auth0 con estándares de seguridad de última generación y cifrado de extremo a extremo.'
        ]
      }
    ]
  }
]

// Base de Conocimiento para DUEÑOS
const ownerSections = [
  {
    id: 'complejos',
    title: 'Gestión de Complejos Deportivos',
    items: [
      {
        id: 'o-1',
        category: 'complejos',
        question: '¿Cómo creo o doy de alta un nuevo complejo deportivo?',
        shortTip: 'Desde "Modo Dueño", pulsá en "Agregar Complejo" o el botón "+".',
        icon: 'fas fa-plus-circle',
        iconBg: 'rgba(45,156,219,0.1)',
        iconColor: '#2D9CDB',
        answer: [
          'Para dar de alta un complejo deportivo:',
          '1. En la pantalla principal de Modo Dueño, tocá "Agregar Complejo".',
          '2. Ingresá el nombre comercial de tu predio.',
          '3. Seleccioná la categoría deportiva principal (ej. Fútbol, Pádel, Multideporte).',
          '4. Asigná la ciudad correspondiente e ingresá la dirección exacta.',
          '5. Pegá la URL de una fotografía de portada (.png, .jpg o .jpeg).'
        ],
        steps: [
          'Nombre del complejo (ej. "La Cantera FC").',
          'Categoría y deportes ofrecidos.',
          'Ciudad y dirección física.',
          'Imagen de portada en alta calidad.'
        ],
        tip: 'Podés gestionar múltiples complejos bajo una misma cuenta de dueño.',
        actionRoute: { name: 'OwnerComplejoCreate' },
        actionLabel: 'Crear Nuevo Complejo'
      },
      {
        id: 'o-2',
        category: 'complejos',
        question: '¿Cómo edito la foto de portada, la dirección o la ciudad de mi complejo?',
        shortTip: 'Ingresá a "Configurar" en la tarjeta de tu complejo.',
        icon: 'fas fa-pen-to-square',
        iconBg: 'rgba(187,107,217,0.1)',
        iconColor: '#BB6BD9',
        answer: [
          'En Modo Dueño, tocá "Configurar" en la tarjeta del complejo:',
          '• Para la foto: Tocá el lápiz sobre la imagen del complejo para ingresar una nueva URL pública con previsualización en tiempo real.',
          '• Para la ubicación: Tocá el botón "Editar" en la fila de dirección para modificar la ciudad o la calle.'
        ],
        tip: 'La URL de la imagen debe ser pública y terminar en .png, .jpg o .jpeg.'
      },
      {
        id: 'o-3',
        category: 'complejos',
        question: '¿Cómo oculto temporalmente mi complejo para que no reciba reservas?',
        shortTip: 'Utilizá el botón de visibilidad "Ocultar / Mostrar".',
        icon: 'fas fa-eye-slash',
        iconBg: 'rgba(239,68,68,0.1)',
        iconColor: '#EF4444',
        answer: [
          'Dentro de la pantalla de configuración del complejo, en la esquina superior derecha verás el botón "Ocultar" / "Mostrar".',
          'Al cambiar el estado a "Oculto", el complejo no aparecerá en los resultados de búsqueda de los jugadores ni permitirá nuevas reservas hasta que decidas volver a activarlo.'
        ]
      }
    ]
  },
  {
    id: 'canchas',
    title: 'Administración de Canchas y Precios',
    items: [
      {
        id: 'o-4',
        category: 'canchas',
        question: '¿Cómo agrego una nueva cancha y configuro su precio?',
        shortTip: 'En "Configurar Complejo" → Canchas → botón "Agregar".',
        icon: 'fas fa-futbol',
        iconBg: 'rgba(45,156,219,0.1)',
        iconColor: '#2D9CDB',
        answer: [
          'Dentro de la configuración de tu complejo:',
          '1. En la sección "Canchas", pulsá el botón "Agregar".',
          '2. Asigná un nombre o número (ej. "Cancha 1 - Techada", "Cancha Sintética Principal").',
          '3. Seleccioná el tipo de cancha (Fútbol 5 Césped Sintético, Fútbol 8, Pádel Panorámico, etc.).',
          '4. Establecé el precio por hora en pesos argentinos (ARS).'
        ],
        steps: [
          'Nombre / Número identificador.',
          'Tipo de cancha / superficie.',
          'Precio por hora en ARS.'
        ]
      },
      {
        id: 'o-5',
        category: 'canchas',
        question: '¿Cómo actualizo el precio por hora de una cancha?',
        shortTip: 'Tocá el ícono de lápiz al lado del precio de la cancha.',
        icon: 'fas fa-tag',
        iconBg: 'rgba(16,185,129,0.1)',
        iconColor: '#10B981',
        answer: [
          'En la lista de canchas de tu complejo, al lado del valor por hora verás un ícono de lápiz. Hacé clic para abrir el modal de edición, ingresá la nueva tarifa y presioná "Actualizar Precio". Las nuevas reservas tomarán el nuevo precio inmediatamente.'
        ]
      },
      {
        id: 'o-6',
        category: 'canchas',
        question: '¿Cómo desactivo una cancha específica por mantenimiento o refacción?',
        shortTip: 'Presioná "Desactivar" en la tarjeta de la cancha.',
        icon: 'fas fa-toggle-on',
        iconBg: 'rgba(249,115,22,0.1)',
        iconColor: '#F97316',
        answer: [
          'Podés pausar una cancha en cualquier momento tocando "Desactivar". La cancha se marcará como inactiva y no se mostrará a los jugadores en la app hasta que la vuelvas a "Activar".'
        ]
      }
    ]
  },
  {
    id: 'horarios',
    title: 'Horarios de Apertura y Franjas Horarias',
    items: [
      {
        id: 'o-7',
        category: 'horarios',
        question: '¿Cómo configuro los días y horarios en los que está abierta cada cancha?',
        shortTip: 'En "Configurar Complejo", tocá el botón "Horarios" de la cancha.',
        icon: 'fas fa-clock',
        iconBg: 'rgba(99,102,241,0.1)',
        iconColor: '#6366F1',
        answer: [
          'Para habilitar turnos reservables:',
          '1. En la lista de canchas, tocá el botón "Horarios".',
          '2. Seleccioná el día de la semana (Lunes a Domingo).',
          '3. Definí la hora de inicio y la hora de finalización de la franja (ej. de 18:00 a 23:00).',
          '4. Tocá el botón "+" para guardar la franja horaria.'
        ],
        steps: [
          'Elegí el día de la semana en el desplegable.',
          'Definí hora de inicio y fin.',
          'Pulsá "+" para registrar la franja.',
          'El sistema generará los turnos por hora automáticamente para los jugadores.'
        ],
        tip: 'Podés cargar múltiples franjas para un mismo día si tu complejo abre en horario cortado (ej. tarde y noche).'
      },
      {
        id: 'o-8',
        category: 'horarios',
        question: '¿Cómo elimino una franja horaria cargada por error?',
        shortTip: 'En el modal de Horarios Disponibles, tocá el tacho de basura de la franja.',
        icon: 'fas fa-trash-alt',
        iconBg: 'rgba(239,68,68,0.1)',
        iconColor: '#EF4444',
        answer: [
          'Dentro del modal de horarios de la cancha, revisá la lista de "Franjas Registradas" y pulsá el ícono de eliminar en la franja que quieras quitar.'
        ]
      }
    ]
  },
  {
    id: 'agenda',
    title: 'Agenda en Vivo y Gestión de Turnos',
    items: [
      {
        id: 'o-9',
        category: 'agenda',
        question: '¿Cómo visualizo la agenda diaria de mis canchas?',
        shortTip: 'Desde "Mis Complejos", tocá el botón "Agenda" de tu complejo.',
        icon: 'fas fa-calendar-days',
        iconBg: 'rgba(45,156,219,0.1)',
        iconColor: '#2D9CDB',
        answer: [
          'La Agenda interactiva te muestra el día completo hora por hora:',
          '• Navegá entre fechas con los botones de flechas anterior/siguiente.',
          '• Si tenés varias canchas, alterná entre ellas con las pestañas superiores.',
          '• Los turnos tienen colores intuitivos: Azul (Reservado por jugador), Gris (Bloqueado por el dueño) y Blanco (Libre disponible).'
        ],
        actionRoute: { name: 'Home' },
        actionLabel: 'Ir a Mis Complejos y Ver Agenda'
      },
      {
        id: 'o-10',
        category: 'agenda',
        question: '¿Cómo bloqueo un horario para reservas telefónicas o uso propio?',
        shortTip: 'Tocá un turno en estado "Libre" y seleccioná "Bloquear Horario".',
        icon: 'fas fa-lock',
        iconBg: 'rgba(100,116,139,0.1)',
        iconColor: '#64748B',
        answer: [
          'Si un cliente te reserva por WhatsApp o teléfono, o necesitás reservar la cancha para torneo o limpieza:',
          '1. Buscá el turno libre en la agenda.',
          '2. Tocá sobre la tarjeta del turno libre.',
          '3. Presioná "Bloquear Horario". El turno pasará a estado bloqueado y ningún jugador online podrá seleccionarlo.'
        ],
        tip: 'Podés desbloquear el turno en cualquier momento tocándolo nuevamente y seleccionando "Desbloquear Horario".'
      },
      {
        id: 'o-11',
        category: 'agenda',
        question: '¿Cómo veo los datos del jugador y el detalle de una reserva?',
        shortTip: 'Tocá el turno reservado y seleccioná "Ver detalle reserva".',
        icon: 'fas fa-user-check',
        iconBg: 'rgba(16,185,129,0.1)',
        iconColor: '#10B981',
        answer: [
          'Al tocar cualquier turno reservado en la agenda podrás presionar "Ver detalle reserva" para consultar el nombre completo del jugador, su número de teléfono, correo electrónico, estado del pago (Pagado o Pendiente) y medio de pago utilizado.'
        ]
      },
      {
        id: 'o-12',
        category: 'agenda',
        question: '¿Cómo cancelo una reserva de un cliente desde la agenda?',
        shortTip: 'Tocá el turno reservado y seleccioná "Cancelar Turno".',
        icon: 'fas fa-calendar-xmark',
        iconBg: 'rgba(239,68,68,0.1)',
        iconColor: '#EF4444',
        answer: [
          'En caso de fuerza mayor o solicitud del cliente, tocá el turno reservado, pulsá "Cancelar Turno" y confirmá la acción. El turno se cancelará en el sistema y quedará libre de inmediato.'
        ]
      }
    ]
  },
  {
    id: 'estadisticas',
    title: 'Estadísticas del Día, Ocupación y Ganancias',
    items: [
      {
        id: 'o-13',
        category: 'estadisticas',
        question: '¿Cómo veo el resumen de ganancias y ocupación del día?',
        shortTip: 'Tocá el botón de menú (tres puntos) en la esquina superior de la Agenda.',
        icon: 'fas fa-chart-line',
        iconBg: 'rgba(16,185,129,0.1)',
        iconColor: '#10B981',
        answer: [
          'En la vista de Agenda de cualquier complejo, tocá el botón circular con tres puntos (⋮) en el encabezado superior.',
          'Se desplegará el panel de Estadísticas del Día con métricas financieras y operativas calculadas en tiempo real:'
        ],
        steps: [
          'Ganancias Confirmadas totales en ARS.',
          'Desglose entre dinero Cobrado vs Por Cobrar (Pendiente).',
          'Cantidad de reservas totales, confirmadas y pendientes.',
          'Porcentaje de Ocupación con barra de progreso visual.',
          'Horarios disponibles y bloqueados.',
          'Rendimiento individual por cada cancha del complejo.',
          'Desglose de ingresos por cada medio de pago registrado.'
        ]
      }
    ]
  }
]

const currentBaseSections = computed(() => {
  return isOwnerRole.value ? ownerSections : playerSections
})

const totalItemsCount = computed(() => {
  return currentBaseSections.value.reduce((acc, sec) => acc + sec.items.length, 0)
})

// Filtrado de secciones según búsqueda y categoría seleccionada
const filteredSections = computed(() => {
  const query = searchQuery.value.trim().toLowerCase()
  const cat = selectedCategory.value

  return currentBaseSections.value
    .map(section => {
      const items = section.items.filter(item => {
        const matchesCat = cat === 'all' || item.category === cat
        if (!matchesCat) return false

        if (!query) return true

        const inQuestion = item.question.toLowerCase().includes(query)
        const inTip = item.shortTip?.toLowerCase().includes(query) || item.tip?.toLowerCase().includes(query)
        const inAnswer = item.answer.some(a => a.toLowerCase().includes(query))
        const inSteps = item.steps ? item.steps.some(s => s.toLowerCase().includes(query)) : false

        return inQuestion || inTip || inAnswer || inSteps
      })

      return {
        ...section,
        items
      }
    })
    .filter(section => section.items.length > 0)
})

// Control de expandir / colapsar todo
const areAllExpanded = computed(() => {
  const allItemIds = []
  filteredSections.value.forEach(s => s.items.forEach(i => allItemIds.push(i.id)))
  return allItemIds.length > 0 && allItemIds.every(id => openItems.value.includes(id))
})

const toggleExpandAll = () => {
  const allItemIds = []
  filteredSections.value.forEach(s => s.items.forEach(i => allItemIds.push(i.id)))
  
  if (areAllExpanded.value) {
    openItems.value = []
  } else {
    openItems.value = [...allItemIds]
  }
}

// Al montar, abrir la primera pregunta por defecto para orientar al usuario
onMounted(() => {
  if (isOwnerRole.value) {
    openItems.value = ['o-1', 'o-9']
  } else {
    openItems.value = ['p-1', 'p-3']
  }
})
</script>

<style scoped>
.animate-slide-up {
  animation: slideUp 0.35s cubic-bezier(0.16, 1, 0.3, 1) both;
}

.animate-fade-in {
  animation: fadeIn 0.25s cubic-bezier(0.16, 1, 0.3, 1) both;
}

@keyframes slideUp {
  from {
    transform: translateY(12px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(-4px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.no-scrollbar::-webkit-scrollbar {
  display: none;
}
.no-scrollbar {
  -ms-overflow-style: none;
  scrollbar-width: none;
  -webkit-overflow-scrolling: touch;
}
</style>
