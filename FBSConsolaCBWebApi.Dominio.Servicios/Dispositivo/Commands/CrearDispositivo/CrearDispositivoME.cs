using MediatR;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Commands
{
    public class CrearDispositivoME : IRequest<string>
    {
        public string MacAddress { get; set; }
        public string Modelo { get; set; }
        public string NumeroSerie { get; set; }
        public bool TieneImpresora { get; set; }
        public string DireccionImpresora { get; set; }
        public string Observaciones { get; set; }
        public string Ubicacion { get; set; }
        public string Imei { get; set; }
        public string IdMarca { get; set; }
        public string IdSistemaOperativo { get; set; }
    }
}
