<script setup>
import { ref, onMounted, watch, computed } from 'vue';
import { useRoute } from 'vue-router';
import { getProductos } from '../api/productos';
import ProductCard from '../components/ProductCard.vue';
import Footer from '../components/Footer.vue';

const route = useRoute();
const plataforma = ref(route.params.plataforma);
const productos = ref([]); // Store all products initially
const cargando = ref(true);
const searchQuery = ref('');
const showFilterMenu = ref(false);
const selectedFilterType = ref(null); // 'videojuego', 'periferico', 'consola', or null

const cargarProductos = async () => {
  cargando.value = true;
  try {
    // Fetch all products or filter by platform here if API supports it
    // For now, let's assume getProductos fetches all and we filter locally
    productos.value = await getProductos();
  } catch (error) {
    console.error('Error al cargar productos:', error);
  } finally {
    cargando.value = false;
  }
};

const filteredProducts = computed(() => {
  let result = productos.value.filter(producto => {
    // Filter by platform (existing logic)
    const nombre = producto.nombre.toLowerCase();
    const descripcion = producto.descripcion.toLowerCase();
    return (
      nombre.includes(plataforma.value.toLowerCase()) ||
      descripcion.includes(plataforma.value.toLowerCase())
    );
  });

  // Filter by search query
  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase();
    result = result.filter(producto =>
      producto.nombre.toLowerCase().includes(query) ||
      producto.descripcion.toLowerCase().includes(query)
    );
  }

  // Filter by type (new logic)
  if (selectedFilterType.value) {
    result = result.filter(producto => producto.tipo === selectedFilterType.value);
  }

  return result;
});

const toggleFilterMenu = () => {
  showFilterMenu.value = !showFilterMenu.value;
};

const applyFilter = (type) => {
  selectedFilterType.value = type;
  showFilterMenu.value = false; // Close menu after selecting
};

const resetFilter = () => {
  selectedFilterType.value = null;
  showFilterMenu.value = false;
};

onMounted(cargarProductos);

// If route params change, reload all products and apply new platform filter
watch(() => route.params.plataforma, (newVal) => {
  plataforma.value = newVal;
  // Reset other filters when platform changes
  searchQuery.value = '';
  selectedFilterType.value = null;
  cargarProductos();
});
</script>

<template>
  <div class="container mx-auto px-4 py-6 bg-gradient-to-r from-[#16213e] to-[#1a1a2e] min-h-screen text-white">
    <h1 class="text-3xl font-bold mb-6 text-center">
      Productos de {{ plataforma }}
    </h1>
    
    <!-- Search and Filter Section -->
    <div class="flex justify-between items-center mb-6">
      <div class="relative flex-1 mr-4">
        <input
          type="text"
          placeholder="Buscar en PixelVault..."
          class="w-full p-3 pl-10 rounded-full bg-gray-700 text-white placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500"
          v-model="searchQuery"
        />
        <svg class="absolute left-3 top-1/2 transform -translate-y-1/2 w-5 h-5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path></svg>
      </div>
      <div class="relative">
        <button @click="toggleFilterMenu" class="p-3 rounded-full bg-gray-700 hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-blue-500">
          <svg class="w-6 h-6 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16"></path></svg>
        </button>
        
        <!-- Filter Dropdown -->
        <div v-if="showFilterMenu" class="absolute right-0 mt-2 w-48 bg-gray-700 rounded-md shadow-lg py-1 z-50">
          <button @click="applyFilter('videojuego')" class="block px-4 py-2 text-sm text-white hover:bg-gray-600 w-full text-left">Videojuego</button>
          <button @click="applyFilter('periferico')" class="block px-4 py-2 text-sm text-white hover:bg-gray-600 w-full text-left">Periferico</button>
          <button @click="applyFilter('consola')" class="block px-4 py-2 text-sm text-white hover:bg-gray-600 w-full text-left">Consola</button>
          <button @click="resetFilter" class="block px-4 py-2 text-sm text-gray-400 hover:bg-gray-600 w-full text-left">Mostrar todo</button>
        </div>
      </div>
    </div>

    <div v-if="cargando" class="text-gray-300 text-center">
      Cargando productos...
    </div>

    <div v-else>
      <div v-if="filteredProducts.length === 0" class="text-gray-300 text-center">
        No se encontraron productos que coincidan con los filtros.
      </div>

      <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        <ProductCard
          v-for="producto in filteredProducts"
          :key="producto.idProducto"
          :producto="producto"
        />
      </div>
    </div>
  </div>
  <Footer />
</template>

<style scoped>
/* Add any specific styles here if needed */
</style>