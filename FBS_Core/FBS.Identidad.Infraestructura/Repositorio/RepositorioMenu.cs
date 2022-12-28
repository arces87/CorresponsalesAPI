using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FBS.Infraestructura.Repositorio;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace FBS.Identidad.Infraestructura.Repositorio
{
    public class RepositorioMenu : Repositorio<Menu>, IRepositorioMenu
    {
        private readonly IConfiguration _configuracion;

        public RepositorioMenu(ContextoFBSIdentidad context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
        }
        public async Task<IEnumerable<Menu>> GetAllActive()
        {
            var menus = await GetContext.Menus.Where(m => m.EstaActivo == true).OrderBy(m => m.Orden).ToListAsync();
            return menus.Where(m => m.MenuId == null);
        }

        public async Task<IEnumerable<Menu>> GetAllWithAssociations()
        {
            var menus = await GetContext.Menus.Include(m => m.Menus).Where(m => m.EstaActivo == true).ToListAsync();
            return menus.Where(m => m.MenuId == null);
        }

        public async Task<IEnumerable<Menu>> GetMenuUsuario(string idUsuario)
        {
            var menu = await GetContext.Menus.OrderBy(m => m.Orden).Join(GetContext.RolesMenus, m => m.Id, r => r.Menu.Id, (m, r) => new { Menu = m, RoleMenu = r })
                 .Join(GetContext.UserRoles, mr => mr.RoleMenu.Rol.Id, ur => ur.RoleId, (mr, ur) => new { MenuRole = mr, UserRole = ur })
                 .Where(mru => mru.MenuRole.Menu.EstaActivo == true && mru.UserRole.UserId == idUsuario)
                 .Select(mru => mru.MenuRole.Menu).Distinct().OrderBy(m => m.Orden).ToListAsync();
            return menu.Where(m => m.MenuId == null);
        }
        public override async Task<Menu> Get(string Id)
        {
            using (var conexion = Conexion)
            {
                conexion.Open();
                var menus = await conexion.QueryAsync<Menu>(@"SELECT * FROM Seguridad.Menu " +
                    " where Seguridad.Menu.EstaActivo='true' and Seguridad.Menu.Id = @Id", param: new { Id });

                return menus.FirstOrDefault();
            }
        }

        public async Task<Menu> GetWithAssociations(string Id)
        {
            return await GetContext.Menus.Include(m => m.Menus).FirstOrDefaultAsync(m => m.EstaActivo == true && m.Id.ToString() == Id);
        }

        public async Task<IEnumerable<Menu>> GetMenuRole(string idRole)
        {
            return await GetContext.RolesMenus.Where(r => r.Rol.Id == idRole).Select(r => r.Menu).ToListAsync();
        }
        public async Task RemoveMenuRole(string idRole)
        {
            GetContext.RolesMenus.RemoveRange(GetContext.RolesMenus.Where(rm => rm.Rol.Id == idRole).ToList());
            await GetContext.SaveChangesAsync();
        }

        public override async Task Remove(Menu entity)
        {
            var cargo = _contexto.Set<Menu>().FirstOrDefault(o => o.Id == entity.Id);
            cargo.EstaActivo = false;
            await _contexto.SaveChangesAsync();
        }

        public ContextoFBSIdentidad GetContext
        {
            get { return _contexto as ContextoFBSIdentidad; }
        }

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
