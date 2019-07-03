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
    public class ServicioLimiteTransaccional : IServicioLimiteTransaccional
    {
        private readonly IRepositorioLimiteTransaccional _repositorio;
        private readonly IMapper _mapper;

        public ServicioLimiteTransaccional(IRepositorioLimiteTransaccional repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModeloLimiteTransaccional>> List()
        {
            var _model = await _repositorio.GetAllWithAssociations();
            return _mapper.Map<IEnumerable<ModeloLimiteTransaccional>>(_model);
        }

        public async Task<ModeloFuenteDatos<ModeloLimiteTransaccional>> List(ModeloPaginacion filtro)
        {
            IEnumerable<LimiteTransaccional> _model = await _repositorio.GetAllWithAssociations();

            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloLimiteTransaccional>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<LimiteTransaccional>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloLimiteTransaccional>>(_model);
            return _fuente;
        }

        public async Task<ModeloLimiteTransaccional> Get(int Id)
        {
            var _model = await _repositorio.GetWithAssociations(Id);
            if (_model != null)
                return _mapper.Map<ModeloLimiteTransaccional>(_model);
            return null;
        }

        public async Task<ModeloLimiteTransaccional> Create(ModeloLimiteTransaccional model)
        {
            var _model = _mapper.Map<LimiteTransaccional>(model);
            await _repositorio.Add(_model);
            return _mapper.Map<ModeloLimiteTransaccional>(_model);
        }
        public async Task<ModeloLimiteTransaccional> Update(ModeloLimiteTransaccional model)
        {
            var _model = await _repositorio.GetWithAssociations(model.Id);
            _mapper.Map(model, _model);
            await _repositorio.Update(_model);

            return model;
        }
        public async Task<ModeloLimiteTransaccional> Delete(int Id)
        {
            var _model = await _repositorio.Get(Id);
            if (_model != null)
            {
                await _repositorio.Remove(_model);
                return _mapper.Map<ModeloLimiteTransaccional>(_model);
            }
            return null;
        }
    }
}
