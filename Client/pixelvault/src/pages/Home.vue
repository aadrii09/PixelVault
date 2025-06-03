<script setup>
import { ref, onMounted, computed, inject } from 'vue';
import { getProductos } from '../api/productos';
import { agregarAlCarrito } from '../api/carrito';
import { useUserStore } from '../store/user';
import ProductCard from '../components/ProductCard.vue';

const productos = ref([]);
const cargando = ref(true);
const userStore = useUserStore();
const mostrarBoton = computed(() => !!userStore.token);
const email = ref('');
// Obtener función de notificación global
const showNotification = inject('showNotification', null);
const currentIndex = ref(0);
const autoScrollInterval = ref(null);
const carouselRef = ref(null);
const visibleItems = ref(3);
const isSubscribing = ref(false);
const subscriptionMessage = ref('');
const isError = ref(false);

// Computed property para obtener solo productos destacados
const productosDestacados = computed(() => {
    return productos.value.filter(producto => producto.destacado === true);
});

// Datos de noticias
const noticias = ref([
    {
        id: 1,
        fecha: '15 JUNIO 2024',
        titulo: 'Lanzamiento exclusivo: Cyber Odyssey',
        descripcion: 'El esperado RPG de mundo abierto ya está disponible en todas las plataformas.',
        imagen: 'https://images.unsplash.com/photo-1598550476439-6847785fcea6',
        alt: 'Cyber Odyssey'
    },
    {
        id: 2,
        fecha: '18 MAYO 2024',
        titulo: 'Juego indie gana premio principal',
        descripcion: '"Stray Souls" gana el premio al mejor juego en los BAFTA.',
        imagen: 'https://images.unsplash.com/photo-1540206395-68808572332f',
        alt: 'Juego Indie'
    },
    {
        id: 3,
        fecha: '10 MAYO 2024',
        titulo: 'Anuncian nueva generación de consolas',
        descripcion: 'La próxima generación llegará en navidad con revolucionarias características.',
        imagen: 'https://images.unsplash.com/photo-1589254065878-42c9da997008',
        alt: 'Nueva Consola'
    },
    {
        id: 4,
        fecha: '5 MAYO 2024',
        titulo: 'Récord en torneos de eSports',
        descripcion: 'El premio acumulado en torneos este año supera los $50 millones de dólares.',
        imagen: 'https://images.unsplash.com/photo-1542751371-adc38448a05e',
        alt: 'eSports'
    },
    {
        id: 5,
        fecha: '28 ABRIL 2024',
        titulo: 'Avances en realidad virtual',
        descripcion: 'Nuevos lentes de VR prometen una experiencia inmersiva sin mareos.',
        imagen: 'https://images.unsplash.com/photo-1493711662062-fa541adb3fc8',
        alt: 'Realidad Virtual'
    },
    {
        id: 6,
        fecha: '22 ABRIL 2024',
        titulo: 'Resurgimiento de juegos retro',
        descripcion: 'Clásicos de los 90s están siendo remasterizados con gráficos 4K.',
        imagen: 'https://images.unsplash.com/photo-1551103782-8ab07afd45c1',
        alt: 'Juego Retro'
    },
    {
        id: 7,
        fecha: '15 ABRIL 2024',
        titulo: 'Plataforma de streaming para gamers',
        descripcion: 'Nuevo servicio competirá con Twitch con mejores condiciones para creadores.',
        imagen: 'https://images.unsplash.com/photo-1522542550221-31fd19575a2d',
        alt: 'Streaming'
    },
    {
        id: 8,
        fecha: '8 ABRIL 2024',
        titulo: 'Tecnología que cambiará el gaming',
        descripcion: 'Nuevo motor gráfico permite renderizado en tiempo real sin carga.',
        imagen: 'https://images.unsplash.com/photo-1511512578047-dfb367046420',
        alt: 'Tecnología Gaming'
    },
    {
        id: 9,
        fecha: '1 ABRIL 2024',
        titulo: 'Mobile gaming supera a consolas',
        descripcion: 'Por primera vez, los ingresos por juegos móviles superan a consolas y PC juntas.',
        imagen: 'https://images.unsplash.com/photo-1547036967-23d11aacaee0',
        alt: 'Mobile Gaming'
    },
    {
        id: 10,
        fecha: '25 MARZO 2024',
        titulo: 'IA crea NPCs con personalidad única',
        descripcion: 'Nueva tecnología permite a los NPCs aprender y evolucionar durante el juego.',
        imagen: 'https://images.unsplash.com/photo-1560253023-3ec5d502959f',
        alt: 'Inteligencia Artificial'
    }
]);

onMounted(async () => {
    try {
        productos.value = await getProductos();
        cargando.value = false;

        // Inicializar animaciones
        inicializarAnimaciones();

        // Inicializar carrusel
        inicializarCarrusel();

        // Manejar cambios de tamaño
        window.addEventListener('resize', actualizarVisibleItems);
    } catch (error) {
        console.error('Error al cargar productos:', error);
        cargando.value = false;
    }
});

function inicializarAnimaciones() {
    // Inicializar animaciones de aparición al scroll
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };


    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
            }
        });
    }, observerOptions);

    document.querySelectorAll('.fade-in').forEach(el => {
        observer.observe(el);
    });
}

function inicializarCarrusel() {
    actualizarVisibleItems();
    startAutoScroll();
}

function actualizarVisibleItems() {
    const carousel = carouselRef.value;
    if (!carousel) return;

    // Determinar cuántos elementos son visibles basado en el ancho
    const width = carousel.offsetWidth;
    if (width < 640) visibleItems.value = 1;
    else if (width < 1024) visibleItems.value = 2;
    else visibleItems.value = 3;
}

function goToItem(index) {
    if (!carouselRef.value) return;

    currentIndex.value = Math.min(Math.max(index, 0), noticias.value.length - 1);

    // El ancho del elemento + gap (24px)
    const itemWidth = carouselRef.value.querySelector('.carousel-item').offsetWidth + 24;

    carouselRef.value.scrollTo({
        left: currentIndex.value * itemWidth,
        behavior: 'smooth'
    });
}

function prevItem() {
    const newIndex = Math.max(currentIndex.value - visibleItems.value, 0);
    goToItem(newIndex);
    resetAutoScroll();
}

function nextItem() {
    const newIndex = Math.min(currentIndex.value + visibleItems.value, noticias.value.length - 1);
    goToItem(newIndex);
    resetAutoScroll();
}

function startAutoScroll() {
    clearInterval(autoScrollInterval.value);
    autoScrollInterval.value = setInterval(() => {
        const newIndex = (currentIndex.value + visibleItems.value) % noticias.value.length;
        goToItem(newIndex);
    }, 5000);
}

function resetAutoScroll() {
    clearInterval(autoScrollInterval.value);
    startAutoScroll();
}

function pauseAutoScroll() {
    clearInterval(autoScrollInterval.value);
}

function resumeAutoScroll() {
    startAutoScroll();
}

function getDotCount() {
    return Math.ceil(noticias.value.length / Math.max(1, visibleItems.value));
}

function isDotActive(index) {
    const activeDotIndex = Math.floor(currentIndex.value / Math.max(1, visibleItems.value));
    return index === activeDotIndex;
}

function goToDotIndex(index) {
    goToItem(index * visibleItems.value);
    resetAutoScroll();
}

async function suscribirse() {
    if (!email.value) {
        subscriptionMessage.value = 'Por favor ingresa un correo electrónico';
        isError.value = true;
        return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email.value)) {
        subscriptionMessage.value = 'Por favor ingresa un correo electrónico válido';
        isError.value = true;
        return;
    }

    isSubscribing.value = true;
    isError.value = false;
    subscriptionMessage.value = '';

    try {
        const apiUrl = 'http://localhost:5225/api/Email/subscribe';
        console.log('Enviando solicitud a:', apiUrl);

        const response = await fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({
                email: email.value
            })
        });

        console.log('Respuesta recibida, status:', response.status);

        if (!response.ok) {
            const errorData = await response.json().catch(() => ({}));
            throw new Error(errorData.message || `Error HTTP: ${response.status}`);
        }

        const data = await response.json();
        subscriptionMessage.value = data.message || '¡Gracias por suscribirte!';
        isError.value = false;
        email.value = '';

    } catch (error) {
        console.error('Error en suscripción:', error);
        subscriptionMessage.value = 'Error al conectar con el servidor. Por favor, inténtalo de nuevo más tarde.';
        isError.value = true;
    } finally {
        isSubscribing.value = false;

        if (subscriptionMessage.value) {
            setTimeout(() => {
                subscriptionMessage.value = '';
            }, 5000);
        }
    }
}

function añadirAlCarrito(producto) {
    try {
        agregarAlCarrito(producto);
        // Ya tenemos showNotification inyectado desde el script setup
        if (showNotification) {
            showNotification(`¡${producto.nombre} agregado al carrito!`, 'success');
        } else {
            alert(`${producto.nombre} añadido al carrito!`);
        }
    } catch (error) {
        if (showNotification) {
            showNotification('Error al agregar al carrito', 'error');
        } else {
            alert('Error al agregar al carrito');
        }
        console.error('Error al agregar al carrito:', error);
    }
}
</script>

<template>
    <div class="home-container">        <!-- Sección de Productos Destacados -->
        <div class="mx-auto px-8 py-20 bg-gradient-primary">
            <h1 class="text-5xl text-center mb-12 text-gradient section-title">Productos Destacados</h1>
            <div v-if="cargando" class="text-gray-300 text-center">
                <p>Cargando productos...</p>
            </div>
            <div v-else class="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
                <ProductCard v-for="producto in productosDestacados" :key="producto.idProducto" :producto="producto"
                    :mostrarBoton="mostrarBoton" />
            </div>
        </div>

        <!-- Sección de Plataformas -->
        <section class="py-20 px-8 bg-gradient-secondary fade-in platforms" id="plataformas">
            <div class="max-w-7xl mx-auto">
                <h2 class="text-5xl text-center mb-12 text-gradient section-title">Nuestras Plataformas</h2>
                <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8 mb-12 platforms-grid">
                    <!-- PlayStation -->
                    <div class="bg-gradient-to-br from-white/10 to-white/5 border border-white/20 rounded-3xl p-8 text-center transition-all duration-300 hover:transform hover:-translate-y-3 hover:shadow-xl hover:shadow-[#00ff88]/20 hover:border-[#00ff88] platform-card cursor-pointer"
                        @click="$router.push({ name: 'ProductosPlataforma', params: { plataforma: 'PlayStation' } })">
                        <div class="h-32 flex items-center justify-center mb-4">
                            <img src="/images/PlataformaPS5.png" alt="PlayStation" class="h-full object-contain" />
                        </div>
                        <h3 class="text-2xl mb-4 text-[#00ff88]">PlayStation</h3>
                        <p class="opacity-80 leading-relaxed">Descubre los últimos juegos y accesorios para PS5 y PS4.
                            Vive la experiencia gaming definitiva.</p>
                    </div>

                    <!-- Xbox -->
                    <div class="bg-gradient-to-br from-white/10 to-white/5 border border-white/20 rounded-3xl p-8 text-center transition-all duration-300 hover:transform hover:-translate-y-3 hover:shadow-xl hover:shadow-[#00ff88]/20 hover:border-[#00ff88] platform-card cursor-pointer"
                        @click="$router.push({ name: 'ProductosPlataforma', params: { plataforma: 'Xbox' } })">
                        <div class="h-32 flex items-center justify-center mb-4">
                            <img src="/images/PlataformaXBOX.png" alt="Xbox" class="h-full object-contain" />
                        </div>
                        <h3 class="text-2xl mb-4 text-[#00ff88]">Xbox</h3>
                        <p class="opacity-80 leading-relaxed">Explora el mundo Xbox con Game Pass, Series X|S y los
                            mejores títulos exclusivos.</p>
                    </div>

                    <!-- Nintendo -->
                    <div class="bg-gradient-to-br from-white/10 to-white/5 border border-white/20 rounded-3xl p-8 text-center transition-all duration-300 hover:transform hover:-translate-y-3 hover:shadow-xl hover:shadow-[#00ff88]/20 hover:border-[#00ff88] platform-card cursor-pointer"
                        @click="$router.push({ name: 'ProductosPlataforma', params: { plataforma: 'Nintendo' } })">
                        <div class="h-32 flex items-center justify-center mb-4">
                            <img src="/images/PlataformaNINTENDO.png" alt="Nintendo" class="h-full object-contain" />
                        </div>
                        <h3 class="text-2xl mb-4 text-[#00ff88]">Nintendo</h3>
                        <p class="opacity-80 leading-relaxed">Sumérgete en las aventuras únicas de Nintendo Switch y sus
                            increíbles exclusivos.</p>
                    </div>

                    <!-- PC Gaming -->
                    <div class="bg-gradient-to-br from-white/10 to-white/5 border border-white/20 rounded-3xl p-8 text-center transition-all duration-300 hover:transform hover:-translate-y-3 hover:shadow-xl hover:shadow-[#00ff88]/20 hover:border-[#00ff88] platform-card cursor-pointer"
                        @click="$router.push({ name: 'ProductosPlataforma', params: { plataforma: 'PC' } })">
                        <div class="h-32 flex items-center justify-center mb-4">
                            <img src="/images/PlataformaPC.png" alt="PC Gaming" class="h-full object-contain" />
                        </div>
                        <h3 class="text-2xl mb-4 text-[#00ff88]">PC Gaming</h3>
                        <p class="opacity-80 leading-relaxed">Construye tu setup perfecto con componentes de alta gama y
                            periféricos profesionales.</p>
                    </div>
                </div>
            </div>
        </section>


        <!-- Sección de Noticias / Carrusel -->
        <section class="relative py-20 px-8 bg-gradient-to-r from-[#16213e] to-[#1a1a2e]" id="noticias">
            <div class="max-w-7xl mx-auto">
                <h2 class="text-5xl text-center mb-12 text-gradient section-title">Últimas Noticias</h2>

                <div class="relative">
                    <!-- Botones de navegación -->
                    <button @click="prevItem"
                        class="carousel-prev absolute top-1/2 -translate-y-1/2 -left-4 z-10 w-10 h-10 rounded-full bg-[rgba(0,204,255,0.3)] backdrop-blur-sm text-white hover:bg-[rgba(0,204,255,0.6)] transition-all flex items-center justify-center">
                        ❮
                    </button>
                    <button @click="nextItem"
                        class="carousel-next absolute top-1/2 -translate-y-1/2 -right-4 z-10 w-10 h-10 rounded-full bg-[rgba(0,204,255,0.3)] backdrop-blur-sm text-white hover:bg-[rgba(0,204,255,0.6)] transition-all flex items-center justify-center">
                        ❯
                    </button>

                    <!-- Carrusel -->
                    <div ref="carouselRef" @mouseenter="pauseAutoScroll" @mouseleave="resumeAutoScroll"
                        class="news-carousel flex overflow-x-auto hide-scrollbar gap-6 scroll-snap-x-mandatory scroll-smooth transition-transform duration-300 pb-4">

                        <!-- Items de Noticias -->
                        <div v-for="noticia in noticias" :key="noticia.id"
                            class="flex-none w-72 md:w-80 scroll-snap-align-start bg-gradient-to-br from-[rgba(255,255,255,0.1)] to-[rgba(255,255,255,0.05)] border border-[rgba(255,255,255,0.2)] rounded-xl backdrop-blur-sm transition-all hover:-translate-y-2 hover:shadow-[0_15px_30px_rgba(0,204,255,0.2)] hover:border-[#00ccff] carousel-item">
                            <img :src="noticia.imagen" class="w-full h-48 object-cover rounded-t-lg" :alt="noticia.alt">
                            <div class="p-6">
                                <div class="text-[#00ff88] text-sm mb-2">{{ noticia.fecha }}</div>
                                <h3 class="text-white text-xl mb-3">{{ noticia.titulo }}</h3>
                                <p class="text-gray-300 text-sm mb-4">{{ noticia.descripcion }}</p>
                                <a href="#"
                                    class="text-[#00ccff] font-bold text-sm hover:text-[#00ff88] transition-colors">
                                    Leer más →
                                </a>
                            </div>
                        </div>
                    </div>

                    <!-- Puntos de navegación -->
                    <div class="flex justify-center gap-4 mt-8 carousel-dots">
                        <div v-for="index in getDotCount()" :key="index - 1" @click="goToDotIndex(index - 1)" :class="[
                            'w-3 h-3 rounded-full cursor-pointer transition-all',
                            isDotActive(index - 1) ? 'bg-[#00ff88] scale-125' : 'bg-[rgba(255,255,255,0.3)]'
                        ]">
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- Sección de Newsletter -->
        <section class="py-16 px-8 bg-gradient-secondary text-center" id="newsletter">
            <div class="max-w-7xl mx-auto">
                <h2 class="text-5xl text-center mb-12 text-gradient section-title">Únete a la Comunidad</h2>
                <p class="mb-8">Suscríbete para recibir las últimas noticias, ofertas exclusivas y lanzamientos.</p>
                <form class="max-w-lg mx-auto mt-8 flex flex-col md:flex-row gap-4 newsletter-form"
                    @submit.prevent="suscribirse">
                    <input v-model="email" type="email"
                        class="flex-1 p-4 border border-white/20 rounded-full bg-white/10 text-white backdrop-blur-md placeholder-white/60 newsletter-input"
                        placeholder="Tu email aquí..." :disabled="isSubscribing" required>
                    <button type="submit"
                        class="bg-gradient-cta border-none py-4 px-8 rounded-full text-white font-bold cursor-pointer transition-all duration-300 hover:transform hover:-translate-y-0.5 hover:shadow-md hover:shadow-[#ff0080]/40 newsletter-btn"
                        :disabled="isSubscribing">
                        {{ isSubscribing ? 'Enviando...' : 'Suscribirse' }}
                    </button>
                </form>
                <div v-if="subscriptionMessage" :class="[
                    'mt-4 p-3 rounded-lg transition-all duration-300',
                    isError ? 'bg-red-500/20 border border-red-500/50' : 'bg-green-500/20 border border-green-500/50'
                ]">
                    {{ subscriptionMessage }}
                </div>
            </div>
        </section>
    </div>
</template>

<style lang="scss" scoped>
.home-container {
    min-height: 100vh;
    color: white;
    padding-top: 0;
    padding-bottom: 0;
    margin-bottom: 0;
    display: flex;
    flex-direction: column;
    overflow-x: hidden;
}

/* Gradientes */
.bg-gradient-primary {
    background: linear-gradient(135deg, #0f0f23 0%, #1a1a2e 50%, #16213e 100%);
}

.bg-gradient-secondary {
    background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
}

.bg-gradient-dark {
    background: linear-gradient(135deg, #0f0f23 0%, #1a1a2e 100%);
}

.bg-gradient-btn {
    background: linear-gradient(45deg, #00ff88, #00ccff);
}

.bg-gradient-cta {
    background: linear-gradient(45deg, #ff0080, #00ff88);
}

/* Texto con gradientes */
.text-gradient {
    background: linear-gradient(45deg, #00ff88, #00ccff);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
    background-clip: text;
}

.text-gradient-hero {
    background: linear-gradient(45deg, #00ff88, #00ccff, #ff0080);
    -webkit-background-clip: text;
    -webkit-text-fill-color: transparent;
    background-clip: text;
    animation: glow 2s ease-in-out infinite alternate;
}

/* Animaciones */
@keyframes float {

    0%,
    100% {
        transform: translateY(-50%) rotate(0deg);
    }

    50% {
        transform: translateY(-60%) rotate(5deg);
    }
}

@keyframes glow {
    0% {
        text-shadow: 0 0 20px rgba(0, 255, 136, 0.5);
    }

    100% {
        text-shadow: 0 0 40px rgba(0, 204, 255, 0.8);
    }
}

@keyframes pulse {

    0%,
    100% {
        opacity: 0.3;
    }

    50% {
        opacity: 0.6;
    }
}

.animate-float {
    animation: float 3s ease-in-out infinite;
}

/* Animaciones al scroll */
.fade-in {
    opacity: 1;
    transform: translateY(0);
}

.fade-in.visible {
    opacity: 1;
    transform: translateY(0);
}

/* Estilos específicos para el carrusel */
.hide-scrollbar {
    -ms-overflow-style: none;
    scrollbar-width: none;
}

.hide-scrollbar::-webkit-scrollbar {
    display: none;
}

.scroll-snap-x-mandatory {
    scroll-snap-type: x mandatory;
}

.scroll-snap-align-start {
    scroll-snap-align: start;
}

</style>