using AutoMapper;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using ServiciosFinancial;
using ServiciosFinancial.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries.ListarTiposIdentificacion
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
            _mediador = mediador;
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

            return await _financialApi.Clientes.DevuelveTiposIdentificacionAsync();
        }
    }
}
