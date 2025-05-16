<script setup>
import Navbar from './components/Navbar.vue'
import { useUserStore } from './store/user';

const userStore = useUserStore();
userStore.cargarDesdeStorage();
</script>


<template>
  <div class="min-h-screen bg-transparent text-gray-900">
    <Navbar />
    <router-view />
  </div>
</template>

<style scoped>

</style>
