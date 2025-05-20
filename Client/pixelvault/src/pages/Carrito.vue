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
        carrito.value = await getCarrito();
        mostrarBotonPaypal.value = carrito.value?.productos?.length > 0;
    } catch (err) {
        console.error('Error al cargar el carrito:', err);
        error.value = 'Error al cargar el carrito';
    } finally {
        cargando.value = false;
    }
};

const vaciar = async () => {
    await vaciarCarrito();
    await cargarCarrito();
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
        <h1 class="text-2xl font-bold mb-6">Tu Carrito</h1>

        <div v-if="cargando">Cargando...</div>
        <div v-else-if="!carrito || carrito.productos.length === 0">
            <p>No hay productos en el carrito</p>
        </div>
        <div v-else>
            <div class="space-y-4">
                <div v-for="item in carrito.productos" :key="item.idProducto"
                    class="p-4 bg-white shadow rounded flex justify-between">
                    <div>
                        <p class="font-semibold">{{ item.idProducto }} - {{ item.cantidad }} x {{ item.precioUnitario }}</p>
                    </div>
                    <div class="text-right font-bold text-blue-600">
                        ${{ item.subtotal.toFixed(2) }}
                    </div>
                </div>
            </div>

            <div class="my-6 text-right">
                <p class="text-xl font-bold">Total: ${{ carrito.total.toFixed(2) }}</p>
                <div class="mt-4 flex justify-end">
                    <button @click="vaciar" class="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-800">
                        Vaciar Carrito
                    </button>
                </div>

                <div id="paypal-button-container" class="mt-6" v-if="mostrarBotonPaypal"></div>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped></style>