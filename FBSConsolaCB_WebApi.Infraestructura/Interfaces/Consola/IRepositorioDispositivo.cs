using FBS.Infraestructura.Interfaces;
using FBSConsolaCB_WebApi.DAL.Consola;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Infraestructure.Interfaces.Consola
{
    public interface IRepositorioDispositivo : IRepositorio<Dispositivo>
    {
        Task<IEnumerable<Dispositivo>> GetAllActive();
        Task<IEnumerable<Dispositivo>> GetAllWithAssociations();
        Task<Dispositivo> GetWithAssociations(int Id);
        Task Asignar(int idDispositivo, int idCorresponsal);
        Task Desasignar(int idDispositivo, int idCorresponsal);
        Task<Corresponsal> ObtenerCorresponsal(int idDispositivo);

    }
}
