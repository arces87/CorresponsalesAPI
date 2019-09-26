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
    public class ListaAgenteConsolaHandler : IRequestHandler<ListaAgenteConsolaME, ListaAgenteConsolaMS>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IMapper _mapper;

        public ListaAgenteConsolaHandler(IRepositorioAgente repositorio, IRepositorioCuenta repositorioCuenta, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioCuenta = repositorioCuenta;
            _mapper = mapper;
        }

        public async Task<ListaAgenteConsolaMS> Handle(ListaAgenteConsolaME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations();
            var _retorno = new ListaAgenteConsolaMS();
            var totalElementos = 0;
            Filtro<Agente>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Agentes = new List<ModeloListaAgenteConsola>() {
                new ModeloListaAgenteConsola() {
                    ExistenciaCaja =300,
                    NumeroAlerta =1,
                    Id="A8489002-0E14-4AEE-1224-08D73DFC44ED",
                    NombreAgente="Agente",
                    NumeroTransacciones=3,
                    Ubicacion="Casa",
                    ValorComision=4,
                    ValorReposicion=300,
                    Estado=true
                }
            };
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
