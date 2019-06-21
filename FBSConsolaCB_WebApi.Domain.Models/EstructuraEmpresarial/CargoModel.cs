namespace FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial
{
    public class CargoModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool EstaActivo { get; set; }
    }
}
