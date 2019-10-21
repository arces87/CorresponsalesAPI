using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ListaAgenteHandler : IRequestHandler<ListaAgenteME, ListaAgenteMS>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IMapper _mapper;

        public ListaAgenteHandler(IRepositorioAgente repositorio, IRepositorioCuenta repositorioCuenta, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioCuenta = repositorioCuenta;
            _mapper = mapper;
        }

        public async Task<ListaAgenteMS> Handle(ListaAgenteME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations(request.IdSupervisor);
            var _retorno = new ListaAgenteMS();
            var totalElementos = 0;
            Filtro<Agente>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Agentes = _mapper.Map<List<ModeloListaAgente>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            foreach (var item in _retorno.Agentes)
            {
                var cuenta = await _repositorioCuenta.GetForAgente(item.Id);
                if (cuenta != null)
                    _mapper.Map(cuenta, item);
            }
            return _retorno;
        }
    }
}
