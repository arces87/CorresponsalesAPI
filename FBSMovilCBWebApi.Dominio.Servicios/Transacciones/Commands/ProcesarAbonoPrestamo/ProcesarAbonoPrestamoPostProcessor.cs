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
    public class ProcesarAbonoPrestamoPostProcessor : IRequestPostProcessor<ProcesarAbonoPrestamoME, EfectivizacionPrestamoMS>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IHttpContextAccessor _httpContext;

        public ProcesarAbonoPrestamoPostProcessor(IMediator mediador, IRepositorioAgente repositorioAgente, IHttpContextAccessor httpContext)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _httpContext = httpContext;
        }
        public async Task Process(ProcesarAbonoPrestamoME request, EfectivizacionPrestamoMS response, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);
            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);
            var valores = new Dictionary<string, string>();
            var comision = jsonNegocio.AbonoPrestamos.Comisiones.AdministracionCanal + jsonNegocio.Deposito.Comisiones.Agente + jsonNegocio.Deposito.Comisiones.Cooperativa;
            var detalle = $"<ul><li>Operación: Abono de Préstamos</li><li>Tipo de Préstamo: {request.TipoPrestamo}</li><li>No de Préstamo: {request.NumeroPrestamo}</li><li>Valor: {request.Valor}</li><li>Comisión: {comision}</li><li>Total: {request.Valor}</li><li>Descripción:</li></ul>";
            valores.Add("[:detalle:]", detalle);

            if (jsonNegocio.AbonoPrestamos.NotificarCorreoElectronico && string.IsNullOrEmpty(jsonNegocio.AbonoPrestamos.PlantillaCorreoElectronico))
            {
                var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "EmailTemplate", "index.html");

                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    jsonNegocio.AbonoPrestamos.PlantillaCorreoElectronico = SourceReader.ReadToEnd();
                }
            }

            if (jsonNegocio.AbonoPrestamos.NotificarSMS && string.IsNullOrEmpty(jsonNegocio.AbonoPrestamos.PlantillaSMS))
            {
                var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "SmsTemplate", "template.txt");

                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    jsonNegocio.AbonoPrestamos.PlantillaSMS = SourceReader.ReadToEnd();
                }
            }

            var valoresSMS = new Dictionary<string, string>();
            detalle = $"Operación: Abono de Préstamos\nNo de Préstamo: {request.NumeroPrestamo}\nTotal: {request.Valor}";
            valoresSMS.Add("[:detalle:]", detalle);

            await _mediador.Publish(new NotificacionME
            {
                PlantillaCorreoElectronico = jsonNegocio.AbonoPrestamos.NotificarCorreoElectronico ? jsonNegocio.AbonoPrestamos.PlantillaCorreoElectronico : null,
                PlantillaSMS = jsonNegocio.AbonoPrestamos.NotificarSMS ? jsonNegocio.AbonoPrestamos.PlantillaSMS : null,
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
