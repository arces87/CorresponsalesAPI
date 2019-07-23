using System;

namespace FBSConsolaCBWebApi.Dominio.Servicios.Logs.Queries
{
    public class ObtenerModeloLog
    {
        public int Id { get; set; }
        public string Transaccion { get; set; }

        public DateTime Fecha { get; set; }

        public string Canal { get; set; }

        public string Json { get; set; }

        public int IdOperacion { get; set; }

        public string NombreOperacion { get; set; }

        public int IdCorresponsal { get; set; }

        public string NombreCorresponsal { get; set; }
    }
}
