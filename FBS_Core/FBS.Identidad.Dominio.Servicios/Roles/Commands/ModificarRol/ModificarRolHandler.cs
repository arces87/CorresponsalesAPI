using AutoMapper;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Roles.Commands
{
    public class ModificarRolHandler : IRequestHandler<ModificarRolME, string>
    {
        private readonly IRepositorioRol _repositorio;
        private readonly IRepositorioMenu _repositorioMenu;
        private readonly IMapper _mapper;

        public ModificarRolHandler(IRepositorioRol repositorio, IRepositorioMenu repositorioMenu, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioMenu = repositorioMenu;
            _mapper = mapper;
        }

        public async Task<string> Handle(ModificarRolME request, CancellationToken cancellationToken)
        {
            var rol = await _repositorio.GetForName(request.Nombre);
            if (rol == null || rol.Id == request.Id)
            {
                rol = await _repositorio.Get(request.Id);
                rol.Name = request.Nombre;
                rol.Descripcion = request.Descripcion;
                await _repositorio.Update(rol);

                await _repositorioMenu.RemoveMenuRole(rol.Id);
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
