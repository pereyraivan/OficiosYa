# 🔄 Sincronizar Modelo Entity Framework con Base de Datos

Esta guía te ayudará a mantener el modelo de Entity Framework Core sincronizado con tu base de datos y hacer que las migraciones Code First funcionen correctamente.

## ✅ Verificar Estado Actual

### 1. Ver migraciones aplicadas

```powershell
cd src\OficiosYa.Web
dotnet ef migrations list --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
```

### 2. Verificar si hay diferencias entre modelo y BD

```powershell
# Intentar crear una migración (si no hay cambios, no se creará nada)
dotnet ef migrations add VerificarSincronizacion --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
```

Si la migración está vacía (solo tiene `Up()` y `Down()` vacíos), significa que el modelo está sincronizado.

## 🔧 Sincronizar Modelo con Base de Datos

### Opción 1: Si la base de datos está actualizada y el modelo no

1. **Aplicar migraciones pendientes:**
   ```powershell
   dotnet ef database update --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
   ```

2. **Verificar que el modelo coincida:**
   ```powershell
   dotnet ef migrations add VerificarSincronizacion --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
   ```

3. **Si la migración está vacía, eliminarla:**
   ```powershell
   dotnet ef migrations remove --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
   ```

### Opción 2: Si el modelo tiene cambios que no están en la BD

1. **Crear migración con los cambios:**
   ```powershell
   dotnet ef migrations add NombreDescriptivoDelCambio --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
   ```

2. **Revisar la migración generada** en `src\Infrastructure\OficiosYa.Persistence\Migrations\`

3. **Aplicar la migración:**
   ```powershell
   dotnet ef database update --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
   ```

### Opción 3: Si la BD tiene cambios que no están en el modelo

Si modificaste la base de datos manualmente y necesitas actualizar el modelo:

1. **Actualizar el ApplicationDbContext** para reflejar los cambios:
   - Agregar propiedades faltantes en las entidades
   - Actualizar configuraciones en `OnModelCreating`
   - Agregar nuevos `DbSet<>` si hay tablas nuevas

2. **Crear una migración vacía y editarla manualmente:**
   ```powershell
   dotnet ef migrations add ActualizarDesdeBD --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
   ```
   
   Luego edita el archivo de migración para que solo refleje los cambios que ya existen en la BD (sin intentar crearlos).

3. **Marcar la migración como aplicada sin ejecutarla:**
   ```sql
   INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
   VALUES ('ActualizarDesdeBD', '9.0.0');
   ```

## 📝 Actualizar ApplicationDbContext

### Verificar configuraciones importantes:

1. **Nombres de tablas:**
   ```csharp
   entity.ToTable("NombreTabla", "dbo");
   ```

2. **Tipos de datos:**
   ```csharp
   entity.Property(e => e.Precio).HasColumnType("decimal(10,2)");
   ```

3. **Índices:**
   ```csharp
   entity.HasIndex(e => e.Email).IsUnique();
   ```

4. **Relaciones:**
   ```csharp
   entity.HasOne(e => e.Profesional)
         .WithMany(p => p.Suscripciones)
         .HasForeignKey(e => e.ProfesionalId)
         .OnDelete(DeleteBehavior.Cascade);
   ```

## 🚀 Flujo de Trabajo Recomendado

### Cuando hagas cambios en las entidades:

1. **Modifica la entidad** en `src\Core\OficiosYa.Domain\Entities\`

2. **Actualiza el ApplicationDbContext** si es necesario (nuevas relaciones, configuraciones, etc.)

3. **Crea la migración:**
   ```powershell
   dotnet ef migrations add DescripcionDelCambio --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
   ```

4. **Revisa la migración generada** antes de aplicarla

5. **Aplica la migración:**
   ```powershell
   dotnet ef database update --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
   ```

### Cuando la base de datos cambie externamente:

1. **Actualiza el ApplicationDbContext** para reflejar los cambios

2. **Crea una migración vacía** y márcala como aplicada (ver Opción 3 arriba)

## ⚠️ Solución de Problemas Comunes

### Error: "The model backing the context has changed"

**Solución:** Crea una nueva migración con los cambios:
```powershell
dotnet ef migrations add FixModelChanges --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
dotnet ef database update --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
```

### Error: "Migration already applied"

**Solución:** La migración ya está aplicada. Verifica el estado:
```powershell
dotnet ef migrations list --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
```

### Error: "Cannot create migration because model is out of sync"

**Solución:** 
1. Aplica las migraciones pendientes:
   ```powershell
   dotnet ef database update --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
   ```

2. Si persiste, verifica que el `ApplicationDbContextModelSnapshot.cs` esté actualizado

### Las migraciones no detectan cambios

**Solución:** 
1. Verifica que las entidades estén correctamente configuradas en `ApplicationDbContext`
2. Asegúrate de que los `DbSet<>` estén declarados
3. Verifica que las configuraciones en `OnModelCreating` sean correctas

## 📋 Checklist de Sincronización

- [ ] Todas las tablas de la BD tienen su `DbSet<>` en `ApplicationDbContext`
- [ ] Todas las columnas tienen sus propiedades correspondientes en las entidades
- [ ] Los tipos de datos coinciden (decimal precision, max length, etc.)
- [ ] Las relaciones están correctamente configuradas
- [ ] Los índices están definidos
- [ ] Las migraciones están aplicadas (`dotnet ef migrations list`)
- [ ] El modelo no tiene cambios pendientes (migración vacía)

## 🔍 Comandos Útiles

```powershell
# Ver estado de migraciones
dotnet ef migrations list --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj

# Ver SQL que se generaría
dotnet ef migrations script --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj

# Aplicar migraciones
dotnet ef database update --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj

# Eliminar última migración (si no se aplicó)
dotnet ef migrations remove --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj

# Crear nueva migración
dotnet ef migrations add NombreMigracion --project ..\Infrastructure\OficiosYa.Persistence\OficiosYa.Persistence.csproj
```

## 📚 Referencias

- [Entity Framework Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Model Configuration](https://learn.microsoft.com/en-us/ef/core/modeling/)

