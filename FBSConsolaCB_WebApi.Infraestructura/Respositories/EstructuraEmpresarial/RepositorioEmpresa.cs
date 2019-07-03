using FBS.Infraestructura.Repositorio;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.EstructuraEmpresarial
{
    public class RepositorioEmpresa : Repositorio<Empresa>, IRepositorioEmpresa
    {
        public RepositorioEmpresa(ContextoFBSConsolaCB context) : base(context)
        {

        }
        public async Task<IEnumerable<Empresa>> GetAllActive()
        {
            return await _contexto.Set<Empresa>().Where(a => a.EstaActivo == true).ToListAsync();
        }

        public override async Task Remove(Empresa entity)
        {
            entity.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }
    }
}
