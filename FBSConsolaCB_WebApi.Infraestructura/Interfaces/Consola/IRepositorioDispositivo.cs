using FBS.Infraestructura.Interfaces;
using FBSConsolaCB_WebApi.DAL.Consola;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Infraestructure.Interfaces.Consola
{
    public interface IRepositorioDispositivo : IRepositorio<Dispositivo>
    {
        Task<IEnumerable<Dispositivo>> GetAllActive();
        Task<IEnumerable<Dispositivo>> GetAllWithAssociations();
        Task<Dispositivo> GetWithAssociations(int Id);

    }
}
