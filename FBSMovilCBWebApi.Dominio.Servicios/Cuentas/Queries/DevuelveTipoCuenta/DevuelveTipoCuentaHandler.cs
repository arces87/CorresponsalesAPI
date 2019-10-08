using AutoMapper;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class DevuelveTipoCuentaHandler : IRequestHandler<DevuelveTipoCuentaME, TiposCuentaClienteMSL>
    {
        private readonly IFBSCorresponsalesApi _financialApi;
        private readonly IMapper _mapper;

        public DevuelveTipoCuentaHandler(IFBSCorresponsalesApi financialApi, IMapper mapper)
        {
            _financialApi = financialApi;
            _mapper = mapper;
        }

        public async Task<TiposCuentaClienteMSL> Handle(DevuelveTipoCuentaME request, CancellationToken cancellationToken)
        {
            var respuesta = await _financialApi.Cuentas.DevuelveTiposDeCuentasDeUnClienteWithHttpMessagesAsync(_mapper.Map<PorSecuencialClienteDeUnaEmpresaProductoVista>(request));
            return respuesta.Body;
        }
    }
}
