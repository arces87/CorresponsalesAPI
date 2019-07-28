namespace FBSConsolaCBWebApi.Dominio.Servicios.Dispositivos.Queries
{
    public class ModeloObtenerDetalleListaDispositivo
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Imei { get; set; }

        public string Mac { get; set; }

        public string NumeroSerie { get; set; }

        public string NombreTipoDispositivo { get; set; }

        public string Corresponsal { get; set; }

        public bool EstaActivo { get; set; }
    }
}
