using FBS.Identidad.DAL.Seguridad;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales
{
    public interface IRepositorioAgente : IRepositorio<Agente>
    {
        Task<IEnumerable<Agente>> GetAllActive();
        Task<IEnumerable<Agente>> GetAllWithAssociations(string IdSupervisor);
        Task<IEnumerable<Agente>> GetAllWithAssociationsConsola(string IdSupervisor);
        Task<Agente> GetWithAssociations(string Id);
        Task<IEnumerable<Agente>> GetForActivation(string IdSupervisor);
        Task<Agente> GetForUserName(string userName);
        Task<Agente> GetForId(string idUsuario);
        Task Activar(string Id);
        Task<IEnumerable<Usuario>> GetUsuariosDisponibles();
        Task<IEnumerable<Usuario>> GetSupervisoresDisponibles();
        Task<IEnumerable<Dispositivo>> GetDispositivosDisponibles();
        Task UpdateEstado(Agente entidad);

    }
}
