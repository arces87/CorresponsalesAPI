using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class ModificandoTransaccionIdCanal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CanalId",
                schema: "Corresponsales",
                table: "Transaccion",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CanalId",
                schema: "Corresponsales",
                table: "Transaccion",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
