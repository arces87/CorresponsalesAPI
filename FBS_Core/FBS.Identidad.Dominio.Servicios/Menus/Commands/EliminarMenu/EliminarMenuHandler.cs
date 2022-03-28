using AutoMapper;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Menus.Commands
{
    public class EliminarMenuHandler : IRequestHandler<EliminarMenuME, bool>
    {
        private readonly IRepositorioMenu _repositorio;
        private readonly IMapper _mapper;

        public EliminarMenuHandler(IRepositorioMenu repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarMenuME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Menu>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
