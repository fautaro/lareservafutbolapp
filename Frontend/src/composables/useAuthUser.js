import { ref, computed } from 'vue';
import { useAuth0 } from '@auth0/auth0-vue';
import { auth0Config } from '../config/auth0Config';
import { useRole } from './useRole';

const localUser = ref(JSON.parse(localStorage.getItem('localUser') || 'null'));
const debeCambiarPassword = ref(
  localStorage.getItem('debeCambiarPassword') === 'true' || 
  (localUser.value && !!localUser.value.debeCambiarPassword)
);

/**
 * Composable para manejar funcionalidades de autenticación (Auth0 y Local)
 * Proporciona acceso fácil a datos del usuario y funciones de autenticación
 */
export const useAuthUser = () => {
  const auth0 = useAuth0();
  const { setRole } = useRole();

  // Usuario actual
  const user = computed(() => {
    if (localUser.value) {
      return {
        id: localUser.value.id,
        sub: localUser.value.id,
        name: `${localUser.value.nombre} ${localUser.value.apellido || ''}`.trim(),
        email: localUser.value.email,
        picture: 'https://via.placeholder.com/150',
        rol: localUser.value.rol,
        tipoUsuario: localUser.value.tipoUsuario,
        debeCambiarPassword: debeCambiarPassword.value
      };
    }

    if (auth0Config.loginDisabled) {
      return {
        id: 1,
        name: 'Usuario Test',
        email: 'test@example.com',
        picture: 'https://via.placeholder.com/150',
        sub: auth0Config.userTest
      };
    }
    return auth0.user.value;
  });

  // Nombre del usuario
  const userName = computed(() => {
    if (localUser.value) {
      return `${localUser.value.nombre} ${localUser.value.apellido || ''}`.trim();
    }
    if (auth0Config.loginDisabled) return 'Usuario Test';
    return auth0.user?.value?.name || auth0.user?.value?.email || 'Usuario';
  });

  // Email del usuario
  const userEmail = computed(() => {
    if (localUser.value) return localUser.value.email;
    if (auth0Config.loginDisabled) return 'test@example.com';
    return auth0.user?.value?.email;
  });

  // Avatar del usuario
  const userAvatar = computed(() => {
    if (auth0Config.loginDisabled || localUser.value) return 'https://via.placeholder.com/150';
    return auth0.user?.value?.picture;
  });

  // Iniciales del nombre
  const userInitials = computed(() => {
    const name = userName.value || 'U';
    return name
      .split(' ')
      .map((n) => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  });

  const setLocalSession = (userData) => {
    localUser.value = userData;
    debeCambiarPassword.value = !!userData.debeCambiarPassword;
    localStorage.setItem('localUser', JSON.stringify(userData));
    localStorage.setItem('debeCambiarPassword', userData.debeCambiarPassword ? 'true' : 'false');
    if (userData.rol) {
      setRole(userData.rol);
    }
  };

  const setDebeCambiarPassword = (value) => {
    debeCambiarPassword.value = value;
    localStorage.setItem('debeCambiarPassword', value ? 'true' : 'false');
    if (localUser.value) {
      localUser.value.debeCambiarPassword = value;
      localStorage.setItem('localUser', JSON.stringify(localUser.value));
    }
  };

  const logoutLocal = () => {
    localUser.value = null;
    debeCambiarPassword.value = false;
    localStorage.removeItem('localUser');
    localStorage.removeItem('debeCambiarPassword');
  };

  const handleLogout = (options) => {
    logoutLocal();
    if (auth0 && auth0.isAuthenticated && auth0.isAuthenticated.value) {
      auth0.logout(options);
    } else {
      window.location.href = '/#/login';
    }
  };

  return {
    // Estado
    isAuthenticated: computed(() => !!localUser.value || auth0Config.loginDisabled || auth0.isAuthenticated.value),
    isLoading: auth0.isLoading,
    error: auth0.error,
    debeCambiarPassword,

    // Datos del usuario
    user,
    userName,
    userEmail,
    userAvatar,
    userInitials,

    // Métodos
    login: auth0.loginWithRedirect,
    logout: handleLogout,
    loginWithPopup: auth0.loginWithPopup,
    setLocalSession,
    setDebeCambiarPassword,
    logoutLocal
  };
};
