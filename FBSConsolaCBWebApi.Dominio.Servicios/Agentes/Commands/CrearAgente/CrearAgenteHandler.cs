using AutoMapper;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class CrearAgenteHandler : IRequestHandler<CrearAgenteME, string>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IMapper _mapper;

        public CrearAgenteHandler(IRepositorioAgente repositorio, IRepositorioCuenta repositorioCuenta, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioCuenta = repositorioCuenta;
            _mapper = mapper;
        }

        public async Task<string> Handle(CrearAgenteME request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Agente>(request);
            var identificador = await _repositorio.Add(_model);
            if (request.TipoCuenta != null && request.TipoCuenta != ""
                && request.NumeroCuenta != null && request.NumeroCuenta != ""
                && request.SecuencialCuenta != null && request.SecuencialCuenta != "")
                await _repositorioCuenta.Add(new Cuenta()
                {
                    Agente = _model,
                    Tipo = request.TipoCuenta,
                    NumeroCuenta = request.NumeroCuenta,
                    SecuencialCuenta = request.SecuencialCuenta
                });
            return identificador;
        }
    }
}
