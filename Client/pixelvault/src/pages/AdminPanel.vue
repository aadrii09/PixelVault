<script setup>
import { ref, watch, onMounted, computed } from 'vue';
import * as adminPanel from '../api/adminPanel';
import axios from 'axios';
import { useRouter } from 'vue-router';

// Tabs
const tab = ref('marcas');

// --- Marcas ---
const marcas = ref([]);
const nuevaMarca = ref({ nombre: '', logoUrl: '', website: '' });
const marcaEdit = ref(null);

const api = axios.create({
  baseURL: 'http://localhost:5225/api', 
  headers: {
    'Content-Type': 'application/json',
  },
});

const cargarMarcas = async () => { marcas.value = await adminPanel.getMarcas(); };
const crearMarca = async () => {
  await adminPanel.createMarca(nuevaMarca.value);
  nuevaMarca.value = { nombre: '', logoUrl: '', website: '' };
  await cargarMarcas();
};
const actualizarMarca = async (id, data) => {
  await adminPanel.updateMarca(id, data);
  marcaEdit.value = null;
  await cargarMarcas();
};
const eliminarMarca = async (id) => {
  await adminPanel.deleteMarca(id);
  await cargarMarcas();
};

// --- Usuarios ---
const usuarios = ref([]);
const usuarioDetalle = ref(null);
const usuarioIdBuscar = ref('');
const usuarioRolEdit = ref(null);
const usuarioEdit = ref(null);

const cargarUsuarios = async () => {
  try {
    usuarios.value = await adminPanel.getUsuarios();
    // console.log('Usuarios:', usuarios.value); // Descomenta para depurar
  } catch (e) {
    usuarios.value = [];
  }
};

const buscarUsuarioPorId = async () => {
  if (!usuarioIdBuscar.value) return;
  usuarioDetalle.value = await adminPanel.getUsuarioDetalle(usuarioIdBuscar.value);
};

const eliminarUsuario = async (id) => {
  try {
    await adminPanel.deleteUsuario(id);
    await cargarUsuarios();
  } catch (error) {
    if (error.response && error.response.status === 400) {
      showError('¡No puedes eliminarte! Eres el único administrador.');
    }
  }
};

const actualizarRolUsuario = async (id, esAdmin) => {
  try {
    await adminPanel.updateUsuarioRol(id, esAdmin);
    usuarioRolEdit.value = null;
    await cargarUsuarios();
  } catch (error) {
    if (error.response && error.response.status === 400) {
      showError('¡No puedes quitarte el permiso de admin! Eres el único administrador.');
    }
  }
};

const actualizarUsuario = async (id, data) => {
  await adminPanel.updateUsuario(id, data);
  usuarioEdit.value = null;
  await cargarUsuarios();
};

// --- Productos ---
const productos = ref([]);
const nuevoProducto = ref({ nombre: '', precio: 0 });
const productoEdit = ref(null);
const productoIdBuscar = ref('');
const productoDetalle = ref(null);

const cargarProductos = async () => { productos.value = await adminPanel.getProductos(); };
const crearProducto = async () => {
  await adminPanel.createProducto(nuevoProducto.value);
  nuevoProducto.value = { nombre: '', precio: 0 };
  await cargarProductos();
};
const actualizarProducto = async (id, data) => {
  await adminPanel.updateProducto(id, data);
  productoEdit.value = null;
  await cargarProductos();
};
const eliminarProducto = async (id) => {
  await adminPanel.deleteProducto(id);
  await cargarProductos();
};
const buscarProductoPorId = async () => {
  productoDetalle.value = await adminPanel.getProductoById(productoIdBuscar.value);
};

// --- Tipos de Productos ---
const tiposProductos = ref([]);
const nuevoTipoProducto = ref({ nombre: '', descripcion: '' });
const tipoProductoEdit = ref(null);

const cargarTiposProductos = async () => { tiposProductos.value = await adminPanel.getTiposProductos(); };
const crearTipoProducto = async () => {
  await adminPanel.createTipoProducto(nuevoTipoProducto.value);
  nuevoTipoProducto.value = { nombre: '', descripcion: '' };
  await cargarTiposProductos();
};
const actualizarTipoProducto = async (id, data) => {
  await adminPanel.updateTipoProducto(id, data);
  tipoProductoEdit.value = null;
  await cargarTiposProductos();
};
const eliminarTipoProducto = async (id) => {
  await adminPanel.deleteTipoProducto(id);
  await cargarTiposProductos();
};

// --- Carga dinámica por tab ---
const cargarTab = async () => {
  if (tab.value === 'marcas') await cargarMarcas();
  if (tab.value === 'usuarios') await cargarUsuarios();
  if (tab.value === 'productos') await cargarProductos();
  if (tab.value === 'tiposProductos') await cargarTiposProductos();
  // Aquí puedes añadir productos, tiposProductos, pedidos, pagos...
};

watch(tab, cargarTab, { immediate: true });

const cargando = ref(true);

onMounted(async () => {
  productos.value = await adminPanel.getProductos();
  usuarios.value = await adminPanel.getUsuarios();
  tiposProductos.value = await adminPanel.getTiposProductos();
  cargando.value = false;
});

// Modal de Confirmación
const showModal = ref(false);
const modalMessage = ref('');
const confirmAction = ref(null);

const openModal = (message, action) => {
  modalMessage.value = message;
  confirmAction.value = () => {
    action();
    closeModal();
  };
  showModal.value = true;
};

const closeModal = () => {
  showModal.value = false;
  confirmAction.value = null;
};

const confirmDeleteMarca = (id) => {
  openModal('¿Estás seguro de que quieres eliminar esta marca?', () => eliminarMarca(id));
};

const confirmUpdateProducto = (id, data) => {
  openModal('¿Estás seguro de que quieres actualizar este producto?', () => actualizarProducto(id, data));
};

const confirmDeleteProducto = (id) => {
  openModal('¿Estás seguro de que quieres eliminar este producto?', () => eliminarProducto(id));
};

const confirmUpdateTipoProducto = (id, data) => {
  openModal('¿Estás seguro de que quieres actualizar este tipo de producto?', async () => {
    await adminPanel.updateTipoProducto(id, data);
    tipoProductoEdit.value = null;
    await cargarTiposProductos();
  });
};

const confirmDeleteTipoProducto = (id) => {
  openModal('¿Estás seguro de que quieres eliminar este tipo de producto?', () => eliminarTipoProducto(id));
};

const confirmDeleteUsuario = (id) => {
  openModal('¿Estás seguro de que quieres eliminar este usuario?', () => eliminarUsuario(id));
};

const confirmUpdateUsuario = (id, data) => {
  openModal('¿Estás seguro de que quieres actualizar este usuario?', async () => {
    await adminPanel.updateUsuario(id, data);
    usuarioEdit.value = null;
    await cargarUsuarios();
  });
};

const confirmUpdateRolUsuario = (id, esAdmin) => {
  openModal('¿Estás seguro de que quieres cambiar el rol de este usuario?', () => actualizarRolUsuario(id, esAdmin));
};

const confirmUpdateMarca = (id, data) => {
  openModal('¿Estás seguro de que quieres actualizar esta marca?', async () => {
    await adminPanel.updateMarca(id, data);
    marcaEdit.value = null;
    await cargarMarcas();
  });
};

const adminCount = computed(() => usuarios.value.filter(u => u.esAdmin).length);

const showError = (message) => {
  modalMessage.value = message;
  confirmAction.value = closeModal;
  showModal.value = true;
};

const router = useRouter();
</script>

<template>
  <div class="flex min-h-screen">
    <!-- Sidebar -->
    <aside class="w-64 bg-gray-800 text-gray-100 flex flex-col justify-between py-6 px-4 min-h-screen">
      <div>
        <div class="flex flex-col items-center mb-8">
          <img src="/images/logo.png" alt="PixelVault Logo" class="w-40 h-20 mb-2 object-contain shadow-lg" />
          <span class="text-2xl font-bold text-yellow-100 tracking-wide">PixelVault</span>
        </div>
        <nav class="flex flex-col gap-2">
          <button
            v-for="item in [
              { key: 'usuarios', label: 'Users' },
              { key: 'productos', label: 'Productos' },
              { key: 'marcas', label: 'Marcas' },
              { key: 'tiposProductos', label: 'Tipo Producto' }
            ]"
            :key="item.key"
            @click="tab = item.key"
            :class="[
              'w-full text-left px-4 py-2 rounded transition',
              tab === item.key ? 'bg-gray-700 text-white font-semibold' : 'hover:bg-gray-700 hover:text-white text-gray-300'
            ]"
          >
            {{ item.label }}
          </button>
          <!-- Nuevos botones sin funcionalidad -->
          <button class="w-full text-left px-4 py-2 rounded transition hover:bg-gray-700 hover:text-white text-gray-300">Pedidos</button>
          <button class="w-full text-left px-4 py-2 rounded transition hover:bg-gray-700 hover:text-white text-gray-300">Pagos</button>
          <button class="w-full text-left px-4 py-2 rounded transition hover:bg-gray-700 hover:text-white text-gray-300">Historial</button>
        </nav>
      </div>
      <button class="text-left px-4 py-2 text-gray-300 hover:text-white hover:bg-gray-700 rounded transition mt-8" @click="router.push('/')">Leave</button>
    </aside>

    <!-- Main Content -->
    <main class="flex-1 p-6 bg-gray-50 min-h-screen">
      <h1 class="text-2xl font-bold mb-4">Panel de Administración</h1>

      <!-- Modal de Confirmación -->
      <div v-if="showModal" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
        <div class="bg-white p-6 rounded-lg shadow-lg max-w-md w-full">
          <h3 class="text-lg font-bold mb-4">{{ modalMessage }}</h3>
          <div class="flex justify-end space-x-2">
            <button @click="confirmAction" class="px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700">Aceptar</button>
            <button @click="closeModal" class="px-4 py-2 bg-gray-300 text-gray-700 rounded hover:bg-gray-400">Cerrar</button>
          </div>
        </div>
      </div>

      <!-- MARCAS CRUD -->
      <div v-if="tab === 'marcas'">
        <div class="w-full max-w-xl mx-auto bg-white rounded-lg shadow-lg p-8 my-8">
          <div class="flex items-center justify-center mb-6">
            <span class="text-2xl mr-2">➕</span>
            <h2 class="text-xl font-bold tracking-wide">ADD BRAND</h2>
          </div>
          <form @submit.prevent="crearMarca" class="space-y-4 mb-6">
            <div class="grid grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-semibold mb-1">Nombre</label>
                <input v-model="nuevaMarca.nombre" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
              </div>
              <div>
                <label class="block text-sm font-semibold mb-1">Logo URL</label>
                <input v-model="nuevaMarca.logoUrl" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
              </div>
              <div class="col-span-2">
                <label class="block text-sm font-semibold mb-1">Website</label>
                <input v-model="nuevaMarca.website" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
              </div>
            </div>
            <div class="flex justify-center mt-6">
              <button type="submit" class="px-8 py-2 border border-black rounded-full font-semibold hover:bg-black hover:text-white transition">Crear</button>
            </div>
          </form>
          <div class="overflow-x-auto">
            <table class="min-w-full text-sm text-left">
              <thead>
                <tr class="border-b">
                  <th class="py-2 px-4 font-semibold">ID</th>
                  <th class="py-2 px-4 font-semibold">Nombre</th>
                  <th class="py-2 px-4 font-semibold">Logo</th>
                  <th class="py-2 px-4 font-semibold">Website</th>
                  <th class="py-2 px-4 font-semibold">Acciones</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="marca in marcas" :key="marca.idMarca" class="border-b hover:bg-gray-50">
                  <td class="py-2 px-4">{{ marca.idMarca }}</td>
                  <td class="py-2 px-4">{{ marca.nombre }}</td>
                  <td class="py-2 px-4 truncate max-w-xs"><a :href="marca.logoUrl" target="_blank" class="text-blue-600 hover:underline">{{ marca.logoUrl }}</a></td>
                  <td class="py-2 px-4 truncate max-w-xs"><a :href="marca.website" target="_blank" class="text-blue-600 hover:underline">{{ marca.website }}</a></td>
                  <td class="py-2 px-4 flex gap-2">
                    <button @click="marcaEdit = { ...marca }" class="text-blue-600 hover:underline">Editar</button>
                    <button @click="confirmDeleteMarca(marca.idMarca)" class="text-red-600 hover:underline">Eliminar</button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div v-if="marcaEdit" class="mt-6 p-4 bg-gray-50 rounded shadow">
            <h4 class="font-bold mb-2">Editar Marca</h4>
            <form @submit.prevent="confirmUpdateMarca(marcaEdit.idMarca, marcaEdit)">
              <div class="grid grid-cols-2 gap-4 mb-4">
                <div>
                  <label class="block text-sm font-semibold mb-1">Nombre</label>
                  <input v-model="marcaEdit.nombre" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
                </div>
                <div>
                  <label class="block text-sm font-semibold mb-1">Logo URL</label>
                  <input v-model="marcaEdit.logoUrl" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
                </div>
                <div class="col-span-2">
                  <label class="block text-sm font-semibold mb-1">Website</label>
                  <input v-model="marcaEdit.website" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
                </div>
              </div>
              <button type="submit" class="px-4 py-1 border border-green-600 rounded-full text-green-700 font-semibold mr-2 hover:bg-green-600 hover:text-white transition">Guardar</button>
              <button @click="marcaEdit = null" type="button" class="text-red-600 hover:underline">Cancelar</button>
            </form>
          </div>
        </div>
      </div>

      <!-- PRODUCTOS CRUD -->
      <div v-if="tab === 'productos'">
        <div class="w-full max-w-5xl mx-auto bg-white rounded-lg shadow-lg p-8 my-8">
          <div class="flex items-center justify-center mb-6">
            <span class="text-2xl mr-2">➕</span>
            <h2 class="text-xl font-bold tracking-wide">ADD PRODUCT</h2>
          </div>
          <form @submit.prevent="crearProducto" class="space-y-4 mb-6">
            <div class="grid grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-semibold mb-1">Nombre</label>
                <input v-model="nuevoProducto.nombre" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
              </div>
              <div>
                <label class="block text-sm font-semibold mb-1">ID Marca</label>
                <input v-model.number="nuevoProducto.idMarca" type="number" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
              </div>
              <div>
                <label class="block text-sm font-semibold mb-1">ID Tipo</label>
                <input v-model.number="nuevoProducto.idTipo" type="number" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
              </div>
              <div>
                <label class="block text-sm font-semibold mb-1">Stock</label>
                <input v-model.number="nuevoProducto.stock" type="number" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
              </div>
            </div>
            <div>
              <label class="block text-sm font-semibold mb-1">Descripción</label>
              <textarea v-model="nuevoProducto.descripcion" class="w-full border-b border-gray-400 focus:outline-none py-1" required></textarea>
            </div>
            <div>
              <label class="block text-sm font-semibold mb-1">Fecha Lanzamiento</label>
              <input v-model="nuevoProducto.fechaLanzamiento" type="date" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
            </div>
            <div>
              <label class="block text-sm font-semibold mb-1">Imagen URL</label>
              <input v-model="nuevoProducto.imagenUrl" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
            </div>
            <div class="flex gap-4">
              <label class="flex items-center gap-1">
                <input v-model="nuevoProducto.activo" type="checkbox">
                Activo
              </label>
              <label class="flex items-center gap-1">
                <input v-model="nuevoProducto.destacado" type="checkbox">
                Destacado
              </label>
            </div>
            <div class="flex justify-center mt-6">
              <button type="submit" class="px-8 py-2 border border-black rounded-full font-semibold hover:bg-black hover:text-white transition">Crear</button>
            </div>
          </form>
          <div class="overflow-x-auto">
            <table class="min-w-full text-sm text-left">
              <thead>
                <tr class="border-b">
                  <th class="py-2 px-4 font-semibold">ID</th>
                  <th class="py-2 px-4 font-semibold">Nombre</th>
                  <th class="py-2 px-4 font-semibold">Descripción</th>
                  <th class="py-2 px-4 font-semibold">Stock</th>
                  <th class="py-2 px-4 font-semibold">Fecha Lanzamiento</th>
                  <th class="py-2 px-4 font-semibold">Imagen</th>
                  <th class="py-2 px-4 font-semibold">Activo</th>
                  <th class="py-2 px-4 font-semibold">Destacado</th>
                  <th class="py-2 px-4 font-semibold">ID Marca</th>
                  <th class="py-2 px-4 font-semibold">ID Tipo</th>
                  <th class="py-2 px-4 font-semibold">Acciones</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="producto in productos" :key="producto.idProducto" class="border-b hover:bg-gray-50">
                  <td class="py-2 px-4">{{ producto.idProducto }}</td>
                  <td class="py-2 px-4">{{ producto.nombre }}</td>
                  <td class="py-2 px-4 truncate max-w-xs">{{ producto.descripcion }}</td>
                  <td class="py-2 px-4">{{ producto.stock }}</td>
                  <td class="py-2 px-4">{{ producto.fechaLanzamiento }}</td>
                  <td class="py-2 px-4 truncate max-w-xs"><a :href="producto.imagenUrl" target="_blank" class="text-blue-600 hover:underline">Imagen</a></td>
                  <td class="py-2 px-4">
                    <span :class="producto.activo ? 'bg-green-100 text-green-700' : 'bg-gray-200 text-gray-700'" class="px-2 py-0.5 rounded text-xs">
                      {{ producto.activo ? 'Sí' : 'No' }}
                    </span>
                  </td>
                  <td class="py-2 px-4">
                    <span :class="producto.destacado ? 'bg-green-100 text-green-700' : 'bg-gray-200 text-gray-700'" class="px-2 py-0.5 rounded text-xs">
                      {{ producto.destacado ? 'Sí' : 'No' }}
                    </span>
                  </td>
                  <td class="py-2 px-4">{{ producto.idMarca }}</td>
                  <td class="py-2 px-4">{{ producto.idTipo }}</td>
                  <td class="py-2 px-4 flex gap-2">
                    <button @click="productoEdit = { ...producto }" class="text-blue-600 hover:underline">Editar</button>
                    <button @click="confirmDeleteProducto(producto.idProducto)" class="text-red-600 hover:underline">Eliminar</button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div v-if="productoEdit" class="mt-6 p-4 bg-gray-50 rounded shadow">
            <h4 class="font-bold mb-2">Editar Producto</h4>
            <!-- Repite los campos del formulario de creación aquí para editar -->
            <input v-model="productoEdit.nombre" placeholder="Nombre" class="w-full border-b border-gray-400 focus:outline-none py-1 mb-2" required>
            <input v-model="productoEdit.descripcion" placeholder="Descripción" class="w-full border-b border-gray-400 focus:outline-none py-1 mb-2" required>
            <input v-model.number="productoEdit.stock" placeholder="Stock" type="number" class="w-full border-b border-gray-400 focus:outline-none py-1 mb-2" required>
            <input v-model="productoEdit.fechaLanzamiento" placeholder="Fecha Lanzamiento" type="date" class="w-full border-b border-gray-400 focus:outline-none py-1 mb-2" required>
            <input v-model="productoEdit.imagenUrl" placeholder="Imagen URL" class="w-full border-b border-gray-400 focus:outline-none py-1 mb-2" required>
            <input v-model.number="productoEdit.idMarca" placeholder="ID Marca" type="number" class="w-full border-b border-gray-400 focus:outline-none py-1 mb-2" required>
            <input v-model.number="productoEdit.idTipo" placeholder="ID Tipo" type="number" class="w-full border-b border-gray-400 focus:outline-none py-1 mb-2" required>
            <div class="flex gap-4 mb-2">
              <label class="flex items-center gap-1">
                <input v-model="productoEdit.activo" type="checkbox">
                Activo
              </label>
              <label class="flex items-center gap-1">
                <input v-model="productoEdit.destacado" type="checkbox">
                Destacado
              </label>
            </div>
            <button @click="confirmUpdateProducto(productoEdit.idProducto, productoEdit)" class="px-4 py-1 border border-green-600 rounded-full text-green-700 font-semibold mr-2 hover:bg-green-600 hover:text-white transition">Guardar</button>
            <button @click="productoEdit = null" class="text-red-600 hover:underline">Cancelar</button>
          </div>
        </div>
      </div>

      <!-- TIPOSPRODUCTOS CRUD -->
      <div v-if="tab === 'tiposProductos'">
        <div class="w-full max-w-2xl mx-auto bg-white rounded-lg shadow-lg p-8 my-8">
          <div class="flex items-center justify-center mb-6">
            <span class="text-2xl mr-2">🏷️</span>
            <h2 class="text-xl font-bold tracking-wide">ADD PRODUCT TYPE</h2>
          </div>
          <form @submit.prevent="crearTipoProducto" class="space-y-4 mb-6">
            <div>
              <label class="block text-sm font-semibold mb-1">Nombre</label>
              <input v-model="nuevoTipoProducto.nombre" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
            </div>
            <div>
              <label class="block text-sm font-semibold mb-1">Descripción</label>
              <input v-model="nuevoTipoProducto.descripcion" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
            </div>
            <div class="flex justify-center mt-6">
              <button type="submit" class="px-8 py-2 border border-black rounded-full font-semibold hover:bg-black hover:text-white transition">Crear</button>
            </div>
          </form>
          <div class="overflow-x-auto">
            <table class="min-w-full text-sm text-left">
              <thead>
                <tr class="border-b">
                  <th class="py-2 px-4 font-semibold">ID</th>
                  <th class="py-2 px-4 font-semibold">Nombre</th>
                  <th class="py-2 px-4 font-semibold">Descripción</th>
                  <th class="py-2 px-4 font-semibold">Acciones</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="tipo in tiposProductos" :key="tipo.idTipo" class="border-b hover:bg-gray-50">
                  <td class="py-2 px-4">{{ tipo.idTipo }}</td>
                  <td class="py-2 px-4">{{ tipo.nombre }}</td>
                  <td class="py-2 px-4">{{ tipo.descripcion }}</td>
                  <td class="py-2 px-4 flex gap-2">
                    <button @click="tipoProductoEdit = { ...tipo }" class="text-blue-600 hover:underline">Editar</button>
                    <button @click="confirmDeleteTipoProducto(tipo.idTipo)" class="text-red-600 hover:underline">Eliminar</button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div v-if="tipoProductoEdit" class="mt-6 p-4 bg-gray-50 rounded shadow">
            <h4 class="font-bold mb-2">Editar Tipo de Producto</h4>
            <form @submit.prevent="confirmUpdateTipoProducto(tipoProductoEdit.idTipo, tipoProductoEdit)">
              <div class="grid grid-cols-2 gap-4 mb-4">
                <div>
                  <label class="block text-sm font-semibold mb-1">Nombre</label>
                  <input v-model="tipoProductoEdit.nombre" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
                </div>
                <div>
                  <label class="block text-sm font-semibold mb-1">Descripción</label>
                  <input v-model="tipoProductoEdit.descripcion" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
                </div>
              </div>
              <button type="submit" class="px-4 py-1 border border-green-600 rounded-full text-green-700 font-semibold mr-2 hover:bg-green-600 hover:text-white transition">Guardar</button>
              <button @click="tipoProductoEdit = null" type="button" class="text-red-600 hover:underline">Cancelar</button>
            </form>
          </div>
        </div>
      </div>

      <!-- PEDIDOS CRUD -->
      <div v-if="tab === 'pedidos'">
        <h2 class="text-xl font-bold mb-2">Pedidos</h2>
        <!-- Aquí irá el CRUD de pedidos -->
      </div>

      <!-- PAGOS CRUD -->
      <div v-if="tab === 'pagos'">
        <h2 class="text-xl font-bold mb-2">Pagos</h2>
        <!-- Aquí irá el CRUD de pagos -->
      </div>

      <!-- USUARIOS CRUD Y BÚSQUEDA -->
      <div v-if="tab === 'usuarios'">
        <div class="w-full max-w-4xl mx-auto bg-white rounded-lg shadow-lg p-8 my-8">
          <div class="flex items-center justify-center mb-6">
            <span class="text-2xl mr-2">👤</span>
            <h2 class="text-xl font-bold tracking-wide">Usuarios</h2>
          </div>
          <form @submit.prevent="buscarUsuarioPorId" class="flex gap-2 mb-6">
            <input v-model="usuarioIdBuscar" placeholder="Buscar por ID" class="flex-1 border-b border-gray-400 focus:outline-none py-1" />
            <button type="submit" class="px-6 py-1 border border-black rounded-full font-semibold hover:bg-black hover:text-white transition">Buscar</button>
          </form>
          <div v-if="usuarioDetalle" class="mb-6 p-4 bg-gray-50 rounded shadow">
            <h3 class="font-bold mb-2">Detalle de Usuario</h3>
            <pre class="text-xs">{{ usuarioDetalle }}</pre>
          </div>
          <div class="overflow-x-auto">
            <table class="min-w-full text-sm text-left">
              <thead>
                <tr class="border-b">
                  <th class="py-2 px-4 font-semibold">ID</th>
                  <th class="py-2 px-4 font-semibold">Nombre</th>
                  <th class="py-2 px-4 font-semibold">Email</th>
                  <th class="py-2 px-4 font-semibold">Admin</th>
                  <th class="py-2 px-4 font-semibold">Acciones</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="usuario in usuarios" :key="usuario.idUsuario" class="border-b hover:bg-gray-50">
                  <td class="py-2 px-4">{{ usuario.idUsuario }}</td>
                  <td class="py-2 px-4">{{ usuario.nombre }} {{ usuario.apellidos }}</td>
                  <td class="py-2 px-4">{{ usuario.email }}</td>
                  <td class="py-2 px-4">
                    <span :class="usuario.esAdmin ? 'bg-green-100 text-green-700' : 'bg-gray-200 text-gray-700'" class="px-2 py-0.5 rounded text-xs">
                      {{ usuario.esAdmin ? 'Sí' : 'No' }}
                    </span>
                  </td>
                  <td class="py-2 px-4 flex gap-2">
                    <button
                      @click="confirmDeleteUsuario(usuario.idUsuario)"
                      class="text-red-600 hover:underline"
                      :disabled="usuario.esAdmin && adminCount.value === 1"
                      :class="usuario.esAdmin && adminCount.value === 1 ? 'opacity-50 cursor-not-allowed' : ''"
                      :title="usuario.esAdmin && adminCount.value === 1 ? 'Debe haber al menos un administrador' : ''"
                    >
                      Eliminar
                    </button>
                    <button
                      @click="usuarioRolEdit = usuario"
                      class="text-blue-600 hover:underline"
                      :disabled="usuario.esAdmin && adminCount.value === 1"
                      :class="usuario.esAdmin && adminCount.value === 1 ? 'opacity-50 cursor-not-allowed' : ''"
                      :title="usuario.esAdmin && adminCount.value === 1 ? 'Debe haber al menos un administrador' : ''"
                    >
                      Editar Rol
                    </button>
                    <button @click="usuarioEdit = { ...usuario }" class="text-yellow-600 hover:underline">Editar</button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div v-if="usuarioRolEdit" class="mt-6 p-4 bg-gray-50 rounded shadow">
            <h4 class="font-bold mb-2">Editar Rol de {{ usuarioRolEdit.nombre }}</h4>
            <button @click="confirmUpdateRolUsuario(usuarioRolEdit.idUsuario, true)" class="px-4 py-1 border border-green-600 rounded-full text-green-700 font-semibold mr-2 hover:bg-green-600 hover:text-white transition">Hacer Admin</button>
            <button @click="confirmUpdateRolUsuario(usuarioRolEdit.idUsuario, false)" class="px-4 py-1 border border-gray-600 rounded-full text-gray-700 font-semibold mr-2 hover:bg-gray-600 hover:text-white transition" :disabled="usuarioRolEdit.esAdmin && adminCount.value === 1" :class="usuarioRolEdit.esAdmin && adminCount.value === 1 ? 'opacity-50 cursor-not-allowed' : ''" :title="usuarioRolEdit.esAdmin && adminCount.value === 1 ? 'Debe haber al menos un administrador' : ''">Quitar Admin</button>
            <button @click="usuarioRolEdit = null" class="text-red-600 hover:underline">Cancelar</button>
          </div>
          <div v-if="usuarioEdit" class="mt-6 p-4 bg-gray-50 rounded shadow">
            <h4 class="font-bold mb-2">Editar Usuario</h4>
            <form @submit.prevent="confirmUpdateUsuario(usuarioEdit.idUsuario, usuarioEdit)">
              <div class="grid grid-cols-2 gap-4 mb-4">
                <div>
                  <label class="block text-sm font-semibold mb-1">Nombre</label>
                  <input v-model="usuarioEdit.nombre" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
                </div>
                <div>
                  <label class="block text-sm font-semibold mb-1">Apellidos</label>
                  <input v-model="usuarioEdit.apellidos" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
                </div>
                <div class="col-span-2">
                  <label class="block text-sm font-semibold mb-1">Email</label>
                  <input v-model="usuarioEdit.email" class="w-full border-b border-gray-400 focus:outline-none py-1" required>
                </div>
              </div>
              <button type="submit" class="px-4 py-1 border border-green-600 rounded-full text-green-700 font-semibold mr-2 hover:bg-green-600 hover:text-white transition">Guardar</button>
              <button @click="usuarioEdit = null" type="button" class="text-red-600 hover:underline">Cancelar</button>
            </form>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>