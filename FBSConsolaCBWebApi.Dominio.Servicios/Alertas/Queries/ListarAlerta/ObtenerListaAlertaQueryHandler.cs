using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ObtenerListaAlertaQueryHandler : IRequestHandler<ObtenerListaAlertaQuery, ModeloObtenerListaAlerta>
    {
        private readonly IRepositorioAlerta _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaAlertaQueryHandler(IRepositorioAlerta repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaAlerta> Handle(ObtenerListaAlertaQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            var _retorno = new ModeloObtenerListaAlerta();
            _retorno.TotalElementos = _model.Count();
            Filtro<Alerta>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request));
            _retorno.Alertas = _mapper.Map<List<ModeloObtenerDetalleListaAlerta>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
