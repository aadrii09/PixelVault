<script setup>
import { defineProps, defineEmits } from 'vue';

const props = defineProps({
  message: {
    type: String,
    required: true
  },
  type: {
    type: String,
    default: 'success',
    validator: (value) => ['success', 'error', 'info', 'warning'].includes(value)
  },
  duration: {
    type: Number,
    default: 3000
  },
  show: {
    type: Boolean,
    default: true
  }
});

const emit = defineEmits(['close']);

// Cerrar la notificación después del tiempo especificado
if (props.duration > 0) {
  setTimeout(() => {
    emit('close');
  }, props.duration);
}
</script>

<template>
  <transition name="notification-fade">
    <div v-if="show" 
      class="fixed top-5 right-5 z-50 p-4 rounded-lg shadow-lg backdrop-blur-md flex items-center max-w-md transition-all duration-300 transform"
      :class="{
        'bg-gradient-to-r from-green-500/80 to-emerald-600/80 border border-green-400/50 text-white': type === 'success',
        'bg-gradient-to-r from-red-500/80 to-pink-600/80 border border-red-400/50 text-white': type === 'error',
        'bg-gradient-to-r from-blue-500/80 to-indigo-600/80 border border-blue-400/50 text-white': type === 'info',
        'bg-gradient-to-r from-yellow-500/80 to-amber-600/80 border border-yellow-400/50 text-white': type === 'warning'
      }">
      
      <!-- Icono según el tipo de notificación -->
      <div class="mr-3">
        <svg v-if="type === 'success'" class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7"></path>
        </svg>
        <svg v-if="type === 'error'" class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
        </svg>
        <svg v-if="type === 'info'" class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"></path>
        </svg>
        <svg v-if="type === 'warning'" class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"></path>
        </svg>
      </div>
      
      <!-- Mensaje -->
      <div class="flex-1 font-medium text-lg">{{ message }}</div>
      
      <!-- Botón para cerrar -->
      <button @click="emit('close')" class="ml-3 text-white hover:text-gray-100">
        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"></path>
        </svg>
      </button>
    </div>
  </transition>
</template>

<style scoped>
.notification-fade-enter-active,
.notification-fade-leave-active {
  transition: all 0.5s ease;
}
.notification-fade-enter-from {
  opacity: 0;
  transform: translateX(30px);
}
.notification-fade-leave-to {
  opacity: 0;
  transform: translateX(30px);
}
</style>
