<script setup>
import { ref } from 'vue';
import * as adminPanel from '../api/adminPanel';

const tab = ref('marcas');
const marcas = ref([]);
const productos = ref([]);
const tiposProductos = ref([]);
const pedidos = ref([]);
const pagos = ref([]);
const usuarios = ref([]);
const usuarioDetalle = ref(null);

// Ejemplo de carga inicial
const cargarMarcas = async () => { marcas.value = await adminPanel.getMarcas(); };
const cargarProductos = async () => { productos.value = await adminPanel.getProductos(); };
const cargarTiposProductos = async () => { tiposProductos.value = await adminPanel.getTiposProductos(); };
const cargarPedidos = async () => { pedidos.value = await adminPanel.getPedidos(); };
const cargarPagos = async () => { pagos.value = await adminPanel.getPagos(); };
const cargarUsuarios = async () => { usuarios.value = await adminPanel.getUsuarios(); };

const cargarUsuarioDetalle = async (id) => {
  usuarioDetalle.value = await adminPanel.getUsuarioDetalle(id);
};

const cargarTab = async () => {
  if (tab.value === 'marcas') await cargarMarcas();
  if (tab.value === 'productos') await cargarProductos();
  if (tab.value === 'tiposProductos') await cargarTiposProductos();
  if (tab.value === 'pedidos') await cargarPedidos();
  if (tab.value === 'pagos') await cargarPagos();
  if (tab.value === 'usuarios') await cargarUsuarios();
};

watch(tab, cargarTab, { immediate: true });
</script>

<template>
  <div class="p-6">
    <h1 class="text-2xl font-bold mb-4">Panel de Administración</h1>
    <div class="flex space-x-4 mb-6">
      <button v-for="t in ['marcas','productos','tiposProductos','pedidos','pagos','usuarios']"
              :key="t"
              :class="['px-4 py-2 rounded', tab === t ? 'bg-blue-600 text-white' : 'bg-gray-200']"
              @click="tab = t">
        {{ t.charAt(0).toUpperCase() + t.slice(1) }}
      </button>
    </div>

    <!-- Marcas -->
    <div v-if="tab === 'marcas'">
      <h2 class="text-xl font-bold mb-2">Marcas</h2>
      <ul>
        <li v-for="marca in marcas" :key="marca.id">{{ marca.nombre }}</li>
      </ul>
      <!-- Aquí irían los formularios y botones CRUD -->
    </div>

    <!-- Productos -->
    <div v-if="tab === 'productos'">
      <h2 class="text-xl font-bold mb-2">Productos</h2>
      <ul>
        <li v-for="producto in productos" :key="producto.id">{{ producto.nombre }}</li>
      </ul>
    </div>

    <!-- Tipos de Productos -->
    <div v-if="tab === 'tiposProductos'">
      <h2 class="text-xl font-bold mb-2">Tipos de Productos</h2>
      <ul>
        <li v-for="tipo in tiposProductos" :key="tipo.id">{{ tipo.nombre }}</li>
      </ul>
    </div>

    <!-- Pedidos -->
    <div v-if="tab === 'pedidos'">
      <h2 class="text-xl font-bold mb-2">Pedidos</h2>
      <ul>
        <li v-for="pedido in pedidos" :key="pedido.id">{{ pedido.id }}</li>
      </ul>
    </div>

    <!-- Pagos -->
    <div v-if="tab === 'pagos'">
      <h2 class="text-xl font-bold mb-2">Pagos</h2>
      <ul>
        <li v-for="pago in pagos" :key="pago.id">{{ pago.id }}</li>
      </ul>
    </div>

    <!-- Usuarios -->
    <div v-if="tab === 'usuarios'">
      <h2 class="text-xl font-bold mb-2">Usuarios</h2>
      <ul>
        <li v-for="usuario in usuarios" :key="usuario.id">
          <span @click="cargarUsuarioDetalle(usuario.id)" class="cursor-pointer text-blue-600 underline">
            {{ usuario.nombre }}
          </span>
        </li>
      </ul>
      <div v-if="usuarioDetalle">
        <h3 class="font-bold mt-4">Detalle de Usuario</h3>
        <pre>{{ usuarioDetalle }}</pre>
      </div>
    </div>
  </div>
</template>