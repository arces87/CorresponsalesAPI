using FBS_Core.Identity.Domain.Models.Seguridad;
using FBS_Core.Identity.Domain.Services.Interfaces.Seguridad;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Domain.Services.Interfaces.Seguridad
{
    public interface IUsuarioService : IUserService
    {
        Task<PersonaModel> Autenticar(UserModel model);


    }
}
