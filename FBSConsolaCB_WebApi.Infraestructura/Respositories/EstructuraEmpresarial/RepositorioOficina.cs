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
    public class RepositorioOficina : Repositorio<Oficina>, IRepositorioOficina
    {
        public RepositorioOficina(ContextoFBSConsolaCB context) : base(context)
        {

        }
        public async Task<IEnumerable<Oficina>> GetAllActive()
        {
            return await _contexto.Set<Oficina>().Where(a => a.EstaActivo == true).ToListAsync();
        }

        public async Task<IEnumerable<Oficina>> GetAllWithAssociations()
        {
            return await _contexto.Set<Oficina>().Where(a => a.EstaActivo == true).Include(o => o.Empresa).ToListAsync();
        }

        public async Task<Oficina> GetWithAssociations(int Id)
        {
            return await _contexto.Set<Oficina>().Where(a => a.EstaActivo == true)
                .Include(o => o.Empresa)
                .FirstOrDefaultAsync(o => o.Id == Id);
        }

        public override async Task Remove(Oficina entity)
        {
            entity.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public override async Task Add(Oficina entity)
        {
            entity.Empresa = Context.Empresas.FirstOrDefault(c => c.Id == entity.Empresa.Id);
            Context.Oficinas.Add(entity);
            await Context.SaveChangesAsync();
        }
        public override async Task Update(Oficina entity)
        {
            entity.Empresa = Context.Empresas.FirstOrDefault(c => c.Id == entity.Empresa.Id);
            _contexto.Entry(entity).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSConsolaCB Context { get { return _contexto as ContextoFBSConsolaCB; } }
    }
}
