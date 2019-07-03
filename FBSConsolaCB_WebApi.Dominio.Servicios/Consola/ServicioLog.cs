using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCB_WebApi.DAL.Consola;
using FBSConsolaCB_WebApi.Dominio.Modelos.Consola;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Consola;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Consola;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.Consola
{
    public class ServicioLog : IServicioLog
    {
        private readonly IRepositorioLog _repositorio;
        private readonly IMapper _mapper;

        public ServicioLog(IRepositorioLog repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModeloLog>> List()
        {
            var _model = await _repositorio.GetAllWithAssociations();
            return _mapper.Map<IEnumerable<ModeloLog>>(_model);
        }

        public async Task<ModeloFuenteDatos<ModeloLog>> List(ModeloPaginacion filtro)
        {
            IEnumerable<Log> _model = await _repositorio.GetAllWithAssociations();

            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloLog>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Log>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloLog>>(_model);
            return _fuente;
        }

        public async Task<ModeloLog> Get(int Id)
        {
            var _model = await _repositorio.GetWithAssociations(Id);
            if (_model != null)
                return _mapper.Map<ModeloLog>(_model);
            return null;
        }

        public async Task<ModeloLog> Create(ModeloLog model)
        {
            var _model = _mapper.Map<Log>(model);
            await _repositorio.Add(_model);
            return _mapper.Map<ModeloLog>(_model);
        }
    }
}
