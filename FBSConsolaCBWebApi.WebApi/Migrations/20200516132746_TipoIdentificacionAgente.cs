using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class TipoIdentificacionAgente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoIdentificación",
                schema: "Corresponsales",
                table: "Agente",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoIdentificación",
                schema: "Corresponsales",
                table: "Agente");
        }
    }
}
