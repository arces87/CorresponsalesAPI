using AutoMapper;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Commands
{
    public class CrearAlertaCommandHandle : IRequestHandler<CrearAlertaCommand, int>
    {
        private readonly IRepositorioAlerta _repositorio;
        private readonly IMapper _mapper;

        public CrearAlertaCommandHandle(IRepositorioAlerta repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<int> Handle(CrearAlertaCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Alerta>(request);
            await _repositorio.Add(_model);
            return 0;
        }
    }
}
