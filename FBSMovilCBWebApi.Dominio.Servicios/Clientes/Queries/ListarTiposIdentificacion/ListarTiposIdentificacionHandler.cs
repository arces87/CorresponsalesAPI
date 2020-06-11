using AutoMapper;
using FBSConsolaCBWebApi.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries.ListarTiposIdentificacion
{
    public class ListarTiposIdentificacionHandler: IRequestHandler<ListarTiposIdentificacionME, TiposIdentificacionMSL>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;
        public ListarTiposIdentificacionHandler(IFBSCorresponsalesApi financialApi, IMapper mapper, IMediator mediador, IApiKeyGenerator apiKeyGenerator, IRepositorioAgente repositorioAgente)
        {
            _financialApi = financialApi;
            _mapper = mapper;
            _mediador = mediador;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
        }

        public async Task<TiposIdentificacionMSL> Handle(ListarTiposIdentificacionME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud
            });
            var agente = await _repositorioAgente.GetForUserName(request.Usuario);
            var apiKey = _apiKeyGenerator.generateApiKey(agente.Dispositivo.Imei);
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apiKey);
            var respuesta = await _financialApi.Clientes.DevuelveTiposIdentificacionWithHttpMessagesAsync(customHeaders);

            return respuesta.Body;
        }
    }
}
