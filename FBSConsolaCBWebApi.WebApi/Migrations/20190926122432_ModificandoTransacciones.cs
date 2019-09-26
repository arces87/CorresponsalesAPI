using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class ModificandoTransacciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comisiones",
                schema: "Corresponsales",
                table: "Transaccion",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SaldoDisponible",
                schema: "Corresponsales",
                table: "Transaccion",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comisiones",
                schema: "Corresponsales",
                table: "Transaccion");

            migrationBuilder.DropColumn(
                name: "SaldoDisponible",
                schema: "Corresponsales",
                table: "Transaccion");
        }
    }
}
