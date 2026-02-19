-- ============================================
-- SCRIPT DE NORMALIZACIÓN: TipoServicio
-- ============================================
-- Este script normaliza la tabla CategoriaServicios
-- reemplazando el campo string 'Tipo' por una FK a TipoServicios
-- ============================================

USE [OficiosYa_Database]; -- Cambia el nombre de la base de datos si es diferente
GO

-- ============================================
-- 0. PREPARACIÓN: ELIMINAR FK Y COLUMNA SI EXISTEN
-- ============================================
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CategoriaServicios_TipoServicios_TipoServicioId')
    ALTER TABLE [CategoriaServicios] DROP CONSTRAINT [FK_CategoriaServicios_TipoServicios_TipoServicioId];
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CategoriaServicios_TipoServicioId')
    DROP INDEX [IX_CategoriaServicios_TipoServicioId] ON [CategoriaServicios];
GO

IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'TipoServicioId' AND Object_ID = Object_ID(N'CategoriaServicios'))
    ALTER TABLE [CategoriaServicios] DROP COLUMN [TipoServicioId];
GO

-- ============================================
-- 1. CREAR TABLA TIPOSERVICIOS
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TipoServicios')
BEGIN
    CREATE TABLE [TipoServicios] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Nombre] NVARCHAR(50) NOT NULL,
        [Descripcion] NVARCHAR(500) NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_TipoServicios] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    CREATE UNIQUE INDEX [IX_TipoServicios_Nombre] ON [TipoServicios] ([Nombre]);
    PRINT 'Tabla TipoServicios creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'Tabla TipoServicios ya existe.';
END
GO

-- ============================================
-- 2. INSERTAR TIPOS DE SERVICIO
-- ============================================
IF NOT EXISTS (SELECT 1 FROM [TipoServicios] WHERE [Nombre] = N'Oficiante')
BEGIN
    INSERT INTO [TipoServicios] ([Nombre], [Descripcion], [Activo], [FechaCreacion])
    VALUES (N'Oficiante', N'Servicios de oficios y trabajos manuales', 1, GETUTCDATE());
    PRINT 'TipoServicio "Oficiante" insertado.';
END
ELSE
BEGIN
    PRINT 'TipoServicio "Oficiante" ya existe.';
END
GO

IF NOT EXISTS (SELECT 1 FROM [TipoServicios] WHERE [Nombre] = N'Profesional')
BEGIN
    INSERT INTO [TipoServicios] ([Nombre], [Descripcion], [Activo], [FechaCreacion])
    VALUES (N'Profesional', N'Servicios profesionales y especializados', 1, GETUTCDATE());
    PRINT 'TipoServicio "Profesional" insertado.';
END
ELSE
BEGIN
    PRINT 'TipoServicio "Profesional" ya existe.';
END
GO

-- ============================================
-- 3. AGREGAR COLUMNA TIPOSERVICIOID A CATEGORIASERVICIOS
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'TipoServicioId' AND Object_ID = Object_ID(N'CategoriaServicios'))
BEGIN
    ALTER TABLE [CategoriaServicios] ADD [TipoServicioId] INT NULL;
    PRINT 'Columna TipoServicioId agregada a CategoriaServicios.';
END
ELSE
BEGIN
    PRINT 'Columna TipoServicioId ya existe en CategoriaServicios.';
END
GO

-- ============================================
-- 4. MIGRAR DATOS: ASIGNAR TIPOSERVICIOID SEGÚN EL STRING TIPO
-- ============================================
DECLARE @OficianteId INT = (SELECT Id FROM [TipoServicios] WHERE Nombre = N'Oficiante');
DECLARE @ProfesionalId INT = (SELECT Id FROM [TipoServicios] WHERE Nombre = N'Profesional');

-- Actualizar registros con Tipo = 'Oficiante'
UPDATE [CategoriaServicios]
SET [TipoServicioId] = @OficianteId
WHERE [Tipo] = N'Oficiante' AND [TipoServicioId] IS NULL;
PRINT 'Registros con Tipo "Oficiante" actualizados.';

-- Actualizar registros con Tipo = 'Profesional'
UPDATE [CategoriaServicios]
SET [TipoServicioId] = @ProfesionalId
WHERE [Tipo] = N'Profesional' AND [TipoServicioId] IS NULL;
PRINT 'Registros con Tipo "Profesional" actualizados.';

-- Verificar si hay registros sin TipoServicioId asignado
IF EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [TipoServicioId] IS NULL)
BEGIN
    PRINT 'ADVERTENCIA: Hay registros en CategoriaServicios sin TipoServicioId asignado.';
    SELECT Id, Nombre, Tipo FROM [CategoriaServicios] WHERE [TipoServicioId] IS NULL;
END
GO

-- ============================================
-- 5. HACER TIPOSERVICIOID REQUERIDO (NOT NULL)
-- ============================================
-- Primero actualizar cualquier NULL restante con un valor por defecto
UPDATE [CategoriaServicios]
SET [TipoServicioId] = (SELECT TOP 1 Id FROM [TipoServicios] WHERE Nombre = N'Oficiante')
WHERE [TipoServicioId] IS NULL;
GO

-- Ahora hacer la columna NOT NULL
ALTER TABLE [CategoriaServicios]
ALTER COLUMN [TipoServicioId] INT NOT NULL;
PRINT 'Columna TipoServicioId configurada como NOT NULL.';
GO

-- ============================================
-- 6. CREAR ÍNDICE Y FOREIGN KEY
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CategoriaServicios_TipoServicioId')
BEGIN
    CREATE INDEX [IX_CategoriaServicios_TipoServicioId] ON [CategoriaServicios] ([TipoServicioId]);
    PRINT 'Índice IX_CategoriaServicios_TipoServicioId creado.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CategoriaServicios_TipoServicios_TipoServicioId')
BEGIN
    ALTER TABLE [CategoriaServicios]
    ADD CONSTRAINT [FK_CategoriaServicios_TipoServicios_TipoServicioId]
    FOREIGN KEY ([TipoServicioId]) REFERENCES [TipoServicios] ([Id]) ON DELETE NO ACTION;
    PRINT 'Foreign Key FK_CategoriaServicios_TipoServicios_TipoServicioId creada.';
END
GO

-- ============================================
-- 7. ELIMINAR ÍNDICE ÚNICO ANTIGUO (Tipo, Nombre)
-- ============================================
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CategoriaServicios_Tipo_Nombre')
BEGIN
    DROP INDEX [IX_CategoriaServicios_Tipo_Nombre] ON [CategoriaServicios];
    PRINT 'Índice antiguo IX_CategoriaServicios_Tipo_Nombre eliminado.';
END
GO

-- ============================================
-- 8. CREAR NUEVO ÍNDICE ÚNICO (TipoServicioId, Nombre)
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CategoriaServicios_TipoServicioId_Nombre')
BEGIN
    CREATE UNIQUE INDEX [IX_CategoriaServicios_TipoServicioId_Nombre]
    ON [CategoriaServicios] ([TipoServicioId], [Nombre]);
    PRINT 'Nuevo índice único IX_CategoriaServicios_TipoServicioId_Nombre creado.';
END
GO

-- ============================================
-- 9. ELIMINAR COLUMNA TIPO (STRING)
-- ============================================
-- NOTA: Comentar esta sección si quieres mantener la columna Tipo temporalmente
-- para verificación antes de eliminarla definitivamente

-- Verificar que todos los registros tienen TipoServicioId asignado
IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [TipoServicioId] IS NULL)
BEGIN
    -- Eliminar la columna Tipo
    IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Tipo' AND Object_ID = Object_ID(N'CategoriaServicios'))
    BEGIN
        ALTER TABLE [CategoriaServicios] DROP COLUMN [Tipo];
        PRINT 'Columna Tipo (string) eliminada exitosamente.';
    END
    ELSE
    BEGIN
        PRINT 'Columna Tipo ya no existe.';
    END
END
ELSE
BEGIN
    PRINT 'ERROR: No se puede eliminar la columna Tipo porque hay registros sin TipoServicioId.';
END
GO

-- ============================================
-- 10. VERIFICACIÓN FINAL
-- ============================================
PRINT '';
PRINT '============================================';
PRINT 'VERIFICACIÓN FINAL';
PRINT '============================================';

-- Verificar tipos de servicio
PRINT 'Tipos de Servicio:';
SELECT Id, Nombre, Descripcion, Activo FROM [TipoServicios];
GO

-- Verificar categorías con sus tipos
PRINT 'Categorías de Servicio (primeras 10):';
SELECT TOP 10 
    CS.Id,
    CS.Nombre,
    TS.Nombre AS TipoServicio,
    CS.Grupo,
    CS.Activo
FROM [CategoriaServicios] CS
INNER JOIN [TipoServicios] TS ON CS.TipoServicioId = TS.Id
ORDER BY TS.Nombre, CS.Nombre;
GO

-- Contar categorías por tipo
PRINT 'Conteo de categorías por tipo:';
SELECT 
    TS.Nombre AS TipoServicio,
    COUNT(CS.Id) AS TotalCategorias
FROM [TipoServicios] TS
LEFT JOIN [CategoriaServicios] CS ON TS.Id = CS.TipoServicioId
GROUP BY TS.Nombre
ORDER BY TS.Nombre;
GO

PRINT '';
PRINT '============================================';
PRINT 'NORMALIZACIÓN COMPLETADA EXITOSAMENTE';
PRINT '============================================';
GO

