using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Empresas.Queries
{
    public class ObtenerEmpresaQuery : IRequest<ObtenerModeloEmpresa>
    {
        public int Id { get; set; }
    }
}