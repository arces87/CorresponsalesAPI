using AutoMapper;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Queries
{
    public class VerificarCorreoElectronicoHandler : IRequestHandler<VerificarCorreoElectronicoME, bool>
    {
        private readonly IRepositorioUsuario _repositorio;

        public VerificarCorreoElectronicoHandler(IRepositorioUsuario repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
        }

        public async Task<bool> Handle(VerificarCorreoElectronicoME request, CancellationToken cancellationToken)
        {
            if (request.IdUsuario != null && request.IdUsuario != "")
                return await _repositorio.VerificarCorreoElectronico(request.CorreoElectronico, request.IdUsuario);
            return await _repositorio.VerificarCorreoElectronico(request.CorreoElectronico);
        }
    }
}
