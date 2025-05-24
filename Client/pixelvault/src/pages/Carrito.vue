<script setup>
import { ref, onMounted, nextTick, watch } from 'vue';
import { getCarrito, vaciarCarrito } from '../api/carrito';
import { crearOrder, verificarOrden } from '../api/paypal';

const carrito = ref(null);
const cargando = ref(true);
const error = ref('');
const mostrarBotonPaypal = ref(false);

const cargarCarrito = async () => {
    try {
        cargando.value = true;
        
        // Usar getCarrito() para obtener los datos iniciales
        try {
            const data = await getCarrito();
            // Aseguramos que cada producto tenga una propiedad cantidad y calculemos el subtotal
            if (data && data.productos) {
                data.productos = data.productos.map(producto => {
                    return {
                        ...producto,
                        cantidad: producto.cantidad || 1,
                        subtotal: (producto.cantidad || 1) * (producto.precioUnitario || 0)
                    };
                });
                // Recalcular el total
                data.total = data.productos.reduce((sum, item) => sum + item.subtotal, 0);
            }
            carrito.value = data;
        } catch (err) {
            // Si falla la API, crear datos de ejemplo para que siga funcionando
            console.warn('⚠️ Error cargando el carrito desde la API, usando datos de ejemplo', err);
            carrito.value = {
                total: 0,
                productos: []
            };
        }
        
        mostrarBotonPaypal.value = carrito.value?.productos?.length > 0;
    } catch (err) {
        console.error('Error general al cargar el carrito:', err);
        error.value = 'Error al cargar el carrito';
    } finally {
        cargando.value = false;
    }
};

const vaciar = async () => {
    try {
        await vaciarCarrito();
    } catch (err) {
        console.warn('⚠️ Error al vaciar el carrito en la API', err);
    }
    // Vaciar localmente de todas formas
    if (carrito.value) {
        carrito.value.productos = [];
        carrito.value.total = 0;
    }
};

// Implementación local de incrementar/decrementar sin llamar al API
const incrementarCantidad = (item) => {
    if (!item) return;
    item.cantidad++;
    actualizarSubtotales();
};

const decrementarCantidad = (item) => {
    if (!item || item.cantidad <= 1) return;
    item.cantidad--;
    actualizarSubtotales();
};

// Recalcula subtotales y total
const actualizarSubtotales = () => {
    if (!carrito.value || !carrito.value.productos) return;
    
    let total = 0;
    carrito.value.productos.forEach(item => {
        item.subtotal = item.cantidad * item.precioUnitario;
        total += item.subtotal;
    });
    
    carrito.value.total = total;
};

const renderizarPaypal = async () => {
    await nextTick();

    const container = document.getElementById('paypal-button-container');
    if (!window.paypal || !container) {
        console.warn("⚠️ El contenedor de PayPal aún no está disponible.");
        return;
    }

    window.paypal.Buttons({
        createOrder: async () => {
            return await crearOrder(carrito.value.total);
        },
        onApprove: async (data) => {
            const respuesta = await verificarOrden(data.orderID);
            alert('Pago realizado con éxito: ' + respuesta.estado);
            await vaciarCarrito();
        },
        onError: (err) => {
            console.error('Error al procesar el pago:', err);
            alert('Error al procesar el pago');
        },
        onCancel: () => {
            alert('Pago cancelado');
        }
    }).render('#paypal-button-container');
};

watch(mostrarBotonPaypal, async (nuevoValor) => {
    if (nuevoValor) {
        await renderizarPaypal();
    }
});

onMounted(cargarCarrito);
</script>

<template>
    <div class="max-w-4xl mx-auto px-4 scroll-py-64">
        <div class="bg-white rounded-lg shadow-lg p-6 my-8">
            <h1 class="text-2xl font-bold mb-6">CARRITO</h1>

            <div v-if="cargando" class="text-center py-8">Cargando...</div>
            <div v-else-if="!carrito || carrito.productos.length === 0" class="text-center py-8">
                <p>No hay productos en el carrito</p>
            </div>
            <div v-else>
                <div class="space-y-4 mb-8">
                    <div v-for="item in carrito.productos" :key="item.idProducto"
                        class="flex items-center py-4 border-b">
                        <div class="w-20 h-20 mr-4">
                            <!-- Sistema completo de fallback para imágenes -->
                            <img 
                                :src="item.urlImagen || item.imagenUrl || item.imagen" 
                                :alt="item.nombre || `Producto ${item.idProducto}`" 
                                class="w-full h-full object-contain"
                                @error="e => e.target.outerHTML = `<div class='w-full h-full flex items-center justify-center bg-gray-200 text-gray-600 text-sm'>Producto ${item.idProducto}</div>`"
                            >
                        </div>
                        <div class="flex-grow">
                            <p class="font-medium">{{ item.nombre || `Producto ${item.idProducto}` }}</p>
                        </div>
                        <div class="flex items-center">
                            <button @click="decrementarCantidad(item)" class="w-8 h-8 flex items-center justify-center bg-gray-200 hover:bg-gray-300 rounded-l">
                                -
                            </button>
                            <input type="text" :value="item.cantidad" readonly class="w-10 h-8 text-center border-y outline-none bg-white">
                            <button @click="incrementarCantidad(item)" class="w-8 h-8 flex items-center justify-center bg-gray-200 hover:bg-gray-300 rounded-r">
                                +
                            </button>
                        </div>
                        <div class="ml-8 text-right font-bold w-32">
                            {{ (item.subtotal || (item.cantidad * item.precioUnitario)).toFixed(2) }}€
                        </div>
                    </div>
                </div>

                <div class="border-t pt-4">
                    <div class="flex justify-between items-center mb-4">
                        <h2 class="text-xl font-bold">TOTAL</h2>
                        <p class="text-xl font-bold">{{ carrito.total.toFixed(2) }}€</p>
                    </div>
                    
                    <div class="mb-4">
                        <p class="font-medium mb-2">Método de pago</p>
                        <div class="border rounded-md p-2 flex items-center">
                            <img src="../../public/images/paypallogo.png" alt="PayPal" class="h-6 mr-2">
                            <span>PayPal</span>
                            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 ml-auto" viewBox="0 0 20 20" fill="currentColor">
                                <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
                            </svg>
                        </div>
                    </div>
                    
                    <div class="mb-6">
                        <p class="font-medium mb-2">CÓDIGO DESCUENTO</p>
                        <input type="text" placeholder="xxx-xxx-xx" class="w-full border rounded-md p-2">
                    </div>
                    
                    <div class="flex justify-between">
                        <button @click="vaciar" class="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-800">
                            Vaciar Carrito
                        </button>
                        <button class="bg-blue-600 text-white px-8 py-2 rounded hover:bg-blue-800">
                            COMPRAR
                        </button>
                    </div>

                    <div id="paypal-button-container" class="mt-6" v-if="mostrarBotonPaypal"></div>
                </div>
            </div>
        </div>
    </div>
</template>
<style>
body {
    background-color: #0e0b30;
}
#paypal-button-container {
    display: flex;
    justify-content: center;
    justify-self: center;  
    width: 50%;
}
</style>