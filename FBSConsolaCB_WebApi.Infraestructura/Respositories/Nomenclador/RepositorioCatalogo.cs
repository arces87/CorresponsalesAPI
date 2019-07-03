using FBS.Infraestructura.Repositorio;
using FBSConsolaCB_WebApi.DAL;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Infraestructure.Repositories.Nomenclador
{
    public class RepositorioCatalogo : Repositorio<Catalogo>, IRepositorioCatalogo
    {
        public RepositorioCatalogo(ContextoFBSConsolaCB context) : base(context)
        {

        }
        public async Task<IEnumerable<Catalogo>> GetAllActive()
        {
            return await _contexto.Set<Catalogo>().Where(a => a.EstaActivo == true).ToListAsync();
        }
        public async Task<IEnumerable<Catalogo>> GetAllWithAssociations()
        {
            return await _contexto.Set<Catalogo>().Where(a => a.EstaActivo == true)
                .Include(c => c.TipoCatalogo).ToListAsync();
        }
        public async Task<Catalogo> GetWithAssociations(int Id)
        {
            return await _contexto.Set<Catalogo>().Where(a => a.EstaActivo == true)
                .Include(c => c.TipoCatalogo).FirstOrDefaultAsync(c => c.Id == Id);
        }

        public override async Task Remove(Catalogo entity)
        {
            entity.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public override async Task Add(Catalogo entity)
        {
            entity.TipoCatalogo = Context.TiposCatalogos.FirstOrDefault(c => c.Id == entity.TipoCatalogo.Id);
            Context.Catalogos.Add(entity);
            await Context.SaveChangesAsync();
        }
        public override async Task Update(Catalogo entity)
        {
            entity.TipoCatalogo = Context.TiposCatalogos.FirstOrDefault(c => c.Id == entity.TipoCatalogo.Id);
            _contexto.Entry(entity).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSConsolaCB Context { get { return _contexto as ContextoFBSConsolaCB; } }
    }
}
