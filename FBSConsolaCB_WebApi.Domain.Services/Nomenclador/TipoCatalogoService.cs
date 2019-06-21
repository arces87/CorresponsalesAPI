using AutoMapper;
using FBS_Core.Base.Domain.Models.Filtro;
using FBS_Core.Base.Domain.Services.Utilidades;
using FBSConsolaCB_WebApi.DAL.Nomenclador;
using FBSConsolaCB_WebApi.Domain.Models.Nomenclador;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.Nomenclador;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Domain.Services.Nomenclador
{
    public class TipoCatalogoService : ITipoCatalogoService
    {
        private readonly ITipoCatalogoRepository _repository;

        public TipoCatalogoService(ITipoCatalogoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<TipoCatalogoModel> List()
        {
            var _model = _repository.GetAllActive();
            return Mapper.Map<IEnumerable<TipoCatalogoModel>>(_model); ;

        }
        public FuenteDatosModel<TipoCatalogoModel> List(PaginacionModel filtro)
        {
            IEnumerable<TipoCatalogo> _model = _repository.GetAllActive();

            var _fuente = Mapper.Map<FuenteDatosModel<TipoCatalogoModel>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<TipoCatalogo>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = Mapper.Map<IEnumerable<TipoCatalogoModel>>(_model);
            return _fuente;
        }

        public TipoCatalogoModel Get(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
                return Mapper.Map<TipoCatalogoModel>(_model);
            return null;
        }

        public async Task<TipoCatalogoModel> Update(TipoCatalogoModel model)
        {
            var _model = _repository.Get(model.Id);
            Mapper.Map(model, _model);
            _repository.Update(_model);

            return model;
        }

    }
}
