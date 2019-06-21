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
        private readonly ITipoCatalogoRepository _tipoCatalogoRepository;

        public CatalogoService(ICatalogoRepository repository, ITipoCatalogoRepository tipoCatalogoRepository)
        {
            _repository = repository;
            _tipoCatalogoRepository = tipoCatalogoRepository;
        }

        public IEnumerable<CatalogoModel> List(int Tipo)
        {
            var _model = _repository.GetAllWithAssociations();
            if (Tipo != 0)
                _model = _model.Where(c => c.TipoCatalogo.Id == Tipo).ToList();

            return Mapper.Map<IEnumerable<CatalogoModel>>(_model); ;

        }

        public FuenteDatosModel<CatalogoModel> List(PaginacionModel filtro)
        {
            IEnumerable<Catalogo> _model = _repository.GetAllWithAssociations();

            var _fuente = Mapper.Map<FuenteDatosModel<CatalogoModel>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Catalogo>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = Mapper.Map<IEnumerable<CatalogoModel>>(_model);
            return _fuente;
        }

        public CatalogoModel Get(int Id)
        {
            var _model = _repository.GetWithAssociations(Id);
            if (_model != null)
                return Mapper.Map<CatalogoModel>(_model);
            return null;
        }

        public CatalogoModel Create(CatalogoModel model)
        {
            var _model = Mapper.Map<Catalogo>(model);
            var _tipo = _tipoCatalogoRepository.Get(_model.TipoCatalogo.Id);
            _model.TipoCatalogo = _tipo;
            _repository.Add(_model);
            return Mapper.Map<CatalogoModel>(_model);
        }
        public CatalogoModel Update(CatalogoModel model)
        {
            var _model = _repository.GetWithAssociations(model.Id);
            Mapper.Map(model, _model);
            _model.TipoCatalogo = _tipoCatalogoRepository.Get(_model.TipoCatalogo.Id);
            _repository.Update(_model);

            return model;
        }
        public CatalogoModel Delete(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
            {
                _repository.Remove(_model);
                return Mapper.Map<CatalogoModel>(_model);
            }
            return null;
        }
    }
}
