using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ObtenerListaAlertaQueryHandler : IRequestHandler<ObtenerListaAlertaQuery, ModeloObtenerListaAlerta>
    {
        private readonly IRepositorioAlerta _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaAlertaQueryHandler(IRepositorioAlerta repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaAlerta> Handle(ObtenerListaAlertaQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            return new ModeloObtenerListaAlerta() { Alertas = _mapper.Map<List<ModeloObtenerDetalleListaAlerta>>(_model) };
        }
    }
}
