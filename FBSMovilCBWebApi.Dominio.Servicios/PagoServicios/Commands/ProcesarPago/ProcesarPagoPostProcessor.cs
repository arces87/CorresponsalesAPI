using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Notificaciones;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Commands.ProcesarPago
{
    public class ProcesarPagoPostProcessor : IRequestPostProcessor<ProcesarPagoME, AfectacionMS>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IHttpContextAccessor _httpContext;
        public ProcesarPagoPostProcessor(IMediator mediador, IRepositorioAgente repositorioAgente, IHttpContextAccessor httpContext)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _httpContext = httpContext;
        }
        public async Task Process(ProcesarPagoME request, AfectacionMS response, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);
            var valores = new Dictionary<string, string>();

            var fechaActual = DateTime.Now.ToString("DD/MM/yyyy/ H:mm");

            if (jsonNegocio.Deposito.NotificarCorreoElectronico)
            {
                if (string.IsNullOrEmpty(jsonNegocio.Deposito.PlantillaCorreoElectronico))
                {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "EmailTemplate", "index_pago_servicios.html");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Deposito.PlantillaCorreoElectronico = SourceReader.ReadToEnd();
                    }
                }

                valores.Add("[:[:NOMBRECLIENTE:]:]", request.NombreCliente);
                valores.Add("[:[:NOMBRECORRESPONSAL:]:]", agente.NombreAgente);
                valores.Add("[:[:FECHAACTUAL:]:]", fechaActual);
            }

            var valoresSMS = new Dictionary<string, string>();

            if (jsonNegocio.Deposito.NotificarSMS)
            {
                if (string.IsNullOrEmpty(jsonNegocio.Deposito.PlantillaSMS))
                {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "SmsTemplate", "template_pago_servicios.txt");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Deposito.PlantillaSMS = SourceReader.ReadToEnd();
                    }

                }

                valores.Add("[:[:VALOROPERACION:]:]", request.Valor.ToString());
                valores.Add("[:[:NOMBRECORRESPONSAL:]:]", agente.NombreAgente);
                valores.Add("[:[:FECHAACTUAL:]:]", fechaActual);
            }


            await _mediador.Publish(new NotificacionME
            {
                PlantillaCorreoElectronico = jsonNegocio.Deposito.NotificarCorreoElectronico ? jsonNegocio.Deposito.PlantillaCorreoElectronico : null,
                PlantillaSMS = jsonNegocio.Deposito.NotificarSMS ? jsonNegocio.Deposito.PlantillaSMS : null,
                CorreoElectronicoDestinatario = agente.Usuario.Email,
                NombreDestinatario = agente.NombreAgente,
                AsuntoCorreoElectronico = "Operación Pago de Servicios realizada con éxito",
                NumeroCliente = 1,
                SecuencialEmpresa = 1,
                ValoresEmail = valores,
                ValoresSms = valoresSMS
            });
        }
    }
}
