using MediatR;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Commands
{
    public class CrearClienteME : IRequest<bool>
    {
        public int? SecuencialTipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public bool? EsMasculino { get; set; }
        public System.DateTime? FechaNacimiento { get; set; }
        public string TelefonoDomicilio { get; set; }
        public string TelefonoCelular { get; set; }
        public string DireccionDomiciliaria { get; set; }
        public string ReferenciaDomiciliaria { get; set; }
        public string Mail { get; set; }
    }
}
