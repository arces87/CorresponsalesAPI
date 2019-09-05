using Dapper;
using FBS.DAL.Nomenclador;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.Consola
{
    public class RepositorioLog : Repositorio<Log>, IRepositorioLog
    {
        private readonly IConfiguration _configuracion;
        public RepositorioLog(ContextoFBSConsolaCB context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }
        public async Task<IEnumerable<Log>> GetAllActive()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var consolas = await conexion.QueryAsync<Log>("SELECT * FROM Consola.Log where EstaActivo='true'");
                return consolas.ToList();
            }
        }
        public async Task<IEnumerable<Log>> GetAllWithAssociations()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var consolas = await conexion.QueryAsync<Log, Corresponsal, Persona, Catalogo, Log>(@"SELECT * FROM Consola.Log l join EstructuraEmpresarial.Conrresponsal c" +
                    "on l.CorresponsalId = c.Id join EstructuraEmpresarial.Persona p on c.Id = p.Id join Nomenclador.Catalogo ca on l.OperacionId = ca.Id where EstaActivo='true'",
                    (log, corresponsal, persona, catalogo) =>
                    {
                        corresponsal.Persona = persona;
                        log.Corresponsal = corresponsal;
                        log.Operacion = catalogo;
                        return log;
                    }, splitOn: "CorresponsalId, Id, OperacionId");
                return consolas.ToList();
            }
        }
        public override async Task<Log> Get(int Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var consolas = await conexion.QueryAsync<Log>("SELECT * FROM Consola.Log where EstaActivo='true' and Id = @Id", param: new { Id });
                return consolas.FirstOrDefault();
            }
        }
        public async Task<Log> GetWithAssociations(int Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var consolas = await conexion.QueryAsync<Log, Corresponsal, Persona, Catalogo, Log>(@"SELECT * FROM Consola.Log l join EstructuraEmpresarial.Conrresponsal c" +
                    "on l.CorresponsalId = c.Id join EstructuraEmpresarial.Persona p on c.Id = p.Id join Nomenclador.Catalogo ca on l.OperacionId = ca.Id where EstaActivo='true'and Id = @Id",
                    (log, corresponsal, persona, catalogo) =>
                    {
                        corresponsal.Persona = persona;
                        log.Corresponsal = corresponsal;
                        log.Operacion = catalogo;
                        return log;
                    },
                    splitOn: "CorresponsalId, Id, OperacionId",
                    param: new { Id });
                return consolas.FirstOrDefault();
            }
        }
        public override async Task<string> Add(Log entity)
        {
            entity.Operacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Operacion.Id);
            entity.Corresponsal = Context.Corresponsales.FirstOrDefault(c => c.Id == entity.Corresponsal.Id);
            entity.EstaActivo = true;
            Context.Logs.Add(entity);
            await Context.SaveChangesAsync();
            return entity.Id.ToString();
        }


        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;
        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
