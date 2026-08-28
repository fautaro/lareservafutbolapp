const BASE_URL = 'https://localhost:7116/api'

export const API_CONFIG = {
  baseURL: BASE_URL
}

export const API_ENDPOINTS = {
  complejos: {
    getAll: (duenoId) => duenoId ? `${BASE_URL}/Complejos?duenoId=${duenoId}` : `${BASE_URL}/Complejos`,
    getById: (id) => `${BASE_URL}/Complejos/${id}`,
    getAgenda: (id, fecha, usuarioId, soloReservas = false) => `${BASE_URL}/Complejos/${id}/agenda?fecha=${fecha}&usuarioId=${usuarioId}&soloReservas=${soloReservas}`
  },
  reservas: {
    getNext: (usuarioId) => `${BASE_URL}/Reserva/GetNext?UsuarioId=${usuarioId}`,
    getHorariosDisponibles: (complejoId) => `${BASE_URL}/Reserva/HorariosDisponiblesComplejo?ComplejoId=${complejoId}`,
    getUserReservations: (usuarioId) => `${BASE_URL}/Reserva/UserReservations?UsuarioId=${usuarioId}`,
    cancelReservation: (id) => `${BASE_URL}/Reserva/CancelReservation/${id}`,
    create: `${BASE_URL}/Reserva/CreateReserva`,
    block: `${BASE_URL}/Reserva/BlockHorario`
  },
  usuarios: {
    getProfile: (id) => `${BASE_URL}/Usuario/${id}`
  },
  medioPagos: {
    getAll: () => `${BASE_URL}/MedioPago`
  }
}

export default API_CONFIG
