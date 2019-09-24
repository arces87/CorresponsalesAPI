using MediatR;
using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands
{
    public class CrearLogME : IRequest<string>
    {
        public string IdUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Criptografia { get; set; }
        public string JsonDispositivo { get; set; }
        public string JsonLog { get; set; }
        public string IdTipoAccion { get; set; }
        public string RelacionadoId { get; set; }
    }
}
