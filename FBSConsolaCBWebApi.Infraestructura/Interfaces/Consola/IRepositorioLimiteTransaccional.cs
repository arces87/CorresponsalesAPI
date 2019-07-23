using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Consola;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola
{
    public interface IRepositorioLimiteTransaccional : IRepositorio<LimiteTransaccional>
    {
        Task<IEnumerable<LimiteTransaccional>> GetAllActive();
        Task<IEnumerable<LimiteTransaccional>> GetAllWithAssociations();
        Task<LimiteTransaccional> GetWithAssociations(int Id);

    }
}
