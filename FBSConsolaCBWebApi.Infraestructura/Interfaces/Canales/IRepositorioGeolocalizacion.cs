using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Canales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales
{
    public interface IRepositorioGeolocalizacion : IRepositorio<Geolocalizacion>
    {
        Task<Geolocalizacion> GetForAgente(string Id);

    }
}
