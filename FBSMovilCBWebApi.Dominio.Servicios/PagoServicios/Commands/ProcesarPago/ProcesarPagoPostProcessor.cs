using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Notificaciones;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ServiciosFinancial.Models;
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
            //var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            //var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);
            //jsonNegocio.CobroServicios.NotificarCorreoElectronico = false;


            //if (jsonNegocio.CobroServicios.NotificarSMS && string.IsNullOrEmpty(jsonNegocio.CobroServicios.PlantillaSMS))
            //{
            //    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "SmsTemplate", "template.txt");

            //    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            //    {
            //        jsonNegocio.CobroServicios.PlantillaSMS = SourceReader.ReadToEnd();
            //    }
            //}

            //var valoresSMS = new Dictionary<string, string>();
            //var detalle = $"Operación: Abono de Préstamos\nNo de Producto: {request.SecuencialServicio}\nTotal: {request.Valor}";
            //valoresSMS.Add("[:detalle:]", detalle);

            //await _mediador.Publish(new NotificacionME
            //{
            //    PlantillaCorreoElectronico = null,
            //    PlantillaSMS = jsonNegocio.CobroServicios.NotificarSMS ? jsonNegocio.CobroServicios.PlantillaSMS : null,
            //    CorreoElectronicoDestinatario = agente.Usuario.Email,
            //    NombreDestinatario = agente.NombreAgente,
            //    AsuntoCorreoElectronico = "",
            //    NumeroCliente = 1,
            //    SecuencialEmpresa = 1,
            //    ValoresEmail = null,
            //    ValoresSms = valoresSMS
            //});
        }
    }
}
