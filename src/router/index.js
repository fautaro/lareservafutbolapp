import { createRouter, createWebHashHistory } from 'vue-router';
import DefaultLayout from '../layouts/DefaultLayout.vue';
import Home from '../views/Home.vue';
import MisReservas from '../views/misReservas.vue';
import nuevaReserva from '../views/nuevaReserva.vue';
import Menu from '../views/Menu.vue';
import UserProfile from '../views/User/UserProfile.Vue';
import ConfigUser from '../views/User/ConfigUser.vue';
import Help from '../views/User/Help.Vue';

const routes = [
  {
    path: '/',
    component: DefaultLayout,
    children: [
      {
        path: '',
        name: 'Home',
        component: Home
      },
      {
        path: 'reservas',
        name: 'MisReservas',
        component: MisReservas
      },
      {
        path: 'menu',
        name: 'Menu',
        component: Menu
      },
      {
        path: 'reservar',
        name: 'NuevaReserva',
        component: nuevaReserva
      },
      {
        path: 'perfil',
        name: 'UserProfile',
        component: UserProfile
      },
      {
        path: 'configuracion',
        name: 'ConfigUser',
        component: ConfigUser
      },
            {
        path: 'help',
        name: 'Help',
        component: Help
      }
    ]
  }
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
  scrollBehavior() {
    return { top: 0 }
  }
});

export default router;
