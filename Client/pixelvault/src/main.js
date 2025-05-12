import { createApp } from 'vue'
import App from './App.vue'
import router from './Router'
import { createPinia } from 'pinia'
import pinia from './pinia'

import './assets/tailwind.css'

createApp(App)
.use(router)
.use(pinia)
.mount('#app')
