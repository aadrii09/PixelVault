import axios from 'axios';
import { loadStripe } from '@stripe/stripe-js';
import pinia from '../pinia';
import { useUserStore } from '../store/user';

const userStore = useUserStore(pinia);
const stripe = await loadStripe('pk_test_51RT8U9QfbUGb0jQPDFUoqI1nG4mqKUlzVcMDdMob5WDfEfFpl3UL5TTN3Qta0Sah3TTPgfvVIWWud5xxG4LLqvBf00RfCRCmAq');

const api = axios.create({
    baseURL: 'https://localhost:5225/api',
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

export const processPayment = async (amount) => {
    try {
        const { clientSecret } = await createPaymentIntent(amount);

        const { error, paymentIntent } = await stripe.confirmCardPayment(clientSecret, {
            payment_method: {
                card: elements.getElement('card'),
                billing_details: {
                    name: 'Nombre del Cliente'
                }
            }
        });

        if (error) {
            throw new Error(error.message);
        }

        const { success } = await verifyPayment(paymentIntent.id);
        return success;
    } catch (error) {
        console.error('Error en el proceso de pago:', error);
        throw error;
    }
};