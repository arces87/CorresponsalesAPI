using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using Financial_Services_Banca;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Personas.Queries
{
    public class ObtenerPersonaIdentificacionQueryHandler : IRequestHandler<ObtenerPersonaIdentificacionQuery, ObtenerModeloPersonaIdentificacion>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;
        private readonly IFBSBancaApi _bancaVirtual;

        public ObtenerPersonaIdentificacionQueryHandler(IRepositorioPersona repositorio, IMapper mapper, IFBSBancaApi bancaVirtual)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _bancaVirtual = bancaVirtual;
        }

        public async Task<ObtenerModeloPersonaIdentificacion> Handle(ObtenerPersonaIdentificacionQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerModeloPersonaIdentificacion>(await _repositorio.GetWithAssociations(request.Id));
        }
    }
}
