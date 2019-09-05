using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Canales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola
{
    public interface IRepositorioLog : IRepositorio<Log>
    {
        Task<IEnumerable<Log>> GetAllActive();
        Task<IEnumerable<Log>> GetAllWithAssociations();
        Task<Log> GetWithAssociations(int Id);

    }
}
