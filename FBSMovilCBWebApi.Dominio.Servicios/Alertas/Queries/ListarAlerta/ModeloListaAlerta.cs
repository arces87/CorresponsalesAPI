using System;

namespace FBSMovilCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ModeloListaAlerta
    {
        public string Id { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string Descripcion { get; set; }
        public string IdEstado { get; set; }
        public string NombreEstado { get; set; }
        public string IdAgente { get; set; }
        public string NombreAgente { get; set; }
        public string IdTipo { get; set; }
        public string NombreTipo { get; set; }
        public string Comentario { get; set; }
    }
}
