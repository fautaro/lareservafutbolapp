const BASE_URL = 'https://localhost:7116/api'

export const API_CONFIG = {
  baseURL: BASE_URL
}

export const API_ENDPOINTS = {
  complejos: {
    getAll: () => `${BASE_URL}/Complejos`
  }
}

export default API_CONFIG
