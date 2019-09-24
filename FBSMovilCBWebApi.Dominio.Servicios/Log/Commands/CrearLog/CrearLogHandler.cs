using AutoMapper;
using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands
{
    public class CrearLogHandler : IRequestHandler<CrearLogME, string>
    {
        private readonly IRepositorioLog _repositorio;
        private readonly IMapper _mapper;

        public CrearLogHandler(IRepositorioLog repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<string> Handle(CrearLogME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Log>(request);
            var identificador = await _repositorio.Add(_model);
            return identificador;
        }
    }
}
