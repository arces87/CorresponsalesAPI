using FBS_Core.Base.Infraestructure.Repository;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Linq;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.EstructuraEmpresarial
{
    public class CargoRepository : Repository<Cargo>, ICargoRepository
    {
        public CargoRepository(FBSConsolaCBContext context) : base(context)
        {

        }
        public IEnumerable<Cargo> GetAllActive()
        {
            return _context.Set<Cargo>().Where(a => a.EstaActivo == true);
        }

        public override void Remove(Cargo entity)
        {
            entity.EstaActivo = false;
            _context.SaveChanges();
        }
    }
}
