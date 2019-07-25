using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Personas.Commands
{
    public class EliminarPersonaCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
