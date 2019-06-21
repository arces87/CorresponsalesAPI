using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador;
using System.Collections.Generic;
using System.Linq;
using FBS_Core.Base.Infraestructure.Repository;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using FBSConsolaCB_WebApi.DAL;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.Nomenclador
{
    public class TipoCatalogoRepository : Repository<TipoCatalogo>, ITipoCatalogoRepository
    {
        public TipoCatalogoRepository(FBSConsolaCBContext context) : base(context)
        {

        }
        public IEnumerable<TipoCatalogo> GetAllActive()
        {
            return _context.Set<TipoCatalogo>().Where(a => a.EstaActivo == true);
        }

        public override void Remove(TipoCatalogo entity)
        {
            entity.EstaActivo = false;
            _context.SaveChanges();
        }
    }
}
