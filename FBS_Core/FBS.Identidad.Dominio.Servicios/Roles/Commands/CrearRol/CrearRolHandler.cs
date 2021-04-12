using AutoMapper;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Roles.Commands
{
    public class CrearRolHandler : IRequestHandler<CrearRolME, string>
    {
        private readonly IRepositorioRol _repositorio;
        private readonly IRepositorioMenu _repositorioMenu;
        private readonly RoleManager<Rol> _manejadorRol;
        private readonly IMapper _mapper;

        public CrearRolHandler(IRepositorioRol repositorio,
            IRepositorioMenu repositorioMenu,
            RoleManager<Rol> manejadorRol, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioMenu = repositorioMenu;
            _manejadorRol = manejadorRol;
            _mapper = mapper;
        }

        public async Task<string> Handle(CrearRolME request, CancellationToken cancellationToken)
        {
            var rol = await _repositorio.GetForName(request.Nombre);
            if (rol == null)
            {
                rol = _mapper.Map<Rol>(request);
                rol.EstaActivo = true;
                await _manejadorRol.CreateAsync(rol);
                if (request.Menus != null)
                {
                    foreach (var item in request.Menus)
                    {
                        var _menuRole = new RolMenu() { Rol = rol, Menu = await _repositorioMenu.Get(item.IdMenu) };
                        await _repositorio.AddMenu(_menuRole);
                    }
                }
                return rol.Id;
            }
            return "";
        }
    }
}
