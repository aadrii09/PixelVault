import axios from 'axios';  

const api = axios.create({
  baseURL: 'https://localhost:5225/api', // con esa url llamamos a la api
    headers: {
        'Content-Type': 'application/json',
    },
});

export const login = async (email, password) => {
    const response = await api.post('/Auth/login', {
        email, 
        password,
    })
    return response.data;
}    

export const register = async (nombre, apellidos, email, password ) => {
    const response = await api.post('/Auth/register', {
        nombre,
        apellidos,
        email,
        password,
    })
    return response.data;
}