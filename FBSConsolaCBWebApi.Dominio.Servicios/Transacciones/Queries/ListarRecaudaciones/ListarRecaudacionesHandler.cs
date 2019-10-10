using AutoMapper;
using FBS.Identidad.DAL.Modelado;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarRecaudacionesHandler : IRequestHandler<ListarRecaudacionesME, ListarRecaudacionesMS>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IMapper _mapper;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public ListarRecaudacionesHandler(IRepositorioTransaccion repositorio, IMapper mapper, IJsonConfiguracion jsonConfiguracion)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _jsonConfiguracion = jsonConfiguracion;
        }

        public async Task<ListarRecaudacionesMS> Handle(ListarRecaudacionesME request, CancellationToken cancellationToken)
        {
            var idDeposito = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDeposito")?.Valor;
            var idRetiro = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdRetiro")?.Valor;
            var idCobroServicio = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCobroServicio")?.Valor;
            var valorDeposito = 0.0;
            var valorRetiro = 0.0;
            var valorCobroServicio = 0.0;
            var _retorno = new ListarRecaudacionesMS();
            _retorno.MontoCaja = await _repositorio.GetSaldoActual(request.IdAgente);
            if (idDeposito != null)
            {

                var _model = await _repositorio.GetForTipo(idDeposito, request.IdAgente);
                valorDeposito = _model.Sum(m => m.Valor);
                var lista = _model != null ? _mapper.Map<IEnumerable<ModeloTransaccion>>(_model) : new List<ModeloTransaccion>();
                _retorno.Deposito = new ModeloListaRecaudaciones() { Total = valorDeposito, Lista = lista };
            }
            if (idRetiro != null)
            {
                var _model = await _repositorio.GetForTipo(idRetiro, request.IdAgente);
                valorRetiro = _model.Sum(m => m.Valor);
                var lista = _model != null ? _mapper.Map<IEnumerable<ModeloTransaccion>>(_model) : new List<ModeloTransaccion>();
                _retorno.Retiro = new ModeloListaRecaudaciones() { Total = valorRetiro, Lista = lista };
            }
            if (idCobroServicio != null)
            {
                var _model = await _repositorio.GetForTipo(idCobroServicio, request.IdAgente);
                valorCobroServicio = _model.Sum(m => m.Valor);
                var lista = _model != null ? _mapper.Map<IEnumerable<ModeloTransaccion>>(_model) : new List<ModeloTransaccion>();
                _retorno.CobroServicios = new ModeloListaRecaudaciones() { Total = valorCobroServicio, Lista = lista };
            }
            return _retorno;
        }
    }
}
