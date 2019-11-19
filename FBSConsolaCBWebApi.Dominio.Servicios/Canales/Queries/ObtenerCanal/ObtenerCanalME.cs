using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Canales.Queries
{
    public class ObtenerCanalME : IRequest<ObtenerCanalMS>
    {
        public string Id { get; set; }
    }
}