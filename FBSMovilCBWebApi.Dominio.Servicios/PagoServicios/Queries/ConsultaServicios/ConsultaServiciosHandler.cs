using AutoMapper;
using FBSConsolaCBWebApi.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;
using FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands;
using Newtonsoft.Json;
using FBS.Identidad.DAL.Modelado;
using System.Linq;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class ConsultaServiciosHandler : IRequestHandler<ConsultaServiciosME, ConsultaMS>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;

        public ConsultaServiciosHandler(IFBSCorresponsalesApi facilitoApi, IMapper mapper, IMediator mediador, IApiKeyGenerator apiKeyGenerator, IRepositorioAgente repositorioAgente, IJsonConfiguracion jsonConfiguracion)
        {
            _financialApi = facilitoApi;
            _mapper = mapper;
            _mediador = mediador;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
            _jsonConfiguracion = jsonConfiguracion;
        }

        public async Task<ConsultaMS> Handle(ConsultaServiciosME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud,
                VerificarGeolocalizacion = false
            });

            var agente = await _repositorioAgente.GetForUserName(request.Usuario);
            var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

            var IdTipoAccion = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCobroServicio").Valor;
            
            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(request),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogEnviado").Valor,
            });

            var respuesta = await _financialApi.PagoServiciosPagoAgil.ConsultaWithHttpMessagesAsync(_mapper.Map<ConsultaME>(request), customHeaders);

            await _mediador.Send(new CrearLogME()
            {
                JsonLog = JsonConvert.SerializeObject(respuesta.Body),
                IdTipoAccion = IdTipoAccion,
                IdEstado = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdLogRecibido").Valor,
            });

            return respuesta.Body;
        }
    }
}
