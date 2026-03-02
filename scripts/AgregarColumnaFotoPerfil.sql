-- Agregar columna FotoPerfil a Profesionales si no existe
-- Guarda la ruta relativa de la foto (ej: /uploads/perfiles/123_guid.jpg)

IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID(N'[dbo].[Profesionales]') AND name = 'FotoPerfil'
)
BEGIN
    ALTER TABLE [dbo].[Profesionales]
    ADD [FotoPerfil] NVARCHAR(200) NULL;
END
GO
