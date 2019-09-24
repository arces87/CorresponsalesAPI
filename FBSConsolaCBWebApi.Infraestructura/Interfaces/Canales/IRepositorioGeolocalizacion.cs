using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Canales;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales
{
    public interface IRepositorioGeolocalizacion : IRepositorio<Geolocalizacion>
    {
        Task<Geolocalizacion> GetForAgente(string Id);
        Task AdicionarGeolocalizacionAgente(double latitud, double longitud, string idAgente);

    }
}
