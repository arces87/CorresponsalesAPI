using FBSConsolaCBWebApi.Infraestructura.Utiles;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FBS.Infraestructura.Interfaces;
using Corresponsales.Query.Api;
using Corresponsales.Query.Model;
using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.PagoServisios.Queries
{
    public class DevuelveServiciosPorCategoriaHandler : IRequestHandler<DevuelveServiciosPorCategoriaME, DevuelveServiciosResponse>
    {
        private readonly Corresponsales.Query.Api.IPagoApi _pagoApi;
        private readonly IMediator _mediador;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;

        public DevuelveServiciosPorCategoriaHandler(IPagoApi pagoApi, IMediator mediador, IApiKeyGenerator apiKeyGenerator, IRepositorioAgente repositorioAgente)
        {
            _pagoApi = pagoApi;
            _mediador = mediador;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
        }

        public async Task<DevuelveServiciosResponse> Handle(DevuelveServiciosPorCategoriaME request, CancellationToken cancellationToken)
        {
            try
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

                var requestModel = new DevuelveServiciosRequest(request.NombreCategoria);
                var respuesta = await _pagoApi.DevuelveServiciosPorCategoriaAsync(requestModel);

                return respuesta;
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error al obtener servicios por categoría", ex);
            }
        }
    }
}