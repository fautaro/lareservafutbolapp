<template>
  <div class="min-h-screen bg-slate-50 pb-28 text-slate-800" style="font-family: Inter, 'Noto Sans', sans-serif;">

    <!-- ══ HEADER — version con CitySelectorDrawer ══ -->
    <nav
      class="sticky top-4 z-50 bg-white/95 backdrop-blur-md shadow-sm border border-slate-100 mx-2 rounded-2xl px-3 py-3 mb-6 flex items-center justify-between">
      <div class="flex flex-col items-center gap-1 min-w-[80px]">
        <svg class="h-8 w-auto" viewBox="0 0 52 40" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path d="M0 0.19043V39.8487H30.8524L24.8072 29.1348H10.1606V0.19043H0Z" fill="#2D9CDB" />
          <path
            d="M37.7816 0.00362063C45.0797 -0.165953 51.306 5.64486 51.8218 12.4088C52.25 18.0208 48.8028 23.5772 43.2304 25.941L51.8563 39.849H38.6107L29.3726 26.1281H15.4243V17.5764H32.7911C33.1173 17.5808 34.5456 17.5662 35.7469 16.4493C36.7024 15.5605 36.6765 14.4714 36.7513 14.0884C36.7211 13.761 36.859 12.5404 35.8388 11.5624C34.5212 10.2994 32.8371 10.4982 32.6057 10.5289H15.4272V0.192198C15.4272 0.192198 37.3203 0.0138535 37.7816 0.00362063Z"
            fill="#2D9CDB" />
        </svg>
        <h1 class="text-[13px] font-bold text-[#2D9CDB] tracking-tight leading-none">
          La Reserva
        </h1>
      </div>
      <template v-if="isAuthenticated">
        <!-- logout styled like city drawer button -->
        <button @click="logoutWithAuth0"
            class="group flex items-center gap-2 px-2 py-1.5 rounded-xl transition-all active:scale-95 focus:outline-none ml-auto text-gray-500">
            <div class="flex items-center justify-center transition-transform group-hover:scale-110">
            </div>
            <div class="flex flex-col items-start leading-tight">
                <span class="text-[13px] font-bold text-gray-500">Cerrar sesión</span>
            </div>
        </button>
      </template>
    </nav>

    <!-- ══ CUERPO ══ -->
    <div class="px-3 pt-5 animate-slide-up">

      <!-- Sección: Modo (Switch) -->
      <p class="section-label">Modo de uso</p>
      <div class="nav-card mb-5 p-1">
        <div class="flex p-1 bg-slate-50 rounded-[14px]">
          <button 
            @click="setRole('user')"
            class="flex-1 py-3 text-[11px] font-black uppercase tracking-widest transition-all rounded-xl"
            :class="isUser() ? 'bg-white text-[#2D9CDB] shadow-sm' : 'text-slate-400'"
          >
            Modo Usuario
          </button>
          <button 
            @click="setRole('owner')"
            class="flex-1 py-3 text-[11px] font-black uppercase tracking-widest transition-all rounded-xl"
            :class="isOwner() ? 'bg-white text-[#2D9CDB] shadow-sm' : 'text-slate-400'"
          >
            Modo Dueño
          </button>
        </div>
      </div>

      <!-- ── Estado autenticado ── -->
      <template v-if="isAuthenticated">

        <!-- Sección: Dueño (Solo si es modo dueño) -->
        <template v-if="isOwner()">
          <p class="section-label">Gestión de Dueño</p>
          <div class="nav-card mb-5">
            <router-link :to="{ name: 'Home' }" class="nav-row">
              <div class="nav-icon" style="background: rgba(45,156,219,0.1);">
                <i class="fas fa-list-check" style="color: #2D9CDB;"></i>
              </div>
              <span class="nav-text">Mis canchas</span>
              <i class="fas fa-chevron-right nav-arrow"></i>
            </router-link>
            <div class="nav-sep"></div>
            <button class="nav-row w-full text-left">
              <div class="nav-icon" style="background: rgba(29,185,84,0.1);">
                <i class="fas fa-plus" style="color: #1DB954;"></i>
              </div>
              <span class="nav-text">Agregar cancha</span>
              <i class="fas fa-chevron-right nav-arrow"></i>
            </button>
          </div>
        </template>

        <!-- Sección: Cuenta -->
        <p class="section-label">Cuenta</p>
        <div class="nav-card mb-5">
          <router-link :to="{ name: 'UserProfile' }" class="nav-row">
            <div class="nav-icon" style="background: rgba(45,156,219,0.1); /* #2D9CDB @10% */">
              <i class="fas fa-user" style="color: var(--brand-blue); /* #2D9CDB */"></i>
            </div>
            <span class="nav-text">Mi perfil</span>
            <i class="fas fa-chevron-right nav-arrow"></i>
          </router-link>

          <div class="nav-sep"></div>

          <router-link :to="{ name: 'ConfigUser' }" class="nav-row">
            <div class="nav-icon" style="background: rgba(187,107,217,0.1); /* #BB6BD9 @10% */">
              <i class="fas fa-sliders" style="color: var(--brand-purple); /* #BB6BD9 */"></i>
            </div>
            <span class="nav-text">Configuración</span>
            <i class="fas fa-chevron-right nav-arrow"></i>
          </router-link>
        </div>

        <!-- Sección: Soporte -->
        <p class="section-label">Soporte</p>
        <div class="nav-card mb-5">
          <router-link :to="{ name: 'Help' }" class="nav-row">
            <div class="nav-icon" style="background: rgba(29,185,84,0.1); /* #1DB954 @10% */">
              <i class="fas fa-circle-question" style="color: var(--brand-green); /* #1DB954 */"></i>
            </div>
            <span class="nav-text">Ayuda</span>
            <i class="fas fa-chevron-right nav-arrow"></i>
          </router-link>
        </div>

        <!-- Sección: Sesión -->

      </template>

      <!-- ── Estado no autenticado ── -->
      <template v-else>
        <div class="nav-card p-8 flex flex-col items-center text-center">
          <div class="unauth-icon">
            <i class="fas fa-user" style="font-size:24px; color: var(--brand-gray); /* #B3B3B3 */"></i>
          </div>
          <h2 class="text-base font-bold text-slate-900 mb-1">¡Bienvenido!</h2>
          <p class="text-sm text-slate-400 mb-6 max-w-[200px] leading-relaxed">
            Ingresá para ver tus reservas y configurar tu cuenta.
          </p>
          <button @click="loginWithAuth0" class="login-btn" style="background: var(--brand-blue, #2D9CDB); /* #2D9CDB */">
            <i class="fas fa-sign-in-alt"></i>
            Ingresar
          </button>
        </div>
      </template>

    </div>
    <footer class="text-center text-xs text-gray-400 mt-6 mb-4">La Reserva - 2026</footer>
  </div>
</template>

<script setup>
import { useAuthUser } from '../composables/useAuthUser';
import { useRole } from '../composables/useRole';

const { isAuthenticated, login, logout } = useAuthUser();
const { isOwner, isUser, setRole } = useRole();

const loginWithAuth0 = () => login();
const logoutWithAuth0 = () => logout({ logoutParams: { returnTo: window.location.origin } });
</script>

<style scoped>
/* header-block styles are no longer used; header is a nav */

/* ── Section label ──────────────────────────────────────────────── */
.section-label {
  font-size: 10px;
  font-weight: 900;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: var(--brand-gray, #B3B3B3);
  margin: 0 0 8px 2px;
}

/* ── Nav card — las cards sí son "tappable" ─────────────────────── */
.nav-card {
  background: #ffffff;
  border-radius: 18px;
  border: 1px solid #E9EEF4;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.05);
  overflow: hidden;
}

/* ── Nav row ─────────────────────────────────────────────────────── */
.nav-row {
  display: flex;
  align-items: center;
  gap: 13px;
  padding: 13px 16px;
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
  /* slightly lighter than brand-dark for better readability */
  color: #374151; /* slate-700 */
  text-align: left;
}

.nav-arrow {
  font-size: 9px;
  color: var(--brand-gray-light, #E0E0E0);
}

.nav-sep {
  height: 1px;
  background: #F1F5F9;
  margin: 0 16px;
}

/* ── Unauthenticated icon ───────────────────────────────────────── */
.unauth-icon {
  width: 64px;
  height: 64px;
  background: var(--brand-gray-light, #E0E0E0);
  border-radius: 20px;
  border: 1px solid #E2E8F0;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 16px;
}

/* ── Login button ───────────────────────────────────────────────── */
.login-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  width: 100%;
  padding: 13px 20px;
  background: var(--brand-blue, #2D9CDB);
  color: #fff;
  font-size: 13px;
  font-weight: 700;
  border-radius: 14px;
  border: none;
  cursor: pointer;
  box-shadow: 0 4px 16px rgba(45, 156, 219, 0.25);
  transition: opacity 0.12s ease;
  letter-spacing: 0.02em;
}

.login-btn:active {
  opacity: 0.85;
}

/* ── Animation ──────────────────────────────────────────────────── */
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
</style>
