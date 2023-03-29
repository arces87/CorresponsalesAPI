using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Canales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales
{
    public interface IRepositorioDispositivo : IRepositorio<Dispositivo>
    {
        Task<IEnumerable<Dispositivo>> GetAllActive();
        Task<IEnumerable<Dispositivo>> GetAllWithAssociations(bool? activo);
        Task<Dispositivo> GetWithAssociations(string Id);
        Task<bool> VerificarDispositivo(string Marca, string Modelo, string NoSerie);
        Task<bool> VerificarDispositivo(string Marca, string Modelo, string NoSerie, string IdDispositivo);
        Task<bool> VerificarImei(string imei, string IdDispositivo);
    }
}
