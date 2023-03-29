using AutoMapper;
using FBSConsolaCBWebApi.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;
using System;
using Org.OpenAPITools.Model;
using Org.OpenAPITools.Api;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class ConsultaServiciosHandler : IRequestHandler<ConsultaServiciosME, ConsultaValorAPagarMS>
    {
        private readonly IPagoServiciosFacilitoApi _pagoApi;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;

        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;

        public ConsultaServiciosHandler(IPagoServiciosFacilitoApi pagoApi, IMapper mapper, IMediator mediador, IApiKeyGenerator apiKeyGenerator, IRepositorioAgente repositorioAgente)
        {
            _pagoApi = pagoApi;
            _mapper = mapper;
            _mediador = mediador;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
        }

        public async Task<ConsultaValorAPagarMS> Handle(ConsultaServiciosME request, CancellationToken cancellationToken)
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

            var respuesta = await _pagoApi.PagoServiciosFacilitoConsultaValorAPagarAsync(_mapper.Map<ConsultaValorAPagarME>(request));
            
            return respuesta;            
        }
    }
}
