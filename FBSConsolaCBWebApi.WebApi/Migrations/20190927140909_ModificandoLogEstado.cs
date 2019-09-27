using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class ModificandoLogEstado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EstadoId",
                schema: "Canales",
                table: "Log",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Log_EstadoId",
                schema: "Canales",
                table: "Log",
                column: "EstadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Log_Catalogo_EstadoId",
                schema: "Canales",
                table: "Log",
                column: "EstadoId",
                principalSchema: "Nomenclador",
                principalTable: "Catalogo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_Catalogo_EstadoId",
                schema: "Canales",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Log_EstadoId",
                schema: "Canales",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "EstadoId",
                schema: "Canales",
                table: "Log");
        }
    }
}
