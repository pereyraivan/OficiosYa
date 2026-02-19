using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OficiosYa.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEsAdminUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add column only if it does not exist to make migration idempotent when applied to existing DBs
            migrationBuilder.Sql(@"IF COL_LENGTH('dbo.Usuarios', 'EsAdmin') IS NULL
BEGIN
    ALTER TABLE dbo.Usuarios ADD EsAdmin bit NOT NULL CONSTRAINT DF_Usuarios_EsAdmin DEFAULT(0);
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF COL_LENGTH('dbo.Usuarios', 'EsAdmin') IS NOT NULL
BEGIN
    ALTER TABLE dbo.Usuarios DROP CONSTRAINT IF EXISTS DF_Usuarios_EsAdmin;
    ALTER TABLE dbo.Usuarios DROP COLUMN EsAdmin;
END");
        }
    }
}
