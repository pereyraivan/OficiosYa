-- Tabla para imágenes de trabajos realizados (portfolio) de profesionales/oficiantes
-- Ejecutar si no usás migraciones de EF

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ImagenesTrabajo')
BEGIN
  CREATE TABLE dbo.ImagenesTrabajo (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProfesionalId INT NOT NULL,
    UrlImagen NVARCHAR(500) NOT NULL,
    Titulo NVARCHAR(200) NULL,
    Descripcion NVARCHAR(500) NULL,
    Orden INT NOT NULL DEFAULT 0,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    Activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_ImagenesTrabajo_Profesional FOREIGN KEY (ProfesionalId) REFERENCES dbo.Profesionales(Id) ON DELETE CASCADE
  );
  CREATE INDEX IX_ImagenesTrabajo_ProfesionalId ON dbo.ImagenesTrabajo(ProfesionalId);
END
GO
