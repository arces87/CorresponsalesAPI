using Dapper;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Seguridad;
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
    public class RepositorioAgente : Repositorio<Agente>, IRepositorioAgente
    {
        private readonly IConfiguration _configuracion;

        public RepositorioAgente(ContextoFBSConsolaCB context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }
        public async Task<IEnumerable<Agente>> GetAllActive()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var Agentes = await conexion.QueryAsync<Agente>("SELECT * FROM Corresponsales.Agente where EstaActivo='true'");
                return Agentes.ToList();
            }
        }
        public async Task<IEnumerable<Agente>> GetAllWithAssociations()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var Agentes = await conexion.QueryAsync<Agente, Catalogo, Usuario, Usuario, Agente>(@"SELECT * FROM Corresponsales.Agente " +
                    "join Nomenclador.Catalogo estado on Corresponsales.Agente.EstadoId = estado.Id " +
                    "join Seguridad.Usuario usuario on Corresponsales.Agente.UsuarioId = usuario.Id " +
                    "join Seguridad.Usuario supervisor on Corresponsales.Agente.SupervisorId = supervisor.Id " +
                    "where Corresponsales.Agente.EstaActivo='true'",
                    (agente, estado, usuario, supervisor) =>
                    {
                        agente.Estado = estado;
                        agente.Usuario = usuario;
                        agente.Supervisor = supervisor;
                        return agente;
                    });
                return Agentes.ToList();
            }
        }
        public async Task<Agente> GetWithAssociations(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var agentes = await conexion.QueryAsync<Agente, Catalogo, Usuario, Usuario, Agente>(@"SELECT * FROM Corresponsales.Agente " +
                     "join Nomenclador.Catalogo estado on Corresponsales.Agente.EstadoId = estado.Id " +
                    "join Seguridad.Usuario usuario on Corresponsales.Agente.UsuarioId = usuario.Id " +
                    "join Seguridad.Usuario supervisor on Corresponsales.Agente.SupervisorId = supervisor.Id " +
                    "where Corresponsales.Agente.EstaActivo='true' and Corresponsales.Agente.Id = @Id",
                   (agente, estado, usuario, supervisor) =>
                   {
                       agente.Estado = estado;
                       agente.Usuario = usuario;
                       agente.Supervisor = supervisor;
                       return agente;
                   }, param: new { Id });

                return agentes.FirstOrDefault();
            }
        }

        public override async Task<Agente> Get(string Id)
        {
            return await Context.Agentes.FirstOrDefaultAsync(d => d.Id.ToString() == Id);
        }

        public override async Task Remove(Agente entidad)
        {
            var catalogo = _contexto.Set<Agente>().FirstOrDefault(o => o.Id == entidad.Id);
            catalogo.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public override async Task<string> Add(Agente entidad)
        {
            entidad.Estado = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.Estado.Id);
            entidad.Usuario = Context.Users.FirstOrDefault(c => c.Id == entidad.Usuario.Id);
            entidad.Supervisor = Context.Users.FirstOrDefault(c => c.Id == entidad.Supervisor.Id);
            entidad.EstaActivo = true;
            Context.Agentes.Add(entidad);
            await Context.SaveChangesAsync();
            return entidad.Id.ToString();
        }
        public override async Task Update(Agente entidad)
        {
            entidad.Estado = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.Estado.Id);
            entidad.Usuario = Context.Users.FirstOrDefault(c => c.Id == entidad.Usuario.Id);
            entidad.Supervisor = Context.Users.FirstOrDefault(c => c.Id == entidad.Supervisor.Id);
            _contexto.Entry(entidad).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosDisponibles()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var usuarios = await conexion.QueryAsync<Usuario>(@"SELECT Seguridad.Usuario.Id,Seguridad.Usuario.Codigo as UserName FROM Seguridad.Usuario " +
                    "left join Corresponsales.Agente on Seguridad.Usuario.Id = Corresponsales.Agente.UsuarioId " +
                    "where Seguridad.Usuario.EstaActivo='true' and Corresponsales.Agente.Id is null");
                return usuarios.ToList();
            }
        }

        public async Task<IEnumerable<Usuario>> GetSupervisoresDisponibles()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var usuarios = await conexion.QueryAsync<Usuario>(@"SELECT Seguridad.Usuario.Id,Seguridad.Usuario.Codigo as UserName FROM Seguridad.Usuario " +
                    "left join Corresponsales.Agente on Seguridad.Usuario.Id = Corresponsales.Agente.UsuarioId " +
                    "where Seguridad.Usuario.EstaActivo='true' and Corresponsales.Agente.Id is null");
                return usuarios.ToList();
            }
        }

        public async Task<Agente> GetForUserName(string userName)
        {
            return await _contexto.Set<Agente>().Where(r => r.EstaActivo == true)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Usuario.UserName == userName);
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
