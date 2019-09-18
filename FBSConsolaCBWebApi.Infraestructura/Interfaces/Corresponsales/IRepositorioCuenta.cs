using FBS.Identidad.DAL.Seguridad;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales
{
    public interface IRepositorioCuenta : IRepositorio<Cuenta>
    {
        Task<IEnumerable<Cuenta>> GetAllActive();
        Task<IEnumerable<Cuenta>> GetAllWithAssociations();
        Task<Cuenta> GetWithAssociations(string Id);
        Task<Cuenta> GetForAgente(string IdAgente);

    }
}
