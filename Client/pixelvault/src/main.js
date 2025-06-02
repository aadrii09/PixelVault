import { createApp } from 'vue'
import App from './App.vue'
import router from './Router'
import { createPinia } from 'pinia'
import pinia from './pinia'
import piniaPluginPersistedState from 'pinia-plugin-persistedstate';

import './assets/tailwind.css'
pinia.use(piniaPluginPersistedState);

createApp(App)
    .use(router)
    .use(pinia)
    .mount('#app')
