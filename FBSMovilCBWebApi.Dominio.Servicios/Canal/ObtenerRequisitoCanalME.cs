using MediatR;


namespace FBSMovilCBWebApi.Dominio.Servicios.Canal
{
    public class ObtenerRequisitoCanalME: IRequest<RequisitosCanalMS>
    {
        public string CanalId { get; set; }
    }
}
