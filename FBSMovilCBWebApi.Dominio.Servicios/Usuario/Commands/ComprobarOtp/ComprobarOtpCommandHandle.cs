using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands
{
    public class ComprobarOtpCommandHandle : IRequestHandler<DatosValidarOTPME, ProcesarValidarOtpMS>
    {
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IConfiguration _configuracion;

        public ComprobarOtpCommandHandle(IRepositorioUsuario repositorioUsuario, IConfiguration configuracion)
        {
            _repositorioUsuario = repositorioUsuario;
            _configuracion = configuracion;
        }

        public async Task<ProcesarValidarOtpMS> Handle(DatosValidarOTPME request, CancellationToken cancellationToken)
        {
            try
            {
                await _repositorioUsuario.ComprobarOtp(request.Usuario, request.Otp);
            }
            catch (Exception)
            {
                var intentos = int.Parse(_configuracion["IntentosOtp"]);
                if (request.NumeroIntento >= intentos)
                    throw new Exception("Usted ha excedido el número de intentos permitidos para digitar el OTP");
                throw new Exception("Error generando el OTP para el usuario");
            }
            return new ProcesarValidarOtpMS() { CodigoRespuesta = true };
        }

    }
}
