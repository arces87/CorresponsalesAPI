using AutoMapper;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Dominio.Servicios.Usuarios.Commands;
using FBS.Identidad.Infraestructura.Interfaces;
using FBSConsolaCBWebApi.DAL.EstructuraEmpresarial;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.EstructuraEmpresarial;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Commands
{
    public class CrearCorresponsalCommandHandle : IRequestHandler<CrearSupervisorCommand, int>
    {
        private readonly IRepositorioPersona _repositorio;
        private readonly IMediator _mediador;
        private readonly IMapper _mapper;

        public CrearCorresponsalCommandHandle(IRepositorioPersona repositorio, IMediator mediador, IMapper mapper)
        {
            _repositorio = repositorio;
            _mediador = mediador;
            _mapper = mapper;
        }

        public async Task<int> Handle(CrearSupervisorCommand request, CancellationToken cancellationToken)
        {
            var _model = _mapper.Map<Supervisor>(request);
            var idUsuario = await _mediador.Send(_mapper.Map<CrearUsuarioCommand>(request.Persona.Usuario));
            if (idUsuario.Contains("Error:"))
                throw new Exception("Error al registrar el usuario:" + idUsuario);
            _model.Persona.Usuario = new Usuario() { Id = idUsuario };
            var _segundoNombre = _model.Persona.SegundoNombre != null && _model.Persona.SegundoNombre != "" ? " " + _model.Persona.SegundoNombre : "";
            var _segundoApellido = _model.Persona.SegundoApellido != null && _model.Persona.SegundoApellido != "" ? " " + _model.Persona.SegundoApellido : "";
            _model.Persona.NombreUnido = _model.Persona.PrimerNombre + _segundoNombre + " " + _model.Persona.PrimerApellido + _segundoApellido;
            var identificador = await _repositorio.Add(_model);
            return int.Parse(identificador);
        }
    }
}
