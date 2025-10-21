# OficiosYa - Plataforma de Servicios Profesionales

Una plataforma full-stack para conectar clientes con profesionales de servicios.

## 🏗️ Arquitectura

- **Frontend**: Vue.js 3 + TypeScript + Vite
- **Backend**: ASP.NET Core 9 + Entity Framework Core
- **Base de Datos**: SQL Server

## 🎯 Categorías de Servicio

La plataforma separa los servicios en dos categorías principales:

### 🧰 Oficiantes
Servicios técnicos y manuales:
- Plomero, Electricista, Albañil
- Gasista, Jardinero, Pintor
- Cerrajero, Carpintero, Herrero
- Técnico en Aire Acondicionado/Refrigeración
- Limpieza

### 💼 Profesionales
Servicios profesionales especializados:
- Abogado, Contador, Médico
- Psicólogo, Dentista, Veterinario
- Arquitecto, Ingeniero
- Diseñador Gráfico, Traductor
- Coach, Nutricionista

## 🚀 Inicio Rápido

### Prerrequisitos

- Node.js 18+ 
- .NET 9 SDK
- SQL Server (LocalDB o Express)

### Instalación

1. **Clonar el repositorio**
   ```bash
   git clone <tu-repo>
   cd OficiosYa
   ```

2. **Instalar dependencias**
   ```bash
   npm run install:all
   ```

3. **Configurar base de datos**
   ```bash
   npm run migrate
   ```

4. **Ejecutar la aplicación**
   ```bash
   npm run dev
   ```

## 💻 Desarrollo con Cursor

### **IDE Recomendado: Cursor**
- ✅ **Soporte completo para .NET** (C#, ASP.NET Core)
- ✅ **Soporte excelente para Vue.js** (TypeScript, HTML, CSS)
- ✅ **IntelliSense avanzado** para ambos lenguajes
- ✅ **Debugging integrado** para .NET y Node.js
- ✅ **Terminal integrado** para comandos
- ✅ **Git integrado**

### **Extensiones Necesarias para Cursor:**

**Para .NET:**
- **C#** (Microsoft)
- **.NET Install Tool**
- **NuGet Package Manager**

**Para Vue.js:**
- **Vue Language Features (Volar)**
- **TypeScript Vue Plugin (Volar)**
- **Auto Rename Tag**
- **Prettier - Code formatter**

### **Flujo de Trabajo en Cursor:**

```bash
# Ejecutar todo (frontend + backend)
npm run dev

# Solo backend
npm run dev:backend

# Solo frontend  
npm run dev:frontend

# Debugging
F5 → Debug completo
Ctrl+F5 → Ejecutar sin debug
```

### **URLs de Desarrollo:**
- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000/api
- **Swagger**: http://localhost:5000/swagger

## 📝 Scripts Disponibles

### Desarrollo
- `npm run dev` - Ejecuta frontend y backend simultáneamente
- `npm run dev:frontend` - Solo frontend (puerto 3000)
- `npm run dev:backend` - Solo backend (puerto 5000)

### Construcción
- `npm run build` - Construye frontend y backend
- `npm run build:frontend` - Solo frontend
- `npm run build:backend` - Solo backend

### Base de Datos
- `npm run migrate` - Aplica migraciones
- `npm run migrate:add <nombre>` - Crea nueva migración

### Testing
- `npm run test` - Ejecuta tests del backend

### Limpieza
- `npm run clean` - Limpia archivos generados

## 🌐 URLs de Desarrollo

- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000/api
- **Swagger**: http://localhost:5000/swagger

## 📁 Estructura del Proyecto

```
OficiosYa/
├── src/
│   ├── frontend-oficiosya/     # Frontend Vue.js
│   │   ├── src/
│   │   │   ├── views/          # Páginas Vue
│   │   │   ├── components/     # Componentes reutilizables
│   │   │   ├── services/       # Servicios API
│   │   │   ├── utils/          # Utilidades
│   │   │   └── composables/    # Composables Vue
│   │   └── package.json
│   └── OficiosYa.Web/          # Backend ASP.NET Core
│       ├── Controllers/         # Controladores API
│       ├── Models/             # Modelos y DTOs
│       ├── Services/           # Lógica de negocio
│       ├── Repositories/       # Acceso a datos
│       └── Data/               # Contexto de EF
├── tests/                      # Tests unitarios e integración
└── docs/                       # Documentación
```

## 🛠️ Desarrollo

### Frontend (Vue.js)
- Usa TypeScript para tipado fuerte
- Composables para lógica reutilizable
- Validadores centralizados
- Constantes en archivos separados

### Backend (ASP.NET Core)
- Arquitectura en capas (Controllers → Services → Repositories)
- Entity Framework Core para ORM
- DTOs para transferencia de datos
- Inyección de dependencias

## 🧪 Testing

```bash
# Ejecutar todos los tests
npm run test

# Tests específicos
cd src/OficiosYa.Web
dotnet test --filter "ClassName"
```

## 🔧 Solución de Problemas

### **Errores Comunes:**

#### **1. Error de Compilación en Backend:**
```bash
# Limpiar y reconstruir
cd src/OficiosYa.Web
dotnet clean
dotnet build
```

#### **2. Dependencias Faltantes:**
```bash
# Restaurar paquetes .NET
cd src/OficiosYa.Web
dotnet restore

# Instalar dependencias Node.js
cd src/frontend-oficiosya
npm install
```

#### **3. Base de Datos:**
```bash
# Aplicar migraciones
cd src/OficiosYa.Web
dotnet ef database update

# Crear nueva migración
dotnet ef migrations add NombreMigracion
```

#### **4. Puerto en Uso:**
```bash
# Cambiar puerto del frontend (vite.config.ts)
server: { port: 3001 }

# Cambiar puerto del backend (launchSettings.json)
"applicationUrl": "http://localhost:5001"
```

### **Estado Actual del Proyecto:**
- ✅ **Frontend**: Funcionando en http://localhost:3000
- ❌ **Backend**: Errores de compilación (revisar ProfesionalService.cs)
- ✅ **Estructura**: Completa y organizada

## 📦 Despliegue

### Frontend
```bash
npm run build:frontend
# Los archivos se generan en src/frontend-oficiosya/dist
```

### Backend
```bash
npm run build:backend
# La aplicación se compila en src/OficiosYa.Web/bin
```

## 🤝 Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para detalles.
