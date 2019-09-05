using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Logs.Queries
{
    public class ObtenerListaLogQueryHandler : IRequestHandler<ObtenerListaLogQuery, ModeloObtenerListaLog>
    {
        private readonly IRepositorioLog _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaLogQueryHandler(IRepositorioLog repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaLog> Handle(ObtenerListaLogQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllWithAssociations();
            return new ModeloObtenerListaLog() { Logs = _mapper.Map<List<ModeloObtenerDetalleListaLog>>(_model) };
        }
    }
}
