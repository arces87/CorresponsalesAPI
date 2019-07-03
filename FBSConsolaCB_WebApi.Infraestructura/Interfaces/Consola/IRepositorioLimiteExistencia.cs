using FBS.Infraestructura.Interfaces;
using FBSConsolaCB_WebApi.DAL.Consola;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Infraestructure.Interfaces.Consola
{
    public interface IRepositorioLimiteExistencia : IRepositorio<LimiteExistencia>
    {
        Task<IEnumerable<LimiteExistencia>> GetAllActive();
        Task<IEnumerable<LimiteExistencia>> GetAllWithAssociations();
        Task<LimiteExistencia> GetWithAssociations(int Id);

    }
}
