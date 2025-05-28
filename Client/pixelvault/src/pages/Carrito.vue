<script setup>
import { ref, onMounted, nextTick, watch } from 'vue';
import { getCarrito, vaciarCarrito } from '../api/carrito';
import { processPayment } from '../api/stripe';
import { loadStripe } from '@stripe/stripe-js';
import { useRouter } from 'vue-router';
import { crearOrder, verificarOrden } from '../api/paypal';

const router = useRouter();
const carrito = ref(null);
const cargando = ref(true);
const error = ref('');
const procesandoPago = ref(false);
const mostrarFormularioPago = ref(false);
const cardElement = ref(null);
const stripe = ref(null);
const elements = ref(null);

// Inicializar Stripe
const initStripe = async () => {
  console.log('💳 Iniciando Stripe...');
  stripe.value = await loadStripe('pk_test_51RTSVtQv5xnysdLKAXO57tv8Y5LvK4mP0SnkDl7zaCAE4fHP6c5bLoln640AzUs8gkJjlhFgiJZNXXv2w6TV7wRY00HlcD7Gfc');
  elements.value = stripe.value.elements();

  // Crear el elemento de tarjeta
  cardElement.value = elements.value.create('card', {
    style: {
      base: {
        fontSize: '16px',
        color: '#424770',
        '::placeholder': {
          color: '#aab7c4',
        },
      },
      invalid: {
        color: '#9e2146',
      },
    },
  });

  // Montar el elemento de tarjeta
  await nextTick();
  cardElement.value.mount('#card-element');
  console.log('✅ Stripe card element montado en #card-element');
};

// Cargar el carrito
const cargarCarrito = async () => {
  try {
    cargando.value = true;
    carrito.value = await getCarrito();
    console.log('📦 Carrito recibido del backend:', carrito.value);
    if (carrito.value && carrito.value.Productos) {
      carrito.value.productos = carrito.value.Productos;
    }
  } catch (err) {
    error.value = 'Error al cargar el carrito';
    console.error(err);
  } finally {
    cargando.value = false;
  }
};

// Procesar el pago
const procesarPago = async () => {
  if (!carrito.value || carrito.value.Total <= 0) {
    error.value = 'El carrito está vacío';
    return;
  }

  try {
    procesandoPago.value = true;
    error.value = '';

    const success = await processPayment(carrito.value.Total, elements.value); // ✅ pasa el `elements`

    if (success) {
      await vaciarCarrito();
      router.push({ name: 'PagoExitoso' });
    } else {
      error.value = 'El pago no fue exitoso';
    }
  } catch (err) {
    error.value = 'Error al procesar el pago';
    console.error(err);
  } finally {
    procesandoPago.value = false;
  }
};

const renderPayPalButton = () => {
  if (!window.paypal) return;
  const container = document.getElementById('paypal-button-container');
  if (!container) {
    console.warn('⚠️ Contenedor #paypal-button-container no encontrado en el DOM');
    return;
  } else {
    console.log('🟢 Contenedor de PayPal encontrado');
  }


  window.paypal.Buttons({
    createOrder: async () => {
      const orderId = await crearOrder(carrito.value.total);
      console.log("🧾 ID de orden generado:", orderId);
      return orderId;
    },

    onApprove: async (data, actions) => {
      const result = await verificarOrden(data.orderID);
      if (result.success) {
        await vaciarCarrito();
        router.push({ name: 'PagoExitoso' });
      } else {
        error.value = 'No se pudo verificar el pago con PayPal';
      }
    },
    onError: (err) => {
      console.error('❌ Error en PayPal:', err);
      error.value = 'Error con PayPal';
    }
  }).render('#paypal-button-container');
};


// Observar cambios en el carrito
watch(carrito, (newVal) => {
  console.log('🔁 Carrito actualizado:', newVal);
  console.log('✅ mostrarFormularioPago debería ser true?', newVal?.total > 0); // minúscula
  mostrarFormularioPago.value = !!(newVal && newVal.total > 0);
});

onMounted(async () => {
  await cargarCarrito();
  await nextTick();

  // Espera al DOM para montar Stripe y PayPal
  watch(
    mostrarFormularioPago,
    async (mostrar) => {
      console.log('👀 Cambio en mostrarFormularioPago:', mostrar);
      if (mostrar) {
        await nextTick();
        console.log('⌛ DOM actualizado. Inicializando métodos de pago...');
        await initStripe();

        if (!document.getElementById("paypal-sdk")) {
          console.log('📦 SDK de PayPal aún no cargado. Añadiendo script...');
          const script = document.createElement("script");
          script.src = "https://www.paypal.com/sdk/js?client-id=AUyxWpP73OMKhrokIfzR-qKJuPfLdsE4OfdVF6XgJscBRMcMKsndcf4rBU3jUUTerM6umvnU0ElwRVwk&currency=USD";
          script.id = "paypal-sdk";
          script.onload = () => {
            console.log('✅ SDK de PayPal cargado y listo');
            renderPayPalButton();
          };
          document.body.appendChild(script);
        } else {
          console.log('♻️ SDK de PayPal ya cargado. Renderizando botón...');
          renderPayPalButton();
        }
      }
    },
    { immediate: true }
  );
});


</script>

<template>
  <div class="container mx-auto px-4 py-8">
    <h1 class="text-3xl font-bold mb-8">Carrito de Compras</h1>

    <!-- Mensaje de carga -->
    <div v-if="cargando" class="text-center">
      <p>Cargando carrito...</p>
    </div>

    <!-- Mensaje de error -->
    <div v-if="error" class="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
      {{ error }}
    </div>

    <!-- Carrito vacío -->
    <div v-if="!cargando && (!carrito || !carrito.productos || carrito.productos.length === 0)">
      <p class="text-gray-600">Tu carrito está vacío</p>
      <router-link to="/" class="text-blue-500 hover:underline mt-4 inline-block">
        Continuar comprando
      </router-link>
    </div>

    <!-- Contenido del carrito -->
    <div v-if="carrito && carrito.productos.length > 0" class="grid grid-cols-1 md:grid-cols-3 gap-8">
      <!-- Lista de productos -->
      <div class="md:col-span-2">
        <div v-for="producto in carrito.productos" :key="producto.IdProducto"
          class="flex items-center justify-between border-b py-4">
          <div class="flex items-center">
            <img :src="producto.imagenUrl" :alt="producto.nombre" class="w-20 h-20 object-cover rounded">
            <div class="ml-4">
              <h3 class="font-semibold">{{ producto.nombre }}</h3>
              <p class="text-gray-600">Cantidad: {{ producto.cantidad }}</p>
              <p class="text-gray-600">Precio: ${{ producto.precioUnitario.toFixed(2) }}</p>
            </div>
          </div>
          <p class="font-semibold">${{ producto.subtotal }}</p>
        </div>
      </div>

      <!-- Resumen y pago -->
      <div class="md:col-span-1">
        <div class="bg-gray-50 p-6 rounded-lg">
          <h2 class="text-xl font-semibold mb-4">Resumen del Pedido</h2>
          <div class="space-y-2 mb-4">
            <div class="flex justify-between">
              <span>Subtotal:</span>
              <span>${{ carrito?.total }}</span>
            </div>
            <div class="flex justify-between font-semibold">
              <span>Total:</span>
              <span>${{ carrito?.total }}</span>
            </div>
          </div>

          <!-- Pago con Stripe y PayPal -->
          <div v-if="mostrarFormularioPago" class="mt-6 space-y-4">
            <!-- Stripe -->
            <div>
              <div id="card-element" class="mb-4"></div>
              <button @click="procesarPago" :disabled="procesandoPago" class="w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700 
                   disabled:bg-gray-400 disabled:cursor-not-allowed">
                {{ procesandoPago ? 'Procesando...' : 'Pagar con tarjeta' }}
              </button>
            </div>

            <!-- PayPal -->
            <div id="paypal-button-container" class="mt-6"></div>
          </div>



        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
#card-element {
  padding: 1rem;
  border: 1px solid #e2e8f0;
  border-radius: 0.375rem;
  margin-bottom: 1rem;
  background-color: white;
}

/* Estilos adicionales para el formulario de Stripe */
.StripeElement--focus {
  border-color: #4299e1;
  box-shadow: 0 0 0 1px #4299e1;
}

.StripeElement--invalid {
  border-color: #e53e3e;
}

.StripeElement--webkit-autofill {
  background-color: #fefde5 !important;
}

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