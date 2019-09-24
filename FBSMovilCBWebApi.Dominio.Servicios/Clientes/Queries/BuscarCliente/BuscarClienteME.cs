using MediatR;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries
{
    public class BuscarClienteME : IRequest<BuscarClienteMS>
    {
        public string CodigoInstitucion { get; set; }
        public string Identificacion { get; set; }
        public string TipoIdentificacion { get; set; }
    }
}
