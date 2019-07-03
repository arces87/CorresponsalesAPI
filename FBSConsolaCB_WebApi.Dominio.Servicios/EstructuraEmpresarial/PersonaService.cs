using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.EstructuraEmpresarial
{
    public class PersonaService : IPersonaService
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;

        public PersonaService(IRepositorioPersona repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloFuenteDatos<ModeloPersona>> List(ModeloPaginacion filtro)
        {
            IEnumerable<Persona> _model = await _repositorio.GetAllWithAssociations();
            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloPersona>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Persona>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloPersona>>(_model);
            return _fuente;
        }

        public async Task<object> Get(int Id)
        {
            var _model = await _repositorio.GetWithAssociations(Id);
            if (_model != null)
            {
                if (_model is Corresponsal)
                    return _mapper.Map<ModeloCorresponsal>(_model);
                else
                    return _mapper.Map<ModeloSupervisor>(_model);
            }
            return null;
        }

        public async Task<ModeloCorresponsal> Create(ModeloCorresponsal model)
        {
            var _model = _mapper.Map<Corresponsal>(model);
            var _segundoNombre = _model.Persona.SegundoNombre != null && _model.Persona.SegundoNombre != "" ? " " + _model.Persona.SegundoNombre : "";
            var _segundoApellido = _model.Persona.SegundoApellido != null && _model.Persona.SegundoApellido != "" ? " " + _model.Persona.SegundoApellido : "";
            _model.Persona.NombreUnido = _model.Persona.PrimerNombre + _segundoNombre + " " + _model.Persona.PrimerApellido + _segundoApellido;
            await _repositorio.Add(_model);

            return _mapper.Map<ModeloCorresponsal>(_model);
        }
        public async Task<ModeloSupervisor> Create(ModeloSupervisor model)
        {
            var _model = _mapper.Map<Supervisor>(model);
            var _segundoNombre = _model.Persona.SegundoNombre != null && _model.Persona.SegundoNombre != "" ? " " + _model.Persona.SegundoNombre : "";
            var _segundoApellido = _model.Persona.SegundoApellido != null && _model.Persona.SegundoApellido != "" ? " " + _model.Persona.SegundoApellido : "";
            _model.Persona.NombreUnido = _model.Persona.PrimerNombre + _segundoNombre + " " + _model.Persona.PrimerApellido + _segundoApellido;
            await _repositorio.Add(_model);

            return _mapper.Map<ModeloSupervisor>(_model);
        }
        public async Task<ModeloCorresponsal> Update(ModeloCorresponsal model)
        {
            var _model = await _repositorio.GetWithAssociations(model.Id) as Corresponsal;
            _mapper.Map(model, _model);
            var _segundoNombre = _model.Persona.SegundoNombre != null && _model.Persona.SegundoNombre != "" ? " " + _model.Persona.SegundoNombre : "";
            var _segundoApellido = _model.Persona.SegundoApellido != null && _model.Persona.SegundoApellido != "" ? " " + _model.Persona.SegundoApellido : "";
            _model.Persona.NombreUnido = _model.Persona.PrimerNombre + _segundoNombre + " " + _model.Persona.PrimerApellido + _segundoApellido;

            await _repositorio.Update(_model);
            return _mapper.Map<ModeloCorresponsal>(_model);
        }

        public async Task<ModeloSupervisor> Update(ModeloSupervisor model)
        {
            var _model = await _repositorio.GetWithAssociations(model.Id) as Supervisor;
            _mapper.Map(model, _model);
            var _segundoNombre = _model.Persona.SegundoNombre != null && _model.Persona.SegundoNombre != "" ? " " + _model.Persona.SegundoNombre : "";
            var _segundoApellido = _model.Persona.SegundoApellido != null && _model.Persona.SegundoApellido != "" ? " " + _model.Persona.SegundoApellido : "";
            _model.Persona.NombreUnido = _model.Persona.PrimerNombre + _segundoNombre + " " + _model.Persona.PrimerApellido + _segundoApellido;

            await _repositorio.Update(_model);
            return _mapper.Map<ModeloSupervisor>(_model);
        }

        public async Task<ModeloPersona> Delete(int Id)
        {
            var _model = await _repositorio.Get(Id);
            if (_model != null)
            {
                _model.EstaActivo = false;
                return _mapper.Map<ModeloPersona>(_model);
            }
            return null;
        }

    }
}
