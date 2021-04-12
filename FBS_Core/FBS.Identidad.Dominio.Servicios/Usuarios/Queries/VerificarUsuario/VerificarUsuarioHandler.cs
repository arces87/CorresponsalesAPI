using AutoMapper;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Queries
{
    public class VerificarUsuarioHandler : IRequestHandler<VerificarUsuarioME, bool>
    {
        private readonly IRepositorioUsuario _repositorio;

        public VerificarUsuarioHandler(IRepositorioUsuario repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
        }

        public async Task<bool> Handle(VerificarUsuarioME request, CancellationToken cancellationToken)
        {
            if (request.IdUsuario != null && request.IdUsuario != "")
                return await _repositorio.VerificarUserName(request.Codigo, request.IdUsuario);
            return await _repositorio.VerificarUserName(request.Codigo);
        }
    }
}
