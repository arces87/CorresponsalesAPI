using AutoMapper;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Queries
{
    public class ObtenerUsuarioHandler : IRequestHandler<ObtenerUsuarioME, ModeloObtenerUsuario>
    {
        private readonly IRepositorioUsuario _repositorio;
        private readonly IRepositorioRol _repositorioRol;
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly IMapper _mapper;

        public ObtenerUsuarioHandler(IRepositorioUsuario repositorio, IRepositorioMenu repositorioMenu,
            UserManager<Usuario> manejadorUsuario, IRepositorioRol repositorioRol, IMapper mapper)
        {
            _repositorio = repositorio;
            _manejadorUsuario = manejadorUsuario;
            _repositorioRol = repositorioRol;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerUsuario> Handle(ObtenerUsuarioME request, CancellationToken cancellationToken)
        {
            var _user = await _repositorio.Get(request.Id);
            if (_user == null)
                return null;

            var roles = await _manejadorUsuario.GetRolesAsync(_user);
            var user = _mapper.Map<ModeloObtenerUsuario>(_user);
            var _roles = new List<ObtenerUsuarioRol>();
            if (roles != null)
            {
                foreach (var item in roles)
                {
                    var rol = await _repositorioRol.GetForName(item);
                    var modeloRol = _mapper.Map<ObtenerUsuarioRol>(rol);
                    _roles.Add(modeloRol);
                }
            }
            user.Roles = _roles;
            return user;
        }

    }
}
