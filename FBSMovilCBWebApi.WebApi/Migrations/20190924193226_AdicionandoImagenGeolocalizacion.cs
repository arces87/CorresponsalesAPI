using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FBSMovilCBWebApi.WebApi.Migrations
{
    public partial class AdicionandoImagenGeolocalizacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Nomenclador");

            migrationBuilder.EnsureSchema(
                name: "Seguridad");

            migrationBuilder.EnsureSchema(
                name: "Canales");

            migrationBuilder.EnsureSchema(
                name: "Corresponsales");

            migrationBuilder.CreateTable(
                name: "Geolocalizacion",
                schema: "Canales",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Latitud = table.Column<double>(nullable: false),
                    Longitud = table.Column<double>(nullable: false),
                    FechaAlta = table.Column<DateTime>(nullable: false),
                    FechaBaja = table.Column<DateTime>(nullable: false),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geolocalizacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoCatalogo",
                schema: "Nomenclador",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
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
                name: "Canal",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    JsonConfiguracion = table.Column<string>(nullable: true),
                    JsonNegocio = table.Column<string>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Orden = table.Column<int>(nullable: false),
                    Icono = table.Column<string>(nullable: true),
                    Ruta = table.Column<string>(nullable: true),
                    MenuPadreId = table.Column<Guid>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
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
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    Identificador = table.Column<string>(nullable: true),
                    UrlEndPoint = table.Column<string>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
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
                    Descripcion = table.Column<string>(maxLength: 256, nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false, defaultValue: true),
                    Tipo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Catalogo",
                schema: "Nomenclador",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nombre = table.Column<string>(nullable: true),
                    Descripcion = table.Column<string>(nullable: true),
                    TipoCatalogoId = table.Column<Guid>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
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
                    Id = table.Column<Guid>(nullable: false),
                    RolId = table.Column<string>(nullable: true),
                    MenuId = table.Column<Guid>(nullable: true),
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
                name: "Dispositivo",
                schema: "Canales",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MacAddress = table.Column<string>(nullable: true),
                    Modelo = table.Column<string>(nullable: true),
                    NumeroSerie = table.Column<string>(nullable: true),
                    TieneImpresora = table.Column<bool>(nullable: false),
                    DireccionImpresora = table.Column<string>(nullable: true),
                    Observaciones = table.Column<string>(nullable: true),
                    Ubicacion = table.Column<string>(nullable: true),
                    Imei = table.Column<string>(nullable: true),
                    MarcaId = table.Column<Guid>(nullable: true),
                    SistemaOperativoId = table.Column<Guid>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispositivo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dispositivo_Catalogo_MarcaId",
                        column: x => x.MarcaId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dispositivo_Catalogo_SistemaOperativoId",
                        column: x => x.SistemaOperativoId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Codigo = table.Column<string>(maxLength: 256, nullable: true),
                    CodigoNormalizado = table.Column<string>(maxLength: 256, nullable: true),
                    CorreoElectronico = table.Column<string>(maxLength: 256, nullable: true),
                    CorreoElectronicoNormalizado = table.Column<string>(maxLength: 256, nullable: true),
                    CorreoElectronicoConfirmado = table.Column<bool>(nullable: false),
                    Contrasenia = table.Column<string>(nullable: true),
                    MarcaSeguridad = table.Column<string>(nullable: true),
                    Concurrencia = table.Column<string>(nullable: true),
                    TelefonoCelular = table.Column<string>(nullable: true),
                    TelefonoConfirmado = table.Column<bool>(nullable: false),
                    DobleVerificacion = table.Column<bool>(nullable: false),
                    FinBloqueo = table.Column<DateTimeOffset>(nullable: true),
                    BloqueoActivo = table.Column<bool>(nullable: false),
                    AccesosFallidos = table.Column<int>(nullable: false),
                    NombreCompleto = table.Column<string>(maxLength: 256, nullable: true),
                    NombreMostrar = table.Column<string>(maxLength: 256, nullable: true),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    Imagen = table.Column<string>(maxLength: 256, nullable: true),
                    CambioContrasenia = table.Column<bool>(nullable: false),
                    OperadoraId = table.Column<Guid>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Catalogo_OperadoraId",
                        column: x => x.OperadoraId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Imagen",
                schema: "Canales",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DireccionImagen = table.Column<string>(nullable: true),
                    DispositivoId = table.Column<Guid>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imagen_Dispositivo_DispositivoId",
                        column: x => x.DispositivoId,
                        principalSchema: "Canales",
                        principalTable: "Dispositivo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                schema: "Canales",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsuarioId = table.Column<string>(nullable: true),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Hora = table.Column<TimeSpan>(nullable: false),
                    Criptografia = table.Column<string>(nullable: true),
                    JsonDispositivo = table.Column<string>(nullable: true),
                    JsonLog = table.Column<string>(nullable: true),
                    TipoAccionId = table.Column<Guid>(nullable: true),
                    RelacionadoId = table.Column<int>(nullable: false),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Catalogo_TipoAccionId",
                        column: x => x.TipoAccionId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Log_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "Seguridad",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Agente",
                schema: "Corresponsales",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NombreAgente = table.Column<string>(nullable: true),
                    JsonAgente = table.Column<string>(nullable: true),
                    Identificacion = table.Column<string>(nullable: true),
                    Ubicacion = table.Column<string>(nullable: true),
                    EstadoId = table.Column<Guid>(nullable: true),
                    UsuarioId = table.Column<string>(nullable: true),
                    SupervisorId = table.Column<string>(nullable: true),
                    DispositivoId = table.Column<Guid>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agente_Dispositivo_DispositivoId",
                        column: x => x.DispositivoId,
                        principalSchema: "Canales",
                        principalTable: "Dispositivo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agente_Catalogo_EstadoId",
                        column: x => x.EstadoId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agente_Usuario_SupervisorId",
                        column: x => x.SupervisorId,
                        principalSchema: "Seguridad",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agente_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "Seguridad",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "CanalUsuario",
                schema: "Seguridad",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CanalId = table.Column<Guid>(nullable: true),
                    UsuarioId = table.Column<string>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanalUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanalUsuario_Canal_CanalId",
                        column: x => x.CanalId,
                        principalSchema: "Seguridad",
                        principalTable: "Canal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CanalUsuario_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "Seguridad",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "AgenteGeolocalizacion",
                schema: "Canales",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AgenteId = table.Column<Guid>(nullable: true),
                    GeolocalizacionId = table.Column<Guid>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgenteGeolocalizacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgenteGeolocalizacion_Agente_AgenteId",
                        column: x => x.AgenteId,
                        principalSchema: "Corresponsales",
                        principalTable: "Agente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AgenteGeolocalizacion_Geolocalizacion_GeolocalizacionId",
                        column: x => x.GeolocalizacionId,
                        principalSchema: "Canales",
                        principalTable: "Geolocalizacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Alerta",
                schema: "Corresponsales",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    Hora = table.Column<TimeSpan>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true),
                    EstadoId = table.Column<Guid>(nullable: true),
                    AgenteId = table.Column<Guid>(nullable: true),
                    TipoId = table.Column<Guid>(nullable: true),
                    Comentario = table.Column<string>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerta_Agente_AgenteId",
                        column: x => x.AgenteId,
                        principalSchema: "Corresponsales",
                        principalTable: "Agente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alerta_Catalogo_EstadoId",
                        column: x => x.EstadoId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alerta_Catalogo_TipoId",
                        column: x => x.TipoId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cuenta",
                schema: "Corresponsales",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Tipo = table.Column<string>(nullable: true),
                    NumeroCuenta = table.Column<string>(nullable: true),
                    SaldoActual = table.Column<double>(nullable: false),
                    AgenteId = table.Column<Guid>(nullable: true),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuenta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cuenta_Agente_AgenteId",
                        column: x => x.AgenteId,
                        principalSchema: "Corresponsales",
                        principalTable: "Agente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transaccion",
                schema: "Corresponsales",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FechaSistema = table.Column<DateTime>(nullable: false),
                    FechaDispositivo = table.Column<DateTime>(nullable: false),
                    HoraDispositivo = table.Column<TimeSpan>(nullable: false),
                    AgenteId = table.Column<Guid>(nullable: true),
                    Criptografia = table.Column<string>(nullable: true),
                    Tipo = table.Column<string>(nullable: true),
                    JsonDatos = table.Column<string>(nullable: true),
                    EstadoId = table.Column<Guid>(nullable: true),
                    Valor = table.Column<double>(nullable: false),
                    CanalId = table.Column<int>(nullable: false),
                    ReposicionRealizada = table.Column<bool>(nullable: false),
                    EstaActivo = table.Column<bool>(nullable: false),
                    Concurrencia = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaccion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaccion_Agente_AgenteId",
                        column: x => x.AgenteId,
                        principalSchema: "Corresponsales",
                        principalTable: "Agente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaccion_Catalogo_EstadoId",
                        column: x => x.EstadoId,
                        principalSchema: "Nomenclador",
                        principalTable: "Catalogo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgenteGeolocalizacion_AgenteId",
                schema: "Canales",
                table: "AgenteGeolocalizacion",
                column: "AgenteId");

            migrationBuilder.CreateIndex(
                name: "IX_AgenteGeolocalizacion_GeolocalizacionId",
                schema: "Canales",
                table: "AgenteGeolocalizacion",
                column: "GeolocalizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivo_MarcaId",
                schema: "Canales",
                table: "Dispositivo",
                column: "MarcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivo_SistemaOperativoId",
                schema: "Canales",
                table: "Dispositivo",
                column: "SistemaOperativoId");

            migrationBuilder.CreateIndex(
                name: "IX_Imagen_DispositivoId",
                schema: "Canales",
                table: "Imagen",
                column: "DispositivoId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagenGeolocalizacion_GeolocalizacionId",
                schema: "Canales",
                table: "ImagenGeolocalizacion",
                column: "GeolocalizacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_TipoAccionId",
                schema: "Canales",
                table: "Log",
                column: "TipoAccionId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_UsuarioId",
                schema: "Canales",
                table: "Log",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Agente_DispositivoId",
                schema: "Corresponsales",
                table: "Agente",
                column: "DispositivoId");

            migrationBuilder.CreateIndex(
                name: "IX_Agente_EstadoId",
                schema: "Corresponsales",
                table: "Agente",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Agente_SupervisorId",
                schema: "Corresponsales",
                table: "Agente",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Agente_UsuarioId",
                schema: "Corresponsales",
                table: "Agente",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerta_AgenteId",
                schema: "Corresponsales",
                table: "Alerta",
                column: "AgenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerta_EstadoId",
                schema: "Corresponsales",
                table: "Alerta",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerta_TipoId",
                schema: "Corresponsales",
                table: "Alerta",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Cuenta_AgenteId",
                schema: "Corresponsales",
                table: "Cuenta",
                column: "AgenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_AgenteId",
                schema: "Corresponsales",
                table: "Transaccion",
                column: "AgenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaccion_EstadoId",
                schema: "Corresponsales",
                table: "Transaccion",
                column: "EstadoId");

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
                name: "IX_CanalUsuario_CanalId",
                schema: "Seguridad",
                table: "CanalUsuario",
                column: "CanalId");

            migrationBuilder.CreateIndex(
                name: "IX_CanalUsuario_UsuarioId",
                schema: "Seguridad",
                table: "CanalUsuario",
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
                column: "CodigoNormalizado",
                unique: true,
                filter: "[CodigoNormalizado] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_OperadoraId",
                schema: "Seguridad",
                table: "Usuario",
                column: "OperadoraId");

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
                name: "AgenteGeolocalizacion",
                schema: "Canales");

            migrationBuilder.DropTable(
                name: "Imagen",
                schema: "Canales");

            migrationBuilder.DropTable(
                name: "ImagenGeolocalizacion",
                schema: "Canales");

            migrationBuilder.DropTable(
                name: "Log",
                schema: "Canales");

            migrationBuilder.DropTable(
                name: "Alerta",
                schema: "Corresponsales");

            migrationBuilder.DropTable(
                name: "Cuenta",
                schema: "Corresponsales");

            migrationBuilder.DropTable(
                name: "Transaccion",
                schema: "Corresponsales");

            migrationBuilder.DropTable(
                name: "AutenticacionUsuario",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "CanalUsuario",
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
                name: "Geolocalizacion",
                schema: "Canales");

            migrationBuilder.DropTable(
                name: "Agente",
                schema: "Corresponsales");

            migrationBuilder.DropTable(
                name: "Canal",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Menu",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Rol",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Dispositivo",
                schema: "Canales");

            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "Seguridad");

            migrationBuilder.DropTable(
                name: "Catalogo",
                schema: "Nomenclador");

            migrationBuilder.DropTable(
                name: "TipoCatalogo",
                schema: "Nomenclador");
        }
    }
}
