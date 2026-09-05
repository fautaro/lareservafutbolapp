<template>
  <component :is="activeHome" />
</template>

<script>
import UserHome from './home/UserHome.vue'
import OwnerHome from './owner/OwnerHome.vue'
import AdminUsers from './admin/AdminUsers.vue'
import { useRole } from '../composables/useRole'
import { computed } from 'vue'

export default {
  name: 'HomeWrapper',
  components: {
    UserHome,
    OwnerHome,
    AdminUsers
  },
  setup() {
    const { currentRole } = useRole()
    
    const activeHome = computed(() => {
      if (currentRole.value === 'admin') return 'AdminUsers'
      if (currentRole.value === 'owner') return 'OwnerHome'
      return 'UserHome'
    })

    return {
      activeHome
    }
  }
}
</script>
