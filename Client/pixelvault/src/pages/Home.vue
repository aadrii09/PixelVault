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
                    :key="producto.idProducto"
                    :producto="producto"
                    :mostrarBoton="mostrarBoton"
                />
            </div>
        </div>
        
        <!-- Sección Game Lover con tamaño controlado -->
        <div class="game-lover-container">
            <!-- Banner principal -->
            <div class="banner-wrapper">
                <img src="../../public/images/banner.png" alt="GAME LOVER Banner" class="game-lover-banner">
            </div>
            
            <!-- Rejilla de consolas -->
            <div class="consoles-grid">
                <div class="console-item">
                    <img src="../../public/images/PlataformaPS5.png" alt="PlayStation 5" class="console-image" />
                </div>
                <div class="console-item">
                    <img src="../../public/images/PlataformaXbox.png" alt="Xbox Series X" class="console-image" />
                </div>
                <div class="console-item">
                    <img src="../../public/images/PlataformaNINTENDO.png" alt="Nintendo Switch" class="console-image" />
                </div>
                <div class="console-item">
                    <img src="../../public/images/PlataformaPC.png" alt="Gaming PC" class="console-image" />
                </div>
            </div>
            
            <!-- Sección de suscripciones -->
            <div class="subscriptions-section">
                <h2 class="subscriptions-title">Subscriptiones</h2>
                <ul class="subscriptions-list">
                    <li>• Descuentos exclusivos para suscriptores</li>
                    <li>• Noticias sobre próximos lanzamientos de juegos</li>
                    <li>• Guías de compra para componentes de PC gaming</li>
                    <li>• Invitaciones a eventos especiales</li>
                </ul>
                <button class="subscribe-button">Subscribe</button>
            </div>
        </div>
    </div>
</template>



<style lang="scss" scoped>
.home-container {
    min-height: 100vh;
    background-color: #0e0b30;
    color: white;
    padding-top: 0;
    padding-bottom: 0;
    margin-bottom: 0;
    display: flex;
    flex-direction: column;
}

/* Contenedor principal de Game Lover con tamaño controlado */
.game-lover-container {
    width: 100%;
    max-width: 800px;
    margin: 2rem auto;
    display: flex;
    flex-direction: column;
    background-color: #080819;
}

.banner-wrapper {
    width: 100%;
}

.game-lover-banner {
    width: 100%;
    height: 100%;
    display: block;
}

.consoles-grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    grid-gap: 1px;
    width: 100%;
}

.console-item {
    overflow: hidden;
    background-color: #131131;
}

.console-image {
    width: 100%;
    height: auto;
    display: block;
    object-fit: cover;
}

.subscriptions-section {
    background-color: #171733;
    width: 100%;
    padding: 20px 0;
    display: flex;
    flex-direction: column;
    align-items: center;
}

.subscriptions-title {
    font-size: 24px;
    font-weight: bold;
    margin-bottom: 15px;
}

.subscriptions-list {
    list-style-type: none;
    padding: 0;
    margin: 0 0 20px 0;
    text-align: center;
}

.subscriptions-list li {
    margin: 5px 0;
    color: #d9d9d9;
    font-size: 14px;
}

.subscribe-button {
    background-color: transparent;
    border: 1px solid white;
    color: white;
    padding: 8px 30px;
    border-radius: 20px;
    cursor: pointer;
    transition: all 0.3s ease;
}

.subscribe-button:hover {
    background-color: rgba(255, 255, 255, 0.1);
}
</style>