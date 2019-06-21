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

    }
}
