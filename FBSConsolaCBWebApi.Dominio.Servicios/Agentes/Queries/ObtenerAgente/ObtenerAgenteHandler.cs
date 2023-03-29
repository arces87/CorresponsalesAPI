using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
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
        private readonly IRepositorioDispositivoAgente _repositorioDispositivoAgente;
        private readonly IRepositorioDispositivo _repositorioDispositivo;
        private readonly IMapper _mapper;

        public ObtenerAgenteHandler(IRepositorioAgente repositorio, IRepositorioCuenta repositorioCuenta, IRepositorioDispositivoAgente repositorioDispositivoAgente, IRepositorioDispositivo repositorioDispositivo, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioCuenta = repositorioCuenta;
            _repositorioDispositivoAgente = repositorioDispositivoAgente;
            _repositorioDispositivo = repositorioDispositivo;
            _mapper = mapper;
        }

        public async Task<ObtenerAgenteMS> Handle(ObtenerAgenteME request, CancellationToken cancellationToken)
        {
            var agente = _mapper.Map<ObtenerAgenteMS>(await _repositorio.GetWithAssociations(request.Id));
            var cuenta = await _repositorioCuenta.GetForAgente(agente.Id);
            var dispositivoagente = await _repositorioDispositivoAgente.GetForAgente(agente.Id);
            agente.IdDispositivo = dispositivoagente.DispositivoId.ToString();
            var dispositivo = await _repositorioDispositivo.GetWithAssociations(dispositivoagente.DispositivoId.ToString());
            agente.NombreDispositivo = dispositivo.Marca.Descripcion +" - "+ dispositivo.Modelo + " - " + dispositivo.Imei;
            if (cuenta != null)
                _mapper.Map(cuenta, agente);
            return agente;
        }
    }
}
