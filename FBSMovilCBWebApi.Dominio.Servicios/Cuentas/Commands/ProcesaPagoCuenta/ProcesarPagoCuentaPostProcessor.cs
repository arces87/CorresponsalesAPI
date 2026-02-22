using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Infraestructura.Interfaces;
using FBS.Infraestructura.Excepciones;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Notificaciones;
using FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands.ProcesarDeposito;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Commands
{
    public class ProcesaPagoCuentaPostProcessor : IRequestPostProcessor<ProcesaPagoCuentaME, ProcesaPagoCuentaMS>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        public ProcesaPagoCuentaPostProcessor(
            IMediator mediador, 
            IRepositorioAgente repositorioAgente, 
            IHttpContextAccessor httpContext,
            IJsonConfiguracion jsonConfiguracion,
            IApiKeyGenerator apiKeyGenerator)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _httpContext = httpContext;
            _jsonConfiguracion = jsonConfiguracion;
            _apiKeyGenerator = apiKeyGenerator;
        }
        public async Task Process(ProcesaPagoCuentaME request, ProcesaPagoCuentaMS response, CancellationToken cancellationToken)
        {

            var IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdObligacion").Valor;

            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);    
            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);
            var valores = new Dictionary<string, string>();
            var comision = jsonNegocio.Obligaciones.Comisiones.AdministracionCanal + jsonNegocio.Obligaciones.Comisiones.Agente + jsonNegocio.Obligaciones.Comisiones.Cooperativa;
            
            var fechaActualEmail = response.Fecha;
            var fechaActual = response.Fecha;
            var horaActual = response.Fecha;

            if (jsonNegocio.Obligaciones.NotificarCorreoElectronico)
            {
                if(string.IsNullOrEmpty(jsonNegocio.Obligaciones.PlantillaCorreoElectronico)) {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "EmailTemplate", "index_obligaciones.html");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Obligaciones.PlantillaCorreoElectronico = SourceReader.ReadToEnd();
                    }
                }
                
                var cuenta = response.PagoCuentaResponse[0].Detalle;

                valores.Add("[:NOMBRECLIENTE:]", request.NombreCliente);
                valores.Add("[:NOMBRECORRESPONSAL:]", agente.Usuario.NombreMostrar);
                valores.Add("[:FECHAACTUAL:]", fechaActualEmail);
                valores.Add("[:CUENTA:]", cuenta);
                valores.Add("[:VALOR:]", request.ValorAfectado.ToString());
                
        }

        var valoresSMS = new Dictionary<string, string>();

            if (jsonNegocio.Obligaciones.NotificarSMS)
            {
                if(string.IsNullOrEmpty(jsonNegocio.Obligaciones.PlantillaSMS)){
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "SmsTemplate", "template_obligaciones.txt");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Obligaciones.PlantillaSMS = SourceReader.ReadToEnd();
                    }                    
                }

                var cuenta = response.PagoCuentaResponse[0].Detalle;

                valoresSMS.Add("[:VALOROPERACION:]", request.ValorAfectado.ToString());
                valoresSMS.Add("[:CUENTA:]", cuenta);
                valoresSMS.Add("[:NOMBRECORRESPONSAL:]", agente.Usuario.NombreMostrar);
                valoresSMS.Add("[:FECHAACTUAL:]", fechaActual);
                valoresSMS.Add("[:HORAACTUAL:]", horaActual);
            }
           
            try
            {
                var buscarClienteME = new BuscarClienteME
                {
                    SecuencialTipoIdentificacion = 1,
                    Identificacion = request.IdentificacionCliente,
                    Imei = request.Imei,
                    Mac = request.Mac,
                    Usuario = request.Usuario,
                    Latitud = request.Latitud,
                    Longitud = request.Longitud,
                };

                var datosCliente = await _mediador.Send(buscarClienteME);

                //var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
                //var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

                var notificacion = new NotificacionME
                {
                    PlantillaCorreoElectronico = jsonNegocio.Obligaciones.NotificarCorreoElectronico ? jsonNegocio.Obligaciones.PlantillaCorreoElectronico : null,
                    PlantillaSMS = jsonNegocio.Obligaciones.NotificarSMS ? jsonNegocio.Obligaciones.PlantillaSMS : null,
                    CorreoElectronicoDestinatario = datosCliente.CorreoElectronico,
                    NombreDestinatario = request.NombreCliente,
                    AsuntoCorreoElectronico = "Operación Cuenta por Pagar realizada con éxito",
                    NombreUsuarioCorresponsal= agente.Usuario.UserName,
                    NumeroCliente = 1,
                    SecuencialEmpresa = 1,
                    ValoresEmail = valores,
                    ValoresSms = valoresSMS,
                    TipoIdentificacion = 1,
                    Identificacion = request.IdentificacionCliente,
                    //Encabezado = customHeaders
                };

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
                        response.NotificationErrorMensaje = "Ha ocurrido un error al notificar la operación.";
                        break;
                }
            }
        }
    }
}
