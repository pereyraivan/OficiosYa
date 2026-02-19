using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OficiosYa.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChanges_20251215 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DescuentoTrimestral",
                table: "PlanesSuscripcion",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DescuentoSemestral",
                table: "PlanesSuscripcion",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DescuentoAnual",
                table: "PlanesSuscripcion",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Instalaci?n y reparaci?n de sistemas de plomer?a", "Construcci?n", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Instalaciones y reparaciones el?ctricas", "Construcci?n", "?" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Construcci?n y reparaci?n de estructuras", "Construcci?n", "??", "Alba?il" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Instalaci?n y reparaci?n de sistemas de gas", "Construcci?n", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 5,
                column: "Icono",
                value: "??");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Grupo", "Icono" },
                values: new object[] { "Construcci?n", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 7,
                column: "Icono",
                value: "??");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Grupo", "Icono" },
                values: new object[] { "Construcci?n", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Trabajos en metal y herrer?a", "Construcci?n", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Instalaci?n y reparaci?n de sistemas de climatizaci?n", "??", "T?cnico en Aire Acondicionado" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Reparaci?n de heladeras y freezers", "??", "T?cnico en Refrigeraci?n" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de limpieza dom?stica y comercial", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de salud y atenci?n m?dica", "??", "M?dico" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Icono", "Nombre" },
                values: new object[] { "??", "Psic?logo" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 15,
                column: "Icono",
                value: "??");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 16,
                column: "Icono",
                value: "??");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de nutrici?n y alimentaci?n saludable", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios legales y asesoramiento jur?dico", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 19,
                column: "Icono",
                value: "??");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Dise?o arquitect?nico y planificaci?n", "Dise?o y Arquitectura", "???" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Servicios de ingenier?a y consultor?a t?cnica", "Dise?o y Arquitectura", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Servicios de dise?o visual y comunicaci?n", "Dise?o y Arquitectura", "??", "Dise?ador Gr?fico" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de traducci?n e interpretaci?n", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 24,
                column: "Icono",
                value: "??");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Instalaci?n de construcci?n en seco (durlock)", "Construcci?n", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Trabajos en construcci?n en seco y durlock", "Construcci?n", "???", "Construcci?n en Seco" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Reparaci?n y construcci?n de techos", "Construcci?n", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Grupo", "Icono" },
                values: new object[] { "Construcci?n", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Reparaci?n y tapizado de muebles", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Trabajos en m?rmol y granito", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Instalaci?n y reparaci?n de vidrios", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Reparaci?n y restauraci?n de colchones", "???" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de fonoaudiolog?a y terapia del lenguaje", "???", "Fonoaudi?logo" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de kinesiolog?a y fisioterapia", "??", "Kinesi?logo" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de enfermer?a y cuidados de salud", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 36,
                column: "Icono",
                value: "??");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de salud visual y optometr?a", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Icono", "Nombre" },
                values: new object[] { "??", "Martillero P?blico" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 39,
                column: "Icono",
                value: "??");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 40,
                column: "Icono",
                value: "??");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Dise?o de interiores y decoraci?n", "Dise?o y Arquitectura", "??", "Dise?ador de Interiores" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Dise?o y desarrollo web", "Dise?o y Arquitectura", "??", "Dise?ador Web" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Servicios de fotograf?a profesional", "Dise?o y Arquitectura", "??", "Fot?grafo" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Clases particulares y tutor?as", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 45,
                column: "Icono",
                value: "??");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de masajes y relajaci?n", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "Grupo", "Icono" },
                values: new object[] { "Tecnolog?a e IT", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "Grupo", "Icono" },
                values: new object[] { "Tecnolog?a e IT", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Desarrollo de aplicaciones m?viles", "Tecnolog?a e IT", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "Grupo", "Icono" },
                values: new object[] { "Tecnolog?a e IT", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "An?lisis y dise?o de sistemas inform?ticos", "Tecnolog?a e IT", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Seguridad inform?tica y protecci?n de datos", "Tecnolog?a e IT", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Automatizaci?n e infraestructura de desarrollo", "Tecnolog?a e IT", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Administraci?n y mantenimiento de sistemas", "Tecnolog?a e IT", "???" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Administraci?n de bases de datos", "Tecnolog?a e IT", "???" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Dise?o arquitect?nico de software y sistemas", "Tecnolog?a e IT", "???" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Gesti?n de productos tecnol?gicos", "Tecnolog?a e IT", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Gesti?n ?gil de proyectos de software", "Tecnolog?a e IT", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "An?lisis de datos y ciencia de datos", "Tecnolog?a e IT", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Ingenier?a y administraci?n en la nube", "Tecnolog?a e IT", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Dise?o de experiencia e interfaz de usuario", "Tecnolog?a e IT", "??" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Soporte t?cnico y helpdesk", "Tecnolog?a e IT", "???", "Soporte T?cnico IT" });

            migrationBuilder.UpdateData(
                table: "Oficios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Reparaci?n y mantenimiento de sistemas de plomer?a", "??", "Plomer?a" });

            migrationBuilder.UpdateData(
                table: "Oficios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Instalaciones y reparaciones el?ctricas", "?" });

            migrationBuilder.UpdateData(
                table: "Oficios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Icono", "Nombre" },
                values: new object[] { "??", "Carpinter?a" });

            migrationBuilder.UpdateData(
                table: "Oficios",
                keyColumn: "Id",
                keyValue: 4,
                column: "Icono",
                value: "??");

            migrationBuilder.UpdateData(
                table: "Oficios",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Icono", "Nombre" },
                values: new object[] { "??", "Jardiner?a" });

            migrationBuilder.UpdateData(
                table: "Oficios",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de limpieza dom?stica y comercial", "??" });

            migrationBuilder.UpdateData(
                table: "PlanesSuscripcion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descripcion",
                value: "Plan b?sico para profesionales y oficiantes");

            migrationBuilder.UpdateData(
                table: "PlanesSuscripcion",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descripcion",
                value: "Plan premium con m?s visibilidad y funciones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DescuentoTrimestral",
                table: "PlanesSuscripcion",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DescuentoSemestral",
                table: "PlanesSuscripcion",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DescuentoAnual",
                table: "PlanesSuscripcion",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Instalación y reparación de sistemas de plomería", "Construcción", "🔧" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Instalaciones y reparaciones eléctricas", "Construcción", "⚡" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Construcción y reparación de estructuras", "Construcción", "🧱", "Albañil" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Instalación y reparación de sistemas de gas", "Construcción", "🔥" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 5,
                column: "Icono",
                value: "🌿");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Grupo", "Icono" },
                values: new object[] { "Construcción", "🎨" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 7,
                column: "Icono",
                value: "🔒");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Grupo", "Icono" },
                values: new object[] { "Construcción", "🪚" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Trabajos en metal y herrería", "Construcción", "⚒️" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Instalación y reparación de sistemas de climatización", "❄️", "Técnico en Aire Acondicionado" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Reparación de heladeras y freezers", "🧊", "Técnico en Refrigeración" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de limpieza doméstica y comercial", "🧹" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de salud y atención médica", "🩺", "Médico" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Icono", "Nombre" },
                values: new object[] { "🧠", "Psicólogo" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 15,
                column: "Icono",
                value: "🦷");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 16,
                column: "Icono",
                value: "🐕");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de nutrición y alimentación saludable", "🥗" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios legales y asesoramiento jurídico", "⚖️" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 19,
                column: "Icono",
                value: "📊");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Diseño arquitectónico y planificación", "Diseño y Arquitectura", "🏗️" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Servicios de ingeniería y consultoría técnica", "Diseño y Arquitectura", "⚙️" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Servicios de diseño visual y comunicación", "Diseño y Arquitectura", "🎨", "Diseñador Gráfico" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de traducción e interpretación", "🌐" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 24,
                column: "Icono",
                value: "🎯");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Instalación de construcción en seco (durlock)", "Construcción", "🧱" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Trabajos en construcción en seco y durlock", "Construcción", "🏗️", "Construcción en Seco" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Reparación y construcción de techos", "Construcción", "🏠" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Grupo", "Icono" },
                values: new object[] { "Construcción", "🔥" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Reparación y tapizado de muebles", "🪑" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Trabajos en mármol y granito", "🪨" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Instalación y reparación de vidrios", "🪟" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Reparación y restauración de colchones", "🛏️" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de fonoaudiología y terapia del lenguaje", "🗣️", "Fonoaudiólogo" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Servicios de kinesiología y fisioterapia", "💪", "Kinesiólogo" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de enfermería y cuidados de salud", "🏥" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 36,
                column: "Icono",
                value: "🤲");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de salud visual y optometría", "👓" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Icono", "Nombre" },
                values: new object[] { "🔨", "Martillero Público" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 39,
                column: "Icono",
                value: "📝");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 40,
                column: "Icono",
                value: "💰");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Diseño de interiores y decoración", "Diseño y Arquitectura", "🪑", "Diseñador de Interiores" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Diseño y desarrollo web", "Diseño y Arquitectura", "💻", "Diseñador Web" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Servicios de fotografía profesional", "Diseño y Arquitectura", "📷", "Fotógrafo" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Clases particulares y tutorías", "📚" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 45,
                column: "Icono",
                value: "💪");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de masajes y relajación", "👐" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "Grupo", "Icono" },
                values: new object[] { "Tecnología e IT", "💻" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "Grupo", "Icono" },
                values: new object[] { "Tecnología e IT", "🌐" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Desarrollo de aplicaciones móviles", "Tecnología e IT", "📱" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "Grupo", "Icono" },
                values: new object[] { "Tecnología e IT", "🔍" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Análisis y diseño de sistemas informáticos", "Tecnología e IT", "📊" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Seguridad informática y protección de datos", "Tecnología e IT", "🔒" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Automatización e infraestructura de desarrollo", "Tecnología e IT", "⚙️" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Administración y mantenimiento de sistemas", "Tecnología e IT", "🖥️" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Administración de bases de datos", "Tecnología e IT", "🗄️" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Diseño arquitectónico de software y sistemas", "Tecnología e IT", "🏛️" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Gestión de productos tecnológicos", "Tecnología e IT", "📦" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Gestión ágil de proyectos de software", "Tecnología e IT", "🎯" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Análisis de datos y ciencia de datos", "Tecnología e IT", "📈" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Ingeniería y administración en la nube", "Tecnología e IT", "☁️" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "Descripcion", "Grupo", "Icono" },
                values: new object[] { "Diseño de experiencia e interfaz de usuario", "Tecnología e IT", "🎨" });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "Descripcion", "Grupo", "Icono", "Nombre" },
                values: new object[] { "Soporte técnico y helpdesk", "Tecnología e IT", "🛠️", "Soporte Técnico IT" });

            migrationBuilder.UpdateData(
                table: "Oficios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Descripcion", "Icono", "Nombre" },
                values: new object[] { "Reparación y mantenimiento de sistemas de plomería", "🔧", "Plomería" });

            migrationBuilder.UpdateData(
                table: "Oficios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Instalaciones y reparaciones eléctricas", "⚡" });

            migrationBuilder.UpdateData(
                table: "Oficios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Icono", "Nombre" },
                values: new object[] { "🪚", "Carpintería" });

            migrationBuilder.UpdateData(
                table: "Oficios",
                keyColumn: "Id",
                keyValue: 4,
                column: "Icono",
                value: "🎨");

            migrationBuilder.UpdateData(
                table: "Oficios",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Icono", "Nombre" },
                values: new object[] { "🌱", "Jardinería" });

            migrationBuilder.UpdateData(
                table: "Oficios",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Descripcion", "Icono" },
                values: new object[] { "Servicios de limpieza doméstica y comercial", "🧹" });

            migrationBuilder.UpdateData(
                table: "PlanesSuscripcion",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descripcion",
                value: "Plan básico para profesionales y oficiantes");

            migrationBuilder.UpdateData(
                table: "PlanesSuscripcion",
                keyColumn: "Id",
                keyValue: 2,
                column: "Descripcion",
                value: "Plan premium con más visibilidad y funciones");
        }
    }
}
