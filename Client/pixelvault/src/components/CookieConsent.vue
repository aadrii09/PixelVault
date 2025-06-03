<template>
  <div v-if="showConsent" class="cookie-consent-wrapper">
    <div class="cookie-consent">
      <div class="cookie-content">
        <div class="cookie-icon">
          <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10"/>
            <circle cx="8" cy="8" r="1"/>
            <circle cx="12" cy="12" r="1"/>
            <circle cx="16" cy="16" r="1"/>
            <circle cx="16" cy="8" r="1"/>
            <circle cx="8" cy="16" r="1"/>
          </svg>
        </div>
        <div class="cookie-text">          <h3>Uso de cookies</h3>
          <p>Este sitio utiliza cookies para mejorar tu experiencia de navegación. Al continuar navegando, aceptas nuestro uso de cookies.</p>
          <router-link to="/cookies-policy" class="cookie-link">Más información</router-link>
        </div>
      </div>
      <div class="cookie-buttons">
        <button class="cookie-btn cookie-btn-secondary" @click="rejectCookies">Rechazar</button>
        <button class="cookie-btn cookie-btn-primary" @click="acceptCookies">Aceptar</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';

const showConsent = ref(false);

onMounted(() => {
  // Comprobar si ya se ha aceptado las cookies
  const cookiesAccepted = localStorage.getItem('cookies-accepted');
  if (!cookiesAccepted) {
    // Mostrar el modal después de un pequeño retraso para mejor experiencia
    setTimeout(() => {
      showConsent.value = true;
    }, 1000);
  }
});

const acceptCookies = () => {
  localStorage.setItem('cookies-accepted', 'true');
  showConsent.value = false;
};

const rejectCookies = () => {
  localStorage.setItem('cookies-accepted', 'false');
  showConsent.value = false;
};
</script>

<style scoped>
.cookie-consent-wrapper {
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  z-index: 9999;
  display: flex;
  justify-content: center;
  padding: 1rem;
  background-color: rgba(0, 0, 0, 0.8);
  animation: slideUp 0.5s ease-in-out forwards;
}

.cookie-consent {
  display: flex;
  flex-direction: column;
  max-width: 1200px;
  width: 100%;
  background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
  border: 1px solid rgba(255, 255, 255, 0.2);
  border-radius: 1rem;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
  overflow: hidden;
  color: white;
  padding: 1.5rem;
}

@media (min-width: 768px) {
  .cookie-consent {
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
  }
  
  .cookie-buttons {
    margin-top: 0;
    margin-left: 1.5rem;
    flex-shrink: 0;
    display: flex;
    flex-direction: row;
  }
}

.cookie-content {
  display: flex;
  align-items: flex-start;
}

.cookie-icon {
  margin-right: 1rem;
  color: #00ff88;
  flex-shrink: 0;
}

.cookie-text h3 {
  font-size: 1.25rem;
  font-weight: bold;
  margin-bottom: 0.5rem;
  color: #00ccff;
}

.cookie-text p {
  font-size: 0.9rem;
  margin-bottom: 0.5rem;
  line-height: 1.5;
  color: rgba(255, 255, 255, 0.9);
}

.cookie-link {
  color: #00ff88;
  text-decoration: underline;
  font-size: 0.85rem;
  transition: color 0.3s;
}

.cookie-link:hover {
  color: #00ccff;
}

.cookie-buttons {
  display: flex;
  margin-top: 1rem;
  gap: 0.75rem;
}

.cookie-btn {
  padding: 0.5rem 1.25rem;
  border-radius: 2rem;
  font-weight: bold;
  font-size: 0.9rem;
  cursor: pointer;
  transition: all 0.3s;
  border: none;
}

.cookie-btn-primary {
  background: linear-gradient(45deg, #00ff88, #00ccff);
  color: #16213e;
}

.cookie-btn-primary:hover {
  box-shadow: 0 0 15px rgba(0, 255, 136, 0.5);
  transform: translateY(-2px);
}

.cookie-btn-secondary {
  background: rgba(255, 255, 255, 0.1);
  color: white;
  border: 1px solid rgba(255, 255, 255, 0.3);
}

.cookie-btn-secondary:hover {
  background: rgba(255, 255, 255, 0.2);
}

@keyframes slideUp {
  from {
    transform: translateY(100%);
    opacity: 0;
  }
  to {
    transform: translateY(0);
    opacity: 1;
  }
}
</style>
