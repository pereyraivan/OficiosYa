-- ============================================
-- SCRIPT DE MIGRACIÓN: NORMALIZAR TIPOSERVICIO
-- ============================================
-- Este script normaliza la tabla CategoriaServicios
-- reemplazando el campo Tipo (string) por TipoServicioId (FK)
-- ============================================

USE [OficiosYa_Database]; -- Cambia el nombre de la base de datos si es diferente
GO

-- ============================================
-- 0. PREPARACIÓN: ELIMINAR FK Y COLUMNA TIPOSERVICIOID SI EXISTEN
-- ============================================
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CategoriaServicios_TipoServicios_TipoServicioId')
    ALTER TABLE [CategoriaServicios] DROP CONSTRAINT [FK_CategoriaServicios_TipoServicios_TipoServicioId];
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CategoriaServicios_TipoServicioId' AND object_id = OBJECT_ID('CategoriaServicios'))
    DROP INDEX [IX_CategoriaServicios_TipoServicioId] ON [CategoriaServicios];
GO

-- ============================================
-- 1. CREAR TABLA TIPOSERVICIOS (SI NO EXISTE)
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
-- 2. INSERTAR TIPOS DE SERVICIO (SI NO EXISTEN)
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
-- 3. VERIFICAR SI EXISTE LA COLUMNA TIPO EN CATEGORIASERVICIOS
-- ============================================
DECLARE @TieneColumnaTipo BIT = 0;
IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Tipo' AND Object_ID = Object_ID(N'CategoriaServicios'))
    SET @TieneColumnaTipo = 1;

-- ============================================
-- 4. AGREGAR COLUMNA TIPOSERVICIOID A CATEGORIASERVICIOS (SI NO EXISTE)
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
-- 5. MIGRAR DATOS: CONVERTIR TIPO (STRING) A TIPOSERVICIOID (INT)
-- ============================================
-- Obtener los IDs de los tipos de servicio
DECLARE @OficianteId INT = (SELECT Id FROM [TipoServicios] WHERE [Nombre] = N'Oficiante');
DECLARE @ProfesionalId INT = (SELECT Id FROM [TipoServicios] WHERE [Nombre] = N'Profesional');

-- Verificar si existe la columna Tipo antes de migrar
IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Tipo' AND Object_ID = Object_ID(N'CategoriaServicios'))
BEGIN
    -- Migrar datos de Tipo a TipoServicioId
    UPDATE [CategoriaServicios]
    SET [TipoServicioId] = CASE 
        WHEN LOWER(LTRIM(RTRIM([Tipo]))) = N'oficiante' THEN @OficianteId
        WHEN LOWER(LTRIM(RTRIM([Tipo]))) = N'profesional' THEN @ProfesionalId
        ELSE NULL -- Si hay algún valor inesperado, se deja NULL para revisión manual
    END
    WHERE [TipoServicioId] IS NULL; -- Solo actualizar si no tiene TipoServicioId asignado
    
    DECLARE @RegistrosActualizados INT = @@ROWCOUNT;
    PRINT CONCAT('Se actualizaron ', @RegistrosActualizados, ' registros en CategoriaServicios.');
    
    -- Verificar si hay registros con TipoServicioId NULL (valores inesperados)
    DECLARE @RegistrosSinTipo INT = (SELECT COUNT(*) FROM [CategoriaServicios] WHERE [TipoServicioId] IS NULL);
    IF @RegistrosSinTipo > 0
    BEGIN
        PRINT CONCAT('ADVERTENCIA: Hay ', @RegistrosSinTipo, ' registros con TipoServicioId NULL. Revisar manualmente.');
        SELECT [Id], [Nombre], [Tipo] 
        FROM [CategoriaServicios] 
        WHERE [TipoServicioId] IS NULL;
    END
END
ELSE
BEGIN
    PRINT 'La columna Tipo no existe en CategoriaServicios. Los datos ya fueron migrados o la tabla está normalizada.';
END
GO

-- ============================================
-- 6. HACER TIPOSERVICIOID NOT NULL (SI TODOS LOS REGISTROS TIENEN VALOR)
-- ============================================
IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [TipoServicioId] IS NULL)
BEGIN
    -- Primero eliminar la FK si existe para poder modificar la columna
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CategoriaServicios_TipoServicios_TipoServicioId')
        ALTER TABLE [CategoriaServicios] DROP CONSTRAINT [FK_CategoriaServicios_TipoServicios_TipoServicioId];
    
    -- Hacer la columna NOT NULL
    ALTER TABLE [CategoriaServicios] ALTER COLUMN [TipoServicioId] INT NOT NULL;
    PRINT 'Columna TipoServicioId establecida como NOT NULL.';
END
ELSE
BEGIN
    PRINT 'ADVERTENCIA: No se puede establecer TipoServicioId como NOT NULL porque hay registros con valores NULL.';
END
GO

-- ============================================
-- 7. CREAR FOREIGN KEY Y ÍNDICE
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CategoriaServicios_TipoServicios_TipoServicioId')
BEGIN
    ALTER TABLE [CategoriaServicios] 
    ADD CONSTRAINT [FK_CategoriaServicios_TipoServicios_TipoServicioId] 
    FOREIGN KEY ([TipoServicioId]) REFERENCES [TipoServicios] ([Id]) ON DELETE NO ACTION;
    
    CREATE INDEX [IX_CategoriaServicios_TipoServicioId] ON [CategoriaServicios] ([TipoServicioId]);
    
    PRINT 'Foreign Key y índice creados exitosamente.';
END
ELSE
BEGIN
    PRINT 'Foreign Key ya existe.';
END
GO

-- ============================================
-- 8. ELIMINAR ÍNDICES QUE DEPENDEN DE LA COLUMNA TIPO
-- ============================================
-- Eliminar índice único antiguo IX_CategoriaServicios_Tipo_Nombre si existe
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CategoriaServicios_Tipo_Nombre' AND object_id = OBJECT_ID('CategoriaServicios'))
BEGIN
    DROP INDEX [IX_CategoriaServicios_Tipo_Nombre] ON [CategoriaServicios];
    PRINT 'Índice IX_CategoriaServicios_Tipo_Nombre eliminado.';
END
ELSE
BEGIN
    PRINT 'Índice IX_CategoriaServicios_Tipo_Nombre no existe.';
END
GO

-- Eliminar cualquier otro índice que use la columna Tipo
DECLARE @IndexName NVARCHAR(128);
DECLARE @SQL NVARCHAR(MAX);

DECLARE index_cursor CURSOR FOR
SELECT i.name
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE i.object_id = OBJECT_ID('CategoriaServicios')
  AND c.name = 'Tipo'
  AND i.name IS NOT NULL;

OPEN index_cursor;
FETCH NEXT FROM index_cursor INTO @IndexName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SQL = 'DROP INDEX [' + @IndexName + '] ON [CategoriaServicios];';
    EXEC sp_executesql @SQL;
    PRINT CONCAT('Índice ', @IndexName, ' eliminado.');
    FETCH NEXT FROM index_cursor INTO @IndexName;
END;

CLOSE index_cursor;
DEALLOCATE index_cursor;
GO

-- ============================================
-- 9. CREAR NUEVO ÍNDICE ÚNICO CON TIPOSERVICIOID Y NOMBRE
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CategoriaServicios_TipoServicioId_Nombre' AND object_id = OBJECT_ID('CategoriaServicios'))
BEGIN
    CREATE UNIQUE INDEX [IX_CategoriaServicios_TipoServicioId_Nombre] 
    ON [CategoriaServicios] ([TipoServicioId], [Nombre]);
    PRINT 'Nuevo índice único IX_CategoriaServicios_TipoServicioId_Nombre creado.';
END
ELSE
BEGIN
    PRINT 'Índice IX_CategoriaServicios_TipoServicioId_Nombre ya existe.';
END
GO

-- ============================================
-- 10. ELIMINAR COLUMNA TIPO (STRING) SI EXISTE
-- ============================================
IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Tipo' AND Object_ID = Object_ID(N'CategoriaServicios'))
BEGIN
    -- Verificar que todos los registros tienen TipoServicioId antes de eliminar Tipo
    IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [TipoServicioId] IS NULL)
    BEGIN
        ALTER TABLE [CategoriaServicios] DROP COLUMN [Tipo];
        PRINT 'Columna Tipo (string) eliminada exitosamente.';
    END
    ELSE
    BEGIN
        PRINT 'ADVERTENCIA: No se puede eliminar la columna Tipo porque hay registros con TipoServicioId NULL.';
        PRINT 'Por favor, revisa y corrige los registros antes de eliminar la columna Tipo.';
    END
END
ELSE
BEGIN
    PRINT 'La columna Tipo no existe. Ya fue eliminada o nunca existió.';
END
GO

-- ============================================
-- 11. VERIFICACIÓN FINAL
-- ============================================
PRINT '';
PRINT '============================================';
PRINT 'VERIFICACIÓN FINAL';
PRINT '============================================';

-- Verificar estructura de TipoServicios
PRINT '';
PRINT 'Tabla TipoServicios:';
SELECT [Id], [Nombre], [Descripcion], [Activo] 
FROM [TipoServicios]
ORDER BY [Id];

-- Verificar estructura de CategoriaServicios
PRINT '';
PRINT 'Muestra de CategoriaServicios (primeros 10 registros):';
SELECT TOP 10 
    [Id], 
    [Nombre], 
    [TipoServicioId],
    CASE 
        WHEN [TipoServicioId] IS NOT NULL 
        THEN (SELECT [Nombre] FROM [TipoServicios] WHERE [Id] = [CategoriaServicios].[TipoServicioId])
        ELSE 'NULL'
    END AS [TipoServicioNombre],
    [Activo]
FROM [CategoriaServicios]
ORDER BY [Id];

-- Verificar si existe la columna Tipo (debería estar eliminada)
IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Tipo' AND Object_ID = Object_ID(N'CategoriaServicios'))
    PRINT 'ADVERTENCIA: La columna Tipo todavía existe.';
ELSE
    PRINT 'OK: La columna Tipo fue eliminada correctamente.';

-- Verificar foreign key
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CategoriaServicios_TipoServicios_TipoServicioId')
    PRINT 'OK: Foreign Key FK_CategoriaServicios_TipoServicios_TipoServicioId existe.';
ELSE
    PRINT 'ADVERTENCIA: Foreign Key no existe.';

-- Contar registros por tipo
PRINT '';
PRINT 'Resumen de CategoriaServicios por TipoServicio:';
SELECT 
    TS.[Nombre] AS [TipoServicio],
    COUNT(CS.[Id]) AS [CantidadCategorias]
FROM [TipoServicios] TS
LEFT JOIN [CategoriaServicios] CS ON TS.[Id] = CS.[TipoServicioId]
GROUP BY TS.[Nombre]
ORDER BY TS.[Nombre];

PRINT '';
PRINT '============================================';
PRINT 'MIGRACIÓN COMPLETADA';
PRINT '============================================';
GO

