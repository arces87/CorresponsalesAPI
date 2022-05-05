using FBS.Identidad.DAL.Modelado;
using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using FBSMovilCBWebApi.Dominio.Servicios.Notificaciones;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarAbonoPrestamoPostProcessor : IRequestPostProcessor<ProcesarAbonoPrestamoME, EfectivizacionPrestamoMS>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IClientesApi _cliente;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public ProcesarAbonoPrestamoPostProcessor(IMediator mediador, IRepositorioAgente repositorioAgente, IHttpContextAccessor httpContext, IClientesApi cliente, IJsonConfiguracion jsonConfiguracion)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _httpContext = httpContext;
            _cliente = cliente;
            _jsonConfiguracion = jsonConfiguracion;
        }
        public async Task Process(ProcesarAbonoPrestamoME request, EfectivizacionPrestamoMS response, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);
            var valores = new Dictionary<string, string>();

            var fechaActual = DateTime.Now.ToString("dd/MM/yyyy/ H:mm");

            if (jsonNegocio.AbonoPrestamos.NotificarCorreoElectronico)
            {
                if (string.IsNullOrEmpty(jsonNegocio.AbonoPrestamos.PlantillaCorreoElectronico))
                {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "EmailTemplate", "index_abono_prestamo.html");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.AbonoPrestamos.PlantillaCorreoElectronico = SourceReader.ReadToEnd();
                    }
                }

                valores.Add("[:NOMBRECLIENTE:]", request.NombreCliente);
                valores.Add("[:NOMBRECORRESPONSAL:]", agente.NombreAgente);
                valores.Add("[:FECHAACTUAL:]", fechaActual);
            }

            var valoresSMS = new Dictionary<string, string>();

            if (jsonNegocio.AbonoPrestamos.NotificarSMS)
            {
                if (string.IsNullOrEmpty(jsonNegocio.AbonoPrestamos.PlantillaSMS))
                {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "SmsTemplate", "template_abono_prestamo.txt");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.AbonoPrestamos.PlantillaSMS = SourceReader.ReadToEnd();
                    }
                }

                valoresSMS.Add("[:VALOROPERACION:]", request.Valor.ToString());
                valoresSMS.Add("[:NOMBRECORRESPONSAL:]", agente.NombreAgente);
                valoresSMS.Add("[:FECHAACTUAL:]", fechaActual);
            }

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDevuelveDatosPersona").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });

            var informacionPersona = await _cliente.ClientesDevuelveDatosPersonaIdentificacionAsync(new PorIdentificacionSocioME()
            {
                Identificacion = request.IdentificacionCliente
            });

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(informacionPersona),
                IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDevuelveDatosPersona").Valor,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });

            await _mediador.Publish(new NotificacionME
            {
                PlantillaCorreoElectronico = jsonNegocio.AbonoPrestamos.NotificarCorreoElectronico ? jsonNegocio.AbonoPrestamos.PlantillaCorreoElectronico : null,
                PlantillaSMS = jsonNegocio.AbonoPrestamos.NotificarSMS ? jsonNegocio.AbonoPrestamos.PlantillaSMS : null,
                CorreoElectronicoDestinatario = agente.Usuario.Email,
                NombreDestinatario = agente.NombreAgente,
                AsuntoCorreoElectronico = "Operación Abono de Préstamo realizada con éxito",
                NumeroCliente = 1,
                SecuencialEmpresa = 1,
                ValoresEmail = valores,
                ValoresSms = valoresSMS                
            });
        }
    }
}
