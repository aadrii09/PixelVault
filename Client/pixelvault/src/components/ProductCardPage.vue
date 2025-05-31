<template>
  <div class="product-card bg-primary dark:bg-gray-800 rounded-lg overflow-hidden flex flex-col">
    <div class="p-4 text-center text-white font-medium">
      {{ product.title }}
    </div>
    <div class="flex-1 bg-secondary dark:bg-gray-900 p-4 flex items-center justify-center">
      <img :src="product.image" :alt="product.title" class="max-h-40 object-contain" @click="$emit('view-details', product)" />
    </div>
    <div class="p-4 flex items-center justify-between">
      <span class="text-accent font-bold text-xl">{{ formatPrice(product.price) }}</span>
      <button @click="$emit('add-to-cart', product)" class="bg-primary-light dark:bg-blue-700 text-white px-3 py-1 rounded flex items-center text-sm hover:bg-opacity-80 transition-colors">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
        </svg>
        Añadir carrito
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
interface Product {
  id: number;
  title: string;
  image: string;
  price: number;
  category: string;
}

defineProps<{
  product: Product
}>();

defineEmits(['add-to-cart', 'view-details']);

const formatPrice = (price: number): string => {
  return `${price}€`;
};
</script>