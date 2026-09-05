const BASE_URL = import.meta.env.VITE_API_BASE_URL || 'https://localhost:7116/api'

export const API_CONFIG = {
  baseURL: BASE_URL
}

export const API_ENDPOINTS = {
  referenceData: {
    getAll: () => `${BASE_URL}/reference-data`
  },
  complejos: {
    getAll: (duenoId) => duenoId ? `${BASE_URL}/Complejos?duenoId=${duenoId}` : `${BASE_URL}/Complejos`,
    getById: (id) => `${BASE_URL}/Complejos/${id}`,
    getAgenda: (id, fecha, usuarioId, soloReservas = false) => `${BASE_URL}/Complejos/${id}/agenda?fecha=${fecha}&usuarioId=${usuarioId}&soloReservas=${soloReservas}`,
    getEstadisticasDia: (id, fecha, usuarioId) => `${BASE_URL}/Complejos/${id}/estadisticas-dia?fecha=${fecha}&usuarioId=${usuarioId}`,
    getEstadisticas: (complejoId, fechaInicio, fechaFin, usuarioId) => 
      `${BASE_URL}/Complejos/estadisticas?${complejoId ? `complejoId=${complejoId}&` : ''}fechaInicio=${fechaInicio}&fechaFin=${fechaFin}&usuarioId=${usuarioId}`,
    getConfig: (id, usuarioId) => `${BASE_URL}/Complejos/${id}/config?usuarioId=${usuarioId}`,
    create: () => `${BASE_URL}/Complejos`,
    createCancha: (id) => `${BASE_URL}/Complejos/${id}/canchas`,
    updateEstado: (id) => `${BASE_URL}/Complejos/${id}/estado`,
    updateImagen: (id) => `${BASE_URL}/Complejos/${id}/imagen`,
    updateDireccion: (id) => `${BASE_URL}/Complejos/${id}/direccion`
  },
  canchas: {
    getHorarios: (canchaId) => `${BASE_URL}/Canchas/${canchaId}/horarios`,
    createHorario: (canchaId) => `${BASE_URL}/Canchas/${canchaId}/horarios`,
    deleteHorario: (canchaId, horarioId) => `${BASE_URL}/Canchas/${canchaId}/horarios/${horarioId}`,
    updateEstado: (canchaId) => `${BASE_URL}/Canchas/${canchaId}/estado`,
    updatePrecio: (canchaId) => `${BASE_URL}/Canchas/${canchaId}/precio`
  },
  reservas: {
    getNext: (usuarioId) => `${BASE_URL}/Reserva/GetNext?UsuarioId=${usuarioId}`,
    getHorariosDisponibles: (complejoId) => `${BASE_URL}/Reserva/HorariosDisponiblesComplejo?ComplejoId=${complejoId}`,
    getUserReservations: (usuarioId) => `${BASE_URL}/Reserva/UserReservations?UsuarioId=${usuarioId}`,
    cancelReservation: (id) => `${BASE_URL}/Reserva/CancelReservation/${id}`,
    confirm: (id, usuarioId) => `${BASE_URL}/Reserva/ConfirmReservation/${id}?UsuarioId=${usuarioId}`,
    create: `${BASE_URL}/Reserva/CreateReserva`,
    block: `${BASE_URL}/Reserva/BlockHorario`
  },
  usuarios: {
    getProfile: (id) => `${BASE_URL}/Usuario/${id}`,
    getAll: () => `${BASE_URL}/Usuario`,
    create: () => `${BASE_URL}/Usuario`,
    login: () => `${BASE_URL}/Usuario/login`,
    cambiarPassword: () => `${BASE_URL}/Usuario/cambiar-password`,
    baja: (id) => `${BASE_URL}/Usuario/${id}/baja`,
    reactivar: (id) => `${BASE_URL}/Usuario/${id}/reactivar`
  },
  medioPagos: {
    getAll: () => `${BASE_URL}/MedioPago`
  }
}

export default API_CONFIG
