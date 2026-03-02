-- Tablas para publicar empleos (clientes publican, oficiantes/profesionales se postularán después)
-- Ejecutar en la base de datos del proyecto OficiosYa

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TrabajosPublicados')
BEGIN
    CREATE TABLE [dbo].[TrabajosPublicados] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Descripcion] NVARCHAR(2000) NOT NULL,
        [Telefono] NVARCHAR(20) NULL,
        [Email] NVARCHAR(150) NOT NULL,
        [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [Estado] NVARCHAR(20) NOT NULL DEFAULT 'Abierto',
        [Activo] BIT NOT NULL DEFAULT 1,
        CONSTRAINT [PK_TrabajosPublicados] PRIMARY KEY ([Id])
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TrabajoCategorias')
BEGIN
    CREATE TABLE [dbo].[TrabajoCategorias] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [TrabajoPublicadoId] INT NOT NULL,
        [CategoriaServicioId] INT NOT NULL,
        CONSTRAINT [PK_TrabajoCategorias] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TrabajoCategorias_TrabajosPublicados_TrabajoPublicadoId] 
            FOREIGN KEY ([TrabajoPublicadoId]) REFERENCES [dbo].[TrabajosPublicados] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_TrabajoCategorias_CategoriaServicios_CategoriaServicioId] 
            FOREIGN KEY ([CategoriaServicioId]) REFERENCES [dbo].[CategoriaServicios] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [UQ_TrabajoCategorias_TrabajoPublicadoId_CategoriaServicioId] 
            UNIQUE ([TrabajoPublicadoId], [CategoriaServicioId])
    );
    CREATE INDEX [IX_TrabajoCategorias_TrabajoPublicadoId] ON [dbo].[TrabajoCategorias] ([TrabajoPublicadoId]);
    CREATE INDEX [IX_TrabajoCategorias_CategoriaServicioId] ON [dbo].[TrabajoCategorias] ([CategoriaServicioId]);
END
GO
