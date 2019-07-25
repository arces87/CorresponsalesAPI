using AutoMapper;
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
            return new ModeloObtenerListaPersona() { Personas = _personas };
        }
    }
}
