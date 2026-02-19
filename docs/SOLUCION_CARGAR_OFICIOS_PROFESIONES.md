# Solución: No se cargan los oficios y profesiones en el registro

## Problema
Los oficios y profesiones no se están cargando en el formulario de registro de profesionales.

## Causa Raíz
El error ocurre porque la tabla `TipoServicios` no existe en la base de datos. Esta tabla es necesaria para que el endpoint `/api/profesionales/categorias` funcione correctamente.

## Cambios Realizados

### 1. Mejoras en el Frontend (`RegistrarProfesional.vue`)
- ✅ Las categorías ahora solo se cargan cuando el usuario selecciona un tipo de servicio (Oficiante o Profesional)
- ✅ Mejor manejo de errores con mensajes más claros
- ✅ Botón "Reintentar" cuando hay un error al cargar categorías
- ✅ Limpieza automática de errores al cambiar el tipo de servicio

### 2. Mejoras en el Servicio (`profesionalService.ts`)
- ✅ Manejo mejorado de errores del backend
- ✅ Mensajes de error más descriptivos para el usuario
- ✅ Detección específica de errores relacionados con TipoServicios

### 3. Mejoras en el Backend (`ProfesionalesController.cs`)
- ✅ Manejo específico del error cuando la tabla TipoServicios no existe
- ✅ Mensajes de error más informativos con instrucciones de solución

## Solución Completa

Para resolver completamente el problema, necesitas crear la tabla `TipoServicios` en la base de datos. Tienes dos opciones:

### Opción 1: Aplicar Migraciones (Recomendado)

```powershell
cd "c:\Users\perey\Desktop\Proyectos-Clientes\OficiosYa"
dotnet ef database update --project "src\Infrastructure\OficiosYa.Persistence" --startup-project "src\OficiosYa.Web"
```

Esto aplicará todas las migraciones pendientes, incluyendo la creación de la tabla `TipoServicios` y la inserción de datos iniciales.

### Opción 2: Ejecutar Script SQL Manual

1. Abre SQL Server Management Studio (SSMS) o Azure Data Studio
2. Conéctate a tu instancia: `(localdb)\mssqllocaldb`
3. Selecciona la base de datos: `OficiosYa_Database`
4. Abre y ejecuta el archivo: `scripts/VerificarYCrearTipoServicios.sql`

Este script:
- Verifica si la tabla existe
- La crea si no existe
- Crea el índice único
- Inserta los datos iniciales (Oficiante y Profesional)

## Verificación

Después de aplicar la solución, verifica que todo funcione:

1. **Verificar la tabla en la base de datos:**
   ```sql
   USE [OficiosYa_Database];
   SELECT * FROM [dbo].[TipoServicios];
   ```
   Deberías ver dos registros: "Oficiante" y "Profesional"

2. **Probar el endpoint:**
   ```
   GET /api/profesionales/categorias
   GET /api/profesionales/categorias?tipo=Oficiante
   GET /api/profesionales/categorias?tipo=Profesional
   ```

3. **Probar en el frontend:**
   - Ve a la página de registro de profesional
   - Selecciona "Soy Oficiante" o "Soy Profesional"
   - Deberías ver las categorías disponibles cargándose

## Comportamiento Esperado

1. El usuario accede al formulario de registro de profesional
2. Selecciona un tipo de servicio (Oficiante o Profesional)
3. El sistema carga automáticamente las categorías correspondientes
4. El usuario puede seleccionar múltiples oficios/profesiones
5. Si hay un error, se muestra un mensaje claro con opción de reintentar

## Notas Importantes

- ⚠️ **Asegúrate de detener la aplicación** antes de aplicar migraciones o ejecutar scripts SQL
- ✅ El script SQL es idempotente: puedes ejecutarlo múltiples veces sin problemas
- ✅ Las categorías se cargan bajo demanda (solo cuando se selecciona un tipo)
- ✅ Si la tabla no existe, el sistema mostrará un mensaje de error claro

## Troubleshooting

### Si las categorías aún no se cargan después de crear la tabla:

1. **Verifica la conexión a la base de datos:**
   - Revisa `appsettings.json` para confirmar la cadena de conexión
   - Asegúrate de que la base de datos esté accesible

2. **Verifica que los datos estén en la tabla:**
   ```sql
   SELECT COUNT(*) FROM [dbo].[CategoriaServicios];
   SELECT COUNT(*) FROM [dbo].[TipoServicios];
   ```

3. **Revisa la consola del navegador:**
   - Abre las herramientas de desarrollador (F12)
   - Ve a la pestaña "Console" para ver errores de JavaScript
   - Ve a la pestaña "Network" para ver las peticiones HTTP

4. **Revisa los logs del backend:**
   - Verifica que no haya errores en la consola de la aplicación
   - Revisa los logs de Entity Framework si están habilitados
