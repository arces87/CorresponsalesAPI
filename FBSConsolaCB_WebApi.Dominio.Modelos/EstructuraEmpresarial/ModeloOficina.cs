namespace FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial
{
    public class ModeloOficina
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ciudad { get; set; }
        public bool EstaActivo { get; set; }
        public ModeloEmpresa Empresa { get; set; }
        
    }
}
