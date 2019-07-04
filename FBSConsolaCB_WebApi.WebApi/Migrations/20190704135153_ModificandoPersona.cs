using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCB_WebApi.WebApi.Migrations
{
    public partial class ModificandoPersona : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direccion",
                schema: "EstructuraEmpresarial",
                table: "Corresponsal");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                schema: "EstructuraEmpresarial",
                table: "Corresponsal");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                schema: "EstructuraEmpresarial",
                table: "Persona",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                schema: "EstructuraEmpresarial",
                table: "Persona",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direccion",
                schema: "EstructuraEmpresarial",
                table: "Persona");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                schema: "EstructuraEmpresarial",
                table: "Persona");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                schema: "EstructuraEmpresarial",
                table: "Corresponsal",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                schema: "EstructuraEmpresarial",
                table: "Corresponsal",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
