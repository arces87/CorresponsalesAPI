using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Canales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales
{
    public interface IRepositorioImagenGeolocalizacion : IRepositorio<ImagenGeolocalizacion>
    {
        Task<IEnumerable<ImagenGeolocalizacion>> GetAllActive();
        Task<IEnumerable<ImagenGeolocalizacion>> GetAllWithAssociations();
        Task<ImagenGeolocalizacion> GetWithAssociations(string Id);
        Task<IEnumerable<ImagenGeolocalizacion>> GetForGeolocalizacion(string IdGeolocalizacion);
        Task RemoveAllForGeolocalizacion(string IdDispositivo);

    }
}
