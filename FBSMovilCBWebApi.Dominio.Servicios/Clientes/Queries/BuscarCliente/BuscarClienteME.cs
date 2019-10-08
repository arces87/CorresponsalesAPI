using MediatR;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries
{
    public class BuscarClienteME : IRequest<InformacionPersonaMS>
    {
        public string CodigoInstitucion { get; set; }
        public string Identificacion { get; set; }
        public string TipoIdentificacion { get; set; }
    }
}
