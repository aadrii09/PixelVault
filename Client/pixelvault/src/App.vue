<script setup>
import Navbar from './components/Navbar.vue'
import Footer from './components/Footer.vue'
import CookieConsent from './components/CookieConsent.vue'
import Notification from './components/Notification.vue'
import { useUserStore } from './store/user';
import { useRoute } from 'vue-router';
import { ref, provide } from 'vue';

const userStore = useUserStore();
const route = useRoute();
userStore.cargarDesdeStorage();

// Sistema de notificaciones
const notification = ref({
  show: false,
  message: '',
  type: 'success',
  duration: 3000
});

const showNotification = (message, type = 'success', duration = 3000) => {
  notification.value = {
    show: true,
    message,
    type,
    duration
  };
};

const closeNotification = () => {
  notification.value.show = false;
};

// Proporcionar la función para uso global
provide('showNotification', showNotification);
</script>


<template>
  <div class="min-h-screen flex flex-col bg-transparent text-gray-900">    <Navbar v-if="!['/login', '/register', '/admin'].includes(route.path)" />
    <div class="flex-1">
      <router-view />
    </div>
    <Footer v-if="!['/login', '/register', '/admin', '/carrito', '/about', '/wishlist', '/plataforma/PlayStation', '/plataforma/Xbox', '/plataforma/Nintendo', '/plataforma/PC', ''].includes(route.path)" />
    <!-- El componente de consentimiento de cookies se mostrará en todas las páginas -->
    
    <!-- Sistema de notificaciones -->
    <Notification 
      v-if="notification.show" 
      :message="notification.message"
      :type="notification.type"
      :duration="notification.duration"
      :show="notification.show"
      @close="closeNotification"
    />    <CookieConsent />
  </div>
</template>

<style scoped>

</style>
