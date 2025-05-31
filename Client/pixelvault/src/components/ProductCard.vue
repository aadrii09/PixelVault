<script setup>
import { agregarAlCarrito } from '../api/carrito';
import { RouterLink } from 'vue-router'

const props = defineProps([
    'producto',
    'mostrarBoton',
]);

const agregar = async () => {
    try {
        const productoFormateado = {
            idProducto: props.producto.idProducto,
            nombre: props.producto.nombre || "",      // debe ser string
            imagenUrl: props.producto.imagenUrl || "", // debe ser string
            cantidad: 1,
            precioUnitario: props.producto.precio || 0 // debe ser número
        };

        console.log("📦 Producto enviado al backend:", productoFormateado);

        await agregarAlCarrito(productoFormateado);
        alert(`Producto ${props.producto.nombre} agregado al carrito`);
    } catch (error) {
        alert('Error al agregar al carrito');
        console.error('Error al agregar al carrito:', error.response?.data || error);
    }
};

</script>

<template>
    <div
        class="bg-gradient-to-br from-white/10 to-white/5 border border-white/20 rounded-3xl p-6 transition-all duration-300 hover:transform hover:-translate-y-2 hover:shadow-lg hover:shadow-[#00ccff]/20 hover:border-[#00ccff] product-card">
        <!-- Product Image -->
        <div
            class="w-full h-[200px] bg-gradient-btn rounded-2xl mb-4 flex items-center justify-center overflow-hidden product-image">
            <img v-if="props.producto.imagenUrl" :src="props.producto.imagenUrl" :alt="props.producto.nombre"
                class="w-full h-full object-cover" />
            <div v-else class="text-5xl text-[#0f0f23]">🎮</div>
        </div>

        <!-- Product Info -->
        <h3 class="text-xl mb-2 text-[#00ff88] product-title">{{ props.producto.nombre }}</h3>
        <p v-if="props.producto.descripcion" class="text-gray-300 text-sm mb-3 line-clamp-2">
            {{ props.producto.descripcion }}
        </p>
        <div class="flex justify-between items-center mb-4">
            <div class="text-2xl font-bold text-[#00ccff] product-price">
                ${{ props.producto.precio?.toFixed(2) }}
            </div>
            <div class="text-sm text-gray-400">
                Stock: {{ props.producto.stock }}
            </div>
        </div>

        <!-- Buttons Container -->
        <div class="flex space-x-2 mt-4">
            <!-- Add to Cart Button -->
            <button v-if="props.mostrarBoton" @click="agregar"
                class="relative flex-1 group overflow-hidden px-3 py-2 text-sm rounded-lg font-bold text-white transition-all duration-300 hover:shadow-lg hover:shadow-[#00ccff]/30"
                style="
                    background: linear-gradient(135deg, #00ccff 0%, #00ff88 100%);
                    box-shadow: 0 2px 10px rgba(0, 204, 255, 0.3);
                ">
                <span class="relative z-10 flex items-center justify-center">
                    <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-1.5" fill="none" viewBox="0 0 24 24"
                        stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                            d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                    </svg>
                    Añadir
                </span>
                <span
                    class="absolute inset-0 bg-gradient-to-r from-[#00ff88] to-[#00ccff] opacity-0 group-hover:opacity-100 transition-opacity duration-300"></span>
            </button>

            <!-- View Details Button -->
            <button @click="$emit('view-details', props.producto)"
                class="relative flex-1 group overflow-hidden px-3 py-2 text-sm rounded-lg font-bold text-white transition-all duration-300 hover:shadow-lg hover:shadow-[#9f7aea]/30"
                style="
                    background: linear-gradient(135deg, #8b5cf6 0%, #c084fc 100%);
                    box-shadow: 0 2px 10px rgba(139, 92, 246, 0.3);
                ">
                <!-- View Details Button -->
                <RouterLink :to="`/producto/${props.producto.idProducto}`"
                    class="relative flex-1 group overflow-hidden px-3 py-2 text-sm rounded-lg font-bold text-white transition-all duration-300 hover:shadow-lg hover:shadow-[#9f7aea]/30"
                    style="background: linear-gradient(135deg, #8b5cf6 0%, #c084fc 100%); box-shadow: 0 2px 10px rgba(139, 92, 246, 0.3);">
                    <span class="relative z-10 flex items-center justify-center">
                        <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-1.5" fill="none" viewBox="0 0 24 24"
                            stroke="currentColor">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                d="M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                        </svg>
                        Ver más
                    </span>
                    <span
                        class="absolute inset-0 bg-gradient-to-r from-[#c084fc] to-[#8b5cf6] opacity-0 group-hover:opacity-100 transition-opacity duration-300"></span>
                </RouterLink>

                <span
                    class="absolute inset-0 bg-gradient-to-r from-[#c084fc] to-[#8b5cf6] opacity-0 group-hover:opacity-100 transition-opacity duration-300"></span>
            </button>
        </div>
    </div>
</template>

<style scoped>
/* Add any additional styles here if needed */
</style>