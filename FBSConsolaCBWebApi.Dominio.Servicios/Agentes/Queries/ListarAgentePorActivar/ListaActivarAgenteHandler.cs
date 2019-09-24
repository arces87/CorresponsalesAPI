using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ListaActivarAgenteHandler : IRequestHandler<ListaActivarAgenteME, ListaActivarAgenteMS>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IRepositorioGeolocalizacion _repositorioGeolocalizacion;
        private readonly IMapper _mapper;

        public ListaActivarAgenteHandler(IRepositorioAgente repositorio, IRepositorioGeolocalizacion repositorioGeolocalizacion, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioGeolocalizacion = repositorioGeolocalizacion;
            _mapper = mapper;
        }

        public async Task<ListaActivarAgenteMS> Handle(ListaActivarAgenteME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetForActivation();
            var _retorno = new ListaActivarAgenteMS();
            var totalElementos = 0;
            Filtro<Agente>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Agentes = _mapper.Map<List<ModeloListaActivarAgente>>(_model);
            foreach (var item in _retorno.Agentes)
            {
                var geolocalizacion = await _repositorioGeolocalizacion.GetForAgente(item.Id);
                if (geolocalizacion != null)
                    _mapper.Map(geolocalizacion, item);
            }
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
