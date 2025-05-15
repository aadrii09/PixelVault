<script setup>
    import { ref, onMounted } from 'vue';  
    import { getCarrito, vaciarCarrito, agregarAlCarrito } from '../api/carrito';

    const carrito = ref(null);
    const cargando = ref(true);
    const error = ref('');
    const paypalReady = ref(false);

    

    const cargarCarrito = async () => {
        try {
            carrito.value = await getCarrito();
            if (carrito.value && carrito.value.productos && carrito.value.productos.length > 0) {
                await renderizarPaypal();
            }
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
        if(!window.paypal || !carrito.value) return
            
        window.paypal.Buttons({
            createOrder: async()=>{
                return await crearOrder(carrito.value.total);
            },
            onApprove: async(data)=>{
                const respuesta = await verificarOrden(data.orderID);
                alert('Pago realizado con exito' + respuesta.estado);
                await vaciarCarrito();
            },
            onError: (err)=>{
                console.error('Error al procesar el pago:', err);
                alert('Error al procesar el pago');
            },
            onCancel: ()=>{
                alert('Pago cancelado');
            }

        }).render('#paypal-button-container');
    }


    onMounted(cargarCarrito);
</script>

<template>
    <div class="max-w-4xl mx-auto px-4 scroll-py-64">
        <h1 class="text-2xl font-bold mb-6">Tu Carrito</h1>

        <div v-if="cargando">Cargando...</div>
        <div v-else-if="!carrito || carrito.productoslength ===0">
            <p>No hay productos en el carrito</p>
        </div>
        <div v-else>
            <div class="space-y-4">
                <div v-for="item in carrito.productos"
                 :key="item.idProducto"
                  class="p-4 bg-white shadow rounded flex-justify-between">
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

                    <!-- <button class="px-4 py-2 bg-green-600 text-white rounded hover:bg-green-800">Ir a pagar</button> -->

                    <!-- boton de paypal -->
                     <div id="paypal-button-container" class="mt-6" v-show="!cargando && !error && !paypalReady"></div>

                </div>
            </div>

        </div>
    </div>
</template>

<style lang="scss" scoped>

</style>
