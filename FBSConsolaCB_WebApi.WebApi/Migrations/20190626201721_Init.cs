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
                name: "CONSOLA");

            migrationBuilder.EnsureSchema(
                name: "ESTRUCTURAEMPRESARIAL");

            migrationBuilder.EnsureSchema(
                name: "NOMENCLADOR");

            migrationBuilder.EnsureSchema(
                name: "SEGURIDAD");

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

            migrationBuilder.CreateTable(
                name: "PERSONA",
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
                    NUMEROIDENTIFICADOR = table.Column<int>(nullable: false),
                    IDENTIFICACION = table.Column<string>(nullable: true),
                    CORREOELECTRONICO = table.Column<string>(nullable: true),
                    TELEFONO = table.Column<string>(nullable: true),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    TIPOIDENTIFICACIONID = table.Column<int>(nullable: true),
                    OFICINAID = table.Column<int>(nullable: true),
                    USUARIOID = table.Column<string>(nullable: true),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PERSONA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PERSONA_OFICINA_OFICINAID",
                        column: x => x.OFICINAID,
                        principalSchema: "ESTRUCTURAEMPRESARIAL",
                        principalTable: "OFICINA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PERSONA_CATALOGO_TIPOIDENTIFICACIONID",
                        column: x => x.TIPOIDENTIFICACIONID,
                        principalSchema: "NOMENCLADOR",
                        principalTable: "CATALOGO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PERSONA_USUARIO_USUARIOID",
                        column: x => x.USUARIOID,
                        principalSchema: "SEGURIDAD",
                        principalTable: "USUARIO",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

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
                        name: "FK_DISPOSITIVO_PERSONA_CORRESPONSALID",
                        column: x => x.CORRESPONSALID,
                        principalSchema: "ESTRUCTURAEMPRESARIAL",
                        principalTable: "PERSONA",
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

            migrationBuilder.CreateTable(
                name: "SUPERVISOR",
                schema: "ESTRUCTURAEMPRESARIAL",
                columns: table => new
                {
                    PERSONAID = table.Column<int>(nullable: false),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUPERVISOR", x => x.PERSONAID);
                    table.ForeignKey(
                        name: "FK_SUPERVISOR_PERSONA_PERSONAID",
                        column: x => x.PERSONAID,
                        principalSchema: "ESTRUCTURAEMPRESARIAL",
                        principalTable: "PERSONA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CORRESPONSAL",
                schema: "ESTRUCTURAEMPRESARIAL",
                columns: table => new
                {
                    PERSONAID = table.Column<int>(nullable: false),
                    FECHANACIMIENTO = table.Column<DateTime>(nullable: false),
                    DIRECCION = table.Column<string>(nullable: true),
                    LATITUD = table.Column<float>(nullable: false),
                    LONGITUD = table.Column<float>(nullable: false),
                    SUPERVISORID = table.Column<int>(nullable: true),
                    ESTAACTIVO = table.Column<bool>(nullable: false),
                    CONCURRENCIA = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORRESPONSAL", x => x.PERSONAID);
                    table.ForeignKey(
                        name: "FK_CORRESPONSAL_PERSONA_PERSONAID",
                        column: x => x.PERSONAID,
                        principalSchema: "ESTRUCTURAEMPRESARIAL",
                        principalTable: "PERSONA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CORRESPONSAL_SUPERVISOR_SUPERVISORID",
                        column: x => x.SUPERVISORID,
                        principalSchema: "ESTRUCTURAEMPRESARIAL",
                        principalTable: "SUPERVISOR",
                        principalColumn: "PERSONAID",
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

            migrationBuilder.CreateIndex(
                name: "IX_CORRESPONSAL_SUPERVISORID",
                schema: "ESTRUCTURAEMPRESARIAL",
                table: "CORRESPONSAL",
                column: "SUPERVISORID");

            migrationBuilder.CreateIndex(
                name: "IX_OFICINA_EMPRESAID",
                schema: "ESTRUCTURAEMPRESARIAL",
                table: "OFICINA",
                column: "EMPRESAID");

            migrationBuilder.CreateIndex(
                name: "IX_PERSONA_OFICINAID",
                schema: "ESTRUCTURAEMPRESARIAL",
                table: "PERSONA",
                column: "OFICINAID");

            migrationBuilder.CreateIndex(
                name: "IX_PERSONA_TIPOIDENTIFICACIONID",
                schema: "ESTRUCTURAEMPRESARIAL",
                table: "PERSONA",
                column: "TIPOIDENTIFICACIONID");

            migrationBuilder.CreateIndex(
                name: "IX_PERSONA_USUARIOID",
                schema: "ESTRUCTURAEMPRESARIAL",
                table: "PERSONA",
                column: "USUARIOID");

            migrationBuilder.CreateIndex(
                name: "IX_CATALOGO_TIPOCATALOGOID",
                schema: "NOMENCLADOR",
                table: "CATALOGO",
                column: "TIPOCATALOGOID");

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
                name: "DISPOSITIVO",
                schema: "CONSOLA");

            migrationBuilder.DropTable(
                name: "CORRESPONSAL",
                schema: "ESTRUCTURAEMPRESARIAL");

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
                name: "SUPERVISOR",
                schema: "ESTRUCTURAEMPRESARIAL");

            migrationBuilder.DropTable(
                name: "MENU",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "ROL",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "PERSONA",
                schema: "ESTRUCTURAEMPRESARIAL");

            migrationBuilder.DropTable(
                name: "OFICINA",
                schema: "ESTRUCTURAEMPRESARIAL");

            migrationBuilder.DropTable(
                name: "CATALOGO",
                schema: "NOMENCLADOR");

            migrationBuilder.DropTable(
                name: "USUARIO",
                schema: "SEGURIDAD");

            migrationBuilder.DropTable(
                name: "EMPRESA",
                schema: "ESTRUCTURAEMPRESARIAL");

            migrationBuilder.DropTable(
                name: "TIPOCATALOGO",
                schema: "NOMENCLADOR");
        }
    }
}
