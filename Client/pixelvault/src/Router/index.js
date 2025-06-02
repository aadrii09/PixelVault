import { createRouter, createWebHistory } from 'vue-router'

import pinia from '../pinia'
import { useUserStore } from '../store/user'

import Home from '../pages/Home.vue'
import Login from '../pages/Login.vue'
import Register from '../pages/Register.vue'
import Carrito from '../pages/Carrito.vue'
import AdminPanel from '../pages/AdminPanel.vue'
import SobreNosotros from '../pages/SobreNosotros.vue'
import ProductoDetalle from '../pages/ProductoDetalle.vue'
import ProductosPlataforma from '../pages/ProductosPlataforma.vue';
import Wishlist from '../pages/WishList.vue';

const routes = [
    { path: '/', name: 'Home', component: Home },
    { path: '/login', name: 'Login', component: Login },
    { path: '/register', name: 'Register', component: Register },
    { path: '/carrito', name: 'Carrito', component: Carrito, meta: { requiresAuth: true } },
    { path: '/admin', name: 'AdminPanel', component: AdminPanel, meta: { requiresAdmin: true } },
    { path: '/about', name: 'SobreNosotros', component: SobreNosotros },
    { path: '/producto/:id', name: 'ProductoDetalle', component: ProductoDetalle },
    { path: '/plataforma/:plataforma', name: 'ProductosPlataforma', component: ProductosPlataforma },
    { path: '/wishlist', name: 'Wishlist', component: Wishlist },

]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach((to, from, next) => {
    const userStore = useUserStore(pinia);
    if (to.meta.requiresAuth && !userStore.token) {
        next({ name: 'Login' });
    } else if (to.meta.requiresAdmin && (!userStore.token || !userStore.user?.esAdmin)) {
        next({ name: 'Home' });
    } else {
        next();
    }
});

export default router;