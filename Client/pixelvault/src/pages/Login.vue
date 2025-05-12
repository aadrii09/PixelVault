<script setup>

import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useUserStore } from '../store/auth';
import {login} from '../api/auth';




const email = ref('');
const password = ref('');
const error = ref('');
const routerr = useRouter();
const userStore = useUserStore();

const handleLogin = async () => {
    try {
        const response = await login(email.value, password.value);
        //descodificar token con jwt-decode

        const decoded = JSON.parse(atob(response.token.split('.')[1]));

        userStore.login({
            nombre: decoded.?email?.split('@')[0],
            email: decoded.email,
            esAdmin: decoded.esAdmin == 'True',
            id: parseInt(decoded.sub),
        },
        response.token
    )
    reouter.push('/')

    } catch (error) {
        console.error('Error al iniciar sesión:', error);
        error.value = 'Error al iniciar sesión';
    }
};

</script>



<template>
    <div class="max-w-md mx-auto mt-10 p-6 bg-white shadow rounded">
        <h2 class="text-xl font-bold mb-4">Inicar Sesión</h2>
        <!-- Formulario -->
         <form @submit.prevent="handleLogin" class="space-y-4">
            <input v-model="email" type="email" placeholder="Escriba su email" class="w-full px-4 py-2 border rounded" required>
            <input v-model="password" type="password" placeholder="Escriba su contraseña" class="w-full px-4 py-2 border rounded" required>
            <button type="submit" class="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-800">Entrar</button>
         </form>
         <p v-if="error" class="text-red-500 mt-4">{{ error }}</p>
    </div>
</template>


<style lang="scss" scoped>

</style>