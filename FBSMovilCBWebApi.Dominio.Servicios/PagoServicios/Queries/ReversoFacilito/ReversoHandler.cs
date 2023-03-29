using AutoMapper;
using FBSConsolaCBWebApi.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;
using System;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class ReversoHandler : IRequestHandler<ReversoME, ReversoFacilitoMS>
    {
        private readonly IPagoServiciosFacilitoApi _pagoApi;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;

        public ReversoHandler(IPagoServiciosFacilitoApi pagoApi, IMapper mapper, IMediator mediador, IApiKeyGenerator apiKeyGenerator, IRepositorioAgente repositorioAgente)
        {
            _pagoApi = pagoApi;
            _mapper = mapper;
            _mediador = mediador;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
        }

        public async Task<ReversoFacilitoMS> Handle(ReversoME request, CancellationToken cancellationToken)
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

            //var agente = await _repositorioAgente.GetForUserName(request.Usuario);
            //var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
            //var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);

            var respuesta = await _pagoApi.PagoServiciosFacilitoReversoAsync(request);
            
            return respuesta;             
        }        
    }
}
