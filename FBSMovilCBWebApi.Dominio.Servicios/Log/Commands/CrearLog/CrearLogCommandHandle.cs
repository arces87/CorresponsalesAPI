using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands
{
    public class CrearLogCommandHandle : IRequestHandler<CrearLogCommand, int>
    {
        private readonly IRepositorioLog _repositorio;
        private readonly IMapper _mapper;

        public CrearLogCommandHandle(IRepositorioLog repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(CrearLogCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Log>(request);
            await _repositorio.Add(_model);
            return 0;
        }
    }
}
