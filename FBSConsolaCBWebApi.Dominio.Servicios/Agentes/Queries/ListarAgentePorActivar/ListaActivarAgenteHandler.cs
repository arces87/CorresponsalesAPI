using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.Corresponsales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ListaActivarAgenteHandler : IRequestHandler<ListaActivarAgenteME, ListaActivarAgenteMS>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IRepositorioGeolocalizacion _repositorioGeolocalizacion;
        private readonly IRepositorioCuenta _repositorioCuenta;
        private readonly IRepositorioDispositivoAgente _repositorioDispositivoAgente;
        private readonly IRepositorioDispositivo _repositorioDispositivo;
        private readonly IMapper _mapper;

        public ListaActivarAgenteHandler(IRepositorioAgente repositorio, IRepositorioGeolocalizacion repositorioGeolocalizacion,
            IRepositorioCuenta repositorioCuenta, IRepositorioDispositivoAgente repositorioDispositivoAgente, IRepositorioDispositivo repositorioDispositivo, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioGeolocalizacion = repositorioGeolocalizacion;
            _repositorioCuenta = repositorioCuenta;
            _repositorioDispositivoAgente = repositorioDispositivoAgente;
            _repositorioDispositivo = repositorioDispositivo;
            _mapper = mapper;
        }

        public async Task<ListaActivarAgenteMS> Handle(ListaActivarAgenteME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetForActivation(request.IdSupervisor);
            var _retorno = new ListaActivarAgenteMS();
            var totalElementos = 0;
            Filtro<Agente>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Agentes = _mapper.Map<List<ModeloListaActivarAgente>>(_model);
            foreach (var item in _retorno.Agentes)
            {
                var dispositivoAgente = await _repositorioDispositivoAgente.GetForAgente(item.Id);
                item.IdDispositivo = dispositivoAgente.DispositivoId.ToString();
                var dispositivo = await _repositorioDispositivo.Get(dispositivoAgente.DispositivoId.ToString());
                item.NombreDispositivo = dispositivo.Marca + " " + dispositivo.Modelo + " " + dispositivo.Imei;
                var geolocalizacion = await _repositorioGeolocalizacion.GetForAgente(item.Id);
                if (geolocalizacion != null)
                    _mapper.Map(geolocalizacion, item);
                var cuenta = await _repositorioCuenta.GetForAgente(item.Id);
                if (cuenta != null)
                    _mapper.Map(cuenta, item);
            }
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
