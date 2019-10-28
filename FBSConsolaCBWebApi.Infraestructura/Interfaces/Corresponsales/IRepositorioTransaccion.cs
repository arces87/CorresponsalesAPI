using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales
{
    public interface IRepositorioTransaccion : IRepositorio<Transaccion>
    {
        Task<IEnumerable<Transaccion>> GetAllActive();
        Task<IEnumerable<Transaccion>> GetAllWithAssociations(string IdAgente);
        Task<IEnumerable<Transaccion>> GetForAgente(string Id);
        Task<IEnumerable<Transaccion>> GetForTipo(string idTipo, string IdAgente);
        Task<Transaccion> GetWithAssociations(string Id);
        Task<double> GetSaldoActual(string IdAgente);
        Task<double> GetSaldoCuenta(string IdAgente);
        Task ReponerTransaccion(string IdAgente);

    }
}
