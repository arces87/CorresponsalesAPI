using AutoMapper;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Roles.Commands
{
    public class EliminarRolHandler : IRequestHandler<EliminarRolME, bool>
    {
        private readonly IRepositorioRol _repositorio;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IRepositorioMenu _repositorioMenu;
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly IMapper _mapper;

        public EliminarRolHandler(IRepositorioRol repositorio, IRepositorioUsuario repositorioUsuario,
            IRepositorioMenu repositorioMenu, UserManager<Usuario> manejadorUsuario, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioUsuario = repositorioUsuario;
            _repositorioMenu = repositorioMenu;
            _manejadorUsuario = manejadorUsuario;
            _mapper = mapper;
        }

        public async Task<bool> Handle(EliminarRolME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.Get(request.Id);
            await _repositorio.Remove(_model);
            var _usuarios = await _repositorioUsuario.GetAll();
            foreach (var item in _usuarios)
            {
                await _manejadorUsuario.RemoveFromRoleAsync(item, _model.Id);
            }
            await _repositorioMenu.RemoveMenuRole(_model.Id);
            return true;
        }
    }
}
