using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.EstructuraEmpresarial
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
        public override async Task Add(Empresa entity)
        {
            entity.EstaActivo = true;
            _contexto.Set<Empresa>().Add(entity);
            await _contexto.SaveChangesAsync();
        }
        public override async Task Remove(Empresa entity)
        {
            entity.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }
    }
}
