using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.Consola;
using FBSConsolaCBWebApi.Dominio.Modelos.Consola;
using FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.Consola;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Consola
{
    public class ServicioLimiteExistencia : IServicioLimiteExistencia
    {
        private readonly IRepositorioLimiteExistencia _repositorio;
        private readonly IMapper _mapper;

        public ServicioLimiteExistencia(IRepositorioLimiteExistencia repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModeloLimiteExistencia>> List()
        {
            var _model = await _repositorio.GetAllWithAssociations();
            return _mapper.Map<IEnumerable<ModeloLimiteExistencia>>(_model);
        }

        public async Task<ModeloFuenteDatos<ModeloLimiteExistencia>> List(ModeloPaginacion filtro)
        {
            IEnumerable<LimiteExistencia> _model = await _repositorio.GetAllWithAssociations();

            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloLimiteExistencia>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<LimiteExistencia>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloLimiteExistencia>>(_model);
            return _fuente;
        }

        public async Task<ModeloLimiteExistencia> Get(int Id)
        {
            var _model = await _repositorio.GetWithAssociations(Id);
            if (_model != null)
                return _mapper.Map<ModeloLimiteExistencia>(_model);
            return null;
        }

        public async Task<ModeloLimiteExistencia> Create(ModeloLimiteExistencia model)
        {
            var _model = _mapper.Map<LimiteExistencia>(model);
            await _repositorio.Add(_model);
            return _mapper.Map<ModeloLimiteExistencia>(_model);
        }
        public async Task<ModeloLimiteExistencia> Update(ModeloLimiteExistencia model)
        {
            var _model = await _repositorio.GetWithAssociations(model.Id);
            _mapper.Map(model, _model);
            await _repositorio.Update(_model);

            return model;
        }
        public async Task<ModeloLimiteExistencia> Delete(int Id)
        {
            var _model = await _repositorio.Get(Id);
            if (_model != null)
            {
                await _repositorio.Remove(_model);
                return _mapper.Map<ModeloLimiteExistencia>(_model);
            }
            return null;
        }
    }
}
