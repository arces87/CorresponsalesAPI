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

namespace FBSMovilCBWebApi.Dominio.Servicios.Facilito.Commands.ProcesarPago
{
    public class ProcesarPagoPostProcessor : IRequestPostProcessor<ProcesarPagoME, PagoFacilitoMSL>
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
        public async Task Process(ProcesarPagoME request, PagoFacilitoMSL response, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);
            var valores  =  new Dictionary<string, string>();
            var comision = jsonNegocio.CobroServicios.Comisiones.AdministracionCanal + jsonNegocio.CobroServicios.Comisiones.Agente + jsonNegocio.CobroServicios.Comisiones.Cooperativa;
            var detalle = $"<ul><li>Operación: Pago de Servicios</li><li>Producto: {request.IdProducto}</li><li>No Cuenta: {request.SecuencialCuentaCliente}</li><li>Valor: {request.Valor}</li><li>Comisión: {comision}</li><li>Total: {request.Valor}</li><li>Descripción:</li></ul>";
            valores.Add("[:detalle:]", detalle);

            if (jsonNegocio.CobroServicios.NotificarCorreoElectronico && string.IsNullOrEmpty(jsonNegocio.CobroServicios.PlantillaCorreoElectronico))
            {
                var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "EmailTemplate", "index.html");

                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    jsonNegocio.CobroServicios.PlantillaCorreoElectronico = SourceReader.ReadToEnd();
                }
            }

            if (jsonNegocio.CobroServicios.NotificarSMS && string.IsNullOrEmpty(jsonNegocio.CobroServicios.PlantillaSMS))
            {
                var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "SmsTemplate", "template.txt");

                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    jsonNegocio.CobroServicios.PlantillaSMS = SourceReader.ReadToEnd();
                }
            }

            var valoresSMS = new Dictionary<string, string>();
            detalle = $"Operación: Abono de Préstamos\nNo de Producto: {request.IdProducto}\nTotal: {request.Valor}";
            valoresSMS.Add("[:detalle:]", detalle);


            await _mediador.Publish(new NotificacionME
            {
                PlantillaCorreoElectronico = jsonNegocio.CobroServicios.NotificarCorreoElectronico ? jsonNegocio.CobroServicios.PlantillaCorreoElectronico : null,
                PlantillaSMS = jsonNegocio.CobroServicios.NotificarSMS ? jsonNegocio.CobroServicios.PlantillaSMS : null,
                CorreoElectronicoDestinatario = agente.Usuario.Email,
                NombreDestinatario = agente.NombreAgente,
                AsuntoCorreoElectronico = "",
                NumeroCliente = 1,
                SecuencialEmpresa = 1,
                ValoresEmail = valores,
                ValoresSms = valoresSMS
            });
        }
    }
}
