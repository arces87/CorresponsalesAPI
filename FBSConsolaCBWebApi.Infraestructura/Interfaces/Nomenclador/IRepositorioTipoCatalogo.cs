using FBS.DAL.Nomenclador;
using FBS.Infraestructura.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador
{
    public interface IRepositorioTipoCatalogo : IRepositorio<TipoCatalogo>
    {
        Task<IEnumerable<TipoCatalogo>> GetAllActive();
    }
}
