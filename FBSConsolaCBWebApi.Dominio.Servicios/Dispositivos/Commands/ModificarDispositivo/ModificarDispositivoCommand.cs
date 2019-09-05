using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands
{
    public class ModificarDispositivoCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public string Imei { get; set; }

        public string Mac { get; set; }

        public string NumeroSerie { get; set; }

        public string IdTipoDispositivo { get; set; }
    }
}
