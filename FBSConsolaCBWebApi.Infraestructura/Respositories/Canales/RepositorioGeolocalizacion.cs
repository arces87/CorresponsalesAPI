using Dapper;
using FBS.DAL.Nomenclador;
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
    public class RepositorioGeolocalizacion : Repositorio<Geolocalizacion>, IRepositorioGeolocalizacion
    {
        private readonly IConfiguration _configuracion;

        public RepositorioGeolocalizacion(ContextoFBSConsolaCB context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }

        public async Task<Geolocalizacion> GetForAgente(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var geolocalizaciones = await conexion.QueryAsync<Geolocalizacion>(@"SELECT * FROM Canales.Geolocalizacion " +
                    "join Canales.AgenteGeolocalizacion on Canales.Geolocalizacion.Id = Canales.AgenteGeolocalizacion.GeolocalizacionId " +
                    "where Canales.AgenteGeolocalizacion.AgenteId = @Id and Canales.Geolocalizacion.EstaActivo='true'", param: new { Id });
                return geolocalizaciones.FirstOrDefault();
            }
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
