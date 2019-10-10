using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
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

        public ListarTransaccionesHandler(IRepositorioTransaccion repositorio, IRepositorioAgente repositorioAgente, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioAgente = repositorioAgente;
            _mapper = mapper;
        }

        public async Task<ListarTransaccionesMS> Handle(ListarTransaccionesME request, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForUserName(request.Usuario);
            var transacciones = await _repositorio.GetForAgente(agente.Id.ToString());
            transacciones = transacciones.Where(t => t.FechaSistema >= request.FechaInicio && t.FechaSistema <= request.FechaFin).ToList();
            var _retorno = new ListarTransaccionesMS();
            _retorno.Transacciones = _mapper.Map<IEnumerable<ModeloTransaccion>>(transacciones);
            return _retorno;
        }
    }
}
