using FBS_Core.Base.Infraestructure.Repository;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.EstructuraEmpresarial
{
    public class PersonaRepository : Repository<Persona>, IPersonaRepository
    {
        public PersonaRepository(FBSConsolaCBContext context) : base(context)
        {

        }
        public IEnumerable<Persona> GetAllActive()
        {
            return _context.Set<Persona>().Where(a => a.EstaActivo == true);
        }

        public IEnumerable<Persona> GetAllWithAssociations()
        {
            return _context.Set<Persona>().Where(r => r.EstaActivo == true)
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Oficina)
                .Include(c => c.Usuario);
        }

        public object GetWithAssociations(int Id)
        {
            var _persona = _context.Set<Persona>().Where(r => r.EstaActivo == true)
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Usuario)
                .Include(c => c.Oficina).ThenInclude(o => o.Empresa)
                .FirstOrDefault(c => c.Id == Id);
            var _corresponsal = _context.Set<Corresponsal>().Where(c => c.Id == Id)
                .Include(c => c.Supervisor)
                .ThenInclude(s => s.Persona).FirstOrDefault();
            if (_corresponsal != null)
            {
                _corresponsal.Persona = _persona;
                return _corresponsal;
            }
            else
            {
                var _supervisor = _context.Set<Supervisor>().Where(s => s.Id == Id).FirstOrDefault();
                _supervisor.Persona = _persona;
                return _supervisor;
            }
        }

        public Persona GetForUserName(string userName)
        {
            return _context.Set<Persona>().Where(r => r.EstaActivo == true)
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Usuario)
                .Include(c => c.Oficina).ThenInclude(o => o.Empresa)
                .FirstOrDefault(c => c.Usuario.UserName == userName);
        }

        public override void Remove(Persona entity)
        {
            entity.EstaActivo = false;
            _context.SaveChanges();
        }

        public override void Add(Persona entity)
        {
            entity.Usuario = Context.Users.FirstOrDefault(c => c.Id == entity.Usuario.Id);
            entity.Oficina = Context.Oficinas.FirstOrDefault(c => c.Id == entity.Oficina.Id);
            entity.TipoIdentificacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.TipoIdentificacion.Id);
            entity.EstaActivo = true;
            Context.Personas.Add(entity);
            Context.SaveChanges();
        }

        public void Add(Supervisor entity)
        {
            entity.Persona.Usuario = Context.Users.FirstOrDefault(c => c.Id == entity.Persona.Usuario.Id);
            entity.Persona.Oficina = Context.Oficinas.FirstOrDefault(c => c.Id == entity.Persona.Oficina.Id);
            entity.Persona.TipoIdentificacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Persona.TipoIdentificacion.Id);
            entity.EstaActivo = true;
            entity.Persona.EstaActivo = true;
            Context.Supervisores.Add(entity);
            Context.SaveChanges();
        }

        public void Add(Corresponsal entity)
        {
            entity.Persona.Usuario = Context.Users.FirstOrDefault(c => c.Id == entity.Persona.Usuario.Id);
            entity.Persona.Oficina = Context.Oficinas.FirstOrDefault(c => c.Id == entity.Persona.Oficina.Id);
            entity.Persona.TipoIdentificacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Persona.TipoIdentificacion.Id);
            entity.Supervisor = Context.Supervisores.FirstOrDefault(c => c.Id == entity.Supervisor.Id);
            entity.EstaActivo = true;
            entity.Persona.EstaActivo = true;
            Context.Corresponsales.Add(entity);
            Context.SaveChanges();
        }

        public void Update(Supervisor entity)
        {
            entity.Persona.Usuario = Context.Users.FirstOrDefault(c => c.Id == entity.Persona.Usuario.Id);
            entity.Persona.Oficina = Context.Oficinas.FirstOrDefault(c => c.Id == entity.Persona.Oficina.Id);
            entity.Persona.TipoIdentificacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Persona.TipoIdentificacion.Id);
            _context.Entry(entity.Persona).State = EntityState.Modified;
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Update(Corresponsal entity)
        {
            entity.Persona.Usuario = Context.Users.FirstOrDefault(c => c.Id == entity.Persona.Usuario.Id);
            entity.Persona.Oficina = Context.Oficinas.FirstOrDefault(c => c.Id == entity.Persona.Oficina.Id);
            entity.Persona.TipoIdentificacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Persona.TipoIdentificacion.Id);
            entity.Supervisor = Context.Supervisores.FirstOrDefault(c => c.Id == entity.Supervisor.Id);
            _context.Entry(entity.Persona).State = EntityState.Modified;
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public FBSConsolaCBContext Context { get { return _context as FBSConsolaCBContext; } }
    }
}
