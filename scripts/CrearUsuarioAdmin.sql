/*
  Administradores OficiosYa — ejecutar en SSMS.

  1) Listar admins actuales
  2) Promover TU cuenta (más simple)
  3) Crear admin@oficiosya.com / Admin123!
*/

USE OficiosYa_Database;
GO

-- ========== 1) ¿Quién es admin? ==========
SELECT Id, Nombre, Apellido, Email, EsAdmin, Activo
FROM dbo.Usuarios
WHERE EsAdmin = 1;
GO

-- ========== 2) Hacer admin a un usuario que YA existe ==========
-- Descomentá y cambiá el email:
/*
UPDATE dbo.Usuarios
SET EsAdmin = 1, Activo = 1, EmailVerificado = 1
WHERE Email = N'oficiosyapro@gmail.com';

SELECT Id, Email, EsAdmin FROM dbo.Usuarios WHERE Email = N'oficiosyapro@gmail.com';
*/

-- ========== 3) Crear o resetear admin@oficiosya.com ==========
-- Login: admin@oficiosya.com
-- Contraseña: Admin123!

DECLARE @Email NVARCHAR(150) = N'admin@oficiosya.com';
DECLARE @Hash NVARCHAR(255) = N'$2a$11$VsdqHUGzZk10HBU0Y7ab8.ccnPq.Q2i8JoiVQdbF6v.qS0.yb0jae';

IF EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Email = @Email)
BEGIN
    UPDATE dbo.Usuarios
    SET
        Nombre = N'Administrador',
        Apellido = N'Sistema',
        PasswordHash = @Hash,
        EsAdmin = 1,
        Activo = 1,
        EmailVerificado = 1
    WHERE Email = @Email;

    PRINT N'Admin actualizado: admin@oficiosya.com / Admin123!';
END
ELSE
BEGIN
    INSERT INTO dbo.Usuarios (
        Nombre, Apellido, Email, PasswordHash,
        FechaRegistro, EmailVerificado, Activo, EsAdmin
    )
    VALUES (
        N'Administrador', N'Sistema', @Email, @Hash,
        GETUTCDATE(), 1, 1, 1
    );

    PRINT N'Admin creado: admin@oficiosya.com / Admin123!';
END
GO

SELECT Id, Email, EsAdmin, Activo FROM dbo.Usuarios WHERE Email = N'admin@oficiosya.com';
GO
