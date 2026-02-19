using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OficiosYa.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InsertMissingCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insertar todas las categorías faltantes usando SQL directo
            migrationBuilder.Sql(@"
                -- Oficiantes adicionales
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
                
                -- Profesionales de Salud adicionales
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
                
                -- Profesionales Legal y Financiero adicionales
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Martillero Público' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Martillero Público', N'Servicios de remates y subastas', N'Profesional', N'Legal y Financiero', N'🔨', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Escribano' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Escribano', N'Servicios notariales y certificaciones', N'Profesional', N'Legal y Financiero', N'📝', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Asesor Financiero' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Asesor Financiero', N'Asesoramiento financiero y inversiones', N'Profesional', N'Legal y Financiero', N'💰', 1, '2024-01-01');
                
                -- Profesionales Diseño y Arquitectura adicionales
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Diseñador de Interiores' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Diseñador de Interiores', N'Diseño de interiores y decoración', N'Profesional', N'Diseño y Arquitectura', N'🪑', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Diseñador Web' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Diseñador Web', N'Diseño y desarrollo web', N'Profesional', N'Diseño y Arquitectura', N'💻', 1, '2024-01-01');
                
                IF NOT EXISTS (SELECT 1 FROM [CategoriaServicios] WHERE [Nombre] = N'Fotógrafo' AND [Tipo] = N'Profesional')
                    INSERT INTO [CategoriaServicios] ([Nombre], [Descripcion], [Tipo], [Grupo], [Icono], [Activo], [FechaCreacion])
                    VALUES (N'Fotógrafo', N'Servicios de fotografía profesional', N'Profesional', N'Diseño y Arquitectura', N'📷', 1, '2024-01-01');
                
                -- Profesionales Otros Servicios adicionales
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No hacer nada en el Down, ya que estas categorías pueden estar en uso
        }
    }
}
