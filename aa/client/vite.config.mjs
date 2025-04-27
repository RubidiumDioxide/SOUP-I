import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import mkcert from 'vite-plugin-mkcert'
import { dirname, resolve } from 'node:path'
import { fileURLToPath } from 'node:url'

const __dirname = dirname(fileURLToPath(import.meta.url))

// https://vitejs.dev/config/
export default defineConfig({
    base: '/app',  
    plugins: [react(), mkcert()],
    server: {
        https: true,
        port: 6363,
        strictPort: true,
        proxy: {
            '/api': {
                target: 'https://localhost:5001',
                changeOrigin: true, 
                secure: false
                //rewrite: (path) => path.replace(/^\/api/, '/api')
            }
        }
    },
})
