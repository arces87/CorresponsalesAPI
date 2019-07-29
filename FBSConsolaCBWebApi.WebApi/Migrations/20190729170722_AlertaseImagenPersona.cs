using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class AlertaseImagenPersona : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                schema: "EstructuraEmpresarial",
                table: "Persona",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Alerta",
                schema: "Consola",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdConversacion = table.Column<int>(nullable: false),
                    DestinatarioId = table.Column<int>(nullable: true),
                    RemitenteId = table.Column<int>(nullable: true),
                    Asunto = table.Column<string>(nullable: true),
                    Mensaje = table.Column<string>(nullable: true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Estado = table.Column<int>(nullable: false),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    CategoriaId = table.Column<int>(nullable: true),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerta_Catalogo_CategoriaId",
                        column: x => x.CategoriaId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alerta_Persona_DestinatarioId",
                        column: x => x.DestinatarioId,
                        principalSchema: "EstructuraEmpresarial",
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alerta_Persona_RemitenteId",
                        column: x => x.RemitenteId,
                        principalSchema: "EstructuraEmpresarial",
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerta_CategoriaId",
                schema: "Consola",
                table: "Alerta",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerta_DestinatarioId",
                schema: "Consola",
                table: "Alerta",
                column: "DestinatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerta_RemitenteId",
                schema: "Consola",
                table: "Alerta",
                column: "RemitenteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerta",
                schema: "Consola");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                schema: "EstructuraEmpresarial",
                table: "Persona");
        }
    }
}
