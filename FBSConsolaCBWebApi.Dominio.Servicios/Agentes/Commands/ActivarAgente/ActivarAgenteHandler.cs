using FBSConsolaCBWebApi.DAL.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class ActivarAgenteHandler : IRequestHandler<ActivarAgenteME, string>
    {
        private readonly IRepositorioAgente _repositorio;
        private readonly IRepositorioGeolocalizacion _repositorioGeolocalizacion;
        private readonly IRepositorioImagenGeolocalizacion _repositorioImagenGeolocalizacion;

        public ActivarAgenteHandler(IRepositorioAgente repositorio, IRepositorioGeolocalizacion repositorioGeolocalizacion, IRepositorioImagenGeolocalizacion repositorioImagenGeolocalizacion)
        {
            _repositorio = repositorio;
            _repositorioGeolocalizacion = repositorioGeolocalizacion;
            _repositorioImagenGeolocalizacion = repositorioImagenGeolocalizacion;
        }

        public async Task<string> Handle(ActivarAgenteME request, CancellationToken cancellationToken)
        {
            await _repositorio.Activar(request.IdAgente);
            var geolocalizacion = await _repositorioGeolocalizacion.GetForAgente(request.IdAgente);
            if (geolocalizacion != null)
            {
                foreach (var item in request.Imagenes)
                {
                    await _repositorioImagenGeolocalizacion.Add(new ImagenGeolocalizacion() { DireccionImagen = item.DireccionImagen, Geolocalizacion = geolocalizacion, EstaActivo = true });
                }

            }
            return request.IdAgente;
        }
    }
}
