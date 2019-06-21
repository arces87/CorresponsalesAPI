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
    public class OficinaService : IOficinaService
    {
        private readonly IOficinaRepository _repository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly ICorresponsalRepository _corresponsalRepository;

        public OficinaService(IOficinaRepository repository, IEmpresaRepository empresaRepository, ICorresponsalRepository corresponsalRepository)
        {
            _repository = repository;
            _corresponsalRepository = corresponsalRepository;
            _empresaRepository = empresaRepository;
        }

        public IEnumerable<OficinaModel> List()
        {
            var _oficinas = _repository.GetAllWithAssociations();
            return Mapper.Map<IEnumerable<OficinaModel>>(_oficinas); ;

        }
        public FuenteDatosModel<OficinaModel> List(PaginacionModel filtro)
        {
            IEnumerable<Oficina> _model = _repository.GetAllWithAssociations();

            var _fuente = Mapper.Map<FuenteDatosModel<OficinaModel>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Oficina>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = Mapper.Map<IEnumerable<OficinaModel>>(_model);
            return _fuente;
        }
        public OficinaModel Get(int Id)
        {
            var _oficina = _repository.Get(Id);
            if (_oficina != null)
                return Mapper.Map<OficinaModel>(_oficina);
            return null;
        }

        public OficinaModel Create(OficinaModel oficina)
        {
            var _oficina = Mapper.Map<Oficina>(oficina);
            _oficina.Empresa = _empresaRepository.Get(oficina.Empresa.Id);
            _repository.Add(_oficina);
            return Mapper.Map<OficinaModel>(_oficina); ;
        }
        public OficinaModel Update(OficinaModel oficina)
        {
            var _oficina = _repository.Get(oficina.Id);
            Mapper.Map(oficina, _oficina);
            _oficina.Empresa = _empresaRepository.Get(oficina.Empresa.Id);
            _repository.Update(_oficina);

            return oficina;
        }
        public OficinaModel Delete(int Id)
        {
            var _oficina = _repository.Get(Id);
            if (_oficina != null)
            {
                _repository.Remove(_oficina);
                var corresponsal = _corresponsalRepository.Where(o => o.Oficina == _oficina);
                foreach (var item in corresponsal)
                {
                    _corresponsalRepository.Remove(item);
                }
                return Mapper.Map<OficinaModel>(_oficina);
            }
            return null;
        }

    }
}
