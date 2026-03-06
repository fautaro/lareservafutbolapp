import { reactive, computed } from 'vue'

const state = reactive({ pending: 0 })

export function startLoader() {
  state.pending++
}

export function stopLoader() {
  state.pending = Math.max(0, state.pending - 1)
}

export function setLoader(active) {
  state.pending = active ? 1 : 0
}

export function useGlobalLoader() {
  return {
    loading: computed(() => state.pending > 0),
    pending: computed(() => state.pending),
  }
}
