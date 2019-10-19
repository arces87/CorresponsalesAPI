using AutoMapper;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class ModificarAgenteHandler : IRequestHandler<ModificarAgenteME, string>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IMapper _mapper;

        public ModificarAgenteHandler(IRepositorioAgente repositorio, IRepositorioCuenta repositorioCuenta, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioCuenta = repositorioCuenta;
            _mapper = mapper;
        }

        public async Task<string> Handle(ModificarAgenteME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.Get(request.Id);
            _mapper.Map(request, _model);
            await _repositorio.Update(_model);
            if (request.TipoCuenta != null && request.TipoCuenta != "" && request.NumeroCuenta != null && request.NumeroCuenta !="")
            {
                var cuenta = await _repositorioCuenta.GetForAgente(request.Id);

                if (cuenta != null && cuenta.NumeroCuenta != request.NumeroCuenta)
                {
                    await _repositorioCuenta.Remove(cuenta);
                }
                if (cuenta == null || (cuenta != null && cuenta.NumeroCuenta != request.NumeroCuenta))
                {
                    await _repositorioCuenta.Add(new Cuenta()
                    {
                        Agente = _model,
                        Tipo = request.TipoCuenta,
                        NumeroCuenta = request.NumeroCuenta
                    });
                }

            }
            return _model.Id.ToString();
        }
    }
}
