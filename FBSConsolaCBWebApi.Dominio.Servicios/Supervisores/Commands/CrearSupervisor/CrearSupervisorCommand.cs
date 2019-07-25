using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Commands
{
    public class CrearSupervisorCommand : IRequest<int>
    {
        public CrearPersonaSupervisor Persona { get; set; }
    }
}
