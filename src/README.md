# OficiosYa API Backend

Backend de la aplicación OficiosYa construido con ASP.NET Core Web API.

## 🚀 Instalación y Ejecución

### Prerrequisitos
- .NET 9 SDK
- SQL Server LocalDB (o SQL Server)
- Visual Studio 2022 o VS Code

### Pasos para ejecutar

1. **Restaurar dependencias:**
```bash
dotnet restore
```

2. **Configurar base de datos:**
   - Asegúrate de que SQL Server LocalDB esté instalado
   - La cadena de conexión está configurada para LocalDB por defecto

3. **Ejecutar la aplicación:**
```bash
dotnet run
```

4. **Abrir Swagger:**
```
https://localhost:7000/swagger
```

## 📁 Estructura del Proyecto

```
src/
├── Controllers/     # Controladores API
├── Models/         # Entidades y DTOs
├── Services/       # Servicios de negocio
├── Data/          # DbContext y configuración de BD
└── Program.cs     # Configuración de la aplicación
```

## 🔧 Endpoints Disponibles

### Usuarios
- `POST /api/usuarios/registrar` - Registrar nuevo usuario
- `POST /api/usuarios/login` - Login de usuario
- `GET /api/usuarios/{id}` - Obtener usuario por ID
- `GET /api/usuarios/verificar-email?email={email}` - Verificar disponibilidad de email

## 🗄️ Base de Datos

La aplicación usa Entity Framework Core con SQL Server. Las tablas se crean automáticamente al ejecutar la aplicación.

### Tablas principales:
- **Usuarios** - Usuarios del sistema
- **Profesionales** - Datos específicos de profesionales
- **Oficios** - Tipos de oficios disponibles
- **Resenas** - Sistema de calificaciones
- **Certificados** - Certificaciones de profesionales
- **Suscripciones** - Suscripciones de profesionales

## 🔗 Integración con Frontend

El backend está configurado para aceptar requests desde el frontend Vue.js en `http://localhost:3000`.

## 📋 Funcionalidades Implementadas

- ✅ **Registro de usuarios** con validaciones
- ✅ **Login de usuarios** con hash de contraseñas
- ✅ **Verificación de email** disponible
- ✅ **Base de datos** con Entity Framework
- ✅ **CORS** configurado para frontend
- ✅ **Swagger** para documentación de API
- ✅ **Logging** configurado

## 🚧 Próximas Funcionalidades

- [ ] JWT Authentication
- [ ] Búsqueda de profesionales
- [ ] Gestión de reseñas
- [ ] Sistema de suscripciones
- [ ] Upload de archivos
