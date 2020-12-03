using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Notificaciones;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarRetiro;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarRetiroPostProcessor : IRequestPostProcessor<ProcesarRetiroME, AfectacionAUnCorresponsalRepositorioMS>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public ProcesarRetiroPostProcessor(
            IMediator mediador, 
            IRepositorioAgente repositorioAgente, 
            IHttpContextAccessor httpContext,
            IJsonConfiguracion jsonConfiguracion)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _httpContext = httpContext;
            _jsonConfiguracion = jsonConfiguracion;
        }
        public async Task Process(ProcesarRetiroME request, AfectacionAUnCorresponsalRepositorioMS response, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);

            var valores = new Dictionary<string, string>();
            var comision = jsonNegocio.Retiro.Comisiones.AdministracionCanal + jsonNegocio.Retiro.Comisiones.Agente + jsonNegocio.Retiro.Comisiones.Cooperativa;

            var fechaActual = DateTime.Now.ToString("dd/MM/yyyy/ H:mm");

            if (jsonNegocio.Retiro.NotificarCorreoElectronico)
            {
                if(string.IsNullOrEmpty(jsonNegocio.Retiro.PlantillaCorreoElectronico))
                {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "EmailTemplate", "index_retiro.html");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Retiro.PlantillaCorreoElectronico = SourceReader.ReadToEnd();
                    }
                }

                valores.Add("[:NOMBRECLIENTE:]", request.NombreCliente);
                valores.Add("[:NOMBRECORRESPONSAL:]", agente.NombreAgente);
                valores.Add("[:FECHAACTUAL:]", fechaActual);
            }

            var valoresSMS = new Dictionary<string, string>();

            if (jsonNegocio.Retiro.NotificarSMS)
            {
                if(string.IsNullOrEmpty(jsonNegocio.Retiro.PlantillaSMS))
                {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "SmsTemplate", "template_retiro.txt");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Retiro.PlantillaSMS = SourceReader.ReadToEnd();
                    }
                }

                valoresSMS.Add("[:VALOROPERACION:]", request.Valor.ToString());
                valoresSMS.Add("[:NOMBRECORRESPONSAL:]", agente.NombreAgente);
                valoresSMS.Add("[:FECHAACTUAL:]", fechaActual);
            }

            var buscarClienteME = new BuscarClienteME
            {
                SecuencialTipoIdentificacion = request.TipoIdentificacionCliente,
                Identificacion = request.IdentificacionCliente,
                Imei = request.Imei,
                Mac = request.Mac,
                Usuario = request.Usuario,
                Latitud = request.Latitud,
                Longitud = request.Longitud,
            };

            var datosCliente = await _mediador.Send(buscarClienteME);

            try
            {
                await _mediador.Publish(new NotificacionME
                {
                    PlantillaCorreoElectronico = jsonNegocio.Retiro.NotificarCorreoElectronico ? jsonNegocio.Retiro.PlantillaCorreoElectronico : null,
                    PlantillaSMS = jsonNegocio.Retiro.NotificarSMS ? jsonNegocio.Retiro.PlantillaSMS : null,
                    CorreoElectronicoDestinatario = datosCliente.CorreoElectronico,
                    NombreDestinatario = request.NombreCliente,
                    AsuntoCorreoElectronico = "Operación Retiro realizada con éxito",
                    NumeroCliente = 1,
                    SecuencialEmpresa = 1,
                    ValoresEmail = valores,
                    ValoresSms = valoresSMS,
                    TipoIdentificacion = request.TipoIdentificacionCliente,
                    Identificacion = request.IdentificacionCliente
                });
            }
            catch (Exception error)
            {

                var IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdRetiro").Valor;

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
        }
    }
}
