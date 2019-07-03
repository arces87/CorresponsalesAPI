using FBSConsolaCB_WebApi.Dominio.Modelos.EstructuraEmpresarial;
using FBSConsolaCB_WebApi.Dominio.Modelos.Nomenclador;


namespace FBSConsolaCB_WebApi.Dominio.Modelos.Consola
{
    public class ModeloDispositivo
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Imei { get; set; }

        public string Mac { get; set; }

        public string NumeroSerie { get; set; }

        public ModeloCatalogo TipoDispositivo { get; set; }

        public ModeloCorresponsal Corresponsal { get; set; }

        public bool EstaActivo { get; set; }
    }
}
