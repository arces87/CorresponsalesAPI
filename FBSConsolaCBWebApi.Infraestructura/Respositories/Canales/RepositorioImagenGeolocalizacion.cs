using Dapper;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.Canales
{
    public class RepositorioImagenGeolocalizacion : Repositorio<ImagenGeolocalizacion>, IRepositorioImagenGeolocalizacion
    {
        private readonly IConfiguration _configuracion;

        public RepositorioImagenGeolocalizacion(ContextoFBSConsolaCB context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }
        public async Task<IEnumerable<ImagenGeolocalizacion>> GetAllActive()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var imagenes = await conexion.QueryAsync<ImagenGeolocalizacion>("SELECT * FROM Canales.ImagenGeolocalizacion where EstaActivo='true'");
                return imagenes.ToList();
            }
        }
        public async Task<IEnumerable<ImagenGeolocalizacion>> GetAllWithAssociations()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var imagenes = await conexion.QueryAsync<ImagenGeolocalizacion, Geolocalizacion, ImagenGeolocalizacion>(@"SELECT * FROM Canales.ImagenGeolocalizacion " +
                    "join Canales.Geolocalizacion on Canales.ImagenGeolocalizacion.GeolocalizacionId = Canales.Geolocalizacion.Id" +
                    " where Canales.ImagenGeolocalizacion.EstaActivo='true'",
                    (imagen, geolocalizacion) =>
                    {
                        imagen.Geolocalizacion = geolocalizacion;
                        return imagen;
                    });
                return imagenes.ToList();
            }
        }
        public async Task<ImagenGeolocalizacion> GetWithAssociations(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var imagenes = await conexion.QueryAsync<ImagenGeolocalizacion, Geolocalizacion, ImagenGeolocalizacion>(@"SELECT * FROM Canales.ImagenGeolocalizacion " +
                   "join Canales.Geolocalizacion on Canales.ImagenGeolocalizacion.GeolocalizacionId = Canales.Geolocalizacion.Id" +
                   " where Canales.ImagenGeolocalizacion.EstaActivo='true' and Canales.ImagenGeolocalizacion.Id = @Id",
                   (imagen, geolocalizacion) =>
                   {
                       imagen.Geolocalizacion = geolocalizacion;
                       return imagen;
                   }, param: new { Id });

                return imagenes.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<ImagenGeolocalizacion>> GetForGeolocalizacion(string IdGeolocalizacion)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var imagenes = await conexion.QueryAsync<ImagenGeolocalizacion, Geolocalizacion, ImagenGeolocalizacion>(@"SELECT * FROM Canales.ImagenGeolocalizacion " +
                   "join Canales.Geolocalizacion on Canales.ImagenGeolocalizacion.GeolocalizacionId = Canales.Geolocalizacion.Id" +
                   " where Canales.ImagenGeolocalizacion.EstaActivo='true' and Canales.ImagenGeolocalizacion.GeolocalizacionId = @IdGeolocalizacion",
                   (imagen, geolocalizacion) =>
                   {
                       imagen.Geolocalizacion = geolocalizacion;
                       return imagen;
                   }, param: new { IdGeolocalizacion });

                return imagenes.ToList();
            }
        }

        public override async Task Remove(ImagenGeolocalizacion entidad)
        {
            var catalogo = _contexto.Set<ImagenGeolocalizacion>().FirstOrDefault(o => o.Id == entidad.Id);
            catalogo.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }
        public async Task RemoveAllForGeolocalizacion(string IdGeolocalizacion)
        {
            var imagen = Context.ImagenesGeolocalizaciones.Where(i => i.Geolocalizacion.Id.ToString() == IdGeolocalizacion).ToList();
            Context.ImagenesGeolocalizaciones.RemoveRange(imagen);
            await _contexto.SaveChangesAsync();
        }

        public override async Task<string> Add(ImagenGeolocalizacion entidad)
        {
            entidad.Geolocalizacion = Context.Geolocalizaciones.FirstOrDefault(c => c.Id == entidad.Geolocalizacion.Id);
            entidad.EstaActivo = true;
            Context.ImagenesGeolocalizaciones.Add(entidad);
            await Context.SaveChangesAsync();
            return entidad.Id.ToString();
        }
        public override async Task Update(ImagenGeolocalizacion entidad)
        {
            entidad.Geolocalizacion = Context.Geolocalizaciones.FirstOrDefault(c => c.Id == entidad.Geolocalizacion.Id);
            _contexto.Entry(entidad).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
