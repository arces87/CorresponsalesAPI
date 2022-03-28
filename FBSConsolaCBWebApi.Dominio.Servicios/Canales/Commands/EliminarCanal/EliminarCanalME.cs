using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Canales.Commands
{
    public class EliminarCanalME : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
