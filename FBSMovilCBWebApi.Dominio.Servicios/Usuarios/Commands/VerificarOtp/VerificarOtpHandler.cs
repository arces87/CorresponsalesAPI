using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Utilidad;
using FBS.Identidad.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Newtonsoft.Json;
using OtpNet;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class VerificarOtpHandler : IRequestHandler<VerificarOtpME, bool>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly byte[] _llave;

        public VerificarOtpHandler(IMediator mediador, IRepositorioUsuario repositorioUsuario,
            IJsonConfiguracion jsonConfiguracion, IRepositorioAgente repositorioAgente)
        {
            _mediador = mediador;
            _repositorioUsuario = repositorioUsuario;
            _jsonConfiguracion = jsonConfiguracion;
            _repositorioAgente = repositorioAgente;
            _llave = Encoding.UTF8.GetBytes("!A%D*G-KaPdSgVkY");
        }

        public async Task<bool> Handle(VerificarOtpME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdVerificarOtp").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogSolicitado").Valor,
            });

            await _mediador.Send(new VerificarAgenteME()
            {
                Imei = request.Imei,
                Mac = request.Mac,
                Latitud = request.Latitud,
                Longitud = request.Longitud,
                Usuario = request.Usuario
            });

            var secretKey = Criptografia.EncryptStringToBytes_Aes(request.Identificacion, _llave, _llave);
            var tiempoVida = _jsonConfiguracion.TiempoVidaOtp;
            var totp = new Totp(secretKey, tiempoVida);
            long tiempoVerificacion = 0;
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdVerificarOtp").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogTerminado").Valor,
            });
            var vw = new VerificationWindow(1, 1);
            var verificacion = totp.VerifyTotp(request.Otp, out tiempoVerificacion, vw);

            if (verificacion && !(await _repositorioUsuario.ComprobarOtp(request.Usuario, request.Identificacion, tiempoVerificacion.ToString())))
            {
                await _repositorioUsuario.SalvarOtp(request.Usuario, request.Identificacion, tiempoVerificacion.ToString());
                return true;
            }
            return false;
        }
    }
}
