using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FBS.Infraestructura.Repositorio;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace FBS.Identidad.Infraestructura.Repositorio
{
    public class RepositorioRol : Repositorio<Rol>, IRepositorioRol
    {
        private readonly IConfiguration _configuracion; 
        public RepositorioRol(ContextoFBSIdentidad context, IConfiguration configuracion) : base(context)
        {
            _configuracion = configuracion;
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

        public async Task<Rol> GetRolUsuario(string UsuarioId)
        {
            var conexion = Conexion;
            conexion.Open();
            var rol = await conexion.QueryAsync<Rol>("SELECT Nombre AS Descripcion FROM Seguridad.UsuarioRol " +
                "JOIN Seguridad.Rol AS R ON Seguridad.UsuarioRol.RolId = R.Id " +
                "WHERE UsuarioId = '" + UsuarioId +"'");
            return rol.FirstOrDefault();
        }

        public IDbConnection Conexion => new SqlConnection(_configuracion.GetConnectionString("DapperConnection"));
    }
}
