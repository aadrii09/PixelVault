<script setup>
// Importamos el navbar global, pero ahora no necesitamos añadir contenido duplicado
// ya que el Navbar.vue ahora tiene un tratamiento especial para la ruta /about
import Navbar from '../components/Navbar.vue';
import Footer from '../components/Footer.vue';
</script>

<template>
  <!-- El contenido principal, el navbar y el header ya están en el componente Navbar.vue -->
  <div class="sobre-nosotros-container">
    <!-- Contenido replicado de la imagen -->
    <div class="contenido-principal">
      <div class="grid-layout">
        <!-- Primera fila -->
        <div class="imagen-teclado">
          <img src="../../public/images/ImagenArribaIzquierdaSobreNosotros.png" alt="Teclado gaming" />
        </div>
        
        <div class="texto-bienvenida">
          <div class="texto-box">
            <h2 class="text-lg">Bienvenido a PixelVault.</h2>
            <p class="text-sm">Este sitio web de videojuegos y consolas es operado por nuestra empresa. Al navegar por nuestra plataforma, los términos "nosotros" o "PixelVault" se refieren a nuestra compañía. Al utilizar nuestros servicios y recursos, aceptas cumplir con todas las condiciones y políticas aquí establecidas.</p>
          </div>
        </div>

        <!-- Segunda fila -->
        <div class="texto-exploracion">
          <div class="texto-box">
            <p class="text-sm">Al explorar nuestro catálogo virtual o adquirir cualquiera de nuestros productos gaming, automáticamente estás participando en nuestra experiencia PixelVault y aceptas las reglas del juego establecidas en estas condiciones, incluyendo todas las políticas adicionales que encontrarás enlazadas en nuestra plataforma.</p>
          </div>
        </div>
        
        <div class="imagen-pantallas">
          <img src="../../public/images/ImagenAbajoDerechaSobreNosotros.png" alt="Pantallas gaming" />
        </div>
      </div>
    </div>
    
    <!-- Sección de valores y contacto -->
    <div class="valores-contacto">
      <div class="mt-16">
        <h2 class="text-2xl font-semibold mb-6 text-center text-white">Nuestros Valores</h2>
        
        <div class="grid grid-cols-1 md:grid-cols-3 gap-8 mt-8">
          <div class="bg-gray-800 bg-opacity-50 p-6 rounded-lg shadow-md text-white border border-blue-900">
            <h3 class="text-xl font-medium mb-3">Pasión por los Juegos</h3>
            <p>Somos jugadores como tú. Entendemos la emoción de un nuevo lanzamiento y la satisfacción de completar ese juego que tanto te ha costado.</p>
          </div>
          
          <div class="bg-gray-800 bg-opacity-50 p-6 rounded-lg shadow-md text-white border border-blue-900">
            <h3 class="text-xl font-medium mb-3">Calidad Garantizada</h3>
            <p>Todos nuestros productos son originales y vienen con garantía. No vendemos copias ni versiones piratas.</p>
          </div>
          
          <div class="bg-gray-800 bg-opacity-50 p-6 rounded-lg shadow-md text-white border border-blue-900">
            <h3 class="text-xl font-medium mb-3">Comunidad</h3>
            <p>Creemos en la creación de una comunidad de jugadores. Organizamos eventos, torneos y nos encanta escuchar vuestras opiniones.</p>
          </div>
        </div>
      </div>
      
      <div class="mt-16 text-center text-white">
        <h2 class="text-2xl font-semibold mb-6">Contacta con Nosotros</h2>
        <p class="mb-4">¿Tienes alguna pregunta o sugerencia? Estamos aquí para ayudarte.</p>
        
        <div class="flex justify-center space-x-4 mb-10">
          <a href="mailto:info@pixelvault.com" class="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700">
            Envíanos un Email
          </a>
          <a href="#" class="bg-gray-700 text-white px-6 py-2 rounded hover:bg-gray-600">
            Síguenos en Redes
          </a>
        </div>
      </div>
    </div>
    
    <!-- Añadimos el componente footer -->
    <Footer />
  </div>
</template>

<style scoped>
.sobre-nosotros-container {
  min-height: 100vh;
  background-color: #0e0b30;
  color: white;
}

.contenido-principal {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto; 
}

.grid-layout {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 2rem;
  width: 100%;
  max-width: 1100px;
  margin: 0 auto;
}

.texto-box {
  padding: 2rem;
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.texto-box h2 {
  font-size: 1.8rem;
  margin-bottom: 1.2rem;
}

.texto-box p {
  line-height: 1.7;
  font-size: 1.2rem;
}

.imagen-teclado, 
.imagen-pantallas,
.texto-bienvenida,
.texto-exploracion {
  width: 100%;
  height: 350px;
  overflow: hidden;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 8px;
}

.texto-bienvenida,
.texto-exploracion {
  background-color: #101041;
}

.imagen-teclado, 
.imagen-pantallas {
  background-color: rgba(0, 0, 0, 0.5);
  padding: 0;
  overflow: hidden;
}

.imagen-teclado img, 
.imagen-pantallas img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 8px;
}

@media (max-width: 768px) {
  .grid-layout {
    grid-template-columns: 1fr;
  }
}

.valores-contacto {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 2rem;
}
</style>