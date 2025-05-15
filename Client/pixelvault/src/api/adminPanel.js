// src/api/adminPanel.js
import api from './axiosInstance';

// Marcas
export const getMarcas = async () => (await api.get('/Marcas')).data;
export const createMarca = async (data) => (await api.post('/Marcas', data)).data;
export const updateMarca = async (id, data) => (await api.put(`/Marcas/${id}`, data)).data;
export const deleteMarca = async (id) => (await api.delete(`/Marcas/${id}`)).data;

// Productos
export const getProductos = async () => (await api.get('/Productos')).data;
export const createProducto = async (data) => (await api.post('/Productos', data)).data;
export const updateProducto = async (id, data) => (await api.put(`/Productos/${id}`, data)).data;
export const deleteProducto = async (id) => (await api.delete(`/Productos/${id}`)).data;

// Tipos de Productos
export const getTiposProductos = async () => (await api.get('/TiposProductos')).data;
export const createTipoProducto = async (data) => (await api.post('/TiposProductos', data)).data;
export const updateTipoProducto = async (id, data) => (await api.put(`/TiposProductos/${id}`, data)).data;
export const deleteTipoProducto = async (id) => (await api.delete(`/TiposProductos/${id}`)).data;

// Pedidos
export const getPedidos = async () => (await api.get('/Pedidos/todos(admin)')).data;

// Pagos
export const getPagos = async () => []; // Implementa si tienes endpoint para listar pagos

// Usuarios
export const getUsuarios = async () => (await api.get('/Usuarios')).data;
export const getUsuarioDetalle = async (id) => (await api.get(`/Usuarios/${id}`)).data;
export const updateUsuarioRol = async (id, esAdmin) => (await api.put(`/Usuarios/actualizarRol/${id}`, esAdmin)).data;
export const deleteUsuario = async (id) => (await api.delete(`/Usuarios/${id}`)).data;
export const updateUsuario = async (id, data) => (await api.put(`/Usuarios/${id}`, data)).data;