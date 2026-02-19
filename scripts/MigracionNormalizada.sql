-- ============================================
-- SCRIPT DE MIGRACIÓN NORMALIZADA
-- Aplicar normalización: Tabla Categorias + Foreign Key en CategoriaServicios
-- ============================================

USE [OficiosYa_Database] -- Cambia el nombre de la base de datos si es diferente
GO

-- ============================================
-- 1. CREAR TABLA CATEGORIAS (NORMALIZADA)
-- ============================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categorias]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Categorias](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Nombre] [nvarchar](100) NOT NULL,
        [Descripcion] [nvarchar](500) NULL,
        [Activo] [bit] NOT NULL DEFAULT 1,
        [FechaCreacion] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_Categorias] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
    
    -- Índice único para evitar duplicados
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Categorias_Nombre] 
    ON [dbo].[Categorias] ([Nombre] ASC)
END
GO

-- ============================================
-- 2. CREAR TABLA ZONASCOBERTURA (si no existe)
-- ============================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ZonasCobertura]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ZonasCobertura](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ProfesionalId] [int] NOT NULL,
        [Localidad] [nvarchar](100) NOT NULL,
        [Provincia] [nvarchar](100) NOT NULL,
        [Departamento] [nvarchar](100) NULL,
        [Latitud] [decimal](10, 8) NOT NULL,
        [Longitud] [decimal](11, 8) NOT NULL,
        [CodigoPostal] [nvarchar](20) NULL,
        [FechaRegistro] [datetime2](7) NOT NULL DEFAULT GETUTCDATE(),
        [Activo] [bit] NOT NULL DEFAULT 1,
        CONSTRAINT [PK_ZonasCobertura] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_ZonasCobertura_Profesionales] FOREIGN KEY([ProfesionalId])
            REFERENCES [dbo].[Profesionales] ([Id])
            ON DELETE CASCADE
    )
    
    CREATE NONCLUSTERED INDEX [IX_ZonasCobertura_ProfesionalId] 
    ON [dbo].[ZonasCobertura] ([ProfesionalId] ASC)
END
GO

-- ============================================
-- 3. MODIFICAR TABLA CATEGORIASERVICIOS
-- ============================================

-- Eliminar columna Grupo si existe
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CategoriaServicios]') AND name = 'Grupo')
BEGIN
    ALTER TABLE [dbo].[CategoriaServicios] DROP COLUMN [Grupo]
END
GO

-- Agregar columna CategoriaId si no existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CategoriaServicios]') AND name = 'CategoriaId')
BEGIN
    ALTER TABLE [dbo].[CategoriaServicios] 
    ADD [CategoriaId] [int] NULL
END
GO

-- Crear Foreign Key si no existe
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_CategoriaServicios_Categorias')
BEGIN
    ALTER TABLE [dbo].[CategoriaServicios]
    ADD CONSTRAINT [FK_CategoriaServicios_Categorias] 
    FOREIGN KEY([CategoriaId])
    REFERENCES [dbo].[Categorias] ([Id])
    ON DELETE SET NULL
END
GO

-- Crear índice para mejorar rendimiento
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_CategoriaServicios_CategoriaId')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_CategoriaServicios_CategoriaId] 
    ON [dbo].[CategoriaServicios] ([CategoriaId] ASC)
END
GO

-- ============================================
-- 4. INSERTAR CATEGORÍAS (TABLA NORMALIZADA)
-- ============================================

-- Construcción
IF NOT EXISTS (SELECT 1 FROM [Categorias] WHERE [Nombre] = N'Construcción')
    INSERT INTO [Categorias] ([Nombre], [Descripcion], [Activo], [FechaCreacion])
    VALUES (N'Construcción', N'Categoría para oficios y profesiones relacionadas con construcción', 1, GETDATE());

-- Hogar y Mantenimiento
IF NOT EXISTS (SELECT 1 FROM [Categorias] WHERE [Nombre] = N'Hogar y Mantenimiento')
    INSERT INTO [Categorias] ([Nombre], [Descripcion], [Activo], [FechaCreacion])
    VALUES (N'Hogar y Mantenimiento', N'Categoría para servicios de mantenimiento del hogar', 1, GETDATE());

-- Salud
IF NOT EXISTS (SELECT 1 FROM [Categorias] WHERE [Nombre] = N'Salud')
    INSERT INTO [Categorias] ([Nombre], [Descripcion], [Activo], [FechaCreacion])
    VALUES (N'Salud', N'Categoría para profesionales de la salud', 1, GETDATE());

-- Legal y Financiero
IF NOT EXISTS (SELECT 1 FROM [Categorias] WHERE [Nombre] = N'Legal y Financiero')
    INSERT INTO [Categorias] ([Nombre], [Descripcion], [Activo], [FechaCreacion])
    VALUES (N'Legal y Financiero', N'Categoría para servicios legales y financieros', 1, GETDATE());

-- Diseño y Arquitectura
IF NOT EXISTS (SELECT 1 FROM [Categorias] WHERE [Nombre] = N'Diseño y Arquitectura')
    INSERT INTO [Categorias] ([Nombre], [Descripcion], [Activo], [FechaCreacion])
    VALUES (N'Diseño y Arquitectura', N'Categoría para profesionales de diseño y arquitectura', 1, GETDATE());

-- Otros Servicios
IF NOT EXISTS (SELECT 1 FROM [Categorias] WHERE [Nombre] = N'Otros Servicios')
    INSERT INTO [Categorias] ([Nombre], [Descripcion], [Activo], [FechaCreacion])
    VALUES (N'Otros Servicios', N'Categoría para otros servicios profesionales', 1, GETDATE());

-- Tecnología e IT
IF NOT EXISTS (SELECT 1 FROM [Categorias] WHERE [Nombre] = N'Tecnología e IT')
    INSERT INTO [Categorias] ([Nombre], [Descripcion], [Activo], [FechaCreacion])
    VALUES (N'Tecnología e IT', N'Categoría para profesionales de tecnología e informática', 1, GETDATE());

GO

-- ============================================
-- 5. ACTUALIZAR CATEGORIASERVICIOS EXISTENTES
-- ============================================

-- Obtener IDs de categorías
DECLARE @CategoriaConstruccion INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Construcción');
DECLARE @CategoriaHogar INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Hogar y Mantenimiento');
DECLARE @CategoriaSalud INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Salud');
DECLARE @CategoriaLegal INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Legal y Financiero');
DECLARE @CategoriaDiseno INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Diseño y Arquitectura');
DECLARE @CategoriaOtros INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Otros Servicios');
DECLARE @CategoriaIT INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Tecnología e IT');

-- Oficiantes - Construcción
UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaConstruccion 
WHERE [Nombre] = N'Plomero' AND [Tipo] = N'Oficiante';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaConstruccion 
WHERE [Nombre] = N'Electricista' AND [Tipo] = N'Oficiante';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaConstruccion 
WHERE [Nombre] = N'Albañil' AND [Tipo] = N'Oficiante';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaConstruccion 
WHERE [Nombre] = N'Gasista' AND [Tipo] = N'Oficiante';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaConstruccion 
WHERE [Nombre] = N'Pintor' AND [Tipo] = N'Oficiante';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaConstruccion 
WHERE [Nombre] = N'Carpintero' AND [Tipo] = N'Oficiante';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaConstruccion 
WHERE [Nombre] = N'Herrero' AND [Tipo] = N'Oficiante';

-- Oficiantes - Hogar y Mantenimiento
UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaHogar 
WHERE [Nombre] = N'Jardinero' AND [Tipo] = N'Oficiante';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaHogar 
WHERE [Nombre] = N'Cerrajero' AND [Tipo] = N'Oficiante';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaHogar 
WHERE [Nombre] = N'Técnico en Aire Acondicionado' AND [Tipo] = N'Oficiante';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaHogar 
WHERE [Nombre] = N'Técnico en Refrigeración' AND [Tipo] = N'Oficiante';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaHogar 
WHERE [Nombre] = N'Limpieza' AND [Tipo] = N'Oficiante';

-- Profesionales - Salud
UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaSalud 
WHERE [Nombre] = N'Médico' AND [Tipo] = N'Profesional';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaSalud 
WHERE [Nombre] = N'Psicólogo' AND [Tipo] = N'Profesional';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaSalud 
WHERE [Nombre] = N'Dentista' AND [Tipo] = N'Profesional';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaSalud 
WHERE [Nombre] = N'Veterinario' AND [Tipo] = N'Profesional';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaSalud 
WHERE [Nombre] = N'Nutricionista' AND [Tipo] = N'Profesional';

-- Profesionales - Legal y Financiero
UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaLegal 
WHERE [Nombre] = N'Abogado' AND [Tipo] = N'Profesional';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaLegal 
WHERE [Nombre] = N'Contador' AND [Tipo] = N'Profesional';

-- Profesionales - Diseño y Arquitectura
UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaDiseno 
WHERE [Nombre] = N'Arquitecto' AND [Tipo] = N'Profesional';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaDiseno 
WHERE [Nombre] = N'Ingeniero' AND [Tipo] = N'Profesional';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaDiseno 
WHERE [Nombre] = N'Diseñador Gráfico' AND [Tipo] = N'Profesional';

-- Profesionales - Otros Servicios
UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaOtros 
WHERE [Nombre] = N'Traductor' AND [Tipo] = N'Profesional';

UPDATE [CategoriaServicios] SET [CategoriaId] = @CategoriaOtros 
WHERE [Nombre] = N'Coach' AND [Tipo] = N'Profesional';

GO

-- ============================================
-- 6. INSERTAR NUEVAS CATEGORÍAS DE SERVICIO
-- ============================================

DECLARE @CategoriaConstruccion INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Construcción');
DECLARE @CategoriaHogar INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Hogar y Mantenimiento');
DECLARE @CategoriaSalud INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Salud');
DECLARE @CategoriaLegal INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Legal y Financiero');
DECLARE @CategoriaDiseno INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Diseño y Arquitectura');
DECLARE @CategoriaOtros INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Otros Servicios');
DECLARE @CategoriaIT INT = (SELECT [Id] FROM [Categorias] WHERE [Nombre] = N'Tecnología e IT');

-- Oficiantes adicionales - Construcción
IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Durlockero' AND [Tipo] = N'Oficiante')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Durlockero', N'Instalación de construcción en seco (durlock)', N'Oficiante', @CategoriaConstruccion, N'🧱', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Construcción en Seco' AND [Tipo] = N'Oficiante')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Construcción en Seco', N'Trabajos en construcción en seco y durlock', N'Oficiante', @CategoriaConstruccion, N'🏗️', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Techista' AND [Tipo] = N'Oficiante')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Techista', N'Reparación y construcción de techos', N'Oficiante', @CategoriaConstruccion, N'🏠', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Soldador' AND [Tipo] = N'Oficiante')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Soldador', N'Trabajos de soldadura y metalurgia', N'Oficiante', @CategoriaConstruccion, N'🔥', 1, GETDATE());

-- Oficiantes adicionales - Hogar y Mantenimiento
IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Tapicero' AND [Tipo] = N'Oficiante')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Tapicero', N'Reparación y tapizado de muebles', N'Oficiante', @CategoriaHogar, N'🪑', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Marmolista' AND [Tipo] = N'Oficiante')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Marmolista', N'Trabajos en mármol y granito', N'Oficiante', @CategoriaHogar, N'🪨', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Vidriero' AND [Tipo] = N'Oficiante')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Vidriero', N'Instalación y reparación de vidrios', N'Oficiante', @CategoriaHogar, N'🪟', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Colchonero' AND [Tipo] = N'Oficiante')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Colchonero', N'Reparación y restauración de colchones', N'Oficiante', @CategoriaHogar, N'🛏️', 1, GETDATE());

-- Profesionales de Salud adicionales
IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Fonoaudiólogo' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Fonoaudiólogo', N'Servicios de fonoaudiología y terapia del lenguaje', N'Profesional', @CategoriaSalud, N'🗣️', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Kinesiólogo' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Kinesiólogo', N'Servicios de kinesiología y fisioterapia', N'Profesional', @CategoriaSalud, N'💪', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Enfermero' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Enfermero', N'Servicios de enfermería y cuidados de salud', N'Profesional', @CategoriaSalud, N'🏥', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Terapeuta Ocupacional' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Terapeuta Ocupacional', N'Servicios de terapia ocupacional', N'Profesional', @CategoriaSalud, N'🤲', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Optometrista' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Optometrista', N'Servicios de salud visual y optometría', N'Profesional', @CategoriaSalud, N'👓', 1, GETDATE());

-- Profesionales Legal y Financiero adicionales
IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Martillero Público' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Martillero Público', N'Servicios de remates y subastas', N'Profesional', @CategoriaLegal, N'🔨', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Escribano' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Escribano', N'Servicios notariales y certificaciones', N'Profesional', @CategoriaLegal, N'📝', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Asesor Financiero' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Asesor Financiero', N'Asesoramiento financiero y inversiones', N'Profesional', @CategoriaLegal, N'💰', 1, GETDATE());

-- Profesionales Diseño y Arquitectura adicionales
IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Diseñador de Interiores' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Diseñador de Interiores', N'Diseño de interiores y decoración', N'Profesional', @CategoriaDiseno, N'🪑', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Diseñador Web' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Diseñador Web', N'Diseño y desarrollo web', N'Profesional', @CategoriaDiseno, N'💻', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Fotógrafo' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Fotógrafo', N'Servicios de fotografía profesional', N'Profesional', @CategoriaDiseno, N'📷', 1, GETDATE());

-- Profesionales Otros Servicios adicionales
IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Profesor Particular' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Profesor Particular', N'Clases particulares y tutorías', N'Profesional', @CategoriaOtros, N'📚', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Instructor de Fitness' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Instructor de Fitness', N'Entrenamiento personal y fitness', N'Profesional', @CategoriaOtros, N'💪', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Masajista' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Masajista', N'Servicios de masajes y relajación', N'Profesional', @CategoriaOtros, N'👐', 1, GETDATE());

-- Tecnología e IT
IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Programador' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Programador', N'Desarrollo de software y aplicaciones', N'Profesional', @CategoriaIT, N'💻', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Desarrollador Web' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Desarrollador Web', N'Desarrollo de aplicaciones y sitios web', N'Profesional', @CategoriaIT, N'🌐', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Desarrollador Mobile' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Desarrollador Mobile', N'Desarrollo de aplicaciones móviles', N'Profesional', @CategoriaIT, N'📱', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Tester/QA' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Tester/QA', N'Testing y aseguramiento de calidad de software', N'Profesional', @CategoriaIT, N'🔍', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Analista de Sistemas' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Analista de Sistemas', N'Análisis y diseño de sistemas informáticos', N'Profesional', @CategoriaIT, N'📊', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Especialista en Ciberseguridad' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Especialista en Ciberseguridad', N'Seguridad informática y protección de datos', N'Profesional', @CategoriaIT, N'🔒', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'DevOps Engineer' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'DevOps Engineer', N'Automatización e infraestructura de desarrollo', N'Profesional', @CategoriaIT, N'⚙️', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Administrador de Sistemas' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Administrador de Sistemas', N'Administración y mantenimiento de sistemas', N'Profesional', @CategoriaIT, N'🖥️', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'DBA' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'DBA', N'Administración de bases de datos', N'Profesional', @CategoriaIT, N'🗄️', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Arquitecto de Software' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Arquitecto de Software', N'Diseño arquitectónico de software y sistemas', N'Profesional', @CategoriaIT, N'🏛️', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Product Manager' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Product Manager', N'Gestión de productos tecnológicos', N'Profesional', @CategoriaIT, N'📦', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Scrum Master' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Scrum Master', N'Gestión ágil de proyectos de software', N'Profesional', @CategoriaIT, N'🎯', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Data Scientist' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Data Scientist', N'Análisis de datos y ciencia de datos', N'Profesional', @CategoriaIT, N'📈', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Cloud Engineer' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Cloud Engineer', N'Ingeniería y administración en la nube', N'Profesional', @CategoriaIT, N'☁️', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'UX/UI Designer' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'UX/UI Designer', N'Diseño de experiencia e interfaz de usuario', N'Profesional', @CategoriaIT, N'🎨', 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Soporte Técnico IT' AND [Tipo] = N'Profesional')
    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [CategoriaId], [Icono], [Activo], [FechaCreacion])
    VALUES (N'Soporte Técnico IT', N'Soporte técnico y helpdesk', N'Profesional', @CategoriaIT, N'🛠️', 1, GETDATE());

GO

-- ============================================
-- 7. VERIFICACIÓN
-- ============================================

-- Verificar categorías creadas
SELECT [Id], [Nombre], [Descripcion], [Activo] 
FROM [Categorias] 
ORDER BY [Nombre];

-- Verificar total de categorías de servicio
SELECT COUNT(*) as TotalCategoriaServicios FROM [CategoriaServicios];

-- Verificar por categoría (usando JOIN)
SELECT 
    c.[Nombre] as Categoria,
    COUNT(cs.[Id]) as CantidadServicios
FROM [Categorias] c
LEFT JOIN [CategoriaServicios] cs ON c.[Id] = cs.[CategoriaId]
GROUP BY c.[Id], c.[Nombre]
ORDER BY c.[Nombre];

-- Verificar por tipo
SELECT [Tipo], COUNT(*) as Cantidad 
FROM [CategoriaServicios] 
GROUP BY [Tipo]
ORDER BY [Tipo];

-- Verificar servicios sin categoría asignada
SELECT [Id], [Nombre], [Tipo] 
FROM [CategoriaServicios] 
WHERE [CategoriaId] IS NULL;

GO

