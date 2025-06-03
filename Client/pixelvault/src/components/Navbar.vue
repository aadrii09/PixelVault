<script setup>
import { ref, computed } from 'vue'
import { useUserStore } from '../store/user.js'
import { useRoute, useRouter } from 'vue-router'

const userStore = useUserStore()
const route = useRoute()
const router = useRouter()

const isLoggedIn = computed(() => !!userStore.token)
const showProfileMenu = ref(false)
const isHomePage = computed(() => route.path === '/')
const isSobreNosotrosPage = computed(() => route.path === '/about')
const isProductoDetallePage = computed(() => route.name === 'ProductoDetalle')
const isProductosPlataformaPage = computed(() => route.name === 'ProductosPlataforma')
const isWishlistPage = computed(() => route.path === '/wishlist')

const logout = async () => {
    userStore.logout()
    router.push('/login')
    showProfileMenu.value = false
}

const goTo = (path) => {
    router.push(path)
    showProfileMenu.value = false
}
</script>

<template>
  <!-- Elementos específicos solo visibles en la página de inicio -->
  <div v-if="isHomePage" class="homepage-content">
    <!-- Fondo de imagen para la página de inicio, ahora incluye el navbar -->
    <div class="bg-image-overlay"></div>
    
    <!-- Barra de navegación común para todas las páginas, ahora dentro del contenedor de la homepage -->
    <nav  class="w-full flex justify-between items-center px-6 py-8 z-10 relative">
      <div class="flex space-x-20">
        <router-link to="/" class="font-bold text-white text-xl hover:underline ml-20">Inicio</router-link>
        <router-link to="/about" class="font-bold text-white text-xl hover:underline">Sobre Nosotros</router-link>
      </div>
      
      <!-- Logo en el centro -->
      <div class="absolute left-1/2 transform -translate-x-1/2 flex items-center h-full">
        <img src="/images/logo2.png" alt="PixelVault" id="logo" />
      </div>
      
      <div class="flex space-x-20">
        <router-link to="/carrito" class="font-bold text-white text-xl hover:underline">Carrito</router-link>
        <div class="relative">
          <button @click="showProfileMenu = !showProfileMenu" class="font-bold text-white text-xl focus:outline-none mr-20">Perfil</button>
          <div v-if="showProfileMenu" class="absolute right-0 mt-2 w-48 bg-gray-800 rounded shadow-lg z-50">
            <template v-if="!isLoggedIn">
              <button @click="goTo('/login')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Iniciar Sesión</button>
              <button @click="goTo('/register')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Registro</button>
            </template>
            <template v-else>
              <div class="px-4 py-2 text-white">Hola, {{ userStore.user?.nombre?.split(' ')[0] }}</div>
              <button @click="goTo('/wishlist')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Favoritos</button>
              <button @click="logout" class="block w-full text-left px-4 py-2 text-red-400 hover:bg-gray-700">Cerrar Sesión</button>
            </template>
          </div>
        </div>
      </div>
    </nav>
    
    <!-- Contenido central -->
    <div class="flex flex-col items-center justify-center flex-grow py-20">
      <h1 class="text-7xl font-bold text-white mb-6" style="text-shadow: 0 2px 8px #000">PIXEL VAULT</h1>
      <a href="#productos-destacados" class="bg-white bg-opacity-20 text-white text-xl font-medium rounded-full px-10 py-2 hover:bg-opacity-30 transition-all duration-200">
        Explora
      </a>
    </div>
    
    <!-- Admin Panel centrado -->
    <div v-if="userStore.user?.esAdmin" class="flex justify-center mb-8">
      <router-link to="/admin" class="admin-panel-btn relative overflow-hidden text-2xl font-bold rounded-md px-16 py-4 shadow-lg hover:brightness-110 transition-all duration-200">
        <span class="admin-text">ADMIN PANEL</span>
      </router-link>
    </div>
  </div>

  <!-- Elementos específicos para la página Sobre Nosotros -->
  <div v-else-if="isSobreNosotrosPage" class="sobre-nosotros-content">
    <!-- Fondo de imagen para la página Sobre Nosotros -->
    <div class="sobre-nosotros-bg-overlay"></div>
    
    <!-- Navbar para Sobre Nosotros -->
    <nav class="w-full flex justify-between items-center px-6 py-8 z-10 relative">
      <div class="flex space-x-20">
        <router-link to="/" class="font-bold text-white text-xl hover:underline ml-20">Inicio</router-link>
        <router-link to="/about" class="font-bold text-white text-xl hover:underline">Sobre Nosotros</router-link>
      </div>
      
      <!-- Logo en el centro -->
      <div class="absolute left-1/2 transform -translate-x-1/2 flex items-center h-full">
        <img src="/images/logo2.png" alt="PixelVault" id="logo" />
      </div>
      
      <div class="flex space-x-20">
        <router-link to="/carrito" class="font-bold text-white text-xl hover:underline">Carrito</router-link>
        <div class="relative">
          <button @click="showProfileMenu = !showProfileMenu" class="font-bold text-white text-xl focus:outline-none mr-20">Perfil</button>
          <div v-if="showProfileMenu" class="absolute right-0 mt-2 w-48 bg-gray-800 rounded shadow-lg z-50">
            <template v-if="!isLoggedIn">
              <button @click="goTo('/login')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Iniciar Sesión</button>
              <button @click="goTo('/register')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Registro</button>
            </template>
            <template v-else>
              <div class="px-4 py-2 text-white">Hola, {{ userStore.user?.nombre?.split(' ')[0] }}</div>
              <button @click="goTo('/wishlist')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Favoritos</button>
              <button @click="logout" class="block w-full text-left px-4 py-2 text-red-400 hover:bg-gray-700">Cerrar Sesión</button>
            </template>
          </div>
        </div>
      </div>
    </nav>
    
    <!-- Contenido central de Sobre Nosotros -->
    <div class="flex flex-col items-center justify-center flex-grow py-20">
      <h1 class="text-7xl font-bold text-white mb-6" style="text-shadow: 0 2px 8px #000">Sobre nosotros</h1>
    </div>
  </div>

  <!-- Elementos específicos para la página ProductosPlataforma -->
  <div v-else-if="isProductosPlataformaPage" class="productos-plataforma-content">
    <!-- Fondo de imagen para la página ProductosPlataforma -->
    <div class="productos-plataforma-bg-overlay"></div>
    
    <!-- Navbar para ProductosPlataforma -->
    <nav class="w-full flex justify-between items-center px-6 py-8 z-10 relative">
      <div class="flex space-x-20">
        <router-link to="/" class="font-bold text-white text-xl hover:underline ml-20">Inicio</router-link>
        <router-link to="/about" class="font-bold text-white text-xl hover:underline">Sobre Nosotros</router-link>
      </div>
      
      <!-- Logo en el centro -->
      <div class="absolute left-1/2 transform -translate-x-1/2 flex items-center h-full">
        <img src="/images/logo2.png" alt="PixelVault" id="logo" />
      </div>
      
      <div class="flex space-x-20">
        <router-link to="/carrito" class="font-bold text-white text-xl hover:underline">Carrito</router-link>
        <div class="relative">
          <button @click="showProfileMenu = !showProfileMenu" class="font-bold text-white text-xl focus:outline-none mr-20">Perfil</button>
          <div v-if="showProfileMenu" class="absolute right-0 mt-2 w-48 bg-gray-800 rounded shadow-lg z-50">
            <template v-if="!isLoggedIn">
              <button @click="goTo('/login')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Iniciar Sesión</button>
              <button @click="goTo('/register')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Registro</button>
            </template>
            <template v-else>
              <div class="px-4 py-2 text-white">Hola, {{ userStore.user?.nombre?.split(' ')[0] }}</div>
              <button @click="goTo('/wishlist')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Favoritos</button>
              <button @click="logout" class="block w-full text-left px-4 py-2 text-red-400 hover:bg-gray-700">Cerrar Sesión</button>
            </template>
          </div>
        </div>
      </div>
    </nav>
    
    <!-- Contenido central de ProductosPlataforma -->
    <div class="flex flex-col items-center justify-center flex-grow py-20">
      <h1 class="text-7xl font-bold text-white mb-6" style="text-shadow: 0 2px 8px #000">Productos Plataforma</h1>
    </div>
  </div>

  <!-- Elementos específicos para la página ProductoDetalle -->
  <div v-else-if="isProductoDetallePage || isWishlistPage" class="producto-detalle-content">
    <!-- Fondo de imagen para la página ProductoDetalle -->
    <div class="producto-detalle-bg-overlay"></div>
    
    <!-- Navbar para ProductoDetalle -->
    <nav class="w-full flex justify-between items-center px-6 py-8 z-10 relative">
      <div class="flex space-x-20">
        <router-link to="/" class="font-bold text-white text-xl hover:underline ml-20">Inicio</router-link>
        <router-link to="/about" class="font-bold text-white text-xl hover:underline">Sobre Nosotros</router-link>
      </div>
      
      <!-- Logo en el centro -->
      <div class="absolute left-1/2 transform -translate-x-1/2 flex items-center h-full">
        <img src="/images/logo2.png" alt="PixelVault" id="logo" />
      </div>
      
      <div class="flex space-x-20">
        <router-link to="/carrito" class="font-bold text-white text-xl hover:underline">Carrito</router-link>
        <div class="relative">
          <button @click="showProfileMenu = !showProfileMenu" class="font-bold text-white text-xl focus:outline-none mr-20">Perfil</button>
          <div v-if="showProfileMenu" class="absolute right-0 mt-2 w-48 bg-gray-800 rounded shadow-lg z-50">
            <template v-if="!isLoggedIn">
              <button @click="goTo('/login')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Iniciar Sesión</button>
              <button @click="goTo('/register')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Registro</button>
            </template>
            <template v-else>
              <div class="px-4 py-2 text-white">Hola, {{ userStore.user?.nombre?.split(' ')[0] }}</div>
              <button @click="goTo('/wishlist')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Favoritos</button>
              <button @click="logout" class="block w-full text-left px-4 py-2 text-red-400 hover:bg-gray-700">Cerrar Sesión</button>
            </template>
          </div>
        </div>
      </div>
    </nav>
    
    <!-- Contenido central de ProductoDetalle -->
    <div class="flex flex-col items-center justify-center flex-grow py-20">
      <h1 class="text-7xl font-bold text-white mb-6" style="text-shadow: 0 2px 8px #000">{{ isWishlistPage ? 'Mi Lista de Deseos' : 'Producto Detalle' }}</h1>
    </div>
  </div>

  <!-- Navbar para otras páginas que no son la página de inicio, Sobre Nosotros, ProductosPlataforma ni ProductoDetalle -->
  <nav v-else id="navbar" class="w-full flex justify-between items-center px-6 py-10 ">
    <div class="flex space-x-20">
      <router-link to="/" class="font-bold text-white text-xl hover:underline ml-20">Inicio</router-link>
      <router-link to="/about" class="font-bold text-white text-xl hover:underline">Sobre Nosotros</router-link>
    </div>
    
    <!-- Logo en el centro -->
    <div class="absolute left-1/2 transform -translate-x-1/2 flex items-center h-full">
      <img src="/images/logo2.png" alt="PixelVault" id="logo1" />
    </div>
    
    <div class="flex space-x-20">
      <router-link to="/carrito" class="font-bold text-white text-xl hover:underline ">Carrito</router-link>
      <div class="relative">
        <button @click="showProfileMenu = !showProfileMenu" class="font-bold text-white text-xl focus:outline-none mr-20">Perfil</button>
        <div v-if="showProfileMenu" class="absolute right-0 mt-2 w-48 bg-gray-800 rounded shadow-lg z-50">
          <template v-if="!isLoggedIn">
            <button @click="goTo('/login')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Iniciar Sesión</button>
            <button @click="goTo('/register')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Registro</button>
          </template>
          <template v-else>
            <div class="px-4 py-2 text-white">Hola, {{ userStore.user?.nombre?.split(' ')[0] }}</div>
            <button @click="goTo('/wishlist')" class="block w-full text-left px-4 py-2 text-white hover:bg-gray-700">Favoritos</button>
            <button @click="logout" class="block w-full text-left px-4 py-2 text-red-400 hover:bg-gray-700">Cerrar Sesión</button>
          </template>
        </div>
      </div>
    </div>
  </nav>
</template>

<style lang="scss" scoped>
#logo {
    height: 10rem !important;
    width: 11rem !important;
    margin-top: 4rem !important;
}
#logo1 {
    height: 8.5rem !important;
    width: 10rem !important;
    margin-bottom: 0.5rem !important;
}
// Estilos para el navbar
.transparent-bg {
  background: transparent;
}

.solid-bg {
  background-color: rgba(0, 0, 0, 0.7);
  backdrop-filter: blur(5px);
}

#navbar {
  min-height: 7rem;
  background: linear-gradient(to right, rgba(15, 15, 26, 0.95), rgba(22, 22, 48, 0.95), rgba(16, 16, 42, 0.95));
  box-shadow: 0 4px 15px rgba(66, 220, 255, 0.2);
  border-bottom: 1px solid rgba(83, 134, 244, 0.2);
  backdrop-filter: blur(10px);
  position: relative;
  
  &::after {
    content: '';
    position: absolute;
    left: 0;
    right: 0;
    bottom: 0;
    height: 1px;
    background: linear-gradient(to right, 
      transparent, 
      rgba(0, 255, 255, 0.5), 
      rgba(128, 0, 255, 0.5), 
      rgba(0, 255, 255, 0.5), 
      transparent
    );
    opacity: 0.6;
  }
}

// Estilos para elementos de la página de inicio
.homepage-content {
  min-height: 80vh;
  width: 100%;
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.bg-image-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-image: url('/images/parteArribaHome.png');
  background-size: cover;
  background-position: center top;
  background-repeat: no-repeat;
  z-index: -1;
}

// Estilos para la página Sobre Nosotros
.sobre-nosotros-content {
  min-height: 80vh;
  width: 100%;
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.sobre-nosotros-bg-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-image: url('../../public/images/prueba4.png') !important;
  background-size: cover;
  background-position: center top;
  background-repeat: no-repeat;
  z-index: -1;
}

// Estilos para la página ProductosPlataforma
.productos-plataforma-content {
  min-height: 80vh;
  width: 100%;
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.productos-plataforma-bg-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-image: url('../../public/images/astrobot.svg') !important;
  background-size: cover;
  background-position: center top;
  background-repeat: no-repeat;
  z-index: -1;
}

// Estilos para la página ProductoDetalle
.producto-detalle-content {
  min-height: 80vh;
  width: 100%;
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.producto-detalle-bg-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-image: url('../../public/images/Nav.svg') !important;
  background-size: cover;
  background-position: center top;
  background-repeat: no-repeat;
  z-index: -1;
}

.admin-panel-btn {
  background: repeating-linear-gradient(
    -45deg,
    #ffdd00,
    #ffdd00 10px,
    #000000 10px,
    #000000 20px
  );
  border: 3px solid #000;
  box-shadow: 0 0 0 3px #ffdd00, 0 5px 15px rgba(0,0,0,0.4);
  position: relative;
  
  &::before, &::after {
    content: '';
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    width: 30px;
    height: 30px;
    background-color: #ff3d00;
    border-radius: 50%;
    box-shadow: 0 0 10px 2px rgba(255, 61, 0, 0.7);
    border: 2px solid #000;
  }
  
  &::before {
    left: 10px;
    background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='%23000000'%3E%3Cpath d='M13 14h-2v-4h2v4zm0 4h-2v-2h2v2zm-1-15c-5.52 0-10 4.48-10 10s4.48 10 10 10 10-4.48 10-10-4.48-10-10-10zm0 18c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8-3.59 8-8 8z'/%3E%3C/svg%3E");
    background-size: 80%;
    background-repeat: no-repeat;
    background-position: center;
    animation: pulse 1.5s infinite alternate;
  }
  
  &::after {
    right: 10px;
    background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='%23000000'%3E%3Cpath d='M12 5.99L19.53 19H4.47L12 5.99M12 2L1 21h22L12 2zm1 14h-2v2h2v-2zm0-6h-2v4h2v-4z'/%3E%3C/svg%3E");
    background-size: 80%;
    background-repeat: no-repeat;
    background-position: center;
    animation: pulse 1.5s infinite alternate-reverse;
  }
  
  @keyframes pulse {
    0% {
      transform: translateY(-50%) scale(1);
      box-shadow: 0 0 10px 2px rgba(255, 61, 0, 0.7);
    }
    100% {
      transform: translateY(-50%) scale(1.15);
      box-shadow: 0 0 15px 4px rgba(255, 61, 0, 0.9);
    }
  }
  
  &:hover {
    transform: scale(1.03);
  }
}

.admin-text {
  position: relative;
  z-index: 10;
  color: #ff0000;
  text-shadow: 0 0 2px #000, 0 0 3px #000, 0 0 5px #000;
  background-color: rgba(0, 0, 0, 0.7);
  padding: 5px 15px;
  border-radius: 4px;
  letter-spacing: 1px;
  border: 1px solid #ffdd00;
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
</style>