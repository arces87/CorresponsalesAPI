using AutoMapper;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveTipoCuentaHandler : IRequestHandler<DevuelveTipoCuentaME, TiposCuentaClienteMSL>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;

        public DevuelveTipoCuentaHandler(IFBSCorresponsalesApi financialApi, IMapper mapper, IMediator mediador)
        {
            _financialApi = financialApi;
            _mapper = mapper;
            _mediador = mediador;
        }

        public async Task<TiposCuentaClienteMSL> Handle(DevuelveTipoCuentaME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
                Usuario = request.Usuario,
                Imei = request.Imei,
                Mac = request.Mac,
                Longitud = request.Longitud,
                Latitud = request.Latitud
            });

            var respuesta = await _financialApi.Cuentas.DevuelveTiposDeCuentasDeUnClienteWithHttpMessagesAsync(_mapper.Map<PorSecuencialClienteDeUnaEmpresaProductoVista>(request));
            return respuesta.Body;
        }
    }
}
