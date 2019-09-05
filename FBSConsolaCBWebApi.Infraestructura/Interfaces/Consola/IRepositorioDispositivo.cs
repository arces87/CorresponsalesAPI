using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Canales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola
{
    public interface IRepositorioDispositivo : IRepositorio<Dispositivo>
    {
        Task<IEnumerable<Dispositivo>> GetAllActive();
        Task<IEnumerable<Dispositivo>> GetAllWithAssociations();
        Task<Dispositivo> GetWithAssociations(int Id);
        Task Asignar(int idDispositivo, int idCorresponsal);
        Task Desasignar(int idDispositivo, int idCorresponsal);

    }
}
