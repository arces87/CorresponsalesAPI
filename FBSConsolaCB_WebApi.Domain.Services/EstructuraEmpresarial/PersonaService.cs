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
    public class PersonaService : IPersonaService
    {
        private readonly IPersonaRepository _repository;
        private readonly IMapper _mapper;

        public PersonaService(IPersonaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public FuenteDatosModel<PersonaModel> List(PaginacionModel filtro)
        {
            IEnumerable<Persona> _model = _repository.GetAllWithAssociations();
            var _fuente = _mapper.Map<FuenteDatosModel<PersonaModel>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Persona>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<PersonaModel>>(_model);
            return _fuente;
        }

        public object Get(int Id)
        {
            var _model = _repository.GetWithAssociations(Id);
            if (_model != null)
            {
                if (_model is Corresponsal)
                    return _mapper.Map<CorresponsalModel>(_model);
                else
                    return _mapper.Map<SupervisorModel>(_model);
            }
            return null;
        }

        public CorresponsalModel Create(CorresponsalModel model)
        {
            var _model = _mapper.Map<Corresponsal>(model);
            var _segundoNombre = _model.Persona.SegundoNombre != null && _model.Persona.SegundoNombre != "" ? " " + _model.Persona.SegundoNombre : "";
            var _segundoApellido = _model.Persona.SegundoApellido != null && _model.Persona.SegundoApellido != "" ? " " + _model.Persona.SegundoApellido : "";
            _model.Persona.NombreUnido = _model.Persona.PrimerNombre + _segundoNombre + " " + _model.Persona.PrimerApellido + _segundoApellido;
            _repository.Add(_model);

            return _mapper.Map<CorresponsalModel>(_model);
        }
        public SupervisorModel Create(SupervisorModel model)
        {
            var _model = _mapper.Map<Supervisor>(model);
            var _segundoNombre = _model.Persona.SegundoNombre != null && _model.Persona.SegundoNombre != "" ? " " + _model.Persona.SegundoNombre : "";
            var _segundoApellido = _model.Persona.SegundoApellido != null && _model.Persona.SegundoApellido != "" ? " " + _model.Persona.SegundoApellido : "";
            _model.Persona.NombreUnido = _model.Persona.PrimerNombre + _segundoNombre + " " + _model.Persona.PrimerApellido + _segundoApellido;
            _repository.Add(_model);

            return _mapper.Map<SupervisorModel>(_model);
        }
        public CorresponsalModel Update(CorresponsalModel model)
        {
            var _model = _repository.GetWithAssociations(model.Id) as Corresponsal;
            _mapper.Map(model, _model);
            var _segundoNombre = _model.Persona.SegundoNombre != null && _model.Persona.SegundoNombre != "" ? " " + _model.Persona.SegundoNombre : "";
            var _segundoApellido = _model.Persona.SegundoApellido != null && _model.Persona.SegundoApellido != "" ? " " + _model.Persona.SegundoApellido : "";
            _model.Persona.NombreUnido = _model.Persona.PrimerNombre + _segundoNombre + " " + _model.Persona.PrimerApellido + _segundoApellido;

            _repository.Update(_model);
            return _mapper.Map<CorresponsalModel>(_model);
        }

        public SupervisorModel Update(SupervisorModel model)
        {
            var _model = _repository.GetWithAssociations(model.Id) as Supervisor;
            _mapper.Map(model, _model);
            var _segundoNombre = _model.Persona.SegundoNombre != null && _model.Persona.SegundoNombre != "" ? " " + _model.Persona.SegundoNombre : "";
            var _segundoApellido = _model.Persona.SegundoApellido != null && _model.Persona.SegundoApellido != "" ? " " + _model.Persona.SegundoApellido : "";
            _model.Persona.NombreUnido = _model.Persona.PrimerNombre + _segundoNombre + " " + _model.Persona.PrimerApellido + _segundoApellido;

            _repository.Update(_model);
            return _mapper.Map<SupervisorModel>(_model);
        }

        public PersonaModel Delete(int Id)
        {
            var _model = _repository.Get(Id);
            if (_model != null)
            {
                _model.EstaActivo = false;
                return _mapper.Map<PersonaModel>(_model);
            }
            return null;
        }

    }
}
