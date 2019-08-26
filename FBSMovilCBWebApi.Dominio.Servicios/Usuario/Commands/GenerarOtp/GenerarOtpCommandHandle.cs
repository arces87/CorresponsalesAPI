using FBS.Dominio.Modelos.CorreoElectronico;
using FBS.Dominio.Servicios.Interfaces.CorreoElectronico;
using FBS.Identidad.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using Microsoft.Extensions.Configuration;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Usuario.Commands
{
    public class GenerarOtpCommandHandle : IRequestHandler<DatosOtpME, ProcesarOtpMS>
    {
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IRepositorioPersona _repositorioPersona;
        private readonly IConfiguration _configuracion;
        private readonly IServicioCorreoElectronico _servicioCorreo;

        public GenerarOtpCommandHandle(IRepositorioUsuario repositorioUsuario, IRepositorioPersona repositorioPersona, IConfiguration configuracion, IServicioCorreoElectronico servicioCorreo)
        {
            _repositorioUsuario = repositorioUsuario;
            _repositorioPersona = repositorioPersona;
            _configuracion = configuracion;
            _servicioCorreo = servicioCorreo;
        }

        public async Task<ProcesarOtpMS> Handle(DatosOtpME request, CancellationToken cancellationToken)
        {
            try
            {
                var _secretKey = _configuracion["JwtKey"];
                var remitente = _configuracion.GetSection("ConfiguracionCorreo")["CuentaRemitente"];
                var totp = new Totp(Encoding.ASCII.GetBytes(_secretKey));
                var otp = totp.ComputeTotp();
                var usuario = await _repositorioUsuario.GetPorUsuario(request.Usuario);
                if (usuario == null)
                    throw new Exception("El usuario no se encuentra registrado en nuestro sistema");
                var persona = await _repositorioPersona.GetForUserName(usuario.UserName);
                if (persona == null)
                    throw new Exception("El usuario no tiene datos asociados en nuestro sistema");
                await _repositorioUsuario.EliminarOtp(request.Usuario);
                await _repositorioUsuario.SalvarOtp(request.Usuario, otp);
                var mensaje = "Su código de verificación es: " + otp;
                _servicioCorreo.Enviar(new ModeloMensaje()
                {
                    Asunto = "Código de verificacion Corresponsales",
                    Mensaje = mensaje,
                    DireccionesDestino = new List<ModeloCuentaCorreo>() {
                        new ModeloCuentaCorreo() {
                        Direccion = usuario.Email,
                        Nombre = persona.NombreUnido}
                    },
                    DireccionesRemitente = new List<ModeloCuentaCorreo>() {
                        new ModeloCuentaCorreo() {
                        Direccion = remitente,
                        Nombre = "Sistema de Corresponsales"}
                    }
                });
            }
            catch (Exception)
            {
                throw new Exception("Error generando el OTP para el usuario");
            }
            return new ProcesarOtpMS() { CodigoRespuesta = true };
        }

    }
}
