-- ============================================
-- SCRIPT PARA VERIFICAR Y AGREGAR COLUMNAS FALTANTES
-- ============================================

USE [OficiosYa_Database] -- Cambia el nombre de la base de datos si es diferente
GO

-- ============================================
-- 1. VERIFICAR Y AGREGAR COLUMNAS EN USUARIOS
-- ============================================

PRINT '=== VERIFICANDO TABLA USUARIOS ===';

-- EsAdmin
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') 
               AND name = 'EsAdmin')
BEGIN
    ALTER TABLE [dbo].[Usuarios]
    ADD [EsAdmin] [bit] NOT NULL DEFAULT 0;
    PRINT '✓ Columna EsAdmin agregada';
END
ELSE
BEGIN
    PRINT '✓ Columna EsAdmin ya existe';
END
GO

-- EmailVerificado
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') 
               AND name = 'EmailVerificado')
BEGIN
    ALTER TABLE [dbo].[Usuarios]
    ADD [EmailVerificado] [bit] NOT NULL DEFAULT 0;
    PRINT '✓ Columna EmailVerificado agregada';
END
ELSE
BEGIN
    PRINT '✓ Columna EmailVerificado ya existe';
END
GO

-- Apellido
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') 
               AND name = 'Apellido')
BEGIN
    ALTER TABLE [dbo].[Usuarios]
    ADD [Apellido] [nvarchar](100) NULL;
    PRINT '✓ Columna Apellido agregada';
END
ELSE
BEGIN
    PRINT '✓ Columna Apellido ya existe';
END
GO

-- Direccion
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') 
               AND name = 'Direccion')
BEGIN
    ALTER TABLE [dbo].[Usuarios]
    ADD [Direccion] [nvarchar](200) NULL;
    PRINT '✓ Columna Direccion agregada';
END
ELSE
BEGIN
    PRINT '✓ Columna Direccion ya existe';
END
GO

-- Ciudad
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') 
               AND name = 'Ciudad')
BEGIN
    ALTER TABLE [dbo].[Usuarios]
    ADD [Ciudad] [nvarchar](100) NULL;
    PRINT '✓ Columna Ciudad agregada';
END
ELSE
BEGIN
    PRINT '✓ Columna Ciudad ya existe';
END
GO

-- Provincia
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') 
               AND name = 'Provincia')
BEGIN
    ALTER TABLE [dbo].[Usuarios]
    ADD [Provincia] [nvarchar](100) NULL;
    PRINT '✓ Columna Provincia agregada';
END
ELSE
BEGIN
    PRINT '✓ Columna Provincia ya existe';
END
GO

-- CodigoPostal
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') 
               AND name = 'CodigoPostal')
BEGIN
    ALTER TABLE [dbo].[Usuarios]
    ADD [CodigoPostal] [nvarchar](10) NULL;
    PRINT '✓ Columna CodigoPostal agregada';
END
ELSE
BEGIN
    PRINT '✓ Columna CodigoPostal ya existe';
END
GO

-- PasswordHash (si no existe pero existe ContraseñaHash, migrar datos)
IF EXISTS (SELECT * FROM sys.columns 
           WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') 
           AND name = 'ContraseñaHash')
   AND NOT EXISTS (SELECT * FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') 
                   AND name = 'PasswordHash')
BEGIN
    ALTER TABLE [dbo].[Usuarios]
    ADD [PasswordHash] [nvarchar](255) NULL;
    
    UPDATE [dbo].[Usuarios]
    SET [PasswordHash] = [ContraseñaHash]
    WHERE [PasswordHash] IS NULL AND [ContraseñaHash] IS NOT NULL;
    
    PRINT '✓ Columna PasswordHash agregada y datos migrados';
END
ELSE IF NOT EXISTS (SELECT * FROM sys.columns 
                     WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') 
                     AND name = 'PasswordHash')
BEGIN
    ALTER TABLE [dbo].[Usuarios]
    ADD [PasswordHash] [nvarchar](255) NULL;
    PRINT '✓ Columna PasswordHash agregada';
END
ELSE
BEGIN
    PRINT '✓ Columna PasswordHash ya existe';
END
GO

-- ============================================
-- 2. VERIFICAR TABLA ZONASCOBERTURA
-- ============================================

PRINT '';
PRINT '=== VERIFICANDO TABLA ZONASCOBERTURA ===';

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ZonasCobertura]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ZonasCobertura](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ProfesionalId] [int] NOT NULL,
        [Localidad] [nvarchar](100) NOT NULL,
        [Provincia] [nvarchar](100) NOT NULL,
        [Departamento] [nvarchar](100) NULL,
        [Latitud] [decimal](10, 8) NOT NULL,
        [Longitud] [decimal](11, 8) NOT NULL,
        [CodigoPostal] [nvarchar](20) NULL,
        [FechaRegistro] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        [Activo] [bit] NOT NULL DEFAULT 1,
        CONSTRAINT [PK_ZonasCobertura] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_ZonasCobertura_Profesionales] FOREIGN KEY([ProfesionalId])
            REFERENCES [dbo].[Profesionales] ([Id])
            ON DELETE CASCADE
    );
    
    CREATE NONCLUSTERED INDEX [IX_ZonasCobertura_ProfesionalId] 
    ON [dbo].[ZonasCobertura] ([ProfesionalId] ASC);
    
    PRINT '✓ Tabla ZonasCobertura creada';
END
ELSE
BEGIN
    PRINT '✓ Tabla ZonasCobertura ya existe';
END
GO

-- ============================================
-- 3. VERIFICAR ESTRUCTURA DE CATEGORIASERVICIOS
-- ============================================

PRINT '';
PRINT '=== VERIFICANDO TABLA CATEGORIASERVICIOS ===';

-- Verificar si existe columna Grupo
DECLARE @TieneGrupo BIT = 0;
IF EXISTS (SELECT * FROM sys.columns 
           WHERE object_id = OBJECT_ID(N'[dbo].[CategoriaServicios]') 
           AND name = 'Grupo')
BEGIN
    SET @TieneGrupo = 1;
    PRINT '✓ Columna Grupo existe en CategoriaServicios';
END
ELSE
BEGIN
    PRINT '⚠ Columna Grupo NO existe en CategoriaServicios';
END

-- Verificar si existe columna CategoriaId
DECLARE @TieneCategoriaId BIT = 0;
IF EXISTS (SELECT * FROM sys.columns 
           WHERE object_id = OBJECT_ID(N'[dbo].[CategoriaServicios]') 
           AND name = 'CategoriaId')
BEGIN
    SET @TieneCategoriaId = 1;
    PRINT '✓ Columna CategoriaId existe en CategoriaServicios';
END
ELSE
BEGIN
    PRINT '⚠ Columna CategoriaId NO existe en CategoriaServicios';
END

-- Si tiene Grupo pero no CategoriaId, el código actual debería funcionar
-- Si tiene CategoriaId pero no Grupo, necesitamos actualizar el código C#
-- Por ahora, solo informamos

GO

-- ============================================
-- 4. RESUMEN DE COLUMNAS EN USUARIOS
-- ============================================

PRINT '';
PRINT '=== RESUMEN DE COLUMNAS EN USUARIOS ===';

SELECT 
    COLUMN_NAME as NombreColumna,
    DATA_TYPE as TipoDato,
    IS_NULLABLE as PermiteNull,
    COLUMN_DEFAULT as ValorPorDefecto
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Usuarios'
ORDER BY ORDINAL_POSITION;

GO

-- ============================================
-- 5. VERIFICAR DATOS
-- ============================================

PRINT '';
PRINT '=== RESUMEN DE DATOS ===';

SELECT 
    'Usuarios' as Tabla,
    COUNT(*) as TotalRegistros,
    SUM(CASE WHEN [EsAdmin] = 1 THEN 1 ELSE 0 END) as TotalAdmins,
    SUM(CASE WHEN [EmailVerificado] = 1 THEN 1 ELSE 0 END) as EmailsVerificados,
    SUM(CASE WHEN [PasswordHash] IS NULL THEN 1 ELSE 0 END) as SinPasswordHash
FROM [dbo].[Usuarios];

SELECT 
    'Profesionales' as Tabla,
    COUNT(*) as TotalRegistros
FROM [dbo].[Profesionales];

SELECT 
    'ZonasCobertura' as Tabla,
    COUNT(*) as TotalRegistros
FROM [dbo].[ZonasCobertura];

GO

PRINT '';
PRINT '=== SCRIPT COMPLETADO ===';


