using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial
{
    public interface IRepositorioEmpresa : IRepositorio<Empresa>
    {
        Task<IEnumerable<Empresa>> GetAllActive();
    }
}
