const BASE_URL = 'https://localhost:7116/api'

export const API_CONFIG = {
  baseURL: BASE_URL
}

export const API_ENDPOINTS = {
  complejos: {
    getAll: () => `${BASE_URL}/Complejos`,
    getById: (id) => `${BASE_URL}/Complejos/${id}`
  },
  reservas: {
    getHorariosDisponibles: (complejoId) => `${BASE_URL}/Reserva/HorariosDisponiblesComplejo?ComplejoId=${complejoId}`,
    getUserReservations: (usuarioId) => `${BASE_URL}/Reserva/UserReservations?UsuarioId=${usuarioId}`,
    cancelReservation: (id) => `${BASE_URL}/Reserva/CancelReservation/${id}`
  },
  usuarios: {
    getProfile: (id) => `${BASE_URL}/Usuario/${id}`
  }
}

export default API_CONFIG
