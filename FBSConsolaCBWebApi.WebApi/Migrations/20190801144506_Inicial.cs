using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSConsolaCBWebApi.WebApi.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Seguridad");

            migrationBuilder.EnsureSchema(
                name: "Consola");

            migrationBuilder.EnsureSchema(
                name: "EstructuraEmpresarial");

            migrationBuilder.EnsureSchema(
                name: "Nomenclador");

            migrationBuilder.CreateTable(
                name: "Empresa",
                schema: "EstructuraEmpresarial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true),
                    Direccion = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Ruc = table.Column<string>(nullable: true),
                    ActividadEconomica = table.Column<string>(nullable: true),
                    Provincia = table.Column<string>(nullable: true),
                    Departamento = table.Column<string>(nullable: true),
                    Distrito = table.Column<string>(nullable: true),
                    FechaInicioActividad = table.Column<DateTime>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoCatalogo",
                schema: "Nomenclador",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCatalogo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true),
                    Orden = table.Column<int>(nullable: false),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Icono = table.Column<string>(nullable: true),
                    Ruta = table.Column<string>(nullable: true),
                    MenuPadreId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menu_Menu_MenuPadreId",
                        column: x => x.MenuPadreId,
                        principalSchema: "Seguridad",
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permiso",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    Identificador = table.Column<string>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    UrlEndPoint = table.Column<string>(nullable: true),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permiso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 256, nullable: true),
                    NombreNormalizado = table.Column<string>(maxLength: 256, nullable: true),
                    Concurrencia = table.Column<string>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false, defaultValue: true),
                    Descripcion = table.Column<string>(maxLength: 256, nullable: true),
                    Tipo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Usuario = table.Column<string>(maxLength: 256, nullable: true),
                    NombreUsuarioNormalizado = table.Column<string>(maxLength: 256, nullable: true),
                    CorreoElectronico = table.Column<string>(maxLength: 256, nullable: true),
                    CorreoElectronicoNormalizado = table.Column<string>(maxLength: 256, nullable: true),
                    CorreoElectronicoConfirmado = table.Column<bool>(nullable: false),
                    Contrasenna = table.Column<string>(nullable: true),
                    MarcaSeguridad = table.Column<string>(nullable: true),
                    Concurrencia = table.Column<string>(nullable: true),
                    Telefono = table.Column<string>(nullable: true),
                    TelefonoConfirmado = table.Column<bool>(nullable: false),
                    DobleVerificacion = table.Column<bool>(nullable: false),
                    FinBloqueo = table.Column<DateTimeOffset>(nullable: true),
                    BloqueoActivo = table.Column<bool>(nullable: false),
                    AccesosFallidos = table.Column<int>(nullable: false),
                    EstaActivo = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oficina",
                schema: "EstructuraEmpresarial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Ciudad = table.Column<string>(nullable: true),
                    EmpresaId = table.Column<int>(nullable: true),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oficina", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Oficina_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalSchema: "EstructuraEmpresarial",
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Catalogo",
                schema: "Nomenclador",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    TipoCatalogoId = table.Column<int>(nullable: true),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catalogo_TipoCatalogo_TipoCatalogoId",
                        column: x => x.TipoCatalogoId,
                        principalSchema: "Nomenclador",
                        principalTable: "TipoCatalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolMenu",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RolId = table.Column<string>(nullable: true),
                    MenuId = table.Column<int>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolMenu_Menu_MenuId",
                        column: x => x.MenuId,
                        principalSchema: "Seguridad",
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolMenu_Rol_RolId",
                        column: x => x.RolId,
                        principalSchema: "Seguridad",
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolPermiso",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RolId = table.Column<string>(nullable: false),
                    Tipo = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolPermiso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolPermiso_Rol_RolId",
                        column: x => x.RolId,
                        principalSchema: "Seguridad",
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutenticacionUsuario",
                schema: "Seguridad",
                columns: table => new
                {
                    Proveedor = table.Column<string>(nullable: false),
                    LlaveProveedor = table.Column<string>(nullable: false),
                    NombreProveedor = table.Column<string>(nullable: true),
                    UsuarioId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutenticacionUsuario", x => new { x.Proveedor, x.LlaveProveedor });
                    table.ForeignKey(
                        name: "FK_AutenticacionUsuario_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "Seguridad",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioPermiso",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<string>(nullable: false),
                    Tipo = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPermiso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioPermiso_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "Seguridad",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRol",
                schema: "Seguridad",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(nullable: false),
                    RolId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRol", x => new { x.UsuarioId, x.RolId });
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Rol_RolId",
                        column: x => x.RolId,
                        principalSchema: "Seguridad",
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "Seguridad",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioToken",
                schema: "Seguridad",
                columns: table => new
                {
                    UsuarioId = table.Column<string>(nullable: false),
                    Proveedor = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioToken", x => new { x.UsuarioId, x.Proveedor, x.Nombre });
                    table.ForeignKey(
                        name: "FK_UsuarioToken_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "Seguridad",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dispositivo",
                schema: "Consola",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(nullable: true),
                    Imei = table.Column<string>(nullable: true),
                    Mac = table.Column<string>(nullable: true),
                    NumeroSerie = table.Column<string>(nullable: true),
                    TipoDispositivoId = table.Column<int>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispositivo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dispositivo_Catalogo_TipoDispositivoId",
                        column: x => x.TipoDispositivoId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Persona",
                schema: "EstructuraEmpresarial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PrimerNombre = table.Column<string>(nullable: true),
                    SegundoNombre = table.Column<string>(nullable: true),
                    PrimerApellido = table.Column<string>(nullable: true),
                    SegundoApellido = table.Column<string>(nullable: true),
                    NombreUnido = table.Column<string>(nullable: true),
                    NumeroIdentificador = table.Column<int>(nullable: false),
                    Identificacion = table.Column<string>(nullable: true),
                    Direccion = table.Column<string>(nullable: true),
                    FechaNacimiento = table.Column<DateTime>(nullable: true),
                    CorreoElectronico = table.Column<string>(nullable: true),
                    Telefono = table.Column<string>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    ImagenUrl = table.Column<string>(nullable: true),
                    TipoIdentificacionId = table.Column<int>(nullable: true),
                    OficinaId = table.Column<int>(nullable: true),
                    UsuarioId = table.Column<string>(nullable: true),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persona_Oficina_OficinaId",
                        column: x => x.OficinaId,
                        principalSchema: "EstructuraEmpresarial",
                        principalTable: "Oficina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Persona_Catalogo_TipoIdentificacionId",
                        column: x => x.TipoIdentificacionId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Persona_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "Seguridad",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "Supervisor",
                schema: "EstructuraEmpresarial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supervisor_Persona_Id",
                        column: x => x.Id,
                        principalSchema: "EstructuraEmpresarial",
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Corresponsal",
                schema: "EstructuraEmpresarial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Latitud = table.Column<float>(nullable: false),
                    Longitud = table.Column<float>(nullable: false),
                    SupervisorId = table.Column<int>(nullable: true),
                    UsuarioActivo = table.Column<bool>(nullable: false),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corresponsal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Corresponsal_Persona_Id",
                        column: x => x.Id,
                        principalSchema: "EstructuraEmpresarial",
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Corresponsal_Supervisor_SupervisorId",
                        column: x => x.SupervisorId,
                        principalSchema: "EstructuraEmpresarial",
                        principalTable: "Supervisor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "LimiteExistencia",
                schema: "Consola",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Limite = table.Column<double>(nullable: false),
                    CorresponsalId = table.Column<int>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LimiteExistencia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LimiteExistencia_Corresponsal_CorresponsalId",
                        column: x => x.CorresponsalId,
                        principalSchema: "EstructuraEmpresarial",
                        principalTable: "Corresponsal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LimiteTransaccional",
                schema: "Consola",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Monto = table.Column<double>(nullable: false),
                    Dias = table.Column<int>(nullable: false),
                    CorresponsalId = table.Column<int>(nullable: true),
                    OperacionId = table.Column<int>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LimiteTransaccional", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LimiteTransaccional_Corresponsal_CorresponsalId",
                        column: x => x.CorresponsalId,
                        principalSchema: "EstructuraEmpresarial",
                        principalTable: "Corresponsal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LimiteTransaccional_Catalogo_OperacionId",
                        column: x => x.OperacionId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                schema: "Consola",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Transaccion = table.Column<string>(nullable: true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Canal = table.Column<string>(nullable: true),
                    Json = table.Column<string>(nullable: true),
                    OperacionId = table.Column<int>(nullable: true),
                    CorresponsalId = table.Column<int>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Corresponsal_CorresponsalId",
                        column: x => x.CorresponsalId,
                        principalSchema: "EstructuraEmpresarial",
                        principalTable: "Corresponsal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Log_Catalogo_OperacionId",
                        column: x => x.OperacionId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
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

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivo_TipoDispositivoId",
                schema: "Consola",
                table: "Dispositivo",
                column: "TipoDispositivoId");

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

            migrationBuilder.CreateIndex(
                name: "IX_LimiteExistencia_CorresponsalId",
                schema: "Consola",
                table: "LimiteExistencia",
                column: "CorresponsalId");

            migrationBuilder.CreateIndex(
                name: "IX_LimiteTransaccional_CorresponsalId",
                schema: "Consola",
                table: "LimiteTransaccional",
                column: "CorresponsalId");

            migrationBuilder.CreateIndex(
                name: "IX_LimiteTransaccional_OperacionId",
                schema: "Consola",
                table: "LimiteTransaccional",
                column: "OperacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_CorresponsalId",
                schema: "Consola",
                table: "Log",
                column: "CorresponsalId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_OperacionId",
                schema: "Consola",
                table: "Log",
                column: "OperacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Corresponsal_SupervisorId",
                schema: "EstructuraEmpresarial",
                table: "Corresponsal",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Oficina_EmpresaId",
                schema: "EstructuraEmpresarial",
                table: "Oficina",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_OficinaId",
                schema: "EstructuraEmpresarial",
                table: "Persona",
                column: "OficinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_TipoIdentificacionId",
                schema: "EstructuraEmpresarial",
                table: "Persona",
                column: "TipoIdentificacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Persona_UsuarioId",
                schema: "EstructuraEmpresarial",
                table: "Persona",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogo_TipoCatalogoId",
                schema: "Nomenclador",
                table: "Catalogo",
                column: "TipoCatalogoId");

            migrationBuilder.CreateIndex(
                name: "IX_AutenticacionUsuario_UsuarioId",
                schema: "Seguridad",
                table: "AutenticacionUsuario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_MenuPadreId",
                schema: "Seguridad",
                table: "Menu",
                column: "MenuPadreId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Seguridad",
                table: "Rol",
                column: "NombreNormalizado",
                unique: true,
                filter: "[NombreNormalizado] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RolMenu_MenuId",
                schema: "Seguridad",
                table: "RolMenu",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RolMenu_RolId",
                schema: "Seguridad",
                table: "RolMenu",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_RolPermiso_RolId",
                schema: "Seguridad",
                table: "RolPermiso",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Seguridad",
                table: "Usuario",
                column: "CorreoElectronicoNormalizado");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Seguridad",
                table: "Usuario",
                column: "NombreUsuarioNormalizado",
                unique: true,
                filter: "[NombreUsuarioNormalizado] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPermiso_UsuarioId",
                schema: "Seguridad",
                table: "UsuarioPermiso",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_RolId",
                schema: "Seguridad",
                table: "UsuarioRol",
                column: "RolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerta",
                schema: "Consola");

            migrationBuilder.DropTable(
                name: "DispositivoCorresponsal",
                schema: "Consola");

            migrationBuilder.DropTable(
                name: "LimiteExistencia",
                schema: "Consola");

            migrationBuilder.DropTable(
                name: "LimiteTransaccional",
                schema: "Consola");

            migrationBuilder.DropTable(
                name: "Log",
                schema: "Consola");

            migrationBuilder.DropTable(
                name: "AutenticacionUsuario",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Permiso",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "RolMenu",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "RolPermiso",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "UsuarioPermiso",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "UsuarioRol",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "UsuarioToken",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Dispositivo",
                schema: "Consola");

            migrationBuilder.DropTable(
                name: "Corresponsal",
                schema: "EstructuraEmpresarial");

            migrationBuilder.DropTable(
                name: "Menu",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Rol",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Supervisor",
                schema: "EstructuraEmpresarial");

            migrationBuilder.DropTable(
                name: "Persona",
                schema: "EstructuraEmpresarial");

            migrationBuilder.DropTable(
                name: "Oficina",
                schema: "EstructuraEmpresarial");

            migrationBuilder.DropTable(
                name: "Catalogo",
                schema: "Nomenclador");

            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Empresa",
                schema: "EstructuraEmpresarial");

            migrationBuilder.DropTable(
                name: "TipoCatalogo",
                schema: "Nomenclador");
        }
    }
}
