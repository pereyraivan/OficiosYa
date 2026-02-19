# Solución al Error: Invalid object name 'TipoServicios'

## Problema
La aplicación muestra el error `Invalid object name 'TipoServicios'` cuando se intenta acceder al endpoint `/api/profesionales/categorias`.

El error ocurre porque la tabla `TipoServicios` no existe en la base de datos, aunque está definida en las migraciones de Entity Framework Core.

## Soluciones

### Opción 1: Aplicar Migraciones (Recomendado)

Si las migraciones no se han aplicado, ejecuta:

```powershell
cd "c:\Users\perey\Desktop\Proyectos-Clientes\OficiosYa"
dotnet ef database update --project "src\Infrastructure\OficiosYa.Persistence" --startup-project "src\OficiosYa.Web"
```

Esto aplicará todas las migraciones pendientes, incluyendo la creación de la tabla `TipoServicios`.

### Opción 2: Ejecutar Script SQL Manual

Si prefieres crear la tabla manualmente, ejecuta el script SQL:

1. Abre SQL Server Management Studio (SSMS) o Azure Data Studio
2. Conéctate a tu instancia de SQL Server LocalDB: `(localdb)\mssqllocaldb`
3. Selecciona la base de datos: `OficiosYa_Database`
4. Abre y ejecuta el archivo: `scripts/VerificarYCrearTipoServicios.sql`

Este script:
- Verifica si la tabla existe
- La crea si no existe
- Crea el índice único en el campo `Nombre`
- Inserta los datos iniciales (Oficiante y Profesional)
- Verifica la estructura y los datos

### Opción 3: Verificar Migraciones Pendientes

Para ver qué migraciones están pendientes:

```powershell
cd "c:\Users\perey\Desktop\Proyectos-Clientes\OficiosYa"
dotnet ef migrations list --project "src\Infrastructure\OficiosYa.Persistence" --startup-project "src\OficiosYa.Web"
```

Las migraciones marcadas con `(Pending)` no se han aplicado.

## Verificación

Después de aplicar la solución, verifica que la tabla existe:

```sql
USE [OficiosYa_Database];
SELECT * FROM [dbo].[TipoServicios];
```

Deberías ver dos registros:
- Id = 1, Nombre = "Oficiante"
- Id = 2, Nombre = "Profesional"

## Prueba el Endpoint

Una vez resuelto, prueba el endpoint:

```
GET /api/profesionales/categorias
```

Debería devolver la lista de categorías con sus tipos de servicio asociados.

## Notas

- Asegúrate de que la aplicación no esté ejecutándose cuando apliques las migraciones
- Si la base de datos no existe, Entity Framework la creará automáticamente al aplicar las migraciones
- El script SQL es idempotente: puedes ejecutarlo múltiples veces sin problemas
