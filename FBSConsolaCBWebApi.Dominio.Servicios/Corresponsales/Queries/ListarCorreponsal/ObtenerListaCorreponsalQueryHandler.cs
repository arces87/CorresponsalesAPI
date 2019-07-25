using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Queries
{
    public class ObtenerListaCorreponsalQueryHandler : IRequestHandler<ObtenerListaCorresponsalQuery, ModeloObtenerListaCorresponsal>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaCorreponsalQueryHandler(IRepositorioPersona repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaCorresponsal> Handle(ObtenerListaCorresponsalQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations();
            var _personas = _mapper.Map<List<ModeloObtenerDetalleListaCorresponsal>>(_model);
            return new ModeloObtenerListaCorresponsal() { Personas = _personas };
        }
    }
}
