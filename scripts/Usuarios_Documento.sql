-- Agrega DNI/CUIT/CUIL en Usuarios (referencia de pago por transferencia)
IF COL_LENGTH('dbo.Usuarios', 'Documento') IS NULL
BEGIN
    ALTER TABLE dbo.Usuarios
    ADD Documento NVARCHAR(20) NULL;
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'IX_Usuarios_Documento' AND object_id = OBJECT_ID('dbo.Usuarios')
)
BEGIN
    CREATE UNIQUE INDEX IX_Usuarios_Documento
    ON dbo.Usuarios(Documento)
    WHERE Documento IS NOT NULL;
END
GO
