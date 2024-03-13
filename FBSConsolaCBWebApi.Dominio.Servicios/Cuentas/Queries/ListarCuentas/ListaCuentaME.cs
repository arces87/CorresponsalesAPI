using Corresponsales.Query.Model;
using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Cuentas.Queries
{
    public class ListaCuentaME : IRequest<DevuelveConsolidadoCuentasIdentificacionResponse>
    {
        public string Identificacion { get; set; }
        public int TipoIdentificacion { get; set; }
    }
}
