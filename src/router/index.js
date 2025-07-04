import { createRouter, createWebHashHistory } from 'vue-router';
import DefaultLayout from '../layouts/DefaultLayout.vue';
import Home from '../views/Home.vue';
import MisReservas from '../views/misReservas.vue';
import nuevaReserva from '../views/nuevaReserva.vue';
import Menu from '../views/Menu.vue';

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
      }
    ]
  }
];

const router = createRouter({
  history: createWebHashHistory(),
  routes
});

export default router;
