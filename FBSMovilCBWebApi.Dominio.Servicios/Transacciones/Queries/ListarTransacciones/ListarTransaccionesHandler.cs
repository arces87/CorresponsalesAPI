using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTransaccionesHandler : IRequestHandler<ListarTransaccionesME, ListarTransaccionesMS>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IMapper _mapper;
        private readonly IMediator _mediador;

        public ListarTransaccionesHandler(IRepositorioTransaccion repositorio, IRepositorioAgente repositorioAgente, IMapper mapper, IMediator mediador)
        {
            _repositorio = repositorio;
            _repositorioAgente = repositorioAgente;
            _mapper = mapper;
            _mediador = mediador;
        }

        public async Task<ListarTransaccionesMS> Handle(ListarTransaccionesME request, CancellationToken cancellationToken)
        {
            await _mediador.Send(new VerificarAgenteME()
            {
               Imei = request.Imei,
               Mac =  request.Mac,
               Latitud = request.Latitud,
               Longitud = request.Longitud,
               Usuario = request.Usuario
            });

            var agente = await _repositorioAgente.GetForUserName(request.Usuario);
            var transacciones = await _repositorio.GetForAgente(agente.Id.ToString());
            transacciones = transacciones.Where(t => t.FechaSistema >= request.FechaInicio && t.FechaSistema <= request.FechaFin).ToList();
            var _retorno = new ListarTransaccionesMS();
            _retorno.Transacciones = _mapper.Map<IEnumerable<ModeloTransaccion>>(transacciones);
            return _retorno;
        }
    }
}
