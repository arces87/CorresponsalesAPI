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
    public class DevuelveDetalleDelServicioHandler : IRequestHandler<DevuelveDetalleDelServicioME, DevuelveServicioDetalleResponse>
    {
        private readonly Corresponsales.Query.Api.IPagoApi _pagoApi;
        private readonly IMediator _mediador;
        private readonly IApiKeyGenerator _apiKeyGenerator;
        private readonly IRepositorioAgente _repositorioAgente;

        public DevuelveDetalleDelServicioHandler(IPagoApi pagoApi, IMediator mediador, IApiKeyGenerator apiKeyGenerator, IRepositorioAgente repositorioAgente)
        {
            _pagoApi = pagoApi;
            _mediador = mediador;
            _apiKeyGenerator = apiKeyGenerator;
            _repositorioAgente = repositorioAgente;
        }

        public async Task<DevuelveServicioDetalleResponse> Handle(DevuelveDetalleDelServicioME request, CancellationToken cancellationToken)
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

                var requestModel = new DevuelveServicioDetalleRequest(request.IdServicio, request.Valor);
                var respuesta = await _pagoApi.DevuelveDetalleDelServicioAsync(requestModel);

                return respuesta;
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error al obtener detalle del servicio", ex);
            }
        }
    }
}