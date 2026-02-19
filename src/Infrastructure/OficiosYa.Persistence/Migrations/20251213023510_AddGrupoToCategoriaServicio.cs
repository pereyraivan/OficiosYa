using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OficiosYa.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGrupoToCategoriaServicio : Migration
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

            // Actualizar grupos basándose en el nombre, no en el ID
            migrationBuilder.Sql(@"
                UPDATE [CategoriaServicios] SET [Grupo] = N'Construcción' WHERE [Nombre] = N'Plomero' AND [Tipo] = N'Oficiante';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Construcción' WHERE [Nombre] = N'Electricista' AND [Tipo] = N'Oficiante';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Construcción' WHERE [Nombre] = N'Albañil' AND [Tipo] = N'Oficiante';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Construcción' WHERE [Nombre] = N'Gasista' AND [Tipo] = N'Oficiante';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Hogar y Mantenimiento' WHERE [Nombre] = N'Jardinero' AND [Tipo] = N'Oficiante';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Construcción' WHERE [Nombre] = N'Pintor' AND [Tipo] = N'Oficiante';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Hogar y Mantenimiento' WHERE [Nombre] = N'Cerrajero' AND [Tipo] = N'Oficiante';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Construcción' WHERE [Nombre] = N'Carpintero' AND [Tipo] = N'Oficiante';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Construcción' WHERE [Nombre] = N'Herrero' AND [Tipo] = N'Oficiante';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Hogar y Mantenimiento' WHERE [Nombre] = N'Técnico en Aire Acondicionado' AND [Tipo] = N'Oficiante';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Hogar y Mantenimiento' WHERE [Nombre] = N'Técnico en Refrigeración' AND [Tipo] = N'Oficiante';
                UPDATE [CategoriaServicios] SET [Grupo] = N'Hogar y Mantenimiento' WHERE [Nombre] = N'Limpieza' AND [Tipo] = N'Oficiante';
                
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

            migrationBuilder.InsertData(
                table: "CategoriaServicios",
                columns: new[] { "Id", "Activo", "Descripcion", "FechaCreacion", "Grupo", "Icono", "Nombre", "Tipo" },
                values: new object[,]
                {
                    { 25, true, "Instalación de construcción en seco (durlock)", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Construcción", "🧱", "Durlockero", "Oficiante" },
                    { 26, true, "Trabajos en construcción en seco y durlock", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Construcción", "🏗️", "Construcción en Seco", "Oficiante" },
                    { 27, true, "Reparación y construcción de techos", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Construcción", "🏠", "Techista", "Oficiante" },
                    { 28, true, "Trabajos de soldadura y metalurgia", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Construcción", "🔥", "Soldador", "Oficiante" },
                    { 29, true, "Reparación y tapizado de muebles", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hogar y Mantenimiento", "🪑", "Tapicero", "Oficiante" },
                    { 30, true, "Trabajos en mármol y granito", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hogar y Mantenimiento", "🪨", "Marmolista", "Oficiante" },
                    { 31, true, "Instalación y reparación de vidrios", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hogar y Mantenimiento", "🪟", "Vidriero", "Oficiante" },
                    { 32, true, "Reparación y restauración de colchones", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hogar y Mantenimiento", "🛏️", "Colchonero", "Oficiante" },
                    { 33, true, "Servicios de fonoaudiología y terapia del lenguaje", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Salud", "🗣️", "Fonoaudiólogo", "Profesional" },
                    { 34, true, "Servicios de kinesiología y fisioterapia", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Salud", "💪", "Kinesiólogo", "Profesional" },
                    { 35, true, "Servicios de enfermería y cuidados de salud", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Salud", "🏥", "Enfermero", "Profesional" },
                    { 36, true, "Servicios de terapia ocupacional", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Salud", "🤲", "Terapeuta Ocupacional", "Profesional" },
                    { 37, true, "Servicios de salud visual y optometría", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Salud", "👓", "Optometrista", "Profesional" },
                    { 38, true, "Servicios de remates y subastas", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Legal y Financiero", "🔨", "Martillero Público", "Profesional" },
                    { 39, true, "Servicios notariales y certificaciones", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Legal y Financiero", "📝", "Escribano", "Profesional" },
                    { 40, true, "Asesoramiento financiero y inversiones", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Legal y Financiero", "💰", "Asesor Financiero", "Profesional" },
                    { 41, true, "Diseño de interiores y decoración", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diseño y Arquitectura", "🪑", "Diseñador de Interiores", "Profesional" },
                    { 42, true, "Diseño y desarrollo web", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diseño y Arquitectura", "💻", "Diseñador Web", "Profesional" },
                    { 43, true, "Servicios de fotografía profesional", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diseño y Arquitectura", "📷", "Fotógrafo", "Profesional" },
                    { 44, true, "Clases particulares y tutorías", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Otros Servicios", "📚", "Profesor Particular", "Profesional" },
                    { 45, true, "Entrenamiento personal y fitness", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Otros Servicios", "💪", "Instructor de Fitness", "Profesional" },
                    { 46, true, "Servicios de masajes y relajación", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Otros Servicios", "👐", "Masajista", "Profesional" }
                });

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
