import axios from "axios";
import pinia from '../pinia';
import { useUserStore } from "../store/user";

const userStore = useUserStore(pinia);



const api = axios.create({
  baseURL: "http://localhost:5225/api", // con esa url llamamos a la api
});

api.interceptors.request.use((config) => {
  if (userStore.token) {
    config.headers.Authorization = `Bearer ${userStore.token}`;
  }
  console.log('🟢 Token actual:', userStore.token)
  console.log('🟢 ID usuario actual:', userStore.user?.id)

  return config;
});

export const agregarAlCarrito = async (producto) => {
  const userId = userStore.user.id;
  const body = {
    idProducto: producto.idProducto,
    cantidad: 1,
    precioUnitario: 49.99
  }
  await api.post(`/Carritos/${userId}`, body)
}

export const getCarrito = async () => {
  const userId = userStore.user.id;
  const response = await api.get(`/Carritos/${userId}`);
  return response.data;
}

export const vaciarCarrito = async () => {
  const userId = userStore.user.id;
  await api.delete(`/Carritos/${userId}/vaciar`);
}

export const actualizarCantidad = async (idProducto, cantidad) => {
  try {
    const userId = userStore.user.id;
    console.log(`🔄 Actualizando cantidad: Producto ${idProducto}, cantidad ${cantidad}`);
    
    // Asegurarnos que la cantidad sea al menos 1
    cantidad = Math.max(1, cantidad);
    
    // Ajustando al endpoint real según los errores de la consola
    const response = await api.patch(`/Carritos/${userId}/productos/${idProducto}`, { 
      cantidad 
    });
    
    console.log('✅ Cantidad actualizada correctamente', response.data);
    return response.data;
  } catch (error) {
    console.error('❌ Error al actualizar cantidad:', error);
    throw error;
  }
}