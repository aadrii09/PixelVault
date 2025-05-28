import axios from 'axios';
import { loadStripe } from '@stripe/stripe-js';
import pinia from '../pinia';
import { useUserStore } from '../store/user';

const userStore = useUserStore(pinia);

const api = axios.create({
    baseURL: 'http://localhost:5225/api',
    headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${userStore.token}`
    }
});

export const createPaymentIntent = async (amount) => {
    const response = await api.post('/stripe/crear-intent', { amount });
    return response.data;
};

export const verifyPayment = async (paymentIntentId) => {
    const response = await api.post('/stripe/verificar-pago', { paymentIntentId });
    return response.data;
};

export const processPayment = async (amount, elements) => {
    try {
        console.log('🧾 Total a pagar:', carrito.value.Total);
        const stripe = await loadStripe('pk_test_51RTSVtQv5xnysdLKAXO57tv8Y5LvK4mP0SnkDl7zaCAE4fHP6c5bLoln640AzUs8gkJjlhFgiJZNXXv2w6TV7wRY00HlcD7Gfc');
        const { clientSecret } = await createPaymentIntent(amount);
        console.log('📨 ClientSecret recibido:', clientSecret);
        const { error, paymentIntent } = await stripe.confirmCardPayment(clientSecret, {
            payment_method: {
                card: elements.getElement('card'),
                billing_details: {
                    name: 'Nombre del Cliente'
                }
            }
        })
        console.log('💳 Confirmación de pago:', paymentIntent?.status);

        ;

        if (error) throw new Error(error.message);

        const { success } = await verifyPayment(paymentIntent.id);
        return success;
    } catch (error) {
        console.error('Error en el proceso de pago:', error);
        throw error;
    }
};