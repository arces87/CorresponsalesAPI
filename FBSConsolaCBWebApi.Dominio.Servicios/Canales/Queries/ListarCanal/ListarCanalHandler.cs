using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Canales.Queries
{
    public class ListarCanalHandler : IRequestHandler<ListarCanalME, ListarCanalMS>
    {
        private readonly IRepositorioCanal _repositorio;
        private readonly IMapper _mapper;

        public ListarCanalHandler(IRepositorioCanal repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ListarCanalMS> Handle(ListarCanalME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations(request.Activos);
            var _retorno = new ListarCanalMS();
            var totalElementos = 0;
            Filtro<Canal>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Catalogos = _mapper.Map<List<ModeloListaCanal>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
