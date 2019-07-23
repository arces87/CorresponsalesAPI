using MediatR;
using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.Logs.Commands
{
    public class CrearLogCommand : IRequest<int>
    {
        public string Transaccion { get; set; }

        public DateTime Fecha { get; set; }

        public string Canal { get; set; }

        public string Json { get; set; }

        public int IdOperacion { get; set; }

        public int IdCorresponsal { get; set; }
    }
}
