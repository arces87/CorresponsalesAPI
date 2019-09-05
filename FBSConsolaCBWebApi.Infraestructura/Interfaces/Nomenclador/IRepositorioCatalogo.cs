using FBS.DAL.Nomenclador;
using FBS.Infraestructura.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador
{
    public interface IRepositorioCatalogo : IRepositorio<Catalogo>
    {
        Task<IEnumerable<Catalogo>> GetAllActive();
        Task<IEnumerable<Catalogo>> GetAllWithAssociations();
        Task<Catalogo> GetWithAssociations(int Id);

    }
}
