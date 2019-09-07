using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using System.Collections.Generic;
using System.Linq;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using FBS.DAL.Nomenclador;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.Nomenclador
{
    public class RepositorioTipoCatalogo : Repositorio<TipoCatalogo>, IRepositorioTipoCatalogo
    {
        private readonly IConfiguration _configuracion;

        public RepositorioTipoCatalogo(ContextoFBSConsolaCB context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }
        public async Task<IEnumerable<TipoCatalogo>> GetAllActive()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var tiposCatalogos = await conexion.QueryAsync<TipoCatalogo>("SELECT * FROM Nomenclador.TipoCatalogo where EstaActivo='true'");
                return tiposCatalogos.ToList();
            }
        }

        public override async Task Remove(TipoCatalogo entidad)
        {
            var tipo = _contexto.Set<TipoCatalogo>().FirstOrDefault(o => o.Id == entidad.Id);
            tipo.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public override async Task<TipoCatalogo> Get(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var tiposCatalogos = await conexion.QueryAsync<TipoCatalogo>("SELECT * FROM Nomenclador.TipoCatalogo where EstaActivo='true' and Id = @Id", param: new { Id });
                return tiposCatalogos.FirstOrDefault();
            }
        }

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
