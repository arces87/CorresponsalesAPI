using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Supervisores.Commands
{
    public class ModificarSupervisorCommand : IRequest<int>
    {
        public int Id { get; set; }
        public ModificarPersonaSupervisor Persona { get; set; }
    }
}
