using FBS.Infraestructura.Interfaces;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial
{
    public interface IRepositorioEmpresa : IRepositorio<Empresa>
    {
        Task<IEnumerable<Empresa>> GetAllActive();
    }
}
