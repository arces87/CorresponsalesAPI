using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class DatosClienteTransacciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentificacionCliente",
                schema: "Corresponsales",
                table: "Transaccion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreCliente",
                schema: "Corresponsales",
                table: "Transaccion",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentificacionCliente",
                schema: "Corresponsales",
                table: "Transaccion");

            migrationBuilder.DropColumn(
                name: "NombreCliente",
                schema: "Corresponsales",
                table: "Transaccion");
        }
    }
}
