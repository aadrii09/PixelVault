<template>
    <div class="fixed top-20 right-4 bg-white dark:bg-gray-800 text-black dark:text-white p-4 rounded-lg shadow-lg w-80 z-50 max-h-[80vh] overflow-y-auto cart-container">
      <div class="flex justify-between items-center mb-4">
        <h3 class="font-bold">Carrito de Compras</h3>
        <button @click="$emit('close')" class="text-gray-500 dark:text-gray-400">&times;</button>
      </div>
      <div v-if="items.length === 0" class="text-center py-4">
        El carrito está vacío
      </div>
      <div v-else>
        <div v-for="item in items" :key="item.id" class="flex justify-between items-center mb-2 pb-2 border-b dark:border-gray-700">
          <div class="flex-1">
            <div class="font-medium">{{ item.title }}</div>
            <div class="text-sm text-gray-600 dark:text-gray-400 flex items-center mt-1">
              <button @click="$emit('update-quantity', item.id, Math.max(1, (item.quantity || 1) - 1))" class="w-5 h-5 bg-gray-200 dark:bg-gray-700 rounded flex items-center justify-center">-</button>
              <span class="mx-2">{{ item.quantity || 1 }}</span>
              <button @click="$emit('update-quantity', item.id, (item.quantity || 1) + 1)" class="w-5 h-5 bg-gray-200 dark:bg-gray-700 rounded flex items-center justify-center">+</button>
            </div>
          </div>
          <div class="flex flex-col items-end ml-2">
            <div class="text-accent">{{ ((item.price || 0) * (item.quantity || 1)).toFixed(2) }}€</div>
            <button @click="$emit('remove-item', item.id)" class="text-red-500 text-sm mt-1">Eliminar</button>
          </div>
        </div>
        <div class="mt-4 font-bold text-right">
          Total: {{ calculateTotal() }}€
        </div>
        <button @click="$emit('checkout')" class="w-full mt-4 bg-accent hover:bg-green-600 text-white py-2 rounded transition-colors">
          Finalizar Compra
        </button>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  interface CartItem {
    id: number;
    title: string;
    price: number;
    quantity?: number;
    image?: string;
    category?: string;
  }
  
  const props = defineProps<{
    items: CartItem[]
  }>();
  
  defineEmits(['close', 'remove-item', 'update-quantity', 'checkout']);
  
  const calculateTotal = () => {
    return props.items.reduce((total, item) => total + ((item.price || 0) * (item.quantity || 1)), 0).toFixed(2);
  };
  </script>