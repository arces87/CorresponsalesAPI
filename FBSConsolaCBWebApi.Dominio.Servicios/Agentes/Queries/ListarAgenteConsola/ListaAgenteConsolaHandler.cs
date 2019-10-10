using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBS.Identidad.DAL.Modelado;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ListaAgenteConsolaHandler : IRequestHandler<ListaAgenteConsolaME, ListaAgenteConsolaMS>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IRepositorioTransaccion _repositorioTransaccion;
        private readonly IRepositorioAlerta _repositorioAlerta;
        private readonly IJsonConfiguracion _jsonConfiguracion;
        private readonly IMapper _mapper;

        public ListaAgenteConsolaHandler(IRepositorioAgente repositorio, IRepositorioAlerta repositorioAlerta, IRepositorioTransaccion repositorioTransaccion,
            IJsonConfiguracion jsonConfiguracion, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioAlerta = repositorioAlerta;
            _repositorioTransaccion = repositorioTransaccion;
            _jsonConfiguracion = jsonConfiguracion;
            _mapper = mapper;
        }

        public async Task<ListaAgenteConsolaMS> Handle(ListaAgenteConsolaME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations();
            var _retorno = new ListaAgenteConsolaMS();
            var totalElementos = 0;
            Filtro<Agente>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Agentes = new List<ModeloListaAgenteConsola>();
            foreach (var item in _model)
            {
                var transacciones = await _repositorioTransaccion.GetForAgente(item.Id.ToString());
                var comisiones = 0.0;
                transacciones = transacciones.Where(t => !t.ReposicionRealizada).ToList();
                foreach (var transaccion in transacciones)
                {
                    var comision = JsonConvert.DeserializeObject<ComsionAgente>(transaccion.Comisiones);
                    comisiones += comision.Agente + comision.Cooperativa + comision.AdministracionCanal;
                }
                var alertas = await _repositorioAlerta.GetForAgente(item.Id.ToString());
                var saldoDisponible = await _repositorioTransaccion.GetSaldoActual(item.Id.ToString());
                _retorno.Agentes.Add(new ModeloListaAgenteConsola()
                {
                    ExistenciaCaja = saldoDisponible,
                    NumeroAlerta = alertas.Count(),
                    Id = item.Id.ToString(),
                    NombreAgente = item.NombreAgente,
                    NumeroTransacciones = transacciones != null ? transacciones.Count() : 0,
                    Ubicacion = item.Ubicacion,
                    ValorComision = comisiones,
                    ValorReposicion = transacciones != null ? transacciones.Sum(t => t.Valor) : 0,
                    Estado = item.Estado.Id.ToString() == _jsonConfiguracion.Parametrizaciones.FirstOrDefault(j => j.Llave == "AgenteIdEstadoActivo").Valor ? true : false
                });
            }
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
