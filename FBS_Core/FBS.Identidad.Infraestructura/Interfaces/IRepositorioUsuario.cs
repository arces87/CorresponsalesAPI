using FBS.Infraestructura.Interfaces;
using FBS.Identidad.DAL.Seguridad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBS.Identidad.Infraestructura.Interfaces
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        Task<IEnumerable<Usuario>> GetAllActive();
        Task<Usuario> GetPorUsuario(string Usuario);
        Task<bool> ComprobarOtp(string Usuario, string identificacion, string otpComprobar);
        Task<string> ReferenciaOtp(string Usuario, string identificacion);
        Task SalvarOtp(string Usuario, string identificacion, string otp);
        Task EliminarOtp(string identificacion);
        Task SalvarTokenRecuperarContrasenia(string idUsuario, string token);
        Task<string> ComprobarTokenRecuperarContrasenia(string token);
        Task EliminarTokenRecuperarContrasenia(string Usuario);
        Task AsignarCanal(CanalUsuario entidad);
        Task<bool> VerificarCorreoElectronico(string email);
        Task<bool> VerificarCorreoElectronico(string email, string IdUsuario);
        Task<bool> VerificarUserName(string username);
        Task<bool> VerificarUserName(string username, string IdUsuario);

    }
}
