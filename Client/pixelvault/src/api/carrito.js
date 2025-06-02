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
  try {
    const response = await api.get(`/Carritos/${userId}`);
    const data = response.data;

    console.log("📥 Backend respondió carrito:", data);

    // Verifica si contiene productos antes de mapear
    // Backend might return { Productos: [] } or { productos: [] }
    const productos = data.Productos || data.productos;

    if (!productos || !Array.isArray(productos)) {
      // If products is null, undefined, or not an array, treat as empty cart scenario (status 404 handled in Vue)
      console.log("🟡 Backend response indicates no products or invalid format.", data);
      // Depending on backend, you might return empty array or null, or throw error.
      // Let's rely on the 404 handling in Vue for empty, but handle non-array if 200 is returned with no products field.
      if (response.status === 200) {
        return { ...data, productos: [], total: 0 }; // Return empty structure if 200 but no products
      }
      // If not 200, let the Vue catch handle it (e.g., 404)
      throw new Error("❌ Formato de respuesta de carrito inválido");
    }

    return {
      ...data,
      productos: productos.map(p => ({ ...p, subtotal: parseFloat(p.subtotal) || 0, precioUnitario: parseFloat(p.precioUnitario) || 0 })), // Ensure numbers
      total: parseFloat(data.Total || data.total) || 0 // Ensure total is a number
    };
  } catch (error) {
    console.error('❌ Error al obtener carrito:', error.response?.status, error.message);
    // Rethrow to be handled by Vue component, which checks for 404
    throw error;
  }
};


export const vaciarCarrito = async () => {
  const userId = userStore.user.id;
  console.log(`🗑️ Intentando vaciar carrito para usuario ${userId}`);
  try {
    const response = await api.delete(`/Carritos/${userId}/vaciar`);
    console.log(`✅ Respuesta del backend al vaciar carrito:`, response);
    console.log(`✅ Status del backend al vaciar carrito: ${response.status}`);
    // Backend returns 204 (NoContent) or 200 (Ok) on success
    return response.status === 204 || response.status === 200;
  } catch (error) {
    console.error('❌ Error al llamar al backend para vaciar carrito:', error);
    console.error('❌ Detalles del error al vaciar carrito:', error.response);
    return false;
  }
}

export const removeProducto = async (productoId) => {
  try {
    const userId = userStore.user.id;
    console.log(`🗑️ Eliminando producto ${productoId} del carrito para usuario ${userId}`);

    const response = await api.delete(`/Carritos/${userId}/${productoId}`);
    console.log(`✅ Respuesta del backend al eliminar producto: Status ${response.status}`);
    // Backend returns 204 (NoContent) or 200 (Ok) on success
    return response.status === 204 || response.status === 200;
  } catch (error) {
    console.error(`❌ Error al llamar al backend para eliminar producto ${productoId}:`, error.response?.status, error.message);
    return false;
  }
}

export const actualizarCantidad = async (idProducto, cantidad) => {
  const userId = userStore.user.id;
  console.log(`🔄 Actualizando cantidad: Producto ${idProducto}, cantidad ${cantidad} para usuario ${userId}`);

  const response = await api.patch(`/Carritos/${userId}/productos/${idProducto}`, cantidad, {
    headers: {
      'Content-Type': 'application/json'
    }
  });

  console.log('✅ Cantidad actualizada correctamente', response.data);
  return response.status === 200;
};
