using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCB_WebApi.WebApi.Migrations
{
    public partial class AgregandoDispositivo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CONSOLA");

            migrationBuilder.CreateTable(
                name: "DISPOSITIVO",
                schema: "CONSOLA",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(nullable: true),
                    IMEI = table.Column<string>(nullable: true),
                    MAC = table.Column<string>(nullable: true),
                    NUMEROSERIE = table.Column<string>(nullable: true),
                    TIPODISPOSITIVOID = table.Column<int>(nullable: true),
                    CORRESPONSALID = table.Column<int>(nullable: true),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISPOSITIVO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DISPOSITIVO_CORRESPONSAL_CORRESPONSALID",
                        column: x => x.CORRESPONSALID,
                        principalSchema: "ESTRUCTURAEMPRESARIAL",
                        principalTable: "CORRESPONSAL",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DISPOSITIVO_CATALOGO_TIPODISPOSITIVOID",
                        column: x => x.TIPODISPOSITIVOID,
                        principalSchema: "NOMENCLADOR",
                        principalTable: "CATALOGO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DISPOSITIVO_CORRESPONSALID",
                schema: "CONSOLA",
                table: "DISPOSITIVO",
                column: "CORRESPONSALID");

            migrationBuilder.CreateIndex(
                name: "IX_DISPOSITIVO_TIPODISPOSITIVOID",
                schema: "CONSOLA",
                table: "DISPOSITIVO",
                column: "TIPODISPOSITIVOID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DISPOSITIVO",
                schema: "CONSOLA");
        }
    }
}
