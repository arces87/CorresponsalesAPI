using FBSConsolaCBWebApi.Infraestructure.Interfaces.Canales;
using FBSConsolaCBWebApi.Infraestructure.Interfaces.Corresponsales;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ObtenerObtenerGeolocalizacionHandler : IRequestHandler<ObtenerObtenerGeolocalizacionME, ObtenerObtenerGeolocalizacionMS>
    {
        private readonly IRepositorioGeolocalizacion _repositorioGeolocalizacion;

        public ObtenerObtenerGeolocalizacionHandler(IRepositorioGeolocalizacion repositorioGeolocalizacion)
        {
            _repositorioGeolocalizacion = repositorioGeolocalizacion;
        }

        public async Task<ObtenerObtenerGeolocalizacionMS> Handle(ObtenerObtenerGeolocalizacionME request, CancellationToken cancellationToken)
        {
            var geolocalizacion = await _repositorioGeolocalizacion.GetForAgente(request.Id);
            var imagenes = await _repositorioGeolocalizacion.GetImagenesPorAgente(request.Id);
            var retorno = new ObtenerObtenerGeolocalizacionMS()
            {
                Id = geolocalizacion.Id,
                Latitud = geolocalizacion.Latitud,
                Longitud = geolocalizacion.Longitud,
                Imagenes = imagenes.Select(i => i.DireccionImagen)
            };
            return retorno;
        }
    }
}
