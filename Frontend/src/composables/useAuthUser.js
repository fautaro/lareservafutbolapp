import { computed } from 'vue';
import { useAuth0 } from '@auth0/auth0-vue';
import { auth0Config } from '../config/auth0Config';

/**
 * Composable para manejar funcionalidades de Auth0
 * Proporciona acceso fácil a datos del usuario y funciones de autenticación
 */
export const useAuthUser = () => {
  const auth0 = useAuth0();

  // Usuario actual
  const user = computed(() => {
    if (auth0Config.loginDisabled) {
      return {
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
    if (auth0Config.loginDisabled) return 'Usuario Test';
    return auth0.user?.value?.name || auth0.user?.value?.email || 'Usuario';
  });

  // Email del usuario
  const userEmail = computed(() => {
    if (auth0Config.loginDisabled) return 'test@example.com';
    return auth0.user?.value?.email;
  });

  // Avatar del usuario
  const userAvatar = computed(() => {
    if (auth0Config.loginDisabled) return 'https://via.placeholder.com/150';
    return auth0.user?.value?.picture;
  });

  // Iniciales del nombre
  const userInitials = computed(() => {
    const name = auth0.user?.value?.name || 'U';
    return name
      .split(' ')
      .map((n) => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
  });

  return {
    // Estado de Auth0
    isAuthenticated: computed(() => auth0Config.loginDisabled || auth0.isAuthenticated.value),
    isLoading: auth0.isLoading,
    error: auth0.error,

    // Datos del usuario
    user,
    userName,
    userEmail,
    userAvatar,
    userInitials,

    // Métodos
    login: auth0.loginWithRedirect,
    logout: auth0.logout,
    loginWithPopup: auth0.loginWithPopup,
  };
};
