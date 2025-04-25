using AutoMapper;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class EliminarAgenteHandler : IRequestHandler<EliminarAgenteME, bool>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IRepositorioDispositivoAgente _repositorioDispositivoAgente;
        private readonly IMapper _mapper;

        public EliminarAgenteHandler(IRepositorioAgente repositorio, IMapper mapper, IRepositorioDispositivoAgente repositorioDispositivoAgente)
        {
            _repositorio = repositorio;
            _repositorioDispositivoAgente = repositorioDispositivoAgente;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarAgenteME request, CancellationToken cancellationToken)
        {
            if (request.Eliminar)
            {
                var _model = _mapper.Map<Agente>(request);
                await _repositorio.Remove(_model);
                await _repositorioDispositivoAgente.DesactivarAgenteDispositivo(_model.Id.ToString());
            }
            else
            {
                var _model = _mapper.Map<Agente>(request);
                await _repositorio.Desactivar(_model);
            }      
            return true;
        }
    }
}
