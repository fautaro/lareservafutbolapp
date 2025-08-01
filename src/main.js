import { createApp } from 'vue'
import App from './App.vue'
import './style.css' 
import '@fortawesome/fontawesome-free/css/all.min.css'
import 'flowbite' 
import router from './router' 

createApp(App).use(router).mount('#app')
