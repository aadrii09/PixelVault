import axios from "axios";
import pinia from '../pinia';
import { useUserStore } from "../store/user";

const userStore = useUserStore(pinia);

const api = axios.create({
  baseURL: "https://localhost:5225/api", // con esa url llamamos a la api
});

api.interceptors.request.use((config) => {
  if (userStore.token) {
    config.headers.Authorization = `Bearer ${userStore.token}`;
  }
  return config;    
});

export const getCarrito = async () => {
const userId = userStore.user.id;
const response = await api.get(`/Carritos/${userId}`);
return response.data;
}

export const vaciarCarrito = async () => {
const userId = userStore.user.id;
await api.delete(`/Carritos/${userId}/vaciar`);
}