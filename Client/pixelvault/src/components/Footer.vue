<template>
  <footer class="w-full py-8 px-4 md:px-6 border-t border-[#00ccff]/30 text-center relative overflow-hidden" 
         style="background: linear-gradient(to bottom, rgba(22, 22, 48, 0.95), rgba(15, 15, 26, 0.98)); backdrop-filter: blur(10px);">
    
    <!-- Efectos de fondo -->
    <div class="absolute inset-0 z-0">
      <div class="absolute top-0 left-1/4 w-40 h-40 bg-[#00ccff]/5 rounded-full filter blur-3xl"></div>
      <div class="absolute bottom-0 right-1/4 w-40 h-40 bg-[#00ff88]/5 rounded-full filter blur-3xl"></div>
      <div class="absolute -top-20 -right-20 w-60 h-60 bg-[#8b5cf6]/5 rounded-full filter blur-3xl"></div>
    </div>
    
    <div class="max-w-5xl mx-auto flex flex-col items-center justify-center relative z-10">
      <!-- Redes sociales con líneas -->
      <div class="flex items-center w-full mb-6 md:mb-8 justify-center">
        <div class="hidden sm:block flex-1 border-t border-[#00ccff]/20"></div>
        <div class="flex justify-center space-x-4 md:space-x-8 px-4 mx-auto">
          <a href="#" class="text-gray-400 hover:text-[#00ccff] transition-all duration-300 transform hover:scale-110 hover:shadow-glow">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 md:h-7 md:w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path d="M12 2.163c3.204 0 3.584.012 4.85.07 3.252.148 4.771 1.691 4.919 4.919.058 1.265.069 1.645.069 4.849 0 3.205-.012 3.584-.069 4.849-.149 3.225-1.664 4.771-4.919 4.919-1.266.058-1.644.07-4.85.07-3.204 0-3.584-.012-4.849-.07-3.26-.149-4.771-1.699-4.919-4.92-.058-1.265-.07-1.644-.07-4.849 0-3.204.013-3.583.07-4.849.149-3.227 1.664-4.771 4.919-4.919 1.266-.057 1.645-.069 4.849-.069z"/>
            </svg>
          </a>
          <a href="#" class="text-gray-400 hover:text-[#00ccff] transition-all duration-300 transform hover:scale-110 hover:shadow-glow">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 md:h-7 md:w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path d="M24 4.557c-.883.392-1.832.656-2.828.775 1.017-.609 1.798-1.574 2.165-2.724-.951.564-2.005.974-3.127 1.195-.897-.957-2.178-1.555-3.594-1.555-3.179 0-5.515 2.966-4.797 6.045-4.091-.205-7.719-2.165-10.148-5.144-1.29 2.213-.669 5.108 1.523 6.574-.806-.026-1.566-.247-2.229-.616-.054 2.281 1.581 4.415 3.949 4.89-.693.188-1.452.232-2.224.084.626 1.956 2.444 3.379 4.6 3.419-2.07 1.623-4.678 2.348-7.29 2.04 2.179 1.397 4.768 2.212 7.548 2.212 9.142 0 14.307-7.721 13.995-14.646.962-.695 1.797-1.562 2.457-2.549z"/>
            </svg>
          </a>
          <a href="#" class="text-gray-400 hover:text-[#00ccff] transition-all duration-300 transform hover:scale-110 hover:shadow-glow">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 md:h-7 md:w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path d="M9 8h-3v4h3v12h5v-12h3.642l.358-4h-4v-1.667c0-.955.192-1.333 1.115-1.333h2.885v-5h-3.808c-3.596 0-5.192 1.583-5.192 4.615v3.385z"/>
            </svg>
          </a>
          <a href="#" class="text-gray-400 hover:text-[#00ccff] transition-all duration-300 transform hover:scale-110 hover:shadow-glow">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 md:h-7 md:w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path d="M19.812 5.418c.861.23 1.538.907 1.768 1.768C21.998 8.746 22 12 22 12s0 3.255-.418 4.814a2.504 2.504 0 0 1-1.768 1.768c-1.56.419-7.814.419-7.814.419s-6.255 0-7.814-.419a2.505 2.505 0 0 1-1.768-1.768C2 15.255 2 12 2 12s0-3.255.417-4.814a2.507 2.507 0 0 1 1.768-1.768C5.744 5 11.998 5 11.998 5s6.255 0 7.814.418ZM15.194 12 10 15V9l5.194 3Z" />
            </svg>
          </a>
        </div>
        <div class="hidden sm:block flex-1 border-t border-[#00ccff]/20"></div>
      </div>
      
      <!-- Logo y enlaces - Responsive layout -->
      <div class="grid grid-cols-1 sm:grid-cols-3 w-full items-center justify-between gap-6 md:gap-8">
        <!-- Enlaces izquierda -->
        <div class="flex flex-col space-y-3 text-gray-300 text-base md:text-lg order-2 sm:order-1">
          <h3 class="text-[#00ff88] text-lg font-medium mb-1">Navegación</h3>
          <router-link to="/" class="hover:text-[#00ccff] transition-all duration-300 transform hover:translate-x-1">Inicio</router-link>
          <router-link to="/about" class="hover:text-[#00ccff] transition-all duration-300 transform hover:translate-x-1">Sobre Nosotros</router-link>
          <router-link to="/carrito" class="hover:text-[#00ccff] transition-all duration-300 transform hover:translate-x-1">Carrito</router-link>
        </div>
        
        <!-- Logo central -->
        <div class="flex-shrink-0 mx-auto flex justify-center mb-4 sm:mb-0 order-1 sm:order-2">
          <img src="/images/logo2.png" alt="PixelVault Logo" class="w-28 h-28 md:w-32 md:h-32 object-contain logo-glow" />
        </div>
        
        <!-- Enlaces derecha -->
        <div class="flex flex-col space-y-3 text-gray-300 text-base md:text-lg order-3">
          <h3 class="text-[#00ff88] text-lg font-medium mb-1">Cuenta</h3>
          <router-link to="/login" class="hover:text-[#00ccff] transition-all duration-300 transform hover:translate-x-1">Iniciar Sesión</router-link>
          <router-link to="/register" class="hover:text-[#00ccff] transition-all duration-300 transform hover:translate-x-1">Registro</router-link>
          <router-link to="/wishlist" class="hover:text-[#00ccff] transition-all duration-300 transform hover:translate-x-1">Mis Favoritos</router-link>
        </div>
      </div>
      
      <!-- Separador neón -->
      <div class="w-full max-w-2xl mx-auto my-6 h-px bg-gradient-to-r from-transparent via-[#00ccff]/30 to-transparent"></div>
      
      <!-- Copyright -->
      <div class="mt-4 text-sm text-gray-400">
        <p class="mb-2">© 2025 PixelVault. Todos los derechos reservados.</p>
        <div class="flex flex-wrap justify-center gap-x-4 gap-y-2 mt-2">
          <router-link to="/cookies" class="hover:text-[#00ccff] transition-colors">Política de Cookies</router-link>
          <span class="text-gray-600 hidden sm:inline">|</span>
          <router-link to="#" class="hover:text-[#00ccff] transition-colors">Privacidad</router-link>
          <span class="text-gray-600 hidden sm:inline">|</span>
          <router-link to="#" class="hover:text-[#00ccff] transition-colors">Términos y condiciones</router-link>
        </div>
      </div>
    </div>
  </footer>
</template>

<script setup lang="ts">
// No additional logic needed
</script>

<style scoped lang="scss">
/* Efectos neón y animaciones */
.shadow-glow {
  filter: drop-shadow(0 0 0.5rem rgba(0, 204, 255, 0.5));
}

.hover\:shadow-glow:hover {
  filter: drop-shadow(0 0 0.75rem rgba(0, 204, 255, 0.7));
}

.logo-glow {
  filter: drop-shadow(0 0 8px rgba(0, 255, 136, 0.4));
  animation: pulse-glow 3s infinite alternate;
}

@keyframes pulse-glow {
  0% {
    filter: drop-shadow(0 0 5px rgba(0, 204, 255, 0.3));
  }
  50% {
    filter: drop-shadow(0 0 10px rgba(0, 255, 136, 0.5));
  }
  100% {
    filter: drop-shadow(0 0 7px rgba(0, 204, 255, 0.4));
  }
}

/* Optimizaciones para diferentes tamaños de pantalla */
@media (max-width: 360px) {
  .flex-col-xs {
    flex-direction: column;
  }
  
  .space-y-2-xs > * + * {
    margin-top: 0.5rem;
  }
  
  .logo-glow {
    width: 5rem;
    height: 5rem;
  }
}

/* Efectos de hover mejorados */
a {
  position: relative;
  white-space: nowrap;
}

a::after {
  content: '';
  position: absolute;
  width: 0;
  height: 1px;
  bottom: -2px;
  left: 0;
  background: linear-gradient(to right, #00ccff, #00ff88);
  transition: width 0.3s ease;
  opacity: 0;
}

a:hover::after {
  width: 100%;
  opacity: 1;
}
</style>
