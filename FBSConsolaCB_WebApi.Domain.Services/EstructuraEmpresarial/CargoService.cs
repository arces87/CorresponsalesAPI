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
        private readonly IMapper _mapper;

        public CargoService(ICargoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<CargoModel> List()
        {
            var _model = _repository.GetAllActive();
            return _mapper.Map<IEnumerable<CargoModel>>(_model);

        }
        public FuenteDatosModel<CargoModel> List(PaginacionModel filtro)
        {
            IEnumerable<Cargo> _model = _repository.GetAllActive();

            var _fuente = _mapper.Map<FuenteDatosModel<CargoModel>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Cargo>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<CargoModel>>(_model);
            return _fuente;
        }
        public CargoModel Get(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
                return _mapper.Map<CargoModel>(_model);
            return null;
        }

        public CargoModel Create(CargoModel model)
        {
            var _model = _mapper.Map<Cargo>(model);
            _repository.Add(_model);
            return _mapper.Map<CargoModel>(_model);
        }
        public CargoModel Update(CargoModel model)
        {
            var _model = _repository.Get(model.Id);
            _mapper.Map(model, _model);
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
                return _mapper.Map<CargoModel>(_model);
            }
            return null;
        }

    }
}
