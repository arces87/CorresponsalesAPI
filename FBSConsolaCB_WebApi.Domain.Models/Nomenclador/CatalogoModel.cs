namespace FBSConsolaCB_WebApi.Domain.Models.Nomenclador
{
    public class CatalogoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EstaActivo { get; set; }

        public TipoCatalogoModel TipoCatalogo { get; set; }
    }
}
