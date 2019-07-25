using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Queries
{
    public class ObtenerSupervisorQuery : IRequest<ObtenerModeloSupervisor>
    {
        public int Id { get; set; }
    }
}