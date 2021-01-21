using Dapper;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Modelado;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.Corresponsales
{
    public class RepositorioTransaccionRetiro : Repositorio<TransaccionRetiro>, IRepositorioTransaccionRetiro
    {
        private readonly IConfiguration _configuracion;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public RepositorioTransaccionRetiro(ContextoFBSConsolaCB context, IConfiguration configuracion, IJsonConfiguracion jsonConfiguracion) : base(context)
        {
            _configuracion = configuracion;
            _jsonConfiguracion = jsonConfiguracion;
        }
        public async Task<IEnumerable<TransaccionRetiro>> GetAll()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var Transacciones = await conexion.QueryAsync<TransaccionRetiro>("SELECT * FROM Corresponsales.TransaccionRetiro");
                return Transacciones.ToList();
            }
        }
        
        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
