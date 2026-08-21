-- Tabla para tokens de recuperación de contraseña (ejecutar si no usás EF migrations)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PasswordResetTokens')
BEGIN
    CREATE TABLE PasswordResetTokens (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        UsuarioId INT NOT NULL,
        TokenHash NVARCHAR(64) NOT NULL,
        ExpiresAt DATETIME2 NOT NULL,
        UsedAt DATETIME2 NULL,
        CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_PasswordResetTokens_CreatedAt DEFAULT (GETUTCDATE()),
        CONSTRAINT FK_PasswordResetTokens_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id) ON DELETE CASCADE
    );

    CREATE INDEX IX_PasswordResetTokens_UsuarioId ON PasswordResetTokens(UsuarioId);
    CREATE INDEX IX_PasswordResetTokens_TokenHash ON PasswordResetTokens(TokenHash);
END
GO
