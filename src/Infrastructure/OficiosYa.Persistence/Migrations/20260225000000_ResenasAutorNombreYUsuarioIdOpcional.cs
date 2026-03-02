using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OficiosYa.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ResenasAutorNombreYUsuarioIdOpcional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Resenas]') AND name = 'AutorNombre')
                BEGIN
                    ALTER TABLE [Resenas] ADD [AutorNombre] nvarchar(100) NULL;
                END
            ");
            migrationBuilder.Sql(@"
                ALTER TABLE [Resenas] ALTER COLUMN [UsuarioId] int NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Resenas]') AND name = 'AutorNombre')
                BEGIN
                    ALTER TABLE [Resenas] DROP COLUMN [AutorNombre];
                END
            ");
            migrationBuilder.Sql(@"
                ALTER TABLE [Resenas] ALTER COLUMN [UsuarioId] int NOT NULL;
            ");
        }
    }
}
