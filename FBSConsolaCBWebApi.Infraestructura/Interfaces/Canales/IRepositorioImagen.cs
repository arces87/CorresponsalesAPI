using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Canales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales
{
    public interface IRepositorioImagen : IRepositorio<Imagen>
    {
        Task<IEnumerable<Imagen>> GetAllActive();
        Task<IEnumerable<Imagen>> GetAllWithAssociations();
        Task<Imagen> GetWithAssociations(string Id);
        Task<IEnumerable<Imagen>> GetForDispositivo(string IdDispositivo);
        Task RemoveAllForDispositivo(string IdDispositivo);

    }
}
