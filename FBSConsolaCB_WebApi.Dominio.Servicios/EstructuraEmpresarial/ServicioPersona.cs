using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBS.Identidad.Infraestructura.Interfaces;
using FBSConsolaCB_WebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Modelos.Consola;
using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Servicios.Interfaces.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using Financial_Services_Banca;
using Financial_Services_Banca.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBSConsolaCB_WebApi.Dominio.Servicios.EstructuraEmpresarial
{
    public class ServicioPersona : IServicioPersona
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMapper _mapper;
        private readonly IFBSBancaApi _bancaVirtual;
        private readonly IRepositorioUsuario _repositorioUsuario;

        public ServicioPersona(IRepositorioPersona repositorio, IMapper mapper,
            IFBSBancaApi bancaVirtual,
            IRepositorioUsuario repositorioUsuario)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _bancaVirtual = bancaVirtual;
            _repositorioUsuario = repositorioUsuario;
        }

        public async Task<ModeloFuenteDatos<ModeloPersona>> List(ModeloPaginacion filtro)
        {
            IEnumerable<Persona> _model = await _repositorio.GetAllWithAssociations();
            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloPersona>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Persona>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloPersona>>(_model);
            foreach (var item in _fuente.Datos)
            {
                var persona = await _repositorio.GetWithAssociations(item.Id);
                if (persona is Supervisor)
                    item.Tipo = "Supervisor";
                else if (persona is Corresponsal)
                    item.Tipo = "Corresponsal";
                else
                    item.Tipo = "Administrador";
            }
            return _fuente;
        }

        public async Task<ModeloFuenteDatos<ModeloSupervisor>> ListaSupervisores(ModeloPaginacion filtro)
        {
            IEnumerable<Supervisor> _model = await _repositorio.GetSupervisoresWithAssociations();
            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloSupervisor>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Supervisor>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloSupervisor>>(_model);
            return _fuente;
        }

        public async Task<ModeloFuenteDatos<ModeloCorresponsal>> ListaCorresponsales(ModeloPaginacion filtro)
        {
            IEnumerable<Corresponsal> _model = await _repositorio.GetCorresponsalesWithAssociations();
            var _fuente = _mapper.Map<ModeloFuenteDatos<ModeloCorresponsal>>(filtro);
            _fuente.TotalElementos = _model.Count();
            Filtro<Corresponsal>.ProcesarLista(ref _model, filtro);

            _fuente.Datos = _mapper.Map<IEnumerable<ModeloCorresponsal>>(_model);
            return _fuente;
        }

        public async Task<object> Get(int Id)
        {
            var _model = await _repositorio.GetWithAssociations(Id);
            if (_model != null)
            {
                if (_model is Corresponsal)
                {
                    var corresponsal = _mapper.Map<ModeloCorresponsal>(_model);
                    var dispositivo = _repositorio.ObtenerDispositivo(corresponsal.Id);
                    if (dispositivo != null)
                        corresponsal.Dispositivo = _mapper.Map<ModeloDispositivo>(dispositivo);
                    return corresponsal;
                }
                else if (_model is Supervisor)
                    return _mapper.Map<ModeloSupervisor>(_model);
                else
                    return _mapper.Map<ModeloPersona>(_model);
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

        public async Task<ModeloPersona> DevuelveDatosPersonaIdentificacion(string identificacion)
        {
            var parametro = new PorIdentificacionSocioME()
            {
                Identificacion = identificacion
            };
            var _retorno = await _bancaVirtual.Clientes.DevuelveDatosPersonaIdentificacionWithHttpMessagesAsync(parametro);
            var contenido = _retorno.Body;

            if (contenido != null)
            {
                var retorno = _mapper.Map<ModeloPersona>(contenido);
                return retorno;
            }
            return null;
        }

        public async Task<ModeloCorresponsal> CambiarEstadoCorresponsal(int id)
        {
            var corresponsal = await _repositorio.GetWithAssociations(id);
            var usuario = (corresponsal as Corresponsal).Persona.Usuario;
            usuario.LockoutEnabled = !usuario.LockoutEnabled;
            await _repositorioUsuario.Update(usuario);
            return _mapper.Map<ModeloCorresponsal>(corresponsal as Corresponsal);
        }

    }
}
