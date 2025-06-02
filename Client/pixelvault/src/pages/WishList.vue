<script setup>
import { useWishlistStore } from '../store/wishlist';
import ProductCard from '../components/ProductCard.vue';
import Footer from '../components/Footer.vue';
const wishlistStore = useWishlistStore();
</script>

<template>
  <!-- Contenedor principal con gradiente mejorado -->
  <div class="min-h-screen bg-gradient-to-br from-purple-900 via-blue-900 to-indigo-900 relative overflow-hidden">
    <!-- Elementos decorativos de fondo -->
    <div class="absolute inset-0 opacity-10">
      <div class="absolute top-20 left-10 w-72 h-72 bg-purple-500 rounded-full mix-blend-multiply filter blur-xl animate-pulse"></div>
      <div class="absolute top-40 right-10 w-72 h-72 bg-pink-500 rounded-full mix-blend-multiply filter blur-xl animate-pulse animation-delay-2000"></div>
      <div class="absolute bottom-20 left-1/2 w-72 h-72 bg-indigo-500 rounded-full mix-blend-multiply filter blur-xl animate-pulse animation-delay-4000"></div>
    </div>

    <!-- Contenido principal -->
    <div class="relative z-10 py-16 px-4 sm:px-6 lg:px-8">
      <div class="max-w-7xl mx-auto">
        
        <!-- Header mejorado -->
        <div class="text-center mb-16">
          <div class="inline-flex items-center justify-center p-2 bg-gradient-to-r from-pink-500 to-purple-600 rounded-full mb-6">
            <svg class="w-8 h-8 text-white" fill="currentColor" viewBox="0 0 20 20">
              <path fill-rule="evenodd" d="M3.172 5.172a4 4 0 015.656 0L10 6.343l1.172-1.171a4 4 0 115.656 5.656L10 17.657l-6.828-6.829a4 4 0 010-5.656z" clip-rule="evenodd" />
            </svg>
          </div>
          <h1 class="text-5xl md:text-6xl font-black text-transparent bg-clip-text bg-gradient-to-r from-pink-400 via-purple-400 to-indigo-400 mb-4">
            Mi Lista de Deseos
          </h1>
          <p class="text-gray-300 text-lg max-w-2xl mx-auto">
            Tus productos favoritos esperándote. ¡Haz realidad tus sueños gaming!
          </p>
        </div>

        <!-- Estado vacío mejorado -->
        <div v-if="wishlistStore.wishlist.length === 0" class="text-center py-20">
          <div class="bg-white/5 backdrop-blur-lg rounded-3xl p-12 max-w-md mx-auto border border-white/10">
            <div class="w-24 h-24 mx-auto mb-6 bg-gradient-to-r from-purple-500 to-pink-500 rounded-full flex items-center justify-center">
              <svg class="w-12 h-12 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" />
              </svg>
            </div>
            <h3 class="text-2xl font-bold text-white mb-4">¡Tu lista está vacía!</h3>
            <p class="text-gray-400 mb-6">
              Empieza a explorar nuestra increíble colección y guarda tus productos favoritos aquí.
            </p>
            <button class="bg-gradient-to-r from-purple-600 to-pink-600 hover:from-purple-700 hover:to-pink-700 text-white font-semibold py-3 px-8 rounded-full transition-all duration-300 transform hover:scale-105 hover:shadow-lg">
              Explorar Productos
            </button>
          </div>
        </div>

        <!-- Grid de productos mejorado -->
        <div v-else>
          <!-- Contador de items -->
          <div class="flex items-center justify-between mb-8">
            <div class="bg-white/10 backdrop-blur-lg rounded-full px-6 py-3 border border-white/20">
              <span class="text-white font-semibold">
                {{ wishlistStore.wishlist.length }} 
                {{ wishlistStore.wishlist.length === 1 ? 'producto' : 'productos' }} 
                en tu lista
              </span>
            </div>
            
            <!-- Botones de acción -->
            <div class="flex gap-3">
              <button class="bg-white/10 hover:bg-white/20 backdrop-blur-lg text-white font-medium py-2 px-4 rounded-full border border-white/20 transition-all duration-300">
                Compartir Lista
              </button>
              <button class="bg-gradient-to-r from-green-500 to-emerald-600 hover:from-green-600 hover:to-emerald-700 text-white font-medium py-2 px-4 rounded-full transition-all duration-300 transform hover:scale-105">
                Comprar Todo
              </button>
            </div>
          </div>

          <!-- Grid responsivo con animaciones -->
          <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8">
            <div
              v-for="(producto, index) in wishlistStore.wishlist"
              :key="producto.idProducto"
              class="transform transition-all duration-500 hover:scale-105"
              :style="{ 'animation-delay': `${index * 100}ms` }"
            >
              <ProductCard
                :producto="producto"
                class="h-full bg-white/5 backdrop-blur-lg rounded-2xl border border-white/10 hover:border-white/20 hover:bg-white/10 transition-all duration-300"
              />
            </div>
          </div>

          <!-- Sección de estadísticas -->
          <div class="mt-16 grid grid-cols-1 md:grid-cols-3 gap-6">
            <div class="bg-white/5 backdrop-blur-lg rounded-2xl p-6 border border-white/10 text-center">
              <div class="text-3xl font-bold text-purple-400 mb-2">
                ${{ wishlistStore.wishlist.reduce((total, item) => total + item.precio, 0).toLocaleString() }}
              </div>
              <div class="text-gray-400">Valor Total</div>
            </div>
            
            <div class="bg-white/5 backdrop-blur-lg rounded-2xl p-6 border border-white/10 text-center">
              <div class="text-3xl font-bold text-pink-400 mb-2">
                {{ wishlistStore.wishlist.length }}
              </div>
              <div class="text-gray-400">Productos Guardados</div>
            </div>
            
            <div class="bg-white/5 backdrop-blur-lg rounded-2xl p-6 border border-white/10 text-center">
              <div class="text-3xl font-bold text-indigo-400 mb-2">
                {{ wishlistStore.wishlist.filter(item => item.stock > 0).length }}
              </div>
              <div class="text-gray-400">Disponibles</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
  <Footer />
</template>

<style scoped>
/* Animaciones personalizadas */
@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.grid > div {
  animation: fadeInUp 0.6s ease-out forwards;
  opacity: 0;
}

/* Delays para animación escalonada */
.animation-delay-2000 {
  animation-delay: 2s;
}

.animation-delay-4000 {
  animation-delay: 4s;
}

/* Efectos de vidrio */
.backdrop-blur-lg {
  backdrop-filter: blur(16px);
}

/* Efectos hover mejorados */
.hover\:scale-105:hover {
  transform: scale(1.05);
}

/* Scrollbar personalizado */
::-webkit-scrollbar {
  width: 8px;
}

::-webkit-scrollbar-track {
  background: rgba(255, 255, 255, 0.1);
  border-radius: 4px;
}

::-webkit-scrollbar-thumb {
  background: linear-gradient(180deg, #8b5cf6, #ec4899);
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: linear-gradient(180deg, #7c3aed, #db2777);
}
</style>