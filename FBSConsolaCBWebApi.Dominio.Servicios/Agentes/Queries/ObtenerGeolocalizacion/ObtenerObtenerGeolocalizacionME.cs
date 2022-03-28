using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ObtenerObtenerGeolocalizacionME : IRequest<ObtenerObtenerGeolocalizacionMS>
    {
        public string Id { get; set; }
    }
}