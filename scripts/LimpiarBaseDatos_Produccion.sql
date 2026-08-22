/*
  ============================================================
  OficiosYa — Limpiar base de datos para producción
  ============================================================

  Elimina datos de prueba y deja la BD lista para un servidor
  nuevo, conservando catálogos y usuarios administradores.

  CONSERVA:
    - TipoServicios, CategoriaServicios, PlanesSuscripcion
    - Categorias (si existe por migración normalizada)
    - Usuarios con EsAdmin = 1
    - __EFMigrationsHistory (historial de migraciones EF)

  ELIMINA:
    - Profesionales, suscripciones, reseñas, contactos, trabajos
      publicados, imágenes de portfolio, tokens de reset, etc.
    - Usuarios que NO son admin

  ANTES DE EJECUTAR:
    1. Hacé un BACKUP completo de la base.
    2. Cambiá USE [OficiosYa_Database] si tu BD tiene otro nombre.
    3. Poné @ConfirmarEjecucion = 1 más abajo.
    4. Borrá manualmente archivos subidos en el servidor:
       wwwroot/uploads/  (fotos de perfil, portfolio, certificados)
    5. Después podés ejecutar CrearUsuarioAdmin.sql si necesitás
       resetear la cuenta admin@oficiosya.com

  NO ejecutar en producción con usuarios reales ya activos.
*/

USE [OficiosYa_Database];
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

-- ========== CONFIGURACIÓN ==========
-- Cambiar 0 → 1 SOLO después de hacer backup y revisar este script.
DECLARE @ConfirmarEjecucion BIT = 0;

IF @ConfirmarEjecucion <> 1
BEGIN
    RAISERROR(
        N'Abortado: poné @ConfirmarEjecucion = 1 después de hacer backup y revisar este script.',
        16, 1
    );
    RETURN;
END

PRINT N'=== Inicio limpieza OficiosYa ===';
PRINT N'Fecha: ' + CONVERT(NVARCHAR(30), GETUTCDATE(), 126) + N' UTC';

BEGIN TRANSACTION;

BEGIN TRY

    DECLARE @Filas INT;

    -- 1) Trabajos publicados (hijos primero)
    IF OBJECT_ID(N'dbo.TrabajoCategorias', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.TrabajoCategorias;
        SET @Filas = @@ROWCOUNT;
        PRINT N'TrabajoCategorias: ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    IF OBJECT_ID(N'dbo.TrabajosPublicados', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.TrabajosPublicados;
        SET @Filas = @@ROWCOUNT;
        PRINT N'TrabajosPublicados: ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    -- 2) Contactos y tokens (referencian Usuario/Profesional)
    IF OBJECT_ID(N'dbo.ContactosProfesional', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.ContactosProfesional;
        SET @Filas = @@ROWCOUNT;
        PRINT N'ContactosProfesional: ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    IF OBJECT_ID(N'dbo.PasswordResetTokens', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.PasswordResetTokens;
        SET @Filas = @@ROWCOUNT;
        PRINT N'PasswordResetTokens: ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    -- 3) Reseñas (UsuarioId tiene RESTRICT — hay que borrarlas antes que usuarios)
    IF OBJECT_ID(N'dbo.Resenas', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.Resenas;
        SET @Filas = @@ROWCOUNT;
        PRINT N'Resenas: ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    -- 4) Datos del profesional
    IF OBJECT_ID(N'dbo.ProfesionalCategorias', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.ProfesionalCategorias;
        SET @Filas = @@ROWCOUNT;
        PRINT N'ProfesionalCategorias: ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    IF OBJECT_ID(N'dbo.ImagenesTrabajo', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.ImagenesTrabajo;
        SET @Filas = @@ROWCOUNT;
        PRINT N'ImagenesTrabajo: ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    IF OBJECT_ID(N'dbo.ZonasCobertura', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.ZonasCobertura;
        SET @Filas = @@ROWCOUNT;
        PRINT N'ZonasCobertura: ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    IF OBJECT_ID(N'dbo.Certificados', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.Certificados;
        SET @Filas = @@ROWCOUNT;
        PRINT N'Certificados: ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    IF OBJECT_ID(N'dbo.Suscripciones', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.Suscripciones;
        SET @Filas = @@ROWCOUNT;
        PRINT N'Suscripciones: ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    -- Tabla legacy (por si aún existe)
    IF OBJECT_ID(N'dbo.ProfesionalOficios', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.ProfesionalOficios;
        SET @Filas = @@ROWCOUNT;
        PRINT N'ProfesionalOficios (legacy): ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    IF OBJECT_ID(N'dbo.Profesionales', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.Profesionales;
        SET @Filas = @@ROWCOUNT;
        PRINT N'Profesionales: ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    -- 5) Usuarios no admin
    IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NOT NULL
    BEGIN
        DELETE FROM dbo.Usuarios WHERE EsAdmin = 0;
        SET @Filas = @@ROWCOUNT;
        PRINT N'Usuarios (no admin): ' + CAST(@Filas AS NVARCHAR(20)) + N' filas eliminadas';
    END

    COMMIT TRANSACTION;
    PRINT N'=== Transacción confirmada (COMMIT) ===';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    DECLARE @Msg NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @Num INT = ERROR_NUMBER();
    RAISERROR(N'Error %d — rollback aplicado: %s', 16, 1, @Num, @Msg);
    RETURN;
END CATCH

-- ========== RESETEAR IDENTITY (tablas vaciadas) ==========
PRINT N'=== Reseteando contadores IDENTITY ===';

DECLARE @Tablas TABLE (Nombre SYSNAME);
INSERT INTO @Tablas (Nombre) VALUES
    (N'dbo.TrabajoCategorias'),
    (N'dbo.TrabajosPublicados'),
    (N'dbo.ContactosProfesional'),
    (N'dbo.PasswordResetTokens'),
    (N'dbo.Resenas'),
    (N'dbo.ProfesionalCategorias'),
    (N'dbo.ImagenesTrabajo'),
    (N'dbo.ZonasCobertura'),
    (N'dbo.Certificados'),
    (N'dbo.Suscripciones'),
    (N'dbo.ProfesionalOficios'),
    (N'dbo.Profesionales');

DECLARE @Tabla SYSNAME;
DECLARE @Sql NVARCHAR(500);
DECLARE @MaxId INT;

DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
    SELECT Nombre FROM @Tablas;

OPEN cur;
FETCH NEXT FROM cur INTO @Tabla;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF OBJECT_ID(@Tabla, N'U') IS NOT NULL
    BEGIN
        SET @Sql = N'SELECT @MaxId = ISNULL(MAX(Id), 0) FROM ' + @Tabla;
        EXEC sp_executesql @Sql, N'@MaxId INT OUTPUT', @MaxId = @MaxId OUTPUT;

        IF @MaxId = 0
            DBCC CHECKIDENT (@Tabla, RESEED, 0);
        ELSE
            DBCC CHECKIDENT (@Tabla, RESEED, @MaxId);

        PRINT N'  ' + @Tabla + N' → RESEED ' + CAST(@MaxId AS NVARCHAR(20));
    END

    FETCH NEXT FROM cur INTO @Tabla;
END

CLOSE cur;
DEALLOCATE cur;

-- Usuarios: reseed al Id máximo restante (admins conservados)
IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NOT NULL
BEGIN
    SELECT @MaxId = ISNULL(MAX(Id), 0) FROM dbo.Usuarios;
    IF @MaxId = 0
        DBCC CHECKIDENT (N'dbo.Usuarios', RESEED, 0);
    ELSE
        DBCC CHECKIDENT (N'dbo.Usuarios', RESEED, @MaxId);

    PRINT N'  dbo.Usuarios → RESEED ' + CAST(@MaxId AS NVARCHAR(20));
END

-- ========== RESUMEN POST-LIMPIEZA ==========
PRINT N'';
PRINT N'=== Resumen (datos conservados) ===';

IF OBJECT_ID(N'dbo.TipoServicios', N'U') IS NOT NULL
    SELECT N'TipoServicios' AS Tabla, COUNT(*) AS Filas FROM dbo.TipoServicios;

IF OBJECT_ID(N'dbo.CategoriaServicios', N'U') IS NOT NULL
    SELECT N'CategoriaServicios' AS Tabla, COUNT(*) AS Filas FROM dbo.CategoriaServicios;

IF OBJECT_ID(N'dbo.PlanesSuscripcion', N'U') IS NOT NULL
    SELECT N'PlanesSuscripcion' AS Tabla, COUNT(*) AS Filas FROM dbo.PlanesSuscripcion;

IF OBJECT_ID(N'dbo.Categorias', N'U') IS NOT NULL
    SELECT N'Categorias' AS Tabla, COUNT(*) AS Filas FROM dbo.Categorias;

IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NOT NULL
BEGIN
    SELECT N'Usuarios (admins restantes)' AS Tabla, COUNT(*) AS Filas
    FROM dbo.Usuarios WHERE EsAdmin = 1;

    SELECT Id, Nombre, Apellido, Email, EsAdmin, Activo
    FROM dbo.Usuarios
    WHERE EsAdmin = 1;
END

PRINT N'';
PRINT N'=== Limpieza finalizada ===';
PRINT N'Próximos pasos sugeridos:';
PRINT N'  1. Ejecutar scripts pendientes en prod (PasswordResetTokens, Usuarios_Documento, etc.)';
PRINT N'  2. Ejecutar CrearUsuarioAdmin.sql si querés resetear admin@oficiosya.com';
PRINT N'  3. Vaciar carpeta wwwroot/uploads/ en el servidor web';
PRINT N'  4. Actualizar connection string, Jwt:SecretKey y Email en appsettings de prod';
