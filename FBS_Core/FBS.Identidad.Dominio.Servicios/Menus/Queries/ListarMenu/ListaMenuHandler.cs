using AutoMapper;
using FBS.Identidad.Infraestructura.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBS.Identidad.Dominio.Servicios.Menus.Queries
{
    public class ListaMenuHandler : IRequestHandler<ListaMenuME, ListaMenuMS>
    {
        private readonly IRepositorioMenu _repositorio;
        private readonly IMapper _mapper;

        public ListaMenuHandler(IRepositorioMenu repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ListaMenuMS> Handle(ListaMenuME request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            return new ListaMenuMS() { Menus = _mapper.Map<List<ModeloListaMenu>>(_model) };
        }
    }
}
