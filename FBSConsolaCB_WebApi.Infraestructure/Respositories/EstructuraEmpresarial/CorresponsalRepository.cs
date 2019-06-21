using FBS_Core.Base.Infraestructure.Repository;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.EstructuraEmpresarial
{
    public class CorresponsalRepository : Repository<Corresponsal>, ICorresponsalRepository
    {
        public CorresponsalRepository(FBSConsolaCBContext context) : base(context)
        {

        }
        public IEnumerable<Corresponsal> GetAllActive()
        {
            return _context.Set<Corresponsal>().Where(a => a.EstaActivo == true);
        }

        public IEnumerable<Corresponsal> GetAllWithAssociations()
        {
            return _context.Set<Corresponsal>().Where(r => r.EstaActivo == true)
                .Include(c => c.Oficina).Include(c => c.Cargo)
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Oficina)
                .Include(c => c.Usuario)
                .Include(c => c.AreaTrabajo);
        }

        public Corresponsal GetWithAssociations(int Id)
        {
            return _context.Set<Corresponsal>().Where(r => r.EstaActivo == true)
                .Include(c => c.Oficina).Include(c => c.Cargo)
                .Include(c => c.TipoIdentificacion)
                .Include(c => c.Usuario)
                .Include(c => c.Oficina).Include(c => c.AreaTrabajo).FirstOrDefault(c => c.Id == Id);
        }

        public override void Remove(Corresponsal entity)
        {
            entity.EstaActivo = false;
            _context.SaveChanges();
        }

        public override void Add(Corresponsal entity)
        {
            entity.Cargo = Context.Cargos.FirstOrDefault(c => c.Id == entity.Cargo.Id);
            entity.Usuario = Context.Users.FirstOrDefault(c => c.Id == entity.Usuario.Id);
            entity.Oficina = Context.Oficinas.FirstOrDefault(c => c.Id == entity.Oficina.Id);
            entity.TipoIdentificacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.TipoIdentificacion.Id);
            entity.AreaTrabajo = Context.AreasTrabajos.FirstOrDefault(c => c.Id == entity.AreaTrabajo.Id);
            Context.Corresponsales.Add(entity);
            Context.SaveChanges();
        }
        public override void Update(Corresponsal entity)
        {
            entity.Cargo = Context.Cargos.FirstOrDefault(c => c.Id == entity.Cargo.Id);
            entity.Usuario = Context.Users.FirstOrDefault(c => c.Id == entity.Usuario.Id);
            entity.Oficina = Context.Oficinas.FirstOrDefault(c => c.Id == entity.Oficina.Id);
            entity.TipoIdentificacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.TipoIdentificacion.Id);
            entity.AreaTrabajo = Context.AreasTrabajos.FirstOrDefault(c => c.Id == entity.AreaTrabajo.Id);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public FBSConsolaCBContext Context { get { return _context as FBSConsolaCBContext; } }
    }
}
