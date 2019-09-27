using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class ModificandoTipoDatoIdRelacionadoLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RelacionadoId",
                schema: "Canales",
                table: "Log",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RelacionadoId",
                schema: "Canales",
                table: "Log",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
