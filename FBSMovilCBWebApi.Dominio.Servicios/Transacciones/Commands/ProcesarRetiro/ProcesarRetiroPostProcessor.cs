using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Notificaciones;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ServiciosFinancial.Models;
using System.Collections.Generic;
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
            await _mediador.Publish(new NotificacionME
            {
                PlantillaCorreoElectronico = jsonNegocio.Retiro.NotificarCorreoElectronico ? jsonNegocio.Retiro.PlantillaCorreoElectronico : null,
                PlantillaSMS = jsonNegocio.Retiro.NotificarSMS ? jsonNegocio.Retiro.PlantillaSMS : null,
                CorreoElectronicoDestinatario = agente.Usuario.Email,
                NombreDestinatario = agente.NombreAgente,
                AsuntoCorreoElectronico = "",
                NumeroCliente = 1,
                SecuencialEmpresa = 1,
                Valores = new Dictionary<string, string>()
            });
        }
    }
}
