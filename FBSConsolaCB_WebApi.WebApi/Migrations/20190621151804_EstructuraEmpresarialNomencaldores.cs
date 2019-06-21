using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCB_WebApi.WebApi.Migrations
{
    public partial class EstructuraEmpresarialNomencaldores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ESTRUCTURAEMPRESARIAL");

            migrationBuilder.EnsureSchema(
                name: "NOMENCLADOR");

            migrationBuilder.CreateTable(
                name: "AREATRABAJO",
                schema: "ESTRUCTURAEMPRESARIAL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(nullable: true),
                    DESCRIPCION = table.Column<string>(nullable: true),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AREATRABAJO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CARGO",
                schema: "ESTRUCTURAEMPRESARIAL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(nullable: true),
                    DESCRIPCION = table.Column<string>(nullable: true),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CARGO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EMPRESA",
                schema: "ESTRUCTURAEMPRESARIAL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(nullable: true),
                    DIRECCION = table.Column<string>(nullable: true),
                    LOGO = table.Column<string>(nullable: true),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    RUC = table.Column<string>(nullable: true),
                    ACTIVIDADECONOMICA = table.Column<string>(nullable: true),
                    PROVINCIA = table.Column<string>(nullable: true),
                    DEPARTAMENTO = table.Column<string>(nullable: true),
                    DISTRITO = table.Column<string>(nullable: true),
                    FECHAINICIOACTIVIDAD = table.Column<DateTime>(nullable: false),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMPRESA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TIPOCATALOGO",
                schema: "NOMENCLADOR",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(nullable: true),
                    DESCRIPCION = table.Column<string>(nullable: true),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TIPOCATALOGO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OFICINA",
                schema: "ESTRUCTURAEMPRESARIAL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(nullable: true),
                    DESCRIPCION = table.Column<string>(nullable: true),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    CIUDAD = table.Column<string>(nullable: true),
                    EMPRESAID = table.Column<int>(nullable: true),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OFICINA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OFICINA_EMPRESA_EMPRESAID",
                        column: x => x.EMPRESAID,
                        principalSchema: "ESTRUCTURAEMPRESARIAL",
                        principalTable: "EMPRESA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CATALOGO",
                schema: "NOMENCLADOR",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(nullable: true),
                    DESCRIPCION = table.Column<string>(nullable: true),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    TIPOCATALOGOID = table.Column<int>(nullable: true),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATALOGO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CATALOGO_TIPOCATALOGO_TIPOCATALOGOID",
                        column: x => x.TIPOCATALOGOID,
                        principalSchema: "NOMENCLADOR",
                        principalTable: "TIPOCATALOGO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CORRESPONSAL",
                schema: "ESTRUCTURAEMPRESARIAL",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PRIMERNOMBRE = table.Column<string>(nullable: true),
                    SEGUNDONOMBRE = table.Column<string>(nullable: true),
                    PRIMERAPELLIDO = table.Column<string>(nullable: true),
                    SEGUNDOAPELLIDO = table.Column<string>(nullable: true),
                    NOMBREUNIDO = table.Column<string>(nullable: true),
                    DIRECCION = table.Column<string>(nullable: true),
                    NUMEROIDENTIFICADOR = table.Column<int>(nullable: false),
                    IDENTIFICACION = table.Column<string>(nullable: true),
                    CORREOELECTRONICO = table.Column<string>(nullable: true),
                    TELEFONO = table.Column<string>(nullable: true),
                    FECHANACIMIENTO = table.Column<DateTime>(nullable: false),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    TIPOIDENTIFICACIONID = table.Column<int>(nullable: true),
                    CARGOID = table.Column<int>(nullable: true),
                    OFICINAID = table.Column<int>(nullable: true),
                    AREATRABAJOID = table.Column<int>(nullable: true),
                    USUARIOID = table.Column<string>(nullable: true),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORRESPONSAL", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CORRESPONSAL_AREATRABAJO_AREATRABAJOID",
                        column: x => x.AREATRABAJOID,
                        principalSchema: "ESTRUCTURAEMPRESARIAL",
                        principalTable: "AREATRABAJO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CORRESPONSAL_CARGO_CARGOID",
                        column: x => x.CARGOID,
                        principalSchema: "ESTRUCTURAEMPRESARIAL",
                        principalTable: "CARGO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CORRESPONSAL_OFICINA_OFICINAID",
                        column: x => x.OFICINAID,
                        principalSchema: "ESTRUCTURAEMPRESARIAL",
                        principalTable: "OFICINA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CORRESPONSAL_CATALOGO_TIPOIDENTIFICACIONID",
                        column: x => x.TIPOIDENTIFICACIONID,
                        principalSchema: "NOMENCLADOR",
                        principalTable: "CATALOGO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CORRESPONSAL_USUARIO_USUARIOID",
                        column: x => x.USUARIOID,
                        principalSchema: "SEGURIDAD",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CORRESPONSAL_AREATRABAJOID",
                schema: "ESTRUCTURAEMPRESARIAL",
                table: "CORRESPONSAL",
                column: "AREATRABAJOID");

            migrationBuilder.CreateIndex(
                name: "IX_CORRESPONSAL_CARGOID",
                schema: "ESTRUCTURAEMPRESARIAL",
                table: "CORRESPONSAL",
                column: "CARGOID");

            migrationBuilder.CreateIndex(
                name: "IX_CORRESPONSAL_OFICINAID",
                schema: "ESTRUCTURAEMPRESARIAL",
                table: "CORRESPONSAL",
                column: "OFICINAID");

            migrationBuilder.CreateIndex(
                name: "IX_CORRESPONSAL_TIPOIDENTIFICACIONID",
                schema: "ESTRUCTURAEMPRESARIAL",
                table: "CORRESPONSAL",
                column: "TIPOIDENTIFICACIONID");

            migrationBuilder.CreateIndex(
                name: "IX_CORRESPONSAL_USUARIOID",
                schema: "ESTRUCTURAEMPRESARIAL",
                table: "CORRESPONSAL",
                column: "USUARIOID");

            migrationBuilder.CreateIndex(
                name: "IX_OFICINA_EMPRESAID",
                schema: "ESTRUCTURAEMPRESARIAL",
                table: "OFICINA",
                column: "EMPRESAID");

            migrationBuilder.CreateIndex(
                name: "IX_CATALOGO_TIPOCATALOGOID",
                schema: "NOMENCLADOR",
                table: "CATALOGO",
                column: "TIPOCATALOGOID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CORRESPONSAL",
                schema: "ESTRUCTURAEMPRESARIAL");

            migrationBuilder.DropTable(
                name: "AREATRABAJO",
                schema: "ESTRUCTURAEMPRESARIAL");

            migrationBuilder.DropTable(
                name: "CARGO",
                schema: "ESTRUCTURAEMPRESARIAL");

            migrationBuilder.DropTable(
                name: "OFICINA",
                schema: "ESTRUCTURAEMPRESARIAL");

            migrationBuilder.DropTable(
                name: "CATALOGO",
                schema: "NOMENCLADOR");

            migrationBuilder.DropTable(
                name: "EMPRESA",
                schema: "ESTRUCTURAEMPRESARIAL");

            migrationBuilder.DropTable(
                name: "TIPOCATALOGO",
                schema: "NOMENCLADOR");
        }
    }
}
