using FBS_Core.Base.Infraestructure.Repository;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Linq;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.EstructuraEmpresarial
{
    public class EmpresaRepository : Repository<Empresa>, IEmpresaRepository
    {
        public EmpresaRepository(FBSConsolaCBContext context) : base(context)
        {

        }
        public IEnumerable<Empresa> GetAllActive()
        {
            return _context.Set<Empresa>().Where(a => a.EstaActivo == true);
        }

        public override void Remove(Empresa entity)
        {
            entity.EstaActivo = false;
            _context.SaveChanges();
        }
    }
}
