using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Consola;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class ObtenerListaDispositivoQueryHandler : IRequestHandler<ObtenerListaDispositivoQuery, ModeloObtenerListaDispositivo>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IMapper _mapper;

        public ObtenerListaDispositivoQueryHandler(IRepositorioDispositivo repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<ModeloObtenerListaDispositivo> Handle(ObtenerListaDispositivoQuery request, CancellationToken cancellationToken)
        {
            var _model = await _repositorio.GetAllActive();
            return new ModeloObtenerListaDispositivo() { Dispositivos = _mapper.Map<List<ModeloObtenerDetalleListaDispositivo>>(_model) };
        }
    }
}
