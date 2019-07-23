using AutoMapper;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Commands
{
    public class EliminarAlertaCommandHandle : IRequestHandler<EliminarAlertaCommand, bool>
    {
        private readonly IRepositorioAlerta _repositorio;
        private readonly IMapper _mapper;

        public EliminarAlertaCommandHandle(IRepositorioAlerta repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarAlertaCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Alerta>(request);
            await _repositorio.Remove(_model);
            return true;
        }
    }
}
