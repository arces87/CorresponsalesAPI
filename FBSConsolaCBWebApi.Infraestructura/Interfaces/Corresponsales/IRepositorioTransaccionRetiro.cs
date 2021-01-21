using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales
{
    public interface IRepositorioTransaccionRetiro : IRepositorio<TransaccionRetiro>
    {        
        Task<IEnumerable<TransaccionRetiro>> GetAll();     
    }
}
