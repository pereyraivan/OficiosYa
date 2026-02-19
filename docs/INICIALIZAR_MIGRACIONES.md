# 🔧 Inicializar Migraciones de EF Core

## Problema

Si tu base de datos fue creada manualmente o con scripts SQL (no usando migraciones de EF Core), no tendrás la tabla `__EFMigrationsHistory`. Esta tabla es necesaria para que EF Core sepa qué migraciones ya se aplicaron.

## Solución

### Paso 1: Crear la tabla de control

Ejecuta el script `scripts/CrearTablaEFMigrationsHistory.sql` en tu base de datos:

```sql
-- Este script crea la tabla __EFMigrationsHistory
-- Ejecútalo en SQL Server Management Studio
```

### Paso 2: Marcar migraciones existentes como aplicadas

Si ya tienes tablas creadas manualmente, ejecuta `scripts/InicializarEFMigrationsHistory.sql`:

```sql
-- Este script:
-- 1. Crea la tabla __EFMigrationsHistory
-- 2. Marca las migraciones existentes como aplicadas (si las tablas ya existen)
```

### Paso 3: Aplicar migraciones pendientes

Después de inicializar, aplica las migraciones pendientes:

```powershell
cd src\Infrastructure\OficiosYa.Persistence
dotnet ef database update --project . --startup-project ..\..\OficiosYa.Web\OficiosYa.Web.csproj
```

## Verificar Estado

Para ver qué migraciones están aplicadas:

```sql
SELECT * FROM __EFMigrationsHistory
ORDER BY MigrationId;
```

## Importante

- **NO marques manualmente** la migración `AddPlanesSuscripcion` si aún no la aplicaste
- Si ejecutaste `MigracionTipoServicio.sql` manualmente, esa migración NO está en el historial de EF Core
- La nueva migración `AddPlanesSuscripcion` debe aplicarse usando `dotnet ef database update`

