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

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarRetiroPostProcessor : IRequestPostProcessor<ProcesarRetiroME, AfectacionAUnCorresponsalMS>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IHttpContextAccessor _httpContext;

        public ProcesarRetiroPostProcessor(IMediator mediador, IRepositorioAgente repositorioAgente, IHttpContextAccessor httpContext)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _httpContext = httpContext;
        }
        public async Task Process(ProcesarRetiroME request, AfectacionAUnCorresponsalMS response, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);

            var valores = new Dictionary<string, string>();
            var comision = jsonNegocio.Retiro.Comisiones.AdministracionCanal + jsonNegocio.Retiro.Comisiones.Agente + jsonNegocio.Retiro.Comisiones.Cooperativa;
            var detalle = "";

            if (jsonNegocio.Retiro.NotificarCorreoElectronico)
            {
                if(string.IsNullOrEmpty(jsonNegocio.Retiro.PlantillaCorreoElectronico))
                {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "EmailTemplate", "index.html");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Retiro.PlantillaCorreoElectronico = SourceReader.ReadToEnd();
                    }
                }                

                detalle = $"<ul><li>Operación: Retiro</li><li>Tipo de Cuenta: {request.NumeroCuentaCliente}</li><li>No Cuenta: {request.TipoCuentaCliente}</li><li>Valor: {request.Valor}</li><li>Comisión: {comision}</li><li>Total: {request.Valor}</li><li>Descripción:</li></ul>";
                valores.Add("[:detalle:]", detalle);
            }

            var valoresSMS = new Dictionary<string, string>();

            if (jsonNegocio.Retiro.NotificarSMS)
            {
                if(string.IsNullOrEmpty(jsonNegocio.Retiro.PlantillaSMS))
                {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "SmsTemplate", "template.txt");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Retiro.PlantillaSMS = SourceReader.ReadToEnd();
                    }
                }              

                detalle = $"Operación: Retiro\nNo Cuenta: {request.TipoCuentaCliente}\nTotal: {request.Valor}";
                valoresSMS.Add("[:detalle:]", detalle);
            }
                      
            await _mediador.Publish(new NotificacionME
            {
                PlantillaCorreoElectronico = jsonNegocio.Retiro.NotificarCorreoElectronico ? jsonNegocio.Retiro.PlantillaCorreoElectronico : null,
                PlantillaSMS = jsonNegocio.Retiro.NotificarSMS ? jsonNegocio.Retiro.PlantillaSMS : null,
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
