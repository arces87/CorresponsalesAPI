namespace FBSConsolaCB_WebApi.Domain.Models.EstructuraEmpresarial
{
    public class OficinaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ciudad { get; set; }
        public bool EstaActivo { get; set; }
        public EmpresaModel Empresa { get; set; }
        
    }
}
