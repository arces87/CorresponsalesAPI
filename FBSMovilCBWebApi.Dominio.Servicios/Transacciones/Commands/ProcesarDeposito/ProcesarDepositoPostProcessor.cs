using Corresponsales.Query.Model;
using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Infraestructura.Excepciones;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Repositories.Corresponsales;
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

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarDepositoPostProcessor : IRequestPostProcessor<ProcesarDepositoME, AfectacionAUnCorresponsalDepositoMS>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly Corresponsales.Query.Api.ICaptacionesVistaApi _cuentaApi;
        public ProcesarDepositoPostProcessor(
            IMediator mediador, 
            IRepositorioAgente repositorioAgente, 
            IHttpContextAccessor httpContext,
            IJsonConfiguracion jsonConfiguracion,
            IApiKeyGenerator apiKeyGenerator,
            IRepositorioCuenta repositorioCuenta,
            Corresponsales.Query.Api.ICaptacionesVistaApi cuentaApi)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _httpContext = httpContext;
            _jsonConfiguracion = jsonConfiguracion;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioCuenta = repositorioCuenta;
            _cuentaApi = cuentaApi;
        }
        public async Task Process(ProcesarDepositoME request, AfectacionAUnCorresponsalDepositoMS response, CancellationToken cancellationToken)
        {

            var IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDeposito").Valor;

            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);    
            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);
            var valores = new Dictionary<string, string>();
            var comision = jsonNegocio.Deposito.Comisiones.AdministracionCanal + jsonNegocio.Deposito.Comisiones.Agente + jsonNegocio.Deposito.Comisiones.Cooperativa;

            var cuenta = await _repositorioCuenta.GetForAgente(agente.Id.ToString());           
            DevuelveCuentaRequest cuentaAsociada = new DevuelveCuentaRequest() { SecuencialCuenta = int.Parse(cuenta.SecuencialCuenta) };
            var respuestaCuentaAsociada = await _cuentaApi.DevuelveCuentaAsync(cuentaAsociada);

            var fechaActualEmail = response.FechaTransaccion.ToString("dd/MM/yyyy/ H:mm");
            var fechaActual = response.FechaTransaccion.ToString("yyyy/MM/dd");
            var horaActual = response.FechaTransaccion.ToString("H:mm:ss");

            if (jsonNegocio.Deposito.NotificarCorreoElectronico)
            {
                if(string.IsNullOrEmpty(jsonNegocio.Deposito.PlantillaCorreoElectronico)) {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "EmailTemplate", "index_deposito.html");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Deposito.PlantillaCorreoElectronico = SourceReader.ReadToEnd();
                    }
                }

                var cuentaOrigen = respuestaCuentaAsociada.Codigo;
                cuentaOrigen = cuentaOrigen.Substring(0, 4) + "XXXXXXXX";

                var cuentaDestino = response.NumeroCuenta.ToString();
                cuentaDestino = cuentaDestino.Substring(0, 4) + "XXXXXXXX";

                valores.Add("[:NOMBRECLIENTE:]", request.NombreCliente);
                valores.Add("[:NOMBRECORRESPONSAL:]", agente.Usuario.NombreMostrar);
                valores.Add("[:FECHAACTUAL:]", fechaActualEmail);
                valores.Add("[:CUENTAORIGEN:]", cuentaOrigen);
                valores.Add("[:CUENTADESTINO:]", cuentaDestino);
                valores.Add("[:VALOR:]", response.Valor.ToString());
                valores.Add("[:NTRANSACCION:]", response.NumeroTransaccion);
            }

            var valoresSMS = new Dictionary<string, string>();

            if (jsonNegocio.Deposito.NotificarSMS)
            {
                if(string.IsNullOrEmpty(jsonNegocio.Deposito.PlantillaSMS)){
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "SmsTemplate", "template_deposito.txt");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Deposito.PlantillaSMS = SourceReader.ReadToEnd();
                    }                    
                }

                var cuentaDestino = response.NumeroCuenta.ToString();
                cuentaDestino = cuentaDestino.Substring(0, 4)+"XXXXXXXX";

                valoresSMS.Add("[:VALOROPERACION:]", response.Valor.ToString());
                valoresSMS.Add("[:CUENTA:]", cuentaDestino);
                valoresSMS.Add("[:NOMBRECORRESPONSAL:]", agente.Usuario.NombreMostrar);
                valoresSMS.Add("[:FECHAACTUAL:]", fechaActual);
                valoresSMS.Add("[:HORAACTUAL:]", horaActual);
            }
           
            try
            {
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

                //var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
                //var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

                var notificacion = new NotificacionME
                {
                    PlantillaCorreoElectronico = jsonNegocio.Deposito.NotificarCorreoElectronico ? jsonNegocio.Deposito.PlantillaCorreoElectronico : null,
                    PlantillaSMS = jsonNegocio.Deposito.NotificarSMS ? jsonNegocio.Deposito.PlantillaSMS : null,
                    CorreoElectronicoDestinatario = datosCliente.CorreoElectronico,
                    NombreDestinatario = request.NombreCliente,
                    AsuntoCorreoElectronico = "Operación Déposito realizada con éxito",
                    NombreUsuarioCorresponsal = agente.Usuario.UserName,
                    NumeroCliente = 1,
                    SecuencialEmpresa = 1,
                    ValoresEmail = valores,
                    ValoresSms = valoresSMS,
                    TipoIdentificacion = request.TipoIdentificacionCliente,
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
