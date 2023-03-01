using System.Linq;
using FBS.Infraestructura.Repositorio;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FBS.Identidad.Infraestructura.Repositorio
{
    public class RepositorioCanal : Repositorio<Canal>, IRepositorioCanal
    {
        private readonly IConfiguration _configuracion;

        public RepositorioCanal(ContextoFBSIdentidad context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }
        public async Task<Canal> GetCanalUsuario(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var canales = await conexion.QueryAsync<Canal>(@"SELECT * FROM Seguridad.Canal " +
                    "join Seguridad.CanalUsuario on Seguridad.Canal.Id = Seguridad.CanalUsuario.CanalId " +
                    " where Seguridad.Canal.EstaActivo='true' and Seguridad.CanalUsuario.UsuarioId = @Id", param: new { Id });

                return canales.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Canal>> GetAllWithAssociations(bool? Activo)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                if (Activo != null)
                {
                    var canales = await conexion.QueryAsync<Canal>(@"SELECT * FROM Seguridad.Canal " +
                    "where Seguridad.Canal.EstaActivo=@Activo", param: new { Activo });
                    return canales.ToList();
                }
                else
                {
                    var canales = await conexion.QueryAsync<Canal>(@"SELECT * FROM Seguridad.Canal");
                    return canales.ToList();
                }
            }
        }
        public async Task<Canal> GetWithAssociations(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var canales = await conexion.QueryAsync<Canal>(@"SELECT * FROM Seguridad.Canal " +
                    "where Seguridad.Canal.EstaActivo='true' and Seguridad.Canal.Id = @Id", param: new { Id });

                return canales.FirstOrDefault();
            }
        }

        public override async Task Remove(Canal entidad)
        {
            var canal = _contexto.Set<Canal>().FirstOrDefault(o => o.Id == entidad.Id);
            canal.EstaActivo = !canal.EstaActivo;
            _contexto.Entry(canal).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public override async Task Update(Canal entidad)
        {
            var canal = _contexto.Set<Canal>().FirstOrDefault(o => o.Id == entidad.Id);
            canal.Nombre = entidad.Nombre;
            canal.JsonNegocio = entidad.JsonNegocio;
            canal.JsonConfiguracion = entidad.JsonConfiguracion;
            _contexto.Entry(canal).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSIdentidad GetContext
        {
            get { return _contexto as ContextoFBSIdentidad; }
        }

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
