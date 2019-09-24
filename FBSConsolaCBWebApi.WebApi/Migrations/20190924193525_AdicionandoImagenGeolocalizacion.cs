using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class AdicionandoImagenGeolocalizacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImagenGeolocalizacion",
                schema: "Canales",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DireccionImagen = table.Column<string>(nullable: true),
                    GeolocalizacionId = table.Column<Guid>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagenGeolocalizacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagenGeolocalizacion_Geolocalizacion_GeolocalizacionId",
                        column: x => x.GeolocalizacionId,
                        principalSchema: "Canales",
                        principalTable: "Geolocalizacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImagenGeolocalizacion_GeolocalizacionId",
                schema: "Canales",
                table: "ImagenGeolocalizacion",
                column: "GeolocalizacionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagenGeolocalizacion",
                schema: "Canales");
        }
    }
}
