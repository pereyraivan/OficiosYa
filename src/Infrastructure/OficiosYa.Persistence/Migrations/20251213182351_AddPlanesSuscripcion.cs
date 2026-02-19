using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OficiosYa.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanesSuscripcion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CategoriaServicios_Tipo_Nombre",
                table: "CategoriaServicios");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "CategoriaServicios");

            migrationBuilder.AddColumn<string>(
                name: "CodigoPago",
                table: "Suscripciones",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPago",
                table: "Suscripciones",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetodoPago",
                table: "Suscripciones",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PagoConfirmado",
                table: "Suscripciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TipoServicioId",
                table: "CategoriaServicios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PlanesSuscripcion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrecioMensual = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PrecioTrimestral = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PrecioSemestral = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PrecioAnual = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DescuentoTrimestral = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DescuentoSemestral = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DescuentoAnual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesSuscripcion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoServicios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoServicios", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 1,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 2,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 3,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 4,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 5,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 6,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 7,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 8,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 9,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 10,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 11,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 12,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 13,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 14,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 15,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 16,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 17,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 18,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 19,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 20,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 21,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 22,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 23,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 24,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 25,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 26,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 27,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 28,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 29,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 30,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 31,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 32,
                column: "TipoServicioId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 33,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 34,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 35,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 36,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 37,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 38,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 39,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 40,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 41,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 42,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 43,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 44,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 45,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 46,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 47,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 48,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 49,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 50,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 51,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 52,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 53,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 54,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 55,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 56,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 57,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 58,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 59,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 60,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 61,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 62,
                column: "TipoServicioId",
                value: 2);

            migrationBuilder.InsertData(
                table: "PlanesSuscripcion",
                columns: new[] { "Id", "Activo", "Descripcion", "DescuentoAnual", "DescuentoSemestral", "DescuentoTrimestral", "FechaCreacion", "Nombre", "PrecioAnual", "PrecioMensual", "PrecioSemestral", "PrecioTrimestral" },
                values: new object[,]
                {
                    { 1, true, "Plan básico para profesionales y oficiantes", 30m, 20m, 10m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Basica", 42000m, 5000m, 24000m, 13500m },
                    { 2, true, "Plan premium con más visibilidad y funciones", 30m, 20m, 10m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Premium", 67200m, 8000m, 38400m, 21600m },
                    { 3, true, "Plan profesional con todas las funciones", 30m, 20m, 10m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pro", 100800m, 12000m, 57600m, 32400m }
                });

            migrationBuilder.InsertData(
                table: "TipoServicios",
                columns: new[] { "Id", "Activo", "Descripcion", "FechaCreacion", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Servicios de oficios y trabajos manuales", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oficiante" },
                    { 2, true, "Servicios profesionales y especializados", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Profesional" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaServicios_TipoServicioId_Nombre",
                table: "CategoriaServicios",
                columns: new[] { "TipoServicioId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanesSuscripcion_Nombre",
                table: "PlanesSuscripcion",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoServicios_Nombre",
                table: "TipoServicios",
                column: "Nombre",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriaServicios_TipoServicios_TipoServicioId",
                table: "CategoriaServicios",
                column: "TipoServicioId",
                principalTable: "TipoServicios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriaServicios_TipoServicios_TipoServicioId",
                table: "CategoriaServicios");

            migrationBuilder.DropTable(
                name: "PlanesSuscripcion");

            migrationBuilder.DropTable(
                name: "TipoServicios");

            migrationBuilder.DropIndex(
                name: "IX_CategoriaServicios_TipoServicioId_Nombre",
                table: "CategoriaServicios");

            migrationBuilder.DropColumn(
                name: "CodigoPago",
                table: "Suscripciones");

            migrationBuilder.DropColumn(
                name: "FechaPago",
                table: "Suscripciones");

            migrationBuilder.DropColumn(
                name: "MetodoPago",
                table: "Suscripciones");

            migrationBuilder.DropColumn(
                name: "PagoConfirmado",
                table: "Suscripciones");

            migrationBuilder.DropColumn(
                name: "TipoServicioId",
                table: "CategoriaServicios");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "CategoriaServicios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 3,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 4,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 5,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 6,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 7,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 8,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 9,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 10,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 11,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 12,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 13,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 14,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 15,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 16,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 17,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 18,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 19,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 20,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 21,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 22,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 23,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 24,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 25,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 26,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 27,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 28,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 29,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 30,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 31,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 32,
                column: "Tipo",
                value: "Oficiante");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 33,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 34,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 35,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 36,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 37,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 38,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 39,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 40,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 41,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 42,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 43,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 44,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 45,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 46,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 47,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 48,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 49,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 50,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 51,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 52,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 53,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 54,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 55,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 56,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 57,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 58,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 59,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 60,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 61,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.UpdateData(
                table: "CategoriaServicios",
                keyColumn: "Id",
                keyValue: 62,
                column: "Tipo",
                value: "Profesional");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaServicios_Tipo_Nombre",
                table: "CategoriaServicios",
                columns: new[] { "Tipo", "Nombre" },
                unique: true);
        }
    }
}
