import { computed } from 'vue';
import { useAuth0 } from '@auth0/auth0-vue';

/**
 * Composable para manejar funcionalidades de Auth0
 * Proporciona acceso fácil a datos del usuario y funciones de autenticación
 */
export const useAuthUser = () => {
  const auth0 = useAuth0();

  // Usuario actual
  const user = computed(() => auth0.user);

  // Nombre del usuario
  const userName = computed(() => {
    return auth0.user?.value?.name || auth0.user?.value?.email || 'Usuario';
  });

  // Email del usuario
  const userEmail = computed(() => auth0.user?.value?.email);

  // Avatar del usuario
  const userAvatar = computed(() => auth0.user?.value?.picture);

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
    isAuthenticated: auth0.isAuthenticated,
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
