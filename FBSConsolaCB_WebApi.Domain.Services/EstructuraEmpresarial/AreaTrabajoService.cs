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
    public class AreaTrabajoService : IAreaTrabajoService
    {
        private readonly IAreaTrabajoRepository _repository;

        public AreaTrabajoService(IAreaTrabajoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<AreaTrabajoModel> List()
        {
            var _model = _repository.GetAllActive();
            return Mapper.Map<IEnumerable<AreaTrabajoModel>>(_model); ;

        }

        public FuenteDatosModel<AreaTrabajoModel> List(PaginacionModel filtro)
        {
            IEnumerable<AreaTrabajo> _model = _repository.GetAllActive();

            var _fuente = Mapper.Map<FuenteDatosModel<AreaTrabajoModel>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<AreaTrabajo>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = Mapper.Map<IEnumerable<AreaTrabajoModel>>(_model);
            return _fuente;
        }

        public AreaTrabajoModel Get(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
                return Mapper.Map<AreaTrabajoModel>(_model);
            return null;
        }

        public AreaTrabajoModel Create(AreaTrabajoModel model)
        {
            var _model = Mapper.Map<AreaTrabajo>(model);
            _repository.Add(_model);
            return Mapper.Map<AreaTrabajoModel>(_model); ;
        }

        public AreaTrabajoModel Update(AreaTrabajoModel model)
        {
            var _model = _repository.Get(model.Id);
            Mapper.Map(model, _model);
            _repository.Update(_model);
            return model;
        }

        public AreaTrabajoModel Delete(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
            {
                _repository.Remove(_model);
                return Mapper.Map<AreaTrabajoModel>(_model);
            }
            return null;
        }

    }
}
