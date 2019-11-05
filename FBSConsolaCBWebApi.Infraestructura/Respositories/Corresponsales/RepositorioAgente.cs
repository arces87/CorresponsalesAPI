using Dapper;
using FBS.DAL.Nomenclador;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.DAL.Seguridad;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.DAL.ModeloUsuario;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
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
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public RepositorioAgente(ContextoFBSConsolaCB context, IConfiguration configuracion, IJsonConfiguracion jsonConfiguracion) : base(context)
        {
            _configuracion = configuracion;
            _jsonConfiguracion = jsonConfiguracion;
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
        public async Task<IEnumerable<Agente>> GetAllWithAssociations(string IdSupervisor, string Estado)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                if (IdSupervisor != null && IdSupervisor != "")
                {
                    if (Estado != null && Estado != "")
                    {
                        var Agentes = await conexion.QueryAsync<Agente, Catalogo, Dispositivo, Catalogo, UsuarioDapper, UsuarioDapper, Agente>(@"SELECT Corresponsales.Agente.*, estado.*, dispositivo.*,marca.*, usuario.Id, usuario.Codigo,usuario.Imagen, " +
                        "supervisor.Id, supervisor.Codigo,supervisor.Imagen FROM Corresponsales.Agente " +
                        "left join Nomenclador.Catalogo estado on Corresponsales.Agente.EstadoId = estado.Id " +
                        "left join Canales.Dispositivo dispositivo on Corresponsales.Agente.DispositivoId = dispositivo.Id " +
                        "left join Nomenclador.Catalogo marca on dispositivo.MarcaId = marca.Id " +
                        "left join Seguridad.Usuario usuario on Corresponsales.Agente.UsuarioId = usuario.Id " +
                        "left join Seguridad.Usuario supervisor on Corresponsales.Agente.SupervisorId = supervisor.Id " +
                        "where Corresponsales.Agente.EstaActivo='true' and Corresponsales.Agente.SupervisorId = @IdSupervisor and Corresponsales.Agente.EstadoId = @Estado",
                       (agente, estado, dispositivo, marca, usuario, supervisor) =>
                       {
                           dispositivo.Marca = marca;
                           agente.Estado = estado;
                           if (usuario != null)
                               agente.Usuario = new Usuario() { Id = usuario.Id, UserName = usuario.Codigo, Imagen = usuario.Imagen };
                           if (supervisor != null)
                               agente.Supervisor = new Usuario() { Id = supervisor.Id, UserName = supervisor.Codigo, Imagen = supervisor.Imagen };
                           agente.Dispositivo = dispositivo;
                           return agente;
                       }, param: new { IdSupervisor, Estado });
                        return Agentes.ToList();
                    }
                    else
                    {
                        var Agentes = await conexion.QueryAsync<Agente, Catalogo, Dispositivo, Catalogo, UsuarioDapper, UsuarioDapper, Agente>(@"SELECT Corresponsales.Agente.*, estado.*, dispositivo.*,marca.*, usuario.Id, usuario.Codigo,usuario.Imagen, " +
                        "supervisor.Id, supervisor.Codigo,supervisor.Imagen FROM Corresponsales.Agente " +
                        "left join Nomenclador.Catalogo estado on Corresponsales.Agente.EstadoId = estado.Id " +
                        "left join Canales.Dispositivo dispositivo on Corresponsales.Agente.DispositivoId = dispositivo.Id " +
                        "left join Nomenclador.Catalogo marca on dispositivo.MarcaId = marca.Id " +
                        "left join Seguridad.Usuario usuario on Corresponsales.Agente.UsuarioId = usuario.Id " +
                        "left join Seguridad.Usuario supervisor on Corresponsales.Agente.SupervisorId = supervisor.Id " +
                        "where Corresponsales.Agente.EstaActivo='true' and Corresponsales.Agente.SupervisorId = @IdSupervisor",
                       (agente, estado, dispositivo, marca, usuario, supervisor) =>
                       {
                           dispositivo.Marca = marca;
                           agente.Estado = estado;
                           if (usuario != null)
                               agente.Usuario = new Usuario() { Id = usuario.Id, UserName = usuario.Codigo, Imagen = usuario.Imagen };
                           if (supervisor != null)
                               agente.Supervisor = new Usuario() { Id = supervisor.Id, UserName = supervisor.Codigo, Imagen = supervisor.Imagen };
                           agente.Dispositivo = dispositivo;
                           return agente;
                       }, param: new { IdSupervisor });
                        return Agentes.ToList();
                    }

                }
                else
                {
                    if (Estado != null && Estado != "")
                    {
                        var Agentes = await conexion.QueryAsync<Agente, Catalogo, Dispositivo, Catalogo, UsuarioDapper, UsuarioDapper, Agente>(@"SELECT Corresponsales.Agente.*, estado.*, dispositivo.*,marca.*, usuario.Id, usuario.Codigo,usuario.Imagen, " +
                          "supervisor.Id, supervisor.Codigo,supervisor.Imagen FROM Corresponsales.Agente " +
                          "left join Nomenclador.Catalogo estado on Corresponsales.Agente.EstadoId = estado.Id " +
                          "left join Canales.Dispositivo dispositivo on Corresponsales.Agente.DispositivoId = dispositivo.Id " +
                          "left join Nomenclador.Catalogo marca on dispositivo.MarcaId = marca.Id " +
                          "left join Seguridad.Usuario usuario on Corresponsales.Agente.UsuarioId = usuario.Id " +
                          "left join Seguridad.Usuario supervisor on Corresponsales.Agente.SupervisorId = supervisor.Id " +
                          "where Corresponsales.Agente.EstaActivo='true' and Corresponsales.Agente.EstadoId = @Estado",
                         (agente, estado, dispositivo, marca, usuario, supervisor) =>
                         {
                             dispositivo.Marca = marca;
                             agente.Estado = estado;
                             if (usuario != null)
                                 agente.Usuario = new Usuario() { Id = usuario.Id, UserName = usuario.Codigo, Imagen = usuario.Imagen };
                             if (supervisor != null)
                                 agente.Supervisor = new Usuario() { Id = supervisor.Id, UserName = supervisor.Codigo, Imagen = supervisor.Imagen };
                             agente.Dispositivo = dispositivo;
                             return agente;
                         }, param: new { Estado });
                        return Agentes.ToList();
                    }
                    else
                    {
                        var Agentes = await conexion.QueryAsync<Agente, Catalogo, Dispositivo, Catalogo, UsuarioDapper, UsuarioDapper, Agente>(@"SELECT Corresponsales.Agente.*, estado.*, dispositivo.*,marca.*, usuario.Id, usuario.Codigo,usuario.Imagen, " +
                      "supervisor.Id, supervisor.Codigo,supervisor.Imagen FROM Corresponsales.Agente " +
                      "left join Nomenclador.Catalogo estado on Corresponsales.Agente.EstadoId = estado.Id " +
                      "left join Canales.Dispositivo dispositivo on Corresponsales.Agente.DispositivoId = dispositivo.Id " +
                      "left join Nomenclador.Catalogo marca on dispositivo.MarcaId = marca.Id " +
                      "left join Seguridad.Usuario usuario on Corresponsales.Agente.UsuarioId = usuario.Id " +
                      "left join Seguridad.Usuario supervisor on Corresponsales.Agente.SupervisorId = supervisor.Id " +
                      "where Corresponsales.Agente.EstaActivo='true'",
                     (agente, estado, dispositivo, marca, usuario, supervisor) =>
                     {
                         dispositivo.Marca = marca;
                         agente.Estado = estado;
                         if (usuario != null)
                             agente.Usuario = new Usuario() { Id = usuario.Id, UserName = usuario.Codigo, Imagen = usuario.Imagen };
                         if (supervisor != null)
                             agente.Supervisor = new Usuario() { Id = supervisor.Id, UserName = supervisor.Codigo, Imagen = supervisor.Imagen };
                         agente.Dispositivo = dispositivo;
                         return agente;
                     });
                        return Agentes.ToList();
                    }

                }

            }
        }
        public async Task<Agente> GetWithAssociations(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var agentes = await conexion.QueryAsync<Agente, Catalogo, Dispositivo, Catalogo, UsuarioDapper, UsuarioDapper, Agente>(@"SELECT Corresponsales.Agente.*, estado.*, dispositivo.*, marca.*, usuario.Id, usuario.Codigo,usuario.Imagen, " +
                    "supervisor.Id, supervisor.Codigo,supervisor.Imagen FROM Corresponsales.Agente " +
                    "left join Nomenclador.Catalogo estado on Corresponsales.Agente.EstadoId = estado.Id " +
                    "left join Canales.Dispositivo dispositivo on Corresponsales.Agente.DispositivoId = dispositivo.Id " +
                    "left join Nomenclador.Catalogo marca on dispositivo.MarcaId = marca.Id " +
                    "left join Seguridad.Usuario usuario on Corresponsales.Agente.UsuarioId = usuario.Id " +
                    "left join Seguridad.Usuario supervisor on Corresponsales.Agente.SupervisorId = supervisor.Id " +
                    "where Corresponsales.Agente.EstaActivo='true' and Corresponsales.Agente.Id = @Id",
                   (agente, estado, dispositivo, marca, usuario, supervisor) =>
                   {
                       dispositivo.Marca = marca;
                       agente.Estado = estado;
                       if (usuario != null)
                           agente.Usuario = new Usuario() { Id = usuario.Id, UserName = usuario.Codigo, Imagen = usuario.Imagen };
                       if (supervisor != null)
                           agente.Supervisor = new Usuario() { Id = supervisor.Id, UserName = supervisor.Codigo, Imagen = supervisor.Imagen };
                       agente.Dispositivo = dispositivo;
                       return agente;
                   }, param: new { Id });

                return agentes.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Agente>> GetAllWithAssociationsConsola(string IdSupervisor)
        {
            using (var conexion = Conexion)
            {
                var IdEstadoActivo = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoActivo").Valor;
                var IdEstadoCobrando = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoCobrando").Valor;
                conexion.Open();
                if (IdSupervisor != null && IdSupervisor != "")
                {
                    var agentes = await conexion.QueryAsync<Agente, Catalogo, Dispositivo, Catalogo, UsuarioDapper, UsuarioDapper, Agente>(@"SELECT Corresponsales.Agente.*, estado.*, dispositivo.*, marca.*, usuario.Id, usuario.Codigo,usuario.Imagen, " +
                        "supervisor.Id, supervisor.Codigo, supervisor.Imagen FROM Corresponsales.Agente " +
                        "left join Nomenclador.Catalogo estado on Corresponsales.Agente.EstadoId = estado.Id " +
                        "left join Canales.Dispositivo dispositivo on Corresponsales.Agente.DispositivoId = dispositivo.Id " +
                        "left join Nomenclador.Catalogo marca on dispositivo.MarcaId = marca.Id " +
                        "left join Seguridad.Usuario usuario on Corresponsales.Agente.UsuarioId = usuario.Id " +
                        "left join Seguridad.Usuario supervisor on Corresponsales.Agente.SupervisorId = supervisor.Id " +
                        "where Corresponsales.Agente.EstaActivo='true' and (Corresponsales.Agente.EstadoId = @IdEstadoActivo or Corresponsales.Agente.EstadoId = @IdEstadoCobrando) " +
                        "and Corresponsales.Agente.SupervisorId = @IdSupervisor",
                       (agente, estado, dispositivo, marca, usuario, supervisor) =>
                       {
                           dispositivo.Marca = marca;
                           agente.Estado = estado;
                           if (usuario != null)
                               agente.Usuario = new Usuario() { Id = usuario.Id, UserName = usuario.Codigo, Imagen = usuario.Imagen };
                           if (supervisor != null)
                               agente.Supervisor = new Usuario() { Id = supervisor.Id, UserName = supervisor.Codigo, Imagen = supervisor.Imagen };
                           agente.Dispositivo = dispositivo;
                           return agente;
                       }, param: new { IdEstadoActivo, IdEstadoCobrando, IdSupervisor });
                    return agentes.ToList();
                }
                else
                {
                    var agentes = await conexion.QueryAsync<Agente, Catalogo, Dispositivo, Catalogo, UsuarioDapper, UsuarioDapper, Agente>(@"SELECT Corresponsales.Agente.*, estado.*, dispositivo.*, marca.*, usuario.Id, usuario.Codigo,usuario.Imagen, " +
                        "supervisor.Id, supervisor.Codigo, supervisor.Imagen FROM Corresponsales.Agente " +
                        "left join Nomenclador.Catalogo estado on Corresponsales.Agente.EstadoId = estado.Id " +
                        "left join Canales.Dispositivo dispositivo on Corresponsales.Agente.DispositivoId = dispositivo.Id " +
                        "left join Nomenclador.Catalogo marca on dispositivo.MarcaId = marca.Id " +
                        "left join Seguridad.Usuario usuario on Corresponsales.Agente.UsuarioId = usuario.Id " +
                        "left join Seguridad.Usuario supervisor on Corresponsales.Agente.SupervisorId = supervisor.Id " +
                        "where Corresponsales.Agente.EstaActivo='true' and (Corresponsales.Agente.EstadoId = @IdEstadoActivo or Corresponsales.Agente.EstadoId = @IdEstadoCobrando)",
                        (agente, estado, dispositivo, marca, usuario, supervisor) =>
                        {
                            dispositivo.Marca = marca;
                            agente.Estado = estado;
                            if (usuario != null)
                                agente.Usuario = new Usuario() { Id = usuario.Id, UserName = usuario.Codigo, Imagen = usuario.Imagen };
                            if (supervisor != null)
                                agente.Supervisor = new Usuario() { Id = supervisor.Id, UserName = supervisor.Codigo, Imagen = supervisor.Imagen };
                            agente.Dispositivo = dispositivo;
                            return agente;
                        }, param: new { IdEstadoActivo, IdEstadoCobrando });
                    return agentes.ToList();
                }

            }
        }

        public async Task<IEnumerable<Agente>> GetForActivation(string IdSupervisor)
        {
            using (var conexion = Conexion)
            {
                var IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoUbicado").Valor;
                conexion.Open();
                if (IdSupervisor != null && IdSupervisor != "")
                {
                    var agentes = await conexion.QueryAsync<Agente, Catalogo, Dispositivo, Catalogo, UsuarioDapper, UsuarioDapper, Agente>(@"SELECT Corresponsales.Agente.*, estado.*, dispositivo.*, marca.*, usuario.Id, usuario.Codigo,usuario.Imagen, " +
                        "supervisor.Id, supervisor.Codigo, supervisor.Imagen FROM Corresponsales.Agente " +
                        "left join Nomenclador.Catalogo estado on Corresponsales.Agente.EstadoId = estado.Id " +
                        "left join Canales.Dispositivo dispositivo on Corresponsales.Agente.DispositivoId = dispositivo.Id " +
                        "left join Nomenclador.Catalogo marca on dispositivo.MarcaId = marca.Id " +
                        "left join Seguridad.Usuario usuario on Corresponsales.Agente.UsuarioId = usuario.Id " +
                        "left join Seguridad.Usuario supervisor on Corresponsales.Agente.SupervisorId = supervisor.Id " +
                        "where Corresponsales.Agente.EstaActivo='true' and Corresponsales.Agente.EstadoId = @IdEstado and Corresponsales.Agente.SupervisorId = @IdSupervisor",
                       (agente, estado, dispositivo, marca, usuario, supervisor) =>
                       {
                           dispositivo.Marca = marca;
                           agente.Estado = estado;
                           if (usuario != null)
                               agente.Usuario = new Usuario() { Id = usuario.Id, UserName = usuario.Codigo, Imagen = usuario.Imagen };
                           if (supervisor != null)
                               agente.Supervisor = new Usuario() { Id = supervisor.Id, UserName = supervisor.Codigo, Imagen = supervisor.Imagen };
                           agente.Dispositivo = dispositivo;
                           return agente;
                       }, param: new { IdEstado, IdSupervisor });
                    return agentes.ToList();
                }
                else
                {
                    var agentes = await conexion.QueryAsync<Agente, Catalogo, Dispositivo, Catalogo, UsuarioDapper, UsuarioDapper, Agente>(@"SELECT Corresponsales.Agente.*, estado.*, dispositivo.*, marca.*, usuario.Id, usuario.Codigo,usuario.Imagen, " +
                        "supervisor.Id, supervisor.Codigo, supervisor.Imagen FROM Corresponsales.Agente " +
                        "left join Nomenclador.Catalogo estado on Corresponsales.Agente.EstadoId = estado.Id " +
                        "left join Canales.Dispositivo dispositivo on Corresponsales.Agente.DispositivoId = dispositivo.Id " +
                        "left join Nomenclador.Catalogo marca on dispositivo.MarcaId = marca.Id " +
                        "left join Seguridad.Usuario usuario on Corresponsales.Agente.UsuarioId = usuario.Id " +
                        "left join Seguridad.Usuario supervisor on Corresponsales.Agente.SupervisorId = supervisor.Id " +
                        "where Corresponsales.Agente.EstaActivo='true' and Corresponsales.Agente.EstadoId = @IdEstado",
                        (agente, estado, dispositivo, marca, usuario, supervisor) =>
                        {
                            dispositivo.Marca = marca;
                            agente.Estado = estado;
                            if (usuario != null)
                                agente.Usuario = new Usuario() { Id = usuario.Id, UserName = usuario.Codigo, Imagen = usuario.Imagen };
                            if (supervisor != null)
                                agente.Supervisor = new Usuario() { Id = supervisor.Id, UserName = supervisor.Codigo, Imagen = supervisor.Imagen };
                            agente.Dispositivo = dispositivo;
                            return agente;
                        }, param: new { IdEstado });
                    return agentes.ToList();
                }

            }
        }

        public override async Task<Agente> Get(string Id)
        {
            return await Context.Agentes.FirstOrDefaultAsync(d => d.Id.ToString() == Id);
        }

        public override async Task Remove(Agente agente)
        {
            var IdEstadoActivo = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoActivo").Valor;
            var IdEstadoEliminado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoEliminado").Valor;
            var entidad = Context.Agentes.Include(a => a.Estado).FirstOrDefault(o => o.Id == agente.Id);
            if (entidad.Id.ToString() == IdEstadoEliminado)
                entidad.Estado = Context.Catalogos.FirstOrDefault(c => c.Id.ToString() == IdEstadoActivo);
            else
                entidad.Estado = Context.Catalogos.FirstOrDefault(c => c.Id.ToString() == IdEstadoEliminado);
            _contexto.Entry(entidad).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public override async Task<string> Add(Agente entidad)
        {
            var IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoRegistrado").Valor;
            entidad.Estado = Context.Catalogos.FirstOrDefault(c => c.Id == new Guid(IdEstado));
            entidad.Dispositivo = Context.Dispositivos.FirstOrDefault(c => c.Id == entidad.Dispositivo.Id);
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
            entidad.Dispositivo = Context.Dispositivos.FirstOrDefault(c => c.Id == entidad.Dispositivo.Id);
            entidad.Usuario = Context.Users.FirstOrDefault(c => c.Id == entidad.Usuario.Id);
            entidad.Supervisor = Context.Users.FirstOrDefault(c => c.Id == entidad.Supervisor.Id);
            _contexto.Entry(entidad).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }
        public async Task UpdateEstado(Agente entidad)
        {
            var agente = Context.Agentes.Include(a => a.Estado)
                .Include(a => a.Dispositivo)
                .Include(a => a.Usuario)
                .Include(a => a.Supervisor).FirstOrDefault(a => a.Id == entidad.Id);
            agente.Estado = Context.Catalogos.FirstOrDefault(c => c.Id == entidad.Estado.Id);
            _contexto.Entry(agente).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public async Task Activar(string Id)
        {
            var IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "AgenteIdEstadoActivo").Valor;
            var entidad = Context.Agentes.FirstOrDefault(c => c.Id == new Guid(Id));
            entidad.Estado = Context.Catalogos.FirstOrDefault(c => c.Id == new Guid(IdEstado));
            _contexto.Entry(entidad).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosDisponibles()
        {
            using (var conexion = Conexion)
            {
                var idRol = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdRolAgente").Valor;
                conexion.Open();
                var usuarios = await conexion.QueryAsync<Usuario>(@"SELECT Seguridad.Usuario.Id,Seguridad.Usuario.Codigo as UserName FROM Seguridad.Usuario " +
                    "left join Corresponsales.Agente on Seguridad.Usuario.Id = Corresponsales.Agente.UsuarioId " +
                    "left join Seguridad.UsuarioRol on Seguridad.Usuario.Id = Seguridad.UsuarioRol.UsuarioId " +
                    "where Seguridad.Usuario.EstaActivo='true' and Corresponsales.Agente.Id is null and Seguridad.UsuarioRol.RolId =@IdRol",
                    param: new { IdRol = idRol });
                return usuarios.ToList();
            }
        }

        public async Task<IEnumerable<Dispositivo>> GetDispositivosDisponibles()
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var dispositivos = await conexion.QueryAsync<Dispositivo, Catalogo, Dispositivo>(@"SELECT * FROM Canales.Dispositivo " +
                    "left join Corresponsales.Agente on Canales.Dispositivo.Id = Corresponsales.Agente.DispositivoId " +
                    "left join Nomenclador.Catalogo on Canales.Dispositivo.MarcaId = Nomenclador.Catalogo.Id " +
                    "where Canales.Dispositivo.EstaActivo='true' and Corresponsales.Agente.Id is null", (dispositivo, catalogo) =>
                    {
                        dispositivo.Marca = catalogo;
                        return dispositivo;
                    });
                return dispositivos.ToList();
            }
        }

        public async Task<IEnumerable<Usuario>> GetSupervisoresDisponibles()
        {
            using (var conexion = Conexion)
            {
                var idRol = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdRolSupervisor").Valor;
                conexion.Open();
                var usuarios = await conexion.QueryAsync<Usuario>(@"SELECT Seguridad.Usuario.Id,Seguridad.Usuario.Codigo as UserName FROM Seguridad.Usuario " +
                    "left join Seguridad.UsuarioRol on Seguridad.Usuario.Id = Seguridad.UsuarioRol.UsuarioId " +
                    "where Seguridad.Usuario.EstaActivo='true' and Seguridad.UsuarioRol.RolId =@IdRol",
                    param: new { IdRol = idRol });
                return usuarios.ToList();
            }
        }

        public async Task<Agente> GetForUserName(string userName)
        {
            return await _contexto.Set<Agente>().Where(r => r.EstaActivo == true)
                .Include(c => c.Usuario).Include(c => c.Dispositivo).Include(c => c.Estado)
                .FirstOrDefaultAsync(c => c.Usuario.UserName == userName);
        }

        public async Task<Agente> GetForId(string IdUsuario)
        {
            return await _contexto.Set<Agente>().Where(r => r.EstaActivo == true)
                .Include(c => c.Usuario).Include(c => c.Dispositivo).Include(c => c.Estado)
                .FirstOrDefaultAsync(c => c.Usuario.Id == IdUsuario);
        }
        public async Task<bool> Verificaridentificacion(string Identificacion)
        {
            var usuario = await _contexto.Set<Agente>()
                .FirstOrDefaultAsync(c => c.Identificacion == Identificacion);
            return usuario != null ? true : false;
        }
        public async Task<bool> Verificaridentificacion(string Identificacion, string idAgente)
        {
            var usuario = await _contexto.Set<Agente>()
                .FirstOrDefaultAsync(c => c.Identificacion == Identificacion && c.Id.ToString() != idAgente);
            return usuario != null ? true : false;
        }

        public ContextoFBSConsolaCB Context => _contexto as ContextoFBSConsolaCB;

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
