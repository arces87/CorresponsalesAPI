using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Consola;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola
{
    public interface IRepositorioAlerta : IRepositorio<Alerta>
    {
        Task<IEnumerable<Alerta>> GetAllActive();
    }
}
