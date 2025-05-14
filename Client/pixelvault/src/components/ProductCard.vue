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
    <div class="bg-white p-4 rounded shadow hover:shadow-lg transition">
        <img :src="props.producto.imagenUrl || 'https://upload.wikimedia.org/wikipedia/commons/1/14/No_Image_Available.jpg'" alt="Product Image" class="w-full h-40 object-cover rounded mb-3">
        <h3 class="text-lg font-semibold">{{ props.producto.nombre }}</h3>
        <p class="text-sm text-gray-500">{{ props.producto.descripcion?.slice(0, 50) }}...</p>
        <p class="mt-2 font-bold text-blue-600">{{ props.producto.stock }}</p>
        <button
            v-if="props.mostrarBoton"
            @click="agregar"
            class="mt-3 bg-blue-600 text-white py-1 rounded hover:bg-blue-800">
            Agregar al carrito
        </button>
    </div>
</template>