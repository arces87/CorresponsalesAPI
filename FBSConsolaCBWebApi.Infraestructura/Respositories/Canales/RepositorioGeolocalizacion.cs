using Dapper;
using FBS.DAL.Nomenclador;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
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
        public async Task AdicionarGeolocalizacionAgente(double latitud, double longitud, string idAgente)
        {
            var geolocalizacion = new Geolocalizacion() { Latitud = latitud, Longitud = longitud, FechaAlta = DateTime.Now };
            Context.Geolocalizaciones.Add(geolocalizacion);
            var agente = Context.Agentes.FirstOrDefault(a => a.Id == new Guid(idAgente));
            var geolocalizacionAgente = new AgenteGeolocalizacion() { Agente = agente, Geolocalizacion = geolocalizacion };
            Context.AgentesGeolocalizaciones.Add(geolocalizacionAgente);
            await Context.SaveChangesAsync();
        }
        public override async Task Remove(Geolocalizacion entidad)
        {
            var geolocalizacion = _contexto.Set<Geolocalizacion>().FirstOrDefault(o => o.Id == entidad.Id);
            geolocalizacion.FechaBaja = DateTime.Now;
            geolocalizacion.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
