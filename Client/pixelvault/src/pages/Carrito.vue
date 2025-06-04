<script setup>
import { ref, onMounted, nextTick, watch, inject } from 'vue';
import { getCarrito, vaciarCarrito, removeProducto, actualizarCantidad } from '../api/carrito';
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
const selectedPaymentMethod = ref('paypal'); // Default to paypal or stripe based on preference
const successMessage = ref(''); // New ref for success messages

// Inyectar la función de notificación desde el componente raíz
const showNotification = inject('showNotification', null);

// Inicializar Stripe
const initStripe = async () => {
  console.log('💳 Iniciando Stripe...');
  // Ensure the Stripe publishable key is correct
  const stripePublicKey = 'pk_test_51RT8U9QfbUGb0jQPDFUoqI1nG4mqKUlzVcMDdMob5WDfEfFpl3UL5TTN3Qta0Sah3TTPgfvVIWWud5xxG4LLqvBf00RfCRCmAq'; // Replace with your actual test key
  stripe.value = await loadStripe(stripePublicKey);
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
  // Only mount if the stripe-card-element div exists and Stripe is selected
  if (selectedPaymentMethod.value === 'stripe' && document.getElementById('stripe-card-element')) {
    try {
      cardElement.value.mount('#stripe-card-element');
      console.log('✅ Stripe card element montado en #stripe-card-element');
    } catch (e) {
      console.error('❌ Error mounting Stripe card element:', e);
    }
  }
};

// Cargar el carrito
const cargarCarrito = async () => {
  try {
    cargando.value = true;
    const response = await getCarrito();
    carrito.value = response;
    console.log('📦 Carrito recibido del backend:', carrito.value);
    console.log('🟢 Productos en carrito:', carrito.value.productos);
    if (carrito.value && carrito.value.productos) {
      // Ensure total is a number if it exists, otherwise default to 0
      carrito.value.total = parseFloat(carrito.value.total) || 0;
      // Ensure each product subtotal is a number
      if (carrito.value.productos && Array.isArray(carrito.value.productos)) {
        carrito.value.productos = carrito.value.productos.map(p => ({ ...p, subtotal: parseFloat(p.subtotal) || 0 }));
      }
    }
  } catch (err) {
    if (err.response && err.response.status === 404) {
      // Carrito no encontrado (vacío)
      carrito.value = { productos: [], total: 0 }; // structure for empty cart
      console.log('🟡 Carrito no encontrado, se considera vacío');
    } else {
      error.value = 'Error al cargar el carrito';
      console.error(err);
    }
  } finally {
    cargando.value = false;
  }
};

// Procesar el pago (unified function)
const handlePayment = async () => {
  if (!carrito.value || carrito.value.total <= 0) {
    error.value = 'El carrito está vacío';
    return;
  }

  if (procesandoPago.value) return; // Evitar doble clic

  try {
    procesandoPago.value = true;
    error.value = '';

    if (selectedPaymentMethod.value === 'stripe') {
      if (!stripe.value || !cardElement.value) {
        error.value = 'Stripe no inicializado correctamente.';
        procesandoPago.value = false;
        return;
      }      const success = await processPayment(carrito.value.total, elements.value);

      if (success) {
        await vaciarCarrito();
        successMessage.value = '¡Compra finalizada con éxito! Gracias por tu pedido.';
        if (showNotification) {
          showNotification('¡Compra finalizada con éxito! Gracias por tu pedido.', 'success', 5000);
        }
        // router.push({ name: 'PagoExitoso' }); // Descomenta si quieres redirigir
      } else {
        error.value = 'El pago con tarjeta no fue exitoso.';
      }

    } else if (selectedPaymentMethod.value === 'paypal') {
      error.value = 'Por favor, haz clic en el botón de PayPal para completar la compra.';
      procesandoPago.value = false;
      return;
    }

  } catch (err) {
    error.value = 'Error general al procesar el pago';
    console.error(err);
  } finally {
    if (selectedPaymentMethod.value === 'stripe') {
      procesandoPago.value = false;
    }
  }
};



const renderPayPalButton = () => {
  console.log('Attempting to render PayPal button...');
  const container = document.getElementById('paypal-button-container');
  if (!container) {
    console.warn('⚠️ Contenedor #paypal-button-container no encontrado en el DOM');
    // You might want to retry rendering later if the element isn't ready
    return;
  }

  // Clear previous PayPal buttons if any to avoid duplicates
  container.innerHTML = '';

  if (window.paypal && window.paypal.Buttons) {
    console.log('🟢 SDK de PayPal disponible. Renderizando botón.');
    window.paypal.Buttons({
      createOrder: async (data, actions) => {
        try {
          console.log('Creating PayPal order...');
          // Pass the total amount to your backend API to create a PayPal order
          const orderId = await crearOrder(carrito.value.total); // Use carrito.value.total
          console.log("🧾 ID de orden de PayPal generado:", orderId);
          return orderId;
        } catch (err) {
          console.error('❌ Error creando orden de PayPal:', err);
          error.value = 'Error al crear la orden de PayPal';
          // Inform PayPal.js that there was an error
          return actions.reject(err);
        }
      },

      onApprove: async (data, actions) => {
        console.log('PayPal payment approved.', data);
        try {
          const result = await verificarOrden(data.orderID);
          console.log('PayPal order verification result:', result);

          if (result.success) {
            console.log('✅ Orden verificada exitosamente con PayPal. Vaciar carrito...');
            await vaciarCarrito();
            console.log('✅ Carrito vaciado (backend). Actualizando datos en frontend...');
            await cargarCarrito();            successMessage.value = '¡Compra finalizada con éxito! Gracias por tu pedido.';
            console.log('✅ Mensaje de éxito actualizado en la vista.');
            if (showNotification) {
              showNotification('¡Compra finalizada con éxito! Gracias por tu pedido.', 'success', 5000);
            }

          } else {
            error.value = result.message || 'No se pudo verificar el pago con PayPal';
            console.error('❌ Error en verificación de orden:', error.value);
          }
        } catch (err) {
          console.error('❌ Error verificando orden de PayPal:', err);
          error.value = 'Error al verificar el pago con PayPal';
        }
      },

      onError: (err) => {
        console.error('❌ Error en PayPal:', err);
        error.value = 'Error con PayPal. Por favor, intenta de nuevo.';
      },
      onCancel: (data) => {
        console.log('PayPal payment cancelled.', data);
        error.value = 'Pago de PayPal cancelado.';
      }

    }).render('#paypal-button-container');
  } else {
    console.warn('⚠️ SDK de PayPal o PayPal.Buttons no disponible para renderizar el botón.');
  }
};

// Watch for changes in selectedPaymentMethod to initialize/render payment UIs
watch(selectedPaymentMethod, async (newMethod, oldMethod) => {
  console.log(`Selected payment method changed from ${oldMethod} to:`, newMethod);
  await nextTick(); // Ensure DOM is updated before mounting/unmounting

  // Clear previous errors related to payment method selection
  if (error.value.includes('Por favor, selecciona un método de pago') || error.value.includes('Stripe no inicializado')) {
    error.value = '';
  }

  if (newMethod === 'stripe') {
    console.log('Switching to Stripe.');
    // Clear PayPal container
    const paypalContainer = document.getElementById('paypal-button-container');
    if (paypalContainer) paypalContainer.innerHTML = '';
    // Initialize and mount Stripe if not already done and element exists
    if (!stripe.value) {
      console.log('Stripe not initialized, initializing...');
      await initStripe();
    }
    // Mount Stripe element after initialization if it exists
    if (cardElement.value && document.getElementById('stripe-card-element')) {
      try {
        cardElement.value.mount('#stripe-card-element');
        console.log('✅ Stripe card element montado en #stripe-card-element (via watch).');
      } catch (e) {
        console.error('❌ Error mounting Stripe card element via watch:', e);
      }
    } else {
      console.warn('⚠️ Stripe card element div #stripe-card-element not found or cardElement not created.');
    }

  } else if (newMethod === 'paypal') {
    console.log('Switching to PayPal.');
    // Destroy Stripe element if it was mounted
    if (cardElement.value) {
      try {
        cardElement.value.unmount();
        console.log('✅ Stripe card element desmontado (via watch).');
      } catch (e) {
        console.warn('⚠️ Error unmounting Stripe card element:', e);
      }
    }
    // Render PayPal button if SDK is loaded, otherwise load it
    if (window.paypal && window.paypal.Buttons) {
      console.log('PayPal SDK ready, rendering button.');
      renderPayPalButton();
    } else if (!document.getElementById("paypal-sdk")) {
      // If SDK is not loaded, load it, and it will render on load
      console.log('📦 SDK de PayPal aún no cargado. Añadiendo script...');
      const script = document.createElement("script");
      script.src = "https://www.paypal.com/sdk/js?client-id=AQiepbT5Qot4jIqhxfcUppb-ogD3WfqkZZpRi7IQvoE-eDsjVaO0aOyEnaWjwC5WxJOHyJHNwveYWddr&currency=USD"; // Ensure correct client ID and currency
      script.id = "paypal-sdk";
      // Use onload to ensure renderPayPalButton is called after the script is loaded
      script.onload = () => {
        console.log('✅ SDK de PayPal cargado y listo (via watch).');
        renderPayPalButton();
      };
      script.onerror = (e) => {
        console.error('❌ Error loading PayPal SDK:', e);
        error.value = 'Error al cargar el SDK de PayPal.';
      };
      document.body.appendChild(script);
    } else {
      console.warn('⚠️ PayPal SDK script already exists but window.paypal is not ready or does not have Buttons.');
      // Attempt to render just in case it became ready
      renderPayPalButton();
    }
  } else {
    // Handle case where no payment method is selected (optional)
    console.warn('No payment method selected.');
    // Clear both containers
    const paypalContainer = document.getElementById('paypal-button-container');
    if (paypalContainer) paypalContainer.innerHTML = '';
    if (cardElement.value) {
      try {
        cardElement.value.unmount();
        console.log('✅ Stripe card element desmontado (no method selected).');
      } catch (e) {
        console.warn('⚠️ Error unmounting Stripe card element:', e);
      }
    }
    error.value = 'Por favor, selecciona un método de pago.';
  }
}, { immediate: true }); // immediate: true to run on component mount

// Observar cambios en el carrito para mostrar formulario de pago
watch(carrito, (newVal) => {
  console.log('🔁 Carrito actualizado para mostrar formulario de pago:', newVal);
  // Ensure total is a number before checking
  const total = parseFloat(newVal?.total) || 0;
  mostrarFormularioPago.value = total > 0;
  // The payment method watcher will handle initializing/rendering based on selectedPaymentMethod
  // when mostrarFormularioPago becomes true.
}, { immediate: true }); // Immediate: true to run on initial load

// Observar cambios en mostrarFormularioPago para renderizar el botón de PayPal cuando sea necesario
watch(mostrarFormularioPago, (isVisible) => {
  console.log('👀 mostrarFormularioPago cambió a:', isVisible);
  
  // Si el formulario de pago es visible y PayPal está seleccionado, renderizar el botón
  if (isVisible && selectedPaymentMethod.value === 'paypal') {
    console.log('🔄 Se debe renderizar el botón de PayPal debido al cambio en mostrarFormularioPago');
    nextTick(() => {
      renderPayPalButton();
    });
  }
}, { immediate: false }); // No necesitamos que se ejecute inmediatamente ya que el watcher de selectedPaymentMethod ya lo cubre

// Initial load of the cart data
onMounted(async () => {
  // Load cart data
  await cargarCarrito();
  
  // Make sure PayPal loads immediately if it's the default option
  if (selectedPaymentMethod.value === 'paypal' && mostrarFormularioPago.value) {
    // If PayPal SDK is already loaded
    if (window.paypal && window.paypal.Buttons) {
      console.log('PayPal SDK already loaded on mount, rendering button...');
      renderPayPalButton();
    } 
    // If SDK is not loaded yet, load it
    else if (!document.getElementById("paypal-sdk")) {
      console.log('Loading PayPal SDK on mount...');
      const script = document.createElement("script");
      script.src = "https://www.paypal.com/sdk/js?client-id=AQiepbT5Qot4jIqhxfcUppb-ogD3WfqkZZpRi7IQvoE-eDsjVaO0aOyEnaWjwC5WxJOHyJHNwveYWddr&currency=USD";
      script.id = "paypal-sdk";
      script.onload = () => {
        console.log('PayPal SDK loaded on mount, rendering button...');
        renderPayPalButton();
      };
      script.onerror = (e) => {
        console.error('Error loading PayPal SDK:', e);
        error.value = 'Error al cargar el método de pago de PayPal.';
      };
      document.body.appendChild(script);
    }
  }
});

// You will need to implement this method to handle quantity updates
const updateCantidad = async (producto) => {
  console.log('Update quantity for product:', producto);

  try {
    const success = await actualizarCantidad(producto.idProducto, producto.cantidad);
    if (success) {
      console.log('✅ Cantidad actualizada en backend. Recargando carrito...');
      await cargarCarrito();
      
      // Re-render PayPal button if PayPal is the selected payment method
      // Usamos nextTick para asegurar que el DOM se actualice antes de renderizar
      if (selectedPaymentMethod.value === 'paypal' && mostrarFormularioPago.value) {
        console.log('Re-rendering PayPal button after quantity update');
        nextTick(() => {
          renderPayPalButton();
        });
      }
    } else {
      error.value = 'No se pudo actualizar la cantidad';
      console.error('❌ No se pudo actualizar la cantidad en el backend.');
      if (showNotification) {
        showNotification('No se pudo actualizar la cantidad', 'error', 3000);
      }
    }
  } catch (err) {
    error.value = 'Error al actualizar la cantidad';
    console.error('❌ Error al actualizar la cantidad:', err);
    if (showNotification) {
      showNotification('Error al actualizar la cantidad', 'error', 3000);
    }
  }
};


// Función para eliminar un producto individual
const eliminarProducto = async (productoId) => {
  try {
    const success = await removeProducto(productoId);
    if (success) {
      await cargarCarrito(); // Recargar el carrito después de eliminar
      
      // Re-render PayPal button if PayPal is the selected payment method
      if (selectedPaymentMethod.value === 'paypal' && mostrarFormularioPago.value) {
        console.log('Re-rendering PayPal button after product removal');
        nextTick(() => {
          renderPayPalButton();
        });
      }
    } else {
      error.value = 'Error al eliminar el producto';
      if (showNotification) {
        showNotification('Error al eliminar el producto', 'error', 3000);
      }
    }
  } catch (err) {
    error.value = 'Error al eliminar el producto';
    console.error(err);
    if (showNotification) {
      showNotification('Error al eliminar el producto', 'error', 3000);
    }
  }
};

// Función para vaciar todo el carrito
const vaciarTodoCarrito = async () => {
  try {
    const success = await vaciarCarrito();
    if (success) {
      await cargarCarrito(); // Recargar el carrito después de vaciar
      successMessage.value = '¡El carrito se vació por completo!';
      
      // Mostrar notificación bonita
      if (showNotification) {
        showNotification('¡El carrito se vació por completo!', 'success', 5000);
      }
      
      // Clear the success message after 5 seconds
      setTimeout(() => {
        successMessage.value = '';
      }, 5000);
      
      // No es necesario renderizar el botón PayPal si el carrito está vacío,
      // ya que mostrarFormularioPago será false y ocultará la sección de pago
    } else {
      error.value = 'Error al vaciar el carrito';
      if (showNotification) {
        showNotification('Error al vaciar el carrito', 'error', 5000);
      }
    }
  } catch (err) {
    error.value = 'Error al vaciar el carrito';
    console.error(err);
    if (showNotification) {
      showNotification('Error al vaciar el carrito', 'error', 5000);
    }
  }
};

</script>

<template>  <div class="flex justify-center items-center min-h-screen py-8 carrito-background">
    <div class="w-full max-w-4xl bg-gradient-to-br from-[#161630] to-[#10102a] p-8 rounded-lg shadow-[0_0_20px_rgba(0,204,255,0.3)] border border-[rgba(0,204,255,0.2)] backdrop-blur-sm grid grid-cols-1 md:grid-cols-2 gap-8">

      <!-- Left Column: CARRITO -->
      <div>
        <h2 class="text-2xl font-bold mb-6 text-[#00ccff]">CARRITO</h2>

        <!-- Mensaje de carga -->
        <div v-if="cargando" class="text-center text-gray-300">
          <p>Cargando carrito...</p>
        </div>        <!-- Mensaje de error for cart loading -->
        <div v-if="error && !cargando && (!carrito || !carrito.productos || carrito.productos.length === 0)"
          class="bg-[rgba(255,70,70,0.15)] border border-[rgba(255,60,60,0.3)] text-red-300 px-4 py-3 rounded mb-4">
          {{ error }}
        </div>

        <!-- Mensaje de éxito -->
        <div v-if="successMessage" class="bg-[rgba(0,255,136,0.15)] border border-[rgba(0,255,136,0.3)] text-[#00ff88] px-4 py-3 rounded mb-4">
          {{ successMessage }}
        </div>

        <!-- Carrito vacío -->
        <div v-if="!cargando && (!carrito || !carrito.productos || carrito.productos.length === 0)">
          <p class="text-gray-300">Tu carrito está vacío</p>
          <router-link to="/" class="text-[#00ccff] hover:text-[#00ff88] hover:underline transition-colors mt-4 inline-block">
            Continuar comprando
          </router-link>
        </div>

        <!-- Lista de productos -->
        <div v-if="carrito && carrito.productos.length > 0" class="space-y-4">
          <div v-for="producto in carrito.productos" :key="producto.idProducto"
            class="flex items-center justify-between border-b border-[rgba(0,204,255,0.2)] pb-4">
            <div class="flex items-center">
              <img :src="producto.imagenUrl" :alt="producto.nombre" class="w-16 h-16 object-contain rounded mr-4 bg-[rgba(255,255,255,0.05)] p-1">
              <div class="flex-1">
                <h3 class="font-semibold text-white">{{ producto.nombre }}</h3>
                <!-- Quantity Input -->
                <div class="flex items-center mt-1">
                  <label :for="'cantidad-' + producto.idProducto" class="text-gray-300 text-sm mr-2">Cantidad:</label>
                  <input :id="'cantidad-' + producto.idProducto" type="number" v-model.number="producto.cantidad"
                    min="1" class="w-12 p-1 border border-[rgba(0,204,255,0.3)] bg-[rgba(0,0,30,0.3)] rounded text-center text-white"
                    @change="updateCantidad(producto)">
                </div>
              </div>
            </div>
            <div class="flex items-center space-x-4">
              <button @click="eliminarProducto(producto.idProducto)" class="text-red-400 hover:text-red-300 transition-colors">
                <i class="fas fa-trash"></i>
              </button>
              <p class="font-semibold text-[#00ff88]">${{ producto.subtotal?.toFixed(2) ?? '0.00' }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Right Column: COMPRA -->
      <div>
        <div class="flex justify-between items-center mb-6">
          <h2 class="text-2xl font-bold text-[#00ccff]">COMPRA</h2>
          <button @click="vaciarTodoCarrito"
            class="bg-gradient-to-r from-red-600 to-red-500 text-white px-4 py-2 rounded hover:brightness-110 transition shadow-md shadow-red-900/30">
            Vaciar Carrito
          </button>
        </div>

        <div v-if="!cargando && carrito && carrito.total > 0" class="bg-[rgba(0,0,30,0.3)] backdrop-blur-sm border border-[rgba(0,204,255,0.15)] p-6 rounded-lg space-y-4">          <!-- Método de pago -->
          <div>
            <label for="payment-method" class="block text-sm font-medium text-gray-200 mb-2">Método de pago</label>
            <select id="payment-method" v-model="selectedPaymentMethod"
              class="mt-1 block w-full pl-3 pr-10 py-2 text-base bg-[rgba(10,10,40,0.8)] border-[rgba(0,204,255,0.3)] text-white focus:outline-none focus:ring-[#00ccff] focus:border-[#00ccff] sm:text-sm rounded-md">
              <option value="paypal">PayPal</option>
              <option value="stripe">Tarjeta de Crédito/Débito</option>
            </select>
          </div>

          <!-- Payment Forms (Conditionally rendered) -->
          <div v-if="mostrarFormularioPago">
            <!-- PayPal Payment Section -->
            <div v-if="selectedPaymentMethod === 'paypal'">
              <div class="mb-4">
                <div class="flex justify-between items-center">
                  <h3 class="text-md font-medium text-white">Pagar con PayPal</h3>
                  <img src="/images/paypallogo.png" alt="PayPal" class="h-5 object-contain" />
                </div>
                <div id="paypal-button-container" class="mt-2"></div>
                <p class="text-xs text-gray-400 mt-1 text-center">Desarrollado por <span class="text-blue-400">Pay</span><span class="text-blue-600">Pal</span></p>
              </div>
            </div>
            
            <!-- Stripe Card Element Container -->
            <div v-if="selectedPaymentMethod === 'stripe'">
              <label for="stripe-card-element" class="block text-sm font-medium text-gray-200 mb-2">Detalles de la tarjeta</label>
              <div id="stripe-card-element" class="p-3 border border-[rgba(0,204,255,0.3)] rounded-md bg-[rgba(10,10,40,0.8)]"></div>
              <div class="flex items-center justify-center mt-3">
                <button class="flex items-center justify-center bg-slate-800 hover:bg-slate-700 text-white py-2 px-4 rounded-md shadow-md transition-colors">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 10h18M7 15h1m4 0h1m-7 4h12a3 3 0 003-3V8a3 3 0 00-3-3H6a3 3 0 00-3 3v8a3 3 0 003 3z" />
                  </svg>
                  Tarjeta de débito o crédito
                </button>
              </div>
            </div>
          </div>

          <!-- Código Descuento -->
          <div>
            <label for="discount-code" class="block text-sm font-medium text-gray-200 mb-2">CÓDIGO DESCUENTO</label>
            <input type="text" id="discount-code" placeholder="kkk-kkk-kk"
              class="mt-1 block w-full p-2 border border-[rgba(0,204,255,0.3)] bg-[rgba(10,10,40,0.8)] rounded-md shadow-sm focus:outline-none focus:ring-[#00ccff] focus:border-[#00ccff] sm:text-sm text-white">
          </div>

          <!-- Total Summary -->
          <div class="border-t border-[rgba(0,204,255,0.3)] pt-4 mt-4 space-y-2">
            <div class="flex justify-between text-lg font-semibold">
              <span class="text-white">Total:</span>
              <span class="text-[#00ff88] text-xl">${{ carrito?.total?.toFixed(2) ?? '0.00' }}</span>
            </div>
          </div>          <!-- COMPRAR Button (only shown for Stripe, since PayPal has its own button) -->
          <button v-if="selectedPaymentMethod === 'stripe'" @click="handlePayment" :disabled="procesandoPago || (!carrito || carrito.total <= 0)"
            class="w-full mt-6 bg-gradient-to-r from-[#00ccff] to-[#00ff88] text-white py-3 px-4 rounded-md font-semibold text-lg hover:brightness-110 transition-all shadow-lg shadow-[rgba(0,204,255,0.3)]
                     disabled:opacity-50 disabled:cursor-not-allowed border border-transparent focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[#00ccff]">
            {{ procesandoPago ? 'Procesando...' : 'COMPRAR' }}
          </button>
          
          <p v-if="selectedPaymentMethod === 'paypal'" class="text-center text-sm text-gray-400 mt-4">
            Haz clic en el botón de PayPal para completar tu compra
          </p>
        </div>
      </div>
    </div>
  </div>
</template>


<style scoped>
/* Add any specific styles here if needed */

/* Styling for elements */
#stripe-card-element {
  min-height: 40px;
  margin-bottom: 1rem;
}

#paypal-button-container {
  margin: 0.5rem 0;
  min-height: 45px;
  width: 100%;
}

.carrito-background {
  background-color: #f0f0f0;
  /* Fallback background color */
  background-image: url('../../public/images/fondocarrito.svg');
  /* **REPLACE with your image path** */
  background-size: cover;
  /* Cover the entire background */
  background-position: center;
  /* Center the background image */
  background-repeat: no-repeat;
  /* Do not repeat the image */
  /* background-attachment: fixed; Uncomment this line if you want the background to be fixed when scrolling */
}
</style>