import pinia from '../pinia';
import axios from 'axios';
import {useUserStore} from '../store/user';

const userStore = useUserStore(pinia);

const api = axios.create({  
baseURL: 'http://localhost:5225/api',
});

api.interceptors.request.use((config) => {
    if(userStore.token){
        config.headers.Authorization = `Bearer ${userStore.token}`;
    }
    return config;
});

export const crearOrder = async (total) => {
  const response = await api.post('/pagos/crear-orden', {
    total: total,
    currency: 'USD'
  });
  console.log("🧾 Respuesta al crear orden:", response.data);
  return response.data.orderId;
};


export const verificarOrden = async (orderId) => {
    const response = await api.post('/pagos/verificar-orden', {
        orderId: orderId
    });
    return response.data;
};

