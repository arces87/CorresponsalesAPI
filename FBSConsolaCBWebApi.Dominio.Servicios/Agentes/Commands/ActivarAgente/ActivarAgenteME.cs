using MediatR;
using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class ActivarAgenteME : IRequest<string>
    {
        public string IdAgente { get; set; }
        public IEnumerable<ActivarAgenteImagen> Imagenes { get; set; }
    }
}
