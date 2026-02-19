-- ============================================
-- CREAR TABLA __EFMigrationsHistory
-- ============================================
-- Esta tabla es necesaria para que Entity Framework Core
-- pueda rastrear qué migraciones se han aplicado
-- ============================================

USE [OficiosYa_Database]; -- Cambia el nombre de la base de datos si es diferente
GO

-- ============================================
-- CREAR TABLA __EFMigrationsHistory SI NO EXISTE
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
-- VERIFICAR MIGRACIONES EXISTENTES
-- ============================================
PRINT '';
PRINT 'Migraciones registradas actualmente:';
SELECT [MigrationId], [ProductVersion]
FROM [__EFMigrationsHistory]
ORDER BY [MigrationId];
GO

