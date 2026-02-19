-- Script para verificar y recrear la tabla TipoServicios en el esquema dbo
-- Ejecutar en SQL Server Management Studio conectado a la base de datos OficiosYa_Database

USE [OficiosYa_Database];
GO

-- Verificar si la tabla existe
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'TipoServicios' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    PRINT 'La tabla dbo.TipoServicios existe.';
    
    -- Verificar datos
    SELECT COUNT(*) AS TotalRegistros FROM [dbo].[TipoServicios];
    
    -- Mostrar datos actuales
    SELECT * FROM [dbo].[TipoServicios];
END
ELSE
BEGIN
    PRINT 'La tabla dbo.TipoServicios NO existe. Creándola...';
    
    -- Crear la tabla en el esquema dbo explícitamente
    CREATE TABLE [dbo].[TipoServicios] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Nombre] NVARCHAR(50) NOT NULL,
        [Descripcion] NVARCHAR(500) NULL,
        [Activo] BIT NOT NULL DEFAULT 1,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_TipoServicios] PRIMARY KEY ([Id]),
        CONSTRAINT [IX_TipoServicios_Nombre] UNIQUE ([Nombre])
    );
    
    PRINT 'Tabla dbo.TipoServicios creada exitosamente.';
    
    -- Insertar datos iniciales
    INSERT INTO [dbo].[TipoServicios] ([Nombre], [Descripcion], [Activo], [FechaCreacion])
    VALUES 
        ('Oficiante', 'Servicios de oficios y trabajos manuales', 1, GETUTCDATE()),
        ('Profesional', 'Servicios profesionales y especializados', 1, GETUTCDATE());
    
    PRINT 'Datos iniciales insertados.';
END
GO

-- Verificar permisos
SELECT 
    t.name AS TableName,
    s.name AS SchemaName,
    p.permission_name AS Permission,
    pr.name AS PrincipalName
FROM sys.tables t
INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
LEFT JOIN sys.database_permissions p ON p.major_id = t.object_id
LEFT JOIN sys.database_principals pr ON p.grantee_principal_id = pr.principal_id
WHERE t.name = 'TipoServicios' AND s.name = 'dbo';
GO

PRINT 'Verificación completada.';