using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBSMovilCBWebApi.Dominio.Servicios.Canal
{
    public class ObtenerRequisitoCanalME: IRequest<RequisitosCanalMS>
    {
        public string CanalId { get; set; }
    }
}
