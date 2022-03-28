using FBS.Infraestructura.Interfaces;
using FBS.Identidad.DAL.Seguridad;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FBS.Identidad.Infraestructura.Interfaces
{
    public interface IRepositorioCanal : IRepositorio<Canal>
    {
        Task<Canal> GetCanalUsuario(string idUsuario);
        Task<IEnumerable<Canal>> GetAllWithAssociations(bool? Activo);
        Task<Canal> GetWithAssociations(string Id);

    }
}
