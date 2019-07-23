using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Nomenclador;

namespace FBSConsolaCBWebApi.Dominio.Servicios.TiposCatalogos.Queries
{
    public class ObtenerListaTipoCatalogoQueryHandler : IRequestHandler<ObtenerListaTipoCatalogoQuery, ModeloObtenerListaTipoCatalogo>
    {
        private readonly IRepositorioTipoCatalogo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaTipoCatalogoQueryHandler(IRepositorioTipoCatalogo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaTipoCatalogo> Handle(ObtenerListaTipoCatalogoQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            return new ModeloObtenerListaTipoCatalogo() { TiposCatalogos = _mapper.Map<List<ModeloObtenerDetalleListaTipoCatalogo>>(_model) };
        }
    }
}
