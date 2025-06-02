<script setup>
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import axios from 'axios';
import Footer from '../components/Footer.vue';

const route = useRoute();
const producto = ref(null);
const cargando = ref(true);
const error = ref('');

onMounted(async () => {
  try {
    const id = route.params.id;
    const res = await axios.get(`http://localhost:5225/api/Productos/${id}`);
    producto.value = res.data;
  } catch (err) {
    error.value = 'No se pudo cargar el producto';
    console.error(err);
  } finally {
    cargando.value = false;
  }
});
</script>

<template>
  <div class="container mx-auto p-8 text-white bg-gradient-to-r from-[#16213e] to-[#1a1a2e] min-h-screen">
    <div v-if="cargando">Cargando...</div>
    <div v-else-if="error">{{ error }}</div>
    <div v-else class="flex flex-col lg:flex-row gap-8">
      <!-- Product Image and Carousel -->
      <div class="flex-1 flex flex-col items-center">
        <img :src="producto.imagenUrl" :alt="producto.nombre" class="mb-4 w-full max-w-sm rounded-xl shadow-lg" />
        <!-- Image Carousel with Arrows -->
        <div class="relative flex items-center w-full max-w-md">
          <button class="absolute left-0 z-10 p-2 ml-2 text-white bg-black bg-opacity-50 rounded-full hover:bg-opacity-75"><</button>
          <div class="flex gap-4 mt-2 overflow-x-auto justify-center w-full">
            <div class="w-16 h-16 bg-gray-300 rounded-md flex-shrink-0"></div>
            <div class="w-16 h-16 bg-gray-300 rounded-md flex-shrink-0"></div>
            <div class="w-16 h-16 bg-gray-300 rounded-md flex-shrink-0"></div>
            <div class="w-16 h-16 bg-gray-300 rounded-md flex-shrink-0"></div>
          </div>
           <button class="absolute right-0 z-10 p-2 mr-2 text-white bg-black bg-opacity-50 rounded-full hover:bg-opacity-75">></button>
        </div>
      </div>

      <!-- Product Details -->
      <div class="flex-1 text-white">
        <h1 class="text-4xl font-bold mb-4">{{ producto.nombre }}</h1>
        <p class="text-green-400 text-2xl font-semibold mb-4">{{ producto.precio?.toFixed(2) ?? 'N/A' }}€</p>
        <p class="mb-4 text-gray-300">{{ producto.descripcion }}</p>
        <p class="mb-2"><strong>Stock:</strong> {{ producto.stock }}</p>
        <p class="mb-4"><strong>Fecha de lanzamiento:</strong> {{ producto.fechaLanzamiento }}</p>
        <button class="bg-[#44475a] text-white py-2 px-6 rounded hover:bg-[#6272a4]">Comprar</button>
      </div>
    </div>

    <!-- Comments Section -->
    <div class="mt-12 bg-[#44475a] p-6 rounded-lg shadow-lg">
      <h2 class="text-2xl font-bold mb-4">Déjanos tu comentario de lo que te ha parecido!</h2>
      <textarea
        class="w-full p-3 mb-4 rounded-md bg-[#f8f8f2] text-gray-900 placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-blue-500"
        rows="4"
        placeholder="Escriba su comentario..."
      ></textarea>
      <div class="flex justify-end">
        <button class="bg-[#bd93f9] text-black py-2 px-6 rounded hover:bg-[#ff79c6]">Enviar</button>
      </div>
      <!-- Placeholder for existing comments -->
      <div class="mt-6 p-4 bg-[#6272a4] rounded-md text-white">
        <div class="font-semibold mb-2">👤 User</div>
        <p class="text-gray-200">Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
      </div>
    </div>
  </div>
  <Footer />
</template>

<style scoped>
/* Add any specific styles here if needed */
</style>