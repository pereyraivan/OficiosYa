using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OficiosYa.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriaServiciosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Crear tabla CategoriaServicios solo si no existe
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CategoriaServicios')
                BEGIN
                    CREATE TABLE [CategoriaServicios] (
                        [Id] int NOT NULL IDENTITY(1,1),
                        [Nombre] nvarchar(100) NOT NULL,
                        [Descripcion] nvarchar(500) NULL,
                        [Tipo] nvarchar(50) NOT NULL,
                        [Icono] nvarchar(10) NULL,
                        [Activo] bit NOT NULL,
                        [FechaCreacion] datetime2 NOT NULL,
                        CONSTRAINT [PK_CategoriaServicios] PRIMARY KEY ([Id])
                    );
                    
                    CREATE UNIQUE INDEX [IX_CategoriaServicios_Tipo_Nombre] 
                    ON [CategoriaServicios] ([Tipo], [Nombre]);
                END
            ");

            // Insertar datos iniciales solo si la tabla está vacía
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT TOP 1 1 FROM [CategoriaServicios])
                BEGIN
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Icono], [Activo], [FechaCreacion])
                    VALUES
                        ('Plomero', 'Instalación y reparación de sistemas de plomería', 'Oficiante', '🔧', 1, '2024-01-01'),
                        ('Electricista', 'Instalaciones y reparaciones eléctricas', 'Oficiante', '⚡', 1, '2024-01-01'),
                        ('Albañil', 'Construcción y reparación de estructuras', 'Oficiante', '🧱', 1, '2024-01-01'),
                        ('Gasista', 'Instalación y reparación de sistemas de gas', 'Oficiante', '🔥', 1, '2024-01-01'),
                        ('Jardinero', 'Mantenimiento de jardines y espacios verdes', 'Oficiante', '🌿', 1, '2024-01-01'),
                        ('Pintor', 'Pintura de interiores y exteriores', 'Oficiante', '🎨', 1, '2024-01-01'),
                        ('Cerrajero', 'Apertura de cerraduras y copias de llaves', 'Oficiante', '🔒', 1, '2024-01-01'),
                        ('Carpintero', 'Trabajos en madera y muebles', 'Oficiante', '🪚', 1, '2024-01-01'),
                        ('Herrero', 'Trabajos en metal y herrería', 'Oficiante', '⚒️', 1, '2024-01-01'),
                        ('Técnico en Aire Acondicionado', 'Instalación y reparación de sistemas de climatización', 'Oficiante', '❄️', 1, '2024-01-01'),
                        ('Técnico en Refrigeración', 'Reparación de heladeras y freezers', 'Oficiante', '🧊', 1, '2024-01-01'),
                        ('Limpieza', 'Servicios de limpieza doméstica y comercial', 'Oficiante', '🧹', 1, '2024-01-01'),
                        ('Abogado', 'Servicios legales y asesoramiento jurídico', 'Profesional', '⚖️', 1, '2024-01-01'),
                        ('Contador', 'Servicios contables y fiscales', 'Profesional', '📊', 1, '2024-01-01'),
                        ('Médico', 'Servicios de salud y atención médica', 'Profesional', '🩺', 1, '2024-01-01'),
                        ('Psicólogo', 'Servicios de salud mental y terapia', 'Profesional', '🧠', 1, '2024-01-01'),
                        ('Dentista', 'Servicios de salud dental', 'Profesional', '🦷', 1, '2024-01-01'),
                        ('Veterinario', 'Servicios de salud animal', 'Profesional', '🐕', 1, '2024-01-01'),
                        ('Arquitecto', 'Diseño arquitectónico y planificación', 'Profesional', '🏗️', 1, '2024-01-01'),
                        ('Ingeniero', 'Servicios de ingeniería y consultoría técnica', 'Profesional', '⚙️', 1, '2024-01-01'),
                        ('Diseñador Gráfico', 'Servicios de diseño visual y comunicación', 'Profesional', '🎨', 1, '2024-01-01'),
                        ('Traductor', 'Servicios de traducción e interpretación', 'Profesional', '🌐', 1, '2024-01-01'),
                        ('Coach', 'Servicios de coaching y desarrollo personal', 'Profesional', '🎯', 1, '2024-01-01'),
                        ('Nutricionista', 'Servicios de nutrición y alimentación saludable', 'Profesional', '🥗', 1, '2024-01-01');
                END
            ");

            // Crear tabla ProfesionalCategorias solo si no existe
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProfesionalCategorias')
                BEGIN
                    CREATE TABLE [ProfesionalCategorias] (
                        [Id] int NOT NULL IDENTITY(1,1),
                        [ProfesionalId] int NOT NULL,
                        [CategoriaServicioId] int NOT NULL,
                        [FechaAsignacion] datetime2 NOT NULL,
                        [Activo] bit NOT NULL,
                        CONSTRAINT [PK_ProfesionalCategorias] PRIMARY KEY ([Id]),
                        CONSTRAINT [FK_ProfesionalCategorias_Profesionales_ProfesionalId] 
                            FOREIGN KEY ([ProfesionalId]) REFERENCES [Profesionales] ([Id]) ON DELETE CASCADE,
                        CONSTRAINT [FK_ProfesionalCategorias_CategoriaServicios_CategoriaServicioId] 
                            FOREIGN KEY ([CategoriaServicioId]) REFERENCES [CategoriaServicios] ([Id]) ON DELETE NO ACTION
                    );
                    
                    CREATE INDEX [IX_ProfesionalCategorias_ProfesionalId] 
                    ON [ProfesionalCategorias] ([ProfesionalId]);
                    
                    CREATE INDEX [IX_ProfesionalCategorias_CategoriaServicioId] 
                    ON [ProfesionalCategorias] ([CategoriaServicioId]);
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfesionalCategorias");

            migrationBuilder.DropTable(
                name: "CategoriaServicios");
        }
    }
}
