import axios from 'axios';  

const api = axios.create({
  baseURL: 'https://localhost:5225/api', // con esa url llamamos a la api
});

export const login = async (email, password) => {
    const response = await api.post('/auth/login', {
        email, 
        password,
    })
    return response.data;
}    

export const register = async (datos) => {
    const response = await api.post('/auth/register', {
        datos
    })
    return response.data;
}