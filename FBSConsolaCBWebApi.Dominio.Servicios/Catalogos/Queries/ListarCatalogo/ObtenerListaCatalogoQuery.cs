using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries
{
    public class ObtenerListaCatalogoQuery : IRequest<ModeloObtenerListaCatalogo>
    {
        public int idTipoCatalogo { get; set; }
    }
}