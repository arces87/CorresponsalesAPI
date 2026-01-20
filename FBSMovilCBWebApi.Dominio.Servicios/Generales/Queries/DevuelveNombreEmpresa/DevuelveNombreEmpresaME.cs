using MediatR;
using Corresponsales.Query.Model;

namespace FBSMovilCBWebApi.Dominio.Servicios.Generales.Queries
{
    public class DevuelveNombreEmpresaME : IRequest<DevuelveNombreEmpresaResponse>
    {
        public int Secuencial { get; set; }
    }
}

