import axios from "axios";
import {useUserStore} from "../store/user";
import pinia from "../pinia";


const userStore = useUserStore(pinia);


const api = axios.create({
  baseURL: "http://localhost:5225/api", 
});

api.interceptors.request.use((config) => {
    const userStore = useUserStore();
    if (userStore.token) {
        config.headers.Authorization = `Bearer ${userStore.token}`;
    }   
    return config;
});

export const getProductos = async () => {
    const response = await api.get("/productos");
    return response.data;
}
