# 📚 Guía de Migraciones de Entity Framework Core

## ¿Qué son las Migraciones?

Las **migraciones** son archivos de código C# que representan cambios en el esquema de la base de datos. Entity Framework Core las usa para:

- ✅ **Crear** tablas nuevas
- ✅ **Modificar** tablas existentes (agregar/eliminar columnas)
- ✅ **Crear** índices y claves foráneas
- ✅ **Insertar** datos iniciales (seed data)
- ✅ **Mantener** un historial de cambios en la base de datos

## 🎯 Flujo de Trabajo

```
1. Modificar Entidades (Domain)
   ↓
2. Crear Migración (EF Core detecta cambios)
   ↓
3. Revisar Migración (verificar SQL generado)
   ↓
4. Aplicar Migración (actualizar base de datos)
```

## 📝 Comandos Principales

### 1. Crear una Nueva Migración

```bash
# Desde la raíz del proyecto
cd src/Infrastructure/OficiosYa.Persistence

# Crear migración
dotnet ef migrations add NombreDescriptivoDeLaMigracion --project . --startup-project ../../OficiosYa.Web/OficiosYa.Web.csproj

# Ejemplo:
dotnet ef migrations add AddPlanesSuscripcion --project . --startup-project ../../OficiosYa.Web/OficiosYa.Web.csproj
```

**¿Qué hace este comando?**
- Compara el estado actual del `DbContext` con el último snapshot
- Genera archivos `.cs` en la carpeta `Migrations/`
- Crea código C# que representa los cambios SQL necesarios

### 2. Aplicar Migraciones a la Base de Datos

```bash
# Aplicar todas las migraciones pendientes
dotnet ef database update --project . --startup-project ../../OficiosYa.Web/OficiosYa.Web.csproj

# Aplicar hasta una migración específica
dotnet ef database update NombreMigracion --project . --startup-project ../../OficiosYa.Web/OficiosYa.Web.csproj
```

**¿Qué hace este comando?**
- Ejecuta las migraciones pendientes en la base de datos
- Actualiza la tabla `__EFMigrationsHistory` con las migraciones aplicadas
- **NO** modifica los archivos de migración (solo la BD)

### 3. Ver el SQL que se Generará

```bash
# Ver el SQL sin aplicarlo
dotnet ef migrations script --project . --startup-project ../../OficiosYa.Web/OficiosYa.Web.csproj

# Ver SQL de una migración específica
dotnet ef migrations script NombreMigracion --project . --startup-project ../../OficiosYa.Web/OficiosYa.Web.csproj
```

### 4. Eliminar la Última Migración (si no se aplicó)

```bash
# Eliminar la última migración creada (solo si NO se aplicó a la BD)
dotnet ef migrations remove --project . --startup-project ../../OficiosYa.Web/OficiosYa.Web.csproj
```

### 5. Listar Migraciones

```bash
# Ver todas las migraciones
dotnet ef migrations list --project . --startup-project ../../OficiosYa.Web/OficiosYa.Web.csproj
```

## 📂 Estructura de Archivos de Migración

Cada migración genera 2 archivos:

```
Migrations/
├── 20251213120000_AddPlanesSuscripcion.cs          # Código de la migración
├── 20251213120000_AddPlanesSuscripcion.Designer.cs # Metadatos
└── ApplicationDbContextModelSnapshot.cs            # Snapshot del modelo actual
```

### Archivo Principal de Migración

```csharp
public partial class AddPlanesSuscripcion : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Código para APLICAR la migración
        migrationBuilder.CreateTable(...);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Código para REVERTIR la migración
        migrationBuilder.DropTable(...);
    }
}
```

## 🔄 Proceso Completo: Ejemplo Práctico

### Escenario: Agregar nueva tabla `PlanesSuscripcion`

**Paso 1: Crear la Entidad**
```csharp
// Ya está creada: PlanSuscripcion.cs
```

**Paso 2: Agregar al DbContext**
```csharp
// Ya está agregado: public DbSet<PlanSuscripcion> PlanesSuscripcion { get; set; }
```

**Paso 3: Crear la Migración**
```bash
cd src/Infrastructure/OficiosYa.Persistence
dotnet ef migrations add AddPlanesSuscripcion --project . --startup-project ../../OficiosYa.Web/OficiosYa.Web.csproj
```

**Paso 4: Revisar la Migración Generada**
- Abrir el archivo generado en `Migrations/`
- Verificar que el código sea correcto
- Opcional: Ver el SQL con `dotnet ef migrations script`

**Paso 5: Aplicar la Migración**
```bash
dotnet ef database update --project . --startup-project ../../OficiosYa.Web/OficiosYa.Web.csproj
```

**Paso 6: Verificar**
- Revisar la base de datos
- Confirmar que la tabla se creó correctamente
- Verificar datos iniciales (seed data)

## ⚠️ Buenas Prácticas

### ✅ HACER:
1. **Revisar** las migraciones antes de aplicarlas
2. **Hacer backup** de la base de datos antes de migraciones importantes
3. **Probar** en desarrollo antes de producción
4. **Usar nombres descriptivos** para las migraciones
5. **Una migración por cambio lógico** (no mezclar cambios no relacionados)

### ❌ NO HACER:
1. **NO modificar** migraciones ya aplicadas a producción
2. **NO eliminar** migraciones que ya están en producción
3. **NO mezclar** cambios de diferentes features en una sola migración
4. **NO aplicar** migraciones sin revisar primero

## 🚨 Resolución de Problemas

### Error: "Unable to create an object of type 'ApplicationDbContext'"

**Solución:** Asegúrate de especificar el proyecto de inicio:
```bash
--startup-project ../../OficiosYa.Web/OficiosYa.Web.csproj
```

### Error: "The migration 'X' has already been applied"

**Solución:** La migración ya está aplicada. Si necesitas revertir:
```bash
# Revertir a una migración anterior
dotnet ef database update NombreMigracionAnterior --project . --startup-project ../../OficiosYa.Web/OficiosYa.Web.csproj
```

### Error: "There is no Entity Framework Core project"

**Solución:** Ejecuta el comando desde la carpeta del proyecto Persistence:
```bash
cd src/Infrastructure/OficiosYa.Persistence
```

## 📊 Tabla de Control: `__EFMigrationsHistory`

Entity Framework crea automáticamente esta tabla para rastrear qué migraciones se han aplicado:

```sql
SELECT * FROM __EFMigrationsHistory
ORDER BY MigrationId DESC
```

## 🔗 Referencias

- [Documentación oficial de EF Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Comandos de la CLI de EF Core](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

