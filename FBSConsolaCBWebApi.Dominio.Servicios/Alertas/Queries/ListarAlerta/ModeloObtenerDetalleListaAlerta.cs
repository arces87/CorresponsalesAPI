using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ModeloObtenerDetalleListaAlerta
    {
        public int Id { get; set; }

        public int IdConversacion { get; set; }

        public string Destinatario { get; set; }

        public string Remitente { get; set; }
        public int Tipo { get; set; }

        public string Mensaje { get; set; }

        public DateTime Fecha { get; set; }

        public int Estado { get; set; }
    }
}
