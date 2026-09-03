import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import path from 'path'

export default defineConfig({
  plugins: [vue()],
  // SPA en la raíz del sitio (sin /dist/)
  base: '/',
  build: {
    outDir: '../OficiosYa.Web/wwwroot',
    // No vaciar wwwroot: preserva uploads/, lib/, css/, etc.
    emptyOutDir: false,
    // Assets compilados en /assets/ (Vite limpia solo esa carpeta de build si hace falta)
    assetsDir: 'assets'
  },
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src')
    }
  },
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5230',
        changeOrigin: true
      }
    }
  }
})
