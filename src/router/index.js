import { createRouter, createWebHashHistory } from 'vue-router';
import { useAuth0 } from '@auth0/auth0-vue';
import { auth0Config } from '../config/auth0Config';
import DefaultLayout from '../layouts/DefaultLayout.vue';
import Home from '../views/Home.vue';
import MisReservas from '../views/misReservas.vue';
import nuevaReserva from '../views/nuevaReserva.vue';
import Menu from '../views/Menu.vue';
import UserProfile from '../views/User/UserProfile.Vue';
import ConfigUser from '../views/user/ConfigUser.vue';
import Help from '../views/User/Help.Vue';
import Login from '../views/Login.vue';

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
