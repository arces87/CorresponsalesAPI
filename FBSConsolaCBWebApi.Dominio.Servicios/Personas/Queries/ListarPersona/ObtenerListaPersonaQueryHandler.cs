using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Personas.Queries
{
    public class ObtenerListaPersonaQueryHandler : IRequestHandler<ObtenerListaPersonaQuery, ModeloObtenerListaPersona>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaPersonaQueryHandler(IRepositorioPersona repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaPersona> Handle(ObtenerListaPersonaQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations();
            var _retorno = new ModeloObtenerListaPersona();
            var totalElementos = 0;
            Filtro<Persona>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;

            var _personas = _mapper.Map<List<ModeloObtenerDetalleListaPersona>>(_model);
            foreach (var item in _personas)
            {
                var persona = await _repositorio.GetWithAssociations(item.Id);
                if (persona is Supervisor)
                    item.Tipo = "Supervisor";
                else if (persona is Corresponsal)
                    item.Tipo = "Corresponsal";
                else
                    item.Tipo = "Administrador";
            }
            _retorno.Personas = _personas;
            return _retorno;
        }
    }
}
