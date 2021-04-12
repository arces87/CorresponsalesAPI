using AutoMapper;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Roles.Queries
{
    public class ObtenerRolHandler : IRequestHandler<ObtenerRolME, ModeloObtenerRol>
    {
        private readonly IRepositorioRol _repositorio;
        private readonly IRepositorioMenu _repositorioMenu;
        private readonly IMapper _mapper;

        public ObtenerRolHandler(IRepositorioRol repositorio, IRepositorioMenu repositorioMenu, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioMenu = repositorioMenu;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerRol> Handle(ObtenerRolME request, CancellationToken cancellationToken)
        {
            var rol = _mapper.Map<ModeloObtenerRol>(await _repositorio.Get(request.Id));
            var menus = await _repositorioMenu.GetMenuRole(rol.Id);
            if (menus != null)
                rol.Menus = _mapper.Map<IEnumerable<ObtenerRolMenu>>(menus);
            return rol;
        }

    }
}
