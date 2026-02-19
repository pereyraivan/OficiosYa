using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OficiosYa.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGrupoAndTecnologiaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Grupo",
                table: "CategoriaServicios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ZonasCobertura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfesionalId = table.Column<int>(type: "int", nullable: false),
                    Localidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Provincia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Latitud = table.Column<decimal>(type: "decimal(10,8)", nullable: false),
                    Longitud = table.Column<decimal>(type: "decimal(11,8)", nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZonasCobertura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZonasCobertura_Profesionales_ProfesionalId",
                        column: x => x.ProfesionalId,
                        principalTable: "Profesionales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Grupo",
                value: "Construcción");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Grupo",
                value: "Construcción");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 3,
                column: "Grupo",
                value: "Construcción");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 4,
                column: "Grupo",
                value: "Construcción");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 5,
                column: "Grupo",
                value: "Hogar y Mantenimiento");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 6,
                column: "Grupo",
                value: "Construcción");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 7,
                column: "Grupo",
                value: "Hogar y Mantenimiento");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 8,
                column: "Grupo",
                value: "Construcción");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 9,
                column: "Grupo",
                value: "Construcción");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 10,
                column: "Grupo",
                value: "Hogar y Mantenimiento");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 11,
                column: "Grupo",
                value: "Hogar y Mantenimiento");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 12,
                column: "Grupo",
                value: "Hogar y Mantenimiento");

            // Actualizar grupos basándose en el nombre, sin cambiar nombres ni descripciones
            migrationBuilder.Sql(@"
                UPDATE [CategoriaServicios] SET [Grupo] = N'Legal y Financiero' WHERE [Nombre] = N'Abogado' AND [Tipo] = N'Profesional';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Legal y Financiero' WHERE [Nombre] = N'Contador' AND [Tipo] = N'Profesional';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Salud' WHERE [Nombre] = N'Médico' AND [Tipo] = N'Profesional';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Salud' WHERE [Nombre] = N'Psicólogo' AND [Tipo] = N'Profesional';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Salud' WHERE [Nombre] = N'Dentista' AND [Tipo] = N'Profesional';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Salud' WHERE [Nombre] = N'Veterinario' AND [Tipo] = N'Profesional';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Salud' WHERE [Nombre] = N'Nutricionista' AND [Tipo] = N'Profesional';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Diseño y Arquitectura' WHERE [Nombre] = N'Arquitecto' AND [Tipo] = N'Profesional';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Diseño y Arquitectura' WHERE [Nombre] = N'Ingeniero' AND [Tipo] = N'Profesional';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Diseño y Arquitectura' WHERE [Nombre] = N'Diseñador Gráfico' AND [Tipo] = N'Profesional';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Otros Servicios' WHERE [Nombre] = N'Traductor' AND [Tipo] = N'Profesional';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Otros Servicios' WHERE [Nombre] = N'Coach' AND [Tipo] = N'Profesional';
            ");

            // Insertar nuevas categorías usando SQL directo para evitar problemas con IDs
            migrationBuilder.Sql(@"
                -- Insertar nuevas categorías solo si no existen (verificando por nombre y tipo)
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Durlockero' AND [Tipo] = N'Oficiante')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Durlockero', N'Instalación de construcción en seco (durlock)', N'Oficiante', N'Construcción', N'🧱', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Construcción en Seco' AND [Tipo] = N'Oficiante')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Construcción en Seco', N'Trabajos en construcción en seco y durlock', N'Oficiante', N'Construcción', N'🏗️', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Techista' AND [Tipo] = N'Oficiante')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Techista', N'Reparación y construcción de techos', N'Oficiante', N'Construcción', N'🏠', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Soldador' AND [Tipo] = N'Oficiante')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Soldador', N'Trabajos de soldadura y metalurgia', N'Oficiante', N'Construcción', N'🔥', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Tapicero' AND [Tipo] = N'Oficiante')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Tapicero', N'Reparación y tapizado de muebles', N'Oficiante', N'Hogar y Mantenimiento', N'🪑', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Marmolista' AND [Tipo] = N'Oficiante')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Marmolista', N'Trabajos en mármol y granito', N'Oficiante', N'Hogar y Mantenimiento', N'🪨', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Vidriero' AND [Tipo] = N'Oficiante')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Vidriero', N'Instalación y reparación de vidrios', N'Oficiante', N'Hogar y Mantenimiento', N'🪟', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Colchonero' AND [Tipo] = N'Oficiante')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Colchonero', N'Reparación y restauración de colchones', N'Oficiante', N'Hogar y Mantenimiento', N'🛏️', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Fonoaudiólogo' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Fonoaudiólogo', N'Servicios de fonoaudiología y terapia del lenguaje', N'Profesional', N'Salud', N'🗣️', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Kinesiólogo' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Kinesiólogo', N'Servicios de kinesiología y fisioterapia', N'Profesional', N'Salud', N'💪', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Enfermero' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Enfermero', N'Servicios de enfermería y cuidados de salud', N'Profesional', N'Salud', N'🏥', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Terapeuta Ocupacional' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Terapeuta Ocupacional', N'Servicios de terapia ocupacional', N'Profesional', N'Salud', N'🤲', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Optometrista' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Optometrista', N'Servicios de salud visual y optometría', N'Profesional', N'Salud', N'👓', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Martillero Público' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Martillero Público', N'Servicios de remates y subastas', N'Profesional', N'Legal y Financiero', N'🔨', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Escribano' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Escribano', N'Servicios notariales y certificaciones', N'Profesional', N'Legal y Financiero', N'📝', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Asesor Financiero' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Asesor Financiero', N'Asesoramiento financiero y inversiones', N'Profesional', N'Legal y Financiero', N'💰', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Diseñador de Interiores' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Diseñador de Interiores', N'Diseño de interiores y decoración', N'Profesional', N'Diseño y Arquitectura', N'🪑', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Diseñador Web' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Diseñador Web', N'Diseño y desarrollo web', N'Profesional', N'Diseño y Arquitectura', N'💻', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Fotógrafo' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Fotógrafo', N'Servicios de fotografía profesional', N'Profesional', N'Diseño y Arquitectura', N'📷', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Profesor Particular' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Profesor Particular', N'Clases particulares y tutorías', N'Profesional', N'Otros Servicios', N'📚', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Instructor de Fitness' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Instructor de Fitness', N'Entrenamiento personal y fitness', N'Profesional', N'Otros Servicios', N'💪', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Masajista' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Masajista', N'Servicios de masajes y relajación', N'Profesional', N'Otros Servicios', N'👐', 1, '2024-01-01');
                
                -- Tecnología e IT
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Programador' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Programador', N'Desarrollo de software y aplicaciones', N'Profesional', N'Tecnología e IT', N'💻', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Desarrollador Web' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Desarrollador Web', N'Desarrollo de aplicaciones y sitios web', N'Profesional', N'Tecnología e IT', N'🌐', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Desarrollador Mobile' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Desarrollador Mobile', N'Desarrollo de aplicaciones móviles', N'Profesional', N'Tecnología e IT', N'📱', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Tester/QA' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Tester/QA', N'Testing y aseguramiento de calidad de software', N'Profesional', N'Tecnología e IT', N'🔍', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Analista de Sistemas' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Analista de Sistemas', N'Análisis y diseño de sistemas informáticos', N'Profesional', N'Tecnología e IT', N'📊', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Especialista en Ciberseguridad' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Especialista en Ciberseguridad', N'Seguridad informática y protección de datos', N'Profesional', N'Tecnología e IT', N'🔒', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'DevOps Engineer' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'DevOps Engineer', N'Automatización e infraestructura de desarrollo', N'Profesional', N'Tecnología e IT', N'⚙️', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Administrador de Sistemas' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Administrador de Sistemas', N'Administración y mantenimiento de sistemas', N'Profesional', N'Tecnología e IT', N'🖥️', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'DBA' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'DBA', N'Administración de bases de datos', N'Profesional', N'Tecnología e IT', N'🗄️', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Arquitecto de Software' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Arquitecto de Software', N'Diseño arquitectónico de software y sistemas', N'Profesional', N'Tecnología e IT', N'🏛️', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Product Manager' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Product Manager', N'Gestión de productos tecnológicos', N'Profesional', N'Tecnología e IT', N'📦', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Scrum Master' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Scrum Master', N'Gestión ágil de proyectos de software', N'Profesional', N'Tecnología e IT', N'🎯', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Data Scientist' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Data Scientist', N'Análisis de datos y ciencia de datos', N'Profesional', N'Tecnología e IT', N'📈', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Cloud Engineer' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Cloud Engineer', N'Ingeniería y administración en la nube', N'Profesional', N'Tecnología e IT', N'☁️', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'UX/UI Designer' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'UX/UI Designer', N'Diseño de experiencia e interfaz de usuario', N'Profesional', N'Tecnología e IT', N'🎨', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Soporte Técnico IT' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Soporte Técnico IT', N'Soporte técnico y helpdesk', N'Profesional', N'Tecnología e IT', N'🛠️', 1, '2024-01-01');
            ");

            migrationBuilder.CreateIndex(
                name: "IX_ZonasCobertura_ProfesionalId",
                table: "ZonasCobertura",
                column: "ProfesionalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZonasCobertura");

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DropColumn(
                name: "Grupo",
                table: "CategoriaServicios");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios legales y asesoramiento jurídico", "⚖️", "Abogado" });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios contables y fiscales", "📊", "Contador" });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de salud y atención médica", "🩺", "Médico" });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de salud mental y terapia", "🧠", "Psicólogo" });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de salud dental", "🦷", "Dentista" });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de salud animal", "🐕", "Veterinario" });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Diseño arquitectónico y planificación", "🏗️", "Arquitecto" });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de ingeniería y consultoría técnica", "⚙️", "Ingeniero" });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de diseño visual y comunicación", "🎨", "Diseñador Gráfico" });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de traducción e interpretación", "🌐", "Traductor" });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de coaching y desarrollo personal", "🎯", "Coach" });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de nutrición y alimentación saludable", "🥗", "Nutricionista" });
        }
    }
}
