using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Commands
{
    public class ModificarAgenteME : IRequest<string>
    {
        public string Id { get; set; }
        public string NombreAgente { get; set; }

        public string JsonAgente { get; set; }
        public string Identificacion { get; set; }
        public string Ubicacion { get; set; }
        public string IdEstado { get; set; }

        public string IdUsuario { get; set; }

        public string IdSupervisor { get; set; }

        public string IdDispositivo { get; set; }

        public string TipoCuenta { get; set; }
        public string NumeroCuenta { get; set; }
        public double SaldoCuenta { get; set; }
    }
}
