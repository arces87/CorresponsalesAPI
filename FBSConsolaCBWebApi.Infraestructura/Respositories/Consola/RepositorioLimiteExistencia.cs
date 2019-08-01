using FBS.Infraestructura.Repositorio;
using FBSConsolaCBWebApi.DAL;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Repositories.Consola
{
    public class RepositorioLimiteExistencia : Repositorio<LimiteExistencia>, IRepositorioLimiteExistencia
    {
        public RepositorioLimiteExistencia(ContextoFBSConsolaCB context) : base(context)
        {

        }
        public async Task<IEnumerable<LimiteExistencia>> GetAllActive()
        {
            return await _contexto.Set<LimiteExistencia>().Where(a => a.EstaActivo == true).ToListAsync();
        }
        public async Task<IEnumerable<LimiteExistencia>> GetAllWithAssociations()
        {
            return await _contexto.Set<LimiteExistencia>().Where(a => a.EstaActivo == true)
                .Include(c => c.Corresponsal).ToListAsync();
        }
        public override async Task<string> Add(LimiteExistencia entity)
        {
            entity.Corresponsal = Context.Corresponsales.FirstOrDefault(c => c.Id == entity.Corresponsal.Id);
            entity.EstaActivo = true;
            Context.LimitesExistencias.Add(entity);
            await Context.SaveChangesAsync();
            return entity.Id.ToString();
        }
        public override async Task Update(LimiteExistencia entity)
        {
            entity.Corresponsal = Context.Corresponsales.FirstOrDefault(c => c.Id == entity.Corresponsal.Id);
            _contexto.Entry(entity).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
            
        }

        public async Task<LimiteExistencia> GetWithAssociations(int Id)
        {
            return await _contexto.Set<LimiteExistencia>().Where(a => a.EstaActivo == true)
                .Include(c => c.Corresponsal).FirstOrDefaultAsync(c => c.Id == Id);
        }

        public override async Task Remove(LimiteExistencia entity)
        {
            var limite = _contexto.Set<LimiteExistencia>().FirstOrDefault(o => o.Id == entity.Id);
            limite.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSConsolaCB Context { get { return _contexto as ContextoFBSConsolaCB; } }
    }
}
