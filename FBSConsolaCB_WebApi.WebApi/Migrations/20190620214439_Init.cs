using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCB_WebApi.WebApi.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SEGURIDAD");

            migrationBuilder.CreateTable(
                name: "MENU",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(nullable: true),
                    ORDEN = table.Column<int>(nullable: false),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ICONO = table.Column<string>(nullable: true),
                    RUTA = table.Column<string>(nullable: true),
                    MENUPADREID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MENU_MENU_MENUPADREID",
                        column: x => x.MENUPADREID,
                        principalSchema: "SEGURIDAD",
                        principalTable: "MENU",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PERMISO",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NOMBRE = table.Column<string>(nullable: true),
                    DESCRIPCION = table.Column<string>(nullable: true),
                    IDENTIFICADOR = table.Column<string>(nullable: true),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    URLENDPOINT = table.Column<string>(nullable: true),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERMISO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ROL",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    NOMBRE = table.Column<string>(maxLength: 256, nullable: true),
                    NOMBRENORMALIZADO = table.Column<string>(maxLength: 256, nullable: true),
                    CONCURRENCIA = table.Column<string>(nullable: true),
                    TIPO = table.Column<bool>(nullable: false),
                    ESTAACTIVO = table.Column<bool>(nullable: false, defaultValue: true),
                    DESCRIPCION = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROL", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    USUARIO = table.Column<string>(maxLength: 256, nullable: true),
                    NOMBREUSUARIONORMALIZADO = table.Column<string>(maxLength: 256, nullable: true),
                    CORREO = table.Column<string>(maxLength: 256, nullable: true),
                    CORREONORMALIZADO = table.Column<string>(maxLength: 256, nullable: true),
                    CORREOCONFIRMADO = table.Column<bool>(nullable: false),
                    CONTRASENNA = table.Column<string>(nullable: true),
                    MARCASEGURIDAD = table.Column<string>(nullable: true),
                    CONCURRENCIA = table.Column<string>(nullable: true),
                    TELEFONO = table.Column<string>(nullable: true),
                    TELEFONOCONFIRMADO = table.Column<bool>(nullable: false),
                    DOBLEVERIFICACION = table.Column<bool>(nullable: false),
                    FINBLOQUEO = table.Column<DateTimeOffset>(nullable: true),
                    BLOQUEOACTIVO = table.Column<bool>(nullable: false),
                    ACCESOSFALLIDOS = table.Column<int>(nullable: false),
                    ESTAACTIVO = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ROLMENU",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ROLID = table.Column<string>(nullable: true),
                    MENUID = table.Column<int>(nullable: true),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLMENU", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ROLMENU_MENU_MENUID",
                        column: x => x.MENUID,
                        principalSchema: "SEGURIDAD",
                        principalTable: "MENU",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ROLMENU_ROL_ROLID",
                        column: x => x.ROLID,
                        principalSchema: "SEGURIDAD",
                        principalTable: "ROL",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ROLPERMISO",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ROLID = table.Column<string>(nullable: false),
                    TIPO = table.Column<string>(nullable: true),
                    VALOR = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLPERMISO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ROLPERMISO_ROL_ROLID",
                        column: x => x.ROLID,
                        principalSchema: "SEGURIDAD",
                        principalTable: "ROL",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AUTENTICACIONUSUARIO",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    PROVEEDOR = table.Column<string>(nullable: false),
                    LLAVEPROVEEDOR = table.Column<string>(nullable: false),
                    NOMBREPROVEEDOR = table.Column<string>(nullable: true),
                    USUARIOID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUTENTICACIONUSUARIO", x => new { x.PROVEEDOR, x.LLAVEPROVEEDOR });
                    table.ForeignKey(
                        name: "FK_AUTENTICACIONUSUARIO_USUARIO_USUARIOID",
                        column: x => x.USUARIOID,
                        principalSchema: "SEGURIDAD",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USUARIOPERMISO",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    USUARIOID = table.Column<string>(nullable: false),
                    TIPO = table.Column<string>(nullable: true),
                    VALOR = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOPERMISO", x => x.ID);
                    table.ForeignKey(
                        name: "FK_USUARIOPERMISO_USUARIO_USUARIOID",
                        column: x => x.USUARIOID,
                        principalSchema: "SEGURIDAD",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USUARIOROL",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    USUARIOID = table.Column<string>(nullable: false),
                    ROLID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOROL", x => new { x.USUARIOID, x.ROLID });
                    table.ForeignKey(
                        name: "FK_USUARIOROL_ROL_ROLID",
                        column: x => x.ROLID,
                        principalSchema: "SEGURIDAD",
                        principalTable: "ROL",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_USUARIOROL_USUARIO_USUARIOID",
                        column: x => x.USUARIOID,
                        principalSchema: "SEGURIDAD",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USUARIOTOKEN",
                schema: "SEGURIDAD",
                columns: table => new
                {
                    USUARIOID = table.Column<string>(nullable: false),
                    PROVEEDOR = table.Column<string>(nullable: false),
                    NOMBRE = table.Column<string>(nullable: false),
                    VALOR = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOTOKEN", x => new { x.USUARIOID, x.PROVEEDOR, x.NOMBRE });
                    table.ForeignKey(
                        name: "FK_USUARIOTOKEN_USUARIO_USUARIOID",
                        column: x => x.USUARIOID,
                        principalSchema: "SEGURIDAD",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AUTENTICACIONUSUARIO_USUARIOID",
                schema: "SEGURIDAD",
                table: "AUTENTICACIONUSUARIO",
                column: "USUARIOID");

            migrationBuilder.CreateIndex(
                name: "IX_MENU_MENUPADREID",
                schema: "SEGURIDAD",
                table: "MENU",
                column: "MENUPADREID");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "SEGURIDAD",
                table: "ROL",
                column: "NOMBRENORMALIZADO",
                unique: true,
                filter: "[NOMBRENORMALIZADO] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ROLMENU_MENUID",
                schema: "SEGURIDAD",
                table: "ROLMENU",
                column: "MENUID");

            migrationBuilder.CreateIndex(
                name: "IX_ROLMENU_ROLID",
                schema: "SEGURIDAD",
                table: "ROLMENU",
                column: "ROLID");

            migrationBuilder.CreateIndex(
                name: "IX_ROLPERMISO_ROLID",
                schema: "SEGURIDAD",
                table: "ROLPERMISO",
                column: "ROLID");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "SEGURIDAD",
                table: "USUARIO",
                column: "CORREONORMALIZADO");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "SEGURIDAD",
                table: "USUARIO",
                column: "NOMBREUSUARIONORMALIZADO",
                unique: true,
                filter: "[NOMBREUSUARIONORMALIZADO] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOPERMISO_USUARIOID",
                schema: "SEGURIDAD",
                table: "USUARIOPERMISO",
                column: "USUARIOID");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIOROL_ROLID",
                schema: "SEGURIDAD",
                table: "USUARIOROL",
                column: "ROLID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUTENTICACIONUSUARIO",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "PERMISO",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "ROLMENU",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "ROLPERMISO",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "USUARIOPERMISO",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "USUARIOROL",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "USUARIOTOKEN",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "MENU",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "ROL",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "USUARIO",
                schema: "SEGURIDAD");
        }
    }
}
