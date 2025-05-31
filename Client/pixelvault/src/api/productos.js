import api from './axiosInstance';

// Obtener todos los productos
export const getProductos = async () => {
    const response = await api.get("/productos");
    return response.data;
};

// Obtener un producto por ID
export const getProductoById = async (id) => {
    const response = await api.get(`/productos/${id}`);
    return response.data;
};

// Obtener productos por marca
export const getProductosByMarca = async (marcaId) => {
    const response = await api.get("/productos");
    return response.data.filter(producto => producto.idMarca === marcaId);
};

// Obtener productos por tipo
export const getProductosByTipo = async (tipoId) => {
    const response = await api.get("/productos");
    return response.data.filter(producto => producto.idTipo === tipoId);
};

// Obtener productos destacados
export const getProductosDestacados = async () => {
    const response = await api.get("/productos");
    return response.data.filter(producto => producto.destacado);
};

// Obtener marcas
export const getMarcas = async () => {
    const response = await api.get("/marcas");
    return response.data;
};

// Obtener una marca por ID
export const getMarcaById = async (id) => {
    const response = await api.get(`/marcas/${id}`);
    return response.data;
};

// Obtener tipos de producto
export const getTiposProducto = async () => {
    const response = await api.get("/tiposproductos");
    return response.data;
};
