using FBSMovilCBWebApi.Dominio.Servicios.Usuarios.Commands.VerificarAgente;
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

        public string Usuario { get; set; }
        public string Imei { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string Mac { get; set; }
    }
}
