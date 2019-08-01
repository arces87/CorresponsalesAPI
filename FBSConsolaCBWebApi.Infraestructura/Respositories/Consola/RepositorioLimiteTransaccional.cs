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
    public class RepositorioLimiteTransaccional : Repositorio<LimiteTransaccional>, IRepositorioLimiteTransaccional
    {
        public RepositorioLimiteTransaccional(ContextoFBSConsolaCB context) : base(context)
        {

        }
        public async Task<IEnumerable<LimiteTransaccional>> GetAllActive()
        {
            return await _contexto.Set<LimiteTransaccional>().Where(a => a.EstaActivo == true).ToListAsync();
        }
        public async Task<IEnumerable<LimiteTransaccional>> GetAllWithAssociations()
        {
            return await _contexto.Set<LimiteTransaccional>().Where(a => a.EstaActivo == true)
                .Include(c => c.Corresponsal).Include(d => d.Operacion).ToListAsync();
        }
        public override async Task<string> Add(LimiteTransaccional entity)
        {
            entity.Operacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Operacion.Id);
            entity.Corresponsal = Context.Corresponsales.FirstOrDefault(c => c.Id == entity.Corresponsal.Id);
            entity.EstaActivo = true;
            Context.LimitesTransaccionales.Add(entity);
            await Context.SaveChangesAsync();
            return entity.Id.ToString();
        }
        public override async Task Update(LimiteTransaccional entity)
        {
            entity.Operacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Operacion.Id);
            entity.Corresponsal = Context.Corresponsales.FirstOrDefault(c => c.Id == entity.Corresponsal.Id);
            _contexto.Entry(entity).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
            
        }

        public async Task<LimiteTransaccional> GetWithAssociations(int Id)
        {
            return await _contexto.Set<LimiteTransaccional>().Where(a => a.EstaActivo == true)
                .Include(c => c.Corresponsal).Include(d => d.Operacion).FirstOrDefaultAsync(c => c.Id == Id);
        }

        public override async Task Remove(LimiteTransaccional entity)
        {
            var limite = _contexto.Set<LimiteTransaccional>().FirstOrDefault(o => o.Id == entity.Id);
            limite.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSConsolaCB Context { get { return _contexto as ContextoFBSConsolaCB; } }
    }
}
