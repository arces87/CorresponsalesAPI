using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.DAL.Nomenclador;
using Microsoft.EntityFrameworkCore;

namespace FBSConsolaCBWebApi.DAL
{
    public class ContextoFBSConsolaCB : ContextoFBSIdentidad
    {
        public ContextoFBSConsolaCB(DbContextOptions<ContextoFBSConsolaCB> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #region Estructura Empresarial
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Oficina> Oficinas { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Supervisor> Supervisores { get; set; }

        public DbSet<Corresponsal> Corresponsales { get; set; }

        #endregion

        #region Nomencladores
        public DbSet<Catalogo> Catalogos { get; set; }
        public DbSet<TipoCatalogo> TiposCatalogos { get; set; }
        #endregion

        #region Consolas
        public DbSet<Dispositivo> Dispositivos { get; set; }
        public DbSet<DispositivoCorresponsal> DispositivosCorresponsales { get; set; }
        public DbSet<LimiteTransaccional> LimitesTransaccionales { get; set; }
        public DbSet<LimiteExistencia> LimitesExistencias { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Alerta> Alertas { get; set; }
        #endregion
    }
}
