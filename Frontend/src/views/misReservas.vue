<template>
  <component :is="activeReservas" />
</template>

<script>
import UserReservas from './user/UserReservas.vue'
import OwnerReservasAgenda from './owner/OwnerReservasAgenda.vue'
import { useRole } from '../composables/useRole'
import { computed } from 'vue'

export default {
  name: 'ReservasWrapper',
  components: {
    UserReservas,
    OwnerReservasAgenda
  },
  setup() {
    const { currentRole } = useRole()
    
    const activeReservas = computed(() => {
      return currentRole.value === 'owner' ? 'OwnerReservasAgenda' : 'UserReservas'
    })

    return {
      activeReservas
    }
  }
}
</script>
