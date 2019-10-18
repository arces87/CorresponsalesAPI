using MediatR;
using ServiciosFinancial.Models;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ListaCuentaME : IRequest<ConsolidadoCuentasMSL>
    {
        public string Identificacion { get; set; }
    }
}
