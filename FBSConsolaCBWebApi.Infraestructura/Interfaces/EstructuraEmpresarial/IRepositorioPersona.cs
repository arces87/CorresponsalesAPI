using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial
{
    public interface IRepositorioPersona : IRepositorio<Agente>
    {
        Task<IEnumerable<Agente>> GetAllActive();
        Task<IEnumerable<Agente>> GetAllWithAssociations();
        Task<Agente> GetForUserName(string userName);
    }
}
