using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Nomenclador;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador
{
    public interface IRepositorioTipoCatalogo : IRepositorio<TipoCatalogo>
    {
        Task<IEnumerable<TipoCatalogo>> GetAllActive();
    }
}
