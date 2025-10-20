-- Script de Base de Datos para OficiosYa
-- SQL Server

-- Crear base de datos
CREATE DATABASE OficiosYa;
GO

USE OficiosYa;
GO

-- Tabla Usuarios
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    ContraseñaHash NVARCHAR(255) NOT NULL,
    Telefono NVARCHAR(20) NULL,
    FechaRegistro DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    EsProfesional BIT NOT NULL DEFAULT 0,
    Activo BIT NOT NULL DEFAULT 1
);

-- Tabla Oficios
CREATE TABLE Oficios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(255) NULL
);

-- Tabla Profesionales
CREATE TABLE Profesionales (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    Descripcion NVARCHAR(500) NULL,
    Direccion NVARCHAR(255) NULL,
    Ciudad NVARCHAR(100) NULL,
    Latitud DECIMAL(10,8) NULL,
    Longitud DECIMAL(11,8) NULL,
    PuntuacionPromedio DECIMAL(3,2) NOT NULL DEFAULT 0,
    SuscripcionActiva BIT NOT NULL DEFAULT 0,
    FechaVencimientoSuscripcion DATETIME2 NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);

-- Tabla ProfesionalOficios (Relación muchos a muchos)
CREATE TABLE ProfesionalOficios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProfesionalId INT NOT NULL,
    OficioId INT NOT NULL,
    FOREIGN KEY (ProfesionalId) REFERENCES Profesionales(Id),
    FOREIGN KEY (OficioId) REFERENCES Oficios(Id)
);

-- Tabla Certificados
CREATE TABLE Certificados (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProfesionalId INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Institucion NVARCHAR(100) NOT NULL,
    FechaEmision DATE NOT NULL,
    FechaVencimiento DATE NULL,
    ArchivoUrl NVARCHAR(500) NULL,
    FOREIGN KEY (ProfesionalId) REFERENCES Profesionales(Id)
);

-- Tabla Resenas
CREATE TABLE Resenas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProfesionalId INT NOT NULL,
    ClienteId INT NOT NULL,
    Puntuacion INT NOT NULL CHECK (Puntuacion >= 1 AND Puntuacion <= 5),
    Comentario NVARCHAR(1000) NULL,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (ProfesionalId) REFERENCES Profesionales(Id),
    FOREIGN KEY (ClienteId) REFERENCES Usuarios(Id)
);

-- Tabla Suscripciones
CREATE TABLE Suscripciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProfesionalId INT NOT NULL,
    FechaInicio DATETIME2 NOT NULL,
    FechaFin DATETIME2 NOT NULL,
    Monto DECIMAL(10,2) NOT NULL,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Activa',
    FOREIGN KEY (ProfesionalId) REFERENCES Profesionales(Id)
);

-- Insertar oficios iniciales
INSERT INTO Oficios (Nombre, Descripcion) VALUES
('Electricista', 'Instalaciones eléctricas y reparaciones'),
('Plomero', 'Instalaciones de plomería y reparaciones'),
('Albañil', 'Construcción y albañilería'),
('Gasista', 'Instalaciones de gas y reparaciones'),
('Jardinero', 'Mantenimiento de jardines'),
('Pintor', 'Pintura interior y exterior'),
('Cerrajero', 'Apertura de cerraduras y copias'),
('Limpieza', 'Servicios de limpieza');

-- Insertar usuario administrador
INSERT INTO Usuarios (Nombre, Email, ContraseñaHash, EsProfesional, Activo) VALUES
('Administrador', 'admin@oficiosya.com', '$2a$11$example.hash.here', 0, 1);

-- Crear índices para mejorar rendimiento
CREATE INDEX IX_Usuarios_Email ON Usuarios(Email);
CREATE INDEX IX_Profesionales_Ciudad ON Profesionales(Ciudad);
CREATE INDEX IX_Profesionales_SuscripcionActiva ON Profesionales(SuscripcionActiva);
CREATE INDEX IX_Resenas_ProfesionalId ON Resenas(ProfesionalId);
CREATE INDEX IX_Resenas_ClienteId ON Resenas(ClienteId);

PRINT 'Base de datos OficiosYa creada exitosamente';
