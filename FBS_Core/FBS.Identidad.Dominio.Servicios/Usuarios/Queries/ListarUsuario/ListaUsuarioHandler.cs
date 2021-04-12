using AutoMapper;
using FBS.Dominio.Modelos.Filtro;
using FBS.Dominio.Servicios.Utilidades;
using FBS.Identidad.DAL.Seguridad;
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

        public ListaUsuarioHandler(UserManager<Usuario> manejadorUsuario, IMapper mapper)
        {
            _manejadorUsuario = manejadorUsuario;
            _mapper = mapper;
        }

        public async Task<ListaUsuarioMS> Handle(ListaUsuarioME request, CancellationToken cancellationToken)
        {
            var _model = (await _manejadorUsuario.Users.Where(u => request.Activo != null ? u.EstaActivo == request.Activo : true).ToListAsync()) as IEnumerable<Usuario>;
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
