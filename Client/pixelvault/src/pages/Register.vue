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
          <div class="not-registered pb-5">
          <a @click.prevent="router.push('/login')" href="#" style="cursor:pointer;">¿Ya tienes cuenta? Inicia Sesión</a>
        </div>
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
  background: url('/images/fondoRegistroLogin.png') center/cover no-repeat fixed;
  display: flex;
  align-items: center;
  justify-content: center;
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
}

.register-container {
  display: flex;
  background: transparent;
  border-radius: 16px;
  box-shadow: 0 8px 32px 0 rgba(31, 38, 135, 0.37);
  overflow: hidden;
  max-width: 700px;
  width: 80vw;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

.register-image {
  width: 300px;
  min-height: 100%;
  background: url('/images/mandoRegistro.png') center center/cover no-repeat, #181c2f;
  border-top-left-radius: 16px;
  border-bottom-left-radius: 16px;
}

.register-form {
  background: #fff;
  padding: 32px 28px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  width: 300px;
  border-top-right-radius: 16px;
  border-bottom-right-radius: 16px;

  h2 {
    font-size: 1.3rem;
    font-weight: bold;
    margin-bottom: 18px;
    text-align: center;
  }

  form {
    display: flex;
    flex-direction: column;
    gap: 14px;
  }

  input {
    padding: 10px;
    border: 1px solid #e0e0e0;
    border-radius: 6px;
    font-size: 0.9rem;
    outline: none;
    transition: border 0.2s;
    &:focus {
      border: 1.5px solid #3b82f6;
    }
  }

  button {
    background: #fff;
    color: #222;
    padding: 10px;
    border: 1.5px solid #222;
    border-radius: 6px;
    font-size: 0.9rem;
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
    margin-top: 14px;
    font-size: 0.9rem;
    text-align: center;
  }
}

.not-registered {
  margin-top: 0.3rem;
  font-size: 0.8rem;
  color: #888;
  text-align: center;
}

@media (max-width: 640px) {
  .register-container {
    flex-direction: column;
    width: 90vw;
    max-width: 300px;
  }
  
  .register-image {
    width: 100%;
    height: 150px;
    border-top-left-radius: 16px;
    border-top-right-radius: 16px;
    border-bottom-left-radius: 0;
  }
  
  .register-form {
    width: 100%;
    border-top-right-radius: 0;
    border-bottom-left-radius: 16px;
    border-bottom-right-radius: 16px;
  }
}
</style>