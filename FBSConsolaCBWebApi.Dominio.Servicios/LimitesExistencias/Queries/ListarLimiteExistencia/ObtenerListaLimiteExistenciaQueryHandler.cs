using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Queries
{
    public class ObtenerListaLimiteExistenciaQueryHandler : IRequestHandler<ObtenerListaLimiteExistenciaQuery, ModeloObtenerLimiteExistencia>
    {
        private readonly IRepositorioLimiteExistencia _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaLimiteExistenciaQueryHandler(IRepositorioLimiteExistencia repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerLimiteExistencia> Handle(ObtenerListaLimiteExistenciaQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            return new ModeloObtenerLimiteExistencia() { Limites = _mapper.Map<List<ModeloObtenerDetalleListaLimiteExistencia>>(_model) };
        }
    }
}
