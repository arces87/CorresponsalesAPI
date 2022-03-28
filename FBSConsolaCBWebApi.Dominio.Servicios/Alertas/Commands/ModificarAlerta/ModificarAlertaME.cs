using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Commands
{
    public class ModificarAlertaME : IRequest<string>
    {
        public string Id { get; set; }
        public string IdEstado { get; set; }
        public string Comentario { get; set; }
    }
}
