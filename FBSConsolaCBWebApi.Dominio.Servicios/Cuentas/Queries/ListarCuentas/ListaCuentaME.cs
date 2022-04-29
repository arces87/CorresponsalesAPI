using MediatR;
using Org.OpenAPITools.Model;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ListaCuentaME : IRequest<ConsolidadoCuentasMSL>
    {
        public string Identificacion { get; set; }
        public int TipoIdentificacion { get; set; }
    }
}
