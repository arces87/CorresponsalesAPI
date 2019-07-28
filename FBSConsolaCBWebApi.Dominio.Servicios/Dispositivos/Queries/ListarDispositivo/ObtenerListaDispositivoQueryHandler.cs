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

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class ObtenerListaDispositivoQueryHandler : IRequestHandler<ObtenerListaDispositivoQuery, ModeloObtenerListaDispositivo>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaDispositivoQueryHandler(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaDispositivo> Handle(ObtenerListaDispositivoQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations();
            var _retorno = new ModeloObtenerListaDispositivo();
            _retorno.TotalElementos = _model.Count();
            Filtro<Dispositivo>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request));
            _retorno.Dispositivos = _mapper.Map<List<ModeloObtenerDetalleListaDispositivo>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
