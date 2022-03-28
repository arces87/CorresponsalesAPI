using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales
{
    public interface IRepositorioAlerta : IRepositorio<Alerta>
    {
        Task<IEnumerable<Alerta>> GetAllActive();
        Task<IEnumerable<Alerta>> GetAllWithAssociations(string Estado, string TipoAlerta);
        Task<IEnumerable<Alerta>> GetForAgente(string IdAgente);
        Task<Alerta> GetWithAssociations(string Id);

    }
}
