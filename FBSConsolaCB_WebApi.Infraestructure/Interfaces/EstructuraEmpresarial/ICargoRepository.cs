using FBS_Core.Base.Infraestructure.Interfaces;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial
{
    public interface ICargoRepository : IRepository<Cargo>
    {
        IEnumerable<Cargo> GetAllActive();
    }
}
