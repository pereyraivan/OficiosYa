# OficiosYa — Nota de funcionamiento y configuraciones

Este documento resume cómo está organizado el proyecto, cómo compilarlo/ejecutarlo, y soluciones a problemas comunes (por ejemplo, errores de Razor con `@media`). Mantén este archivo como referencia para la configuración del equipo.

## Checklist rápido
- [x] Estructura de proyectos: Domain, Application, Persistence, Presentation (WebAPI + AdminPanel).
- [x] Base de datos: EF Core con migraciones y SQL Server.
- [x] Home page del AdminPanel: CSS movido a `wwwroot/css/home.css` para evitar errores de Razor.

## Estructura clave y propósito
- `OficiosYa.sln` — solución raíz.
- `src/Core/OficiosYa.Domain` — entidades y value objects.
- `src/Core/OficiosYa.Application` — casos de uso (MediatR), DTOs, lógica de aplicación.
- `src/Infrastructure/OficiosYa.Persistence` — EF Core DbContext, configuraciones, repositorios.
- `src/Presentation/OficiosYa.WebAPI` — API pública (MediatR, controllers, Swagger).
- `src/Presentation/OficiosYa.AdminPanel` — interfaz administrativa en MVC/Razor.
  - `Views/Home/Index.cshtml` — vista principal del AdminPanel.
  - `wwwroot/css/home.css` — CSS para la página principal (movido desde inline style).

## Cómo compilar y ejecutar localmente
Desde la raíz del repo (Windows PowerShell):

```powershell
# Restaurar y compilar toda la solución
dotnet build .\OficiosYa.sln

# Ejecutar WebAPI (ejemplo)
dotnet run --project src\Presentation\OficiosYa.WebAPI

# Ejecutar AdminPanel (ejemplo)
dotnet run --project src\Presentation\OficiosYa.AdminPanel
```

En Visual Studio 2022, establece el proyecto de inicio desde el Solution Explorer (clic derecho sobre el proyecto -> "Set as Startup Project"). Puedes configurar múltiples proyectos de inicio en las propiedades de la solución.

## Cadena de conexión y migraciones
- La cadena de conexión está en `appsettings.json` del proyecto WebAPI (y/o AdminPanel si aplica). Edita la sección `ConnectionStrings` para apuntar a tu servidor SQL.

Comandos EF Core (desde la carpeta del proyecto que contiene el DbContext o usando `--project`):

```powershell
# Crear una migración (ejemplo desde la raíz)
dotnet ef migrations add InitialCreate --project src\Presentation\OficiosYa.WebAPI --startup-project src\Presentation\OficiosYa.WebAPI

# Aplicar migraciones a la base de datos
dotnet ef database update --project src\Presentation\OficiosYa.WebAPI --startup-project src\Presentation\OficiosYa.WebAPI
```

Si `dotnet ef` falla por no encontrar herramientas, asegúrate de tener instalado `Microsoft.EntityFrameworkCore.Design` en el proyecto correcto.

## Solución al error Razor con `@media`
Problema: Razor interpreta `@media` dentro de un `<style>` en una vista `.cshtml` como inicio de código C#, provocando errores como "El nombre 'media' no existe en el contexto actual".

Opciones para arreglarlo (elige una):

1) Mover el CSS a un archivo estático en `wwwroot` (recomendado)
   - Archivo creado: `src/Presentation/OficiosYa.AdminPanel/wwwroot/css/home.css`
   - En la vista `Views/Home/Index.cshtml` se añadió:
     ```html
     <link rel="stylesheet" href="~/css/home.css" />
     ```
   - Esta solución evita que Razor procese `@` dentro del CSS.

2) Escapar `@` en la vista (menos recomendado para CSS grande):
   - Reemplazar `@media` por `@@media` dentro del archivo `.cshtml` (Razor renderiza `@@` como `@`).

3) Encapsular el bloque CSS en un literal para Razor (ej. `<text>...</text>`), pero normalmente es más limpio usar archivos estáticos.

## Archivos relevantes para este fix
- Vista afectada (original):
  - `src/Presentation/OficiosYa.AdminPanel/Views/Home/Index.cshtml`
- CSS estático nuevo:
  - `src/Presentation/OficiosYa.AdminPanel/wwwroot/css/home.css`

## Puesta en marcha rápida (comprobaciones)
- Compilar: `dotnet build .\OficiosYa.sln` — debe compilar sin errores.
- Ejecutar WebAPI y abrir `http://localhost:5044/swagger` (u otro puerto que muestre la consola al ejecutar).
- Ejecutar AdminPanel y abrir `http://localhost:5XXX/` (puerto mostrado en consola) para verificar la home.

## Notas de desarrollo y siguientes pasos sugeridos
- Implementar búsqueda espacial real (p. ej. NetTopologySuite + SQL Server spatial) si necesitas consultas por radio.
- Agregar tests de integración para endpoints principales (búsqueda de profesionales, detalles).
- Añadir pipeline CI que ejecute build y migraciones en entorno de prueba.

---

Si querés, agrego un README más completo con pasos para desarrollo en Windows/Visual Studio, o puedo añadir scripts/tasks para VS Code/PowerShell que automaticen build, migraciones y arranque del entorno.
