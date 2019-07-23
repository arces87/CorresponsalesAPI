using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial
{
    public interface IRepositorioOficina : IRepositorio<Oficina>
    {
        Task<IEnumerable<Oficina>> GetAllActive();
        Task<IEnumerable<Oficina>> GetAllWithAssociations();
        Task<Oficina> GetWithAssociations(int Id);
    }
}
