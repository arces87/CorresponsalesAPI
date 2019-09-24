using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Canales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales
{
    public interface IRepositorioLog : IRepositorio<Log>
    {
        Task<IEnumerable<Log>> GetAllActive();
        Task<IEnumerable<Log>> GetAllWithAssociations();
        Task<Log> GetWithAssociations(string Id);

    }
}
