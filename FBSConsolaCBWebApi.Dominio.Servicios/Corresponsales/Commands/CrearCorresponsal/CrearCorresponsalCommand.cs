using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Commands
{
    public class CrearCorresponsalCommand : IRequest<int>
    {
        public float Latitud { get; set; }
        public float Longitud { get; set; }
        public int IdSupervisor { get; set; }
        public CrearPersonaCorresponsal Persona { get; set; }
    }
}
