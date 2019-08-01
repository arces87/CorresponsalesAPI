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
            var oficina = _contexto.Set<Oficina>().FirstOrDefault(o => o.Id == entity.Id);
            oficina.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public override async Task<string> Add(Oficina entity)
        {
            entity.EstaActivo = true;
            entity.Empresa = Context.Empresas.FirstOrDefault(c => c.Id == entity.Empresa.Id);
            Context.Oficinas.Add(entity);
            await Context.SaveChangesAsync();
            return entity.Id.ToString();
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
