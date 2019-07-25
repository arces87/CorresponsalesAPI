using AutoMapper;
using FBS.Identidad.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Commands
{
    public class ActivarCorresponsalCommandHandle : INotificationHandler<ActivarCorresponsalCommand>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IMapper _mapper;

        public ActivarCorresponsalCommandHandle(IRepositorioPersona repositorio, IRepositorioUsuario repositorioUsuario, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioUsuario = repositorioUsuario;
            _mapper = mapper;
        }

        public async Task Handle(ActivarCorresponsalCommand request, CancellationToken cancellationToken)
        {
            var corresponsal = await _repositorio.GetWithAssociations(request.IdCorresponsal) as Corresponsal;
            var usuario = corresponsal.Persona.Usuario;
            usuario.LockoutEnabled = !usuario.LockoutEnabled;
            await _repositorioUsuario.Update(usuario);
        }
    }
}
