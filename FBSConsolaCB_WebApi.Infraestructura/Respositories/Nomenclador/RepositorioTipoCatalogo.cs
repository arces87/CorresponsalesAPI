using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador;
using System.Collections.Generic;
using System.Linq;
using FBS.Infraestructura.Repositorio;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using FBSConsolaCB_WebApi.DAL;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.Nomenclador
{
    public class RepositorioTipoCatalogo : Repositorio<TipoCatalogo>, IRepositorioTipoCatalogo
    {
        public RepositorioTipoCatalogo(ContextoFBSConsolaCB context) : base(context)
        {

        }
        public async Task<IEnumerable<TipoCatalogo>> GetAllActive()
        {
            return await _contexto.Set<TipoCatalogo>().Where(a => a.EstaActivo == true).ToListAsync();
        }

        public override async Task Remove(TipoCatalogo entity)
        {
            entity.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }
    }
}
