using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries
{
    public class ObtenerListaCatalogoQueryHandler : IRequestHandler<ObtenerListaCatalogoQuery, ModeloObtenerListaCatalogo>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaCatalogoQueryHandler(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaCatalogo> Handle(ObtenerListaCatalogoQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            return new ModeloObtenerListaCatalogo() { Catalogos = _mapper.Map<List<ModeloObtenerDetalleListaCatalogo>>(_model) };
        }
    }
}
