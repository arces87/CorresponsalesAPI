using AutoMapper;
using FBS_Core.Base.Domain.Models.Filtro;
using FBS_Core.Base.Domain.Services.Utilidades;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Linq;

namespace FBSConsolaCB_WebApi.Domain.Services.EstructuraEmpresarial
{
    public class CargoService : ICargoService
    {
        private readonly ICargoRepository _repository;

        public CargoService(ICargoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CargoModel> List()
        {
            var _model = _repository.GetAllActive();
            return Mapper.Map<IEnumerable<CargoModel>>(_model);

        }
        public FuenteDatosModel<CargoModel> List(PaginacionModel filtro)
        {
            IEnumerable<Cargo> _model = _repository.GetAllActive();

            var _fuente = Mapper.Map<FuenteDatosModel<CargoModel>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Cargo>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = Mapper.Map<IEnumerable<CargoModel>>(_model);
            return _fuente;
        }
        public CargoModel Get(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
                return Mapper.Map<CargoModel>(_model);
            return null;
        }

        public CargoModel Create(CargoModel model)
        {
            var _model = Mapper.Map<Cargo>(model);
            _repository.Add(_model);
            return Mapper.Map<CargoModel>(_model);
        }
        public CargoModel Update(CargoModel model)
        {
            var _model = _repository.Get(model.Id);
            Mapper.Map(model, _model);
            if (_model != null)
            {
                _repository.Update(_model);
                return model;
            }
            return null;
        }
        public CargoModel Delete(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
            {
                _repository.Remove(_model);
                return Mapper.Map<CargoModel>(_model);
            }
            return null;
        }

    }
}
