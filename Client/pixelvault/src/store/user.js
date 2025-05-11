import {defineStore} from 'pinia';
import Login from '../pages/Login.vue';

export const useUserStore = defineStore('user', {
    state: () => ({
        user: null,
        token: null,
    }),
    actions: {
        login(userData, token) {
            this.user = userData;
            this.token = token;
            localStorage.setItem('user', JSON.stringify(userData));
            localStorage.setItem('token', token);
        }
    },
    logout() {
        this.user = null;
        this.token = null;
        localStorage.removeItem('user');
        localStorage.removeItem('token');
    },
    cargarDesdeStorage() { 
        const user = localStorage.getItem('user');
        const token = localStorage.getItem('token');
        
        if (user && token) {
            this.user = JSON.parse(user);
            this.token = token;
        }
    } 
});