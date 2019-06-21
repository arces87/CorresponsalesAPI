using FBS_Core.Base.Infraestructure.Interfaces;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial
{
    public interface ICorresponsalRepository : IRepository<Corresponsal>
    {
        IEnumerable<Corresponsal> GetAllActive();
        IEnumerable<Corresponsal> GetAllWithAssociations();
        Corresponsal GetWithAssociations(int Id);
    }
}
