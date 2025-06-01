<script setup>
import { ref, onMounted, watch } from 'vue';
import { useRoute } from 'vue-router';
import { getProductos } from '../api/productos';
import ProductCard from '../components/ProductCard.vue';

const route = useRoute();
const plataforma = ref(route.params.plataforma);
const productosFiltrados = ref([]);
const cargando = ref(true);

const cargarProductos = async () => {
  cargando.value = true;
  try {
    const productos = await getProductos();
    productosFiltrados.value = productos.filter(producto => {
      const nombre = producto.nombre.toLowerCase();
      const descripcion = producto.descripcion.toLowerCase();
      return (
        nombre.includes(plataforma.value.toLowerCase()) ||
        descripcion.includes(plataforma.value.toLowerCase())
      );
    });
  } catch (error) {
    console.error('Error al cargar productos:', error);
  } finally {
    cargando.value = false;
  }
};

onMounted(cargarProductos);

// Si cambian los params de la ruta (por ejemplo, con navegación interna), recargar
watch(() => route.params.plataforma, (newVal) => {
  plataforma.value = newVal;
  cargarProductos();
});
</script>

<template>
  <div class="max-w-6xl mx-auto px-4 py-6">
    <h1 class="text-3xl font-bold mb-6 text-center text-white">
      Productos de {{ plataforma }}
    </h1>
    
    <div v-if="cargando" class="text-gray-300 text-center">
      Cargando productos...
    </div>

    <div v-else>
      <div v-if="productosFiltrados.length === 0" class="text-gray-300 text-center">
        No se encontraron productos relacionados con "{{ plataforma }}".
      </div>

      <div v-else class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        <ProductCard
          v-for="producto in productosFiltrados"
          :key="producto.idProducto"
          :producto="producto"
        />
      </div>
    </div>
  </div>
</template>