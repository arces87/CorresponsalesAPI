using MediatR;
using ServiciosFinancial.Models;

namespace FBSMovilCBWebApi.Dominio.Servicios.Clientes.Queries
{
    public class BuscarClienteME : PorIdentificacionSocioME, IRequest<InformacionPersonaMS>
    {
      
    }
}
