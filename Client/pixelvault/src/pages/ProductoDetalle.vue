<script setup>
import { onMounted, ref, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import axios from 'axios';
import Footer from '../components/Footer.vue';
import { agregarAlCarrito } from '../api/carrito';
import { useUserStore } from '../store/user';

const route = useRoute();
const router = useRouter();
const userStore = useUserStore();
const producto = ref(null);
const cargando = ref(true);
const error = ref('');

const formattedFechaLanzamiento = computed(() => {
  if (producto.value && producto.value.fechaLanzamiento) {
    const date = new Date(producto.value.fechaLanzamiento);
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
  return 'N/A';
});

// Importar inject para acceder al sistema de notificaciones
import { inject } from 'vue';
const showNotification = inject('showNotification');

const verificarAutenticacion = () => {
    // Verificar si el usuario está autenticado
    if (!userStore.token) {
        // Si no está autenticado, mostrar notificación y redirigir a la página de inicio de sesión
        showNotification('Debes iniciar sesión para agregar productos al carrito', 'info');
        router.push('/login');
        return false;
    }
    return true;
};

const comprarAhora = async () => {
    if (!verificarAutenticacion()) return;
    
    if (!producto.value) return; // Ensure product data is available
    try {
        const productoFormateado = {
            idProducto: producto.value.idProducto,
            nombre: producto.value.nombre || "",
            imagenUrl: producto.value.imagenUrl || "",
            cantidad: 1,
            precioUnitario: producto.value.precio || 0
        };        // Primero agregamos al carrito
        await agregarAlCarrito(productoFormateado);
        showNotification(`¡${producto.value.nombre} agregado al carrito!`, 'success');
        // Luego redirigimos directamente al carrito para completar la compra
        router.push('/carrito');
    } catch (error) {
        showNotification('Error al procesar la compra', 'error');
        console.error('Error al procesar la compra:', error.response?.data || error);
    }
};

const agregar = async () => {
    if (!verificarAutenticacion()) return;
    
    if (!producto.value) return; // Ensure product data is available
    try {
        const productoFormateado = {
            idProducto: producto.value.idProducto,
            nombre: producto.value.nombre || "",
            imagenUrl: producto.value.imagenUrl || "",
            cantidad: 1,
            precioUnitario: producto.value.precio || 0
        };

        console.log("📦 Producto enviado al backend:", productoFormateado);        await agregarAlCarrito(productoFormateado);
        showNotification(`¡${producto.value.nombre} agregado al carrito!`, 'success');
    } catch (error) {
        showNotification('Error al agregar al carrito', 'error');
        console.error('Error al agregar al carrito:', error.response?.data || error);
    }
};

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
  <div class="min-h-screen bg-gradient-to-br from-black via-purple-900 to-blue-900 relative overflow-hidden">
    <!-- Fondo animado con efectos gaming -->
    <div class="absolute inset-0 opacity-10">
      <div class="absolute top-20 left-10 w-32 h-32 bg-gradient-to-r from-pink-500 to-purple-500 rounded-full blur-3xl animate-pulse"></div>
      <div class="absolute top-60 right-20 w-40 h-40 bg-gradient-to-r from-blue-500 to-cyan-500 rounded-full blur-3xl animate-pulse delay-1000"></div>
      <div class="absolute bottom-40 left-1/3 w-28 h-28 bg-gradient-to-r from-purple-500 to-pink-500 rounded-full blur-3xl animate-pulse delay-2000"></div>
    </div>

    <div class="container mx-auto p-8 text-white relative z-10">
      <div v-if="cargando" class="flex justify-center items-center h-64">
        <div class="animate-spin rounded-full h-16 w-16 border-t-4 border-pink-500"></div>
      </div>
      
      <div v-else-if="error" class="text-center text-red-400 bg-red-900/20 p-6 rounded-xl border border-red-500/30">
        {{ error }}
      </div>
      
      <div v-else class="flex flex-col lg:flex-row gap-12">
        <!-- Product Image and Carousel -->
        <div class="flex-1 flex flex-col items-center">
          <img :src="producto.imagenUrl" :alt="producto.nombre" class="mb-4 w-full max-w-md rounded-xl shadow-lg" />
          <!-- Image Carousel with Arrows -->
          <div class="relative flex items-center w-full max-w-lg">
            <button class="absolute left-0 z-10 p-2 ml-2 text-white bg-black bg-opacity-50 rounded-full hover:bg-opacity-75"><</button>
            <div class="flex gap-4 mt-2 overflow-x-auto justify-center w-full">
              <div class="w-24 h-24 bg-gray-300 rounded-md flex-shrink-0"></div>
              <div class="w-24 h-24 bg-gray-300 rounded-md flex-shrink-0"></div>
              <div class="w-24 h-24 bg-gray-300 rounded-md flex-shrink-0"></div>
              <div class="w-24 h-24 bg-gray-300 rounded-md flex-shrink-0"></div>
            </div>
             <button class="absolute right-0 z-10 p-2 mr-2 text-white bg-black bg-opacity-50 rounded-full hover:bg-opacity-75">></button>
          </div>
        </div>

        <!-- Product Details -->
        <div class="flex-1 text-white">
          <h1 class="text-5xl font-bold mb-6 bg-gradient-to-r from-pink-400 via-purple-400 to-blue-400 bg-clip-text text-transparent">
            {{ producto.nombre }}
          </h1>
          
          <div class="mb-6 p-4 bg-gradient-to-r from-green-500/20 to-emerald-500/20 rounded-xl border border-green-500/30">
            <p class="text-green-400 text-3xl font-bold">{{ producto.precio?.toFixed(2) ?? 'N/A' }}€</p>
          </div>
          
          <div class="mb-6 p-4 bg-white/10 backdrop-blur-lg rounded-xl border border-white/20">
            <p class="text-gray-200 leading-relaxed">{{ producto.descripcion }}</p>
          </div>
          
          <div class="grid grid-cols-2 gap-4 mb-6">
            <div class="p-4 bg-gradient-to-r from-purple-500/20 to-pink-500/20 rounded-xl border border-purple-500/30">
              <p class="text-purple-300 font-semibold">Stock</p>
              <p class="text-2xl font-bold">{{ producto.stock }}</p>
            </div>
            <div class="p-4 bg-gradient-to-r from-blue-500/20 to-cyan-500/20 rounded-xl border border-blue-500/30">
              <p class="text-blue-300 font-semibold">Lanzamiento</p>
              <p class="text-lg font-bold">{{ formattedFechaLanzamiento }}</p>
            </div>
          </div>
            <div class="flex space-x-4">
            <button @click="comprarAhora" class="w-full bg-gradient-to-r from-pink-500 via-purple-600 to-blue-500 hover:from-pink-600 hover:via-purple-700 hover:to-blue-600 text-white font-bold py-4 px-8 rounded-xl transition-all duration-300 shadow-lg hover:shadow-2xl hover:shadow-purple-500/50 transform hover:scale-105">
              <span class="flex items-center justify-center gap-2">
                <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4m0 0L7 13m0 0l-2.5 5M7 13l2.5 5m0 0h8.5"></path>
                </svg>
                COMPRAR AHORA
              </span>
            </button>
            <button @click="agregar" class="bg-green-500 text-white py-2 px-6 rounded hover:bg-green-600">Añadir al Carrito</button>
          </div>
        </div>
      </div>

      <!-- Comments Section -->
      <div class="mt-16">
        <div class="bg-gradient-to-r from-purple-900/50 to-blue-900/50 backdrop-blur-lg p-8 rounded-2xl shadow-2xl border border-purple-500/30">
          <h2 class="text-3xl font-bold mb-6 bg-gradient-to-r from-pink-400 to-purple-400 bg-clip-text text-transparent">
            ¡Déjanos tu comentario de lo que te ha parecido!
          </h2>
          
          <div class="relative">
            <textarea
              class="w-full p-4 mb-6 rounded-xl bg-black/30 backdrop-blur-lg text-white placeholder-gray-400 border border-purple-500/30 focus:border-pink-500 focus:outline-none focus:ring-2 focus:ring-pink-500/50 transition-all duration-300"
              rows="4"
              placeholder="Comparte tu experiencia gaming..."
            ></textarea>
            <div class="absolute top-2 right-2 text-xs text-gray-500">💭</div>
          </div>
          
          <div class="flex justify-end">
            <button class="bg-gradient-to-r from-pink-500 to-purple-600 hover:from-pink-600 hover:to-purple-700 text-white font-bold py-3 px-8 rounded-xl transition-all duration-300 shadow-lg hover:shadow-pink-500/50 transform hover:scale-105">
              <span class="flex items-center gap-2">
                <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 19l9 2-9-18-9 18 9-2zm0 0v-8"></path>
                </svg>
                Enviar
              </span>
            </button>
          </div>
          
          <!-- Existing Comments -->
          <div class="mt-8 space-y-4">
            <div class="p-6 bg-gradient-to-r from-gray-800/50 to-gray-900/50 backdrop-blur-lg rounded-xl border border-gray-700/50 hover:border-purple-500/50 transition-all duration-300">
              <div class="flex items-center gap-3 mb-4">
                <div class="w-10 h-10 bg-gradient-to-r from-pink-500 to-purple-500 rounded-full flex items-center justify-center text-white font-bold">
                  🎮
                </div>
                <div>
                  <div class="font-bold text-purple-300">GamerPro2024</div>
                  <div class="text-xs text-gray-500">Hace 2 horas</div>
                </div>
              </div>
              <p class="text-gray-200 leading-relaxed">
                ¡Increíble consola! Los gráficos son espectaculares y la velocidad de carga es impresionante. Definitivamente vale la pena la inversión para cualquier gamer serio. La compatibilidad con PS4 es un plus enorme.
              </p>
              <div class="flex items-center gap-4 mt-4">
                <button class="flex items-center gap-1 text-pink-400 hover:text-pink-300 transition-colors">
                  <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
                    <path d="M3.172 5.172a4 4 0 015.656 0L10 6.343l1.172-1.171a4 4 0 115.656 5.656L10 17.657l-6.828-6.829a4 4 0 010-5.656z"></path>
                  </svg>
                  <span class="text-sm">24</span>
                </button>
                <button class="text-gray-400 hover:text-gray-300 transition-colors text-sm">
                  Responder
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <Footer />
</template>

<style scoped>
@keyframes pulse {
  0%, 100% {
    transform: scale(1);
    opacity: 0.1;
  }
  50% {
    transform: scale(1.1);
    opacity: 0.2;
  }
}

.animate-pulse {
  animation: pulse 4s cubic-bezier(0.4, 0, 0.6, 1) infinite;
}

.delay-1000 {
  animation-delay: 1s;
}

.delay-2000 {
  animation-delay: 2s;
}

/* Scrollbar personalizado para el carousel */
.overflow-x-auto::-webkit-scrollbar {
  height: 4px;
}

.overflow-x-auto::-webkit-scrollbar-track {
  background: rgba(255, 255, 255, 0.1);
  border-radius: 2px;
}

.overflow-x-auto::-webkit-scrollbar-thumb {
  background: linear-gradient(to right, #ec4899, #8b5cf6);
  border-radius: 2px;
}

.overflow-x-auto::-webkit-scrollbar-thumb:hover {
  background: linear-gradient(to right, #db2777, #7c3aed);
}
</style>