using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Seguridad;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.EntityFrameworkCore.DataEncryption.Providers;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace FBS.DAL
{
}
public partial class ContextoFBSIdentidad : IdentityDbContext<Usuario, Rol, string>
{
    public ContextoFBSIdentidad(DbContextOptions options) : base(options)
    {
     
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Seguridad
        modelBuilder.Entity("FBS.Identidad.DAL.Seguridad.Rol", b =>
        {
            b.Property<string>("Id").HasColumnName("Id")
                .ValueGeneratedOnAdd();

            b.Property<string>("Name").HasColumnName("Nombre")
                .HasMaxLength(256);

            b.Property<string>("NormalizedName").HasColumnName("NombreNormalizado")
                .HasMaxLength(256);
            b.Property<string>("Descripcion").HasColumnName("Descripcion")
               .HasMaxLength(256);

            b.Property<bool>("Type").HasColumnName("Tipo");

            b.Property<bool>("EstaActivo").HasColumnName("EstaActivo").HasDefaultValue(true);

            b.Property<string>("ConcurrencyStamp").HasColumnName("Concurrencia")
                .IsConcurrencyToken();
            b.HasKey("Id");

            b.HasIndex("NormalizedName")
                .IsUnique()
                .HasName("RoleNameIndex")
                .HasFilter("[NombreNormalizado] IS NOT NULL");

            b.ToTable("Rol", "Seguridad");
        });

        modelBuilder.Entity("FBS.Identidad.DAL.Seguridad.Usuario", b =>
        {
            b.Property<string>("Id").HasColumnName("Id")
                .ValueGeneratedOnAdd();

            b.Property<string>("UserName").HasColumnName("Codigo")
                .HasMaxLength(256);

            b.Property<string>("NombreCompleto").HasColumnName("NombreCompleto")
                .HasMaxLength(256);

            b.Property<string>("NombreMostrar").HasColumnName("NombreMostrar")
                .HasMaxLength(256);

            b.Property<string>("PasswordHash").HasColumnName("Contrasenia");

            b.Property<DateTime>("FechaCreacion").HasColumnName("FechaCreacion");

            b.Property<string>("Imagen").HasColumnName("Imagen")
                .HasMaxLength(256);

            b.Property<string>("Email").HasColumnName("CorreoElectronico")
                .HasMaxLength(256);

            b.Property<string>("PhoneNumber").HasColumnName("TelefonoCelular");

            b.Property<bool>("CambioContrasenia").HasColumnName("CambioContrasenia");

            b.Property<string>("NormalizedUserName").HasColumnName("CodigoNormalizado")
                .HasMaxLength(256);

            b.Property<string>("NormalizedEmail").HasColumnName("CorreoElectronicoNormalizado")
                .HasMaxLength(256);

            b.Property<bool>("EmailConfirmed").HasColumnName("CorreoElectronicoConfirmado");

            b.Property<bool>("PhoneNumberConfirmed").HasColumnName("TelefonoConfirmado");

            b.Property<string>("SecurityStamp").HasColumnName("MarcaSeguridad");

            b.Property<bool>("TwoFactorEnabled").HasColumnName("DobleVerificacion");

            b.Property<int>("AccessFailedCount").HasColumnName("AccesosFallidos");

            b.Property<bool>("LockoutEnabled").HasColumnName("BloqueoActivo");

            b.Property<DateTimeOffset?>("LockoutEnd").HasColumnName("FinBloqueo");

            b.Property<bool>("EstaActivo").HasColumnName("EstaActivo").HasDefaultValue(true);


            b.Property<string>("ConcurrencyStamp").HasColumnName("Concurrencia")
                .IsConcurrencyToken();

            b.HasKey("Id");

            b.HasIndex("NormalizedEmail")
                .HasName("EmailIndex");

            b.HasIndex("NormalizedUserName")
                .IsUnique()
                .HasName("UserNameIndex")
                .HasFilter("[CodigoNormalizado] IS NOT NULL");

            b.ToTable("Usuario", "Seguridad");
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
        {
            b.Property<int>("Id").HasColumnName("Id")
                .ValueGeneratedOnAdd();

            b.Property<string>("ClaimType").HasColumnName("Tipo");

            b.Property<string>("ClaimValue").HasColumnName("Valor");

            b.Property<string>("RoleId").HasColumnName("RolId")
                .IsRequired();

            b.HasKey("Id");

            b.HasIndex("RoleId");

            b.ToTable("RolPermiso", "Seguridad");
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
        {
            b.Property<int>("Id").HasColumnName("Id")
                .ValueGeneratedOnAdd();

            b.Property<string>("ClaimType").HasColumnName("Tipo");

            b.Property<string>("ClaimValue").HasColumnName("Valor");

            b.Property<string>("UserId").HasColumnName("UsuarioId")
                .IsRequired();

            b.HasKey("Id");

            b.HasIndex("UserId");

            b.ToTable("UsuarioPermiso", "Seguridad");
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
        {
            b.Property<string>("LoginProvider").HasColumnName("Proveedor");

            b.Property<string>("ProviderKey").HasColumnName("LlaveProveedor");

            b.Property<string>("ProviderDisplayName").HasColumnName("NombreProveedor");

            b.Property<string>("UserId").HasColumnName("UsuarioId")
                .IsRequired();

            b.HasKey("LoginProvider", "ProviderKey");

            b.HasIndex("UserId");

            b.ToTable("AutenticacionUsuario", "Seguridad");
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
        {
            b.Property<string>("UserId").HasColumnName("UsuarioId");

            b.Property<string>("RoleId").HasColumnName("RolId");

            b.HasKey("UserId", "RoleId");

            b.HasIndex("RoleId");

            b.ToTable("UsuarioRol", "Seguridad");
        });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
        {
            b.Property<string>("UserId").HasColumnName("UsuarioId");

            b.Property<string>("LoginProvider").HasColumnName("Proveedor");

            b.Property<string>("Name").HasColumnName("Nombre");

            b.Property<string>("Value").HasColumnName("Valor");

            b.HasKey("UserId", "LoginProvider", "Name");

            b.ToTable("UsuarioToken", "Seguridad");
        });      
        #endregion

    }


    #region Seguridad
    public DbSet<Menu> Menus { get; set; }
    public DbSet<RolMenu> RolesMenus { get; set; }
    public DbSet<Canal> Canales { get; set; }
    public DbSet<CanalUsuario> CanalesUsuarios { get; set; }
    #endregion

    #region Nomencladores
    public DbSet<Catalogo> Catalogos { get; set; }
    public DbSet<TipoCatalogo> TiposCatalogo { get; set; }
    #endregion


}
