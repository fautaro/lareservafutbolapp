<template>
  <component :is="activeHome" />
</template>

<script>
import UserHome from './home/UserHome.vue'
import OwnerHome from './owner/OwnerHome.vue'
import { useRole } from '../composables/useRole'
import { computed } from 'vue'

export default {
  name: 'HomeWrapper',
  components: {
    UserHome,
    OwnerHome
  },
  setup() {
    const { currentRole } = useRole()
    
    const activeHome = computed(() => {
      return currentRole.value === 'owner' ? 'OwnerHome' : 'UserHome'
    })

    return {
      activeHome
    }
  }
}
</script>
