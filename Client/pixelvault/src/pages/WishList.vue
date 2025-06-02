<script setup>
import { useWishlistStore } from '../store/wishlist';
import ProductCard from '../components/ProductCard.vue';

const wishlistStore = useWishlistStore();
</script>

<template>
  <div class="max-w-6xl mx-auto px-4 py-6">
    <h1 class="text-3xl font-bold mb-6 text-center text-white">Mi Lista de Deseos</h1>

    <div v-if="wishlistStore.wishlist.length === 0" class="text-center text-gray-300">
      Tu lista está vacía. ¡Empieza a explorar y guarda tus favoritos!
    </div>

    <div v-else class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
      <ProductCard
        v-for="producto in wishlistStore.wishlist"
        :key="producto.idProducto"
        :producto="producto"
      />
    </div>
  </div>
</template>