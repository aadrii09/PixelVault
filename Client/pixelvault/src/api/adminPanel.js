// src/api/adminPanel.js

import axios from 'axios';

// Marcas
export const getMarcas = async () => (await axios.get('/api/Marcas')).data;
export const createMarca = async (data) => (await axios.post('/api/Marcas', data)).data;
export const updateMarca = async (id, data) => (await axios.put(`/api/Marcas/${id}`, data)).data;
export const deleteMarca = async (id) => (await axios.delete(`/api/Marcas/${id}`)).data;

// Productos
export const getProductos = async () => (await axios.get('/api/Productos')).data;
export const createProducto = async (data) => (await axios.post('/api/Productos', data)).data;
export const updateProducto = async (id, data) => (await axios.put(`/api/Productos/${id}`, data)).data;
export const deleteProducto = async (id) => (await axios.delete(`/api/Productos/${id}`)).data;

// Tipos de Productos
export const getTiposProductos = async () => (await axios.get('/api/TiposProductos')).data;
export const createTipoProducto = async (data) => (await axios.post('/api/TiposProductos', data)).data;
export const updateTipoProducto = async (id, data) => (await axios.put(`/api/TiposProductos/${id}`, data)).data;
export const deleteTipoProducto = async (id) => (await axios.delete(`/api/TiposProductos/${id}`)).data;

// Pedidos
export const getPedidos = async () => (await axios.get('/api/Pedidos/todos(admin)')).data;

// Pagos
export const getPagos = async () => []; // Implementa si tienes endpoint para listar pagos

// Usuarios
export const getUsuarios = async () => (await axios.get('/api/Usuarios')).data;
export const getUsuarioDetalle = async (id) => (await axios.get(`/api/Usuarios/${id}`)).data;
export const updateUsuarioRol = async (id, data) => (await axios.put(`/api/Usuarios/actualizarRol/${id}`, data)).data;
export const deleteUsuario = async (id) => (await axios.delete(`/api/Usuarios/${id}`)).data;