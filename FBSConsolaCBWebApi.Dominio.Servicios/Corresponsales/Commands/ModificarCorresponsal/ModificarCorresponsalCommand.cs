using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Commands
{
    public class ModificarCorresponsalCommand : IRequest<int>
    {
        public int Id { get; set; }
        public float Latitud { get; set; }
        public float Longitud { get; set; }
        public int IdSupervisor { get; set; }
        public ModificarPersonaCorresponsal Persona { get; set; }
    }
}
