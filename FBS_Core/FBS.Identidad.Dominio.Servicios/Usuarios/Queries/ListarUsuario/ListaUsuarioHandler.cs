using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBS.Identidad.DAL.Seguridad;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Usuarios.Queries
{
    public class ListaUsuarioHandler : IRequestHandler<ListaUsuarioME, ListaUsuarioMS>
    {
        private readonly UserManager<Usuario> _manejadorUsuario;
        private readonly IMapper _mapper;
        private readonly IRepositorioRol _manejadorRol;

        public ListaUsuarioHandler(UserManager<Usuario> manejadorUsuario, IMapper mapper, IRepositorioRol manejadorRol)
        {
            _manejadorUsuario = manejadorUsuario;
            _mapper = mapper;
            _manejadorRol = manejadorRol;
        }

        public async Task<ListaUsuarioMS> Handle(ListaUsuarioME request, CancellationToken cancellationToken)
        {
            var _model = (await _manejadorUsuario.Users.Where(u => request.Activo != null ? u.EstaActivo == request.Activo : true).ToListAsync()) as IEnumerable<Usuario>;
            foreach (Usuario u in _model)
            { 
                var _rol = await _manejadorRol.GetRolUsuario(u.Id);
                u.NombreCompleto = _rol.Descripcion;
            }            
            var _retorno = new ListaUsuarioMS();
            var totalElementos = 0;
            Filtro<Usuario>.ProcesarLista(ref _model, _mapper.Map<ModeloPaginacion>(request), ref totalElementos);
            _retorno.TotalElementos = totalElementos;
            _retorno.Usuarios = _mapper.Map<List<ModeloListaUsuario>>(_model);
            _retorno.CantidadElementos = request.CantidadElementos;
            _retorno.Pagina = request.Pagina;
            return _retorno;
        }
    }
}
