using AutoMapper;
using Corresponsales.Command.Api;
using Corresponsales.Command.Model;
using FBS.DAL.Nomenclador;
using FBS.Dominio.Servicios.CorreoElectronico;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.DAL.Seguridad;
using FBS.Infraestructura.Excepciones;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Commands
{
    public class CrearAlertaHandler : IRequestHandler<CrearAlertaME, bool>
    {
        private readonly IRepositorioAlerta _repositorio;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly IGeneralesApi _envioSMSApi;
        private readonly IRepositorioCatalogo _repositorioCatalogo;

        public CrearAlertaHandler(
            IRepositorioAlerta repositorio, 
            IMapper mapper, 
            IJsonConfiguracion jsonConfiguracion,
            IRepositorioAgente repositorioAgente, 
            IHttpContextAccessor httpContext,
            IMediator mediador,
            UserManager<Usuario> manejadorUsuario,
            IGeneralesApi envioSMSApi,
            IRepositorioCatalogo repositorioCatalogo)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _jsonConfiguracion = jsonConfiguracion;
            _repositorioAgente = repositorioAgente;
            _httpContext = httpContext;
            _mediador = mediador;
            _manejadorUsuario = manejadorUsuario;
            _envioSMSApi = envioSMSApi;
            _repositorioCatalogo = repositorioCatalogo;
        }

        public async Task<bool> Handle(CrearAlertaME request, CancellationToken cancellationToken)
        {

            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud,
                VerificarGeolocalizacion = false
            });

            var _model = _mapper.Map<Alerta>(request);
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            _model.Agente = agente;
            _model.Estado = new Catalogo() { Id = new Guid(_jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdAlertaNueva").Valor) };
            var identificador = await _repositorio.Add(_model);

            await NotificarAlerta(request.Usuario, request.IdTipo);

            return true;
        }

        private async Task NotificarAlerta(string usuario, string idalerta)
        {
            try
            {                
                var _user = _manejadorUsuario.Users.Where(u => u.UserName == usuario).FirstOrDefault();
                var agente = await _repositorioAgente.GetForId(_user.Id);
                var tipoalerta = await _repositorioCatalogo.GetWithAssociations(idalerta);
                var plantillaCorreo = File.ReadAllText("Resources/EmailTemplate/notificacion_alerta.html");
                var fechaActual = DateTime.Now.ToString("dd/MM/yyyy H:mm");
                plantillaCorreo = plantillaCorreo.Replace("[:NOMBREUSUARIO:]", usuario)
                        .Replace("[:TIPOALERTA:]", tipoalerta.Nombre)
                        .Replace("[:FECHA:]", fechaActual);

                await _mediador.Publish(new EnviarCorreoElectronicoME
                {
                    Asunto = "Notificación de Alerta",
                    Mensaje = plantillaCorreo,
                    DireccionesDestino = new List<ModeloCuentaCorreo>() {
                        new ModeloCuentaCorreo() {
                            Direccion = agente.Supervisor.Email,
                            Nombre = agente.Supervisor.PhoneNumber
                        }
                    }
                });

                //var plantillaSMS = File.ReadAllText("Resources/SmsTemplate/template_alerta.txt");
                //plantillaSMS = plantillaSMS.Replace("[:NOMBREUSUARIO:]", usuario)
                //        .Replace("[:TIPOALERTA:]", tipoalerta.Nombre)
                //        .Replace("[:FECHA:]", fechaActual);

                //var mensajeSMS = new EnvioSmsRequest()
                //{
                //    CodigoUsuarioCorresponsal = usuario,
                //    MensajeTexto = plantillaSMS,
                //    NumeroIdentificacion = "",
                //    SecuencialTipoIdentificacion = 0,
                //    NumeroCelular = _user.PhoneNumber
                //};

                //var respuesta = await _envioSMSApi.EnvioSmsAsync(mensajeSMS);
            }
            catch
            {
                throw new ExcepcionApp("La alerta fue registrada con exito, pero no fue posible notificar al supervisor.", TipoError.EmailNotification);
            }
        }
    }   

}
