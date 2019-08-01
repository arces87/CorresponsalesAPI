using Dapper;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.Consola
{
    public class RepositorioAlerta : Repositorio<Alerta>, IRepositorioAlerta
    {
        private readonly IConfiguration _configuracion;

        public RepositorioAlerta(ContextoFBSConsolaCB context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }

        public async Task<IEnumerable<Alerta>> GetAllActive()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var Alertas = await conexion.QueryAsync<Alerta>("SELECT * FROM Consola.Alerta where EstaActivo='true'");
                return Alertas;
            }
        }

        public override async Task<string> Add(Alerta entity)
        {
            entity.Destinatario = Context.Personas.FirstOrDefault(c => c.Id == entity.Destinatario.Id);
            entity.Remitente = Context.Personas.FirstOrDefault(c => c.Id == entity.Remitente.Id);
            entity.Categoria = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Categoria.Id);
            entity.EstaActivo = true;
            Context.Alertas.Add(entity);
            await Context.SaveChangesAsync();
            return entity.Id.ToString();
        }

        public override async Task<Alerta> Get(int Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var Alertas = await conexion.QueryAsync<Alerta>("SELECT * FROM Consola.Alerta where EstaActivo='true' and Id = @Id", param: new { Id });
                return Alertas.FirstOrDefault();
            }
        }

        public override async Task Remove(Alerta entity)
        {
            var alerta = _contexto.Set<Alerta>().FirstOrDefault(o => o.Id == entity.Id);
            alerta.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
