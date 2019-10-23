using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class SaldoCuentaTransaccion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SaldoCuenta",
                schema: "Corresponsales",
                table: "Transaccion",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaldoCuenta",
                schema: "Corresponsales",
                table: "Transaccion");
        }
    }
}
