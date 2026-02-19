-- ============================================
-- MARCAR MIGRACIÓN AddPlanesSuscripcion COMO APLICADA
-- ============================================
-- Ejecuta este script SOLO si ya aplicaste manualmente
-- las tablas TipoServicios, PlanesSuscripcion y las columnas
-- en Suscripciones usando el script AgregarTablasPago.sql
-- ============================================

USE [OficiosYa_Database];
GO

-- Verificar que las tablas existan antes de marcar la migración
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'PlanesSuscripcion')
AND EXISTS (SELECT * FROM sys.tables WHERE name = 'TipoServicios')
AND EXISTS (SELECT * FROM sys.columns WHERE Name = N'MetodoPago' AND Object_ID = Object_ID(N'Suscripciones'))
BEGIN
    -- Marcar la migración como aplicada
    IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = '20251213182351_AddPlanesSuscripcion')
    BEGIN
        INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
        VALUES ('20251213182351_AddPlanesSuscripcion', '9.0.0');
        
        PRINT 'Migración 20251213182351_AddPlanesSuscripcion marcada como aplicada.';
    END
    ELSE
    BEGIN
        PRINT 'La migración 20251213182351_AddPlanesSuscripcion ya está marcada como aplicada.';
    END
END
ELSE
BEGIN
    PRINT 'ERROR: Las tablas necesarias no existen. Ejecuta primero el script AgregarTablasPago.sql';
END
GO

-- Verificar estado
SELECT [MigrationId], [ProductVersion]
FROM [__EFMigrationsHistory]
ORDER BY [MigrationId];
GO

