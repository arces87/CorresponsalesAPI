using FBS_Core.Base.Infraestructure.Repository;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Linq;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.EstructuraEmpresarial
{
    public class AreaTrabajoRepository : Repository<AreaTrabajo>, IAreaTrabajoRepository
    {
        public AreaTrabajoRepository(FBSConsolaCBContext context) : base(context)
        {

        }
        public IEnumerable<AreaTrabajo> GetAllActive()
        {
            return _context.Set<AreaTrabajo>().Where(a => a.EstaActivo == true);
        }

        public override void Remove(AreaTrabajo entity)
        {
            entity.EstaActivo = false;
            var _corresponsales = Contexto.Corresponsales.Where(o => o.AreaTrabajo == entity);
            foreach (var item in _corresponsales)
            {
                item.EstaActivo = false;
            }
            _context.SaveChanges();
        }

        public FBSConsolaCBContext Contexto { get { return _context as FBSConsolaCBContext; } }
    }
}
