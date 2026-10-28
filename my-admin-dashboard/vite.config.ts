import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import tailwindcss from '@tailwindcss/vite';
import path from 'path';
import fs from 'fs';

export default defineConfig(({ command }) => ({
  plugins: [
    react(),
    tailwindcss(),
  ],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, 'src'),
    },
  },
  server: {
    port: 3000,
    https: command === 'serve' && process.env.VITE_E2E !== '1' && fs.existsSync(path.resolve(__dirname, './certs/localhost-key.pem')) ? {
      key: fs.readFileSync(path.resolve(__dirname, './certs/localhost-key.pem')),
      cert: fs.readFileSync(path.resolve(__dirname, './certs/localhost-cert.pem')),
    } : undefined,
    host: '0.0.0.0'
  },
}));
