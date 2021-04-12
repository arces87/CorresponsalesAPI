using AutoMapper;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Menus.Queries
{
    public class ObtenerMenuHandler : IRequestHandler<ObtenerMenuME, ObtenerMenuMS>
    {
        private readonly IRepositorioMenu _repositorio;
        private readonly IMapper _mapper;

        public ObtenerMenuHandler(IRepositorioMenu repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ObtenerMenuMS> Handle(ObtenerMenuME request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ObtenerMenuMS>(await _repositorio.Get(request.Id));
        }

    }
}
