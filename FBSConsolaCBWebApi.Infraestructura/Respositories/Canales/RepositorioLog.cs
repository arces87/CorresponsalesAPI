using Dapper;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Seguridad;
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
                var logs = await conexion.QueryAsync<Log>("SELECT * FROM Canales.Log where EstaActivo='true'");
                return logs.ToList();
            }
        }
        public async Task<IEnumerable<Log>> GetAllWithAssociations()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var logs = await conexion.QueryAsync<Log, Catalogo, Usuario, Log>(@"SELECT * FROM Canales.Log " +
                    "join Nomenclador.Catalogo tipo on Canales.Log.TipoAccionId = tipo.Id " +
                    "join Seguridad.Usuario usuario on Canales.Log.UsuarioId = usuario.Id " +
                    "where Canales.Log.EstaActivo='true'",
                    (log, tipo, usuario) =>
                    {
                        log.TipoAccion = tipo;
                        log.Usuario = usuario;
                        return log;
                    });
                return logs.ToList();
            }
        }
        public async Task<Log> GetWithAssociations(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var Logs = await conexion.QueryAsync<Log, Catalogo, Usuario, Log>(@"SELECT * FROM Canales.Log " +
                    "join Nomenclador.Catalogo tipo on Canales.Log.TipoAccionId = tipo.Id " +
                    "join Seguridad.Usuario usuario on Canales.Log.UsuarioId = usuario.Id " +
                    "where Canales.Log.EstaActivo='true' and Canales.Log.Id = @Id",
                    (log, tipo, usuario) =>
                    {
                        log.TipoAccion = tipo;
                        log.Usuario = usuario;
                        return log;
                    }, param: new { Id });

                return Logs.FirstOrDefault();
            }
        }

        public override async Task<Log> Get(string Id)
        {
            return await Context.Logs.FirstOrDefaultAsync(d => d.Id.ToString() == Id);
        }

        public override async Task Remove(Log entidad)
        {
            var catalogo = _contexto.Set<Log>().FirstOrDefault(o => o.Id == entidad.Id);
            catalogo.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public override async Task<string> Add(Log entidad)
        {
            entidad.TipoAccion = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.TipoAccion.Id);
            entidad.Estado = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.Estado.Id);
            entidad.Usuario = Context.Users.FirstOrDefault(c => c.UserName == entidad.Usuario.UserName);
            entidad.EstaActivo = true;
            Context.Logs.Add(entidad);
            await Context.SaveChangesAsync();
            return entidad.Id.ToString();
        }
        public override async Task Update(Log entidad)
        {
            entidad.TipoAccion = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.TipoAccion.Id);
            entidad.Estado = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.Estado.Id);
            entidad.Usuario = Context.Users.FirstOrDefault(c => c.Id == entidad.Usuario.Id);
            _contexto.Entry(entidad).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
