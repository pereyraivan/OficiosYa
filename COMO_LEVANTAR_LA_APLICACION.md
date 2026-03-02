# Cómo levantar la aplicación OficiosYa

La aplicación tiene **dos partes**: backend (API .NET) y frontend (Vue + Vite). Hay que levantar ambas.

---

## Requisitos previos

- **.NET 10 SDK** – [Descargar](https://dotnet.microsoft.com/download)
- **Node.js** (v18 o superior) – [Descargar](https://nodejs.org/)
- **SQL Server** (Express o LocalDB) con la base de datos configurada

---

## 1. Backend (API)

En una terminal, desde la raíz del proyecto:

```powershell
cd src/OficiosYa.Web
dotnet restore
dotnet run
```

- La API quedará en **http://localhost:5230**
- **Swagger** se abre automáticamente en **http://localhost:5230/swagger** (tanto al ejecutar desde Cursor como desde Visual Studio, usando el perfil con `ASPNETCORE_ENVIRONMENT=Development`).

Comprueba que en `appsettings.json` la cadena de conexión apunte a tu SQL Server y que la base de datos exista.

---

## 2. Frontend (Vue)

En **otra terminal**, desde la raíz del proyecto:

```powershell
cd src/frontend-oficiosya
npm install
npm run dev
```

- La app web quedará en **http://localhost:5173**
- Las peticiones a `/api` se redirigen al backend (puerto 5230) por el proxy de Vite.

---

## Resumen rápido

| Paso | Comando | Dónde |
|------|---------|--------|
| 1 | `cd src/OficiosYa.Web` y `dotnet run` | Terminal 1 |
| 2 | `cd src/frontend-oficiosya` y `npm run dev` | Terminal 2 |
| 3 | Abrir **http://localhost:5173** en el navegador | Navegador |

---

## Si algo falla

- **Backend**: revisa que SQL Server esté corriendo y que la cadena en `appsettings.json` sea correcta (incluye `TrustServerCertificate=true` si usas SSL local).
- **Frontend**: si da error de dependencias, borra `node_modules` y ejecuta de nuevo `npm install`.
- **CORS**: el backend ya está configurado para aceptar peticiones desde el frontend en desarrollo.
