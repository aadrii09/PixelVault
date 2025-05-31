<template>
  <section class="w-full px-6 py-4 flex flex-col md:flex-row justify-between items-center">
    <!-- Barra de búsqueda -->
    <div class="relative w-full max-w-md mb-4 md:mb-0">
      <div class="absolute inset-y-0 left-3 flex items-center pointer-events-none">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-gray-500" viewBox="0 0 20 20" fill="currentColor">
          <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
        </svg>
      </div>
      <input 
        v-model="searchQuery"
        @keyup.enter="handleSearch"
        type="text" 
        placeholder="Buscar en PixelVault o escribir una URL" 
        class="w-full pl-10 pr-4 py-2 rounded-full bg-[#d9d9d9] dark:bg-gray-700 dark:text-white text-gray-800 focus:outline-none"
      />
    </div>
    
    <!-- Botón de filtros -->
    <div class="relative">
      <button 
        @click="toggleFilters" 
        class="ml-4 px-6 py-2 bg-primary-light dark:bg-gray-700 text-white rounded-md flex items-center hover:bg-opacity-90 transition-colors"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 4a1 1 0 011-1h16a1 1 0 011 1v2.586a1 1 0 01-.293.707l-6.414 6.414a1 1 0 00-.293.707V17l-4 4v-6.586a1 1 0 00-.293-.707L3.293 7.293A1 1 0 013 6.586V4z" />
        </svg>
        Filtrar
        <span v-if="activeFilterCount > 0" class="ml-2 bg-accent text-white text-xs font-bold rounded-full h-5 w-5 flex items-center justify-center">
          {{ activeFilterCount }}
        </span>
      </button>
      
      <!-- Panel de filtros -->
      <Transition name="slide-fade">
        <div v-if="showFilters" class="absolute right-0 mt-2 w-72 bg-white dark:bg-gray-800 rounded-lg shadow-lg p-4 z-10 max-h-[80vh] overflow-y-auto">
          <!-- Encabezado -->
          <div class="flex justify-between items-center mb-3">
            <h3 class="font-bold text-gray-800 dark:text-white text-lg">Filtros</h3>
            <button @click="showFilters = false" class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
          
          <!-- Filtro de Precio -->
          <div class="mb-6">
            <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">Rango de Precio</label>
            <div class="flex items-center space-x-2">
              <div class="flex-1">
                <label class="block text-xs text-gray-500 dark:text-gray-400 mb-1">Mínimo</label>
                <div class="relative">
                  <span class="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-500">€</span>
                  <input 
                    v-model.number="filters.priceMin" 
                    type="number" 
                    min="0" 
                    class="w-full pl-8 pr-3 py-2 border rounded-md dark:bg-gray-700 dark:border-gray-600 dark:text-white"
                    placeholder="Mín"
                  />
                </div>
              </div>
              <div class="flex-1">
                <label class="block text-xs text-gray-500 dark:text-gray-400 mb-1">Máximo</label>
                <div class="relative">
                  <span class="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-500">€</span>
                  <input 
                    v-model.number="filters.priceMax" 
                    type="number" 
                    min="0" 
                    class="w-full pl-8 pr-3 py-2 border rounded-md dark:bg-gray-700 dark:border-gray-600 dark:text-white"
                    placeholder="Máx"
                  />
                </div>
              </div>
            </div>
          </div>
          
          <!-- Filtro de Marcas -->
          <div class="mb-6" v-if="marcas.length > 0">
            <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">Marcas</label>
            <div class="space-y-2 max-h-48 overflow-y-auto p-1">
              <div v-for="marca in marcas" :key="marca.idMarca" class="flex items-center">
                <input 
                  :id="`marca-${marca.idMarca}`"
                  v-model="filters.marcas"
                  type="checkbox" 
                  :value="marca.idMarca"
                  class="h-4 w-4 text-accent rounded border-gray-300 focus:ring-accent"
                />
                <label :for="`marca-${marca.idMarca}`" class="ml-3 text-sm text-gray-700 dark:text-gray-300 flex items-center">
                  <img v-if="marca.logoUrl" :src="marca.logoUrl" :alt="marca.nombre" class="h-5 w-5 mr-2 object-contain" />
                  {{ marca.nombre }}
                </label>
              </div>
            </div>
          </div>
          
          <!-- Filtro de Tipos de Producto -->
          <div class="mb-6" v-if="tiposProducto.length > 0">
            <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">Categorías</label>
            <div class="space-y-2">
              <div v-for="tipo in tiposProducto" :key="tipo.idTipo" class="flex items-center">
                <input 
                  :id="`tipo-${tipo.idTipo}`"
                  v-model="filters.tipos"
                  type="checkbox" 
                  :value="tipo.idTipo"
                  class="h-4 w-4 text-accent rounded border-gray-300 focus:ring-accent"
                />
                <label :for="`tipo-${tipo.idTipo}`" class="ml-3 text-sm text-gray-700 dark:text-gray-300">
                  {{ tipo.nombre }}
                </label>
              </div>
            </div>
          </div>
          
          <!-- Botones de acción -->
          <div class="flex justify-between pt-4 border-t border-gray-200 dark:border-gray-700">
            <button 
              @click="resetFilters" 
              class="px-4 py-2 text-sm font-medium text-gray-700 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white"
              :disabled="!hasActiveFilters"
            >
              Limpiar
            </button>
            <button 
              @click="applyFilters" 
              class="px-4 py-2 text-sm font-medium text-white bg-accent hover:bg-accent-dark rounded-md transition-colors disabled:opacity-50"
              :disabled="!hasActiveFilters"
            >
              Aplicar Filtros
            </button>
          </div>
        </div>
      </Transition>
    </div>
  </section>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { getMarcas, getTiposProducto } from '../api/productos';

// Interfaces
interface Marca {
  idMarca: number;
  nombre: string;
  logoUrl: string;
  website?: string;
}

interface TipoProducto {
  idTipo: number;
  nombre: string;
  descripcion?: string;
}

interface Filters {
  priceMin?: number | null;
  priceMax?: number | null;
  marcas: number[];
  tipos: number[];
}

// Props y Emits
const props = defineProps({
  initialFilters: {
    type: Object as () => Partial<Filters>,
    default: () => ({})
  },
  isLoading: {
    type: Boolean,
    default: false
  }
});

const emit = defineEmits<{
  (e: 'search', query: string): void;
  (e: 'filter', filters: Partial<Filters>): void;
  (e: 'update:modelValue', value: string): void;
}>();

// Estado reactivo
const searchQuery = ref('');
const showFilters = ref(false);
const marcas = ref<Marca[]>([]);
const tiposProducto = ref<TipoProducto[]>([]);
const isLoading = ref(false);

// Filtros
const filters = ref<Filters>({
  priceMin: null,
  priceMax: null,
  marcas: [],
  tipos: []
});

// Computed properties
const activeFilterCount = computed(() => {
  let count = 0;
  if (filters.value.priceMin !== null && filters.value.priceMin > 0) count++;
  if (filters.value.priceMax !== null && filters.value.priceMax > 0) count++;
  count += filters.value.marcas.length;
  count += filters.value.tipos.length;
  return count;
});

const hasActiveFilters = computed(() => {
  return (
    (filters.value.priceMin !== null && filters.value.priceMin > 0) ||
    (filters.value.priceMax !== null && filters.value.priceMax > 0) ||
    filters.value.marcas.length > 0 ||
    filters.value.tipos.length > 0
  );
});

// Métodos
const loadMarcas = async () => {
  try {
    isLoading.value = true;
    const data = await getMarcas();
    marcas.value = data;
  } catch (error) {
    console.error('Error al cargar las marcas:', error);
  } finally {
    isLoading.value = false;
  }
};

const loadTiposProducto = async () => {
  try {
    isLoading.value = true;
    const data = await getTiposProducto();
    tiposProducto.value = data;
  } catch (error) {
    console.error('Error al cargar los tipos de producto:', error);
  } finally {
    isLoading.value = false;
  }
};

const toggleFilters = () => {
  showFilters.value = !showFilters.value;
  
  // Cargar datos si es la primera vez que se abre
  if (showFilters.value && marcas.value.length === 0) {
    loadMarcas();
  }
  
  if (showFilters.value && tiposProducto.value.length === 0) {
    loadTiposProducto();
  }
};

const handleSearch = () => {
  emit('search', searchQuery.value);
};

const applyFilters = () => {
  // Crear un objeto con solo los filtros activos
  const activeFilters: Partial<Filters> = {};
  
  if (filters.value.priceMin !== null && filters.value.priceMin > 0) {
    activeFilters.priceMin = filters.value.priceMin;
  }
  
  if (filters.value.priceMax !== null && filters.value.priceMax > 0) {
    activeFilters.priceMax = filters.value.priceMax;
  }
  
  if (filters.value.marcas.length > 0) {
    activeFilters.marcas = [...filters.value.marcas];
  }
  
  if (filters.value.tipos.length > 0) {
    activeFilters.tipos = [...filters.value.tipos];
  }
  
  emit('filter', activeFilters);
  showFilters.value = false;
};

const resetFilters = () => {
  filters.value = {
    priceMin: null,
    priceMax: null,
    marcas: [],
    tipos: []
  };
  
  // Emitir un objeto vacío para indicar que se quieren quitar todos los filtros
  emit('filter', {});
};

// Inicialización
onMounted(() => {
  // Inicializar con los filtros proporcionados
  if (props.initialFilters) {
    if (props.initialFilters.priceMin !== undefined) {
      filters.value.priceMin = props.initialFilters.priceMin;
    }
    if (props.initialFilters.priceMax !== undefined) {
      filters.value.priceMax = props.initialFilters.priceMax;
    }
    if (props.initialFilters.marcas) {
      filters.value.marcas = [...props.initialFilters.marcas];
    }
    if (props.initialFilters.tipos) {
      filters.value.tipos = [...props.initialFilters.tipos];
    }
  }
});
</script>

<style scoped>
/* Transición para el panel de filtros */
.slide-fade-enter-active {
  transition: all 0.2s ease-out;
}

.slide-fade-leave-active {
  transition: all 0.2s cubic-bezier(1, 0.5, 0.8, 1);
}

.slide-fade-enter-from,
.slide-fade-leave-to {
  transform: translateY(-10px);
  opacity: 0;
}

/* Estilos para la barra de desplazamiento */
::-webkit-scrollbar {
  width: 6px;
  height: 6px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 3px;
}

::-webkit-scrollbar-thumb {
  background: #888;
  border-radius: 3px;
}

::-webkit-scrollbar-thumb:hover {
  background: #555;
}

/* Estilos para modo oscuro */
.dark ::-webkit-scrollbar-track {
  background: #374151;
}

.dark ::-webkit-scrollbar-thumb {
  background: #6b7280;
}

.dark ::-webkit-scrollbar-thumb:hover {
  background: #9ca3af;
}
</style>