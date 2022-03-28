using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class TransaccionRetiro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransaccionRetiro",
                schema: "Corresponsales",
                columns: table => new
                {
                    IdTransaccion = table.Column<Guid>(nullable: false),
                    ValorCaja = table.Column<double>(nullable: false),
                    FondoNegocio = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransaccionRetiro", x => x.IdTransaccion);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransaccionRetiro",
                schema: "Corresponsales");
        }
    }
}
