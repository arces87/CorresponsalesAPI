using FBS.Infraestructura.Interfaces;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial
{
    public interface IRepositorioOficina : IRepositorio<Oficina>
    {
        Task<IEnumerable<Oficina>> GetAllActive();
        Task<IEnumerable<Oficina>> GetAllWithAssociations();
        Task<Oficina> GetWithAssociations(int Id);
    }
}
