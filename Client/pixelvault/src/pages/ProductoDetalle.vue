<script setup>
import { onMounted, ref } from 'vue';
import { useRoute } from 'vue-router';
import axios from 'axios';

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
  <div class="p-8 text-black">
    <div v-if="cargando">Cargando...</div>
    <div v-else-if="error">{{ error }}</div>
    <div v-else>
      <h1 class="text-3xl font-bold mb-4">{{ producto.nombre }}</h1>
      <img :src="producto.imagenUrl" :alt="producto.nombre" class="mb-4 w-full max-w-md rounded-xl" />
      <p class="mb-2 text-gray-600">{{ producto.descripcion }}</p>
      <p><strong>Stock:</strong> {{ producto.stock }}</p>
      <p><strong>Fecha de lanzamiento:</strong> {{ producto.fechaLanzamiento }}</p>
      <p><strong>Precio:</strong> ${{ producto.precio?.toFixed(2) ?? 'N/A' }}</p>
    </div>
  </div>
</template>