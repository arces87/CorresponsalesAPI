using FBS.Identidad.Dominio.Modelos.Seguridad;
using FBS.Identidad.Dominio.Servicios.Interfaces.Seguridad;
using FBSConsolaCBWebApi.Dominio.Modelos.EstructuraEmpresarial;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.Seguridad
{
    public interface IServicioUsuarioLocal : IServicioUsuario
    {
        Task<ModeloPersona> Autenticar(ModeloUsuario model);


    }
}
