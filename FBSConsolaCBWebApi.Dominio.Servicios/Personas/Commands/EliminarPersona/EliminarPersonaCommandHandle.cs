using AutoMapper;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Personas.Commands
{
    public class EliminarPersonaCommandHandle : IRequestHandler<EliminarPersonaCommand, bool>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public EliminarPersonaCommandHandle(IRepositorioPersona repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarPersonaCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Persona>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
