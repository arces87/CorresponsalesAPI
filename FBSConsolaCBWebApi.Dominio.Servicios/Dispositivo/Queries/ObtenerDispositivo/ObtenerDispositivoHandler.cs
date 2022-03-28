using AutoMapper;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class ObtenerDispositivoHandler : IRequestHandler<ObtenerDispositivoME, ObtenerDispositivoMS>
    {
        private readonly IRepositorioDispositivo _repositorio;
        private readonly IRepositorioImagen _repositorioImagen;
        private readonly IMapper _mapper;

        public ObtenerDispositivoHandler(IRepositorioDispositivo repositorio, IRepositorioImagen repositorioImagen, IMapper mapper)
        {
            _repositorio = repositorio;
            _repositorioImagen = repositorioImagen;
            _mapper = mapper;
        }

        public async Task<ObtenerDispositivoMS> Handle(ObtenerDispositivoME request, CancellationToken cancellationToken)
        {
            var dispositivo = _mapper.Map<ObtenerDispositivoMS>(await _repositorio.GetWithAssociations(request.Id));
            dispositivo.Imagenes = _mapper.Map<IEnumerable<ObtenerDispositivoImagen>>(await _repositorioImagen.GetForDispositivo(dispositivo.Id));
            return dispositivo;
        }
    }
}
