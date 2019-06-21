using FBS_Core.Base.Infraestructure.Interfaces;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using System.Collections.Generic;

namespace FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador
{
    public interface ICatalogoRepository : IRepository<Catalogo>
    {
        IEnumerable<Catalogo> GetAllActive();
        IEnumerable<Catalogo> GetAllWithAssociations();
        Catalogo GetWithAssociations(int Id);

    }
}
