using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using FBSConsolaCB_WebApi.Dominio.Modelos.Nomenclador;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.Nomenclador;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.Nomenclador
{
    public class TipoCatalogoService : ITipoCatalogoService
    {
        private readonly IRepositorioTipoCatalogo _Repositorio;
        private readonly IMapper _mapper;

        public TipoCatalogoService(IRepositorioTipoCatalogo Repositorio, IMapper mapper)
        {
            _Repositorio = Repositorio;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModeloTipoCatalogo>> List()
        {
            var _model = await _Repositorio.GetAllActive();
            return _mapper.Map<IEnumerable<ModeloTipoCatalogo>>(_model); ;

        }
        public async Task<ModeloFuenteDatos<ModeloTipoCatalogo>> List(ModeloPaginacion filtro)
        {
            IEnumerable<TipoCatalogo> _model = await _Repositorio.GetAllActive();

            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloTipoCatalogo>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<TipoCatalogo>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloTipoCatalogo>>(_model);
            return _fuente;
        }

        public async Task<ModeloTipoCatalogo> Get(int Id)
        {
            var _model = await _Repositorio.Get(Id);
            if (_model != null)
                return _mapper.Map<ModeloTipoCatalogo>(_model);
            return null;
        }

        public async Task<ModeloTipoCatalogo> Update(ModeloTipoCatalogo model)
        {
            var _model = await _Repositorio.Get(model.Id);
            _mapper.Map(model, _model);
            await _Repositorio.Update(_model);

            return model;
        }

    }
}
