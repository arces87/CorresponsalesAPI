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
        private readonly IMapper _mapper;

        public OficinaService(IOficinaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<OficinaModel> List()
        {
            var _oficinas = _repository.GetAllWithAssociations();
            return _mapper.Map<IEnumerable<OficinaModel>>(_oficinas); ;

        }
        public FuenteDatosModel<OficinaModel> List(PaginacionModel filtro)
        {
            IEnumerable<Oficina> _model = _repository.GetAllWithAssociations();

            var _fuente = _mapper.Map<FuenteDatosModel<OficinaModel>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Oficina>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<OficinaModel>>(_model);
            return _fuente;
        }
        public OficinaModel Get(int Id)
        {
            var _oficina = _repository.Get(Id);
            if (_oficina != null)
                return _mapper.Map<OficinaModel>(_oficina);
            return null;
        }

        public OficinaModel Create(OficinaModel oficina)
        {
            var _oficina = _mapper.Map<Oficina>(oficina);
            _repository.Add(_oficina);
            return _mapper.Map<OficinaModel>(_oficina); ;
        }
        public OficinaModel Update(OficinaModel oficina)
        {
            var _oficina = _repository.Get(oficina.Id);
            _mapper.Map(oficina, _oficina);
            _repository.Update(_oficina);

            return oficina;
        }
        public OficinaModel Delete(int Id)
        {
            var _oficina = _repository.Get(Id);
            if (_oficina != null)
            {
                _repository.Remove(_oficina);
                
                return _mapper.Map<OficinaModel>(_oficina);
            }
            return null;
        }

    }
}
