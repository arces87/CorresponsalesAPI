namespace FBSConsolaCBWebApi.Dominio.Servicios.Catalogos.Queries
{
    public class ObtenerModeloCatalogo
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int IdTipoCatalogo { get; set; }

        public string NombreTipoCatalogo { get; set; }
    }
}
