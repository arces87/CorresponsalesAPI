using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCB_WebApi.WebApi.Migrations
{
    public partial class ModificandoRelacionDispositivoCorresponsal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DISPOSITIVO_PERSONA_CORRESPONSALID",
                schema: "CONSOLA",
                table: "DISPOSITIVO");

            migrationBuilder.AddForeignKey(
                name: "FK_DISPOSITIVO_CORRESPONSAL_CORRESPONSALID",
                schema: "CONSOLA",
                table: "DISPOSITIVO",
                column: "CORRESPONSALID",
                principalSchema: "ESTRUCTURAEMPRESARIAL",
                principalTable: "CORRESPONSAL",
                principalColumn: "PERSONAID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DISPOSITIVO_CORRESPONSAL_CORRESPONSALID",
                schema: "CONSOLA",
                table: "DISPOSITIVO");

            migrationBuilder.AddForeignKey(
                name: "FK_DISPOSITIVO_PERSONA_CORRESPONSALID",
                schema: "CONSOLA",
                table: "DISPOSITIVO",
                column: "CORRESPONSALID",
                principalSchema: "ESTRUCTURAEMPRESARIAL",
                principalTable: "PERSONA",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
