using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Commands
{
    public class EliminarUsuarioHandler : IRequestHandler<EliminarUsuarioME, bool>
    {
        private readonly IRepositorioUsuario _repositorio;

        public EliminarUsuarioHandler(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<bool> Handle(EliminarUsuarioME request, CancellationToken cancellationToken)
        {
            var _user = await _repositorio.Get(request.Id);
            await _repositorio.Remove(_user);
            return true;
        }
    }
}
