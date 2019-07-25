using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Commands
{
    public class ModificarLimiteExistenciaCommand : IRequest<int>
    {
        public int Id { get; set; }
        public double Limite { get; set; }
        public int IdCorresponsal { get; set; }
    }
}
