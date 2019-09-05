using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.EstructuraEmpresarial
{
    public class RepositorioPersona : Repositorio<Agente>, IRepositorioPersona
    {
        public RepositorioPersona(ContextoFBSConsolaCB context) : base(context)
        {

        }
        public async Task<IEnumerable<Agente>> GetAllActive()
        {
            return await _contexto.Set<Agente>().Where(a => a.EstaActivo == true).ToListAsync();
        }

        public async Task<IEnumerable<Agente>> GetAllWithAssociations()
        {
            return await _contexto.Set<Agente>().Where(r => r.EstaActivo == true)
                .Include(c => c.Usuario).ToListAsync();
        }



        public async Task<Agente> GetForUserName(string userName)
        {
            return await _contexto.Set<Agente>().Where(r => r.EstaActivo == true)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Usuario.UserName == userName);
        }


        public ContextoFBSConsolaCB Context { get { return _contexto as ContextoFBSConsolaCB; } }
    }
}
