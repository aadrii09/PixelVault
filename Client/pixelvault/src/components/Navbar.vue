<script setup>
import { useUserStore } from '../store/user.js'
import {computed} from 'vue'
import { useRoute } from 'vue-router'



    const userStore = useUserStore()
    const route = useRoute()

    const isLoggedIn = computed(() => !!userStore.token)

    const logout = async () => {
        userStore.logout()
        router.push('/login')
    }


</script>


<template>
    <nav class="bg-white shadow-md">
        <div class="max-w-7xl mx-auto px-4 py-3 flex justify-between items-center">
           <router-link to="/" class="text-xl font-bold text-blue-600">PixelVault</router-link>
        </div>
        <div class="space-x-4">
            <router-link to="/" class="text-gray-700 hover:text-blue-600">Home</router-link>
            <template v-if="!isLoggedIn">
            <router-link to="/" class="text-gray-700 hover:text-blue-600">Login</router-link>
            <router-link to="/" class="text-gray-700 hover:text-blue-600">Registro</router-link>
            </template>
            <template v-else>
                <span class="text-gray-700">Hola, {{ userStore?.nombre }}</span>
                <button @click="logout" class="text-red-500 hover:underline">Cerrar Sesion</button>
                </template>
        </div>
        
    </nav>
</template>

<style lang="scss" scoped>

</style>