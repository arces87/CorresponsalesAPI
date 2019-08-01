using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class AgregandoCampoUsuarioActivo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UsuarioActivo",
                schema: "EstructuraEmpresarial",
                table: "Corresponsal",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsuarioActivo",
                schema: "EstructuraEmpresarial",
                table: "Corresponsal");
        }
    }
}
