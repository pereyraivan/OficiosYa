# OficiosYa Frontend

Frontend de la aplicación OficiosYa construido con Vue.js 3, TypeScript y Vite.

## 🚀 Instalación y Ejecución

### Prerrequisitos
- Node.js 18+ 
- npm o yarn

### Pasos para ejecutar

1. **Instalar dependencias:**
```bash
npm install
```

2. **Configurar variables de entorno:**
Crea un archivo `.env` en la raíz del proyecto:
```
VITE_API_URL=http://localhost:5000/api
```

3. **Ejecutar en modo desarrollo:**
```bash
npm run dev
```

4. **Abrir en el navegador:**
```
http://localhost:3000
```

## 📁 Estructura del Proyecto

```
src/
├── components/     # Componentes reutilizables
├── views/         # Páginas principales
├── services/      # Servicios para API
├── stores/        # Pinia stores
├── types/         # Definiciones TypeScript
├── router/        # Configuración de rutas
└── assets/        # CSS y recursos estáticos
```

## 🎨 Características

- ✅ **Vue.js 3** con Composition API
- ✅ **TypeScript** para tipado estático
- ✅ **Pinia** para gestión de estado
- ✅ **Vue Router** para navegación
- ✅ **Axios** para llamadas HTTP
- ✅ **Diseño responsivo** basado en el HTML original
- ✅ **Formularios de registro y login**

## 🔧 Scripts Disponibles

- `npm run dev` - Ejecutar en modo desarrollo
- `npm run build` - Construir para producción
- `npm run preview` - Previsualizar build de producción

## 📋 Funcionalidades Implementadas

- **Página principal** con diseño del HTML original
- **Registro de usuarios** (clientes y profesionales)
- **Login de usuarios**
- **Búsqueda de profesionales** (interfaz)
- **Diseño responsivo** para móviles y desktop
- **Integración con API** del backend

## 🔗 Integración con Backend

El frontend está configurado para conectarse con la API del backend en `http://localhost:5000/api`.

Asegúrate de que el backend esté ejecutándose antes de usar el frontend.
