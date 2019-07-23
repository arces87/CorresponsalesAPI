using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Consola;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola
{
    public interface IRepositorioLimiteExistencia : IRepositorio<LimiteExistencia>
    {
        Task<IEnumerable<LimiteExistencia>> GetAllActive();
        Task<IEnumerable<LimiteExistencia>> GetAllWithAssociations();
        Task<LimiteExistencia> GetWithAssociations(int Id);

    }
}
