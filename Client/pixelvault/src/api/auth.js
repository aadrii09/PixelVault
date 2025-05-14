import axios from 'axios';  

const api = axios.create({
  baseURL: 'https://localhost:5225/api', // con esa url llamamos a la api
    headers: {
        'Content-Type': 'application/json',
    },
});

export async function login(email, password) {
  const response = await axios.post('http://localhost:5225/api/Auth/login', {
    email,
    password
  });
  return response.data;
} 
export async function register(nombre, apellidos, email, password) {
  const response = await axios.post('http://localhost:5225/api/auth/register', {
    nombre,
    apellidos,
    email,
    password
  });
  return response.data;
}