using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBS.Identidad.DAL.Seguridad;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ListaUsuariosDiposniblesHandler : IRequestHandler<ListaUsuariosDiposniblesME, ListaUsuariosDiposniblesMS>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IMapper _mapper;

        public ListaUsuariosDiposniblesHandler(IRepositorioAgente repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ListaUsuariosDiposniblesMS> Handle(ListaUsuariosDiposniblesME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetUsuariosDisponibles();
            var _retorno = new ListaUsuariosDiposniblesMS();
            var totalElementos = 0;
            Filtro<Usuario>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Usuarios = _mapper.Map<List<ModeloListaUsuariosDiposnibles>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
