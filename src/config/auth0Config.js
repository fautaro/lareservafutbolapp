// Configuración de Auth0
export const auth0Config = {
  domain: 'lareservafutbolapp.us.auth0.com',
  clientId: 'bzRamnbU2H9atLfPIBVZumuoBwfQyaE6',
  redirectUri: window.location.origin,
  loginDisabled: true,
  userTest: 1,
};

// Función para validar que las credenciales estén configuradas
export const validateAuth0Config = () => {
  const { domain, clientId, redirectUri } = auth0Config;

  if (!domain || domain === '') {
    console.error('Auth0: domain no configurado en config/auth0Config.js');
    return false;
  }

  if (!clientId || clientId === '') {
    console.error('Auth0: clientId no configurado en config/auth0Config.js');
    return false;
  }

  return true;
};
