using AutoMapper;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Usuarios.Commands
{
    public class AutenticarUsuarioCommandHandle : IRequestHandler<AutenticarUsuarioCommand, ModeloAutenticacion>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMediator _mediador;
        private readonly IMapper _mapper;

        public AutenticarUsuarioCommandHandle(IRepositorioPersona repositorio, IMediator mediador, IMapper mapper)
        {
            _repositorio = repositorio;
            _mediador = mediador;
            _mapper = mapper;
        }

        public async Task<ModeloAutenticacion> Handle(AutenticarUsuarioCommand request, CancellationToken cancellationToken)
        {
            var _modelo = _mapper.Map<LoginUsuarioCommand>(request);
            var _usuario = _mapper.Map<ModeloAutenticacion>(await _mediador.Send(_modelo));
            if (_usuario.Errores == null && _usuario.Errores != "")
            {
                var persona = await _repositorio.GetForUserName(_modelo.Usuario);
                if (persona != null)
                {
                    _mapper.Map(persona, _usuario);
                }
            }

            return _usuario;
        }
    }
}
