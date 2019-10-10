using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTransaccionAgenteHandler : IRequestHandler<ListarTransaccionAgenteME, ListarTransaccionAgenteMS>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IMapper _mapper;

        public ListarTransaccionAgenteHandler(IRepositorioTransaccion repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ListarTransaccionAgenteMS> Handle(ListarTransaccionAgenteME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetForTipo(request.IdTipoTransaccion,request.IdAgente);
            var _retorno = new ListarTransaccionAgenteMS();
            _retorno.Transacciones = _mapper.Map<List<ModeloListaTransaccionAgente>>(_model);
            return _retorno;
        }
    }
}
