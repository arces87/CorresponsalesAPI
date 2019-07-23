using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class ModificandoRelacionDispositivoCorresponsal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivo_Corresponsal_CorresponsalId",
                schema: "Consola",
                table: "Dispositivo");

            migrationBuilder.DropIndex(
                name: "IX_Dispositivo_CorresponsalId",
                schema: "Consola",
                table: "Dispositivo");

            migrationBuilder.DropColumn(
                name: "CorresponsalId",
                schema: "Consola",
                table: "Dispositivo");

            migrationBuilder.CreateTable(
                name: "DispositivoCorresponsal",
                schema: "Consola",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DispositivoId = table.Column<int>(nullable: true),
                    CorresponsalId = table.Column<int>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispositivoCorresponsal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispositivoCorresponsal_Corresponsal_CorresponsalId",
                        column: x => x.CorresponsalId,
                        principalSchema: "EstructuraEmpresarial",
                        principalTable: "Corresponsal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispositivoCorresponsal_Dispositivo_DispositivoId",
                        column: x => x.DispositivoId,
                        principalSchema: "Consola",
                        principalTable: "Dispositivo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DispositivoCorresponsal_CorresponsalId",
                schema: "Consola",
                table: "DispositivoCorresponsal",
                column: "CorresponsalId");

            migrationBuilder.CreateIndex(
                name: "IX_DispositivoCorresponsal_DispositivoId",
                schema: "Consola",
                table: "DispositivoCorresponsal",
                column: "DispositivoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispositivoCorresponsal",
                schema: "Consola");

            migrationBuilder.AddColumn<int>(
                name: "CorresponsalId",
                schema: "Consola",
                table: "Dispositivo",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivo_CorresponsalId",
                schema: "Consola",
                table: "Dispositivo",
                column: "CorresponsalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivo_Corresponsal_CorresponsalId",
                schema: "Consola",
                table: "Dispositivo",
                column: "CorresponsalId",
                principalSchema: "EstructuraEmpresarial",
                principalTable: "Corresponsal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
