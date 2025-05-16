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
        // Usar la función register en lugar de axios.post directamente
        await register(nombre.value, apellidos.value, email.value, password.value);
        
        console.log('Registro exitoso');
        mensaje.value = 'Registro exitoso. Ahora puedes iniciar sesión.';
        
        // Redirección después de 1.5 segundos
        setTimeout(() => router.push('/login'), 1500);
    } catch (error) {
        console.error('Error al registrar:', error);
        mensaje.value = 'Error al registrar';
    }
};
</script>

<template>
    <div class="max-w-md max-auto mt-10 bg-white  p-6 shadow rounded">
        <h2 class="text-xl font-bold mb-4">Crear Cuenta</h2>
        <!-- formulario -->
         <form @submit.prevent="handleRegister" class="space-y-4">

            <input v-model="nombre" type="text" placeholder="Escriba su nombre" class="w-full px-4 py-2 border rounded" required />
            <input v-model="apellidos" type="text" placeholder="Escriba sus apellidos" class="w-full px-4 py-2 border rounded" required />
            <input v-model="email" type="email" placeholder="Escriba su email" class="w-full px-4 py-2 border rounded" required />
            <input v-model="password" type="password" placeholder="Escriba su contraseña" class="w-full px-4 py-2 border rounded" required>
            <button type="submit" class="w-full bg-green-600 text-white py-2 rounded hover:bg-green-800">Registrarse</button>

         </form>
         <p v-if="mensaje" class="text-center mt-4 text-blue-600">{{ mensaje }}</p>
    </div>
</template>

<style lang="scss">
body {
    background-image: url('../../public/images/fondoRegistroLogin.png');
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    min-height: 100vh;
    margin: 0;
    width: 100vw;
    height: 100vh;
}
</style>