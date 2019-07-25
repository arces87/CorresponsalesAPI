using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.LimitesExistencias.Commands
{
    public class CrearLimiteExistenciaCommand : IRequest<int>
    {
        public double Limite { get; set; }

        public int IdCorresponsal { get; set; }
    }
}
