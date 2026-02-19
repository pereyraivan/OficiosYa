# Solución al Error: Invalid object name 'TipoServicios'

## Problema
La aplicación muestra el error `Invalid object name 'TipoServicios'` aunque la tabla existe en la base de datos.

## Verificación
✅ La tabla `TipoServicios` existe en el esquema `dbo` de la base de datos `OficiosYa_Database`
✅ La tabla tiene datos (Oficiante y Profesional)
✅ La configuración en `ApplicationDbContext.cs` especifica `.ToTable("TipoServicios", "dbo")`

## Soluciones Aplicadas

1. **Configuración explícita del esquema en ApplicationDbContext.cs**
   ```csharp
   entity.ToTable("TipoServicios", "dbo");
   ```

2. **Actualización del Model Snapshot**
   - Actualizado `ApplicationDbContextModelSnapshot.cs` para incluir el esquema

## Pasos para Resolver

### 1. Limpiar y Reconstruir el Proyecto
```powershell
# En Visual Studio o desde la terminal
dotnet clean
dotnet build
```

### 2. Verificar la Cadena de Conexión
Asegúrate de que la aplicación se conecta a la base de datos correcta:
- Base de datos: `OficiosYa_Database`
- Servidor: `(localdb)\mssqllocaldb`
- Esquema: `dbo`

### 3. Reiniciar la Aplicación
- Cierra completamente la aplicación
- Reinicia el servidor de desarrollo
- Limpia el caché del navegador si es necesario

### 4. Verificar que la Tabla Existe
Ejecuta en SQL Server Management Studio:
```sql
USE [OficiosYa_Database];
SELECT * FROM [dbo].[TipoServicios];
```

### 5. Si el Problema Persiste
- Verifica que no haya múltiples instancias de la base de datos
- Verifica que la cadena de conexión en `appsettings.json` sea correcta
- Intenta eliminar la carpeta `bin` y `obj` y reconstruir

## Verificación Final
Después de aplicar estos pasos, prueba el endpoint:
```
GET /api/profesionales/categorias
```

Debería funcionar correctamente.

