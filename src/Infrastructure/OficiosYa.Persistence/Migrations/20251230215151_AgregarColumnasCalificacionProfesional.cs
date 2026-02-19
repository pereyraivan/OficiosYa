using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OficiosYa.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregarColumnasCalificacionProfesional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Agregar columna CalificacionPromedio si no existe
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Profesionales]') AND name = 'CalificacionPromedio')
                BEGIN
                    ALTER TABLE [Profesionales]
                    ADD [CalificacionPromedio] decimal(3,2) NOT NULL DEFAULT 0;
                END
            ");

            // Agregar columna TotalResenas si no existe
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Profesionales]') AND name = 'TotalResenas')
                BEGIN
                    ALTER TABLE [Profesionales]
                    ADD [TotalResenas] int NOT NULL DEFAULT 0;
                END
            ");

            // Agregar columna Disponible si no existe
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Profesionales]') AND name = 'Disponible')
                BEGIN
                    ALTER TABLE [Profesionales]
                    ADD [Disponible] bit NOT NULL DEFAULT 1;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar columnas si existen
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Profesionales]') AND name = 'Disponible')
                BEGIN
                    ALTER TABLE [Profesionales] DROP COLUMN [Disponible];
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Profesionales]') AND name = 'TotalResenas')
                BEGIN
                    ALTER TABLE [Profesionales] DROP COLUMN [TotalResenas];
                END
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Profesionales]') AND name = 'CalificacionPromedio')
                BEGIN
                    ALTER TABLE [Profesionales] DROP COLUMN [CalificacionPromedio];
                END
            ");
        }
    }
}
