using FBS.Infraestructura.Interfaces;
using FBS.Identidad.DAL.Seguridad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBS.Identidad.Infraestructura.Interfaces
{
    public interface IRepositorioMenu : IRepositorio<Menu>
    {
        Task<IEnumerable<Menu>> GetAllActive();
        Task<IEnumerable<Menu>> GetAllWithAssociations();
        Task<Menu> GetWithAssociations(string Id);
        Task<IEnumerable<Menu>> GetMenuUsuario(string idUsuario);
        Task<IEnumerable<Menu>> GetMenuRole(string idRole);
        Task RemoveMenuRole(string idRole);
    }
}
