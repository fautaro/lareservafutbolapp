import { createRouter, createWebHashHistory } from 'vue-router';
import { useAuth0 } from '@auth0/auth0-vue';
import { auth0Config } from '../config/auth0Config';
import DefaultLayout from '../layouts/DefaultLayout.vue';
import Home from '../views/Home.vue';
import MisReservas from '../views/misReservas.vue';
import nuevaReserva from '../views/nuevaReserva.vue';
import Menu from '../views/Menu.vue';
import UserProfile from '../views/user/UserProfile.vue';
import ConfigUser from '../views/user/ConfigUser.vue';
import Help from '../views/user/Help.vue';
import Login from '../views/Login.vue';
import OwnerAgenda from '../views/owner/OwnerAgenda.vue';

import OwnerComplejoCreate from '../views/owner/OwnerComplejoCreate.vue';
import OwnerComplejoConfig from '../views/owner/OwnerComplejoConfig.vue';
import OwnerEstadisticas from '../views/owner/OwnerEstadisticas.vue';

const routes = [
  {
    path: '/login',
    name: 'Login',
    component: Login,
    meta: { requiresAuth: false }
  },
  {
    path: '/',
    component: DefaultLayout,
    meta: { requiresAuth: true },
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
      },
      {
        path: 'owner/agenda/:id',
        name: 'OwnerAgenda',
        component: OwnerAgenda
      },
      {
        path: 'owner/complejos/nuevo',
        name: 'OwnerComplejoCreate',
        component: OwnerComplejoCreate
      },
      {
        path: 'owner/complejo/:id/config',
        name: 'OwnerComplejoConfig',
        component: OwnerComplejoConfig
      },
      {
        path: 'owner/estadisticas',
        name: 'OwnerEstadisticas',
        component: OwnerEstadisticas
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

router.beforeEach(async (to, from, next) => {
  const { isAuthenticated, isLoading } = useAuth0();

  if (isLoading.value) {
    setTimeout(() => {
      router.beforeEach(async (to, from, next) => {
        next();
      });
    }, 100);
    return next();
  }

  if (to.meta.requiresAuth) {
    if (auth0Config.loginDisabled) {
      return next();
    }
    if (!isAuthenticated.value) {
      return next('/login');
    }
  }

  if (to.path === '/login' && (auth0Config.loginDisabled || isAuthenticated.value)) {
    return next('/');
  }

  next();
});

export default router;
