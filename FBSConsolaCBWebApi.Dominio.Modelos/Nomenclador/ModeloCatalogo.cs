namespace FBSConsolaCBWebApi.Dominio.Modelos.Nomenclador
{
    public class ModeloCatalogo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EstaActivo { get; set; }

        public ModeloTipoCatalogo TipoCatalogo { get; set; }
    }
}
