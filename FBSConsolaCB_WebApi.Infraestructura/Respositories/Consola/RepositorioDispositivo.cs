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
    public class RepositorioDispositivo : Repositorio<Dispositivo>, IRepositorioDispositivo
    {
        public RepositorioDispositivo(ContextoFBSConsolaCB context) : base(context)
        {

        }
        public async Task<IEnumerable<Dispositivo>> GetAllActive()
        {
            return await _contexto.Set<Dispositivo>().Where(a => a.EstaActivo == true).ToListAsync();
        }
        public async Task<IEnumerable<Dispositivo>> GetAllWithAssociations()
        {
            return await _contexto.Set<Dispositivo>().Where(a => a.EstaActivo == true)
                .Include(d => d.TipoDispositivo).ToListAsync();
        }
        public override async Task Add(Dispositivo entity)
        {
            entity.TipoDispositivo = Context.Catalogos.FirstOrDefault(c => c.Id == entity.TipoDispositivo.Id);
            entity.EstaActivo = true;
            Context.Dispositivos.Add(entity);
            await Context.SaveChangesAsync();
        }
        public override async Task Update(Dispositivo entity)
        {
            entity.TipoDispositivo = Context.Catalogos.FirstOrDefault(c => c.Id == entity.TipoDispositivo.Id);
            _contexto.Entry(entity).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();

        }

        public async Task<Dispositivo> GetWithAssociations(int Id)
        {
            return await _contexto.Set<Dispositivo>().Where(a => a.EstaActivo == true)
               .Include(d => d.TipoDispositivo).FirstOrDefaultAsync(c => c.Id == Id);
        }

        public override async Task Remove(Dispositivo entity)
        {
            entity.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public async Task Asignar(int idDispositivo, int idCorresponsal)
        {
            var dispositivo = Context.Dispositivos.FirstOrDefault(d => d.Id == idDispositivo);
            var corresponsal = Context.Corresponsales.FirstOrDefault(c => c.Id == idCorresponsal);
            _contexto.Set<DispositivoCorresponsal>().Add(new DispositivoCorresponsal() { Dispositivo = dispositivo, Corresponsal = corresponsal });
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSConsolaCB Context { get { return _contexto as ContextoFBSConsolaCB; } }
    }
}
