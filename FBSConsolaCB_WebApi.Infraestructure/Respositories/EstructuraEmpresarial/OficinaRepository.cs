using FBS_Core.Base.Infraestructure.Repository;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.EstructuraEmpresarial
{
    public class OficinaRepository : Repository<Oficina>, IOficinaRepository
    {
        public OficinaRepository(FBSConsolaCBContext context) : base(context)
        {

        }
        public IEnumerable<Oficina> GetAllActive()
        {
            return _context.Set<Oficina>().Where(a => a.EstaActivo == true);
        }

        public IEnumerable<Oficina> GetAllWithAssociations()
        {
            return _context.Set<Oficina>().Where(a => a.EstaActivo == true).Include(o => o.Empresa);
        }

        public Oficina GetWithAssociations(int Id)
        {
            return _context.Set<Oficina>().Where(a => a.EstaActivo == true)
                .Include(o => o.Empresa)
                .FirstOrDefault(o => o.Id == Id);
        }

        public override void Remove(Oficina entity)
        {
            entity.EstaActivo = false;
            _context.SaveChanges();
        }

        public override void Add(Oficina entity)
        {
            entity.Empresa = Context.Empresas.FirstOrDefault(c => c.Id == entity.Empresa.Id);
            Context.Oficinas.Add(entity);
            Context.SaveChanges();
        }
        public override void Update(Oficina entity)
        {
            entity.Empresa = Context.Empresas.FirstOrDefault(c => c.Id == entity.Empresa.Id);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public FBSConsolaCBContext Context { get { return _context as FBSConsolaCBContext; } }
    }
}
