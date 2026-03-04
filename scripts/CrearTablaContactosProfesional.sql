-- Ejecutar este script en la base de datos OficiosYa_Database
-- (SQL Server Management Studio o Azure Data Studio: abrir, seleccionar OficiosYa_Database, ejecutar)

USE [OficiosYa_Database];
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ContactosProfesional' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE [dbo].[ContactosProfesional] (
        [Id] int IDENTITY(1,1) NOT NULL,
        [ProfesionalId] int NOT NULL,
        [UsuarioId] int NULL,
        [TipoContacto] nvarchar(20) NULL,
        [FechaHora] datetime2 NOT NULL,
        CONSTRAINT [PK_ContactosProfesional] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ContactosProfesional_Profesionales_ProfesionalId] 
            FOREIGN KEY ([ProfesionalId]) REFERENCES [dbo].[Profesionales] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ContactosProfesional_Usuarios_UsuarioId] 
            FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuarios] ([Id]) ON DELETE SET NULL
    );

    CREATE INDEX [IX_ContactosProfesional_ProfesionalId] ON [dbo].[ContactosProfesional] ([ProfesionalId]);
    CREATE INDEX [IX_ContactosProfesional_FechaHora] ON [dbo].[ContactosProfesional] ([FechaHora]);

    PRINT 'Tabla ContactosProfesional creada correctamente.';

    -- Registrar la migración para que EF Core no intente aplicarla de nuevo
    IF NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] WHERE [MigrationId] = N'20260302000000_AddContactosProfesional')
        INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
        VALUES (N'20260302000000_AddContactosProfesional', N'9.0.0');
END
ELSE
    PRINT 'La tabla ContactosProfesional ya existe.';
GO
