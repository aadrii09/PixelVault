<script setup>
import { ref, onMounted, nextTick, watch } from 'vue';
import { getCarrito, vaciarCarrito } from '../api/carrito';
import { processPayment } from '../api/stripe';
import { loadStripe } from '@stripe/stripe-js';
import { useRouter } from 'vue-router';

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
  stripe.value = await loadStripe('tu_stripe_publishable_key');
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
};

// Cargar el carrito
const cargarCarrito = async () => {
  try {
    cargando.value = true;
    carrito.value = await getCarrito();
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

    const { error: stripeError, paymentIntent } = await stripe.value.confirmCardPayment(
      await processPayment(carrito.value.Total),
      {
        payment_method: {
          card: cardElement.value,
          billing_details: {
            name: 'Nombre del Cliente', // Puedes obtener esto del usuario actual
          },
        },
      }
    );

    if (stripeError) {
      error.value = stripeError.message;
      return;
    }

    if (paymentIntent.status === 'succeeded') {
      // Pago exitoso
      await vaciarCarrito();
      router.push({ name: 'PagoExitoso' });
    }
  } catch (err) {
    error.value = 'Error al procesar el pago';
    console.error(err);
  } finally {
    procesandoPago.value = false;
  }
};

// Observar cambios en el carrito
watch(carrito, (newVal) => {
  if (newVal && newVal.Total > 0) {
    mostrarFormularioPago.value = true;
  } else {
    mostrarFormularioPago.value = false;
  }
});

onMounted(async () => {
  await initStripe();
  await cargarCarrito();
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
    <div v-if="!cargando && (!carrito || carrito.Productos.length === 0)" class="text-center">
      <p class="text-gray-600">Tu carrito está vacío</p>
      <router-link to="/" class="text-blue-500 hover:underline mt-4 inline-block">
        Continuar comprando
      </router-link>
    </div>

    <!-- Contenido del carrito -->
    <div v-if="carrito && carrito.Productos.length > 0" class="grid grid-cols-1 md:grid-cols-3 gap-8">
      <!-- Lista de productos -->
      <div class="md:col-span-2">
        <div v-for="producto in carrito.Productos" :key="producto.IdProducto" 
             class="flex items-center justify-between border-b py-4">
          <div class="flex items-center">
            <img :src="producto.ImagenUrl" :alt="producto.Nombre" 
                 class="w-20 h-20 object-cover rounded">
            <div class="ml-4">
              <h3 class="font-semibold">{{ producto.Nombre }}</h3>
              <p class="text-gray-600">Cantidad: {{ producto.Cantidad }}</p>
              <p class="text-gray-600">Precio: ${{ producto.PrecioUnitario }}</p>
            </div>
          </div>
          <p class="font-semibold">${{ producto.Subtotal }}</p>
        </div>
      </div>

      <!-- Resumen y pago -->
      <div class="md:col-span-1">
        <div class="bg-gray-50 p-6 rounded-lg">
          <h2 class="text-xl font-semibold mb-4">Resumen del Pedido</h2>
          <div class="space-y-2 mb-4">
            <div class="flex justify-between">
              <span>Subtotal:</span>
              <span>${{ carrito?.Total }}</span>
            </div>
            <div class="flex justify-between font-semibold">
              <span>Total:</span>
              <span>${{ carrito?.Total }}</span>
            </div>
          </div>

          <!-- Formulario de pago con Stripe -->
          <div v-if="mostrarFormularioPago" class="mt-6">
            <div id="card-element" class="mb-4"></div>
            <button @click="procesarPago" 
                    :disabled="procesandoPago"
                    class="w-full bg-blue-600 text-white py-2 px-4 rounded hover:bg-blue-700 
                           disabled:bg-gray-400 disabled:cursor-not-allowed">
              {{ procesandoPago ? 'Procesando...' : 'Pagar Ahora' }}
            </button>
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