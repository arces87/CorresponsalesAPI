using AutoMapper;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBS.Identidad.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using Microsoft.Extensions.Configuration;
using OtpNet;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands
{
    public class GenerarOtpCommandHandle : IRequestHandler<DatosOtpME, ProcesarOtpMS>
    {
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IConfiguration _configuracion;

        public GenerarOtpCommandHandle(IRepositorioUsuario repositorioUsuario, IConfiguration configuracion)
        {
            _repositorioUsuario = repositorioUsuario;
            _configuracion = configuracion;
        }

        public async Task<ProcesarOtpMS> Handle(DatosOtpME request, CancellationToken cancellationToken)
        {
            try
            {
                var _secretKey = _configuracion["JwtKey"];
                var totp = new Totp(Encoding.ASCII.GetBytes(_secretKey));
                await _repositorioUsuario.EliminarOtp(request.Usuario);
                await _repositorioUsuario.SalvarOtp(request.Usuario, totp.ComputeTotp());
            }
            catch (Exception)
            {
                throw new Exception("Error generando el OTP para el usuario");
            }
            return new ProcesarOtpMS() { CodigoRespuesta = true };
        }

    }
}
