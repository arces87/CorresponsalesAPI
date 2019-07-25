using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Corresponsales.Queries
{
    public class ObtenerCorresponsalQuery : IRequest<ObtenerModeloCorresponsal>
    {
        public int Id { get; set; }
    }
}