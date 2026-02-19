-- ============================================
-- INICIALIZAR TABLA __EFMigrationsHistory
-- ============================================
-- Este script crea la tabla de control de migraciones de EF Core
-- y marca las migraciones existentes como aplicadas si las tablas ya existen
-- ============================================

USE [OficiosYa_Database]; -- Cambia el nombre de la base de datos si es diferente
GO

-- ============================================
-- 1. CREAR TABLA __EFMigrationsHistory SI NO EXISTE
-- ============================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] NVARCHAR(150) NOT NULL,
        [ProductVersion] NVARCHAR(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId] ASC)
    );
    
    PRINT 'Tabla __EFMigrationsHistory creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'Tabla __EFMigrationsHistory ya existe.';
END
GO

-- ============================================
-- 2. MARCAR MIGRACIONES COMO APLICADAS SI LAS TABLAS EXISTEN
-- ============================================

-- Migración: AddCategoriaServiciosTable
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'CategoriaServicios')
AND NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20251209053955_AddCategoriaServiciosTable')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20251209053955_AddCategoriaServiciosTable', '9.0.0');
    PRINT 'Migración 20251209053955_AddCategoriaServiciosTable marcada como aplicada.';
END
GO

-- Migración: AddGrupoToCategoriaServicio
IF EXISTS (SELECT * FROM sys.columns WHERE Name = N'Grupo' AND Object_ID = Object_ID(N'CategoriaServicios'))
AND NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20251213023510_AddGrupoToCategoriaServicio')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20251213023510_AddGrupoToCategoriaServicio', '9.0.0');
    PRINT 'Migración 20251213023510_AddGrupoToCategoriaServicio marcada como aplicada.';
END
GO

-- Migración: AddGrupoAndTecnologiaCategorias
IF EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Programador' OR [Nombre] = N'Desarrollador Web')
AND NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20251213031020_AddGrupoAndTecnologiaCategorias')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20251213031020_AddGrupoAndTecnologiaCategorias', '9.0.0');
    PRINT 'Migración 20251213031020_AddGrupoAndTecnologiaCategorias marcada como aplicada.';
END
GO

-- Migración: InsertMissingCategorias
-- Esta migración solo inserta datos, así que verificamos si hay más de 24 categorías
IF EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Id] > 24)
AND NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20251213032747_InsertMissingCategorias')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES ('20251213032747_InsertMissingCategorias', '9.0.0');
    PRINT 'Migración 20251213032747_InsertMissingCategorias marcada como aplicada.';
END
GO

-- ============================================
-- 3. VERIFICAR ESTADO ACTUAL
-- ============================================
PRINT '';
PRINT '============================================';
PRINT 'ESTADO DE MIGRACIONES';
PRINT '============================================';
PRINT '';

SELECT 
    [MigrationId],
    [ProductVersion]
FROM [__EFMigrationsHistory]
ORDER BY [MigrationId];

PRINT '';
DECLARE @TotalMigraciones INT;
SELECT @TotalMigraciones = COUNT(*) FROM [__EFMigrationsHistory];
PRINT 'Total de migraciones registradas: ' + CAST(@TotalMigraciones AS NVARCHAR(10));
GO

-- ============================================
-- NOTA IMPORTANTE
-- ============================================
-- Si ejecutaste el script MigracionTipoServicio.sql manualmente,
-- la migración AddPlanesSuscripcion NO debe marcarse como aplicada aquí.
-- Debes aplicarla usando: dotnet ef database update
-- ============================================

