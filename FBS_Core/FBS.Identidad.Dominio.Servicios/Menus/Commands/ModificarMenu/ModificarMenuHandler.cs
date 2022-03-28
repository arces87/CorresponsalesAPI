using AutoMapper;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Menus.Commands
{
    public class ModificarMenuHandler : IRequestHandler<ModificarMenuME, string>
    {
        private readonly IRepositorioMenu _repositorio;
        private readonly IMapper _mapper;

        public ModificarMenuHandler(IRepositorioMenu repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<string> Handle(ModificarMenuME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.Get(request.Id);
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            return _model.Id.ToString();
        }
    }
}
