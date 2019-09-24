using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Agentes.Queries
{
    public class ModeloListaActivarAgente
    {
        public string Id { get; set; }
        public string NombreAgente { get; set; }
        public string Ubicacion { get; set; }
        public string Identificacion { get; set; }
        public string IdEstado { get; set; }
        public string NombreEstado { get; set; }

        public string IdUsuario { get; set; }
        public string NombreUsuario { get; set; }

        public string IdSupervisor { get; set; }
        public string NombreSupervisor { get; set; }

        public string IdDispositivo { get; set; }
        public string NombreDispositivo { get; set; }

        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime FechaBaja { get; set; }
    }
}
