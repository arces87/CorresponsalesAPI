using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCBWebApi.Dominio.Modelos.EstructuraEmpresarial;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial
{
    public interface IServicioPersona
    {
        Task<ModeloFuenteDatos<ModeloPersona>> List(ModeloPaginacion filtro);
        Task<ModeloFuenteDatos<ModeloSupervisor>> ListaSupervisores(ModeloPaginacion filtro);
        Task<ModeloFuenteDatos<ModeloCorresponsal>> ListaCorresponsales(ModeloPaginacion filtro);
        Task<object> Get(int Id);
        Task<ModeloCorresponsal> Create(ModeloCorresponsal model);
        Task<ModeloSupervisor> Create(ModeloSupervisor model);
        Task<ModeloCorresponsal> Update(ModeloCorresponsal model);
        Task<ModeloSupervisor> Update(ModeloSupervisor model);
        Task<ModeloPersona> Delete(int Id);
        Task<ModeloPersona> DevuelveDatosPersonaIdentificacion(string identificacion);
        Task<ModeloCorresponsal> CambiarEstadoCorresponsal(int id);
    }
}
