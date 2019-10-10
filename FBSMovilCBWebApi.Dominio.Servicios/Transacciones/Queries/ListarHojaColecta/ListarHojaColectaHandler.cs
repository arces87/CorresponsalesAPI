using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarHojaColectaHandler : IRequestHandler<ListarHojaColectaME, ListarHojaColectaMS>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IRepositorioAgente _repositorioAgente;
        private readonly IMapper _mapper;

        public ListarHojaColectaHandler(IRepositorioTransaccion repositorio, IRepositorioAgente repositorioAgente, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioAgente = repositorioAgente;
            _mapper = mapper;
        }

        public async Task<ListarHojaColectaMS> Handle(ListarHojaColectaME request, CancellationToken cancellationToken)
        {
            var agente = await _repositorioAgente.GetForUserName(request.Usuario);
            var transacciones = await _repositorio.GetForAgente(agente.Id.ToString());
            transacciones = transacciones.Where(t => !t.ReposicionRealizada).ToList();
            var _retorno = new ListarHojaColectaMS();
            _retorno.Transacciones = _mapper.Map<IEnumerable<ModeloListarHojaColecta>>(transacciones);
            return _retorno;
        }
    }
}
