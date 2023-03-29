using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.DAL.Corresponsales;
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

        #region Corresponsales
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Alerta> Alertas { get; set; }
        public DbSet<Agente> Agentes { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<TransaccionRetiro> TransaccionesRetiro { get; set; }
        public DbSet<DispositivoAgente> DispositivoAgente { get; set; }

        #endregion

        #region Canales
        public DbSet<Dispositivo> Dispositivos { get; set; }
        public DbSet<Geolocalizacion> Geolocalizaciones { get; set; }
        public DbSet<ImagenGeolocalizacion> ImagenesGeolocalizaciones { get; set; }
        public DbSet<AgenteGeolocalizacion> AgentesGeolocalizaciones { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }
        #endregion
    }
}
