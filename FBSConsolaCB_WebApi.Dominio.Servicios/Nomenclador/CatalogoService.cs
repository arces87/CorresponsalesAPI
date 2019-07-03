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
    public class CatalogoService : ICatalogoService
    {
        private readonly IRepositorioCatalogo _Repositorio;
        private readonly IMapper _mapper;

        public CatalogoService(IRepositorioCatalogo Repositorio, IMapper mapper)
        {
            _Repositorio = Repositorio;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModeloCatalogo>> List(int Tipo)
        {
            var _model = await _Repositorio.GetAllWithAssociations();
            if (Tipo != 0)
                _model = _model.ToList().Where(c => c.TipoCatalogo.Id == Tipo).ToList();

            return _mapper.Map<IEnumerable<ModeloCatalogo>>(_model); ;

        }

        public async Task<ModeloFuenteDatos<ModeloCatalogo>> List(ModeloPaginacion filtro)
        {
            IEnumerable<Catalogo> _model = await _Repositorio.GetAllWithAssociations();

            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloCatalogo>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Catalogo>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloCatalogo>>(_model);
            return _fuente;
        }

        public async Task<ModeloCatalogo> Get(int Id)
        {
            var _model = await _Repositorio.GetWithAssociations(Id);
            if (_model != null)
                return _mapper.Map<ModeloCatalogo>(_model);
            return null;
        }

        public async Task<ModeloCatalogo> Create(ModeloCatalogo model)
        {
            var _model = _mapper.Map<Catalogo>(model);
            await _Repositorio.Add(_model);
            return _mapper.Map<ModeloCatalogo>(_model);
        }
        public async Task<ModeloCatalogo> Update(ModeloCatalogo model)
        {
            var _model = await _Repositorio.GetWithAssociations(model.Id);
            _mapper.Map(model, _model);
            await _Repositorio.Update(_model);

            return model;
        }
        public async Task<ModeloCatalogo> Delete(int Id)
        {
            var _model = await _Repositorio.Get(Id);
            if (_model != null)
            {
                await _Repositorio.Remove(_model);
                return _mapper.Map<ModeloCatalogo>(_model);
            }
            return null;
        }
    }
}
