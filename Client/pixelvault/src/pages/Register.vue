<script setup>
import { ref } from 'vue';
import { register } from '../api/auth';
import { useRouter } from 'vue-router';

const nombre = ref('');
const apellidos = ref('');
const email = ref('');
const password = ref('');
const mensaje = ref('');
const router = useRouter();

const handleRegister = async () => {
    try {
        await register(nombre.value, apellidos.value, email.value, password.value);
        console.log('Registro exitoso');
        mensaje.value = 'Registro exitoso. Ahora puedes iniciar sesión.';
        setTimeout(() => router.push('/login'), 1500);
    } catch (error) {
        console.error('Error al registrar:', error);
        mensaje.value = 'Error al registrar';
    }
};
</script>

<template>
  <div class="register-bg">
    <div class="register-container">
      <div class="register-image"></div>
      <div class="register-form">
        <h2>Crear Cuenta</h2>
        <form @submit.prevent="handleRegister">
          <input v-model="nombre" type="text" placeholder="Escriba su nombre" required />
          <input v-model="apellidos" type="text" placeholder="Escriba sus apellidos" required />
          <input v-model="email" type="email" placeholder="Escriba su email" required />
          <input v-model="password" type="password" placeholder="Escriba su contraseña" required />
          <button type="submit">Registrarse</button>
        </form>
        <p v-if="mensaje" class="mensaje">{{ mensaje }}</p>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.register-bg {
  min-height: 100vh;
  width: 100vw;
  background: url('../../public/images/fondoRegistroLogin.png') center/cover no-repeat;
  display: flex;
  align-items: center;
  justify-content: center;
}

.register-container {
  display: flex;
  background: transparent;
  border-radius: 24px;
  box-shadow: 0 8px 32px 0 rgba(31, 38, 135, 0.37);
  overflow: hidden;
  max-width: 900px;
  width: 90vw;
}

.register-image {
  width: 400px;
  min-height: 100%;
  background: url('../../public/images/mandoRegistro.png') center center/cover no-repeat, #181c2f;
  border-top-left-radius: 24px;
  border-bottom-left-radius: 24px;
}

.register-form {
  background: #fff;
  padding: 48px 40px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  width: 400px;
  border-top-right-radius: 24px;
  border-bottom-right-radius: 24px;

  h2 {
    font-size: 1.5rem;
    font-weight: bold;
    margin-bottom: 24px;
    text-align: left;
  }

  form {
    display: flex;
    flex-direction: column;
    gap: 18px;
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

  button {
    background: #fff;
    color: #222;
    padding: 12px;
    border: 1.5px solid #222;
    border-radius: 8px;
    font-size: 1rem;
    font-weight: 600;
    cursor: pointer;
    margin-top: 8px;
    transition: background 0.2s, color 0.2s;
    &:hover {
      background: #3b82f6;
      color: #fff;
      border-color: #3b82f6;
    }
  }

  .mensaje {
    color: #2563eb;
    margin-top: 18px;
    font-size: 1rem;
    text-align: center;
  }
}
</style>