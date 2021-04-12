using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Canales.Commands
{
    public class ModificarCanalME : IRequest<string>
    {
        public string Id { get; set; }

        public string Nombre { get; set; }

        public string JsonNegocio { get; set; }
        
        public int TiempoVidaOtp { get; set; } = 600;

    }
}
