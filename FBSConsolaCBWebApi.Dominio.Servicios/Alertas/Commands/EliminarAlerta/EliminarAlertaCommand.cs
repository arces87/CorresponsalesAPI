using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Commands
{
    public class EliminarAlertaCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
