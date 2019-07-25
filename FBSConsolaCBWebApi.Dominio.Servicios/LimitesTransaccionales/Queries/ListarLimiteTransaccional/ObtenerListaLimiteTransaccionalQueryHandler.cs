using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Queries
{
    public class ObtenerListaLimiteTransaccionalQueryHandler : IRequestHandler<ObtenerListaLimiteTransaccionalQuery, ModeloObtenerLimiteTransaccional>
    {
        private readonly IRepositorioLimiteTransaccional _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaLimiteTransaccionalQueryHandler(IRepositorioLimiteTransaccional repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerLimiteTransaccional> Handle(ObtenerListaLimiteTransaccionalQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            return new ModeloObtenerLimiteTransaccional() { Limites = _mapper.Map<List<ModeloObtenerDetalleListaLimiteTransaccional>>(_model) };
        }
    }
}
