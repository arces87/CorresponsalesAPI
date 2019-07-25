using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesTransaccionales.Commands
{
    public class ModificarLimiteTransaccionalCommand : IRequest<int>
    {
        public int Id { get; set; }
        public double Monto { get; set; }
        public int Dias { get; set; }
        public int IdCorresponsal { get; set; }
        public int IdOperacion { get; set; }
    }
}
