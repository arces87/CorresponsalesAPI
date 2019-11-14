using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBS.Identidad.DAL.Modelado;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Queries
{
    public class ListarTransaccionHandler : IRequestHandler<ListarTransaccionME, ListarTransaccionMS>
    {
        private readonly IRepositorioTransaccion _repositorio;
        private readonly IRepositorioCatalogo _repositorioCatalogo;
        private readonly IMapper _mapper;
        private readonly IJsonConfiguracion _jsonConfiguracion;

        public ListarTransaccionHandler(IRepositorioTransaccion repositorio, IMapper mapper, IRepositorioCatalogo repositorioCatalogo,
            IJsonConfiguracion jsonConfiguracion)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _repositorioCatalogo = repositorioCatalogo;
            _jsonConfiguracion = jsonConfiguracion;
        }

        public async Task<ListarTransaccionMS> Handle(ListarTransaccionME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations(request.IdAgente);
            var _retorno = new ListarTransaccionMS();
            var totalElementos = 0;
            Filtro<Transaccion>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Transacciones = _mapper.Map<List<ModeloListaTransaccion>>(_model);
            _retorno.Caja = await _repositorio.GetSaldoActual(request.IdAgente);
            var idDeposito = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdDeposito")?.Valor;
            var idRetiro = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdRetiro")?.Valor;
            var idCobroServicio = _jsonConfiguracion.Parametrizaciones.FirstOrDefault(p => p.Llave == "IdCobroServicio")?.Valor;
            if (idDeposito != null)
            {
                var modelo = await _repositorio.GetForTipo(idDeposito, request.IdAgente);
                _retorno.Deposito = modelo.Sum(m => m.Valor);
            }
            if (idRetiro != null)
            {
                var modelo = await _repositorio.GetForTipo(idRetiro, request.IdAgente);
                _retorno.Retiro = modelo.Sum(m => m.Valor);
            }
            if (idCobroServicio != null)
            {
                var modelo = await _repositorio.GetForTipo(idCobroServicio, request.IdAgente);
                _retorno.CobroServicios = modelo.Sum(m => m.Valor);
            }
            foreach (var item in _retorno.Transacciones)
            {
                item.NombreTipo = (await _repositorioCatalogo.GetWithAssociations(item.Tipo)).Nombre;
            }
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
