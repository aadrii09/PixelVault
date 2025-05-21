import {createRouter, createWebHistory} from 'vue-router'

import Home from '../pages/Home.vue'
import Login from '../pages/Login.vue'
import Register from '../pages/Register.vue'
import {useUserStore} from '../store/user'
import pinia from '../pinia'
import Carrito from '../pages/Carrito.vue'
import AdminPanel from '../pages/AdminPanel.vue'
import SobreNosotros from '../pages/SobreNosotros.vue'


const routes = [
    {path: '/', name: 'Home', component: Home},
    {path: '/login', name: 'Login', component: Login},
    {path: '/register', name: 'Register', component: Register},
    {path: '/carrito', name: 'Carrito', component: Carrito, meta: {requiresAuth: true}},
    {path: '/admin', name: 'AdminPanel', component: AdminPanel, meta: {requiresAdmin: true}},
    {path: '/about', name: 'SobreNosotros', component: SobreNosotros},
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach((to, from, next) => {
    const userStore = useUserStore(pinia);
    if(to.meta.requiresAuth && !userStore.token) {
        next({name: 'Login'});
    } else if (to.meta.requiresAdmin && (!userStore.token || !userStore.user?.esAdmin)) {
        next({name: 'Home'});
    } else {
        next();
    }
});

export default router;