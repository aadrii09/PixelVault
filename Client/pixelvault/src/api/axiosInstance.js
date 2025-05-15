import axios from 'axios';
import { computed } from 'vue'

const api = axios.create({
  baseURL: 'http://localhost:5225/api', // Cambia al puerto real de tu backend
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor para añadir el token a cada petición
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;

export const updateUsuario = async (id, data) => (await api.put(`/Usuarios/${id}`, data)).data; 