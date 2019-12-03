using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ObtenerAgenteHandler : IRequestHandler<ObtenerAgenteME, ObtenerAgenteMS>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IMapper _mapper;

        public ObtenerAgenteHandler(IRepositorioAgente repositorio, IRepositorioCuenta repositorioCuenta, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioCuenta = repositorioCuenta;
            _mapper = mapper;
        }

        public async Task<ObtenerAgenteMS> Handle(ObtenerAgenteME request, CancellationToken cancellationToken)
        {
            var agente = _mapper.Map<ObtenerAgenteMS>(await _repositorio.GetWithAssociations(request.Id));
            var cuenta = await _repositorioCuenta.GetForAgente(agente.Id);
            if (cuenta != null)
                _mapper.Map(cuenta, agente);
            return agente;
        }
    }
}
