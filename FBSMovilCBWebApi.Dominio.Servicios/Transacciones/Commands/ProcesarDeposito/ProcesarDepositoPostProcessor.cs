using FBS.Identidad.Dominio.Servicios.Canales.Queries;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries;
using FBSMovilCBWebApi.Dominio.Servicios.Notificaciones;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using MimeKit;
using Newtonsoft.Json;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ProcesarDepositoPostProcessor : IRequestPostProcessor<ProcesarDepositoME, AfectacionAUnCorresponsalMS>
    {
        private readonly IMediator _mediador;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IHttpContextAccessor _httpContext;
        public ProcesarDepositoPostProcessor(
            IMediator mediador, 
            IRepositorioAgente repositorioAgente, 
            IHttpContextAccessor httpContext)
        {
            _mediador = mediador;
            _repositorioAgente = repositorioAgente;
            _httpContext = httpContext;
        }
        public async Task Process(ProcesarDepositoME request, AfectacionAUnCorresponsalMS response, CancellationToken cancellationToken)
        {

            var agente = await _repositorioAgente.GetForId(_httpContext.HttpContext.User.Identity.Name);    
            var jsonNegocio = JsonConvert.DeserializeObject<JsonNegocioMS>(agente.JsonAgente);
            var valores = new Dictionary<string, string>();
            var comision = jsonNegocio.Deposito.Comisiones.AdministracionCanal + jsonNegocio.Deposito.Comisiones.Agente + jsonNegocio.Deposito.Comisiones.Cooperativa;

            var fechaActual = DateTime.Now.ToString("dd/MM/yyyy/ H:mm");

            if (jsonNegocio.Deposito.NotificarCorreoElectronico)
            {
                if(string.IsNullOrEmpty(jsonNegocio.Deposito.PlantillaCorreoElectronico)) {
                    var pathToFile = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "EmailTemplate", "index_deposito.html");

                    using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                    {
                        jsonNegocio.Deposito.PlantillaCorreoElectronico = SourceReader.ReadToEnd();
                    }
                }

                valores.Add("[:NOMBRECLIENTE:]", request.NombreCliente);
                valores.Add("[:NOMBRECORRESPONSAL:]", agente.NombreAgente);
                valores.Add("[:FECHAACTUAL:]", fechaActual);
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

            await _mediador.Publish(new NotificacionME
            {
                PlantillaCorreoElectronico = jsonNegocio.Deposito.NotificarCorreoElectronico ? jsonNegocio.Deposito.PlantillaCorreoElectronico : null,
                PlantillaSMS = jsonNegocio.Deposito.NotificarSMS ? jsonNegocio.Deposito.PlantillaSMS : null,
                CorreoElectronicoDestinatario = datosCliente.CorreoElectronico,
                NombreDestinatario = request.NombreCliente,
                AsuntoCorreoElectronico = "Operación Déposito realizada con éxito",
                NumeroCliente = 1,
                SecuencialEmpresa = 1,
                ValoresEmail = valores,
                ValoresSms = valoresSMS,
                TipoIdentificacion = request.TipoIdentificacionCliente,
                Identificacion = request.IdentificacionCliente
            });
        }
    }
}
