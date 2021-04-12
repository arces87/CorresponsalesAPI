using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class Fix_TipoIdentificacionAgente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipoIdentificación",
                schema: "Corresponsales",
                table: "Agente",
                newName: "TipoIdentificacion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipoIdentificacion",
                schema: "Corresponsales",
                table: "Agente",
                newName: "TipoIdentificación");
        }
    }
}
