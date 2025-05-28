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
  console.log('🟢 Token y usuario en headers:', config.headers.Authorization, userStore.user?.id);

  return config;
});

export async function agregarAlCarrito(producto) {
  const token = localStorage.getItem("token");
  const payload = JSON.parse(atob(token.split('.')[1]));
  const userId = payload.sub;
  localStorage.setItem("userId", payload.sub);

  console.log("🟢 Token actual:", token);
  console.log("🟢 ID usuario actual:", userId);
  console.log("🟢 Token y usuario en headers:", `Bearer ${token}`, userId);

  const response = await axios.post(
    `http://localhost:5225/api/Carritos/${userId}`,
    producto,
    {
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    }
  );

  return response.data;
}

export const getCarrito = async () => {
  const userId = userStore.user.id;
  const response = await api.get(`/Carritos/${userId}`);
  const data = response.data;

  console.log("📥 Backend respondió carrito:", data);

  // Verifica si contiene productos antes de mapear
  if (!data.productos && !data.Productos) {
    throw new Error("❌ El carrito no contiene productos");
  }

  return {
    ...data,
    productos: data.Productos || data.productos,
    total: data.Total || data.total
  };
};


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