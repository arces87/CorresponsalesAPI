using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Commands
{
    public class EliminarLimiteExistenciaCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
