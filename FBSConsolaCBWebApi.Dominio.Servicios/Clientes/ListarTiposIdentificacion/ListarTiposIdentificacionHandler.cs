using AutoMapper;
using FBS.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.Infraestructura.Utiles;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Clientes.Queries
{
    public class ListarTiposIdentificacionHandler: IRequestHandler<ListarTiposIdentificacionME, TiposIdentificacionMSL>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        public ListarTiposIdentificacionHandler(IFBSCorresponsalesApi financialApi, IApiKeyGenerator apiKeyGenerator)
        {
            _financialApi = financialApi;
            _apiKeyGenerator = apiKeyGenerator;
        }

        public async Task<TiposIdentificacionMSL> Handle(ListarTiposIdentificacionME request, CancellationToken cancellationToken)
        {
           
            var apyKey = _apiKeyGenerator.generateApiKey("000000000000000");
            var customHeaders = _apiKeyGenerator.generateCustomHeaders(apyKey);
            var respuesta = await _financialApi.Clientes.DevuelveTiposIdentificacionWithHttpMessagesAsync(customHeaders);
            return respuesta.Body;
        }
    }
}
