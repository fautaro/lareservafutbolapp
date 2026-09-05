import { ref } from 'vue'

const currentRole = ref(localStorage.getItem('userRole') || 'user') // 'user', 'owner', or 'admin'

export function useRole() {
  const isOwner = () => currentRole.value === 'owner'
  const isUser = () => currentRole.value === 'user'
  const isAdmin = () => currentRole.value === 'admin'

  const toggleRole = () => {
    currentRole.value = currentRole.value === 'user' ? 'owner' : 'user'
    localStorage.setItem('userRole', currentRole.value)
  }

  const setRole = (role) => {
    currentRole.value = role
    localStorage.setItem('userRole', role)
  }

  return {
    currentRole,
    isOwner,
    isUser,
    isAdmin,
    toggleRole,
    setRole
  }
}
