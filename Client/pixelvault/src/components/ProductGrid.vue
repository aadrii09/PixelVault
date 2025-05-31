<template>
  <section class="w-full px-6 py-4">
    <div v-if="loading" class="flex justify-center items-center py-12">
      <div class="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-accent"></div>
    </div>
    <p v-else-if="filteredProducts.length === 0" class="text-center text-white dark:text-gray-300 py-8">
      No se encontraron productos que coincidan con tu búsqueda.
    </p>
    <div v-else class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      <ProductCardPage 
        v-for="product in filteredProducts" 
        :key="product.idProducto" 
        :product="mapProductToCard(product)"
        @add-to-cart="$emit('add-to-cart', mapProductToCart(product))"
        @view-details="$emit('view-details', mapProductToDetails(product))"
      />
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue';
import { useRoute } from 'vue-router';
import type { Ref } from 'vue';
import ProductCardPage from './ProductCardPage.vue';
import { getProductos, getProductosByMarca, getMarcas } from '../api/productos';

interface Marca {
  idMarca: number;
  nombre: string;
  logoUrl: string;
  website?: string;
}

interface ProductoFiltro {
  priceMin?: number;
  priceMax?: number;
  marcas?: number[];
  tipos?: number[];
}

interface Product {
  idProducto: number;
  nombre: string;
  descripcion: string;
  stock: number;
  fechaLanzamiento: string;
  imagenUrl: string;
  activo: boolean;
  destacado: boolean;
  idMarca: number;
  idTipo: number;
  precio: number;
  marca?: {
    idMarca: number;
    nombre: string;
    logoUrl: string;
  };
}

interface Filter {
  priceMin?: number;
  priceMax?: number;
  marcas?: number[];
  tipos?: number[];
}

const props = withDefaults(defineProps<{
  searchQuery?: string;
  activeFilters?: ProductoFiltro;
  marcaId?: number | null;
  tipoId?: number | null;
  destacados?: boolean;
}>(), {
  searchQuery: '',
  activeFilters: () => ({
    priceMin: undefined,
    priceMax: undefined,
    marcas: [],
    tipos: []
  }),
  marcaId: null,
  tipoId: null,
  destacados: false
});

const route = useRoute();
const allProducts: Ref<Product[]> = ref([]);
const marcas: Ref<Marca[]> = ref([]);
const loading = ref(true);

// Inicializar filtros con valores por defecto
const defaultFilters: ProductoFiltro = {
  priceMin: undefined,
  priceMax: undefined,
  marcas: [],
  tipos: []
};

defineEmits(['add-to-cart', 'view-details']);

// Mapear producto de la API al formato del card
const mapProductToCard = (product: Product) => ({
  id: product.idProducto,
  title: product.nombre,
  image: product.imagenUrl || 'https://via.placeholder.com/300',
  price: product.precio,
  category: product.marca?.nombre || 'Sin categoría'
});

// Mapear producto para el carrito
const mapProductToCart = (product: Product) => ({
  id: product.idProducto,
  nombre: product.nombre,
  precio: product.precio,
  imagenUrl: product.imagenUrl,
  cantidad: 1
});

// Mapear producto para los detalles
const mapProductToDetails = (product: Product) => ({
  id: product.idProducto,
  title: product.nombre,
  description: product.descripcion,
  price: product.precio,
  image: product.imagenUrl || 'https://via.placeholder.com/300',
  stock: product.stock,
  marca: product.marca?.nombre || 'Sin marca',
  fechaLanzamiento: new Date(product.fechaLanzamiento).toLocaleDateString()
});

// Cargar productos
const loadProducts = async () => {
  try {
    loading.value = true;
    
    if (props.marcaId) {
      allProducts.value = await getProductosByMarca(props.marcaId);
    } else if (props.tipoId) {
      // Implementar lógica para filtrar por tipo si es necesario
      const productos = await getProductos();
      allProducts.value = productos.filter(p => p.idTipo === props.tipoId);
    } else if (props.destacados) {
      const productos = await getProductos();
      allProducts.value = productos.filter(p => p.destacado);
    } else {
      allProducts.value = await getProductos();
    }
    
    // Cargar marcas si no están cargadas
    if (marcas.value.length === 0) {
      marcas.value = await getMarcas();
    }
    
    // Añadir información de la marca a cada producto
    allProducts.value = allProducts.value.map(product => {
      const marcaEncontrada = marcas.value.find((m: Marca) => m.idMarca === product.idMarca);
      return {
        ...product,
        marca: marcaEncontrada
      };
    });
    
  } catch (error) {
    console.error('Error al cargar productos:', error);
  } finally {
    loading.value = false;
  }
};

// Cargar productos al montar el componente
onMounted(() => {
  loadProducts();
});

// Observar cambios en los filtros
watch([() => props.marcaId, () => props.tipoId, () => props.destacados], () => {
  loadProducts();
});

// Filtrar productos según búsqueda y filtros
const filteredProducts = computed(() => {
  let result = [...allProducts.value];
  
  // Aplicar búsqueda
  if (props.searchQuery) {
    const query = props.searchQuery.toLowerCase();
    result = result.filter(product => 
      product.nombre.toLowerCase().includes(query) ||
      product.descripcion?.toLowerCase().includes(query)
    );
  }
  
  // Aplicar filtros de precio
  if (props.activeFilters.priceMin !== undefined || props.activeFilters.priceMax !== undefined) {
    const min = props.activeFilters.priceMin || 0;
    const max = props.activeFilters.priceMax || Infinity;
    result = result.filter(product => 
      product.precio >= min && product.precio <= max
    );
  }
  
  // Aplicar filtros de marcas
  if (props.activeFilters.marcas && props.activeFilters.marcas.length > 0) {
    result = result.filter(product => 
      props.activeFilters.marcas?.includes(product.idMarca)
    );
  }
  
  // Aplicar filtros de tipos
  if (props.activeFilters.tipos && props.activeFilters.tipos.length > 0) {
    result = result.filter(product => 
      props.activeFilters.tipos?.includes(product.idTipo)
    );
  }
  
  return result;
});

// Exponer método para recargar productos
defineExpose({
  reloadProducts: loadProducts
});
</script>