<script setup>
import Navbar from './components/Navbar.vue'
import Footer from './components/Footer.vue'
import { useUserStore } from './store/user';
import { useRoute } from 'vue-router';

const userStore = useUserStore();
const route = useRoute();
userStore.cargarDesdeStorage();
</script>


<template>
  <div class="min-h-screen flex flex-col bg-transparent text-gray-900">
    <Navbar v-if="!['/login', '/register', '/admin'].includes(route.path)" />
    <div class="flex-1">
      <router-view />
    </div>
    <Footer v-if="route.path === '/'" />
  </div>
</template>

<style scoped>

</style>
