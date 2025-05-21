<script setup>

import { agregarAlCarrito } from '../api/carrito';


   const props = defineProps([
        'producto',
        'mostrarBoton',
    ]);

    const agregar = async () => {
        try {
            await agregarAlCarrito(props.producto);
            alert(`Producto ${props.producto.nombre} agregado al carrito`); 
        } catch (error) {
           alert('Error al agregar al carrito');
           console.error('Error al agregar al carrito:', error);
        }
    }
</script>

<template>
    <div class="bg-white p-6 rounded shadow hover:shadow-lg transition">
        <div class="h-60 rounded-t overflow-hidden -mx-6 -mt-6 mb-4">
            <img :src="props.producto.imagenUrl || 'https://upload.wikimedia.org/wikipedia/commons/1/14/No_Image_Available.jpg'" alt="Product Image" class="w-full h-full object-cover">
        </div>
        <h3 class="text-xl font-bold">{{ props.producto.nombre }}</h3>
        <p class="text-base text-gray-600">{{ props.producto.descripcion?.slice(0, 80) }}...</p>
        <p class="mt-3 font-bold text-blue-600 text-lg">{{ props.producto.stock }}</p>
        <button
            v-if="props.mostrarBoton"
            @click="agregar"
            class="mt-4 bg-blue-600 text-white py-2.5 px-5 text-base rounded hover:bg-blue-800 w-full font-medium">
            Agregar al carrito
        </button>
    </div>
</template>