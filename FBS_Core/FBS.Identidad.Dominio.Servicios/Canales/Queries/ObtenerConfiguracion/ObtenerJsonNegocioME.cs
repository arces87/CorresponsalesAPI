using MediatR;

namespace FBS.Identidad.Dominio.Servicios.Canales.Queries
{
    public class ObtenerJsonNegocioME : IRequest<JsonNegocioMS>
    {
        public string IdUsuario { get; set; }
    }
}