using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCBWebApi.DAL.Nomenclador;
using FBSConsolaCBWebApi.Dominio.Modelos.Nomenclador;
using FBSConsolaCBWebApi.Dominio.Servicios.Interfaces.Nomenclador;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Nomenclador
{
    public class ServicioTipoCatalogo : IServicioTipoCatalogo
    {
        private readonly IRepositorioTipoCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ServicioTipoCatalogo(IRepositorioTipoCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModeloTipoCatalogo>> List()
        {
            var _model = await _repositorio.GetAllActive();
            return _mapper.Map<IEnumerable<ModeloTipoCatalogo>>(_model); ;

        }
        public async Task<ModeloFuenteDatos<ModeloTipoCatalogo>> List(ModeloPaginacion filtro)
        {
            IEnumerable<TipoCatalogo> _model = await _repositorio.GetAllActive();

            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloTipoCatalogo>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<TipoCatalogo>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloTipoCatalogo>>(_model);
            return _fuente;
        }

        public async Task<ModeloTipoCatalogo> Get(int Id)
        {
            var _model = await _repositorio.Get(Id);
            if (_model != null)
                return _mapper.Map<ModeloTipoCatalogo>(_model);
            return null;
        }

        public async Task<ModeloTipoCatalogo> Update(ModeloTipoCatalogo model)
        {
            var _model = await _repositorio.Get(model.Id);
            _mapper.Map(model, _model);
            await _repositorio.Update(_model);

            return model;
        }

    }
}
