<template>
  <div class="min-h-screen bg-slate-50 pb-28 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">

    <div class="px-3 pt-6 animate-slide-up max-w-4xl mx-auto">

      <div class="bg-white rounded-[24px] shadow-md sticky top-4 z-40 overflow-hidden mx-1 pb-2 border border-slate-100/50">
        <div class="px-4 pt-8 pb-5">
          <h1 class="text-3xl font-bold tracking-tight text-slate-900 leading-none">Centro de ayuda</h1>
          <p class="text-slate-400 text-sm font-medium mt-2">Última actualización: 31 de julio de 2025</p>
        </div>
      </div>

      <div class="nav-card mt-4">
        <div class="p-3 space-y-2">
          <!-- Sección: Cuenta -->
          <p class="section-label">Cuenta</p>
          <div class="space-y-1">
            <details class="group">
              <summary class="nav-row">
                <div class="nav-icon" style="background: rgba(45,156,219,0.06);">
                  <i class="fas fa-user text-[13px]" style="color: var(--brand-blue, #2D9CDB);"></i>
                </div>
                <span class="nav-text">¿Cómo cambio mi contraseña?</span>
                <i class="fas fa-chevron-down nav-arrow transition-transform group-open:rotate-180"></i>
              </summary>
              <div class="px-4 pb-4 text-sm text-slate-600 dropdown-content">Podés cambiar tu contraseña desde la sección "Mi perfil" → "Seguridad".</div>
            </details>

            <details class="group">
              <summary class="nav-row">
                <div class="nav-icon" style="background: rgba(187,107,217,0.06);">
                  <i class="fas fa-envelope text-[13px]" style="color: var(--brand-purple, #BB6BD9);"></i>
                </div>
                <span class="nav-text">¿Cómo actualizo mi correo?</span>
                <i class="fas fa-chevron-down nav-arrow transition-transform group-open:rotate-180"></i>
              </summary>
              <div class="px-4 pb-4 text-sm text-slate-600 dropdown-content">En "Mi perfil" → "Datos personales" podés actualizar tu correo. Recordá verificarlo.</div>
            </details>
          </div>

          <!-- Sección: Reservas -->
          <p class="section-label mt-4">Reservas</p>
          <div class="space-y-1">
            <details class="group">
              <summary class="nav-row">
                <div class="nav-icon" style="background: rgba(45,156,219,0.06);">
                  <i class="fas fa-futbol text-[13px]" style="color: var(--brand-blue, #2D9CDB);"></i>
                </div>
                <span class="nav-text">¿Cómo reservo una cancha?</span>
                <i class="fas fa-chevron-down nav-arrow transition-transform group-open:rotate-180"></i>
              </summary>
              <div class="px-4 pb-4 text-sm text-slate-600 dropdown-content">Ingresá a "Reservar", elegí complejo, día y horario, y confirmá la reserva.</div>
            </details>

            <details class="group">
              <summary class="nav-row">
                <div class="nav-icon" style="background: rgba(29,185,84,0.06);">
                  <i class="fas fa-ban text-[13px]" style="color: var(--brand-green, #1DB954);"></i>
                </div>
                <span class="nav-text">¿Puedo cancelar una reserva?</span>
                <i class="fas fa-chevron-down nav-arrow transition-transform group-open:rotate-180"></i>
              </summary>
              <div class="px-4 pb-4 text-sm text-slate-600 dropdown-content">Podés cancelar desde "Mis reservas" hasta 2 horas antes del horario agendado.</div>
            </details>
          </div>

          <!-- Sección: Soporte técnico -->
          <p class="section-label mt-4">Soporte técnico</p>
          <div class="space-y-1">
            <details class="group">
              <summary class="nav-row">
                <div class="nav-icon" style="background: rgba(99,102,241,0.06);">
                  <i class="fas fa-exclamation-triangle text-[13px]" style="color: #6366F1;"></i>
                </div>
                <span class="nav-text">No puedo iniciar sesión</span>
                <i class="fas fa-chevron-down nav-arrow transition-transform group-open:rotate-180"></i>
              </summary>
              <div class="px-4 pb-4 text-sm text-slate-600 dropdown-content">Verificá el correo y contraseña. Si olvidaste la contraseña, usá "¿Olvidaste tu contraseña?".</div>
            </details>

            <details class="group">
              <summary class="nav-row">
                <div class="nav-icon" style="background: rgba(249,115,22,0.06);">
                  <i class="fas fa-envelope-open-text text-[13px]" style="color: #F97316;"></i>
                </div>
                <span class="nav-text">¿Cómo reporto un problema?</span>
                <i class="fas fa-chevron-down nav-arrow transition-transform group-open:rotate-180"></i>
              </summary>
              <div class="px-4 pb-4 text-sm text-slate-600 dropdown-content">Escribinos a soporte@tuapp.com o usá "Enviar reporte" en el menú lateral.</div>
            </details>
          </div>
        </div>
      </div>

      <footer class="text-center text-xs text-gray-400 mt-6 mb-4">La Reserva - 2026</footer>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Help'
}
</script>

<style scoped>
/* Reused visual language from Menu.vue for consistent UI/UX */
.section-label {
  font-size: 10px;
  font-weight: 900;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: var(--brand-gray, #B3B3B3);
  margin: 0 0 8px 2px;
}

.nav-card {
  background: #ffffff;
  border-radius: 18px;
  border: 1px solid #E9EEF4;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.05);
  overflow: hidden;
}

.nav-row {
  display: flex;
  align-items: center;
  gap: 13px;
  padding: 13px 12px;
  text-decoration: none;
  color: inherit;
  cursor: pointer;
  border: none;
  background: transparent;
  width: 100%;
  transition: background 0.1s ease;
}

.nav-row:active {
  background: var(--brand-gray-light, #E0E0E0);
}

.nav-icon {
  width: 36px;
  height: 36px;
  border-radius: 11px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 13px;
  flex-shrink: 0;
}

.nav-text {
  flex: 1;
  font-size: 14px;
  font-weight: 600;
  color: #374151;
  text-align: left;
}

.nav-arrow {
  font-size: 9px;
  color: var(--brand-gray-light, #E0E0E0);
}

.nav-sep {
  height: 1px;
  background: #F1F5F9;
  margin: 0 0;
}

.animate-slide-up {
  animation: slideUp 0.45s cubic-bezier(0.16, 1, 0.3, 1) both;
}

@keyframes slideUp {
  from {
    transform: translateY(16px);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}

details > summary::-webkit-details-marker { display: none; }
details summary { list-style: none; }
</style>

<style scoped>
/* Dropdown open/close animation */
.dropdown-content {
  max-height: 0;
  opacity: 0;
  transform: translateY(-6px);
  transition: max-height 260ms cubic-bezier(0.16, 1, 0.3, 1), opacity 200ms ease, transform 260ms ease;
  overflow: hidden;
}

details[open] .dropdown-content {
  max-height: 480px; /* large enough to fit content */
  opacity: 1;
  transform: translateY(0);
}

/* Make summary more tappable */
details summary {
  list-style: none;
  cursor: pointer;
}

</style>
