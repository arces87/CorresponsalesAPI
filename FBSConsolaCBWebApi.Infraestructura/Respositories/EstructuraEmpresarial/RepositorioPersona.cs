using FBS.Identidad.DAL.Seguridad;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.EstructuraEmpresarial
{
    public class RepositorioPersona : Repositorio<Persona>, IRepositorioPersona
    {
        public RepositorioPersona(ContextoFBSConsolaCB context) : base(context)
        {

        }
        public async Task<IEnumerable<Persona>> GetAllActive()
        {
            return await _contexto.Set<Persona>().Where(a => a.EstaActivo == true).ToListAsync();
        }

        public async Task<IEnumerable<Persona>> GetAllWithAssociations()
        {
            return await _contexto.Set<Persona>().Where(r => r.EstaActivo == true)
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Oficina)
                .Include(c => c.Usuario).ToListAsync();
        }

        public async Task<IEnumerable<Supervisor>> GetSupervisoresWithAssociations()
        {
            return await _contexto.Set<Supervisor>().Where(r => r.EstaActivo == true)
                .Include(c => c.Persona).ThenInclude(c => c.TipoIdentificacion)
                .Include(c => c.Persona).ThenInclude(c => c.Oficina)
                .Include(c => c.Persona).ThenInclude(c => c.Usuario).ToListAsync();
        }

        public async Task<IEnumerable<Corresponsal>> GetCorresponsalesWithAssociations()
        {
            return await _contexto.Set<Corresponsal>().Where(r => r.EstaActivo == true)
                .Include(c => c.Persona).ThenInclude(c => c.TipoIdentificacion)
                .Include(c => c.Persona).ThenInclude(c => c.Oficina)
                .Include(c => c.Persona).ThenInclude(c => c.Usuario).ToListAsync();
        }

        public async Task<object> GetWithAssociations(int Id)
        {
            var _persona = await _contexto.Set<Persona>().Where(r => r.EstaActivo == true)
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Usuario)
                .Include(c => c.Oficina).ThenInclude(o => o.Empresa)
                .FirstOrDefaultAsync(c => c.Id == Id);
            var _corresponsal = await _contexto.Set<Corresponsal>().Where(c => c.Id == Id)
                .Include(c => c.Supervisor)
                .ThenInclude(s => s.Persona).FirstOrDefaultAsync();
            if (_corresponsal != null)
            {
                _corresponsal.Persona = _persona;
                return _corresponsal;
            }
            else
            {
                var _supervisor = await _contexto.Set<Supervisor>().Where(s => s.Id == Id).FirstOrDefaultAsync();
                if (_supervisor != null)
                {
                    _supervisor.Persona = _persona;
                    return _supervisor;
                }
                else
                {
                    return _persona;
                }

            }
        }

        public async Task<Persona> GetForUserName(string userName)
        {
            return await _contexto.Set<Persona>().Where(r => r.EstaActivo == true)
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Usuario)
                .Include(c => c.Oficina).ThenInclude(o => o.Empresa)
                .FirstOrDefaultAsync(c => c.Usuario.UserName == userName);
        }

        public override async Task Remove(Persona entity)
        {
            entity.EstaActivo = false;
            var usuario = _contexto.Set<Usuario>().FirstOrDefault(u => u.Id == entity.Usuario.Id);
            usuario.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public override async Task Add(Persona entity)
        {
            entity.Oficina = Context.Oficinas.FirstOrDefault(c => c.Id == entity.Oficina.Id);
            entity.TipoIdentificacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.TipoIdentificacion.Id);
            entity.EstaActivo = true;
            Context.Personas.Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Add(Supervisor entity)
        {
            entity.Persona.Usuario = Context.Users.FirstOrDefault(c => c.Id == entity.Persona.Usuario.Id);
            entity.Persona.Oficina = Context.Oficinas.FirstOrDefault(c => c.Id == entity.Persona.Oficina.Id);
            entity.Persona.TipoIdentificacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Persona.TipoIdentificacion.Id);
            entity.EstaActivo = true;
            entity.Persona.EstaActivo = true;
            Context.Supervisores.Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Add(Corresponsal entity)
        {
            entity.Persona.Usuario = Context.Users.FirstOrDefault(c => c.Id == entity.Persona.Usuario.Id);
            entity.Persona.Oficina = Context.Oficinas.FirstOrDefault(c => c.Id == entity.Persona.Oficina.Id);
            entity.Persona.TipoIdentificacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Persona.TipoIdentificacion.Id);
            entity.Supervisor = Context.Supervisores.FirstOrDefault(c => c.Id == entity.Supervisor.Id);
            entity.EstaActivo = true;
            entity.Persona.Usuario.LockoutEnabled = true;
            entity.Persona.EstaActivo = true;
            Context.Corresponsales.Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Update(Supervisor entity)
        {
            entity.Persona.Usuario = Context.Users.FirstOrDefault(c => c.Id == entity.Persona.Usuario.Id);
            entity.Persona.Oficina = Context.Oficinas.FirstOrDefault(c => c.Id == entity.Persona.Oficina.Id);
            entity.Persona.TipoIdentificacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Persona.TipoIdentificacion.Id);
            _contexto.Entry(entity.Persona).State = EntityState.Modified;
            _contexto.Entry(entity).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public async Task Update(Corresponsal entity)
        {
            entity.Persona.Usuario = Context.Users.FirstOrDefault(c => c.Id == entity.Persona.Usuario.Id);
            entity.Persona.Oficina = Context.Oficinas.FirstOrDefault(c => c.Id == entity.Persona.Oficina.Id);
            entity.Persona.TipoIdentificacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Persona.TipoIdentificacion.Id);
            entity.Supervisor = Context.Supervisores.FirstOrDefault(c => c.Id == entity.Supervisor.Id);
            _contexto.Entry(entity.Persona).State = EntityState.Modified;
            _contexto.Entry(entity).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public async Task<Dispositivo> ObtenerDispositivo(int idCorresponsal)
        {
            var dispositivo = await Context.DispositivosCorresponsales.FirstOrDefaultAsync(c => c.Corresponsal.Id == idCorresponsal);
            if (dispositivo != null)
                return await Context.Dispositivos.FirstOrDefaultAsync(d => d.Id == dispositivo.Dispositivo.Id);
            return null;
        }

        public ContextoFBSConsolaCB Context { get { return _contexto as ContextoFBSConsolaCB; } }
    }
}
