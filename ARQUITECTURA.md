# 🏗️ Arquitectura del Proyecto OficiosYa

## 📋 Visión General

**OficiosYa** es una plataforma que conecta profesionales de diversos oficios con clientes que necesiten sus servicios. El proyecto está construido usando **Clean Architecture** para garantizar separación de responsabilidades, mantenibilidad y escalabilidad.

## 🎯 Principios de Clean Architecture

- **Independencia de Frameworks**: El negocio no depende de tecnologías específicas
- **Testeable**: La lógica de negocio se puede probar sin UI, BD o servicios externos
- **Independencia de UI**: Se puede cambiar fácilmente entre Web, Mobile, Console
- **Independencia de Base de Datos**: Se puede cambiar de SQL Server a Oracle, MongoDB, etc.
- **Independencia de Servicios Externos**: La lógica de negocio no conoce el mundo exterior

---

## 📁 Estructura del Proyecto

```
OficiosYa/
├── 📄 OficiosYa.sln                    # Archivo de solución principal
├── 📁 src/                             # Código fuente
│   ├── 📁 Core/                        # Núcleo del negocio (sin dependencias externas)
│   ├── 📁 Infrastructure/              # Implementaciones de servicios externos
│   ├── 📁 Presentation/                # Interfaces de usuario y APIs
│   └── 📁 Frontend/                    # Aplicación cliente Vue.js
├── 📁 tests/                           # Pruebas del proyecto
├── 📁 docs/                            # Documentación adicional
└── 📄 README.md                        # Documentación del proyecto
```

---

## 🔵 CORE LAYER (Núcleo del Negocio)

### 🎯 **OficiosYa.Domain** - Corazón del Sistema

**Propósito**: Contiene las reglas de negocio fundamentales, entidades y lógica de dominio pura.

**Características**:
- ❌ NO tiene dependencias externas
- ✅ Contiene la lógica de negocio más importante
- ✅ Define contratos (interfaces) para servicios externos
- ✅ Es el centro de toda la aplicación

#### 📂 Carpetas y Responsabilidades:

```
📁 OficiosYa.Domain/
├── 📁 Entities/                        # Entidades de negocio
│   ├── Usuario.cs                      # Usuario del sistema (cliente/profesional)
│   ├── Profesional.cs                  # Datos específicos del profesional
│   ├── Oficio.cs                       # Tipos de oficios disponibles
│   ├── Resena.cs                       # Reseñas y calificaciones
│   ├── Suscripcion.cs                  # Suscripciones de profesionales
│   ├── Certificado.cs                  # Certificados de profesionales
│   └── ProfesionalOficio.cs            # Relación muchos a muchos
├── 📁 ValueObjects/                    # Objetos de valor inmutables
│   ├── Email.cs                        # Email con validación
│   ├── Telefono.cs                     # Teléfono con formato
│   └── Ubicacion.cs                    # Coordenadas geográficas
├── 📁 Enums/                          # Enumeraciones del dominio
│   ├── TipoUsuario.cs                  # Cliente, Profesional, Admin
│   └── EstadoSuscripcion.cs            # Activa, Vencida, Cancelada
├── 📁 Interfaces/                     # Contratos para servicios
│   ├── IRepository.cs                  # Repositorio base genérico
│   ├── IUsuarioRepository.cs           # Operaciones específicas de usuarios
│   └── IProfesionalRepository.cs       # Búsquedas específicas de profesionales
└── 📁 Exceptions/                     # Excepciones de dominio
    ├── DomainException.cs              # Excepción base del dominio
    ├── EmailInvalidoException.cs       # Email con formato incorrecto
    └── SuscripcionVencidaException.cs  # Suscripción expirada
```

### 🎯 **OficiosYa.Application** - Casos de Uso

**Propósito**: Orquesta la lógica de negocio y coordina entre el dominio y la infraestructura.

**Características**:
- ✅ Solo depende de Domain
- ✅ Implementa casos de uso específicos
- ✅ Define DTOs para transferencia de datos
- ✅ Contiene validaciones de aplicación

#### 📂 Estructura Prevista:

```
📁 OficiosYa.Application/
├── 📁 UseCases/                       # Casos de uso del negocio
│   ├── 📁 Usuarios/
│   │   ├── RegistrarUsuario/          # Registro de nuevos usuarios
│   │   ├── AutenticarUsuario/         # Login y autenticación
│   │   └── ActualizarPerfil/          # Actualización de datos
│   ├── 📁 Profesionales/
│   │   ├── RegistrarProfesional/      # Registro como profesional
│   │   ├── BuscarProfesionales/       # Búsqueda con filtros
│   │   └── ActualizarDisponibilidad/  # Gestión de disponibilidad
│   └── 📁 Suscripciones/
│       ├── ProcesarPago/              # Procesamiento de pagos
│       └── RenovarSuscripcion/        # Renovación automática
├── 📁 DTOs/                           # Objetos de transferencia de datos
│   ├── 📁 Request/                    # DTOs de entrada
│   └── 📁 Response/                   # DTOs de salida
├── 📁 Interfaces/                     # Servicios de aplicación
│   ├── IEmailService.cs               # Envío de emails
│   ├── IPaymentService.cs             # Procesamiento de pagos
│   └── IStorageService.cs             # Almacenamiento de archivos
├── 📁 Behaviors/                      # Comportamientos transversales
│   ├── ValidationBehavior.cs          # Validación automática con FluentValidation
│   └── LoggingBehavior.cs             # Logging automático con MediatR
├── 📁 Mappings/                       # Mapeo entre objetos
│   └── AutoMapperProfile.cs           # Configuración de AutoMapper
└── 📁 Validators/                     # Validadores FluentValidation
    ├── RegistrarUsuarioValidator.cs
    └── BuscarProfesionalesValidator.cs
```

---

## 🔧 INFRASTRUCTURE LAYER (Servicios Externos)

### 🎯 **OficiosYa.Infrastructure** - Servicios Externos

**Propósito**: Implementa servicios que se comunican con el mundo exterior.

**Características**:
- ✅ Implementa interfaces definidas en Application
- ✅ Se comunica con APIs externas
- ✅ Maneja configuraciones de servicios
- ✅ Abstrae detalles técnicos

#### 📂 Estructura Prevista:

```
📁 OficiosYa.Infrastructure/
├── 📁 Payment/                        # Servicios de pago
│   ├── StripePaymentService.cs        # Implementación con Stripe
│   └── PayPalPaymentService.cs        # Implementación con PayPal
├── 📁 Storage/                        # Almacenamiento de archivos
│   ├── AzureBlobStorageService.cs     # Azure Blob Storage
│   └── LocalFileStorageService.cs     # Almacenamiento local (desarrollo)
├── 📁 Email/                          # Servicios de email
│   ├── SendGridEmailService.cs        # Implementación con SendGrid
│   └── SmtpEmailService.cs            # SMTP tradicional
├── 📁 Maps/                           # Servicios de mapas
│   ├── GoogleMapsService.cs           # Google Maps API
│   └── MapboxService.cs               # Mapbox como alternativa
├── 📁 Authentication/                 # Servicios de autenticación
│   ├── JwtTokenService.cs             # Generación y validación JWT
│   └── PasswordHashingService.cs      # Hash de contraseñas
└── 📁 Notifications/                  # Notificaciones
    ├── PushNotificationService.cs     # Notificaciones push
    └── SmsService.cs                  # Mensajes de texto
```

### 🎯 **OficiosYa.Persistence** - Acceso a Datos

**Propósito**: Maneja toda la comunicación con la base de datos.

**Características**:
- ✅ Implementa repositorios definidos en Domain
- ✅ Configura Entity Framework Core
- ✅ Maneja migraciones de base de datos
- ✅ Optimiza consultas y rendimiento

#### 📂 Estructura Prevista:

```
📁 OficiosYa.Persistence/
├── 📁 Context/                        # Contextos de Entity Framework
│   ├── OficiosYaDbContext.cs          # Contexto principal
│   └── OficiosYaReadOnlyContext.cs    # Contexto solo lectura (queries)
├── 📁 Configurations/                 # Configuraciones EF
│   ├── UsuarioConfiguration.cs        # Configuración entidad Usuario
│   ├── ProfesionalConfiguration.cs    # Configuración entidad Profesional
│   └── ResenaConfiguration.cs         # Configuración entidad Reseña
├── 📁 Repositories/                   # Implementaciones de repositorios
│   ├── Repository.cs                  # Repositorio base genérico
│   ├── UsuarioRepository.cs           # Operaciones específicas usuarios
│   └── ProfesionalRepository.cs       # Búsquedas específicas profesionales
├── 📁 Migrations/                     # Migraciones de base de datos
│   └── [Auto-generadas por EF]
└── 📁 Seed/                           # Datos iniciales
    ├── OficiosSeed.cs                 # Oficios predefinidos
    └── AdminUserSeed.cs               # Usuario administrador inicial
```

---

## 🌐 PRESENTATION LAYER (Interfaces de Usuario)

### 🎯 **OficiosYa.WebAPI** - API REST Principal

**Propósito**: Expone la funcionalidad del sistema a través de endpoints REST.

**Características**:
- ✅ Controllers REST
- ✅ Autenticación JWT
- ✅ Documentación Swagger
- ✅ Manejo de errores centralizado

#### 📂 Estructura Prevista:

```
📁 OficiosYa.WebAPI/
├── 📁 Controllers/                    # Controladores REST
│   ├── UsuariosController.cs          # Gestión de usuarios
│   ├── ProfesionalesController.cs     # Gestión de profesionales
│   ├── BusquedaController.cs          # Búsqueda de profesionales
│   ├── ResenasController.cs           # Sistema de reseñas
│   └── SuscripcionesController.cs     # Gestión de suscripciones
├── 📁 Middleware/                     # Middleware personalizado
│   ├── ErrorHandlingMiddleware.cs     # Manejo global de errores
│   ├── LoggingMiddleware.cs           # Logging de requests
│   └── RateLimitingMiddleware.cs      # Limitación de requests
├── 📁 Filters/                        # Filtros de acción
│   ├── ValidateModelFilter.cs         # Validación automática
│   └── AuthorizeRoleFilter.cs         # Autorización por roles
├── 📁 Extensions/                     # Extensiones de configuración
│   ├── ServiceCollectionExtensions.cs # Configuración DI
│   └── ApplicationBuilderExtensions.cs # Configuración pipeline
└── 📁 Configuration/                  # Configuraciones
    ├── SwaggerConfiguration.cs        # Configuración Swagger
    └── CorsConfiguration.cs           # Configuración CORS
```

### 🎯 **OficiosYa.AdminPanel** - Panel de Administración

**Propósito**: Interfaz web para administradores del sistema.

**Características**:
- ✅ MVC tradicional
- ✅ Dashboard con métricas
- ✅ Gestión de usuarios y contenido
- ✅ Reportes y estadísticas

#### 📂 Estructura Prevista:

```
📁 OficiosYa.AdminPanel/
├── 📁 Controllers/                    # Controladores MVC
│   ├── HomeController.cs              # Dashboard principal
│   ├── UsuariosController.cs          # Gestión de usuarios
│   ├── ProfesionalesController.cs     # Gestión de profesionales
│   └── ReportesController.cs          # Reportes y estadísticas
├── 📁 Views/                          # Vistas Razor
│   ├── 📁 Home/
│   ├── 📁 Usuarios/
│   ├── 📁 Profesionales/
│   └── 📁 Shared/                     # Layouts compartidos
├── 📁 Areas/                          # Áreas organizadas
│   └── 📁 Admin/                      # Área administrativa
└── 📁 wwwroot/                        # Archivos estáticos
    ├── 📁 css/
    ├── 📁 js/
    └── 📁 images/
```

---

## 🎨 FRONTEND LAYER

### 🎯 **oficios-ya-frontend** - Aplicación Vue.js 3

**Propósito**: Interfaz de usuario moderna y reactiva para clientes y profesionales.

**Características**:
- ✅ Vue.js 3 con Composition API
- ✅ TypeScript para mayor robustez
- ✅ Vuetify para componentes UI
- ✅ Pinia para gestión de estado

#### 📂 Estructura Prevista:

```
📁 oficios-ya-frontend/
├── 📁 src/
│   ├── 📁 components/                 # Componentes reutilizables
│   │   ├── 📁 ui/                     # Componentes UI básicos
│   │   ├── 📁 forms/                  # Formularios especializados
│   │   └── 📁 layout/                 # Componentes de layout
│   ├── 📁 views/                      # Páginas principales
│   │   ├── 📁 auth/                   # Login, registro
│   │   ├── 📁 search/                 # Búsqueda de profesionales
│   │   ├── 📁 profile/                # Perfiles de usuario
│   │   └── 📁 dashboard/              # Panel de profesionales
│   ├── 📁 stores/                     # Pinia stores
│   │   ├── auth.ts                    # Estado de autenticación
│   │   ├── search.ts                  # Estado de búsqueda
│   │   └── user.ts                    # Estado de usuario
│   ├── 📁 composables/                # Composition API functions
│   │   ├── useAuth.ts                 # Lógica de autenticación
│   │   ├── useGeolocation.ts          # Geolocalización
│   │   └── useApi.ts                  # Llamadas a API
│   ├── 📁 services/                   # Servicios de API
│   │   ├── api.ts                     # Cliente HTTP base
│   │   ├── authService.ts             # Servicios de autenticación
│   │   └── professionalService.ts     # Servicios de profesionales
│   ├── 📁 types/                      # Definiciones TypeScript
│   │   ├── user.ts                    # Tipos de usuario
│   │   ├── professional.ts            # Tipos de profesional
│   │   └── api.ts                     # Tipos de respuestas API
│   └── 📁 utils/                      # Utilidades
│       ├── constants.ts               # Constantes de la app
│       ├── validators.ts              # Validadores de formularios
│       └── formatters.ts              # Formateadores de datos
```

---

## 🧪 TESTING LAYER

### 🎯 **OficiosYa.UnitTests** - Pruebas Unitarias

**Propósito**: Pruebas aisladas de componentes individuales.

**Estructura**:
- ✅ Pruebas de entidades del dominio
- ✅ Pruebas de casos de uso
- ✅ Pruebas de validadores
- ✅ Mocks de dependencias

### 🎯 **OficiosYa.IntegrationTests** - Pruebas de Integración

**Propósito**: Pruebas de integración entre componentes.

**Estructura**:
- ✅ Pruebas de API endpoints
- ✅ Pruebas de base de datos
- ✅ Pruebas de servicios externos (mocked)

---

## 📊 Flujo de Dependencias

```
┌─────────────────┐
│   Frontend      │ (Vue.js)
└─────────────────┘
         │
         ▼
┌─────────────────┐
│  Presentation   │ (WebAPI, AdminPanel)
└─────────────────┘
         │
         ▼
┌─────────────────┐
│   Application   │ (Use Cases, DTOs)
└─────────────────┘
         │
         ▼
┌─────────────────┐    ┌─────────────────┐
│     Domain      │◄───│ Infrastructure  │
└─────────────────┘    └─────────────────┘
                            │
                            ▼
                   ┌─────────────────┐
                   │   Persistence   │
                   └─────────────────┘
```

## 🔑 Reglas de Dependencia

1. **Domain**: No depende de nada
2. **Application**: Solo depende de Domain
3. **Infrastructure**: Depende de Application y Domain
4. **Presentation**: Depende de Application y Domain
5. **Tests**: Pueden referenciar cualquier proyecto

---

## 🚀 Tecnologías Utilizadas

### Backend (.NET 9)
- **ASP.NET Core** - Framework web
- **Entity Framework Core** - ORM
- **MediatR** - Patrón CQRS y mediador
- **FluentValidation** - Validaciones
- **AutoMapper** - Mapeo de objetos
- **JWT** - Autenticación
- **xUnit** - Testing
- **Serilog** - Logging

### Frontend
- **Vue.js 3** - Framework frontend
- **TypeScript** - Tipado estático
- **Vuetify** - Componentes UI
- **Pinia** - Gestión de estado
- **Vite** - Build tool

### Servicios Externos
- **Stripe/PayPal** - Pagos
- **Azure Blob Storage** - Almacenamiento
- **SendGrid** - Emails
- **Google Maps** - Mapas

---

## 📝 Convenciones de Código

### Nombres
- **Entidades**: PascalCase (Usuario, Profesional)
- **Propiedades**: PascalCase (Nombre, Email)
- **Métodos**: PascalCase (ObtenerPorId, Validar)
- **Variables**: camelCase (usuario, resultado)
- **Constantes**: UPPER_CASE (MAX_INTENTOS)

### Archivos
- **Entidades**: Singular (Usuario.cs, Profesional.cs)
- **Repositorios**: IEntidadRepository.cs
- **Servicios**: IEntidadService.cs
- **DTOs**: EntidadRequestDto.cs, EntidadResponseDto.cs

---

## 📋 Próximos Pasos de Desarrollo

1. ✅ **Domain Layer** - Completado
2. 🔄 **Application Layer** - En progreso
3. 📋 **Persistence Layer** - Pendiente
4. 📋 **Infrastructure Layer** - Pendiente
5. 📋 **WebAPI** - Pendiente
6. 📋 **Frontend** - Pendiente
7. 📋 **Testing** - Pendiente

---

*Este documento debe actualizarse conforme evolucione el proyecto para mantener la documentación sincronizada con la implementación.*
