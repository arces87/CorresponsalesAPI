using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Alertas.Queries
{
    public class ObtenerModeloAlerta
    {
        public int Id { get; set; }

        public int IdConversacion { get; set; }

        public string NombreDestinatario { get; set; }
        public string IdDestinatario { get; set; }

        public string NombreRemitente { get; set; }
        public string IdRemitente { get; set; }

        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public int Tipo { get; set; }
        public string Mensaje { get; set; }

        public DateTime Fecha { get; set; }

        public int Estado { get; set; }
    }
}
