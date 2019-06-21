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
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository _repository;
        private readonly IOficinaRepository _oficinaRepository;
        private readonly ICorresponsalRepository _corresponsalRepository;

        public EmpresaService(IEmpresaRepository repository, IOficinaRepository oficinaRepository, ICorresponsalRepository corresponsalRepository)
        {
            _repository = repository;
            _oficinaRepository = oficinaRepository;
            _corresponsalRepository = corresponsalRepository;
        }

        public IEnumerable<EmpresaModel> List()
        {
            var _model = _repository.GetAllActive();
            return Mapper.Map<IEnumerable<EmpresaModel>>(_model);

        }
        public FuenteDatosModel<EmpresaModel> List(PaginacionModel filtro)
        {
            IEnumerable<Empresa> _model = _repository.GetAllActive();

            var _fuente = Mapper.Map<FuenteDatosModel<EmpresaModel>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Empresa>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = Mapper.Map<IEnumerable<EmpresaModel>>(_model);
            return _fuente;
        }
        public EmpresaModel Get(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
                return Mapper.Map<EmpresaModel>(_model);
            return null;
        }

        public EmpresaModel Create(EmpresaModel model)
        {
            var _model = Mapper.Map<Empresa>(model);
            _repository.Add(_model);
            return Mapper.Map<EmpresaModel>(_model);
        }
        public EmpresaModel Update(EmpresaModel model)
        {
            var _model = _repository.Get(model.Id);
            Mapper.Map(model, _model);
            _repository.Update(_model);
            return model;
        }
        public EmpresaModel Delete(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
            {
                _repository.Remove(_model);
                var _oficinas = _oficinaRepository.Where(o => o.Empresa == _model);
                if (_oficinas != null)
                {
                    foreach (var item in _oficinas)
                    {
                        _oficinaRepository.Remove(item);
                        var corresponsal = _corresponsalRepository.Where(o => o.Oficina == item);
                        foreach (var itemC in corresponsal)
                        {
                            _corresponsalRepository.Remove(itemC);
                        }
                    }
                }
                return Mapper.Map<EmpresaModel>(_model);
            }
            return null;
        }

    }
}
