import { createApp } from 'vue'
import { createAuth0 } from '@auth0/auth0-vue'
import App from './App.vue'
import './style.css' 
import '@fortawesome/fontawesome-free/css/all.min.css'
import 'flowbite' 
import router from './router'
import { auth0Config, validateAuth0Config } from './config/auth0Config'

if (!validateAuth0Config()) {
  console.warn('Auth0 no está completamente configurado. Por favor, actualiza src/config/auth0Config.js')
}

const app = createApp(App)

app.use(
  createAuth0({
    domain: auth0Config.domain,
    clientId: auth0Config.clientId,
    authorizationParams: {
      redirect_uri: auth0Config.redirectUri
    }
  })
)

app.use(router)
app.mount('#app')
