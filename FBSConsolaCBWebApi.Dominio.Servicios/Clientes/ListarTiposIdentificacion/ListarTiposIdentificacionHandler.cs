using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;
        public ListarTiposIdentificacionHandler(IFBSCorresponsalesApi financialApi, IMapper mapper, IMediator mediador)
        {
            _financialApi = financialApi;
            _mapper = mapper;
        }

        public async Task<TiposIdentificacionMSL> Handle(ListarTiposIdentificacionME request, CancellationToken cancellationToken)
        {
            return await _financialApi.Clientes.DevuelveTiposIdentificacionAsync();
        }
    }
}
