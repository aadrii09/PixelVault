<template>
    <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div class="bg-white dark:bg-gray-800 rounded-lg max-w-2xl w-full max-h-[90vh] overflow-y-auto">
        <div class="p-6">
          <div class="flex justify-between items-start mb-4">
            <h2 class="text-2xl font-bold text-gray-900 dark:text-white">{{ product.title }}</h2>
            <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
          
          <div class="flex flex-col md:flex-row gap-6">
            <div class="flex-shrink-0 w-full md:w-1/2 bg-gray-100 dark:bg-gray-700 rounded-lg p-4 flex items-center justify-center">
              <img :src="product.image" :alt="product.title" class="max-h-60 object-contain" />
            </div>
            
            <div class="flex-1">
              <div class="mb-4">
                <span class="text-3xl font-bold text-accent">{{ product.price }}€</span>
              </div>
              
              <div class="mb-6">
                <h3 class="text-lg font-medium text-gray-900 dark:text-white mb-2">Descripción</h3>
                <p class="text-gray-600 dark:text-gray-300">
                  Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam euismod, nisl eget ultricies aliquam, 
                  nunc nisl aliquet nunc, vitae aliquam nisl nunc eu nisl.
                </p>
              </div>
              
              <div class="mb-6">
                <h3 class="text-lg font-medium text-gray-900 dark:text-white mb-2">Características</h3>
                <ul class="list-disc list-inside text-gray-600 dark:text-gray-300">
                  <li>Característica 1</li>
                  <li>Característica 2</li>
                  <li>Característica 3</li>
                  <li>Característica 4</li>
                </ul>
              </div>
              
              <button @click="$emit('add-to-cart', product)" class="w-full bg-accent hover:bg-green-600 text-white py-2 px-4 rounded-md flex items-center justify-center">
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                </svg>
                Añadir al carrito
              </button>
            </div>
          </div>
        </div>
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
  
  defineEmits(['close', 'add-to-cart']);
  </script>