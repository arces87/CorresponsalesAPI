using MediatR;
using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Commands
{
    public class CrearAlertaME : IRequest<bool>
    {
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Descripcion { get; set; }
        public string IdTipo { get; set; }
    }
}
