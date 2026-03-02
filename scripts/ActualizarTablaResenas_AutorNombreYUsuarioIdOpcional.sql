-- Ejecutar este script en la base de datos OficiosYa_DataBase
-- para agregar la columna AutorNombre y hacer UsuarioId opcional (reseñas sin login).

-- Si no estás conectado a la base, descomentá y ajustá el nombre si hace falta:
-- USE [OficiosYa_DataBase];
-- GO

-- 1. Agregar columna AutorNombre si no existe
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'dbo.Resenas') AND name = N'AutorNombre'
)
BEGIN
    ALTER TABLE dbo.Resenas ADD AutorNombre nvarchar(100) NULL;
    PRINT 'Columna AutorNombre agregada.';
END
ELSE
    PRINT 'Columna AutorNombre ya existe.';
GO

-- 2. Hacer UsuarioId nullable (para reseñas de visitantes sin cuenta)
ALTER TABLE dbo.Resenas ALTER COLUMN UsuarioId int NULL;
PRINT 'UsuarioId ahora acepta NULL.';
GO
