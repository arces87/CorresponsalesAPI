using FBS.Identidad.Dominio.Modelos.Seguridad;
using FBS.Identidad.Dominio.Servicios.Interfaces.Seguridad;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Seguridad
{
    public interface IServicioUsuarioLocal : IServicioUsuario
    {
        Task<ModeloPersona> Autenticar(ModeloUsuario model);


    }
}
