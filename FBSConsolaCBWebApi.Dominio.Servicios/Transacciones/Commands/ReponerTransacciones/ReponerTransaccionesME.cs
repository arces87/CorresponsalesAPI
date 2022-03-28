using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Transacciones.Commands
{
    public class ReponerTransaccionesME : IRequest<bool>
    {
        public string IdAgente { get; set; }
    }
}
