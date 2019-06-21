using AutoMapper;
using FBS_Core.Base.Domain.Models.Filtro;
using FBS_Core.Base.Domain.Services.Utilidades;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using FBSConsolaCB_WebApi.Domain.Models.Nomenclador;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.Nomenclador;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador;
using System.Collections.Generic;
using System.Linq;

namespace FBSConsolaCB_WebApi.Domain.Services.Nomenclador
{
    public class CatalogoService : ICatalogoService
    {
        private readonly ICatalogoRepository _repository;
        private readonly IMapper _mapper;

        public CatalogoService(ICatalogoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<CatalogoModel> List(int Tipo)
        {
            var _model = _repository.GetAllWithAssociations();
            if (Tipo != 0)
                _model = _model.Where(c => c.TipoCatalogo.Id == Tipo).ToList();

            return _mapper.Map<IEnumerable<CatalogoModel>>(_model); ;

        }

        public FuenteDatosModel<CatalogoModel> List(PaginacionModel filtro)
        {
            IEnumerable<Catalogo> _model = _repository.GetAllWithAssociations();

            var _fuente = _mapper.Map<FuenteDatosModel<CatalogoModel>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Catalogo>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<CatalogoModel>>(_model);
            return _fuente;
        }

        public CatalogoModel Get(int Id)
        {
            var _model = _repository.GetWithAssociations(Id);
            if (_model != null)
                return _mapper.Map<CatalogoModel>(_model);
            return null;
        }

        public CatalogoModel Create(CatalogoModel model)
        {
            var _model = _mapper.Map<Catalogo>(model);
            _repository.Add(_model);
            return _mapper.Map<CatalogoModel>(_model);
        }
        public CatalogoModel Update(CatalogoModel model)
        {
            var _model = _repository.GetWithAssociations(model.Id);
            _mapper.Map(model, _model);
            _repository.Update(_model);

            return model;
        }
        public CatalogoModel Delete(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
            {
                _repository.Remove(_model);
                return _mapper.Map<CatalogoModel>(_model);
            }
            return null;
        }
    }
}
