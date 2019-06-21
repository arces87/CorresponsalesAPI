using FBSConsolaCB_WebApi.DAL.Consola;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using Microsoft.EntityFrameworkCore;

namespace FBSConsolaCB_WebApi.DAL
{
    public class FBSConsolaCBContext : FBSIdentityDBContext
    {
        public FBSConsolaCBContext(DbContextOptions<FBSConsolaCBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #region Estructura Empresarial
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<AreaTrabajo> AreasTrabajos { get; set; }
        public DbSet<Oficina> Oficinas { get; set; }
        public DbSet<Corresponsal> Corresponsales { get; set; }

        public DbSet<Cargo> Cargos { get; set; }

        #endregion

        #region Nomencladores
        public DbSet<Catalogo> Catalogos { get; set; }
        public DbSet<TipoCatalogo> TiposCatalogos { get; set; }
        #endregion

        #region Consolas
        public DbSet<Dispositivo> Dispositivos { get; set; }
        #endregion
    }
}
