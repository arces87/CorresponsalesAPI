using FBS.Infraestructura.Repositorio;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.DAL.Consola;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Consola;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.Consola
{
    public class RepositorioLog : Repositorio<Log>, IRepositorioLog
    {
        public RepositorioLog(ContextoFBSConsolaCB context) : base(context)
        {

        }
        public async Task<IEnumerable<Log>> GetAllActive()
        {
            return await _contexto.Set<Log>().Where(a => a.EstaActivo == true).ToListAsync();
        }
        public async Task<IEnumerable<Log>> GetAllWithAssociations()
        {
            return await _contexto.Set<Log>().Where(a => a.EstaActivo == true)
                .Include(c => c.Corresponsal).Include(d => d.Operacion).ToListAsync();
        }
        public async Task<Log> GetWithAssociations(int Id)
        {
            return await _contexto.Set<Log>().Where(a => a.EstaActivo == true)
                .Include(c => c.Corresponsal).Include(d => d.Operacion).FirstOrDefaultAsync(c => c.Id == Id);
        }
        public override async Task Add(Log entity)
        {
            entity.Operacion = Context.Catalogos.FirstOrDefault(c => c.Id == entity.Operacion.Id);
            entity.Corresponsal = Context.Corresponsales.FirstOrDefault(c => c.Id == entity.Corresponsal.Id);
            entity.EstaActivo = true;
            Context.Logs.Add(entity);
            await Context.SaveChangesAsync();
        }
       

        public ContextoFBSConsolaCB Context { get { return _contexto as ContextoFBSConsolaCB; } }
    }
}
