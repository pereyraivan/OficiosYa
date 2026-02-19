-- ============================================
-- VERIFICAR NOMBRE EXACTO DE LA TABLA
-- ============================================
-- Este script verifica el nombre exacto de la tabla
-- y su esquema en la base de datos
-- ============================================

USE [OficiosYa_Database];
GO

-- Verificar todas las tablas que contengan "TipoServicio" en el nombre
PRINT '============================================';
PRINT 'TABLAS QUE CONTIENEN "TipoServicio"';
PRINT '============================================';

SELECT 
    TABLE_SCHEMA AS [Esquema],
    TABLE_NAME AS [NombreTabla],
    TABLE_TYPE AS [Tipo]
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME LIKE '%TipoServicio%'
ORDER BY TABLE_SCHEMA, TABLE_NAME;
GO

-- Verificar si existe TipoServicios (con s)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TipoServicios')
BEGIN
    PRINT '';
    PRINT '✓ Tabla TipoServicios (con s) EXISTE';
    
    DECLARE @EsquemaTipoServicios NVARCHAR(128) = OBJECT_SCHEMA_NAME(OBJECT_ID('TipoServicios'));
    PRINT 'Esquema: ' + @EsquemaTipoServicios;
    
    DECLARE @CountTipoServicios INT;
    SELECT @CountTipoServicios = COUNT(*) FROM [TipoServicios];
    PRINT 'Cantidad de registros: ' + CAST(@CountTipoServicios AS NVARCHAR(10));
    
    SELECT TOP 5 * FROM [TipoServicios];
END
ELSE
BEGIN
    PRINT '';
    PRINT '✗ Tabla TipoServicios (con s) NO EXISTE';
END
GO

-- Verificar si existe TipoServicio (sin s)
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TipoServicio')
BEGIN
    PRINT '';
    PRINT '✓ Tabla TipoServicio (sin s) EXISTE';
    
    DECLARE @EsquemaTipoServicio NVARCHAR(128) = OBJECT_SCHEMA_NAME(OBJECT_ID('TipoServicio'));
    PRINT 'Esquema: ' + @EsquemaTipoServicio;
    
    DECLARE @CountTipoServicio INT;
    SELECT @CountTipoServicio = COUNT(*) FROM [TipoServicio];
    PRINT 'Cantidad de registros: ' + CAST(@CountTipoServicio AS NVARCHAR(10));
    
    SELECT TOP 5 * FROM [TipoServicio];
END
ELSE
BEGIN
    PRINT '';
    PRINT '✗ Tabla TipoServicio (sin s) NO EXISTE';
END
GO

-- Listar todas las tablas del esquema actual
PRINT '';
PRINT '============================================';
PRINT 'TODAS LAS TABLAS EN EL ESQUEMA ACTUAL';
PRINT '============================================';

SELECT 
    TABLE_SCHEMA AS [Esquema],
    TABLE_NAME AS [NombreTabla]
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
GO

