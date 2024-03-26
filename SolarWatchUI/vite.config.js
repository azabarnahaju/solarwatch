import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    watch: {
      usePolling: true,
    },
    "/api": {
      target: "http://localhost:5292",
      changeOrigin: true,
      rewrite: (path) => path.replace(/^\/api/, ""),
    }
  },
});
