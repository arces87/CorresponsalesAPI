using AutoMapper;
using FBS_Core.Base.Domain.Models.Filtro;
using FBS_Core.Base.Domain.Services.Utilidades;
using FBS_Core.Identity.Infraestructure.Interfaces;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Domain.Services.Interfaces.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.Nomenclador;
using System.Collections.Generic;
using System.Linq;

namespace FBSConsolaCB_WebApi.Domain.Services.EstructuraEmpresarial
{
    public class CorresponsalService : ICorresponsalService
    {
        private readonly ICorresponsalRepository _repository;
        private readonly IAreaTrabajoRepository _areaTrabajoRepository;
        private readonly IOficinaRepository _oficinaRepository;
        private readonly ICargoRepository _cargoRepository;
        private readonly ICatalogoRepository _catalogoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CorresponsalService(ICorresponsalRepository repository, ICatalogoRepository catalogoRepository, IUserRepository userRepository,
            IAreaTrabajoRepository areaTrabajoRepository, IOficinaRepository oficinaRepository, ICargoRepository cargoRepository, IMapper mapper)
        {
            _repository = repository;
            _catalogoRepository = catalogoRepository;
            _userRepository = userRepository;
            _areaTrabajoRepository = areaTrabajoRepository;
            _oficinaRepository = oficinaRepository;
            _cargoRepository = cargoRepository;
            _mapper = mapper;
        }

        public FuenteDatosModel<CorresponsalModel> List(PaginacionModel filtro)
        {
            IEnumerable<Corresponsal> _model = _repository.GetAllWithAssociations();
            var _fuente = _mapper.Map<FuenteDatosModel<CorresponsalModel>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Corresponsal>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<CorresponsalModel>>(_model);
            return _fuente;
        }

        public object Get(int Id)
        {
            var _model = _repository.GetWithAssociations(Id);
            if (_model != null)
            {
                return _mapper.Map<CorresponsalModel>(_model);
            }
            return null;
        }

        public CorresponsalModel Create(CorresponsalModel model)
        {
            var _model = _mapper.Map<Corresponsal>(model);
            var _segundoNombre = _model.SegundoNombre != null && _model.SegundoNombre != "" ? " " + _model.SegundoNombre : "";
            var _segundoApellido = _model.SegundoApellido != null && _model.SegundoApellido != "" ? " " + _model.SegundoApellido : "";
            _model.NombreUnido = _model.PrimerNombre + _segundoNombre + " " + _model.PrimerApellido + _segundoApellido;
            _model.Cargo = _cargoRepository.Get(model.Cargo.Id);
            _model.Usuario = _userRepository.Get(model.Usuario.Id);
            _model.Oficina = _oficinaRepository.Get(model.Oficina.Id);
            _model.TipoIdentificacion = _catalogoRepository.Get(model.TipoIdentificacion.Id);
            _model.AreaTrabajo = _areaTrabajoRepository.Get(model.AreaTrabajo.Id);

            _repository.Add(_model);

            return _mapper.Map<CorresponsalModel>(_model);
        }
        public CorresponsalModel Update(CorresponsalModel model)
        {
            var _model = _repository.Get(model.Id);
            _mapper.Map(model, _model);
            var _segundoNombre = _model.SegundoNombre != null && _model.SegundoNombre != "" ? " " + _model.SegundoNombre : "";
            var _segundoApellido = _model.SegundoApellido != null && _model.SegundoApellido != "" ? " " + _model.SegundoApellido : "";
            _model.NombreUnido = _model.PrimerNombre + _segundoNombre + " " + _model.PrimerApellido + _segundoApellido;
            _model.Cargo = _cargoRepository.Get(model.Cargo.Id);
            _model.Usuario = _userRepository.Get(model.Usuario.Id);
            _model.Oficina = _oficinaRepository.Get(model.Oficina.Id);
            _model.TipoIdentificacion = _catalogoRepository.Get(model.TipoIdentificacion.Id);
            _model.AreaTrabajo = _areaTrabajoRepository.Get(model.AreaTrabajo.Id);

            _repository.Update(_model);
            return _mapper.Map<CorresponsalModel>(_model);
        }

        public CorresponsalModel Delete(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
            {
                _model.EstaActivo = false;
                return _mapper.Map<CorresponsalModel>(_model);
            }
            return null;
        }

    }
}
