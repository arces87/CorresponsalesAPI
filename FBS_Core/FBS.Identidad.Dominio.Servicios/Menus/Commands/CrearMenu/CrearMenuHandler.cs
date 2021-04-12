using AutoMapper;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Menus.Commands
{
    public class CrearMenuHandler : IRequestHandler<CrearMenuME, string>
    {
        private readonly IRepositorioMenu _repositorio;
        private readonly IMapper _mapper;

        public CrearMenuHandler(IRepositorioMenu repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<string> Handle(CrearMenuME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Menu>(request);
            var identificador = await _repositorio.Add(_model);
            return identificador;
        }
    }
}
