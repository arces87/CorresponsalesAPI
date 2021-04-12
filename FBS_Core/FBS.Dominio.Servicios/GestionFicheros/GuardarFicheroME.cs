using MediatR;
using Microsoft.AspNetCore.Http;

namespace FBS.Dominio.Servicios.GestionFicheros
{
    public class GuardarFicheroME : IRequest<string>
    {
        public IFormFile Fichero { get; set; }
        public string DireccionGuardar { get; set; }
    }
}
