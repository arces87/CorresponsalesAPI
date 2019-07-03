using FBS.Infraestructura.Interfaces;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador
{
    public interface IRepositorioCatalogo : IRepositorio<Catalogo>
    {
        Task<IEnumerable<Catalogo>> GetAllActive();
        Task<IEnumerable<Catalogo>> GetAllWithAssociations();
        Task<Catalogo> GetWithAssociations(int Id);

    }
}
