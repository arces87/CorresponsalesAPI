using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Notificaciones;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarRetiro;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarRetiroPostProcessor : IRequestPostProcessor<ProcesarRetiroME, AfectacionAUnCorresponsalRepositorioMS>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IRepositorioDispositivoAgente _repositorioDispositivoAgente;
        private readonly IRepositorioDispositivo _repositorioDispositivo;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IApiKeyGenerator _apiKeyGenerator;

        public ProcesarRetiroPostProcessor(
            IMediator mediador, 
            IRepositorioAgente repositorioAgente, 
            IHttpContextAccessor httpContext,
            IJsonConfiguracion jsonConfiguracion,
            IApiKeyGenerator apiKeyGenerator, 
            IRepositorioDispositivoAgente repositorioDispositivoAgente, 
            IRepositorioDispositivo repositorioDispositivo)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _httpContext = httpContext;
            _jsonConfiguracion = jsonConfiguracion;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioDispositivoAgente = repositorioDispositivoAgente;
            _repositorioDispositivo = repositorioDispositivo;
        }
        public async Task Process(ProcesarRetiroME request, AfectacionAUnCorresponsalRepositorioMS response, CancellationToken cancellationToken)
        {
            var IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdRetiro").Valor;
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var dispositivoagente = await _repositorioDispositivoAgente.GetForAgente(agente.Id.ToString());
            var dispositivo = await _repositorioDispositivo.Get(dispositivoagente.DispositivoId.ToString());
            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);

            var valores = new Dictionary<string, string>();

            var fechaActualEmail = response.FechaTransaccion.ToString("dd/MM/yyyy/ H:mm");
            var fechaActual = response.FechaTransaccion.ToString("yyyy/MM/dd");
            var horaActual = response.FechaTransaccion.ToString("H:mm:ss");

            PrepararCorreoElectronico(response, request, agente, jsonNegocio, valores, fechaActualEmail);

            var valoresSMS = new Dictionary<string, string>();

            PrepararSMS(response, agente, jsonNegocio, fechaActual, horaActual, valoresSMS);

            await Notificar(request, response, IdTipoAccion, jsonNegocio, valores, valoresSMS, agente.Usuario.UserName, dispositivo.Imei);
        }

        private async Task Notificar(
            ProcesarRetiroME request, 
            AfectacionAUnCorresponsalRepositorioMS response, 
            string IdTipoAccion, 
            JsonNegocioMS jsonNegocio, 
            Dictionary<string, string> valores, 
            Dictionary<string, string> valoresSMS,
            string nombreUsuarioCorresponsal,
            string imei)
        {
            try
            {
                NotificacionME notificacion = await PrepararNotificacion(request, jsonNegocio, valores, valoresSMS, nombreUsuarioCorresponsal, imei); 

                await _mediador.Send(new CrearLogME()
                {
                    JsonLog = JsonConvert.SerializeObject(notificacion),
                    IdTipoAccion = IdTipoAccion,
                    IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
                });

                await _mediador.Publish(notificacion);
            }
            catch (Exception error)
            {
                await ManejarError(response, IdTipoAccion, error);
            }
        }

        private async Task ManejarError(AfectacionAUnCorresponsalRepositorioMS response, string IdTipoAccion, Exception error)
        {
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(error.Message),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });

            response.NotificationError = true;

            switch (error)
            {
                case ExcepcionApp e:
                    response.NotificationErrorMensaje = e.Message;
                    break;
                default:
                    response.NotificationErrorMensaje = "Ha ocurrido un error al notificar la opearación.";
                    break;
            }
        }

        private async Task<NotificacionME> PrepararNotificacion(
            ProcesarRetiroME request, 
            JsonNegocioMS jsonNegocio, 
            Dictionary<string, string> valores, 
            Dictionary<string, string> valoresSMS, 
            string nombreUsuarioCorresponsal,
            string imei)
        {
            var buscarClienteME = new BuscarClienteME
            {
                SecuencialTipoIdentificacion = request.TipoIdentificacionCliente,
                Identificacion = request.IdentificacionCliente,
                Imei = request.Imei,
                Mac = request.Mac,
                Usuario = request.Usuario,
                Latitud = request.Latitud,
                Longitud = request.Longitud
            };

            var datosCliente = await _mediador.Send(buscarClienteME);

            //var apiKey = _apiKeyGenerator.generateApiKey(imei);
            //var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

            var notificacion = new NotificacionME
            {
                PlantillaCorreoElectronico = jsonNegocio.Retiro.NotificarCorreoElectronico ? jsonNegocio.Retiro.PlantillaCorreoElectronico : null,
                PlantillaSMS = jsonNegocio.Retiro.NotificarSMS ? jsonNegocio.Retiro.PlantillaSMS : null,
                CorreoElectronicoDestinatario = datosCliente.CorreoElectronico,
                NombreDestinatario = request.NombreCliente,
                AsuntoCorreoElectronico = "Operación Retiro realizada con éxito",
                NombreUsuarioCorresponsal = nombreUsuarioCorresponsal,
                NumeroCliente = 1,
                SecuencialEmpresa = 1,
                ValoresEmail = valores,
                ValoresSms = valoresSMS,
                TipoIdentificacion = request.TipoIdentificacionCliente,
                Identificacion = request.IdentificacionCliente,
                //Encabezado = customHeaders
            };
            return notificacion;
        }

        private static void PrepararSMS(AfectacionAUnCorresponsalRepositorioMS response, FBSConsolaCBWebApi.DAL.Corresponsales.Agente agente, JsonNegocioMS jsonNegocio, string fechaActual, string horaActual, Dictionary<string, string> valoresSMS)
        {
            if (jsonNegocio.Retiro.NotificarSMS)
            {
                if (string.IsNullOrEmpty(jsonNegocio.Retiro.PlantillaSMS))
                {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "SmsTemplate", "template_retiro.txt");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Retiro.PlantillaSMS = SourceReader.ReadToEnd();
                    }
                }

                var cuenta = response.NumeroCuenta.ToString();
                cuenta = cuenta.Substring(0, 4) + "XXXXXXXX";

                valoresSMS.Add("[:VALOROPERACION:]", response.Valor.ToString());
                valoresSMS.Add("[:CUENTA:]", cuenta);
                valoresSMS.Add("[:NOMBRECORRESPONSAL:]", agente.Usuario.NombreMostrar);
                valoresSMS.Add("[:FECHAACTUAL:]", fechaActual);
                valoresSMS.Add("[:HORAACTUAL:]", horaActual);
            }
        }

        private static void PrepararCorreoElectronico(AfectacionAUnCorresponsalRepositorioMS response, ProcesarRetiroME request, FBSConsolaCBWebApi.DAL.Corresponsales.Agente agente, JsonNegocioMS jsonNegocio, Dictionary<string, string> valores, string fechaActualEmail)
        {
            if (jsonNegocio.Retiro.NotificarCorreoElectronico)
            {
                if (string.IsNullOrEmpty(jsonNegocio.Retiro.PlantillaCorreoElectronico))
                {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "EmailTemplate", "index_retiro.html");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Retiro.PlantillaCorreoElectronico = SourceReader.ReadToEnd();
                    }
                }

                var cuenta = response.NumeroCuenta.ToString();
                cuenta = cuenta.Substring(0, 4) + "XXXXXXXX";

                valores.Add("[:NOMBRECLIENTE:]", request.NombreCliente);
                valores.Add("[:NOMBRECORRESPONSAL:]", agente.Usuario.NombreMostrar);
                valores.Add("[:FECHAACTUAL:]", fechaActualEmail);
                valores.Add("[:CUENTA:]", cuenta);
                valores.Add("[:VALOR:]", response.Valor.ToString());
            }
        }
    }
}
