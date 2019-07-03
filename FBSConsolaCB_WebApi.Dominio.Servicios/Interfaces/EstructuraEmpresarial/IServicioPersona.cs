using FBS.Dominio.Modelos.Filtro;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial
{
    public interface IServicioPersona
    {
        Task<ModeloFuenteDatos<ModeloPersona>> List(ModeloPaginacion filtro);
        Task<object> Get(int Id);
        Task<ModeloCorresponsal> Create(ModeloCorresponsal model);
        Task<ModeloSupervisor> Create(ModeloSupervisor model);
        Task<ModeloCorresponsal> Update(ModeloCorresponsal model);
        Task<ModeloSupervisor> Update(ModeloSupervisor model);
        Task<ModeloPersona> Delete(int Id);


    }
}
