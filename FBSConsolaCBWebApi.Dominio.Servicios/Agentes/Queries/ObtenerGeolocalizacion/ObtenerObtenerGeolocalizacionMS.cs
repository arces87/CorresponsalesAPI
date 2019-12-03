using System.Collections.Generic;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ObtenerObtenerGeolocalizacionMS
    {
        public ObtenerObtenerGeolocalizacionMS()
        {
            Imagenes = new List<string>();
        }

        public string Id { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public IEnumerable<string> Imagenes { get; set; }
    }
}
