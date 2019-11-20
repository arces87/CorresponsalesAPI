using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class ModificandoTransaccionCuenta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumeroCuenta",
                schema: "Corresponsales",
                table: "Transaccion",
                newName: "SecuencialCuenta");

            migrationBuilder.AddColumn<string>(
                name: "SecuencialCuenta",
                schema: "Corresponsales",
                table: "Cuenta",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecuencialCuenta",
                schema: "Corresponsales",
                table: "Cuenta");

            migrationBuilder.RenameColumn(
                name: "SecuencialCuenta",
                schema: "Corresponsales",
                table: "Transaccion",
                newName: "NumeroCuenta");
        }
    }
}
