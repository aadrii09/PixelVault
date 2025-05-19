<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useUserStore } from '../store/user';
import { login } from '../api/auth';

const email = ref('');
const password = ref('');
const error = ref('');
const router = useRouter();
const userStore = useUserStore();
const showModal = ref(false);
const recoveryEmail = ref('');

const handleLogin = async () => {
    try {
        const response = await login(email.value, password.value);
        //descodificar token con jwt-decode

        const decoded = JSON.parse(atob(response.token.split('.')[1]));
        console.log('Decoded JWT:', decoded); // Para depuración

        userStore.login({
            nombre: decoded?.email?.split('@')[0],
            email: decoded.email,
            esAdmin: decoded.esAdmin === true || decoded.esAdmin === 'True' || decoded.esAdmin === 'true' || decoded.esAdmin === 1 || decoded.esAdmin === '1',
            id: parseInt(decoded.sub),
        },
        response.token
        ); // Corregido: cerrado el paréntesis
        
        router.push('/');

    } catch (error) {
        console.error('Error al iniciar sesión:', error);
        error.value = 'Error al iniciar sesión';
    }
};

const openModal = () => {
  showModal.value = true;
  recoveryEmail.value = '';
};
const closeModal = () => {
  showModal.value = false;
};
const handleRecovery = () => {
  closeModal();
  alert('Revise su Correo electrónico');
};
</script>

<template>
  <div class="login-bg">
    <div class="login-container">
      <div class="login-form">
        <h2>Iniciar Sesión</h2>
        <form @submit.prevent="handleLogin">
          <input v-model="email" type="email" placeholder="Correo electrónico" required />
          <input v-model="password" type="password" placeholder="Contraseña" required />
          <a class="forgot" href="#" @click.prevent="openModal">Recuperar contraseña</a>
          <button type="submit">Iniciar Sesión</button>
        </form>
        <div class="not-registered">
          <a @click.prevent="router.push('/register')" href="#" style="cursor:pointer;">¿No tienes cuenta? Regístrate</a>
        </div>
        <p v-if="error" class="error">{{ error }}</p>
      </div>
      <div class="login-image">
        <img src="../../public/images/mandoLogin.png" alt="Gamepad" />
      </div>
    </div>
  </div>

  <!-- Modal Recuperar Contraseña -->
  <div v-if="showModal" class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
    <div class="bg-white p-6 rounded-lg shadow-lg max-w-sm w-full relative">
      <button @click="closeModal" class="absolute top-2 right-2 text-gray-400 hover:text-gray-600 text-xl">&times;</button>
      <h3 class="text-lg font-bold mb-4">Recuperar Contraseña</h3>
      <p class="mb-4">Escribe tu Correo Electrónico para recuperar la contraseña</p>
      <input v-model="recoveryEmail" type="email" placeholder="Correo electrónico" class="w-full border-b border-gray-400 focus:outline-none py-2 mb-6" required />
      <div class="flex justify-end">
        <button @click="handleRecovery" class="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700">Enviar</button>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.login-bg {
  min-height: 100vh;
  width: 100vw;
  background: url('../../public/images/fondoRegistroLogin.png') center/cover no-repeat;
  display: flex;
  align-items: center;
  justify-content: center;
}

.login-container {
  display: flex;
  background: transparent;
  border-radius: 24px;
  box-shadow: 0 8px 32px 0 rgba(31, 38, 135, 0.37);
  overflow: hidden;
  max-width: 700px;
  width: 90vw;
}

.login-form {
  background: #fff;
  padding: 40px 32px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  width: 340px;
  border-top-left-radius: 24px;
  border-bottom-left-radius: 24px;

  h2 {
    font-size: 1.5rem;
    font-weight: bold;
    margin-bottom: 24px;
    text-align: left;
  }

  form {
    display: flex;
    flex-direction: column;
    gap: 16px;
  }

  input {
    padding: 12px;
    border: 1px solid #e0e0e0;
    border-radius: 8px;
    font-size: 1rem;
    outline: none;
    transition: border 0.2s;
    &:focus {
      border: 1.5px solid #3b82f6;
    }
  }

  .forgot {
    font-size: 0.85rem;
    color: #888;
    margin-bottom: 8px;
    text-align: right;
    display: block;
    text-decoration: none;
    &:hover {
      text-decoration: underline;
    }
  }

  button {
    background: #3b82f6;
    color: #fff;
    padding: 12px;
    border: none;
    border-radius: 8px;
    font-size: 1rem;
    font-weight: 600;
    cursor: pointer;
    margin-top: 8px;
    transition: background 0.2s;
    &:hover {
      background: #2563eb;
    }
  }

  .not-registered {
    margin-top: 16px;
    font-size: 0.85rem;
    color: #888;
    text-align: left;
  }

  .error {
    color: #e53e3e;
    margin-top: 12px;
    font-size: 0.95rem;
  }
}

.login-image {
  background: #181c2f;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 320px;
  border-top-right-radius: 24px;
  border-bottom-right-radius: 24px;
  img {
    width: 90%;
    max-width: 260px;
    border-radius: 18px;
    box-shadow: 0 4px 24px rgba(0,0,0,0.2);
  }
}
</style>