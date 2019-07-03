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
    public class ServicioDispositivo : IServicioDispositivo
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public ServicioDispositivo(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModeloDispositivo>> List()
        {
            var _model = await _repositorio.GetAllWithAssociations();
            return _mapper.Map<IEnumerable<ModeloDispositivo>>(_model);
        }

        public async Task<ModeloFuenteDatos<ModeloDispositivo>> List(ModeloPaginacion filtro)
        {
            IEnumerable<Dispositivo> _model = await _repositorio.GetAllWithAssociations();

            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloDispositivo>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Dispositivo>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloDispositivo>>(_model);
            return _fuente;
        }

        public async Task<ModeloDispositivo> Get(int Id)
        {
            var _model = await _repositorio.GetWithAssociations(Id);
            if (_model != null)
                return _mapper.Map<ModeloDispositivo>(_model);
            return null;
        }

        public async Task<ModeloDispositivo> Create(ModeloDispositivo model)
        {
            var _model = _mapper.Map<Dispositivo>(model);
            await _repositorio.Add(_model);
            return _mapper.Map<ModeloDispositivo>(_model);
        }
        public async Task<ModeloDispositivo> Update(ModeloDispositivo model)
        {
            var _model = await _repositorio.GetWithAssociations(model.Id);
            _mapper.Map(model, _model);
            await _repositorio.Update(_model);

            return model;
        }
        public async Task<ModeloDispositivo> Delete(int Id)
        {
            var _model = await _repositorio.Get(Id);
            if (_model != null)
            {
                await _repositorio.Remove(_model);
                return _mapper.Map<ModeloDispositivo>(_model);
            }
            return null;
        }
    }
}
