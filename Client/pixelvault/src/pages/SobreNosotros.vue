<script setup>
// Importamos el navbar global, pero ahora no necesitamos añadir contenido duplicado
// ya que el Navbar.vue ahora tiene un tratamiento especial para la ruta /about
import Navbar from '../components/Navbar.vue';
</script>

<template>
  <!-- El contenido principal, el navbar y el header ya están en el componente Navbar.vue -->
  <div class="sobre-nosotros-container">
    <!-- Aquí podemos añadir el contenido adicional de la página -->
    <div class="contenido-principal">
      <!-- Espacio reservado para contenido específico -->
    </div>
  </div>
</template>

<style scoped>
.sobre-nosotros-container {
  min-height: 100vh;
  /* No necesitamos padding-top porque el navbar ya incluye el espacio necesario */
}

.contenido-principal {
  padding: 2rem;
  max-width: 1200px;
  margin: 8rem auto 2rem;
  /* Margen superior para que el contenido aparezca debajo del título grande */
}
</style>