using FBS.Identidad.DAL.Seguridad;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales
{
    public interface IRepositorioAgente : IRepositorio<Agente>
    {
        Task<IEnumerable<Agente>> GetAllActive();
        Task<IEnumerable<Agente>> GetAllWithAssociations();
        Task<Agente> GetWithAssociations(string Id);
        Task<IEnumerable<Agente>> GetForActivation();
        Task<Agente> GetForUserName(string userName);
        Task Activar(string Id);
        Task<IEnumerable<Usuario>> GetUsuariosDisponibles();
        Task<IEnumerable<Usuario>> GetSupervisoresDisponibles();
        Task UpdateEstado(Agente entidad);

    }
}
