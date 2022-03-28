using AutoMapper;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Menus.Queries
{
    public class ListaMenuUsuarioHandler : IRequestHandler<ListaMenuUsuarioME, ListaMenuUsuarioMS>
    {
        private readonly IRepositorioMenu _repositorio;
        private readonly IMapper _mapper;

        public ListaMenuUsuarioHandler(IRepositorioMenu repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ListaMenuUsuarioMS> Handle(ListaMenuUsuarioME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetMenuUsuario(request.IdUsuario);
            return new ListaMenuUsuarioMS() { Menus = _mapper.Map<List<ModeloListaMenuUsuario>>(_model) };
        }
    }
}
