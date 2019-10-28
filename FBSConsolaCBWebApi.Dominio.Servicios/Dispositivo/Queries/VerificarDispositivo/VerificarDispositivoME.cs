using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class VerificarDispositivoME : IRequest<bool>
    {
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string NoSerie { get; set; }
    }
}