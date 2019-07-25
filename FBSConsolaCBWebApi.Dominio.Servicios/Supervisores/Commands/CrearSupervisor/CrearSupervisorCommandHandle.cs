using AutoMapper;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Commands
{
    public class CrearCorresponsalCommandHandle : IRequestHandler<CrearSupervisorCommand, int>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public CrearCorresponsalCommandHandle(IRepositorioPersona repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(CrearSupervisorCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Supervisor>(request);
            var _segundoNombre = _model.Persona.SegundoNombre != null && _model.Persona.SegundoNombre != "" ? " " + _model.Persona.SegundoNombre : "";
            var _segundoApellido = _model.Persona.SegundoApellido != null && _model.Persona.SegundoApellido != "" ? " " + _model.Persona.SegundoApellido : "";
            _model.Persona.NombreUnido = _model.Persona.PrimerNombre + _segundoNombre + " " + _model.Persona.PrimerApellido + _segundoApellido;
            await _repositorio.Add(_model);
            return 0;
        }
    }
}
