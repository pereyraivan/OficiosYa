<template>
  <div class="app-root">
    <AppNav />
    <main class="app-main">
      <router-view />
    </main>
    <WhatsAppSoporte />
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useAuthStore } from './stores/auth'
import AppNav from './components/AppNav.vue'
import WhatsAppSoporte from './components/WhatsAppSoporte.vue'

const authStore = useAuthStore()

onMounted(async () => {
  if (authStore.token && !authStore.usuario) {
    await authStore.cargarPerfil()
  }
})
</script>

<style>
.app-root {
  font-family: Inter, system-ui, -apple-system, Segoe UI, Roboto, Ubuntu, Cantarell, Noto Sans, Helvetica Neue, Arial, "Apple Color Emoji", "Segoe UI Emoji";
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}
.app-main {
  min-height: calc(100vh - 60px);
  position: relative;
  z-index: 0;
}
</style>
