using FBS.Infraestructura.Interfaces;
using FBS.Identidad.DAL.Seguridad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBS.Identidad.Infraestructura.Interfaces
{
    public interface IRepositorioRol : IRepositorio<Rol>
    {
        Task<IEnumerable<Rol>> GetAllActive();
        Task<Rol> GetForName(string Name);
        Task AddMenu(RolMenu menu);
        Task<Rol> GetRolUsuario(string UsuarioId);
    }
}
