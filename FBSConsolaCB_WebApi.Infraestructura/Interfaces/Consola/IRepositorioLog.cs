using FBS.Infraestructura.Interfaces;
using FBSConsolaCB_WebApi.DAL.Consola;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Infraestructure.Interfaces.Consola
{
    public interface IRepositorioLog : IRepositorio<Log>
    {
        Task<IEnumerable<Log>> GetAllActive();
        Task<IEnumerable<Log>> GetAllWithAssociations();
        Task<Log> GetWithAssociations(int Id);

    }
}
