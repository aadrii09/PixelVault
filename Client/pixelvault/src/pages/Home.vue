<script setup>

import {ref, onMounted, computed} from 'vue';
import {getProductos} from '../api/productos';
import ProductCard from '../components/ProductCard.vue';
import {useUserStore} from '../store/user';


const productos = ref([]);
const cargando = ref(true);
const userStore = useUserStore();

const mostrarBoton = computed(() => !!userStore.token); 

onMounted(async () => {
    productos.value = await getProductos();
    cargando.value = false;
});

</script>



<template>
    <div class="home-container">
        <div class="max-w-6xl mx-auto px-4 py-6">
            <h1 class="text-2xl font-bold mb-6 text-center text-white">Productos Destacados</h1>
            <div v-if="cargando" class="text-gray-300 text-center">
                <p>Cargando productos...</p>
            </div>
            <div v-else class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
                <ProductCard
                    v-for="producto in productos"
                    :key="idProducto"
                    :producto="producto"
                    :mostrarBoton="mostrarBoton"
                />
            </div>
        </div>
    </div>
</template>



<style lang="scss" scoped>
.home-container {
    min-height: 100vh;
    background-color: #0e0b30;
    color: white;
    padding-top: 2rem;
    padding-bottom: 0;
    margin-bottom: 0;
    display: flex;
    flex-direction: column;
}
</style>