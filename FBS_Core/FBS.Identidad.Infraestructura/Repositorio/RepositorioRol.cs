using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBS.Infraestructura.Repositorio;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FBS.Identidad.Infraestructura.Repositorio
{
    public class RepositorioRol : Repositorio<Rol>, IRepositorioRol
    {
        public RepositorioRol(ContextoFBSIdentidad context) : base(context)
        {

        }

        public override async Task<Rol> Get(string Id)
        {
            return await GetContext.Roles.FirstOrDefaultAsync(r => r.Id == Id);
        }
        public async Task<Rol> GetForName(string Name)
        {
            return await GetContext.Roles.FirstOrDefaultAsync(r => r.Name == Name);
        }
        public override async Task<string> Add(Rol entity)
        {
            entity.NormalizedName = entity.Name.ToUpper();
            _contexto.Set<Rol>().Add(entity);
            await _contexto.SaveChangesAsync();
            return entity.Id;
        }

        public override async Task Remove(Rol entity)
        {
            var rol = _contexto.Set<Rol>().FirstOrDefault(o => o.Id == entity.Id);
            rol.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }
        public async Task<IEnumerable<Rol>> GetAllActive()
        {
            return await GetContext.Roles.Where(m => m.EstaActivo == true).ToListAsync();
        }

        public ContextoFBSIdentidad GetContext
        {
            get { return _contexto as ContextoFBSIdentidad; }
        }

        public async Task AddMenu(RolMenu menu)
        {
            menu.Rol = GetContext.Roles.FirstOrDefault(r => r.Id == menu.Rol.Id);
            menu.Menu = GetContext.Menus.FirstOrDefault(r => r.Id == menu.Menu.Id);
            GetContext.RolesMenus.Add(menu);
            await GetContext.SaveChangesAsync();
        }

    }
}
